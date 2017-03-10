using StaRTS.Main.Models;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.GameStates
{
	public class BattleEndPlaybackState : BattleEndState
	{
		public BattleEndPlaybackState(bool isSquadWarBattle) : base(isSquadWarBattle)
		{
		}

		public override void OnEnter()
		{
			base.ShowBattleEndScreen(true);
			Service.Get<UXController>().HUD.ConfigureControls(new HudConfig(new string[0]));
		}

		protected internal BattleEndPlaybackState(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BattleEndPlaybackState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}
	}
}
