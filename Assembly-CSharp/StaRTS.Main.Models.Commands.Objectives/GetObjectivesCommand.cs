using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Objectives;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Objectives
{
	public class GetObjectivesCommand : GameCommand<PlayerIdRequest, GetObjectivesResponse>
	{
		public const string ACTION = "player.planet.objective";

		public GetObjectivesCommand(PlayerIdRequest request) : base("player.planet.objective", request, new GetObjectivesResponse())
		{
		}

		public override void OnSuccess()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			currentPlayer.Objectives.Clear();
			foreach (KeyValuePair<string, ObjectiveGroup> current in base.ResponseResult.Groups)
			{
				currentPlayer.Objectives.Add(current.get_Key(), current.get_Value());
			}
			base.OnSuccess();
		}

		protected internal GetObjectivesCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GetObjectivesCommand)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}
	}
}
