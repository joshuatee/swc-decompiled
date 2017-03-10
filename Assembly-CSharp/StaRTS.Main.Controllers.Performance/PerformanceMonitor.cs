using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Performance
{
	public class PerformanceMonitor : IViewFrameTimeObserver
	{
		private const float SECONDS_PER_SAMPLE = 1f;

		private const float SECONDS_PER_PEAK = 5f;

		private const int NUM_AVERAGED_SAMPLES = 1;

		private FrameSample[] samples;

		private int nextSample;

		private float avgFPS;

		private float peakFrameTime;

		private int currentFrames;

		private float currentSeconds;

		private float currentSecondsForPeak;

		protected List<IPerformanceObserver> fpsObservers;

		protected List<IPerformanceObserver> memObservers;

		protected IPerformanceObserver deviceMemObservers;

		private MutableIterator miterFPS;

		private MutableIterator miterMem;

		public PerformanceMonitor()
		{
			Service.Set<PerformanceMonitor>(this);
			this.samples = new FrameSample[1];
			this.Reset();
			this.fpsObservers = new List<IPerformanceObserver>();
			this.memObservers = new List<IPerformanceObserver>();
			this.miterFPS = new MutableIterator();
			this.miterMem = new MutableIterator();
		}

		public void RegisterFPSObserver(IPerformanceObserver observer)
		{
			if (this.fpsObservers.IndexOf(observer) < 0)
			{
				if (this.fpsObservers.Count == 0)
				{
					this.Enable(true);
				}
				this.fpsObservers.Add(observer);
				observer.OnPerformanceFPS(0f);
				observer.OnPerformanceFPeak(0u);
			}
		}

		public void RegisterMemObserver(IPerformanceObserver observer)
		{
			if (this.memObservers.IndexOf(observer) < 0)
			{
				if (this.memObservers.Count == 0)
				{
					this.Enable(true);
				}
				this.memObservers.Add(observer);
				observer.OnPerformanceMemRsvd(0u);
				observer.OnPerformanceMemUsed(0u);
				observer.OnPerformanceMemTexture(0u);
				observer.OnPerformanceMemMesh(0u);
				observer.OnPerformanceMemAudio(0u);
				observer.OnPerformanceMemAnimation(0u);
				observer.OnPerformanceMemMaterials(0u);
				observer.OnPerformanceDeviceMemUsage(0L);
			}
		}

		public void UnregisterFPSObserver(IPerformanceObserver observer)
		{
			int num = this.fpsObservers.IndexOf(observer);
			if (num >= 0)
			{
				this.fpsObservers.RemoveAt(num);
				this.miterFPS.OnRemove(num);
			}
			if (this.fpsObservers.Count + this.memObservers.Count == 0)
			{
				this.Enable(false);
			}
		}

		public void UnregisterMemObserver(IPerformanceObserver observer)
		{
			int num = this.memObservers.IndexOf(observer);
			if (num >= 0)
			{
				this.memObservers.RemoveAt(num);
				this.miterMem.OnRemove(num);
			}
			if (this.fpsObservers.Count + this.memObservers.Count == 0)
			{
				this.Enable(false);
			}
		}

		private void Enable(bool enable)
		{
			if (enable)
			{
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
				return;
			}
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			this.Reset();
		}

		private void Reset()
		{
			for (int i = 0; i < 1; i++)
			{
				this.samples[i].frames = 0;
				this.samples[i].seconds = 0f;
			}
			this.nextSample = 0;
			this.avgFPS = 0f;
			this.peakFrameTime = 0f;
			this.currentFrames = 0;
			this.currentSeconds = 0f;
			this.currentSecondsForPeak = 0f;
		}

		public virtual void OnViewFrameTime(float dt)
		{
			this.currentFrames++;
			this.currentSeconds += dt;
			this.currentSecondsForPeak += dt;
			if (this.currentSeconds >= 1f)
			{
				this.SaveSample();
				this.ComputeFPS();
				this.SendFPS(this.avgFPS);
			}
			if (this.currentSecondsForPeak >= 5f)
			{
				uint fpeak = (uint)(this.peakFrameTime * 1000f);
				this.SendFPeak(fpeak);
				this.SendMemStats();
				this.peakFrameTime = 0f;
				this.currentSecondsForPeak = 0f;
			}
			if (dt > this.peakFrameTime)
			{
				this.peakFrameTime = dt;
			}
		}

		private void ComputeFPS()
		{
			int num = 0;
			float num2 = 0f;
			for (int i = 0; i < 1; i++)
			{
				num += this.samples[i].frames;
				num2 += this.samples[i].seconds;
			}
			if (num2 > 0f)
			{
				this.avgFPS = (float)num / num2;
			}
		}

		private void SaveSample()
		{
			this.samples[this.nextSample].frames = this.currentFrames;
			this.samples[this.nextSample].seconds = this.currentSeconds;
			int num = this.nextSample + 1;
			this.nextSample = num;
			if (num == 1)
			{
				this.nextSample = 0;
			}
			this.currentFrames = 0;
			this.currentSeconds = 0f;
		}

		private long CaptureDeviceMemUsage()
		{
			return 0L;
		}

		private void SendFPS(float fps)
		{
			this.miterFPS.Init(this.fpsObservers);
			while (this.miterFPS.Active())
			{
				IPerformanceObserver performanceObserver = this.fpsObservers[this.miterFPS.Index];
				performanceObserver.OnPerformanceFPS(fps);
				this.miterFPS.Next();
			}
			this.miterFPS.Reset();
		}

		private void SendFPeak(uint fpeak)
		{
			this.miterFPS.Init(this.fpsObservers);
			while (this.miterFPS.Active())
			{
				IPerformanceObserver performanceObserver = this.fpsObservers[this.miterFPS.Index];
				performanceObserver.OnPerformanceFPeak(fpeak);
				this.miterFPS.Next();
			}
			this.miterFPS.Reset();
		}

		public uint GetTotalMemoryUsed()
		{
			uint monoHeapSize = Profiler.GetMonoHeapSize();
			uint monoUsedSize = Profiler.GetMonoUsedSize();
			uint runtimeMemorySize = this.GetRuntimeMemorySize<Texture>();
			uint runtimeMemorySize2 = this.GetRuntimeMemorySize<Mesh>();
			uint runtimeMemorySize3 = this.GetRuntimeMemorySize<AudioClip>();
			uint runtimeMemorySize4 = this.GetRuntimeMemorySize<AnimationClip>();
			uint runtimeMemorySize5 = this.GetRuntimeMemorySize<Material>();
			return monoHeapSize + monoUsedSize + runtimeMemorySize + runtimeMemorySize3 + runtimeMemorySize2 + runtimeMemorySize4 + runtimeMemorySize5;
		}

		private uint GetRuntimeMemorySize<T>()
		{
			uint num = 0u;
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(T));
			UnityEngine.Object[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				UnityEngine.Object o = array2[i];
				num += (uint)Profiler.GetRuntimeMemorySize(o);
			}
			return num;
		}

		private void SendMemStats()
		{
			if (!Debug.isDebugBuild || this.memObservers.Count == 0)
			{
				return;
			}
			uint monoHeapSize = Profiler.GetMonoHeapSize();
			uint monoUsedSize = Profiler.GetMonoUsedSize();
			uint runtimeMemorySize = this.GetRuntimeMemorySize<Texture>();
			uint runtimeMemorySize2 = this.GetRuntimeMemorySize<Mesh>();
			uint runtimeMemorySize3 = this.GetRuntimeMemorySize<AudioClip>();
			uint runtimeMemorySize4 = this.GetRuntimeMemorySize<AnimationClip>();
			uint runtimeMemorySize5 = this.GetRuntimeMemorySize<Material>();
			long memory = this.CaptureDeviceMemUsage();
			this.miterMem.Init(this.memObservers);
			while (this.miterMem.Active())
			{
				IPerformanceObserver performanceObserver = this.memObservers[this.miterMem.Index];
				performanceObserver.OnPerformanceMemRsvd(monoHeapSize);
				performanceObserver.OnPerformanceMemUsed(monoUsedSize);
				performanceObserver.OnPerformanceMemTexture(runtimeMemorySize);
				performanceObserver.OnPerformanceMemMesh(runtimeMemorySize2);
				performanceObserver.OnPerformanceMemAudio(runtimeMemorySize3);
				performanceObserver.OnPerformanceMemAnimation(runtimeMemorySize4);
				performanceObserver.OnPerformanceMemMaterials(runtimeMemorySize5);
				performanceObserver.OnPerformanceDeviceMemUsage(memory);
				this.miterMem.Next();
			}
			this.miterMem.Reset();
		}

		protected internal PerformanceMonitor(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).CaptureDeviceMemUsage());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).ComputeFPS();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).Enable(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).RegisterFPSObserver((IPerformanceObserver)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).RegisterMemObserver((IPerformanceObserver)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).SaveSample();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).SendFPS(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).SendMemStats();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).UnregisterFPSObserver((IPerformanceObserver)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((PerformanceMonitor)GCHandledObjects.GCHandleToObject(instance)).UnregisterMemObserver((IPerformanceObserver)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
