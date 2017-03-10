using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Utils.Diagnostics
{
	public abstract class BaseLogAppender : ILogAppender
	{
		protected LogEntry entry;

		protected abstract void Trace(string formattedMessage);

		public void AddLogMessage(LogEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.entry = entry;
			string formattedMessage = string.Format("{0} {1}: {2}", new object[]
			{
				entry.Timestamp,
				entry.Level,
				entry.Message
			});
			this.Trace(formattedMessage);
		}

		protected BaseLogAppender()
		{
		}

		protected internal BaseLogAppender(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BaseLogAppender)GCHandledObjects.GCHandleToObject(instance)).AddLogMessage((LogEntry)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BaseLogAppender)GCHandledObjects.GCHandleToObject(instance)).Trace(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
