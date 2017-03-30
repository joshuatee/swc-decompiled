using System;
using System.Collections.Generic;

namespace StaRTS.Utils.Scheduling
{
	public class TimerManager
	{
		public const uint ONE_DAY = 86400000u;

		public const uint MAX_DELAY_MILLIS = 432000000u;

		public const uint MAX_DELAY_SECONDS = 432000u;

		private const uint REBASE_TIME = 60000u;

		protected const uint INFINITY = 4294967295u;

		private uint idLast;

		private TimerList timers;

		private uint lastTimeFire;

		private int lastIndex;

		private uint timeNow;

		private uint timeNext;

		public TimerManager()
		{
			this.idLast = 0u;
			this.timers = new TimerList();
			this.lastTimeFire = 4294967295u;
			this.lastIndex = -1;
			this.timeNow = 0u;
			this.timeNext = 4294967295u;
		}

		protected uint CreateTimer(uint delay, bool repeat, TimerDelegate callback, object cookie)
		{
			if (delay > 432000000u)
			{
				throw new Exception(string.Format("Timer delay {0} exceeds maximum {1}", delay, 432000000u));
			}
			if (delay == 0u)
			{
				delay = 1u;
			}
			if (callback == null)
			{
				throw new Exception("Null timer callback not supported nor useful");
			}
			uint next = TimerId.GetNext(ref this.idLast);
			Timer timer = new Timer(next, delay, repeat, callback, cookie, this.timeNow);
			uint num = this.timeNow + delay;
			if (num == this.lastTimeFire)
			{
				this.timers.Timers.Insert(++this.lastIndex, timer);
			}
			else
			{
				this.lastTimeFire = num;
				this.lastIndex = this.timers.Add(timer);
			}
			if (this.lastIndex == 0)
			{
				this.timeNext = num;
			}
			return next;
		}

		protected void KillTimer(uint id)
		{
			List<Timer> list = this.timers.Timers;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (list[i].Id == id)
				{
					this.KillTimerAt(i);
					break;
				}
				i++;
			}
		}

		protected void TriggerKillTimer(uint id)
		{
			List<Timer> list = this.timers.Timers;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Timer timer = list[i];
				if (timer.Id == id)
				{
					timer.Callback(timer.Id, timer.Cookie);
					break;
				}
				i++;
			}
			this.KillTimer(id);
		}

		private void KillTimerAt(int i)
		{
			this.lastTimeFire = 4294967295u;
			this.lastIndex = -1;
			Timer timer = this.timers.Timers[i];
			timer.Kill();
			this.timers.Timers.RemoveAt(i);
			if (i == 0)
			{
				this.SetTimeNext();
			}
		}

		public void EnsureTimerKilled(ref uint id)
		{
			if (id != 0u)
			{
				this.KillTimer(id);
				id = 0u;
			}
		}

		private void SetTimeNext()
		{
			this.timeNext = ((this.timers.Timers.Count != 0) ? this.timers.Timers[0].TimeFire : 4294967295u);
		}

		protected void OnDeltaTime(uint dt)
		{
			this.timeNow += dt;
			if (this.timeNow >= 60000u)
			{
				if (this.timeNext == 4294967295u)
				{
					this.timeNow -= 60000u;
				}
				else if (this.timeNext >= 60000u)
				{
					this.timeNow -= 60000u;
					this.timeNext -= 60000u;
					this.timers.Rebase(60000u);
				}
			}
			this.lastTimeFire = 4294967295u;
			this.lastIndex = -1;
			while (this.timeNext <= this.timeNow)
			{
				Timer timer = this.timers.Timers[0];
				if (timer.TimeFire > this.timeNow)
				{
					break;
				}
				timer.Callback(timer.Id, timer.Cookie);
				if (timer.Repeat)
				{
					while (!timer.IsKilled)
					{
						if (timer.IncTimeFireByDelay() > this.timeNow)
						{
							this.timers.ReprioritizeFirst();
							this.SetTimeNext();
							break;
						}
						timer.Callback(timer.Id, timer.Cookie);
					}
				}
				else if (!timer.IsKilled)
				{
					this.KillTimer(timer.Id);
				}
			}
		}
	}
}
