using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Store
{
	public class InventoryStorage : ISerializable
	{
		public const int NO_LIMIT = -1;

		public const string INVENTORY_CREDITS = "credits";

		public const string INVENTORY_MATERIALS = "materials";

		public const string INVENTORY_CONTRABAND = "contraband";

		public const string INVENTORY_REPUTATION = "reputation";

		public const string INVENTORY_CRYSTALS = "crystals";

		public const string INVENTORY_DROIDS = "droids";

		public const string INVENTORY_XP = "xp";

		public const string SUBSTORAGE_TROOP = "troop";

		public const string SUBSTORAGE_SPECIALATTACK = "specialAttack";

		public const string SUBSTORAGE_HERO = "hero";

		public const string SUBSTORAGE_CHAMPION = "champion";

		public const string SUBSTORAGE_BUILDING = "building";

		public const string SUBSTORAGE_RESOURCE = "resources";

		private Dictionary<string, InventoryEntry> internalStorage;

		private Dictionary<string, InventoryStorage> subStorage;

		private int totalStorageCapacity;

		private EventId inventoryEvent;

		private Type inventoryType;

		public string Key
		{
			get;
			set;
		}

		public InventoryStorage(string key, EventId updateEvent, Type inventoryType)
		{
			this.inventoryType = inventoryType;
			this.Key = key;
			this.inventoryEvent = updateEvent;
			this.internalStorage = new Dictionary<string, InventoryEntry>();
			this.subStorage = new Dictionary<string, InventoryStorage>();
			this.totalStorageCapacity = -1;
		}

		public void CreateInventoryItem(string inventoryKey, int amount, int capacity)
		{
			InventoryEntry inventoryEntry = new InventoryEntry();
			inventoryEntry.Amount = amount;
			inventoryEntry.Capacity = capacity;
			this.internalStorage.Add(inventoryKey, inventoryEntry);
		}

		private void CalculateScale(string inventoryEntryKey)
		{
			int scale = 1;
			if (this.inventoryType != null)
			{
				IUpgradeableVO upgradeableVO = null;
				if (this.inventoryType == typeof(TroopTypeVO))
				{
					upgradeableVO = Service.Get<IDataController>().Get<TroopTypeVO>(inventoryEntryKey);
				}
				else if (this.inventoryType == typeof(SpecialAttackTypeVO))
				{
					upgradeableVO = Service.Get<IDataController>().Get<SpecialAttackTypeVO>(inventoryEntryKey);
				}
				if (upgradeableVO != null)
				{
					scale = upgradeableVO.Size;
				}
			}
			this.internalStorage[inventoryEntryKey].Scale = scale;
		}

		public void CreateInventoryItem(string inventoryKey, InventoryEntry entry)
		{
			if (this.internalStorage.ContainsKey(inventoryKey))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Key {0} already exists in storage {1}.", new object[]
				{
					inventoryKey,
					this.Key
				});
				return;
			}
			this.internalStorage.Add(inventoryKey, entry);
		}

		public bool HasItem(string inventoryKey)
		{
			return this.internalStorage.ContainsKey(inventoryKey);
		}

		public void SetItemCapacity(string inventoryKey, int newCapacity)
		{
			if (!this.internalStorage.ContainsKey(inventoryKey))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Key {0} not found in storage {1}.", new object[]
				{
					inventoryKey,
					this.Key
				});
				return;
			}
			this.internalStorage[inventoryKey].Capacity = newCapacity;
			Service.Get<EventManager>().SendEvent(this.inventoryEvent, inventoryKey);
		}

		public int GetItemCapacity(string inventoryKey)
		{
			if (!this.internalStorage.ContainsKey(inventoryKey))
			{
				return -1;
			}
			return this.internalStorage[inventoryKey].Capacity;
		}

		public bool ModifyItemAmount(string inventoryKey, int delta)
		{
			if (inventoryKey == null)
			{
				return false;
			}
			if (!this.internalStorage.ContainsKey(inventoryKey))
			{
				this.CreateInventoryItem(inventoryKey, 0, -1);
			}
			InventoryEntry inventoryEntry = this.internalStorage[inventoryKey];
			if (inventoryEntry.Capacity != -1 && delta > 0 && inventoryEntry.Amount + delta > inventoryEntry.Capacity)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Item storage exceeded: {0} + {1} > {2}. Key {3} in storage {4}.", new object[]
				{
					inventoryEntry.Amount,
					delta,
					inventoryEntry.Capacity,
					inventoryKey,
					this.Key
				});
				return false;
			}
			int num = this.GetTotalStorageCapacity();
			if (num != -1)
			{
				int totalStorageAmount = this.GetTotalStorageAmount();
				if (inventoryEntry.Scale == -1)
				{
					this.CalculateScale(inventoryKey);
				}
				int num2 = delta * inventoryEntry.Scale;
				if (num2 > 0 && totalStorageAmount + num2 > num)
				{
					Service.Get<StaRTSLogger>().WarnFormat("Total storage exceeded: {0} + {1} > {2}. Key {3} in storage {4}.", new object[]
					{
						totalStorageAmount,
						num2,
						num,
						inventoryKey,
						this.Key
					});
					return false;
				}
			}
			inventoryEntry.Amount += delta;
			Service.Get<EventManager>().SendEvent(this.inventoryEvent, inventoryKey);
			return true;
		}

		public bool CanStoreAll(int delta, CurrencyType currencyType)
		{
			string key;
			switch (currencyType)
			{
			case CurrencyType.Credits:
				key = "credits";
				break;
			case CurrencyType.Materials:
				key = "materials";
				break;
			case CurrencyType.Contraband:
				key = "contraband";
				break;
			case CurrencyType.Reputation:
				key = "reputation";
				break;
			default:
				return false;
			}
			InventoryEntry inventoryEntry = this.internalStorage[key];
			int num = delta;
			if (inventoryEntry.Capacity != -1)
			{
				num = inventoryEntry.Capacity - inventoryEntry.Amount;
			}
			return delta <= num;
		}

		public int ModifyCredits(int delta)
		{
			if (delta == 0)
			{
				return 0;
			}
			int val = delta;
			if (this.internalStorage["credits"].Capacity != -1)
			{
				val = this.internalStorage["credits"].Capacity - this.internalStorage["credits"].Amount;
			}
			int num = Math.Min(delta, val);
			this.ModifyItemAmount("credits", num);
			return delta - num;
		}

		public int ModifyMaterials(int delta)
		{
			if (delta == 0)
			{
				return 0;
			}
			int val = delta;
			if (this.internalStorage["materials"].Capacity != -1)
			{
				val = this.internalStorage["materials"].Capacity - this.internalStorage["materials"].Amount;
			}
			int num = Math.Min(delta, val);
			this.ModifyItemAmount("materials", num);
			return delta - num;
		}

		public int ModifyContraband(int delta)
		{
			if (delta == 0)
			{
				return 0;
			}
			int val = delta;
			if (this.internalStorage["contraband"].Capacity != -1)
			{
				val = this.internalStorage["contraband"].Capacity - this.internalStorage["contraband"].Amount;
			}
			int num = Math.Min(delta, val);
			this.ModifyItemAmount("contraband", num);
			return delta - num;
		}

		public int ModifyReputation(int delta)
		{
			if (delta == 0)
			{
				return 0;
			}
			int val = delta;
			int capacity = this.internalStorage["reputation"].Capacity;
			int amount = this.internalStorage["reputation"].Amount;
			if (capacity != -1)
			{
				val = capacity - amount;
			}
			int num = Math.Max(Math.Min(delta, val), -amount);
			this.ModifyItemAmount("reputation", num);
			return delta - num;
		}

		public void ModifyCrystals(int delta)
		{
			this.ModifyItemAmount("crystals", delta);
		}

		public void ModifyDroids(int delta)
		{
			this.ModifyItemAmount("droids", delta);
			Service.Get<EventManager>().SendEvent(this.inventoryEvent, "droids");
		}

		public void ModifyXP(int delta)
		{
			this.ModifyItemAmount("xp", delta);
			Service.Get<EventManager>().SendEvent(this.inventoryEvent, "xp");
		}

		public int GetItemAmount(string inventoryKey)
		{
			if (!this.internalStorage.ContainsKey(inventoryKey))
			{
				return 0;
			}
			return this.internalStorage[inventoryKey].Amount;
		}

		public InventoryStorage Sub(string key)
		{
			if (!this.subStorage.ContainsKey(key))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Substorage {0} not found in storage {1}.", new object[]
				{
					key,
					this.Key
				});
				return new InventoryStorage(key, this.inventoryEvent, null);
			}
			return this.subStorage[key];
		}

		public InventoryStorage CreateSubstorage(string key, EventId updateEvent, Type inventoryType)
		{
			InventoryStorage inventoryStorage = new InventoryStorage(key, updateEvent, inventoryType);
			this.subStorage.Add(key, inventoryStorage);
			return inventoryStorage;
		}

		public int GetTotalStorageAmount()
		{
			int num = 0;
			foreach (KeyValuePair<string, InventoryEntry> current in this.internalStorage)
			{
				if (current.get_Value().Scale == -1)
				{
					this.CalculateScale(current.get_Key());
				}
				num += current.get_Value().Amount * current.get_Value().Scale;
			}
			return num;
		}

		public void SetTotalStorageCapacity(int capacity)
		{
			this.totalStorageCapacity = capacity;
			Service.Get<EventManager>().SendEvent(this.inventoryEvent, null);
		}

		public int GetTotalStorageCapacity()
		{
			return this.totalStorageCapacity;
		}

		public void ClearItemAmount(string inventoryKey)
		{
			if (!this.internalStorage.ContainsKey(inventoryKey))
			{
				this.CreateInventoryItem(inventoryKey, 0, -1);
			}
			InventoryEntry inventoryEntry = this.internalStorage[inventoryKey];
			inventoryEntry.Amount = 0;
			Service.Get<EventManager>().SendEvent(this.inventoryEvent, inventoryKey);
		}

		public Dictionary<string, InventoryEntry> GetInternalStorage()
		{
			return this.internalStorage;
		}

		public string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.Add<int>("capacity", this.totalStorageCapacity);
			Serializer serializer2 = Serializer.Start();
			foreach (KeyValuePair<string, InventoryEntry> current in this.internalStorage)
			{
				serializer2.AddObject<InventoryEntry>(current.get_Key(), current.get_Value());
			}
			serializer2.End();
			serializer.Add<string>("storage", serializer2.ToString());
			if (this.subStorage.Count > 0)
			{
				Serializer serializer3 = Serializer.Start();
				foreach (KeyValuePair<string, InventoryStorage> current2 in this.subStorage)
				{
					serializer3.AddObject<InventoryStorage>(current2.get_Key(), current2.get_Value());
				}
				serializer3.End();
				serializer.Add<string>("subStorage", serializer3.ToString());
			}
			return serializer.End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.totalStorageCapacity = Convert.ToInt32(dictionary["capacity"], CultureInfo.InvariantCulture);
			if (dictionary.ContainsKey("storage"))
			{
				Dictionary<string, object> dictionary2 = dictionary["storage"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					foreach (KeyValuePair<string, object> current in dictionary2)
					{
						this.CreateStorageItem(current.get_Key(), current.get_Value());
					}
				}
			}
			if (!dictionary.ContainsKey("subStorage"))
			{
				return this;
			}
			dictionary = (dictionary["subStorage"] as Dictionary<string, object>);
			foreach (KeyValuePair<string, InventoryStorage> current2 in this.subStorage)
			{
				if (dictionary.ContainsKey(current2.get_Key()))
				{
					current2.get_Value().FromObject(dictionary[current2.get_Key()]);
				}
			}
			return this;
		}

		protected void CreateStorageItem(string key, object obj)
		{
			InventoryEntry inventoryEntry = new InventoryEntry();
			inventoryEntry.FromObject(obj);
			this.CreateInventoryItem(key, inventoryEntry);
		}

		public bool IsInventorySubstorageFull()
		{
			if (this.totalStorageCapacity == -1)
			{
				return false;
			}
			int num = 0;
			foreach (string current in this.subStorage.Keys)
			{
				InventoryStorage inventoryStorage = this.subStorage[current];
				num += inventoryStorage.GetTotalStorageAmount();
			}
			return num >= this.totalStorageCapacity;
		}

		public void AddString(StringBuilder sb, bool skipScale)
		{
			List<string> list = new List<string>(this.GetInternalStorage().Keys);
			list.Sort();
			foreach (string current in list)
			{
				if (current != "xp" && current != "credits" && current != "crystals" && current != "materials")
				{
					sb.Append(current).Append("|");
					this.GetInternalStorage()[current].AddString(sb, skipScale);
				}
			}
		}

		public void AddString(StringBuilder sb)
		{
			this.AddString(sb, false);
		}

		protected internal InventoryStorage(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).AddString((StringBuilder)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).AddString((StringBuilder)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).CalculateScale(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).CanStoreAll(*(int*)args, (CurrencyType)(*(int*)(args + 1))));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ClearItemAmount(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).CreateInventoryItem(Marshal.PtrToStringUni(*(IntPtr*)args), (InventoryEntry)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).CreateInventoryItem(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).CreateStorageItem(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).CreateSubstorage(Marshal.PtrToStringUni(*(IntPtr*)args), (EventId)(*(int*)(args + 1)), (Type)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).Key);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).GetInternalStorage());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).GetItemAmount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).GetItemCapacity(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).GetTotalStorageAmount());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).GetTotalStorageCapacity());
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).HasItem(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).IsInventorySubstorageFull());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ModifyContraband(*(int*)args));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ModifyCredits(*(int*)args));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ModifyCrystals(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ModifyDroids(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ModifyItemAmount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ModifyMaterials(*(int*)args));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ModifyReputation(*(int*)args));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ModifyXP(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).Key = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).SetItemCapacity(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).SetTotalStorageCapacity(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).Sub(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryStorage)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
