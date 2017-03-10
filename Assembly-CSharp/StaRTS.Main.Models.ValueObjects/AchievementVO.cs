using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class AchievementVO : IValueObject
	{
		public static int COLUMN_rebel_data
		{
			get;
			private set;
		}

		public static int COLUMN_empire_data
		{
			get;
			private set;
		}

		public static int COLUMN_google_achievement_id
		{
			get;
			private set;
		}

		public static int COLUMN_ios_achievement_id
		{
			get;
			private set;
		}

		public static int COLUMN_amazon_achievement_id
		{
			get;
			private set;
		}

		public static int COLUMN_achievement_type_id
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public AchievementType AchievementType
		{
			get;
			private set;
		}

		public string RebelData
		{
			get;
			private set;
		}

		public string EmpireData
		{
			get;
			private set;
		}

		public string GoogleAchievementId
		{
			get;
			private set;
		}

		public string IosAchievementId
		{
			get;
			private set;
		}

		public string AmazonAchievementId
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.RebelData = row.TryGetString(AchievementVO.COLUMN_rebel_data);
			this.EmpireData = row.TryGetString(AchievementVO.COLUMN_empire_data);
			this.GoogleAchievementId = row.TryGetString(AchievementVO.COLUMN_google_achievement_id);
			this.IosAchievementId = row.TryGetString(AchievementVO.COLUMN_ios_achievement_id);
			this.AmazonAchievementId = row.TryGetString(AchievementVO.COLUMN_amazon_achievement_id);
			string name = row.TryGetString(AchievementVO.COLUMN_achievement_type_id);
			this.AchievementType = StringUtils.ParseEnum<AchievementType>(name);
		}

		public AchievementVO()
		{
		}

		protected internal AchievementVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).AchievementType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).AmazonAchievementId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AchievementVO.COLUMN_achievement_type_id);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AchievementVO.COLUMN_amazon_achievement_id);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AchievementVO.COLUMN_empire_data);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AchievementVO.COLUMN_google_achievement_id);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AchievementVO.COLUMN_ios_achievement_id);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AchievementVO.COLUMN_rebel_data);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).EmpireData);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).GoogleAchievementId);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).IosAchievementId);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).RebelData);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).AchievementType = (AchievementType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).AmazonAchievementId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			AchievementVO.COLUMN_achievement_type_id = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			AchievementVO.COLUMN_amazon_achievement_id = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			AchievementVO.COLUMN_empire_data = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			AchievementVO.COLUMN_google_achievement_id = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			AchievementVO.COLUMN_ios_achievement_id = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			AchievementVO.COLUMN_rebel_data = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).EmpireData = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).GoogleAchievementId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).IosAchievementId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).RebelData = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((AchievementVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
