using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Perks
{
	public class PerksData : ISerializable
	{
		public List<ActivatedPerkData> ActivatedPerks
		{
			get;
			private set;
		}

		public Dictionary<string, uint> Cooldowns
		{
			get;
			private set;
		}

		public bool HasActivatedFirstPerk
		{
			get;
			private set;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("activatedPerks"))
			{
				List<object> list = dictionary["activatedPerks"] as List<object>;
				this.ActivatedPerks = new List<ActivatedPerkData>();
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					ActivatedPerkData activatedPerkData = new ActivatedPerkData();
					activatedPerkData.FromObject(list[i]);
					this.ActivatedPerks.Add(activatedPerkData);
					i++;
				}
			}
			if (dictionary.ContainsKey("cooldowns"))
			{
				this.Cooldowns = new Dictionary<string, uint>();
				Dictionary<string, object> dictionary2 = dictionary["cooldowns"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					foreach (KeyValuePair<string, object> current in dictionary2)
					{
						this.Cooldowns.Add(current.get_Key(), Convert.ToUInt32(current.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			if (dictionary.ContainsKey("hasActivatedFirstPerk"))
			{
				this.HasActivatedFirstPerk = (bool)dictionary["hasActivatedFirstPerk"];
			}
			return this;
		}

		public string ToJson()
		{
			Service.Get<StaRTSLogger>().Warn("Attempting to inappropriately serialize PerksData");
			return string.Empty;
		}

		public PerksData()
		{
		}

		protected internal PerksData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerksData)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerksData)GCHandledObjects.GCHandleToObject(instance)).ActivatedPerks);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerksData)GCHandledObjects.GCHandleToObject(instance)).HasActivatedFirstPerk);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PerksData)GCHandledObjects.GCHandleToObject(instance)).ActivatedPerks = (List<ActivatedPerkData>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PerksData)GCHandledObjects.GCHandleToObject(instance)).HasActivatedFirstPerk = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerksData)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
