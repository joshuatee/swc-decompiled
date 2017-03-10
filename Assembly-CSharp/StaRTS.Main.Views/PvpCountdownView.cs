using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Views.UX;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views
{
	public class PvpCountdownView : IViewFrameTimeObserver
	{
		private float age;

		private float duration;

		private HUD hud;

		private Action onCountdownComplete;

		private bool running;

		public PvpCountdownView(Action onCountdownComplete)
		{
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			this.age = 0f;
			this.duration = GameConstants.PVP_MATCH_COUNTDOWN;
			this.hud = Service.Get<UXController>().HUD;
			this.hud.ShowCountdown(true);
			this.onCountdownComplete = onCountdownComplete;
			this.running = true;
		}

		public void OnViewFrameTime(float dt)
		{
			if (!this.running)
			{
				return;
			}
			this.age += dt;
			if (this.age > this.duration)
			{
				this.Destroy();
				if (this.onCountdownComplete != null)
				{
					this.onCountdownComplete.Invoke();
				}
				return;
			}
			float remaining = this.duration - this.age;
			this.hud.SetCountdownTime(remaining, this.duration);
		}

		public void Destroy()
		{
			this.hud.ShowCountdown(false);
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public void Pause()
		{
			this.running = false;
		}

		public void Resume()
		{
			this.running = true;
		}

		protected internal PvpCountdownView(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PvpCountdownView)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PvpCountdownView)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PvpCountdownView)GCHandledObjects.GCHandleToObject(instance)).Pause();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PvpCountdownView)GCHandledObjects.GCHandleToObject(instance)).Resume();
			return -1L;
		}
	}
}
