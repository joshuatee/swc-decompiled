using StaRTS.Utils.Core;
using System;

namespace StaRTS.Utils.Scheduling
{
	public class SimTimerManager : TimerManager, ISimTimeObserver
	{
		public SimTimerManager()
		{
			Service.Set<SimTimerManager>(this);
			Service.Get<SimTimeEngine>().RegisterSimTimeObserver(this);
		}

		public uint CreateSimTimer(uint delay, bool repeat, TimerDelegate callback, object cookie)
		{
			return base.CreateTimer(delay, repeat, callback, cookie);
		}

		public void KillSimTimer(uint id)
		{
			base.KillTimer(id);
		}

		public void TriggerKillSimTimer(uint id)
		{
			base.TriggerKillTimer(id);
		}

		public void OnSimTime(uint dt)
		{
			base.OnDeltaTime(dt);
		}
	}
}
