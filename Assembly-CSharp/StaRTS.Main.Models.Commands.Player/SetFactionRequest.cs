using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class SetFactionRequest : PlayerIdRequest
	{
		private string factionString;

		public SetFactionRequest(FactionType faction)
		{
			this.factionString = faction.ToString().ToLower();
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("faction", this.factionString);
			return serializer.End().ToString();
		}

		protected internal SetFactionRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SetFactionRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
