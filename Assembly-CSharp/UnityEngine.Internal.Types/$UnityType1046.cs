using StaRTS.Main.Models;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1046 : $UnityType
	{
		public unsafe $UnityType1046()
		{
			*(UnityEngine.Internal.$Metadata.data + 626020) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITrapEventData)GCHandledObjects.GCHandleToObject(instance)).Init(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}
