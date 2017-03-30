using StaRTS.Externals.BI;
using StaRTS.Externals.IAP;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers
{
	public class TargetedBundleController : IEventObserver
	{
		private const string TEXT_NOSPACE_TITLE = "promo_error_nospace_title";

		private const string TEXT_NOSPACE_DESC = "promo_error_nospace_desc";

		public bool FetchingNewOffer;

		public TargetedBundleVO CurrentTargetedOffer;

		private TargetedBundleVO lastKnownTargetedOffer;

		private string currentOfferCost;

		private int getOffersLimiter;

		private uint lastGetOffersQueryTime;

		private List<BuildingType> buildingTypeIgnoreList;

		public uint OfferExpiresAt
		{
			get;
			private set;
		}

		public uint GlobalCooldownExpiresAt
		{
			get;
			private set;
		}

		public uint NextOfferAvailableAt
		{
			get;
			private set;
		}

		public TargetedBundleController()
		{
			Service.Set<TargetedBundleController>(this);
			this.getOffersLimiter = GameConstants.TARGETED_OFFERS_FREQUENCY_LIMIT;
			this.buildingTypeIgnoreList = new List<BuildingType>();
			this.buildingTypeIgnoreList.Add(BuildingType.Wall);
			this.buildingTypeIgnoreList.Add(BuildingType.Turret);
			this.buildingTypeIgnoreList.Add(BuildingType.Resource);
			this.buildingTypeIgnoreList.Add(BuildingType.Barracks);
			this.buildingTypeIgnoreList.Add(BuildingType.Factory);
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (!string.IsNullOrEmpty(currentPlayer.OfferId))
			{
				this.LoadCurrentTargetedOffer(currentPlayer.OfferId, currentPlayer.TriggerDate);
			}
			else
			{
				this.GetNewOffer();
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.PlanetRelocate, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
			eventManager.RegisterObserver(this, EventId.SquadLeft);
			eventManager.RegisterObserver(this, EventId.BuildingLevelUpgraded);
			eventManager.RegisterObserver(this, EventId.BuildingConstructed);
			eventManager.RegisterObserver(this, EventId.InAppPurchaseMade);
			eventManager.RegisterObserver(this, EventId.TargetedBundleRewardRedeemed);
			eventManager.RegisterObserver(this, EventId.DroidPurchaseCompleted);
		}

		public void PrepareContent()
		{
			GetTargetedOffersCommand getTargetedOffersCommand = new GetTargetedOffersCommand(new PlayerIdRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId
			});
			getTargetedOffersCommand.AddSuccessCallback(new AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>.OnSuccessCallback(this.OnSuccess));
			getTargetedOffersCommand.AddFailureCallback(new AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>.OnFailureCallback(this.OnFailure));
			Service.Get<ServerAPI>().Enqueue(getTargetedOffersCommand);
		}

		private void LoadCurrentTargetedOffer(string offerId, uint triggeredTime)
		{
			IDataController dataController = Service.Get<IDataController>();
			this.CurrentTargetedOffer = dataController.GetOptional<TargetedBundleVO>(offerId);
			if (this.CurrentTargetedOffer != null)
			{
				this.OfferExpiresAt = triggeredTime + (uint)this.CurrentTargetedOffer.Duration;
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (this.FetchingNewOffer)
			{
				return EatResponse.NotEaten;
			}
			switch (id)
			{
			case EventId.BuildingLevelUpgraded:
			case EventId.BuildingConstructed:
			{
				ContractEventData contractEventData = (ContractEventData)cookie;
				if (contractEventData == null)
				{
					return EatResponse.NotEaten;
				}
				BuildingTypeVO buildingVO = contractEventData.BuildingVO;
				if (buildingVO == null)
				{
					return EatResponse.NotEaten;
				}
				if (!this.buildingTypeIgnoreList.Contains(buildingVO.Type))
				{
					this.GetNewOffer(id);
				}
				return EatResponse.NotEaten;
			}
			case EventId.BuildingSwapped:
				IL_24:
				switch (id)
				{
				case EventId.SquadLeft:
				case EventId.SquadJoinedByCurrentPlayer:
					goto IL_AA;
				case EventId.SquadChatFilterUpdated:
					IL_3C:
					if (id != EventId.DroidPurchaseCompleted && id != EventId.PlanetRelocate && id != EventId.TargetedBundleRewardRedeemed && id != EventId.InAppPurchaseMade)
					{
						return EatResponse.NotEaten;
					}
					goto IL_AA;
				}
				goto IL_3C;
				IL_AA:
				this.GetNewOffer(id);
				return EatResponse.NotEaten;
			}
			goto IL_24;
		}

		public bool IsCurrencyCostOffer(TargetedBundleVO offerVO)
		{
			if (offerVO.LinkedIAPs != null && offerVO.LinkedIAPs.Count > 0)
			{
				return false;
			}
			if (offerVO.Cost != null && offerVO.Cost.Length > 0)
			{
				return true;
			}
			Service.Get<Logger>().Error("Targeted Bundle Offser has no linked IAP and no Cost: " + offerVO.Uid);
			return false;
		}

		public bool FindAvailableIAP(TargetedBundleVO offerVO, out string currentIapId, out string offerCost)
		{
			bool result = false;
			IDataController dataController = Service.Get<IDataController>();
			currentIapId = string.Empty;
			offerCost = string.Empty;
			InAppPurchaseController inAppPurchaseController = Service.Get<InAppPurchaseController>();
			int i = 0;
			int count = offerVO.LinkedIAPs.Count;
			while (i < count)
			{
				string uid = offerVO.LinkedIAPs[i];
				InAppPurchaseTypeVO optional = dataController.GetOptional<InAppPurchaseTypeVO>(uid);
				if (optional != null)
				{
					InAppPurchaseProductInfo iAPProduct = inAppPurchaseController.GetIAPProduct(optional.ProductId);
					if (iAPProduct != null)
					{
						currentIapId = optional.ProductId;
						offerCost = iAPProduct.FormattedRealCost;
						result = true;
						break;
					}
				}
				i++;
			}
			return result;
		}

		public void BuyTargetedOfferWithCurrency()
		{
			if (this.CurrentTargetedOffer == null)
			{
				Service.Get<Logger>().Warn("No offer available to purchase.");
				return;
			}
			GameUtils.SpendCurrency(this.CurrentTargetedOffer.Cost, true);
			this.GrantTargetedBundleRewards(this.CurrentTargetedOffer);
			ProcessingScreen.Show();
			BuyTargetedOfferRequest request = new BuyTargetedOfferRequest(this.CurrentTargetedOffer.Uid);
			BuyTargetedOfferCommand buyTargetedOfferCommand = new BuyTargetedOfferCommand(request);
			buyTargetedOfferCommand.AddSuccessCallback(new AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>.OnSuccessCallback(this.OnBuySuccess));
			Service.Get<ServerAPI>().Sync(buyTargetedOfferCommand);
		}

		private void OnBuySuccess(BuyTargetedOfferResponse response, object cookie)
		{
			ProcessingScreen.Hide();
			this.HandleTargetedOfferSuccess(response.CrateDataTO, this.CurrentTargetedOffer);
			this.UpdateFromTargetedOfferSummary(response.TargetedOffers);
		}

		public void HandleTargetedOfferSuccess(CrateData crateDataTO, TargetedBundleVO offerVO)
		{
			if (crateDataTO != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.OpeningPurchasedCrate, null);
				List<string> resolvedSupplyIdList = GameUtils.GetResolvedSupplyIdList(crateDataTO);
				Service.Get<InventoryCrateRewardController>().GrantInventoryCrateReward(resolvedSupplyIdList, crateDataTO);
				this.LogTargetedBundleBI("purchase_complete", offerVO);
			}
			else
			{
				TargetedBundleRewardedConfirmScreen screen = new TargetedBundleRewardedConfirmScreen(offerVO);
				Service.Get<ScreenController>().AddScreen(screen);
			}
			this.ResetOffer();
		}

		private void UpdateFromTargetedOfferSummary(TargetedOfferSummary offerSummary)
		{
			DateTime serverDateTime = Service.Get<ServerAPI>().ServerDateTime;
			IDataController dataController = Service.Get<IDataController>();
			bool flag = false;
			TargetedBundleVO targetedBundleVO = null;
			if (!string.IsNullOrEmpty(offerSummary.AvailableOffer))
			{
				string availableOffer = offerSummary.AvailableOffer;
				targetedBundleVO = dataController.GetOptional<TargetedBundleVO>(availableOffer);
				if (targetedBundleVO != null)
				{
					flag = targetedBundleVO.IgnoreCooldown;
				}
			}
			int secondsFromEpoch = DateUtils.GetSecondsFromEpoch(serverDateTime);
			uint globalCooldownExpiresAt = offerSummary.GlobalCooldownExpiresAt;
			if (!flag && globalCooldownExpiresAt != 0u && (long)secondsFromEpoch < (long)((ulong)globalCooldownExpiresAt))
			{
				Service.Get<Logger>().DebugFormat("Offer global cooldown is in effect until {0}", new object[]
				{
					LangUtils.FormatTime((long)offerSummary.GlobalCooldownExpiresAt)
				});
				this.GlobalCooldownExpiresAt = offerSummary.GlobalCooldownExpiresAt;
				this.OfferExpiresAt = 0u;
				this.NextOfferAvailableAt = 0u;
				this.SetBoundedTimer((long)((ulong)this.GlobalCooldownExpiresAt - (ulong)((long)secondsFromEpoch)), new TimerDelegate(this.CheckForNewOffer), false);
			}
			else if (targetedBundleVO != null)
			{
				Service.Get<Logger>().DebugFormat("Available offer: {0}", new object[]
				{
					offerSummary.AvailableOffer
				});
				if (targetedBundleVO.StartTime <= serverDateTime && serverDateTime < targetedBundleVO.EndTime && AudienceConditionUtils.IsValidForClient(targetedBundleVO.AudienceConditions))
				{
					this.InitializeTriggerOffer(targetedBundleVO);
				}
				else
				{
					Service.Get<Logger>().WarnFormat("Server provided offer {0} was outside of date range {1} to {2}.", new object[]
					{
						offerSummary.AvailableOffer,
						targetedBundleVO.StartTime,
						targetedBundleVO.EndTime
					});
				}
			}
			else if (offerSummary.NextOfferAvailableAt > 0u)
			{
				Service.Get<Logger>().DebugFormat("Next offer available at {0}.", new object[]
				{
					LangUtils.FormatTime((long)offerSummary.NextOfferAvailableAt)
				});
				this.NextOfferAvailableAt = offerSummary.NextOfferAvailableAt;
				this.GlobalCooldownExpiresAt = 0u;
				this.OfferExpiresAt = 0u;
				this.SetBoundedTimer((long)((ulong)this.NextOfferAvailableAt - (ulong)((long)DateUtils.GetSecondsFromEpoch(serverDateTime))), new TimerDelegate(this.CheckForNewOffer), false);
			}
			else
			{
				this.GlobalCooldownExpiresAt = 0u;
				this.OfferExpiresAt = 0u;
				this.NextOfferAvailableAt = 0u;
			}
			this.lastKnownTargetedOffer = null;
			this.FetchingNewOffer = false;
			if (this.CurrentTargetedOffer != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.TargetedBundleContentPrepared, null);
			}
		}

		public void ResetOffer()
		{
			this.CurrentTargetedOffer = null;
		}

		public void GetNewOffer()
		{
			this.GetNewOffer(EventId.Nop);
		}

		public void GetNewOffer(EventId triggerEventId)
		{
			if (!GameConstants.TARGETED_OFFERS_ENABLED)
			{
				this.KillSwitchClearOffers();
				Service.Get<EventManager>().SendEvent(EventId.TargetedBundleContentPrepared, null);
				return;
			}
			uint serverTime = Service.Get<ServerAPI>().ServerTime;
			uint num = serverTime - this.lastGetOffersQueryTime;
			bool flag = (ulong)num < (ulong)((long)this.getOffersLimiter);
			if (flag)
			{
				string message = "Querying GetNewOffers too frequently: " + triggerEventId.ToString();
				Service.Get<Logger>().Warn(message);
				return;
			}
			this.lastGetOffersQueryTime = serverTime;
			this.FetchingNewOffer = true;
			this.lastKnownTargetedOffer = this.CurrentTargetedOffer;
			this.PrepareContent();
			Service.Get<EventManager>().SendEvent(EventId.TargetedBundleContentPrepared, null);
		}

		public bool CanDisplaySPDButton()
		{
			return !this.FetchingNewOffer && this.CurrentTargetedOffer != null;
		}

		public void GrantTargetedBundleRewards(TargetedBundleVO offerVO)
		{
			IDataController dataController = Service.Get<IDataController>();
			int i = 0;
			int count = offerVO.RewardUIDs.Count;
			while (i < count)
			{
				RewardVO optional = dataController.GetOptional<RewardVO>(offerVO.RewardUIDs[i]);
				if (optional == null)
				{
					Service.Get<Logger>().WarnFormat("Trying to grant {0} which cannot be found.", new object[]
					{
						offerVO.RewardUIDs[i]
					});
				}
				else
				{
					bool flag = true;
					if (optional.CurrencyRewards != null)
					{
						Dictionary<string, int> dictionary = GameUtils.ListToMap(optional.CurrencyRewards);
						int num = 0;
						dictionary.TryGetValue("crystals", out num);
						if (num > 0)
						{
							flag = false;
						}
					}
					else if (!string.IsNullOrEmpty(optional.DroidRewards))
					{
						flag = false;
					}
					else if (optional.BuildingInstantRewards != null || optional.BuildingInstantUpgrades != null || optional.HeroResearchInstantUpgrades != null || optional.TroopResearchInstantUpgrades != null || optional.SpecAttackResearchInstantUpgrades != null)
					{
						flag = false;
					}
					if (flag)
					{
						RewardUtils.GrantInAppPurchaseRewardToHQInventory(optional);
					}
					else
					{
						RewardUtils.GrantReward(Service.Get<CurrentPlayer>(), optional);
					}
				}
				i++;
			}
		}

		public void LogTargetedBundleBI(string action)
		{
			this.LogTargetedBundleBI(action, this.CurrentTargetedOffer);
		}

		public void LogTargetedBundleBI(string action, TargetedBundleVO currentOffer)
		{
			string context = "SPD";
			string uid = currentOffer.Uid;
			string text = this.currentOfferCost;
			string text2 = currentOffer.Groups[0];
			int count = currentOffer.RewardUIDs.Count;
			string text3 = (count <= 0) ? string.Empty : currentOffer.RewardUIDs[0];
			string text4 = (count <= 1) ? string.Empty : currentOffer.RewardUIDs[1];
			string text5 = (count <= 2) ? string.Empty : currentOffer.RewardUIDs[2];
			string text6 = currentOffer.Duration.ToString();
			int num = (int)(this.OfferExpiresAt - Service.Get<ServerAPI>().ServerTime);
			if (num < 0)
			{
				num = 0;
			}
			string text7 = num.ToString();
			string message = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", new object[]
			{
				uid,
				text,
				text2,
				text3,
				text4,
				text5,
				text6,
				text7
			});
			Service.Get<BILoggingController>().TrackGameAction(context, action, message, null);
		}

		private void OnFailure(uint status, object cookie)
		{
			Service.Get<Logger>().Error("Failed in getting targeted bundle offers");
			this.FetchingNewOffer = false;
		}

		private void OnSuccess(GetTargetedOffersResponse response, object cookie)
		{
			this.ResetOffer();
			this.UpdateFromTargetedOfferSummary(response.TargetedOffers);
		}

		private void KillSwitchClearOffers()
		{
			this.ResetOffer();
			this.GlobalCooldownExpiresAt = 0u;
			this.OfferExpiresAt = 0u;
			this.NextOfferAvailableAt = 0u;
			this.lastKnownTargetedOffer = null;
			this.FetchingNewOffer = false;
		}

		private void SetBoundedTimer(long delay, TimerDelegate callback, bool warnIfExceeds = false)
		{
			if (delay > 0L && delay <= 432000L)
			{
				Service.Get<ViewTimerManager>().CreateViewTimer((float)delay, false, callback, null);
			}
			else if (warnIfExceeds)
			{
				Service.Get<Logger>().WarnFormat("Targeted offer tried to set a timer exceeding TimerManager.MAX_DELAY ({0}): {1} ({2})", new object[]
				{
					LangUtils.FormatTime(432000L),
					LangUtils.FormatTime(delay),
					delay
				});
			}
		}

		private void InitializeTriggerOffer(TargetedBundleVO offerVO)
		{
			if (offerVO == null)
			{
				Service.Get<Logger>().Error("InitializeTriggerOffer called with null value.");
				return;
			}
			if (this.lastKnownTargetedOffer != null && this.lastKnownTargetedOffer.Uid == offerVO.Uid)
			{
				this.CurrentTargetedOffer = this.lastKnownTargetedOffer;
			}
			else
			{
				this.CurrentTargetedOffer = offerVO;
				uint duration = (uint)offerVO.Duration;
				this.OfferExpiresAt = Service.Get<ServerAPI>().ServerTime + duration;
				TriggerTargetedOfferCommand triggerTargetedOfferCommand = new TriggerTargetedOfferCommand(new TargetedOfferIDRequest
				{
					PlayerId = Service.Get<CurrentPlayer>().PlayerId,
					OfferId = offerVO.Uid
				});
				triggerTargetedOfferCommand.AddSuccessCallback(new AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>.OnSuccessCallback(this.OnTriggeredOfferResponse));
				Service.Get<ServerAPI>().Enqueue(triggerTargetedOfferCommand);
			}
			string text;
			this.FindAvailableIAP(this.CurrentTargetedOffer, out text, out this.currentOfferCost);
		}

		private void OnTriggeredOfferResponse(TriggerTargetedOfferResponse response, object cookie)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			bool flag = false;
			if (!string.Equals(currentPlayer.OfferId, response.OfferId))
			{
				flag = true;
			}
			if (response.TriggerDate != currentPlayer.TriggerDate)
			{
				flag = true;
			}
			if (flag)
			{
				this.LoadCurrentTargetedOffer(response.OfferId, response.TriggerDate);
			}
		}

		private void CheckForNewOffer(uint timerId, object cookie)
		{
			this.GetNewOffer();
		}

		public CrateVO GetCrateVOFromTargetedOffer(TargetedBundleVO offerVO)
		{
			IDataController dataController = Service.Get<IDataController>();
			if (offerVO == null)
			{
				return null;
			}
			if (offerVO.RewardUIDs == null || offerVO.RewardUIDs.Count < 1)
			{
				return null;
			}
			int i = 0;
			int count = offerVO.RewardUIDs.Count;
			while (i < count)
			{
				RewardVO optional = Service.Get<IDataController>().GetOptional<RewardVO>(offerVO.RewardUIDs[i]);
				if (optional != null)
				{
					string crateReward = optional.CrateReward;
					if (!string.IsNullOrEmpty(crateReward))
					{
						return dataController.GetOptional<CrateVO>(crateReward);
					}
				}
				i++;
			}
			return null;
		}

		public void MakeTargetedBundlePurchase(TargetedBundleVO currentOffer, string iapId)
		{
			bool flag = false;
			if (this.IsCurrencyCostOffer(currentOffer))
			{
				flag = true;
				this.HandleCurrencyOfferPurchase(currentOffer);
			}
			else if (iapId != null && this.CanPurchaseIAPLinkedOffer(currentOffer))
			{
				flag = true;
				Service.Get<EventManager>().SendEvent(EventId.InAppPurchaseSelect, iapId);
				Service.Get<EventManager>().SendEvent(EventId.TargetedBundleReserve, iapId);
				Service.Get<InAppPurchaseController>().PurchaseProduct(iapId);
			}
			if (flag)
			{
				this.LogTargetedBundleBI("purchase_attempt");
			}
		}

		private void HandleCurrencyOfferPurchase(TargetedBundleVO currentOffer)
		{
			int credits = 0;
			int materials = 0;
			int contraband = 0;
			int num = 0;
			int num2 = 0;
			GameUtils.GetCurrencyCost(currentOffer.Cost, out credits, out materials, out contraband, out num2, out num);
			if (!GameUtils.CanAffordCosts(credits, materials, contraband, num))
			{
				if (num == 0)
				{
					PayMeScreen.ShowIfNotEnoughCurrency(credits, materials, contraband, "SPD", new OnScreenModalResult(this.OnCurrencyPurchased));
				}
				else
				{
					GameUtils.PromptToBuyCrystals();
				}
			}
			else
			{
				this.MakeCurrencyOfferPurchase();
			}
		}

		private void MakeCurrencyOfferPurchase()
		{
			this.BuyTargetedOfferWithCurrency();
		}

		private void OnCurrencyPurchased(object result, object cookie)
		{
			if (GameUtils.HandleSoftCurrencyFlow(result, cookie))
			{
				this.MakeCurrencyOfferPurchase();
			}
		}

		private bool CanPurchaseIAPLinkedOffer(TargetedBundleVO currentOffer)
		{
			bool flag = true;
			IDataController dataController = Service.Get<IDataController>();
			int i = 0;
			int count = currentOffer.RewardUIDs.Count;
			while (i < count)
			{
				RewardVO optional = dataController.GetOptional<RewardVO>(currentOffer.RewardUIDs[i]);
				if (optional != null && optional.BuildingInstantRewards != null)
				{
					flag = this.CanPlaceAllRewardBuildings(optional.BuildingInstantRewards);
					if (!flag)
					{
						AlertScreen.ShowModal(false, Service.Get<Lang>().Get("promo_error_nospace_title", new object[0]), Service.Get<Lang>().Get("promo_error_nospace_desc", new object[0]), null, null);
						break;
					}
				}
				i++;
			}
			return flag;
		}

		private bool CanPlaceAllRewardBuildings(string[] rewardBuildings)
		{
			bool result = true;
			BuildingController buildingController = Service.Get<BuildingController>();
			int i = 0;
			int num = rewardBuildings.Length;
			while (i < num)
			{
				string[] array = rewardBuildings[i].Split(new char[]
				{
					':'
				});
				int num2 = Convert.ToInt32(array[1]);
				string uid = array[0];
				BuildingTypeVO optional = Service.Get<IDataController>().GetOptional<BuildingTypeVO>(uid);
				if (optional != null)
				{
					for (int j = 0; j < num2; j++)
					{
						if (!buildingController.FoundFirstEmptySpaceFor(optional))
						{
							result = false;
							break;
						}
					}
				}
				i++;
			}
			return result;
		}
	}
}
