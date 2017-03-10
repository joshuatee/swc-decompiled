using StaRTS.Main.Controllers;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CampaignVO : ITimedEventVO, IValueObject
	{
		private bool miniCampaign;

		private bool miniCampaignAlreadyCalculated;

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_title
		{
			get;
			private set;
		}

		public static int COLUMN_timed
		{
			get;
			private set;
		}

		public static int COLUMN_unlockOrder
		{
			get;
			private set;
		}

		public static int COLUMN_description
		{
			get;
			private set;
		}

		public static int COLUMN_bundleName
		{
			get;
			private set;
		}

		public static int COLUMN_assetName
		{
			get;
			private set;
		}

		public static int COLUMN_reward
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

		public static int COLUMN_introStory
		{
			get;
			private set;
		}

		public static int COLUMN_purchaseLimit
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public FactionType Faction
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public bool Timed
		{
			get;
			set;
		}

		public int UnlockOrder
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public string BundleName
		{
			get;
			set;
		}

		public string IntroStory
		{
			get;
			set;
		}

		public string Reward
		{
			get;
			set;
		}

		public int PurchaseLimit
		{
			get;
			set;
		}

		public int TotalMasteryStars
		{
			get;
			set;
		}

		public int TotalMissions
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

		public bool UseTimeZoneOffset
		{
			get
			{
				return true;
			}
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(CampaignVO.COLUMN_faction));
			this.Title = row.TryGetString(CampaignVO.COLUMN_title);
			this.Timed = row.TryGetBool(CampaignVO.COLUMN_timed);
			this.UnlockOrder = row.TryGetInt(CampaignVO.COLUMN_unlockOrder);
			this.Description = row.TryGetString(CampaignVO.COLUMN_description);
			this.BundleName = row.TryGetString(CampaignVO.COLUMN_bundleName);
			this.AssetName = row.TryGetString(CampaignVO.COLUMN_assetName);
			this.Reward = row.TryGetString(CampaignVO.COLUMN_reward);
			string dateString = row.TryGetString(CampaignVO.COLUMN_startDate);
			string dateString2 = row.TryGetString(CampaignVO.COLUMN_endDate);
			this.IntroStory = row.TryGetString(CampaignVO.COLUMN_introStory);
			this.PurchaseLimit = row.TryGetInt(CampaignVO.COLUMN_purchaseLimit);
			this.TotalMasteryStars = 0;
			this.TotalMissions = 0;
			if (this.Timed)
			{
				this.StartTimestamp = TimedEventUtils.GetTimestamp(this.Uid, dateString);
				this.EndTimestamp = TimedEventUtils.GetTimestamp(this.Uid, dateString2);
			}
		}

		public int GetUpcomingDurationSeconds()
		{
			return GameConstants.CAMPAIGN_HOURS_UPCOMING * 3600;
		}

		public int GetClosingDurationSeconds()
		{
			return GameConstants.CAMPAIGN_HOURS_CLOSING * 3600;
		}

		public bool IsMiniCampaign()
		{
			if (this.miniCampaignAlreadyCalculated)
			{
				return this.miniCampaign;
			}
			IDataController dataController = Service.Get<IDataController>();
			this.miniCampaign = true;
			foreach (CampaignMissionVO current in dataController.GetAll<CampaignMissionVO>())
			{
				if (current.CampaignUid == this.Uid && current.UnlockOrder > 1)
				{
					this.miniCampaign = false;
					break;
				}
			}
			this.miniCampaignAlreadyCalculated = true;
			return this.miniCampaign;
		}

		public CampaignVO()
		{
		}

		protected internal CampaignVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_description);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_endDate);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_faction);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_introStory);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_purchaseLimit);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_reward);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_startDate);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_timed);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_title);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignVO.COLUMN_unlockOrder);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).IntroStory);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).PurchaseLimit);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Reward);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Timed);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Title);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).TotalMasteryStars);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).TotalMissions);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).UnlockOrder);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).UseTimeZoneOffset);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).GetClosingDurationSeconds());
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).GetUpcomingDurationSeconds());
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).IsMiniCampaign());
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			CampaignVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			CampaignVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			CampaignVO.COLUMN_description = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			CampaignVO.COLUMN_endDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			CampaignVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			CampaignVO.COLUMN_introStory = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			CampaignVO.COLUMN_purchaseLimit = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			CampaignVO.COLUMN_reward = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			CampaignVO.COLUMN_startDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			CampaignVO.COLUMN_timed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			CampaignVO.COLUMN_title = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			CampaignVO.COLUMN_unlockOrder = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Description = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).IntroStory = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).PurchaseLimit = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Reward = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Timed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Title = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).TotalMasteryStars = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).TotalMissions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((CampaignVO)GCHandledObjects.GCHandleToObject(instance)).UnlockOrder = *(int*)args;
			return -1L;
		}
	}
}
