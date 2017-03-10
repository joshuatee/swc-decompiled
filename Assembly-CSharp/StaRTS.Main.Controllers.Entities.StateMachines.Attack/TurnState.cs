using StaRTS.Main.Models.Entities.Shared;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines.Attack
{
	public class TurnState : AttackFSMState
	{
		public TurnState(AttackFSM owner) : base(owner, 2147483647u)
		{
		}

		public uint GetDuration()
		{
			return base.LockDuration;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			base.AttackFSMOwner.StateComponent.CurState = EntityState.Turning;
		}

		protected internal TurnState(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TurnState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}
	}
}
