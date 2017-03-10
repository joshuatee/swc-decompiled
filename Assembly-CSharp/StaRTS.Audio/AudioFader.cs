using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Audio
{
	public class AudioFader : IViewFrameTimeObserver
	{
		private float originalVolume;

		private float age;

		private float constantTime;

		private AudioSource source;

		private AudioTypeVO nextType;

		public AudioFader(AudioSource source)
		{
			if (source == null)
			{
				Service.Get<StaRTSLogger>().Warn("Source is null");
			}
			this.constantTime = GameConstants.FADE_OUT_CONSTANT_LENGTH;
			this.originalVolume = source.volume;
			this.source = source;
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		public void FadeSound(float age)
		{
			if (this.source == null)
			{
				this.UnregisterFrameTimeObserver();
				return;
			}
			if (age <= this.constantTime)
			{
				float t = age / this.constantTime;
				this.source.volume = Mathf.Lerp(this.originalVolume, 0f, t);
				return;
			}
			this.source.Stop();
			this.source.volume = this.originalVolume;
			this.FadeOutComplete();
		}

		public void QueueNextAudio(AudioTypeVO type)
		{
			this.nextType = type;
		}

		public void UnregisterFrameTimeObserver()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public void FadeOutComplete()
		{
			AudioManager audioManager = Service.Get<AudioManager>();
			this.UnregisterFrameTimeObserver();
			audioManager.FadeOutComplete(this.source);
			if (this.nextType != null)
			{
				audioManager.PlayAudio(this.nextType.AssetName);
			}
			this.source = null;
			this.nextType = null;
		}

		public void OnViewFrameTime(float dt)
		{
			this.age += dt;
			this.FadeSound(this.age);
		}

		protected internal AudioFader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AudioFader)GCHandledObjects.GCHandleToObject(instance)).FadeOutComplete();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AudioFader)GCHandledObjects.GCHandleToObject(instance)).FadeSound(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AudioFader)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AudioFader)GCHandledObjects.GCHandleToObject(instance)).QueueNextAudio((AudioTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AudioFader)GCHandledObjects.GCHandleToObject(instance)).UnregisterFrameTimeObserver();
			return -1L;
		}
	}
}
