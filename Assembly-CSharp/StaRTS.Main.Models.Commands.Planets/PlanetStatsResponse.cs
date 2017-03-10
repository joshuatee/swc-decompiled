using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Planets
{
	public class PlanetStatsResponse : Response
	{
		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, object> current in dictionary)
				{
					string key = current.get_Key();
					int population = Convert.ToInt32(current.get_Value() as string, CultureInfo.InvariantCulture);
					Service.Get<GalaxyPlanetController>().UpdatePlanetPopulation(key, population);
				}
			}
			return this;
		}

		public PlanetStatsResponse()
		{
		}

		protected internal PlanetStatsResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetStatsResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
