using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;

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
						this.Available.Add(current.Key, current.Value as string);
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
						this.InProgress.Add(current2.Key, Convert.ToInt32(current2.Value));
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
			Service.Get<Logger>().Warn("Attempting to inappropriately serialize SquadPerks");
			return string.Empty;
		}
	}
}
