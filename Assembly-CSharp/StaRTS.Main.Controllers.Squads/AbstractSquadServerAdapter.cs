using StaRTS.Main.Models.Squads;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Squads
{
	public abstract class AbstractSquadServerAdapter
	{
		protected SquadController.SquadMsgsCallback callback;

		private uint timerId;

		protected float pollFrequency;

		protected List<SquadMsg> list;

		public bool Enabled
		{
			get;
			private set;
		}

		public AbstractSquadServerAdapter()
		{
			this.timerId = 0u;
			this.list = new List<SquadMsg>();
			this.Enabled = false;
		}

		public virtual void Enable(SquadController.SquadMsgsCallback callback, float pollFrequency)
		{
			if (this.Enabled || callback == null || pollFrequency <= 0f)
			{
				return;
			}
			this.callback = callback;
			this.pollFrequency = pollFrequency;
			this.Enabled = true;
			this.Poll();
		}

		public void AdjustPollFrequency(float pollFrequency)
		{
			if (pollFrequency > 0f)
			{
				this.pollFrequency = pollFrequency;
				this.ResetPollTimer();
			}
		}

		protected void OnPollFinished(object response)
		{
			this.list.Clear();
			this.PopulateSquadMsgsReceived(response);
			if (this.callback != null)
			{
				this.callback(this.list);
			}
			if (this.timerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.timerId);
				this.timerId = 0u;
			}
			this.timerId = Service.Get<ViewTimerManager>().CreateViewTimer(this.pollFrequency, false, new TimerDelegate(this.OnPollTimer), null);
		}

		private void OnPollTimer(uint timerId, object cookie)
		{
			this.timerId = 0u;
			this.Poll();
		}

		public void ResetPollTimer()
		{
			if (this.timerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.timerId);
				this.timerId = Service.Get<ViewTimerManager>().CreateViewTimer(this.pollFrequency, false, new TimerDelegate(this.OnPollTimer), null);
			}
		}

		public void DisableTimer()
		{
			if (this.timerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.timerId);
				this.timerId = 0u;
			}
			this.Enabled = false;
		}

		public virtual void Disable()
		{
			this.callback = null;
			this.list.Clear();
			this.DisableTimer();
		}

		protected abstract void Poll();

		protected abstract void PopulateSquadMsgsReceived(object response);

		protected internal AbstractSquadServerAdapter(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).AdjustPollFrequency(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Disable();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).DisableTimer();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Enable((SquadController.SquadMsgsCallback)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).OnPollFinished(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Poll();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).PopulateSquadMsgsReceived(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).ResetPollTimer();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AbstractSquadServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Enabled = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
