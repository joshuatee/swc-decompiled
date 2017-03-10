using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class ExternalCurrencySyncResponse : AbstractResponse
	{
		public object Result
		{
			get;
			protected set;
		}

		public uint Status
		{
			get;
			protected set;
		}

		public List<Data> DataList
		{
			get;
			protected set;
		}

		protected virtual void LogResults(KeyValuePair<string, object> entry, int diff)
		{
		}

		public override ISerializable FromObject(object obj)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (!dictionary.ContainsKey("playerModel"))
			{
				return this;
			}
			Dictionary<string, object> dictionary2 = dictionary["playerModel"] as Dictionary<string, object>;
			if (!dictionary2.ContainsKey("inventory"))
			{
				return this;
			}
			Dictionary<string, object> dictionary3 = dictionary2["inventory"] as Dictionary<string, object>;
			if (!dictionary3.ContainsKey("storage"))
			{
				return this;
			}
			Dictionary<string, object> dictionary4 = dictionary3["storage"] as Dictionary<string, object>;
			InventoryEntry inventoryEntry = new InventoryEntry();
			foreach (KeyValuePair<string, object> current in dictionary4)
			{
				inventoryEntry.FromObject(current.get_Value());
				int itemAmount = currentPlayer.Inventory.GetItemAmount(current.get_Key());
				int amount = inventoryEntry.Amount;
				int num = amount - itemAmount;
				Service.Get<StaRTSLogger>().Debug(current.get_Key() + ":" + num);
				if (num != 0)
				{
					currentPlayer.Inventory.ModifyItemAmount(current.get_Key(), num);
				}
				this.LogResults(current, num);
			}
			return this;
		}

		public ExternalCurrencySyncResponse()
		{
		}

		protected internal ExternalCurrencySyncResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ExternalCurrencySyncResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ExternalCurrencySyncResponse)GCHandledObjects.GCHandleToObject(instance)).DataList);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ExternalCurrencySyncResponse)GCHandledObjects.GCHandleToObject(instance)).Result);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ExternalCurrencySyncResponse)GCHandledObjects.GCHandleToObject(instance)).LogResults((KeyValuePair<string, object>)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ExternalCurrencySyncResponse)GCHandledObjects.GCHandleToObject(instance)).DataList = (List<Data>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ExternalCurrencySyncResponse)GCHandledObjects.GCHandleToObject(instance)).Result = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
