using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public class ArmoryInfo : ISerializable
	{
		public bool FirstCratePurchased;

		public string ToJson()
		{
			return "{}";
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("firstCratePurchased"))
			{
				this.FirstCratePurchased = Convert.ToBoolean(dictionary["firstCratePurchased"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public ArmoryInfo()
		{
		}

		protected internal ArmoryInfo(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryInfo)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryInfo)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
