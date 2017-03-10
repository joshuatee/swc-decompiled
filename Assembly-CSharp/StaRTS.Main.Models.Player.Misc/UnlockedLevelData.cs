using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public class UnlockedLevelData : ISerializable
	{
		public LevelMap Troops;

		public LevelMap Starships;

		public LevelMap Buildings;

		public LevelMap Equipment;

		public UnlockedLevelData()
		{
			this.Troops = new LevelMap();
			this.Starships = new LevelMap();
			this.Buildings = new LevelMap();
			this.Equipment = new LevelMap();
		}

		public void UpgradeTroopsOrStarships(Contract contract)
		{
			bool isStarship = contract.DeliveryType == DeliveryType.UpgradeStarship;
			this.UpgradeTroopsOrStarships(contract.ProductUid, isStarship);
		}

		public void UpgradeTroopsOrStarships(string uid, bool isStarship)
		{
			IDataController dataController = Service.Get<IDataController>();
			IUpgradeableVO upgradeableVO;
			InventoryStorage storage;
			if (isStarship)
			{
				upgradeableVO = dataController.Get<SpecialAttackTypeVO>(uid);
				this.Starships.SetLevel(upgradeableVO);
				storage = GameUtils.GetWorldOwner().Inventory.SpecialAttack;
			}
			else
			{
				upgradeableVO = dataController.Get<TroopTypeVO>(uid);
				this.Troops.SetLevel(upgradeableVO);
				TroopType type = ((TroopTypeVO)upgradeableVO).Type;
				if (type != TroopType.Hero)
				{
					if (type != TroopType.Champion)
					{
						storage = GameUtils.GetWorldOwner().Inventory.Troop;
					}
					else
					{
						storage = GameUtils.GetWorldOwner().Inventory.Champion;
					}
				}
				else
				{
					storage = GameUtils.GetWorldOwner().Inventory.Hero;
				}
			}
			UnlockedLevelData.UpgradeTroopsOrStarshipsInventory(storage, isStarship, upgradeableVO.UpgradeGroup, upgradeableVO.Uid);
		}

		public static void UpgradeTroopsOrStarshipsInventory(InventoryStorage storage, bool isStarship, string productUpgradeGroup, string productUid)
		{
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, InventoryEntry> internalStorage = storage.GetInternalStorage();
			int num = 0;
			foreach (string current in internalStorage.Keys)
			{
				IUpgradeableVO arg_43_0;
				if (!isStarship)
				{
					IUpgradeableVO upgradeableVO = dataController.Get<TroopTypeVO>(current);
					arg_43_0 = upgradeableVO;
				}
				else
				{
					IUpgradeableVO upgradeableVO = dataController.Get<SpecialAttackTypeVO>(current);
					arg_43_0 = upgradeableVO;
				}
				IUpgradeableVO upgradeableVO2 = arg_43_0;
				if (upgradeableVO2.UpgradeGroup == productUpgradeGroup)
				{
					num += internalStorage[current].Amount;
					storage.ClearItemAmount(current);
				}
			}
			storage.ModifyItemAmount(productUid, num);
		}

		public void UpgradeEquipmentLevel(Contract contract)
		{
			IDataController dataController = Service.Get<IDataController>();
			IUpgradeableVO level = dataController.Get<EquipmentVO>(contract.ProductUid);
			this.Equipment.SetLevel(level);
		}

		public string ToJson()
		{
			Serializer serializer = Serializer.Start();
			return serializer.End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("troop"))
			{
				this.Troops.FromObject(dictionary["troop"]);
			}
			if (dictionary.ContainsKey("specialAttack"))
			{
				this.Starships.FromObject(dictionary["specialAttack"]);
			}
			if (dictionary.ContainsKey("building"))
			{
				this.Buildings.FromObject(dictionary["building"]);
			}
			if (dictionary.ContainsKey("equipment"))
			{
				this.Equipment.FromObject(dictionary["equipment"]);
			}
			return this;
		}

		protected internal UnlockedLevelData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UnlockedLevelData)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UnlockedLevelData)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UnlockedLevelData)GCHandledObjects.GCHandleToObject(instance)).UpgradeEquipmentLevel((Contract)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UnlockedLevelData)GCHandledObjects.GCHandleToObject(instance)).UpgradeTroopsOrStarships((Contract)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UnlockedLevelData)GCHandledObjects.GCHandleToObject(instance)).UpgradeTroopsOrStarships(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			UnlockedLevelData.UpgradeTroopsOrStarshipsInventory((InventoryStorage)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}
	}
}
