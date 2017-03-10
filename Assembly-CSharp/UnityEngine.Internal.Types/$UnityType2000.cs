using StaRTS.Main.Story;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2000 : $UnityType
	{
		public unsafe $UnityType2000()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 836664) = ldftn($Invoke0);
			*(data + 836692) = ldftn($Invoke1);
			*(data + 836720) = ldftn($Invoke2);
			*(data + 836748) = ldftn($Invoke3);
			*(data + 836776) = ldftn($Invoke4);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).HasReaction);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).VO);
		}
	}
}
