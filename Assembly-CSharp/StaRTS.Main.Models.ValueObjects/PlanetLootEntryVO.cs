using StaRTS.Main.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class PlanetLootEntryVO : IValueObject
	{
		public static int COLUMN_supplyDataUid
		{
			get;
			private set;
		}

		public static int COLUMN_minHQ
		{
			get;
			private set;
		}

		public static int COLUMN_maxHQ
		{
			get;
			private set;
		}

		public static int COLUMN_reqArmory
		{
			get;
			private set;
		}

		public static int COLUMN_badge
		{
			get;
			private set;
		}

		public static int COLUMN_showDate
		{
			get;
			private set;
		}

		public static int COLUMN_hideDate
		{
			get;
			private set;
		}

		public static int COLUMN_featureAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_featureAssetBundle
		{
			get;
			private set;
		}

		public static int COLUMN_notesString
		{
			get;
			private set;
		}

		public static int COLUMN_typeString
		{
			get;
			private set;
		}

		public string SupplyDataUid
		{
			get;
			private set;
		}

		public int MinHQ
		{
			get;
			private set;
		}

		public int MaxHQ
		{
			get;
			private set;
		}

		public bool ReqArmory
		{
			get;
			private set;
		}

		public bool Badge
		{
			get;
			private set;
		}

		public int ShowDateTimeStamp
		{
			get;
			private set;
		}

		public int HideDateTimeStamp
		{
			get;
			private set;
		}

		public string FeatureAssetName
		{
			get;
			private set;
		}

		public string FeatureAssetBundle
		{
			get;
			private set;
		}

		public string NotesString
		{
			get;
			private set;
		}

		public string TypeStringID
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.SupplyDataUid = row.TryGetString(PlanetLootEntryVO.COLUMN_supplyDataUid);
			this.MinHQ = row.TryGetInt(PlanetLootEntryVO.COLUMN_minHQ);
			this.MaxHQ = row.TryGetInt(PlanetLootEntryVO.COLUMN_maxHQ);
			this.ReqArmory = row.TryGetBool(PlanetLootEntryVO.COLUMN_reqArmory);
			this.Badge = row.TryGetBool(PlanetLootEntryVO.COLUMN_badge);
			string text = row.TryGetString(PlanetLootEntryVO.COLUMN_showDate);
			string text2 = row.TryGetString(PlanetLootEntryVO.COLUMN_hideDate);
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowDateTimeStamp = TimedEventUtils.GetTimestamp(this.Uid, text);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this.HideDateTimeStamp = TimedEventUtils.GetTimestamp(this.Uid, text2);
			}
			this.FeatureAssetName = row.TryGetString(PlanetLootEntryVO.COLUMN_featureAssetName);
			this.FeatureAssetBundle = row.TryGetString(PlanetLootEntryVO.COLUMN_featureAssetBundle);
			this.NotesString = row.TryGetString(PlanetLootEntryVO.COLUMN_notesString);
			this.TypeStringID = row.TryGetString(PlanetLootEntryVO.COLUMN_typeString);
		}

		public PlanetLootEntryVO()
		{
		}

		protected internal PlanetLootEntryVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).Badge);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_badge);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_featureAssetBundle);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_featureAssetName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_hideDate);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_maxHQ);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_minHQ);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_notesString);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_reqArmory);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_showDate);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_supplyDataUid);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootEntryVO.COLUMN_typeString);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).FeatureAssetBundle);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).FeatureAssetName);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).HideDateTimeStamp);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).MaxHQ);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).MinHQ);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).NotesString);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).ReqArmory);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).ShowDateTimeStamp);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).SupplyDataUid);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).TypeStringID);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).Badge = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_badge = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_featureAssetBundle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_featureAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_hideDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_maxHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_minHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_notesString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_reqArmory = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_showDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_supplyDataUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			PlanetLootEntryVO.COLUMN_typeString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).FeatureAssetBundle = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).FeatureAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).HideDateTimeStamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).MaxHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).MinHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).NotesString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).ReqArmory = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).ShowDateTimeStamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).SupplyDataUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).TypeStringID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
