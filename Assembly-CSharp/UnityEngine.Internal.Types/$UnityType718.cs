using StaRTS.Main.Controllers;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType718 : $UnityType
	{
		public unsafe $UnityType718()
		{
			*(UnityEngine.Internal.$Metadata.data + 537176) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IDataController)GCHandledObjects.GCHandleToObject(instance)).Exterminate();
			return -1L;
		}
	}
}
