using Net.RichardLord.Ash.Core;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TroopShieldComponent : ComponentBase
	{
		private bool activiated;

		private uint timerID;

		private uint coolDownInterval;

		private SmartEntity troop;

		public TroopShieldComponent(SmartEntity entity, uint cooldown)
		{
			this.troop = entity;
			this.coolDownInterval = cooldown * 1000u;
			this.Activiate();
		}

		public void CoolDownTimedOut(uint id, object cookie)
		{
			this.Activiate();
		}

		public void Activiate()
		{
			if (this.troop.HealthComp.IsDead())
			{
				return;
			}
			this.timerID = 0u;
			this.troop.TroopShieldHealthComp.Health = this.troop.TroopShieldHealthComp.MaxHealth;
			if (this.troop.TroopShieldViewComp != null)
			{
				this.troop.TroopShieldViewComp.PlayActivateAnimation();
				Service.Get<EventManager>().SendEvent(EventId.ChampionShieldActivated, null);
			}
			this.activiated = true;
		}

		public void Deactiviate()
		{
			if (this.troop.TroopShieldViewComp != null)
			{
				this.troop.TroopShieldViewComp.PlayDeactivateAnimation();
				Service.Get<EventManager>().SendEvent(EventId.ChampionShieldDestroyed, null);
			}
			this.activiated = false;
			this.troop.TroopShieldHealthComp.Health = 0;
			this.timerID = Service.Get<SimTimerManager>().CreateSimTimer(this.coolDownInterval, false, new TimerDelegate(this.CoolDownTimedOut), null);
		}

		public override void OnRemove()
		{
			TroopShieldViewComponent troopShieldViewComp = ((SmartEntity)this.Entity).TroopShieldViewComp;
			if (troopShieldViewComp != null)
			{
				troopShieldViewComp.PlayDeactivateAnimation();
				Service.Get<EventManager>().SendEvent(EventId.ChampionShieldDeactivated, null);
			}
			if (this.timerID != 0u)
			{
				Service.Get<SimTimerManager>().KillSimTimer(this.timerID);
			}
		}

		public bool IsActive()
		{
			return this.activiated;
		}

		protected internal TroopShieldComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TroopShieldComponent)GCHandledObjects.GCHandleToObject(instance)).Activiate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TroopShieldComponent)GCHandledObjects.GCHandleToObject(instance)).Deactiviate();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopShieldComponent)GCHandledObjects.GCHandleToObject(instance)).IsActive());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TroopShieldComponent)GCHandledObjects.GCHandleToObject(instance)).OnRemove();
			return -1L;
		}
	}
}
