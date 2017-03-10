using Net.RichardLord.Ash.Core;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class ViewFader : IViewFrameTimeObserver
	{
		private List<AbstractFadingView> fadingEntities;

		private List<AbstractFadingView> completedFaders;

		public ViewFader()
		{
			this.fadingEntities = new List<AbstractFadingView>();
			this.completedFaders = new List<AbstractFadingView>();
		}

		public void FadeOut(Entity entity, float delay, float fadeTime, FadingDelegate onFadeStart, FadingDelegate onFadeComplete)
		{
			this.MaybeRegisterForViewTime();
			FadingEntity item = new FadingEntity(entity, delay, fadeTime, onFadeStart, onFadeComplete);
			this.fadingEntities.Add(item);
		}

		public void FadeOut(GameObject gameObj, float delay, float fadeTime, FadingDelegate onFadeStart, FadingDelegate onFadeComplete)
		{
			this.MaybeRegisterForViewTime();
			FadingGameObject item = new FadingGameObject(gameObj, delay, fadeTime, onFadeStart, onFadeComplete);
			this.fadingEntities.Add(item);
		}

		private void MaybeRegisterForViewTime()
		{
			if (this.fadingEntities.Count == 0)
			{
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			}
		}

		private void MaybeUnregisterFromViewTime()
		{
			if (this.fadingEntities.Count == 0)
			{
				Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			}
		}

		public void OnViewFrameTime(float dt)
		{
			int num = this.fadingEntities.Count;
			if (num != 0)
			{
				for (int i = 0; i < num; i++)
				{
					AbstractFadingView abstractFadingView = this.fadingEntities[i];
					if (abstractFadingView.Fade(dt))
					{
						this.fadingEntities.RemoveAt(i);
						i--;
						num--;
						this.completedFaders.Add(abstractFadingView);
					}
				}
				num = this.completedFaders.Count;
				if (num != 0)
				{
					this.MaybeUnregisterFromViewTime();
					for (int j = 0; j < num; j++)
					{
						this.completedFaders[j].Complete();
					}
					this.completedFaders.Clear();
				}
			}
		}

		protected internal ViewFader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ViewFader)GCHandledObjects.GCHandleToObject(instance)).FadeOut((Entity)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(float*)(args + 2), (FadingDelegate)GCHandledObjects.GCHandleToObject(args[3]), (FadingDelegate)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ViewFader)GCHandledObjects.GCHandleToObject(instance)).FadeOut((GameObject)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(float*)(args + 2), (FadingDelegate)GCHandledObjects.GCHandleToObject(args[3]), (FadingDelegate)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ViewFader)GCHandledObjects.GCHandleToObject(instance)).MaybeRegisterForViewTime();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ViewFader)GCHandledObjects.GCHandleToObject(instance)).MaybeUnregisterFromViewTime();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ViewFader)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}
	}
}
