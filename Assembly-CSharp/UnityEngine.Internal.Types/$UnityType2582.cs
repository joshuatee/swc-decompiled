using StaRTS.Utils.Pooling;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2582 : $UnityType
	{
		public unsafe $UnityType2582()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 998532) = ldftn($Invoke8);
			*(data + 998560) = ldftn($Invoke9);
			*(data + 998588) = ldftn($Invoke10);
			*(data + 998616) = ldftn($Invoke11);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((IObjectPool<ParticleSystem>)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((IObjectPool<ParticleSystem>)GCHandledObjects.GCHandleToObject(instance)).EnsurePoolCapacity(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IObjectPool<ParticleSystem>)GCHandledObjects.GCHandleToObject(instance)).Capacity);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IObjectPool<ParticleSystem>)GCHandledObjects.GCHandleToObject(instance)).Count);
		}
	}
}
