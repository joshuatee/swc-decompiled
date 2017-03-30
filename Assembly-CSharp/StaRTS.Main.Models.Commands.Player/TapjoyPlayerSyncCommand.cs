using StaRTS.Externals.Manimal.TransferObjects.Request;
using System;

namespace StaRTS.Main.Models.Commands.Player
{
	public class TapjoyPlayerSyncCommand : GameCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>
	{
		public const string ACTION = "player.get";

		public TapjoyPlayerSyncCommand(PlayerIdRequest request) : base("player.get", request, new TapjoyPlayerSyncResponse())
		{
		}
	}
}
