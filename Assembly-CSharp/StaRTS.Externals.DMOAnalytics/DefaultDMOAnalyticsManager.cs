using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.DMOAnalytics
{
	public class DefaultDMOAnalyticsManager : IDMOAnalyticsManager
	{
		public DefaultDMOAnalyticsManager()
		{
		}

		public void LogEvent(string appEvent)
		{
		}

		public void LogAppStart()
		{
		}

		public void LogAppEnd()
		{
		}

		public void LogAppForeground()
		{
		}

		public void LogAppBackground()
		{
		}

		public void LogEventWithContext(string eventName, string parameters)
		{
		}

		public void FlushAnalyticsQueue()
		{
		}

		public void LogGameAction(string parameters)
		{
		}

		public void LogMoneyAction(string parameters)
		{
		}

		public void SetDebugLogging(bool isEnable)
		{
		}

		public void SetCanUseNetwork(bool isEnable)
		{
		}

		protected internal DefaultDMOAnalyticsManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).FlushAnalyticsQueue();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogAppBackground();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogAppEnd();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogAppForeground();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogAppStart();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogEvent(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogEventWithContext(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogGameAction(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogMoneyAction(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).SetCanUseNetwork(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((DefaultDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).SetDebugLogging(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
