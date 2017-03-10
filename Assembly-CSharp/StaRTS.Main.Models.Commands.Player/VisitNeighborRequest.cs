using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class VisitNeighborRequest : AbstractRequest
	{
		private string neighborId;

		private string playerId;

		public VisitNeighborRequest(string neighborId)
		{
			this.neighborId = neighborId;
			this.playerId = Service.Get<CurrentPlayer>().PlayerId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", this.playerId);
			serializer.AddString("neighborId", this.neighborId);
			return serializer.End().ToString();
		}

		protected internal VisitNeighborRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VisitNeighborRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
