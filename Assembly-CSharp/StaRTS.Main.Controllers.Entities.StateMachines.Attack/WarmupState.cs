using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines.Attack
{
	public class WarmupState : AttackFSMState
	{
		public WarmupState(AttackFSM owner) : base(owner, owner.ShooterComp.ShooterVO.WarmupDelay)
		{
		}

		public override void OnEnter()
		{
			base.OnEnter();
			base.AttackFSMOwner.StateComponent.CurState = EntityState.WarmingUp;
			Service.Get<EventManager>().SendEvent(EventId.ShooterWarmingUp, base.AttackFSMOwner.Entity);
		}

		protected internal WarmupState(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((WarmupState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}
	}
}
