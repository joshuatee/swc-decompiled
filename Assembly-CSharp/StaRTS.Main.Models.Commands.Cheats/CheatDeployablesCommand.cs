using StaRTS.Externals.Manimal.TransferObjects.Response;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatDeployablesCommand : GameCommand<CheatDeployablesRequest, DefaultResponse>
	{
		public const string ACTION = "cheats.deployable.set";

		public static CheatDeployablesCommand SetDeployableAmount(string troopUid, int amount)
		{
			CheatDeployablesRequest request = new CheatDeployablesRequest(new Dictionary<string, int>
			{
				{
					troopUid,
					amount
				}
			});
			return new CheatDeployablesCommand(request);
		}

		public CheatDeployablesCommand(CheatDeployablesRequest request) : base("cheats.deployable.set", request, new DefaultResponse())
		{
		}

		protected internal CheatDeployablesCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CheatDeployablesCommand.SetDeployableAmount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}
	}
}
