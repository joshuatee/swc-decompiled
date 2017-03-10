using StaRTS.Main.Models;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Audio
{
	public class AudioEffectLoop : IEventObserver, IViewFrameTimeObserver
	{
		private float duration;

		private List<StrIntPair> effectIds;

		public AudioEffectLoop(float duration, List<StrIntPair> effectIds)
		{
			if (duration <= 0f)
			{
				return;
			}
			this.duration = duration;
			this.effectIds = effectIds;
			this.PlayEffect();
		}

		private void PlayEffect()
		{
			AudioManager audioManager = Service.Get<AudioManager>();
			float num = audioManager.PlayAudio(audioManager.GetRandomClip(this.effectIds));
			if (num == -1f)
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.PlayedLoadedOnDemandAudio, EventPriority.Default);
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
				return;
			}
			this.OnEffectPlayed(num);
		}

		private void OnEffectPlayed(float clipLength)
		{
			if (clipLength == 0f)
			{
				return;
			}
			this.duration -= clipLength;
			if (this.duration > 0f)
			{
				Service.Get<ViewTimerManager>().CreateViewTimer(clipLength, false, new TimerDelegate(this.OnAudioEffectLoopTimer), null);
			}
		}

		private void OnAudioEffectLoopTimer(uint id, object cookie)
		{
			this.PlayEffect();
		}

		public void OnViewFrameTime(float dt)
		{
			this.duration -= dt;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.PlayedLoadedOnDemandAudio)
			{
				Service.Get<EventManager>().UnregisterObserver(this, id);
				Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
				float clipLength = (float)cookie;
				this.OnEffectPlayed(clipLength);
			}
			return EatResponse.NotEaten;
		}

		protected internal AudioEffectLoop(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AudioEffectLoop)GCHandledObjects.GCHandleToObject(instance)).OnEffectPlayed(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioEffectLoop)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AudioEffectLoop)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AudioEffectLoop)GCHandledObjects.GCHandleToObject(instance)).PlayEffect();
			return -1L;
		}
	}
}
