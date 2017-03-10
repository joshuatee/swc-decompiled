using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatGetBattleRecordRequest : PlayerIdRequest
	{
		private string battleId;

		public CheatGetBattleRecordRequest(string battleId)
		{
			this.battleId = battleId;
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("battleId", this.battleId);
			serializer.AddString("playerId", base.PlayerId);
			return serializer.End().ToString();
		}

		protected internal CheatGetBattleRecordRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatGetBattleRecordRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
