using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.MetaData;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class LimitedEditionItemVO : IValueObject, IGeometryVO
	{
		public string[] AudienceConditions;

		public static int COLUMN_faction
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

		public static int COLUMN_credits
		{
			get;
			private set;
		}

		public static int COLUMN_materials
		{
			get;
			private set;
		}

		public static int COLUMN_contraband
		{
			get;
			private set;
		}

		public static int COLUMN_crystals
		{
			get;
			private set;
		}

		public static int COLUMN_crateId
		{
			get;
			private set;
		}

		public static int COLUMN_storeTab
		{
			get;
			private set;
		}

		public static int COLUMN_description
		{
			get;
			private set;
		}

		public static int COLUMN_iconAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_iconBundleName
		{
			get;
			private set;
		}

		public static int COLUMN_iconCameraPosition
		{
			get;
			private set;
		}

		public static int COLUMN_iconLookatPosition
		{
			get;
			private set;
		}

		public static int COLUMN_audienceConditions
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

		public int StartTime
		{
			get;
			set;
		}

		public int EndTime
		{
			get;
			set;
		}

		public string CrateId
		{
			get;
			set;
		}

		public int Credits
		{
			get;
			set;
		}

		public int Materials
		{
			get;
			set;
		}

		public int Contraband
		{
			get;
			set;
		}

		public int Crystals
		{
			get;
			set;
		}

		public StoreTab StoreTab
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public Vector3 IconCameraPosition
		{
			get;
			set;
		}

		public Vector3 IconLookatPosition
		{
			get;
			set;
		}

		public Vector3 IconCloseupCameraPosition
		{
			get;
			set;
		}

		public Vector3 IconCloseupLookatPosition
		{
			get;
			set;
		}

		public string IconBundleName
		{
			get;
			set;
		}

		public string IconAssetName
		{
			get;
			set;
		}

		public float IconRotationSpeed
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(LimitedEditionItemVO.COLUMN_faction));
			string text = row.TryGetString(LimitedEditionItemVO.COLUMN_startDate);
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					DateTime date = DateTime.ParseExact(text, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
					this.StartTime = DateUtils.GetSecondsFromEpoch(date);
					goto IL_9A;
				}
				catch (Exception)
				{
					this.StartTime = 0;
					Service.Get<StaRTSLogger>().Warn("LimitedEditionItemVO:: LEI CMS Start Date Format Error: " + this.Uid);
					goto IL_9A;
				}
			}
			this.StartTime = 0;
			Service.Get<StaRTSLogger>().Warn("LimitedEditionItemVO:: LEI CMS Start Date Not Specified For: " + this.Uid);
			IL_9A:
			string text2 = row.TryGetString(LimitedEditionItemVO.COLUMN_endDate);
			if (!string.IsNullOrEmpty(text2))
			{
				try
				{
					DateTime date2 = DateTime.ParseExact(text2, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
					this.EndTime = DateUtils.GetSecondsFromEpoch(date2);
					goto IL_100;
				}
				catch (Exception)
				{
					this.EndTime = 2147483647;
					Service.Get<StaRTSLogger>().Warn("LimitedEditionItemVO:: LEI CMS End Date Format Error: " + this.Uid);
					goto IL_100;
				}
			}
			this.EndTime = 2147483647;
			IL_100:
			this.CrateId = row.TryGetString(LimitedEditionItemVO.COLUMN_crateId, string.Empty);
			this.Credits = row.TryGetInt(LimitedEditionItemVO.COLUMN_credits, 0);
			this.Materials = row.TryGetInt(LimitedEditionItemVO.COLUMN_materials, 0);
			this.Contraband = row.TryGetInt(LimitedEditionItemVO.COLUMN_contraband, 0);
			this.Crystals = row.TryGetInt(LimitedEditionItemVO.COLUMN_crystals, 0);
			this.StoreTab = StringUtils.ParseEnum<StoreTab>(row.TryGetString(LimitedEditionItemVO.COLUMN_storeTab));
			this.Description = row.TryGetString(LimitedEditionItemVO.COLUMN_description, string.Empty);
			this.IconAssetName = row.TryGetString(LimitedEditionItemVO.COLUMN_iconAssetName, string.Empty);
			this.IconBundleName = row.TryGetString(LimitedEditionItemVO.COLUMN_iconBundleName, string.Empty);
			this.IconCameraPosition = row.TryGetVector3(LimitedEditionItemVO.COLUMN_iconCameraPosition, Vector3.one);
			this.IconLookatPosition = row.TryGetVector3(LimitedEditionItemVO.COLUMN_iconLookatPosition, Vector3.zero);
			this.IconCloseupCameraPosition = this.IconCameraPosition;
			this.IconCloseupLookatPosition = this.IconCloseupLookatPosition;
			this.IconRotationSpeed = 0f;
			this.AudienceConditions = row.TryGetStringArray(LimitedEditionItemVO.COLUMN_audienceConditions);
		}

		public LimitedEditionItemVO()
		{
		}

		protected internal LimitedEditionItemVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_audienceConditions);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_contraband);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_crateId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_credits);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_crystals);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_description);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_endDate);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_faction);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_materials);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_startDate);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemVO.COLUMN_storeTab);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Contraband);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).CrateId);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Credits);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Crystals);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).EndTime);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Materials);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).StartTime);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).StoreTab);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_audienceConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_crateId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_crystals = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_description = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_endDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_startDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			LimitedEditionItemVO.COLUMN_storeTab = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).CrateId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Crystals = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Description = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).EndTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).StartTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).StoreTab = (StoreTab)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
