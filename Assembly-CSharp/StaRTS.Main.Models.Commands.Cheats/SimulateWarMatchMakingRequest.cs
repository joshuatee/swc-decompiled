using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class SimulateWarMatchMakingRequest : PlayerIdRequest
	{
		private string quildId;

		public SimulateWarMatchMakingRequest(string squadId)
		{
			this.quildId = squadId;
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("guildId", this.quildId);
			return serializer.End().ToString();
		}

		protected internal SimulateWarMatchMakingRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SimulateWarMatchMakingRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
