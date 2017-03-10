using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Request
{
	public class PvpGetNextTargetRequest : PlayerIdRequest
	{
		public string planetId;

		public PvpGetNextTargetRequest()
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.planetId = Service.Get<CurrentPlayer>().PlanetId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("planetId", this.planetId);
			return serializer.End().ToString();
		}

		protected internal PvpGetNextTargetRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpGetNextTargetRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
