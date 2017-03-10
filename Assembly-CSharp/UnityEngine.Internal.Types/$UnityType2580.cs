using StaRTS.Main.Views.World.Targeting;
using StaRTS.Utils.Pooling;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2580 : $UnityType
	{
		public unsafe $UnityType2580()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 998308) = ldftn($Invoke0);
			*(data + 998336) = ldftn($Invoke1);
			*(data + 998364) = ldftn($Invoke2);
			*(data + 998392) = ldftn($Invoke3);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).EnsurePoolCapacity(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).Capacity);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).Count);
		}
	}
}
