using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Perks
{
	public class PerksInfo : ISerializable
	{
		public PerksData Perks
		{
			get;
			private set;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("perks"))
			{
				this.UpdatePerksData(dictionary["perks"]);
			}
			return this;
		}

		public void UpdatePerksData(object obj)
		{
			if (this.Perks != null)
			{
				this.Perks = null;
			}
			if (obj != null)
			{
				this.Perks = new PerksData();
				this.Perks.FromObject(obj);
			}
		}

		public string ToJson()
		{
			Service.Get<StaRTSLogger>().Warn("Attempting to inappropriately serialize PerksInfo");
			return string.Empty;
		}

		public PerksInfo()
		{
		}

		protected internal PerksInfo(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerksInfo)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerksInfo)GCHandledObjects.GCHandleToObject(instance)).Perks);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PerksInfo)GCHandledObjects.GCHandleToObject(instance)).Perks = (PerksData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerksInfo)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PerksInfo)GCHandledObjects.GCHandleToObject(instance)).UpdatePerksData(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
