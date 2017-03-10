using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1931 : $UnityType
	{
		public unsafe $UnityType1931()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 755044) = ldftn($Invoke0);
			*(data + 755072) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopShooterVO)GCHandledObjects.GCHandleToObject(instance)).TargetLocking);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopShooterVO)GCHandledObjects.GCHandleToObject(instance)).TargetSelf);
		}
	}
}
