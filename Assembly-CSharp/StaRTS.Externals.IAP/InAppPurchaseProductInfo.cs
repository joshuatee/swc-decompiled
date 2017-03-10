using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using WinRTBridge;

namespace StaRTS.Externals.IAP
{
	public class InAppPurchaseProductInfo
	{
		private const string APP_STORE_ID = "appStoreId";

		private const string CURRENCY_CODE = "currencyCode";

		private const string FORMATTED_REAL_COST = "formattedRealCost";

		private const string NAME = "name";

		private const string PRICE_LOCALE = "priceLocale";

		private const string REAL_COST = "realCost";

		public string AppStoreId
		{
			get;
			set;
		}

		public string FormattedRealCost
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string RealCost
		{
			get;
			set;
		}

		public double Cost
		{
			get;
			set;
		}

		public string PriceLocale
		{
			get;
			set;
		}

		public string CurrencyCode
		{
			get;
			set;
		}

		public static InAppPurchaseProductInfo Parse(object value)
		{
			InAppPurchaseProductInfo inAppPurchaseProductInfo = new InAppPurchaseProductInfo();
			IDictionary<string, object> dictionary = value as Dictionary<string, object>;
			inAppPurchaseProductInfo.FormattedRealCost = (dictionary.get_Item("formattedRealCost") as string);
			string text = dictionary.get_Item("realCost") as string;
			text = Regex.Replace(text, "[^\\s,.0-9]", "");
			inAppPurchaseProductInfo.RealCost = text;
			inAppPurchaseProductInfo.Name = (dictionary.get_Item("name") as string);
			inAppPurchaseProductInfo.AppStoreId = (dictionary.get_Item("appStoreId") as string);
			if (dictionary.ContainsKey("currencyCode"))
			{
				inAppPurchaseProductInfo.CurrencyCode = (dictionary.get_Item("currencyCode") as string);
			}
			return inAppPurchaseProductInfo;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(this.AppStoreId);
			stringBuilder.Append(":");
			stringBuilder.Append(this.Name);
			stringBuilder.Append(":");
			stringBuilder.Append(this.FormattedRealCost);
			stringBuilder.Append(":");
			stringBuilder.Append(this.RealCost);
			return stringBuilder.ToString();
		}

		public InAppPurchaseProductInfo()
		{
		}

		protected internal InAppPurchaseProductInfo(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).AppStoreId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).CurrencyCode);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).FormattedRealCost);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).Name);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).PriceLocale);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).RealCost);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseProductInfo.Parse(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).AppStoreId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).Cost = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).CurrencyCode = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).FormattedRealCost = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).Name = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).PriceLocale = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).RealCost = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(instance)).ToString());
		}
	}
}
