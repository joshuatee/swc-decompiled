using StaRTS.Externals.BI;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.EnvironmentManager;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using WinRTBridge;

namespace StaRTS.Externals.IAP
{
	public class InAppPurchaseController : IEventObserver
	{
		private const int MAX_NUM_VALIDATIONS_ATTEMPTS = 0;

		private const int MAX_STORE_RETRY_ATTEMPTS = 0;

		private const int SCHEDULED_VALIDATION_DELAY = 10;

		private const string TARGETED_OFFER_STRING = "targetedOffer";

		private IInAppPurchaseManager iapManager;

		private EventManager eventManager;

		private int numTimesValidatedItems;

		private int numStoreRetryAttempts;

		private int expectedIAPCount;

		private Dictionary<string, InAppPurchaseProductInfo> products;

		private Dictionary<string, InAppPurchaseTypeVO> validIAPTypes;

		public bool AreProductIdsReady
		{
			get;
			private set;
		}

		public InAppPurchaseController()
		{
			Service.Set<InAppPurchaseController>(this);
			this.numTimesValidatedItems = 0;
			this.AreProductIdsReady = false;
			this.iapManager = new WindowsIAPManager();
			this.iapManager.Init();
			this.products = new Dictionary<string, InAppPurchaseProductInfo>();
			this.validIAPTypes = new Dictionary<string, InAppPurchaseTypeVO>();
			this.eventManager = Service.Get<EventManager>();
			this.eventManager.RegisterObserver(this, EventId.SuccessfullyResumed, EventPriority.Default);
			if (!Service.Get<EnvironmentController>().IsRestrictedProfile())
			{
				this.eventManager.RegisterObserver(this, EventId.InitializeGeneralSystemsEnd, EventPriority.Default);
				this.eventManager.RegisterObserver(this, EventId.StoreScreenReady, EventPriority.Default);
				this.eventManager.RegisterObserver(this, EventId.StoreCategorySelected, EventPriority.Default);
				this.eventManager.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
				this.eventManager.RegisterObserver(this, EventId.TargetedBundleReserve, EventPriority.Default);
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.WorldLoadComplete)
			{
				if (id != EventId.StoreCategorySelected)
				{
					if (id != EventId.StoreScreenReady)
					{
						if (id == EventId.WorldLoadComplete)
						{
							this.eventManager.UnregisterObserver(this, EventId.WorldLoadComplete);
							if (!this.AreProductIdsReady)
							{
								this.eventManager.RegisterObserver(this, EventId.IAPProductIDsReady, EventPriority.Default);
							}
							else
							{
								this.RestorePurchases();
							}
						}
					}
					else if (this.numStoreRetryAttempts < 0)
					{
						if (this.products.Count == 0)
						{
							this.numStoreRetryAttempts++;
							Service.Get<BILoggingController>().TrackGameAction("iap", "get_products_store_retry", "", "");
							this.ValidateIAPItems(true);
						}
					}
					else if (this.products.Count == 0)
					{
						Service.Get<BILoggingController>().TrackGameAction("iap", "get_products_store_fail", this.numStoreRetryAttempts.ToString(), "");
					}
				}
				else
				{
					StoreTab storeTab = (StoreTab)cookie;
					if (storeTab == StoreTab.Treasure && this.products.Count == 0)
					{
						Service.Get<BILoggingController>().TrackGameAction("iap", "get_products_treasure_empty", "no_valid_products", "");
					}
				}
			}
			else if (id <= EventId.SuccessfullyResumed)
			{
				if (id != EventId.InitializeGeneralSystemsEnd)
				{
					if (id == EventId.SuccessfullyResumed)
					{
						Service.Get<InAppPurchaseController>().OnApplicationResume();
					}
				}
				else
				{
					this.CheckExpectedIAPCount();
					this.GetValidProductsFromStore();
					this.ScheduleValidateIAPItems();
					this.eventManager.UnregisterObserver(this, EventId.InitializeGeneralSystemsEnd);
				}
			}
			else if (id != EventId.IAPProductIDsReady)
			{
				if (id == EventId.TargetedBundleReserve)
				{
					string productId = (string)cookie;
					this.ReserveTargetBundleOnServer(productId);
				}
			}
			else
			{
				this.eventManager.UnregisterObserver(this, EventId.IAPProductIDsReady);
			}
			return EatResponse.NotEaten;
		}

		private void ReserveTargetBundleOnServer(string productId)
		{
			ReserveTargetedOfferIDRequest reserveTargetedOfferIDRequest = new ReserveTargetedOfferIDRequest();
			TargetedBundleController targetedBundleController = Service.Get<TargetedBundleController>();
			if (targetedBundleController.CurrentTargetedOffer != null)
			{
				reserveTargetedOfferIDRequest.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
				reserveTargetedOfferIDRequest.ProductId = productId;
				reserveTargetedOfferIDRequest.OfferId = targetedBundleController.CurrentTargetedOffer.Uid;
				ReserveTargetedOfferCommand reserveTargetedOfferCommand = new ReserveTargetedOfferCommand(reserveTargetedOfferIDRequest);
				reserveTargetedOfferCommand.AddFailureCallback(new AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>.OnFailureCallback(this.OnFailure));
				Service.Get<ServerAPI>().Sync(reserveTargetedOfferCommand);
			}
		}

		private void OnFailure(uint status, object cookie)
		{
			Service.Get<StaRTSLogger>().Error("Failed in reserving targeted bundle offers.  Status = " + status.ToString());
		}

		private void ScheduleValidateIAPItems()
		{
			Service.Get<ViewTimerManager>().CreateViewTimer(10f, false, new TimerDelegate(this.ScheduledCallback), 0);
		}

		private void ScheduledCallback(uint id, object cookie)
		{
			this.ValidateIAPItems(true);
		}

		public bool IsIAPForCurrentPlatform(InAppPurchaseTypeVO iapVO)
		{
			bool result = false;
			if (iapVO.Type == "ws")
			{
				result = true;
			}
			return result;
		}

		private void CheckExpectedIAPCount()
		{
		}

		public void GetValidProductsFromStore()
		{
			if (!Service.Get<EnvironmentController>().IsRestrictedProfile())
			{
				this.iapManager.GetProducts();
				return;
			}
			Service.Get<BILoggingController>().TrackGameAction("iap", "iap_restricted_account", "restricted_user", "");
		}

		public void OnGetInfoForProducts(List<InAppPurchaseProductInfo> productsFromNative)
		{
			Dictionary<string, InAppPurchaseTypeVO> allIAPTypesByProductID = this.GetAllIAPTypesByProductID();
			int count = productsFromNative.Count;
			for (int i = 0; i < count; i++)
			{
				InAppPurchaseProductInfo inAppPurchaseProductInfo = productsFromNative[i];
				Service.Get<StaRTSLogger>().Debug("IAP Product: " + inAppPurchaseProductInfo.ToString());
				InAppPurchaseTypeVO inAppPurchaseTypeVO = allIAPTypesByProductID[inAppPurchaseProductInfo.AppStoreId];
				if (!inAppPurchaseTypeVO.IsPromo)
				{
					if (!this.products.ContainsKey(inAppPurchaseProductInfo.AppStoreId))
					{
						this.products.Add(inAppPurchaseProductInfo.AppStoreId, inAppPurchaseProductInfo);
					}
					if (!this.validIAPTypes.ContainsKey(inAppPurchaseTypeVO.ProductId))
					{
						this.validIAPTypes.Add(inAppPurchaseTypeVO.ProductId, inAppPurchaseTypeVO);
					}
				}
			}
			Service.Get<StaRTSLogger>().Debug("Number of valid products: " + count);
			this.ValidateIAPItems(false);
			if (this.products.Count == this.expectedIAPCount)
			{
				if (this.numStoreRetryAttempts < 1)
				{
					int num = this.numStoreRetryAttempts - 1;
					Service.Get<BILoggingController>().TrackGameAction("iap", "get_products_init_success", num.ToString(), "");
				}
				else
				{
					Service.Get<BILoggingController>().TrackGameAction("iap", "get_products_store_success", this.numStoreRetryAttempts.ToString(), "");
				}
				this.AreProductIdsReady = true;
				Service.Get<EventManager>().SendEvent(EventId.IAPProductIDsReady, null);
			}
		}

		public void OnGetInfoForProducts(string value)
		{
			Dictionary<string, InAppPurchaseTypeVO> allIAPTypesByProductID = this.GetAllIAPTypesByProductID();
			IDictionary<string, object> dictionary = new JsonParser(value).Parse() as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("products"))
			{
				List<object> list = dictionary.get_Item("products") as List<object>;
				if (list != null)
				{
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						InAppPurchaseProductInfo inAppPurchaseProductInfo = InAppPurchaseProductInfo.Parse(list[i]);
						Service.Get<StaRTSLogger>().Debug("IAP Product: " + inAppPurchaseProductInfo.ToString());
						if (allIAPTypesByProductID.ContainsKey(inAppPurchaseProductInfo.AppStoreId))
						{
							InAppPurchaseTypeVO inAppPurchaseTypeVO = allIAPTypesByProductID[inAppPurchaseProductInfo.AppStoreId];
							if (!inAppPurchaseTypeVO.IsPromo)
							{
								if (!this.products.ContainsKey(inAppPurchaseProductInfo.AppStoreId))
								{
									this.products.Add(inAppPurchaseProductInfo.AppStoreId, inAppPurchaseProductInfo);
								}
								if (!this.validIAPTypes.ContainsKey(inAppPurchaseTypeVO.ProductId))
								{
									this.validIAPTypes.Add(inAppPurchaseTypeVO.ProductId, inAppPurchaseTypeVO);
								}
							}
						}
						else
						{
							Service.Get<StaRTSLogger>().Debug("IAP Item no longer supported: " + inAppPurchaseProductInfo.AppStoreId);
						}
					}
					Service.Get<StaRTSLogger>().Debug("Number of valid products: " + count);
				}
			}
			this.ValidateIAPItems(false);
			if (this.products.Count == this.expectedIAPCount)
			{
				if (this.numStoreRetryAttempts < 1)
				{
					int num = this.numStoreRetryAttempts - 1;
					Service.Get<BILoggingController>().TrackGameAction("iap", "get_products_init_success", num.ToString(), "");
				}
				else
				{
					Service.Get<BILoggingController>().TrackGameAction("iap", "get_products_store_success", this.numStoreRetryAttempts.ToString(), "");
				}
				this.AreProductIdsReady = true;
				Service.Get<EventManager>().SendEvent(EventId.IAPProductIDsReady, null);
			}
		}

		public void Consume(string consumeId)
		{
			this.iapManager.Consume(consumeId);
		}

		public void RestorePurchases()
		{
			this.iapManager.RestorePurchases();
		}

		public void PurchaseProduct(string productID)
		{
			Service.Get<GameIdleController>().Enabled = false;
			this.iapManager.Purchase(productID);
		}

		public void OnPurchaseProductResponse(string bulkReceipt, bool gameIdleControllerEnabled = true)
		{
			Service.Get<GameIdleController>().Enabled = gameIdleControllerEnabled;
			MoneyReceiptVerifyRequest moneyReceiptVerifyRequest = new MoneyReceiptVerifyRequest();
			string text = Regex.Unescape(bulkReceipt);
			text = Regex.Unescape(text);
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			string receipt = Convert.ToBase64String(bytes);
			moneyReceiptVerifyRequest.Receipt = receipt;
			moneyReceiptVerifyRequest.VendorKey = "Windows";
			moneyReceiptVerifyRequest.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			MoneyReceiptVerifyCommand command = new MoneyReceiptVerifyCommand(moneyReceiptVerifyRequest);
			Service.Get<ServerAPI>().Async(command);
		}

		public void HandleReceiptVerificationResponse(string uid, string transactionId, string currencyCode, double price, double bonusMultiplier, bool isPromo, string offerUid, CrateData crateData)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			IDataController dataController = Service.Get<IDataController>();
			InAppPurchaseTypeVO inAppPurchaseTypeVO = dataController.Get<InAppPurchaseTypeVO>(uid);
			if (inAppPurchaseTypeVO.IsPromo)
			{
				isPromo = true;
			}
			bool flag = !string.IsNullOrEmpty(offerUid);
			string text = inAppPurchaseTypeVO.RewardEmpire;
			if (currentPlayer.Faction == FactionType.Rebel)
			{
				text = inAppPurchaseTypeVO.RewardRebel;
			}
			if (string.IsNullOrEmpty(text) && !flag)
			{
				Service.Get<StaRTSLogger>().Error("MoneyReceiptVerifyResponse:" + inAppPurchaseTypeVO.Uid + " faction specific reward uids do not exist");
				return;
			}
			bool flag2 = false;
			TargetedBundleVO targetedBundleVO = null;
			if (flag)
			{
				targetedBundleVO = dataController.GetOptional<TargetedBundleVO>(offerUid);
				if (targetedBundleVO != null)
				{
					Service.Get<TargetedBundleController>().GrantTargetedBundleRewards(targetedBundleVO);
					flag2 = true;
				}
				else
				{
					Service.Get<StaRTSLogger>().Error("MoneyReceiptVerifyResponse: targeted offer " + offerUid + " does not exist");
				}
			}
			else
			{
				RewardVO optional = dataController.GetOptional<RewardVO>(text);
				if (optional == null)
				{
					Service.Get<StaRTSLogger>().Error("MoneyReceiptVerifyResponse:" + text + " does not exist");
				}
				else if (inAppPurchaseTypeVO.CurrencyType.Equals("hard"))
				{
					RewardUtils.GrantReward(currentPlayer, optional, bonusMultiplier);
				}
				else
				{
					RewardUtils.GrantInAppPurchaseRewardToHQInventory(optional);
				}
			}
			if (inAppPurchaseTypeVO.ProductId.Contains("promo"))
			{
				isPromo = true;
			}
			int amount;
			if (inAppPurchaseTypeVO.CurrencyType.Equals("hard"))
			{
				amount = (int)Math.Floor(bonusMultiplier * (double)inAppPurchaseTypeVO.Amount);
			}
			else
			{
				amount = inAppPurchaseTypeVO.Amount;
			}
			this.Consume(inAppPurchaseTypeVO.ProductId);
			string text2 = uid;
			bool flag3 = false;
			SaleTypeVO currentActiveSale = SaleUtils.GetCurrentActiveSale();
			if (currentActiveSale != null && !isPromo)
			{
				text2 = text2 + "_" + currentActiveSale.Uid;
			}
			if (isPromo)
			{
				text2 += "_promo";
				flag3 = true;
			}
			if (GameConstants.IAP_FORCE_POPUP_ENABLED)
			{
				flag3 = true;
			}
			if (flag2)
			{
				Service.Get<TargetedBundleController>().HandleTargetedOfferSuccess(crateData, targetedBundleVO);
			}
			else if (flag3)
			{
				this.ShowRedemptionScreen(amount, uid);
				Service.Get<EventManager>().SendEvent(EventId.InAppPurchaseMade, null);
			}
			Service.Get<DMOAnalyticsController>().LogPaymentAction(currencyCode, price, inAppPurchaseTypeVO.ProductId, 1, text2);
		}

		private void ShowRedemptionScreen(int amount, string uid)
		{
			PromoRedemptionScreen promoRedemptionScreen = new PromoRedemptionScreen(uid, amount);
			promoRedemptionScreen.IsAlwaysOnTop = true;
			Service.Get<ScreenController>().AddScreen(promoRedemptionScreen, true, true);
		}

		public void SetIAPRewardIcon(UXSprite iconSprite, string uid)
		{
			IDataController dataController = Service.Get<IDataController>();
			InAppPurchaseTypeVO inAppPurchaseTypeVO = dataController.Get<InAppPurchaseTypeVO>(uid);
			string rewardUid;
			if (Service.Get<CurrentPlayer>().Faction == FactionType.Empire)
			{
				rewardUid = inAppPurchaseTypeVO.RewardEmpire;
			}
			else
			{
				rewardUid = inAppPurchaseTypeVO.RewardRebel;
			}
			if (inAppPurchaseTypeVO.CurrencyType.Equals("hard") || inAppPurchaseTypeVO.CurrencyType.Equals("pack"))
			{
				UXUtils.SetupGeometryForIcon(iconSprite, inAppPurchaseTypeVO.CurrencyIconId);
				return;
			}
			RewardType rewardType = RewardType.Invalid;
			IGeometryVO config;
			Service.Get<RewardManager>().GetFirstRewardAssetName(rewardUid, ref rewardType, out config);
			RewardUtils.SetRewardIcon(iconSprite, config, AnimationPreference.NoAnimation);
		}

		public void OnApplicationResume()
		{
			this.iapManager.OnApplicationResume();
		}

		public List<InAppPurchaseTypeVO> GetValidIAPTypes()
		{
			List<InAppPurchaseTypeVO> list = new List<InAppPurchaseTypeVO>();
			foreach (KeyValuePair<string, InAppPurchaseTypeVO> current in this.validIAPTypes)
			{
				if (!current.get_Value().HideFromStore)
				{
					list.Add(current.get_Value());
				}
			}
			list.Sort(new Comparison<InAppPurchaseTypeVO>(InAppPurchaseController.CompareIAPs));
			return list;
		}

		public void ValidateIAPItems(bool bypassCheck)
		{
			if (Service.Get<EnvironmentController>().IsRestrictedProfile())
			{
				return;
			}
			if (!bypassCheck)
			{
				if (this.numTimesValidatedItems >= 0)
				{
					Service.Get<BILoggingController>().TrackGameAction("iap", "get_products_init_fail", this.numTimesValidatedItems.ToString(), "");
					return;
				}
				this.numTimesValidatedItems++;
			}
			if (this.validIAPTypes.Count < this.expectedIAPCount)
			{
				this.GetValidProductsFromStore();
			}
		}

		private static int CompareIAPs(InAppPurchaseTypeVO a, InAppPurchaseTypeVO b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a.Order >= b.Order)
			{
				return -1;
			}
			return 1;
		}

		public InAppPurchaseProductInfo GetIAPProduct(string productID)
		{
			if (this.products.ContainsKey(productID))
			{
				return this.products[productID];
			}
			return null;
		}

		public Dictionary<string, InAppPurchaseTypeVO>.ValueCollection GetAllIAPTypes()
		{
			IDataController dataController = Service.Get<IDataController>();
			return dataController.GetAll<InAppPurchaseTypeVO>();
		}

		private Dictionary<string, InAppPurchaseTypeVO> GetAllIAPTypesByProductID()
		{
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, InAppPurchaseTypeVO>.ValueCollection all = dataController.GetAll<InAppPurchaseTypeVO>();
			Dictionary<string, InAppPurchaseTypeVO> dictionary = new Dictionary<string, InAppPurchaseTypeVO>();
			foreach (InAppPurchaseTypeVO current in all)
			{
				if (this.IsIAPForCurrentPlatform(current))
				{
					dictionary.Add(current.ProductId, current);
				}
			}
			return dictionary;
		}

		protected internal InAppPurchaseController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).CheckExpectedIAPCount();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseController.CompareIAPs((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(*args), (InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).Consume(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).AreProductIdsReady);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).GetAllIAPTypes());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).GetAllIAPTypesByProductID());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).GetIAPProduct(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).GetValidIAPTypes());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).GetValidProductsFromStore();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).HandleReceiptVerificationResponse(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(double*)(args + 3), *(double*)(args + 4), *(sbyte*)(args + 5) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 6)), (CrateData)GCHandledObjects.GCHandleToObject(args[7]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).IsIAPForCurrentPlatform((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).OnApplicationResume();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).OnGetInfoForProducts((List<InAppPurchaseProductInfo>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).OnGetInfoForProducts(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).OnPurchaseProductResponse(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).PurchaseProduct(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).ReserveTargetBundleOnServer(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).RestorePurchases();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).ScheduleValidateIAPItems();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).AreProductIdsReady = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).SetIAPRewardIcon((UXSprite)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).ShowRedemptionScreen(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((InAppPurchaseController)GCHandledObjects.GCHandleToObject(instance)).ValidateIAPItems(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
