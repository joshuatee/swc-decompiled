using StaRTS.Main.Views.UserInput;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2487 : $UnityType
	{
		public unsafe $UnityType2487()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 975264) = ldftn($Invoke0);
			*(data + 975292) = ldftn($Invoke1);
			*(data + 975320) = ldftn($Invoke2);
			*(data + 975348) = ldftn($Invoke3);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUserInputObserver)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUserInputObserver)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUserInputObserver)GCHandledObjects.GCHandleToObject(instance)).OnRelease(*(int*)args));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUserInputObserver)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args, *(*(IntPtr*)(args + 1))));
		}
	}
}
