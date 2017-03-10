using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Planets
{
	public class PlanetStatsRequest : PlayerIdRequest
	{
		private List<string> planetUIDs;

		public PlanetStatsRequest()
		{
			this.planetUIDs = new List<string>();
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
		}

		public void AddPlanetID(string planetUID)
		{
			this.planetUIDs.Add(planetUID);
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddArrayOfPrimitives<string>("planets", this.planetUIDs);
			return serializer.End().ToString();
		}

		protected internal PlanetStatsRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlanetStatsRequest)GCHandledObjects.GCHandleToObject(instance)).AddPlanetID(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetStatsRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
