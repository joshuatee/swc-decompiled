using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines.Attack
{
	public class PreFireState : AttackFSMState
	{
		public PreFireState(AttackFSM owner) : base(owner, owner.ShooterComp.ShooterVO.AnimationDelay)
		{
		}

		public override void OnEnter()
		{
			base.OnEnter();
			base.AttackFSMOwner.StateComponent.CurState = EntityState.Attacking;
		}

		public override void OnExit(IState nextState)
		{
			base.OnExit(nextState);
			base.AttackFSMOwner.Fire();
		}

		protected internal PreFireState(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PreFireState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PreFireState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
