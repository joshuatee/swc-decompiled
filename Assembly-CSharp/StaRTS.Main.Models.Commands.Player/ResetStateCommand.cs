using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class ResetStateCommand : GameCommand<PlayerIdRequest, DefaultResponse>
	{
		public const string ACTION = "player.state.reset";

		public ResetStateCommand(PlayerIdRequest request) : base("player.state.reset", request, new DefaultResponse())
		{
		}

		public override void OnSuccess()
		{
			Service.Get<StaRTSLogger>().Debug("Reset successful.");
		}

		protected internal ResetStateCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ResetStateCommand)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}
	}
}
