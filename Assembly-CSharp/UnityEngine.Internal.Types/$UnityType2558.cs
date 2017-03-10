using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2558 : $UnityType
	{
		public unsafe $UnityType2558()
		{
			*(UnityEngine.Internal.$Metadata.data + 995620) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ILogAppender)GCHandledObjects.GCHandleToObject(instance)).AddLogMessage((LogEntry)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
