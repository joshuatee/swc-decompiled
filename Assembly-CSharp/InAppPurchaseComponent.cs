using StaRTS.Externals.BI;
using StaRTS.Externals.IAP;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

public class InAppPurchaseComponent : MonoBehaviour, IUnitySerializable
{
	public void OnGetInfoForProducts(string jsonString)
	{
		if (!string.IsNullOrEmpty(jsonString))
		{
			if (Service.IsSet<StaRTSLogger>())
			{
				Service.Get<StaRTSLogger>().Debug("ProductInfoString: " + jsonString);
			}
			if (Service.IsSet<InAppPurchaseController>())
			{
				Service.Get<InAppPurchaseController>().OnGetInfoForProducts(jsonString);
				return;
			}
			if (Service.IsSet<StaRTSLogger>())
			{
				Service.Get<StaRTSLogger>().Error("ProductInfoString: InAppPurchaseController is not set");
				return;
			}
		}
		else if (Service.IsSet<StaRTSLogger>())
		{
			Service.Get<StaRTSLogger>().Error("ProductInfoString: IsNullOrEmpty");
		}
	}

	public void OnPurchaseProduct(string jsonString)
	{
		if (Service.IsSet<StaRTSLogger>())
		{
			Service.Get<StaRTSLogger>().Debug("OnPurchaseProduct: " + jsonString);
		}
		if (!string.IsNullOrEmpty(jsonString) && Service.IsSet<InAppPurchaseController>())
		{
			Service.Get<InAppPurchaseController>().OnPurchaseProductResponse(jsonString, true);
		}
	}

	public void onConsumePurchase(string jsonString)
	{
		if (!string.IsNullOrEmpty(jsonString))
		{
			Service.Get<StaRTSLogger>().Debug("OnConsumeProduct: " + jsonString);
		}
	}

	public void OnBILog(string biLog)
	{
		if (string.IsNullOrEmpty(biLog))
		{
			return;
		}
		string text = biLog;
		string action = "";
		int num = text.IndexOf('|');
		if (num >= 0)
		{
			action = text.Substring(num + 1);
			text = text.Substring(0, num);
		}
		if (Service.IsSet<BILoggingController>())
		{
			Service.Get<BILoggingController>().TrackIAPGameAction("iap", action, text);
		}
	}

	public void OnRestorePurchases(string jsonString)
	{
		if (!string.IsNullOrEmpty(jsonString))
		{
			Service.Get<StaRTSLogger>().Debug("OnRestorePurchases: " + jsonString);
		}
	}

	private void RestoreAndroidPurchases(string jsonString)
	{
		if (!string.IsNullOrEmpty(jsonString))
		{
			IDictionary<string, object> dictionary = new JsonParser(jsonString).Parse() as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("purchases"))
			{
				List<object> list = dictionary.get_Item("purchases") as List<object>;
				for (int i = 0; i < list.Count; i++)
				{
					Serializer serializer = new Serializer();
					IDictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
					using (IEnumerator<KeyValuePair<string, object>> enumerator = dictionary2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, object> current = enumerator.get_Current();
							Service.Get<StaRTSLogger>().Debug(string.Concat(new object[]
							{
								"key: ",
								current.get_Key(),
								" value: ",
								current.get_Value()
							}));
							serializer.AddString(current.get_Key(), current.get_Value() as string);
						}
					}
					serializer.End();
					Service.Get<StaRTSLogger>().Debug("Restored Purcase:" + serializer.ToString());
					Service.Get<InAppPurchaseController>().OnPurchaseProductResponse(serializer.ToString(), true);
				}
			}
		}
	}

	private void RestoreIOSPurchases(string jsonString)
	{
		if (string.IsNullOrEmpty(jsonString))
		{
			return;
		}
		if (Service.IsSet<InAppPurchaseController>())
		{
			Service.Get<InAppPurchaseController>().OnPurchaseProductResponse(jsonString, true);
		}
	}

	public InAppPurchaseComponent()
	{
	}

	public override void Unity_Serialize(int depth)
	{
	}

	public override void Unity_Deserialize(int depth)
	{
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public override void Unity_NamedSerialize(int depth)
	{
	}

	public override void Unity_NamedDeserialize(int depth)
	{
	}

	protected internal InAppPurchaseComponent(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).OnBILog(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).onConsumePurchase(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).OnGetInfoForProducts(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).OnPurchaseProduct(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).OnRestorePurchases(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).RestoreAndroidPurchases(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).RestoreIOSPurchases(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((InAppPurchaseComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
