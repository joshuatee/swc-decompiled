using StaRTS.Main.Story;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1998 : $UnityType
	{
		public unsafe $UnityType1998()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 836468) = ldftn($Invoke0);
			*(data + 836496) = ldftn($Invoke1);
			*(data + 836524) = ldftn($Invoke2);
			*(data + 836552) = ldftn($Invoke3);
			*(data + 836580) = ldftn($Invoke4);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IStoryAction)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IStoryAction)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IStoryAction)GCHandledObjects.GCHandleToObject(instance)).VO);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
