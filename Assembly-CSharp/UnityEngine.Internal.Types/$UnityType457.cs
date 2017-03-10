using StaRTS.Externals.Manimal;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType457 : $UnityType
	{
		public unsafe $UnityType457()
		{
			*(UnityEngine.Internal.$Metadata.data + 453372) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IResponseHandler)GCHandledObjects.GCHandleToObject(instance)).SendMessages((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
