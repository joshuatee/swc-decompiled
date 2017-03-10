using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models.Planets;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class SpinPlanetForwardStoryAction : AbstractStoryAction
	{
		private Planet planet;

		public SpinPlanetForwardStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			if (this.prepareArgs.Length != 0)
			{
				this.planet = Service.Get<GalaxyPlanetController>().GetPlanet(this.prepareArgs[0]);
			}
			else
			{
				this.planet = this.FindFirstUnlockedPlanet();
			}
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<GalaxyViewController>().RotatePlanetToForeground(this.planet, true);
		}

		private void OnComplete(uint id, object cookie)
		{
			this.parent.ChildComplete(this);
		}

		private Planet FindFirstUnlockedPlanet()
		{
			PlanetVO planetVO = Service.Get<IDataController>().Get<PlanetVO>("planet1");
			Planet planet = null;
			List<Planet> listOfUnlockedPlanets = Service.Get<GalaxyPlanetController>().GetListOfUnlockedPlanets();
			int i = 0;
			int count = listOfUnlockedPlanets.Count;
			while (i < count)
			{
				Planet planet2 = listOfUnlockedPlanets[i];
				if (planet2.VO != planetVO)
				{
					return planet2;
				}
				planet = planet2;
				i++;
			}
			if (planet != null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Could not find any unlocked planets other than the default", new object[0]);
				return planet;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("Could not find any unlocked planets at all", new object[0]);
			return new Planet(planetVO);
		}

		protected internal SpinPlanetForwardStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SpinPlanetForwardStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpinPlanetForwardStoryAction)GCHandledObjects.GCHandleToObject(instance)).FindFirstUnlockedPlanet());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SpinPlanetForwardStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
