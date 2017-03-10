using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class RaidMissionPoolVO : IValueObject
	{
		public static int COLUMN_HQ
		{
			get;
			private set;
		}

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_campaignMissions
		{
			get;
			private set;
		}

		public static int COLUMN_crate1
		{
			get;
			private set;
		}

		public static int COLUMN_crate2
		{
			get;
			private set;
		}

		public static int COLUMN_crate3
		{
			get;
			private set;
		}

		public static int COLUMN_descCondition1
		{
			get;
			private set;
		}

		public static int COLUMN_descCondition2
		{
			get;
			private set;
		}

		public static int COLUMN_descCondition3
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string[] Missions
		{
			get;
			private set;
		}

		public FactionType Faction
		{
			get;
			private set;
		}

		public string Crate1StarRewardId
		{
			get;
			private set;
		}

		public string Crate2StarRewardId
		{
			get;
			private set;
		}

		public string Crate3StarRewardId
		{
			get;
			private set;
		}

		public string Condition1StarRewardStringId
		{
			get;
			private set;
		}

		public string Condition2StarRewardStringId
		{
			get;
			private set;
		}

		public string Condition3StarRewardStringId
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(RaidMissionPoolVO.COLUMN_faction));
			this.Missions = row.TryGetStringArray(RaidMissionPoolVO.COLUMN_campaignMissions);
			this.Crate1StarRewardId = row.TryGetString(RaidMissionPoolVO.COLUMN_crate1);
			this.Crate2StarRewardId = row.TryGetString(RaidMissionPoolVO.COLUMN_crate2);
			this.Crate3StarRewardId = row.TryGetString(RaidMissionPoolVO.COLUMN_crate3);
			this.Condition1StarRewardStringId = row.TryGetString(RaidMissionPoolVO.COLUMN_descCondition1);
			this.Condition2StarRewardStringId = row.TryGetString(RaidMissionPoolVO.COLUMN_descCondition2);
			this.Condition3StarRewardStringId = row.TryGetString(RaidMissionPoolVO.COLUMN_descCondition3);
		}

		public RaidMissionPoolVO()
		{
		}

		protected internal RaidMissionPoolVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidMissionPoolVO.COLUMN_campaignMissions);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidMissionPoolVO.COLUMN_crate1);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidMissionPoolVO.COLUMN_crate2);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidMissionPoolVO.COLUMN_crate3);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidMissionPoolVO.COLUMN_descCondition1);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidMissionPoolVO.COLUMN_descCondition2);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidMissionPoolVO.COLUMN_descCondition3);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidMissionPoolVO.COLUMN_faction);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidMissionPoolVO.COLUMN_HQ);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Condition1StarRewardStringId);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Condition2StarRewardStringId);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Condition3StarRewardStringId);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Crate1StarRewardId);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Crate2StarRewardId);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Crate3StarRewardId);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Missions);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			RaidMissionPoolVO.COLUMN_campaignMissions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			RaidMissionPoolVO.COLUMN_crate1 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			RaidMissionPoolVO.COLUMN_crate2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			RaidMissionPoolVO.COLUMN_crate3 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			RaidMissionPoolVO.COLUMN_descCondition1 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			RaidMissionPoolVO.COLUMN_descCondition2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			RaidMissionPoolVO.COLUMN_descCondition3 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			RaidMissionPoolVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			RaidMissionPoolVO.COLUMN_HQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Condition1StarRewardStringId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Condition2StarRewardStringId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Condition3StarRewardStringId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Crate1StarRewardId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Crate2StarRewardId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Crate3StarRewardId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Missions = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((RaidMissionPoolVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
