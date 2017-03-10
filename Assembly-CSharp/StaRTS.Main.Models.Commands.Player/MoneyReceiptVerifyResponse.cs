using StaRTS.Externals.IAP;
using StaRTS.Externals.Kochava;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class MoneyReceiptVerifyResponse : AbstractResponse
	{
		private CrateData crateDataTO;

		public object Result
		{
			get;
			protected set;
		}

		public uint Status
		{
			get;
			set;
		}

		public List<Data> DataList
		{
			get;
			protected set;
		}

		public string TransactionId
		{
			get;
			set;
		}

		public override ISerializable FromObject(object obj)
		{
			bool flag = true;
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			Dictionary<string, object> dictionary2 = dictionary["iap"] as Dictionary<string, object>;
			Dictionary<string, object> dictionary3 = null;
			bool isPromo = (bool)dictionary["isPromo"];
			string currencyCode = "USD";
			double price = 0.0;
			string uid = "";
			double num = 1.0;
			string offerUid = null;
			if (dictionary.ContainsKey("sale"))
			{
				dictionary3 = (dictionary["sale"] as Dictionary<string, object>);
			}
			if (dictionary3 != null && dictionary3.ContainsKey("bonusMultiplier"))
			{
				num = Convert.ToDouble(dictionary3["bonusMultiplier"], CultureInfo.InvariantCulture);
				if (flag)
				{
					Service.Get<StaRTSLogger>().Debug("MoneyReceiptVerifyResponse: Bonus Multiplier: " + num);
				}
			}
			if (dictionary2.ContainsKey("uid"))
			{
				uid = (dictionary2["uid"] as string);
			}
			if (dictionary2.ContainsKey("price"))
			{
				price = Convert.ToDouble(dictionary2["price"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("targetedOffer"))
			{
				offerUid = (dictionary["targetedOffer"] as string);
			}
			if (dictionary.ContainsKey("crateData"))
			{
				this.crateDataTO = new CrateData();
				this.crateDataTO.FromObject(dictionary["crateData"]);
			}
			KochavaPlugin.FireEvent("paymentAction", "1");
			KochavaPlugin.FireEvent("revenueAmount", price.ToString());
			if (this.Status == 0u)
			{
				Service.Get<InAppPurchaseController>().HandleReceiptVerificationResponse(uid, this.TransactionId, currencyCode, price, num, isPromo, offerUid, this.crateDataTO);
			}
			return this;
		}

		public MoneyReceiptVerifyResponse()
		{
		}

		protected internal MoneyReceiptVerifyResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyResponse)GCHandledObjects.GCHandleToObject(instance)).DataList);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyResponse)GCHandledObjects.GCHandleToObject(instance)).Result);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyResponse)GCHandledObjects.GCHandleToObject(instance)).TransactionId);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((MoneyReceiptVerifyResponse)GCHandledObjects.GCHandleToObject(instance)).DataList = (List<Data>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((MoneyReceiptVerifyResponse)GCHandledObjects.GCHandleToObject(instance)).Result = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((MoneyReceiptVerifyResponse)GCHandledObjects.GCHandleToObject(instance)).TransactionId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
