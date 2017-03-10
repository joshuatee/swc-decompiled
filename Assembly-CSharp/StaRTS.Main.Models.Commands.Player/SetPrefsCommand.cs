using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class SetPrefsCommand : GameCommand<SetPrefsRequest, DefaultResponse>
	{
		public const string ACTION = "player.prefs.set";

		private bool reloadOnResponse;

		public SetPrefsCommand(bool reloadOnResponse) : base("player.prefs.set", new SetPrefsRequest(), new DefaultResponse())
		{
			this.reloadOnResponse = reloadOnResponse;
		}

		public override void OnSuccess()
		{
			if (this.reloadOnResponse)
			{
				Service.Get<Engine>().Reload();
			}
		}

		protected internal SetPrefsCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SetPrefsCommand)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}
	}
}
