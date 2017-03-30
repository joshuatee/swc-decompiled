using System;
using System.Collections.Generic;

namespace StaRTS.Utils.Scheduling
{
	public class TimerList
	{
		public List<Timer> Timers;

		public TimerList()
		{
			this.Timers = new List<Timer>();
		}

		public virtual int Add(Timer timer)
		{
			uint timeFire = timer.TimeFire;
			int i = 0;
			int count = this.Timers.Count;
			while (i < count)
			{
				if (timeFire < this.Timers[i].TimeFire)
				{
					this.Timers.Insert(i, timer);
					return i;
				}
				i++;
			}
			this.Timers.Add(timer);
			return this.Timers.Count - 1;
		}

		public int ReprioritizeFirst()
		{
			Timer timer = this.Timers[0];
			uint timeFire = timer.TimeFire;
			int i = 1;
			int count = this.Timers.Count;
			while (i < count)
			{
				Timer timer2 = this.Timers[i];
				if (timeFire < timer2.TimeFire)
				{
					return i - 1;
				}
				this.Timers[i] = timer;
				this.Timers[i - 1] = timer2;
				i++;
			}
			return this.Timers.Count - 1;
		}

		public void Rebase(uint amount)
		{
			int i = 0;
			int count = this.Timers.Count;
			while (i < count)
			{
				this.Timers[i].DecTimeFire(amount);
				i++;
			}
		}
	}
}
