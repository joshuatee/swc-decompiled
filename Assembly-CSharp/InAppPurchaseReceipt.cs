using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

public class InAppPurchaseReceipt
{
	public bool useSandbox;

	public uint errorCode;

	public string price;

	public string transactionId;

	public string rawData;

	public string productId;

	public string signature;

	public string userId;

	public static InAppPurchaseReceipt Parse(string value)
	{
		InAppPurchaseReceipt inAppPurchaseReceipt = new InAppPurchaseReceipt();
		IDictionary<string, object> dictionary = new JsonParser(value).Parse() as Dictionary<string, object>;
		if (dictionary.ContainsKey("userSandbox") && dictionary.get_Item("useSandbox") != null)
		{
			inAppPurchaseReceipt.useSandbox = (bool)dictionary.get_Item("useSandbox");
		}
		inAppPurchaseReceipt.errorCode = Convert.ToUInt32(dictionary.get_Item("errorCode") as string, CultureInfo.InvariantCulture);
		inAppPurchaseReceipt.price = (dictionary.get_Item("price") as string);
		inAppPurchaseReceipt.transactionId = (dictionary.get_Item("transactionId") as string);
		inAppPurchaseReceipt.rawData = (dictionary.get_Item("rawData") as string);
		inAppPurchaseReceipt.productId = (dictionary.get_Item("productId") as string);
		if (dictionary.ContainsKey("signature"))
		{
			inAppPurchaseReceipt.signature = (dictionary.get_Item("signature") as string);
		}
		return inAppPurchaseReceipt;
	}

	public string GetManimalReceiptString()
	{
		return "";
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("TransactionID: ");
		stringBuilder.Append(this.transactionId);
		stringBuilder.Append(": Error Code:");
		stringBuilder.Append(this.errorCode);
		stringBuilder.Append(": ProductID:");
		stringBuilder.Append(this.productId);
		stringBuilder.Append(": Price: ");
		stringBuilder.Append(this.price);
		stringBuilder.Append(": Receipt rawData: ");
		stringBuilder.Append(this.rawData);
		if (this.signature != null)
		{
			stringBuilder.Append(": Signature:");
			stringBuilder.Append(this.signature);
		}
		return stringBuilder.ToString();
	}

	public InAppPurchaseReceipt()
	{
	}

	protected internal InAppPurchaseReceipt(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseReceipt)GCHandledObjects.GCHandleToObject(instance)).GetManimalReceiptString());
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(InAppPurchaseReceipt.Parse(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseReceipt)GCHandledObjects.GCHandleToObject(instance)).ToString());
	}
}
