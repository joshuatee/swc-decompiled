using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Store
{
	public class PrizeInventory : ISerializable
	{
		public Dictionary<string, int> Troops
		{
			get;
			private set;
		}

		public Dictionary<string, int> SpecialAttacks
		{
			get;
			private set;
		}

		public Dictionary<string, int> CurrencyResources
		{
			get;
			private set;
		}

		public InventoryCrates Crates
		{
			get;
			private set;
		}

		public PrizeInventory()
		{
			this.Crates = new InventoryCrates();
			this.Troops = new Dictionary<string, int>();
			this.SpecialAttacks = new Dictionary<string, int>();
			this.CurrencyResources = new Dictionary<string, int>();
		}

		public int GetTroopAmount(string troopID)
		{
			return this.GetAmount(this.Troops, troopID);
		}

		public int GetSpecialAttackAmount(string specialAttackID)
		{
			return this.GetAmount(this.SpecialAttacks, specialAttackID);
		}

		public int GetResourceAmount(string resourceName)
		{
			return this.GetAmount(this.CurrencyResources, resourceName);
		}

		private int GetAmount(Dictionary<string, int> dict, string key)
		{
			if (!dict.ContainsKey(key))
			{
				return 0;
			}
			return dict[key];
		}

		public int GetTotalTroopAmount()
		{
			int num = 0;
			foreach (KeyValuePair<string, int> current in this.Troops)
			{
				num += current.get_Value();
			}
			return num;
		}

		public int GetTotalSpecialAttackAmount()
		{
			int num = 0;
			foreach (KeyValuePair<string, int> current in this.SpecialAttacks)
			{
				num += current.get_Value();
			}
			return num;
		}

		public void ModifyTroopAmount(string troopID, int delta)
		{
			this.ModifyAmount(this.Troops, troopID, delta);
		}

		public void ModifySpecialAttackAmount(string specialAttackID, int delta)
		{
			this.ModifyAmount(this.SpecialAttacks, specialAttackID, delta);
		}

		public void ModifyResourceAmount(string resourceName, int delta)
		{
			this.ModifyAmount(this.CurrencyResources, resourceName, delta);
		}

		private void ModifyAmount(Dictionary<string, int> dict, string key, int delta)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] += delta;
				return;
			}
			if (delta > 0)
			{
				dict[key] = delta;
			}
		}

		public string ToJson()
		{
			return "{}";
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				if (dictionary.ContainsKey("troop"))
				{
					this.ObjectToDictionary(dictionary["troop"], this.Troops);
				}
				if (dictionary.ContainsKey("specialAttack"))
				{
					this.ObjectToDictionary(dictionary["specialAttack"], this.SpecialAttacks);
				}
				if (dictionary.ContainsKey("resources"))
				{
					this.ObjectToDictionary(dictionary["resources"], this.CurrencyResources);
				}
				if (dictionary.ContainsKey("crates"))
				{
					object obj2 = dictionary["crates"];
					this.Crates.FromObject(obj2);
				}
			}
			return this;
		}

		private void ObjectToDictionary(object obj, Dictionary<string, int> dict)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, object> current in dictionary)
				{
					dict.Add(current.get_Key(), Convert.ToInt32(current.get_Value(), CultureInfo.InvariantCulture));
				}
			}
		}

		protected internal PrizeInventory(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).Crates);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).CurrencyResources);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).SpecialAttacks);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).Troops);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).GetAmount((Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).GetResourceAmount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).GetSpecialAttackAmount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).GetTotalSpecialAttackAmount());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).GetTotalTroopAmount());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).GetTroopAmount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).ModifyAmount((Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).ModifyResourceAmount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).ModifySpecialAttackAmount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).ModifyTroopAmount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).ObjectToDictionary(GCHandledObjects.GCHandleToObject(*args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).Crates = (InventoryCrates)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).CurrencyResources = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).SpecialAttacks = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).Troops = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventory)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
