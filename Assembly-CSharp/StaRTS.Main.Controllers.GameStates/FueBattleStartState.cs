using StaRTS.Main.Models.Battle;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.GameStates
{
	public class FueBattleStartState : BattleStartState
	{
		public FueBattleStartState(string battleVOId)
		{
			BattleInitializationData data = BattleInitializationData.CreateFromBattleTypeVO(battleVOId);
			base.Setup(data, null);
		}

		public override void OnEnter()
		{
		}

		protected internal FueBattleStartState(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((FueBattleStartState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}
	}
}
