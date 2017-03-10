using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.BI
{
	public class StepTimingController
	{
		private Dictionary<string, float> timeStamps;

		public StepTimingController()
		{
			Service.Set<StepTimingController>(this);
			this.timeStamps = new Dictionary<string, float>();
		}

		public void StartStep(string stepName)
		{
			float realTimeSinceStartUpInMilliseconds = DateUtils.GetRealTimeSinceStartUpInMilliseconds();
			if (this.timeStamps.ContainsKey(stepName))
			{
				this.timeStamps[stepName] = realTimeSinceStartUpInMilliseconds;
				return;
			}
			this.timeStamps.Add(stepName, realTimeSinceStartUpInMilliseconds);
		}

		public void IntermediaryStep(string stepName, BILog log)
		{
			this.AddElapsedTime(stepName, log);
		}

		public void EndStep(string stepName, BILog log)
		{
			this.AddElapsedTime(stepName, log);
			this.timeStamps.Remove(stepName);
		}

		public bool IsStepStarted(string stepName)
		{
			return this.timeStamps.ContainsKey(stepName);
		}

		private void AddElapsedTime(string stepName, BILog log)
		{
			float realTimeSinceStartUpInMilliseconds = DateUtils.GetRealTimeSinceStartUpInMilliseconds();
			if (!this.timeStamps.ContainsKey(stepName))
			{
				this.timeStamps.Add(stepName, realTimeSinceStartUpInMilliseconds);
			}
			float num = this.timeStamps[stepName];
			log.AddParam("elapsed_time_ms", ((int)(realTimeSinceStartUpInMilliseconds - num)).ToString());
		}

		protected internal StepTimingController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StepTimingController)GCHandledObjects.GCHandleToObject(instance)).AddElapsedTime(Marshal.PtrToStringUni(*(IntPtr*)args), (BILog)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((StepTimingController)GCHandledObjects.GCHandleToObject(instance)).EndStep(Marshal.PtrToStringUni(*(IntPtr*)args), (BILog)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StepTimingController)GCHandledObjects.GCHandleToObject(instance)).IntermediaryStep(Marshal.PtrToStringUni(*(IntPtr*)args), (BILog)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StepTimingController)GCHandledObjects.GCHandleToObject(instance)).IsStepStarted(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((StepTimingController)GCHandledObjects.GCHandleToObject(instance)).StartStep(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
