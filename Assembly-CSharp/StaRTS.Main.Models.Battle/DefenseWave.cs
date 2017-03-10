using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

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
				Service.Get<StaRTSLogger>().ErrorFormat("Defense Encounter {0} not found", new object[]
				{
					defenseEncounter
				});
			}
		}

		protected internal DefenseWave(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseWave)GCHandledObjects.GCHandleToObject(instance)).Delay);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseWave)GCHandledObjects.GCHandleToObject(instance)).Encounter);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseWave)GCHandledObjects.GCHandleToObject(instance)).Troops);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DefenseWave)GCHandledObjects.GCHandleToObject(instance)).Delay = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DefenseWave)GCHandledObjects.GCHandleToObject(instance)).Encounter = (DefenseEncounterVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DefenseWave)GCHandledObjects.GCHandleToObject(instance)).Troops = (List<Entity>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
