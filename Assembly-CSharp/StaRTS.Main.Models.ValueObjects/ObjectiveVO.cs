using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class ObjectiveVO : IValueObject
	{
		public static int COLUMN_testSet
		{
			get;
			private set;
		}

		public static int COLUMN_objBucket
		{
			get;
			private set;
		}

		public static int COLUMN_faction
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

		public static int COLUMN_type
		{
			get;
			private set;
		}

		public static int COLUMN_item
		{
			get;
			private set;
		}

		public static int COLUMN_objString
		{
			get;
			private set;
		}

		public static int COLUMN_hq4
		{
			get;
			private set;
		}

		public static int COLUMN_hq5
		{
			get;
			private set;
		}

		public static int COLUMN_hq6
		{
			get;
			private set;
		}

		public static int COLUMN_hq7
		{
			get;
			private set;
		}

		public static int COLUMN_hq8
		{
			get;
			private set;
		}

		public static int COLUMN_hq9
		{
			get;
			private set;
		}

		public static int COLUMN_hq10
		{
			get;
			private set;
		}

		public static int COLUMN_crateRewardUid
		{
			get;
			private set;
		}

		public static int COLUMN_allowPvE
		{
			get;
			private set;
		}

		public static int COLUMN_weight
		{
			get;
			private set;
		}

		public static int COLUMN_objIcon
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public int TestSet
		{
			get;
			set;
		}

		public string ObjectiveBucket
		{
			get;
			set;
		}

		public FactionType faction
		{
			get;
			set;
		}

		public int MinHQ
		{
			get;
			set;
		}

		public int MaxHQ
		{
			get;
			set;
		}

		public ObjectiveType ObjectiveType
		{
			get;
			set;
		}

		public string Item
		{
			get;
			set;
		}

		public string ObjString
		{
			get;
			set;
		}

		public int HQ4
		{
			get;
			set;
		}

		public int HQ5
		{
			get;
			set;
		}

		public int HQ6
		{
			get;
			set;
		}

		public int HQ7
		{
			get;
			set;
		}

		public int HQ8
		{
			get;
			set;
		}

		public int HQ9
		{
			get;
			set;
		}

		public int HQ10
		{
			get;
			set;
		}

		public string CrateRewardUid
		{
			get;
			set;
		}

		public bool AllowPvE
		{
			get;
			set;
		}

		public int Weight
		{
			get;
			set;
		}

		public string ObjIcon
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.TestSet = row.TryGetInt(ObjectiveVO.COLUMN_testSet);
			this.ObjectiveBucket = row.TryGetString(ObjectiveVO.COLUMN_objBucket);
			this.faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(ObjectiveVO.COLUMN_faction));
			this.MinHQ = row.TryGetInt(ObjectiveVO.COLUMN_minHQ, -1);
			this.MaxHQ = row.TryGetInt(ObjectiveVO.COLUMN_maxHQ, -1);
			this.ObjectiveType = StringUtils.ParseEnum<ObjectiveType>(row.TryGetString(ObjectiveVO.COLUMN_type));
			this.Item = row.TryGetString(ObjectiveVO.COLUMN_item);
			this.ObjString = row.TryGetString(ObjectiveVO.COLUMN_objString);
			this.HQ4 = row.TryGetInt(ObjectiveVO.COLUMN_hq4);
			this.HQ5 = row.TryGetInt(ObjectiveVO.COLUMN_hq5);
			this.HQ6 = row.TryGetInt(ObjectiveVO.COLUMN_hq6);
			this.HQ7 = row.TryGetInt(ObjectiveVO.COLUMN_hq7);
			this.HQ8 = row.TryGetInt(ObjectiveVO.COLUMN_hq8);
			this.HQ9 = row.TryGetInt(ObjectiveVO.COLUMN_hq9);
			this.HQ10 = row.TryGetInt(ObjectiveVO.COLUMN_hq10);
			this.CrateRewardUid = row.TryGetString(ObjectiveVO.COLUMN_crateRewardUid);
			this.AllowPvE = row.TryGetBool(ObjectiveVO.COLUMN_allowPvE);
			this.Weight = row.TryGetInt(ObjectiveVO.COLUMN_weight, 0);
			this.ObjIcon = row.TryGetString(ObjectiveVO.COLUMN_objIcon);
		}

		public ObjectiveVO()
		{
		}

		protected internal ObjectiveVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).AllowPvE);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_allowPvE);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_crateRewardUid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_faction);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_hq10);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_hq4);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_hq5);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_hq6);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_hq7);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_hq8);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_hq9);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_item);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_maxHQ);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_minHQ);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_objBucket);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_objIcon);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_objString);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_testSet);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_type);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveVO.COLUMN_weight);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).CrateRewardUid);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).faction);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ10);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ4);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ5);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ6);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ7);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ8);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ9);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).Item);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).MaxHQ);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).MinHQ);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).ObjectiveBucket);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).ObjectiveType);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).ObjIcon);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).ObjString);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).TestSet);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).Weight);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).AllowPvE = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			ObjectiveVO.COLUMN_allowPvE = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			ObjectiveVO.COLUMN_crateRewardUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			ObjectiveVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			ObjectiveVO.COLUMN_hq10 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			ObjectiveVO.COLUMN_hq4 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			ObjectiveVO.COLUMN_hq5 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			ObjectiveVO.COLUMN_hq6 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			ObjectiveVO.COLUMN_hq7 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			ObjectiveVO.COLUMN_hq8 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			ObjectiveVO.COLUMN_hq9 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			ObjectiveVO.COLUMN_item = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			ObjectiveVO.COLUMN_maxHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			ObjectiveVO.COLUMN_minHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			ObjectiveVO.COLUMN_objBucket = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			ObjectiveVO.COLUMN_objIcon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			ObjectiveVO.COLUMN_objString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			ObjectiveVO.COLUMN_testSet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			ObjectiveVO.COLUMN_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			ObjectiveVO.COLUMN_weight = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).CrateRewardUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ10 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ4 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ5 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ6 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ7 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ8 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).HQ9 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).Item = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).MaxHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).MinHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).ObjectiveBucket = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).ObjectiveType = (ObjectiveType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).ObjIcon = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).ObjString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).TestSet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((ObjectiveVO)GCHandledObjects.GCHandleToObject(instance)).Weight = *(int*)args;
			return -1L;
		}
	}
}
