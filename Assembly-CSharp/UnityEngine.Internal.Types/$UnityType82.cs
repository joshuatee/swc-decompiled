using StaRTS.Main.Controllers;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType82 : $UnityType
	{
		public unsafe $UnityType82()
		{
			*(UnityEngine.Internal.$Metadata.data + 362344) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StaticDataController.IStaticDataWrapper)GCHandledObjects.GCHandleToObject(instance)).Flush();
			return -1L;
		}
	}
}
