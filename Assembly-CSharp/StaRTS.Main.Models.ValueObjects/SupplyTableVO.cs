using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class SupplyTableVO : IValueObject
	{
		public static int COLUMN_crateTier
		{
			get;
			private set;
		}

		public static int COLUMN_faction
		{
			get;
			private set;
		}

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

		public static int COLUMN_supplyType
		{
			get;
			private set;
		}

		public static int COLUMN_item
		{
			get;
			private set;
		}

		public static int COLUMN_scalingUid
		{
			get;
			private set;
		}

		public static int COLUMN_weight
		{
			get;
			private set;
		}

		public static int COLUMN_amount
		{
			get;
			private set;
		}

		public static int COLUMN_dataCard
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string crateTier
		{
			get;
			set;
		}

		public string faction
		{
			get;
			set;
		}

		public string planet
		{
			get;
			set;
		}

		public int minHQ
		{
			get;
			set;
		}

		public int maxHQ
		{
			get;
			set;
		}

		public SupplyType supplyType
		{
			get;
			set;
		}

		public string item
		{
			get;
			set;
		}

		public string scalingUid
		{
			get;
			set;
		}

		public int weight
		{
			get;
			set;
		}

		public int amount
		{
			get;
			set;
		}

		public string dataCard
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.crateTier = row.TryGetString(SupplyTableVO.COLUMN_crateTier);
			this.faction = row.TryGetString(SupplyTableVO.COLUMN_faction);
			this.planet = row.TryGetString(SupplyTableVO.COLUMN_planet);
			this.minHQ = row.TryGetInt(SupplyTableVO.COLUMN_minHQ);
			this.maxHQ = row.TryGetInt(SupplyTableVO.COLUMN_maxHQ);
			this.supplyType = StringUtils.ParseEnum<SupplyType>(row.TryGetString(SupplyTableVO.COLUMN_supplyType));
			this.item = row.TryGetString(SupplyTableVO.COLUMN_item);
			this.scalingUid = row.TryGetString(SupplyTableVO.COLUMN_scalingUid);
			this.weight = row.TryGetInt(SupplyTableVO.COLUMN_weight);
			this.amount = row.TryGetInt(SupplyTableVO.COLUMN_amount);
			this.dataCard = row.TryGetString(SupplyTableVO.COLUMN_dataCard);
		}

		public SupplyTableVO()
		{
		}

		protected internal SupplyTableVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).amount);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_amount);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_crateTier);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_dataCard);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_faction);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_item);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_maxHQ);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_minHQ);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_planet);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_scalingUid);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_supplyType);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SupplyTableVO.COLUMN_weight);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).crateTier);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).dataCard);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).faction);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).item);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).maxHQ);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).minHQ);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).planet);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).scalingUid);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).supplyType);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).weight);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).amount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			SupplyTableVO.COLUMN_amount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			SupplyTableVO.COLUMN_crateTier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			SupplyTableVO.COLUMN_dataCard = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			SupplyTableVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			SupplyTableVO.COLUMN_item = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			SupplyTableVO.COLUMN_maxHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			SupplyTableVO.COLUMN_minHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			SupplyTableVO.COLUMN_planet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			SupplyTableVO.COLUMN_scalingUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			SupplyTableVO.COLUMN_supplyType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			SupplyTableVO.COLUMN_weight = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).crateTier = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).dataCard = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).faction = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).item = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).maxHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).minHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).planet = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).scalingUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).supplyType = (SupplyType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((SupplyTableVO)GCHandledObjects.GCHandleToObject(instance)).weight = *(int*)args;
			return -1L;
		}
	}
}
