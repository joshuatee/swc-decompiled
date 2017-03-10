using StaRTS.Externals.Manimal.TransferObjects.Response;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetBattleStatsCommand : GameCommand<CheatSetBattleStatsRequest, DefaultResponse>
	{
		private const string ACTION = "cheats.battleStats.set";

		public CheatSetBattleStatsCommand(CheatSetBattleStatsRequest request) : base("cheats.battleStats.set", request, new DefaultResponse())
		{
		}

		public static CheatSetBattleStatsCommand CreateSetBattleStatsCommand(string type, int amount)
		{
			KeyValuePair<string, int> argument = new KeyValuePair<string, int>(type, amount);
			CheatSetBattleStatsRequest request = new CheatSetBattleStatsRequest(argument);
			return new CheatSetBattleStatsCommand(request);
		}

		protected internal CheatSetBattleStatsCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CheatSetBattleStatsCommand.CreateSetBattleStatsCommand(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}
	}
}
