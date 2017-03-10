using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class InAppPurchaseTypeVO : IValueObject
	{
		public const string TYPE_AMAZON = "am";

		public const string TYPE_ANDROID = "a";

		public const string TYPE_IOS = "i";

		public const string TYPE_METRO = "ws";

		public const string TYPE_WP8 = "wp";

		public const string CURRENCY_TYPE_HARD = "hard";

		public const string CURRENCY_TYPE_UNIT = "unit";

		public const string CURRENCY_TYPE_PACK = "pack";

		public static int COLUMN_productId
		{
			get;
			private set;
		}

		public static int COLUMN_reward_empire
		{
			get;
			private set;
		}

		public static int COLUMN_reward_rebel
		{
			get;
			private set;
		}

		public static int COLUMN_type
		{
			get;
			private set;
		}

		public static int COLUMN_currencyType
		{
			get;
			private set;
		}

		public static int COLUMN_isPromo
		{
			get;
			private set;
		}

		public static int COLUMN_amount
		{
			get;
			private set;
		}

		public static int COLUMN_price
		{
			get;
			private set;
		}

		public static int COLUMN_order
		{
			get;
			private set;
		}

		public static int COLUMN_assetName
		{
			get;
			private set;
		}

		public static int COLUMN_assetName_empire
		{
			get;
			private set;
		}

		public static int COLUMN_assetName_rebel
		{
			get;
			private set;
		}

		public static int COLUMN_topBadgeString
		{
			get;
			private set;
		}

		public static int COLUMN_bottomBadgeString
		{
			get;
			private set;
		}

		public static int COLUMN_redemptionString_empire
		{
			get;
			private set;
		}

		public static int COLUMN_redemptionString_rebel
		{
			get;
			private set;
		}

		public static int COLUMN_hideFromStore
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string ProductId
		{
			get;
			set;
		}

		public string RewardEmpire
		{
			get;
			set;
		}

		public string RewardRebel
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public string CurrencyType
		{
			get;
			set;
		}

		public bool IsPromo
		{
			get;
			set;
		}

		public int Amount
		{
			get;
			set;
		}

		public float Price
		{
			get;
			set;
		}

		public int Order
		{
			get;
			set;
		}

		public string CurrencyIconId
		{
			get;
			set;
		}

		public string TopBadgeString
		{
			get;
			set;
		}

		public string BottomBadgeString
		{
			get;
			set;
		}

		public string RedemptionStringEmpire
		{
			get;
			set;
		}

		public string RedemptionStringRebel
		{
			get;
			set;
		}

		public bool HideFromStore
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.ProductId = row.TryGetString(InAppPurchaseTypeVO.COLUMN_productId);
			this.RewardEmpire = row.TryGetString(InAppPurchaseTypeVO.COLUMN_reward_empire);
			this.RewardRebel = row.TryGetString(InAppPurchaseTypeVO.COLUMN_reward_rebel);
			this.Type = row.TryGetString(InAppPurchaseTypeVO.COLUMN_type);
			this.CurrencyType = row.TryGetString(InAppPurchaseTypeVO.COLUMN_currencyType);
			this.IsPromo = row.TryGetBool(InAppPurchaseTypeVO.COLUMN_isPromo);
			this.Amount = row.TryGetInt(InAppPurchaseTypeVO.COLUMN_amount);
			this.Price = row.TryGetFloat(InAppPurchaseTypeVO.COLUMN_price);
			this.Order = row.TryGetInt(InAppPurchaseTypeVO.COLUMN_order);
			this.CurrencyIconId = row.TryGetString(InAppPurchaseTypeVO.COLUMN_assetName);
			this.TopBadgeString = row.TryGetString(InAppPurchaseTypeVO.COLUMN_topBadgeString);
			this.BottomBadgeString = row.TryGetString(InAppPurchaseTypeVO.COLUMN_bottomBadgeString);
			this.RedemptionStringEmpire = row.TryGetString(InAppPurchaseTypeVO.COLUMN_redemptionString_empire);
			this.RedemptionStringRebel = row.TryGetString(InAppPurchaseTypeVO.COLUMN_redemptionString_rebel);
			this.HideFromStore = row.TryGetBool(InAppPurchaseTypeVO.COLUMN_hideFromStore);
		}

		public InAppPurchaseTypeVO()
		{
		}

		protected internal InAppPurchaseTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Amount);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).BottomBadgeString);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_amount);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_assetName_empire);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_assetName_rebel);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_bottomBadgeString);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_currencyType);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_hideFromStore);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_isPromo);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_order);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_price);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_productId);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_redemptionString_empire);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_redemptionString_rebel);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_reward_empire);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_reward_rebel);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_topBadgeString);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(InAppPurchaseTypeVO.COLUMN_type);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyIconId);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyType);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).HideFromStore);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsPromo);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Order);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Price);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProductId);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).RedemptionStringEmpire);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).RedemptionStringRebel);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).RewardEmpire);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).RewardRebel);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).TopBadgeString);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Amount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).BottomBadgeString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_amount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_assetName_empire = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_assetName_rebel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_bottomBadgeString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_currencyType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_hideFromStore = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_isPromo = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_price = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_productId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_redemptionString_empire = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_redemptionString_rebel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_reward_empire = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_reward_rebel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_topBadgeString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			InAppPurchaseTypeVO.COLUMN_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyIconId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).HideFromStore = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsPromo = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Price = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProductId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).RedemptionStringEmpire = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).RedemptionStringRebel = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).RewardEmpire = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).RewardRebel = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).TopBadgeString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Type = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
