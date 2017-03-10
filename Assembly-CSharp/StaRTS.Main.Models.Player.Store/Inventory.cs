using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Store
{
	public class Inventory : InventoryStorage
	{
		private const string PLAYER_INVENTORY = "playerInventory";

		public InventoryStorage Troop
		{
			get;
			set;
		}

		public InventoryStorage SpecialAttack
		{
			get;
			set;
		}

		public InventoryStorage Hero
		{
			get;
			set;
		}

		public InventoryStorage Champion
		{
			get;
			set;
		}

		public Inventory() : base("playerInventory", EventId.InventoryResourceUpdated, null)
		{
			this.Troop = base.CreateSubstorage("troop", EventId.InventoryTroopUpdated, typeof(TroopTypeVO));
			this.SpecialAttack = base.CreateSubstorage("specialAttack", EventId.InventorySpecialAttackUpdated, typeof(SpecialAttackTypeVO));
			this.Hero = base.CreateSubstorage("hero", EventId.InventoryHeroUpdated, typeof(TroopTypeVO));
			this.Champion = base.CreateSubstorage("champion", EventId.InventoryChampionUpdated, typeof(TroopTypeVO));
		}

		protected internal Inventory(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Inventory)GCHandledObjects.GCHandleToObject(instance)).Champion);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Inventory)GCHandledObjects.GCHandleToObject(instance)).Hero);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Inventory)GCHandledObjects.GCHandleToObject(instance)).SpecialAttack);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Inventory)GCHandledObjects.GCHandleToObject(instance)).Troop);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Inventory)GCHandledObjects.GCHandleToObject(instance)).Champion = (InventoryStorage)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Inventory)GCHandledObjects.GCHandleToObject(instance)).Hero = (InventoryStorage)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((Inventory)GCHandledObjects.GCHandleToObject(instance)).SpecialAttack = (InventoryStorage)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((Inventory)GCHandledObjects.GCHandleToObject(instance)).Troop = (InventoryStorage)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
