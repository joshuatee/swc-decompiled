using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Models.Battle
{
	public class DefenseWave
	{
		public float Delay
		{
			get;
			private set;
		}

		public DefenseEncounterVO Encounter
		{
			get;
			private set;
		}

		public List<Entity> Troops
		{
			get;
			private set;
		}

		public DefenseWave(string defenseEncounter, float delay)
		{
			this.Delay = delay;
			IDataController dataController = Service.Get<IDataController>();
			this.Encounter = dataController.GetOptional<DefenseEncounterVO>(defenseEncounter);
			this.Troops = new List<Entity>();
			if (this.Encounter == null)
			{
				Service.Get<Logger>().ErrorFormat("Defense Encounter {0} not found", new object[]
				{
					defenseEncounter
				});
			}
		}
	}
}
