using StaRTS.Utils.Pooling;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2581 : $UnityType
	{
		public unsafe $UnityType2581()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 998420) = ldftn($Invoke4);
			*(data + 998448) = ldftn($Invoke5);
			*(data + 998476) = ldftn($Invoke6);
			*(data + 998504) = ldftn($Invoke7);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).EnsurePoolCapacity(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).Capacity);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).Count);
		}
	}
}
