using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Squads
{
	public class SquadPerks : ISerializable
	{
		public Dictionary<string, string> Available;

		public Dictionary<string, int> InProgress;

		public ISerializable Default()
		{
			this.Available = new Dictionary<string, string>();
			this.InProgress = new Dictionary<string, int>();
			return this;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Available = new Dictionary<string, string>();
			if (dictionary.ContainsKey("available"))
			{
				Dictionary<string, object> dictionary2 = dictionary["available"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					foreach (KeyValuePair<string, object> current in dictionary2)
					{
						this.Available.Add(current.get_Key(), current.get_Value() as string);
					}
				}
			}
			this.InProgress = new Dictionary<string, int>();
			if (dictionary.ContainsKey("inProgress"))
			{
				Dictionary<string, object> dictionary3 = dictionary["inProgress"] as Dictionary<string, object>;
				if (dictionary3 != null)
				{
					foreach (KeyValuePair<string, object> current2 in dictionary3)
					{
						this.InProgress.Add(current2.get_Key(), Convert.ToInt32(current2.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			return this;
		}

		public void UpdateUnlockedPerk(string perkUID)
		{
			if (this.InProgress.ContainsKey(perkUID))
			{
				this.InProgress.Remove(perkUID);
			}
			IDataController dataController = Service.Get<IDataController>();
			PerkVO perkVO = dataController.Get<PerkVO>(perkUID);
			this.Available[perkVO.PerkGroup] = perkUID;
		}

		public void UpdatePerkInvestedAmt(string perkUID, int investedAmt)
		{
			if (!this.Available.ContainsValue(perkUID))
			{
				this.InProgress[perkUID] = investedAmt;
			}
		}

		public string ToJson()
		{
			Service.Get<StaRTSLogger>().Warn("Attempting to inappropriately serialize SquadPerks");
			return string.Empty;
		}

		public SquadPerks()
		{
		}

		protected internal SquadPerks(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadPerks)GCHandledObjects.GCHandleToObject(instance)).Default());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadPerks)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadPerks)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadPerks)GCHandledObjects.GCHandleToObject(instance)).UpdatePerkInvestedAmt(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadPerks)GCHandledObjects.GCHandleToObject(instance)).UpdateUnlockedPerk(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
