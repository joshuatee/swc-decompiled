using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Utils.Diagnostics
{
	public class StaRTSLogger
	{
		public LogLevel ErrorLevels;

		private List<ILogAppender> logAppenders;

		private List<LogEntry> pendingEntries;

		private bool started;

		public StaRTSLogger()
		{
			this.logAppenders = new List<ILogAppender>();
			base..ctor();
			Service.Set<StaRTSLogger>(this);
		}

		public void AddAppender(ILogAppender appender)
		{
		}

		public void Start()
		{
		}

		public void Start(List<ILogAppender> appenders)
		{
		}

		public void FlushPendingEntries()
		{
		}

		protected void ProcessEntry(LogEntry entry)
		{
		}

		public void Debug(string message)
		{
		}

		public void DebugFormat(string message, params object[] args)
		{
		}

		public void Warn(string message)
		{
		}

		public void WarnFormat(string message, params object[] args)
		{
		}

		public void Error(string message)
		{
		}

		public void ErrorFormat(string message, params object[] args)
		{
		}

		protected internal StaRTSLogger(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).AddAppender((ILogAppender)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).Debug(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).DebugFormat(Marshal.PtrToStringUni(*(IntPtr*)args), (object[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).Error(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).ErrorFormat(Marshal.PtrToStringUni(*(IntPtr*)args), (object[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).FlushPendingEntries();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).ProcessEntry((LogEntry)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).Start((List<ILogAppender>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).Warn(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((StaRTSLogger)GCHandledObjects.GCHandleToObject(instance)).WarnFormat(Marshal.PtrToStringUni(*(IntPtr*)args), (object[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]));
			return -1L;
		}
	}
}
