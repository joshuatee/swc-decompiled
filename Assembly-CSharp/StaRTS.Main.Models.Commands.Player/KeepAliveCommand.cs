using StaRTS.Externals.Manimal.TransferObjects.Response;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class KeepAliveCommand : GameCommand<KeepAliveRequest, DefaultResponse>
	{
		public const string ACTION = "player.keepAlive";

		public KeepAliveCommand(KeepAliveRequest request) : base("player.keepAlive", request, new DefaultResponse())
		{
		}

		protected override bool IsAddToken()
		{
			return false;
		}

		protected internal KeepAliveCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((KeepAliveCommand)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}
	}
}
