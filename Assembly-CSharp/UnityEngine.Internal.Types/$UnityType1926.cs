using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1926 : $UnityType
	{
		public unsafe $UnityType1926()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 754092) = ldftn($Invoke0);
			*(data + 754120) = ldftn($Invoke1);
			*(data + 754148) = ldftn($Invoke2);
			*(data + 754176) = ldftn($Invoke3);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISpeedVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISpeedVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ISpeedVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ISpeedVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed = *(int*)args;
			return -1L;
		}
	}
}
