using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines.Attack
{
	public class IdleState : AttackFSMState
	{
		public IdleState(AttackFSM owner) : base(owner, 0u)
		{
		}

		public override void OnEnter()
		{
			base.NextUnlockTime = 0u;
		}

		protected internal IdleState(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IdleState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}
	}
}
