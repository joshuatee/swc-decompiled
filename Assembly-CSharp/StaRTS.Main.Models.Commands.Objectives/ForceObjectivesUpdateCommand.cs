using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Objectives
{
	public class ForceObjectivesUpdateCommand : GameCommand<PlanetIdRequest, ForceObjectivesResponse>
	{
		public const string ACTION = "player.objective.forceUpdate";

		private string planetUid;

		public ForceObjectivesUpdateCommand(string planetUid) : base("player.objective.forceUpdate", new PlanetIdRequest(planetUid), new ForceObjectivesResponse(planetUid))
		{
			this.planetUid = planetUid;
		}

		public override void OnSuccess()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (!currentPlayer.Objectives.ContainsKey(this.planetUid))
			{
				currentPlayer.Objectives.Add(this.planetUid, base.ResponseResult.Group);
			}
			else
			{
				currentPlayer.Objectives[this.planetUid] = base.ResponseResult.Group;
			}
			base.OnSuccess();
		}

		protected internal ForceObjectivesUpdateCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ForceObjectivesUpdateCommand)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}
	}
}
