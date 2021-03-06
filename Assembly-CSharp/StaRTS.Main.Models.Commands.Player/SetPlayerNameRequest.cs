using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class SetPlayerNameRequest : PlayerIdRequest
	{
		private string name;

		public SetPlayerNameRequest(string nameRequested)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.name = nameRequested;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("playerName", this.name);
			return serializer.End().ToString();
		}

		protected internal SetPlayerNameRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SetPlayerNameRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
