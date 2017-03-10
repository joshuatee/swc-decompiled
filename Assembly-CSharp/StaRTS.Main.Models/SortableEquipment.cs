using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class SortableEquipment
	{
		private const int EFFECTIVE_MIN_INT = -900000;

		public CurrentPlayer Player
		{
			get;
			set;
		}

		public EquipmentVO Equipment
		{
			get;
			private set;
		}

		public int IncrementingIndex
		{
			get;
			set;
		}

		public int EmptyIndex
		{
			get;
			set;
		}

		public SortableEquipment(EquipmentVO equipment)
		{
			this.Equipment = equipment;
			if (this.Equipment == null)
			{
				this.IncrementingIndex = -900000;
				return;
			}
			this.EmptyIndex = -900000;
		}

		public SortableEquipment(CurrentPlayer player, EquipmentVO equipment)
		{
			this.Player = player;
			this.Equipment = equipment;
			this.EmptyIndex = -900000;
		}

		public SortableEquipment(EquipmentVO equipment, int index)
		{
			this.Equipment = equipment;
			this.IncrementingIndex = index;
			this.EmptyIndex = -900000;
		}

		public bool HasEquipment()
		{
			return this.Equipment != null;
		}

		protected internal SortableEquipment(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SortableEquipment)GCHandledObjects.GCHandleToObject(instance)).EmptyIndex);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SortableEquipment)GCHandledObjects.GCHandleToObject(instance)).Equipment);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SortableEquipment)GCHandledObjects.GCHandleToObject(instance)).IncrementingIndex);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SortableEquipment)GCHandledObjects.GCHandleToObject(instance)).Player);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SortableEquipment)GCHandledObjects.GCHandleToObject(instance)).HasEquipment());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SortableEquipment)GCHandledObjects.GCHandleToObject(instance)).EmptyIndex = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SortableEquipment)GCHandledObjects.GCHandleToObject(instance)).Equipment = (EquipmentVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SortableEquipment)GCHandledObjects.GCHandleToObject(instance)).IncrementingIndex = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SortableEquipment)GCHandledObjects.GCHandleToObject(instance)).Player = (CurrentPlayer)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
