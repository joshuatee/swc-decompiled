using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetBattleStatsRequest : PlayerIdRequest
	{
		private KeyValuePair<string, int> argument;

		public CheatSetBattleStatsRequest(KeyValuePair<string, int> argument)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.argument = argument;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.Add<int>(this.argument.get_Key(), this.argument.get_Value());
			return serializer.End().ToString();
		}

		protected internal CheatSetBattleStatsRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetBattleStatsRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
