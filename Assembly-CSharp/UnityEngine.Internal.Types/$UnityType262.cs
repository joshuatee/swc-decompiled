using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType262 : $UnityType
	{
		public unsafe $UnityType262()
		{
			*(UnityEngine.Internal.$Metadata.data + 418512) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((INodeList)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}
	}
}
