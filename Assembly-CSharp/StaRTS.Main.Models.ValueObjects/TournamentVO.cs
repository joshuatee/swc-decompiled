using StaRTS.Main.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TournamentVO : ITimedEventVO, IValueObject
	{
		public static int COLUMN_planetId
		{
			get;
			private set;
		}

		public static int COLUMN_startDate
		{
			get;
			private set;
		}

		public static int COLUMN_endDate
		{
			get;
			private set;
		}

		public static int COLUMN_backgroundTextureName
		{
			get;
			private set;
		}

		public static int COLUMN_startingRating
		{
			get;
			private set;
		}

		public static int COLUMN_rewardGroupId
		{
			get;
			private set;
		}

		public static int COLUMN_rewardBanner
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string PlanetId
		{
			get;
			set;
		}

		public int StartTimestamp
		{
			get;
			set;
		}

		public int EndTimestamp
		{
			get;
			set;
		}

		public string BackgroundTextureName
		{
			get;
			set;
		}

		public int StartingRating
		{
			get;
			set;
		}

		public string RewardGroupId
		{
			get;
			set;
		}

		public int RewardBanner
		{
			get;
			set;
		}

		public bool UseTimeZoneOffset
		{
			get
			{
				return false;
			}
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.PlanetId = row.TryGetString(TournamentVO.COLUMN_planetId);
			string dateString = row.TryGetString(TournamentVO.COLUMN_startDate);
			string dateString2 = row.TryGetString(TournamentVO.COLUMN_endDate);
			this.BackgroundTextureName = row.TryGetString(TournamentVO.COLUMN_backgroundTextureName);
			this.StartingRating = row.TryGetInt(TournamentVO.COLUMN_startingRating);
			this.RewardGroupId = row.TryGetString(TournamentVO.COLUMN_rewardGroupId);
			this.RewardBanner = row.TryGetInt(TournamentVO.COLUMN_rewardBanner);
			this.StartTimestamp = TimedEventUtils.GetTimestamp(this.Uid, dateString);
			this.EndTimestamp = TimedEventUtils.GetTimestamp(this.Uid, dateString2);
		}

		public int GetUpcomingDurationSeconds()
		{
			return GameConstants.TOURNAMENT_HOURS_UPCOMING * 3600;
		}

		public int GetClosingDurationSeconds()
		{
			return GameConstants.TOURNAMENT_HOURS_CLOSING * 3600;
		}

		public TournamentVO()
		{
		}

		protected internal TournamentVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).BackgroundTextureName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentVO.COLUMN_backgroundTextureName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentVO.COLUMN_endDate);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentVO.COLUMN_planetId);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentVO.COLUMN_rewardBanner);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentVO.COLUMN_rewardGroupId);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentVO.COLUMN_startDate);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentVO.COLUMN_startingRating);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).RewardBanner);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).RewardGroupId);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).StartingRating);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).UseTimeZoneOffset);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).GetClosingDurationSeconds());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).GetUpcomingDurationSeconds());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).BackgroundTextureName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			TournamentVO.COLUMN_backgroundTextureName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			TournamentVO.COLUMN_endDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			TournamentVO.COLUMN_planetId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			TournamentVO.COLUMN_rewardBanner = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			TournamentVO.COLUMN_rewardGroupId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			TournamentVO.COLUMN_startDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			TournamentVO.COLUMN_startingRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).RewardBanner = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).RewardGroupId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).StartingRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((TournamentVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
