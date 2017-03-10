using StaRTS.Main.Views.UserInput;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2486 : $UnityType
	{
		public unsafe $UnityType2486()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 975208) = ldftn($Invoke0);
			*(data + 975236) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IBackButtonManager)GCHandledObjects.GCHandleToObject(instance)).RegisterBackButtonHandler((IBackButtonHandler)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IBackButtonManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterBackButtonHandler((IBackButtonHandler)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
