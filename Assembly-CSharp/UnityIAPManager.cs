using StaRTS.Externals.IAP;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using WinRTBridge;

public class UnityIAPManager : IStoreListener
{
	private IStoreController m_StoreController;

	private IExtensionProvider m_StoreExtensionProvider;

	public void InitializePurchasing(List<string> products)
	{
		if (this.IsInitialized())
		{
			return;
		}
		ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(), new IPurchasingModule[0]);
		for (int i = 0; i < products.Count; i++)
		{
			configurationBuilder.AddProduct(products[i], ProductType.Consumable, new IDs
			{
				{
					products[i],
					new string[]
					{
						"WinRT"
					}
				}
			});
		}
		UnityPurchasing.Initialize(this, configurationBuilder);
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		this.m_StoreController = controller;
		this.m_StoreExtensionProvider = extensions;
		List<InAppPurchaseProductInfo> list = new List<InAppPurchaseProductInfo>();
		Product[] all = controller.products.all;
		for (int i = 0; i < all.Length; i++)
		{
			Product product = all[i];
			InAppPurchaseProductInfo inAppPurchaseProductInfo = new InAppPurchaseProductInfo();
			inAppPurchaseProductInfo.AppStoreId = product.definition.storeSpecificId;
			inAppPurchaseProductInfo.Name = product.metadata.localizedTitle;
			inAppPurchaseProductInfo.FormattedRealCost = product.metadata.localizedPriceString;
			inAppPurchaseProductInfo.RealCost = Regex.Replace(inAppPurchaseProductInfo.FormattedRealCost, "[^\\s,.0-9]", "");
			list.Add(inAppPurchaseProductInfo);
		}
		Debug.Log("Billing initialized");
		Service.Get<InAppPurchaseController>().OnGetInfoForProducts(list);
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("Billing failed to initialize!");
		switch (error)
		{
		case InitializationFailureReason.PurchasingUnavailable:
			Debug.Log("Billing disabled!");
			return;
		case InitializationFailureReason.NoProductsAvailable:
			Debug.Log("No products available for purchase!");
			return;
		case InitializationFailureReason.AppNotKnown:
			Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
			return;
		default:
			return;
		}
	}

	public bool IsInitialized()
	{
		return this.m_StoreController != null && this.m_StoreExtensionProvider != null;
	}

	public void MakePurchase(string productID)
	{
		if (!this.IsInitialized())
		{
			return;
		}
		Product product = this.m_StoreController.products.WithID(productID);
		if (product != null && product.availableToPurchase)
		{
			this.m_StoreController.InitiatePurchase(product);
		}
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Service.Get<StaRTSLogger>().Debug("Purchase failed: " + failureReason.ToString());
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		Service.Get<StaRTSLogger>().Debug("ProcessPurchase: " + args.purchasedProduct.receipt);
		IDictionary<string, object> dictionary = new JsonParser(args.purchasedProduct.receipt).Parse() as Dictionary<string, object>;
		string bulkReceipt = dictionary.get_Item("Payload").ToString();
		Service.Get<InAppPurchaseController>().OnPurchaseProductResponse(bulkReceipt, true);
		return PurchaseProcessingResult.Complete;
	}

	public UnityIAPManager()
	{
	}

	protected internal UnityIAPManager(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UnityIAPManager)GCHandledObjects.GCHandleToObject(instance)).InitializePurchasing((List<string>)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UnityIAPManager)GCHandledObjects.GCHandleToObject(instance)).IsInitialized());
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UnityIAPManager)GCHandledObjects.GCHandleToObject(instance)).MakePurchase(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UnityIAPManager)GCHandledObjects.GCHandleToObject(instance)).OnInitialized((IStoreController)GCHandledObjects.GCHandleToObject(*args), (IExtensionProvider)GCHandledObjects.GCHandleToObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UnityIAPManager)GCHandledObjects.GCHandleToObject(instance)).OnInitializeFailed((InitializationFailureReason)(*(int*)args));
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UnityIAPManager)GCHandledObjects.GCHandleToObject(instance)).OnPurchaseFailed((Product)GCHandledObjects.GCHandleToObject(*args), (PurchaseFailureReason)(*(int*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UnityIAPManager)GCHandledObjects.GCHandleToObject(instance)).ProcessPurchase((PurchaseEventArgs)GCHandledObjects.GCHandleToObject(*args)));
	}
}
