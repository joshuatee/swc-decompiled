using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers.Notifications;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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

		protected internal PlayerFueCompleteCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlayerFueCompleteCommand)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}
	}
}
