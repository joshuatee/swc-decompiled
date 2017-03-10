using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines.Attack
{
	public class PostFireState : AttackFSMState
	{
		public PostFireState(AttackFSM owner) : base(owner, owner.ShooterComp.ShooterVO.ShotDelay)
		{
		}

		public override void OnExit(IState nextState)
		{
			base.OnExit(nextState);
		}

		protected internal PostFireState(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PostFireState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
