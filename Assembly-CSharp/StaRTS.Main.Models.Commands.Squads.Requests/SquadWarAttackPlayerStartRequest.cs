using StaRTS.Externals.FileManagement;
using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

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
			startedSerializer.AddString("battleVersion", "21.0");
			startedSerializer.AddString("simSeedA", Service.Get<BattleController>().SimSeed.SimSeedA.ToString());
			startedSerializer.AddString("simSeedB", Service.Get<BattleController>().SimSeed.SimSeedB.ToString());
			return startedSerializer.End().ToString();
		}

		protected internal SquadWarAttackPlayerStartRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarAttackPlayerStartRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
