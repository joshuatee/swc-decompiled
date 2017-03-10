using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Squads
{
	public abstract class AbstractSquadWarBoardInfoElement : AbstractSquadWarBoardElement, IEventObserver
	{
		private const int WIDGET_DEPTH = -1;

		private const float VISIBILITY_FADE_START = 14f;

		private const float VISIBILITY_FADE_END = 18f;

		public AbstractSquadWarBoardInfoElement(string assetName, Transform transformToTrack) : base(assetName)
		{
			this.transformToTrack = transformToTrack;
		}

		protected abstract void SetupView();

		protected abstract void UpdateView();

		protected override void OnScreenLoaded(object cookie)
		{
			base.OnScreenLoaded(cookie);
			base.WidgetDepth = -1;
			this.SetupView();
			this.UpdateVisibility();
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			this.OnViewFrameTime(0f);
		}

		public void UpdateVisibility()
		{
			float num = this.transformToTrack.position.z;
			num = Mathf.Clamp(num, 14f, 18f);
			float proportion = 1f - (num - 14f) / 4f;
			this.FadeElements(proportion);
		}

		protected virtual void FadeElements(float proportion)
		{
		}

		public override void Destroy()
		{
			base.Destroy();
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		protected internal AbstractSquadWarBoardInfoElement(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractSquadWarBoardInfoElement)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractSquadWarBoardInfoElement)GCHandledObjects.GCHandleToObject(instance)).FadeElements(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractSquadWarBoardInfoElement)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractSquadWarBoardInfoElement)GCHandledObjects.GCHandleToObject(instance)).SetupView();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractSquadWarBoardInfoElement)GCHandledObjects.GCHandleToObject(instance)).UpdateView();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractSquadWarBoardInfoElement)GCHandledObjects.GCHandleToObject(instance)).UpdateVisibility();
			return -1L;
		}
	}
}
