using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetSquadLevelRequest : PlayerIdRequest
	{
		private int levelToSet;

		public CheatSetSquadLevelRequest(int levelToSet)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.levelToSet = levelToSet;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("level", this.levelToSet.ToString());
			return serializer.End().ToString();
		}

		protected internal CheatSetSquadLevelRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetSquadLevelRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
