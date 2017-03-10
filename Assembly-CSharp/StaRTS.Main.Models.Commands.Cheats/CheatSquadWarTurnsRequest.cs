using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSquadWarTurnsRequest : PlayerIdRequest
	{
		private int turns;

		public CheatSquadWarTurnsRequest(int turns)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.turns = turns;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.Add<int>("turns", this.turns);
			return serializer.End().ToString();
		}

		protected internal CheatSquadWarTurnsRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSquadWarTurnsRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
