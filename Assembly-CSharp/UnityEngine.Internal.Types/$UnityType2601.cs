using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2601 : $UnityType
	{
		public unsafe $UnityType2601()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 999988) = ldftn($Invoke0);
			*(data + 1000016) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
