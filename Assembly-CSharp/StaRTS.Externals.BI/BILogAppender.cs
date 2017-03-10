using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.BI
{
	public class BILogAppender : ILogAppender
	{
		private UnityLogAppender unityLogAppender;

		public BILogAppender(UnityLogAppender unityLogAppender)
		{
			this.unityLogAppender = unityLogAppender;
			UnityUtils.RegisterLogCallback(typeof(BILogAppender).get_Name(), new UnityUtils.OnUnityLogCallback(this.HandleUnityLog));
		}

		public void AddLogMessage(LogEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.LogBIMessage(entry.Level, entry.Message);
		}

		private void LogBIMessage(LogLevel logLevel, string message)
		{
			if (logLevel == LogLevel.Debug)
			{
				return;
			}
			if (Service.IsSet<BILoggingController>())
			{
				Service.Get<BILoggingController>().TrackError(logLevel, message);
			}
		}

		private void HandleUnityLog(string logString, string stackTrace, LogType type)
		{
			if (type == LogType.Exception)
			{
				this.LogBIMessage(LogLevel.Error, logString);
				return;
			}
			if (this.unityLogAppender == null || !this.unityLogAppender.CurrentlyLogging)
			{
				if (type == LogType.Error)
				{
					this.LogBIMessage(LogLevel.Error, logString);
					return;
				}
				if (type != LogType.Warning)
				{
					return;
				}
				this.LogBIMessage(LogLevel.Warn, logString);
			}
		}

		protected internal BILogAppender(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BILogAppender)GCHandledObjects.GCHandleToObject(instance)).AddLogMessage((LogEntry)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BILogAppender)GCHandledObjects.GCHandleToObject(instance)).HandleUnityLog(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (LogType)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BILogAppender)GCHandledObjects.GCHandleToObject(instance)).LogBIMessage((LogLevel)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}
