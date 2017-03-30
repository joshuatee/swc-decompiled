using StaRTS.Externals.FileManagement;
using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class SquadWarAttackPlayerStartRequest : PlayerIdChecksumRequest
	{
		private string opponentId;

		public SquadWarAttackPlayerStartRequest(string opponentId)
		{
			this.opponentId = opponentId;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("opponentId", this.opponentId);
			startedSerializer.AddString("cmsVersion", Service.Get<FMS>().GetFileVersion("patches/base.json").ToString());
			startedSerializer.AddString("battleVersion", "22.0");
			startedSerializer.AddString("simSeedA", Service.Get<BattleController>().SimSeed.SimSeedA.ToString());
			startedSerializer.AddString("simSeedB", Service.Get<BattleController>().SimSeed.SimSeedB.ToString());
			return startedSerializer.End().ToString();
		}
	}
}
