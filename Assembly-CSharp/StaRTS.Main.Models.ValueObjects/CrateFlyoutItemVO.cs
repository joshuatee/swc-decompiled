using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CrateFlyoutItemVO : IValueObject
	{
		public static int COLUMN_planet
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

		public static int COLUMN_listChanceString
		{
			get;
			private set;
		}

		public static int COLUMN_listDescString
		{
			get;
			private set;
		}

		public static int COLUMN_listIcons
		{
			get;
			private set;
		}

		public static int COLUMN_quantityString
		{
			get;
			private set;
		}

		public static int COLUMN_detailChanceString
		{
			get;
			private set;
		}

		public static int COLUMN_detailDescString
		{
			get;
			private set;
		}

		public static int COLUMN_crateSupplyUid
		{
			get;
			private set;
		}

		public static int COLUMN_detailTypeString
		{
			get;
			private set;
		}

		public static int COLUMN_tournamentTierDisplay3D
		{
			get;
			private set;
		}

		public static int COLUMN_showParametersList
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string[] PlanetIds
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

		public string ListChanceString
		{
			get;
			private set;
		}

		public string ListDescString
		{
			get;
			private set;
		}

		public string[] ListIcons
		{
			get;
			private set;
		}

		public string QuantityString
		{
			get;
			private set;
		}

		public string DetailChanceString
		{
			get;
			private set;
		}

		public string DetailDescString
		{
			get;
			private set;
		}

		public string CrateSupplyUid
		{
			get;
			private set;
		}

		public string DetailTypeStringId
		{
			get;
			private set;
		}

		public bool TournamentTierDisplay3D
		{
			get;
			private set;
		}

		protected string[] ShowParametersList
		{
			get;
			private set;
		}

		public List<CrateFlyoutDisplayType> ShowParams
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.PlanetIds = row.TryGetStringArray(CrateFlyoutItemVO.COLUMN_planet);
			this.MinHQ = row.TryGetInt(CrateFlyoutItemVO.COLUMN_minHQ);
			this.MaxHQ = row.TryGetInt(CrateFlyoutItemVO.COLUMN_maxHQ);
			this.ReqArmory = row.TryGetBool(CrateFlyoutItemVO.COLUMN_reqArmory);
			this.ListChanceString = row.TryGetString(CrateFlyoutItemVO.COLUMN_listChanceString);
			this.ListDescString = row.TryGetString(CrateFlyoutItemVO.COLUMN_listDescString);
			this.ListIcons = row.TryGetStringArray(CrateFlyoutItemVO.COLUMN_listIcons);
			this.QuantityString = row.TryGetString(CrateFlyoutItemVO.COLUMN_quantityString);
			this.DetailChanceString = row.TryGetString(CrateFlyoutItemVO.COLUMN_detailChanceString);
			this.DetailDescString = row.TryGetString(CrateFlyoutItemVO.COLUMN_detailDescString);
			this.CrateSupplyUid = row.TryGetString(CrateFlyoutItemVO.COLUMN_crateSupplyUid);
			this.DetailTypeStringId = row.TryGetString(CrateFlyoutItemVO.COLUMN_detailTypeString);
			this.TournamentTierDisplay3D = row.TryGetBool(CrateFlyoutItemVO.COLUMN_tournamentTierDisplay3D);
			this.ShowParametersList = row.TryGetStringArray(CrateFlyoutItemVO.COLUMN_showParametersList);
			if (this.ShowParametersList != null)
			{
				this.ShowParams = new List<CrateFlyoutDisplayType>();
				int i = 0;
				int num = this.ShowParametersList.Length;
				while (i < num)
				{
					CrateFlyoutDisplayType item = StringUtils.ParseEnum<CrateFlyoutDisplayType>(this.ShowParametersList[i]);
					this.ShowParams.Add(item);
					i++;
				}
			}
		}

		public CrateFlyoutItemVO()
		{
		}

		protected internal CrateFlyoutItemVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_crateSupplyUid);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_detailChanceString);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_detailDescString);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_detailTypeString);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_listChanceString);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_listDescString);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_listIcons);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_maxHQ);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_minHQ);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_planet);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_quantityString);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_reqArmory);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_showParametersList);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateFlyoutItemVO.COLUMN_tournamentTierDisplay3D);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).CrateSupplyUid);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).DetailChanceString);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).DetailDescString);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).DetailTypeStringId);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ListChanceString);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ListDescString);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ListIcons);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).MaxHQ);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).MinHQ);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).PlanetIds);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).QuantityString);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ReqArmory);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ShowParametersList);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ShowParams);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).TournamentTierDisplay3D);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_crateSupplyUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_detailChanceString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_detailDescString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_detailTypeString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_listChanceString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_listDescString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_listIcons = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_maxHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_minHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_planet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_quantityString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_reqArmory = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_showParametersList = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			CrateFlyoutItemVO.COLUMN_tournamentTierDisplay3D = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).CrateSupplyUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).DetailChanceString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).DetailDescString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).DetailTypeStringId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ListChanceString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ListDescString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ListIcons = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).MaxHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).MinHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).PlanetIds = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).QuantityString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ReqArmory = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ShowParametersList = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).ShowParams = (List<CrateFlyoutDisplayType>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).TournamentTierDisplay3D = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((CrateFlyoutItemVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
