using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TournamentRewardsVO : IValueObject
	{
		public static int COLUMN_tournamentRewardsId
		{
			get;
			private set;
		}

		public static int COLUMN_tournamentTier
		{
			get;
			private set;
		}

		public static int COLUMN_crateRewardUid
		{
			get;
			private set;
		}

		public static int COLUMN_rebelGuaranteedReward
		{
			get;
			private set;
		}

		public static int COLUMN_empireGuaranteedReward
		{
			get;
			private set;
		}

		public static int COLUMN_rebelGuaranteedRewardLabel
		{
			get;
			private set;
		}

		public static int COLUMN_empireGuaranteedRewardLabel
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string TournamentRewardsId
		{
			get;
			set;
		}

		public string TournamentTier
		{
			get;
			set;
		}

		public string[] CrateRewardIds
		{
			get;
			set;
		}

		public string RebelGuaranteedReward
		{
			get;
			set;
		}

		public string EmpireGuaranteedReward
		{
			get;
			set;
		}

		public string RebelGuaranteedRewardLabel
		{
			get;
			set;
		}

		public string EmpireGuaranteedRewardlabel
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.TournamentRewardsId = row.TryGetString(TournamentRewardsVO.COLUMN_tournamentRewardsId);
			this.TournamentTier = row.TryGetString(TournamentRewardsVO.COLUMN_tournamentTier);
			this.CrateRewardIds = row.TryGetStringArray(TournamentRewardsVO.COLUMN_crateRewardUid);
			this.RebelGuaranteedReward = row.TryGetString(TournamentRewardsVO.COLUMN_rebelGuaranteedReward);
			this.EmpireGuaranteedReward = row.TryGetString(TournamentRewardsVO.COLUMN_empireGuaranteedReward);
			this.RebelGuaranteedRewardLabel = row.TryGetString(TournamentRewardsVO.COLUMN_rebelGuaranteedRewardLabel);
			this.EmpireGuaranteedRewardlabel = row.TryGetString(TournamentRewardsVO.COLUMN_empireGuaranteedRewardLabel);
		}

		public TournamentRewardsVO()
		{
		}

		protected internal TournamentRewardsVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentRewardsVO.COLUMN_crateRewardUid);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentRewardsVO.COLUMN_empireGuaranteedReward);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentRewardsVO.COLUMN_empireGuaranteedRewardLabel);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentRewardsVO.COLUMN_rebelGuaranteedReward);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentRewardsVO.COLUMN_rebelGuaranteedRewardLabel);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentRewardsVO.COLUMN_tournamentRewardsId);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentRewardsVO.COLUMN_tournamentTier);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).CrateRewardIds);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).EmpireGuaranteedReward);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).EmpireGuaranteedRewardlabel);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).RebelGuaranteedReward);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).RebelGuaranteedRewardLabel);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).TournamentRewardsId);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).TournamentTier);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			TournamentRewardsVO.COLUMN_crateRewardUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			TournamentRewardsVO.COLUMN_empireGuaranteedReward = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			TournamentRewardsVO.COLUMN_empireGuaranteedRewardLabel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			TournamentRewardsVO.COLUMN_rebelGuaranteedReward = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			TournamentRewardsVO.COLUMN_rebelGuaranteedRewardLabel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			TournamentRewardsVO.COLUMN_tournamentRewardsId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			TournamentRewardsVO.COLUMN_tournamentTier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).CrateRewardIds = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).EmpireGuaranteedReward = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).EmpireGuaranteedRewardlabel = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).RebelGuaranteedReward = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).RebelGuaranteedRewardLabel = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).TournamentRewardsId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).TournamentTier = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((TournamentRewardsVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
