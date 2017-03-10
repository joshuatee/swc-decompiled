using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2552 : $UnityType
	{
		public unsafe $UnityType2552()
		{
			*(UnityEngine.Internal.$Metadata.data + 995060) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IServiceWrapper)GCHandledObjects.GCHandleToObject(instance)).Unset();
			return -1L;
		}
	}
}
