using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers.Notifications;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Models.Commands.Player.Fue
{
	public class PlayerFueCompleteCommand : GameActionCommand<PlayerIdChecksumRequest, DefaultResponse>
	{
		public const string ACTION = "player.fue.complete";

		public PlayerFueCompleteCommand(PlayerIdChecksumRequest request) : base("player.fue.complete", request, new DefaultResponse())
		{
		}

		public override void OnSuccess()
		{
			Service.Get<NotificationController>().Init();
		}
	}
}
