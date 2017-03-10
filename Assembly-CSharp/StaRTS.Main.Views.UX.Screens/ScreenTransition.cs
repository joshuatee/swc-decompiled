using StaRTS.Main.Views.Cameras;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ScreenTransition : IViewFrameTimeObserver
	{
		private const int WAIT_FRAMES = 2;

		private string animationToPlay;

		private Animation animation;

		private OnScreenTransitionComplete onComplete;

		private int waitFrames;

		private bool disabledColliders;

		public ScreenTransition(Animation a)
		{
			this.animation = a;
		}

		public void PlayTransition(string transitionName, OnScreenTransitionComplete onTransitionComplete, bool delay)
		{
			this.AnimationComplete();
			this.onComplete = onTransitionComplete;
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			this.animationToPlay = transitionName;
			if (delay)
			{
				this.waitFrames = 2;
				return;
			}
			this.waitFrames = 0;
			this.AnimationPlay();
		}

		public void Destroy()
		{
			this.AnimationComplete();
			ScreenTransition.ForceAlpha(this.animation, 0f);
		}

		public static void ForceAlpha(Animation animation, float alpha)
		{
			animation.gameObject.GetComponent<UIPanel>().alpha = alpha;
			animation.gameObject.GetComponent<AnimatedAlpha>().alpha = alpha;
		}

		public void OnViewFrameTime(float dt)
		{
			if (this.waitFrames == 0)
			{
				if (!this.animation.isPlaying)
				{
					this.AnimationComplete();
					return;
				}
			}
			else
			{
				int num = this.waitFrames - 1;
				this.waitFrames = num;
				if (num == 0)
				{
					this.AnimationPlay();
				}
			}
		}

		private void AnimationComplete()
		{
			this.AnimationStop();
			if (Service.IsSet<ViewTimeEngine>())
			{
				Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			}
			else if (Service.IsSet<StaRTSLogger>())
			{
				Service.Get<StaRTSLogger>().Error("ScreenTransition.AnimationComplete: ViewTimeEngine is not set in Service");
			}
			if (this.disabledColliders)
			{
				if (Service.IsSet<CameraManager>())
				{
					if (!Service.Get<CameraManager>().WipeCamera.IsRendering())
					{
						Service.Get<CameraManager>().UXCamera.ReceiveEvents = true;
					}
				}
				else if (Service.IsSet<StaRTSLogger>())
				{
					Service.Get<StaRTSLogger>().Error("ScreenTransition.AnimationComplete: CameraManager is not set in Service");
				}
				this.disabledColliders = false;
			}
			if (this.onComplete != null)
			{
				OnScreenTransitionComplete onScreenTransitionComplete = this.onComplete;
				this.onComplete = null;
				onScreenTransitionComplete();
			}
		}

		private void AnimationPlay()
		{
			this.AnimationStop();
			if (!this.disabledColliders)
			{
				Service.Get<CameraManager>().UXCamera.ReceiveEvents = false;
				this.disabledColliders = true;
			}
			this.animation.Play(this.animationToPlay);
			this.animationToPlay = null;
		}

		private void AnimationStop()
		{
			if (this.animation.isPlaying)
			{
				this.animation.Stop();
			}
		}

		protected internal ScreenTransition(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ScreenTransition)GCHandledObjects.GCHandleToObject(instance)).AnimationComplete();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ScreenTransition)GCHandledObjects.GCHandleToObject(instance)).AnimationPlay();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ScreenTransition)GCHandledObjects.GCHandleToObject(instance)).AnimationStop();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ScreenTransition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			ScreenTransition.ForceAlpha((Animation)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ScreenTransition)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ScreenTransition)GCHandledObjects.GCHandleToObject(instance)).PlayTransition(Marshal.PtrToStringUni(*(IntPtr*)args), (OnScreenTransitionComplete)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}
	}
}
