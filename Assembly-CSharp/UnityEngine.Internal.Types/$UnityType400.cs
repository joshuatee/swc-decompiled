using StaRTS.Externals.FacebookApi;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType400 : $UnityType
	{
		public unsafe $UnityType400()
		{
			*(UnityEngine.Internal.$Metadata.data + 441948) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGraphResult)GCHandledObjects.GCHandleToObject(instance)).Texture);
		}
	}
}
