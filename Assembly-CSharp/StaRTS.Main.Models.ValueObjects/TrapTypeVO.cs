using StaRTS.Main.Controllers.TrapConditions;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TrapTypeVO : IValueObject
	{
		public TrapEventType EventType;

		public string TargetType;

		public string TriggerConditions;

		public string RevealAudio;

		public string DisarmConditions;

		public List<AddOnMapping> AddOns;

		public int RearmCreditsCost;

		public int RearmMaterialsCost;

		public int RearmContrabandCost;

		public List<TrapCondition> ParsedTrapConditions;

		public TurretTrapEventData TurretTED;

		public SpecialAttackTrapEventData ShipTED;

		private ITrapEventData eventData;

		public static int COLUMN_eventType
		{
			get;
			private set;
		}

		public static int COLUMN_eventData
		{
			get;
			private set;
		}

		public static int COLUMN_targetType
		{
			get;
			private set;
		}

		public static int COLUMN_triggerConditions
		{
			get;
			private set;
		}

		public static int COLUMN_revealAudio
		{
			get;
			private set;
		}

		public static int COLUMN_disarmConditions
		{
			get;
			private set;
		}

		public static int COLUMN_addOns
		{
			get;
			private set;
		}

		public static int COLUMN_rearmCreditsCost
		{
			get;
			private set;
		}

		public static int COLUMN_rearmMaterialsCost
		{
			get;
			private set;
		}

		public static int COLUMN_rearmContrabandCost
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
			this.EventType = StringUtils.ParseEnum<TrapEventType>(row.TryGetString(TrapTypeVO.COLUMN_eventType));
			this.SetEventData(TrapUtils.ParseEventData(this.EventType, row.TryGetString(TrapTypeVO.COLUMN_eventData)));
			this.TargetType = row.TryGetString(TrapTypeVO.COLUMN_targetType);
			this.TriggerConditions = row.TryGetString(TrapTypeVO.COLUMN_triggerConditions);
			this.RevealAudio = row.TryGetString(TrapTypeVO.COLUMN_revealAudio);
			this.DisarmConditions = row.TryGetString(TrapTypeVO.COLUMN_disarmConditions);
			this.AddOns = TrapUtils.ParseAddons(row.TryGetString(TrapTypeVO.COLUMN_addOns));
			this.RearmCreditsCost = row.TryGetInt(TrapTypeVO.COLUMN_rearmCreditsCost);
			this.RearmMaterialsCost = row.TryGetInt(TrapTypeVO.COLUMN_rearmMaterialsCost);
			this.RearmContrabandCost = row.TryGetInt(TrapTypeVO.COLUMN_rearmContrabandCost);
			this.ParsedTrapConditions = TrapUtils.ParseConditions(this.TriggerConditions);
		}

		private void SetEventData(ITrapEventData ted)
		{
			this.eventData = ted;
			if (this.eventData is TurretTrapEventData)
			{
				this.TurretTED = (TurretTrapEventData)this.eventData;
				this.ShipTED = null;
				return;
			}
			if (this.eventData is SpecialAttackTrapEventData)
			{
				this.TurretTED = null;
				this.ShipTED = (SpecialAttackTrapEventData)this.eventData;
				return;
			}
			this.TurretTED = null;
			this.ShipTED = null;
		}

		public TrapTypeVO()
		{
		}

		protected internal TrapTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_addOns);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_disarmConditions);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_eventData);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_eventType);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_rearmContrabandCost);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_rearmCreditsCost);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_rearmMaterialsCost);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_revealAudio);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_targetType);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TrapTypeVO.COLUMN_triggerConditions);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TrapTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			TrapTypeVO.COLUMN_addOns = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			TrapTypeVO.COLUMN_disarmConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			TrapTypeVO.COLUMN_eventData = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			TrapTypeVO.COLUMN_eventType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			TrapTypeVO.COLUMN_rearmContrabandCost = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			TrapTypeVO.COLUMN_rearmCreditsCost = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			TrapTypeVO.COLUMN_rearmMaterialsCost = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			TrapTypeVO.COLUMN_revealAudio = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			TrapTypeVO.COLUMN_targetType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			TrapTypeVO.COLUMN_triggerConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((TrapTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((TrapTypeVO)GCHandledObjects.GCHandleToObject(instance)).SetEventData((ITrapEventData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
