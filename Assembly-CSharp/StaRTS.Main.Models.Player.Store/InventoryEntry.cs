using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Store
{
	public class InventoryEntry : ISerializable
	{
		public const int UNINITIALIZED_SCALE = -1;

		public int Amount;

		public int Capacity;

		public int Scale;

		public string ToJson()
		{
			return Serializer.Start().Add<int>("amount", this.Amount).Add<int>("capacity", this.Capacity).Add<int>("scale", 0).End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Amount = Convert.ToInt32(dictionary["amount"], CultureInfo.InvariantCulture);
			this.Capacity = Convert.ToInt32(dictionary["capacity"], CultureInfo.InvariantCulture);
			return this;
		}

		public void AddString(StringBuilder sb, bool skipScale)
		{
			sb.Append(this.Amount).Append("|").Append(this.Capacity).Append("|").Append(skipScale ? "" : this.Scale.ToString()).Append("\n");
		}

		public InventoryEntry()
		{
			this.Scale = -1;
			base..ctor();
		}

		protected internal InventoryEntry(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((InventoryEntry)GCHandledObjects.GCHandleToObject(instance)).AddString((StringBuilder)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryEntry)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryEntry)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
