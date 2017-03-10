using StaRTS.Main.Controllers.Holonet;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType846 : $UnityType
	{
		public unsafe $UnityType846()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 576208) = ldftn($Invoke0);
			*(data + 576236) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IHolonetContoller)GCHandledObjects.GCHandleToObject(instance)).ControllerType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IHolonetContoller)GCHandledObjects.GCHandleToObject(instance)).PrepareContent(*(int*)args);
			return -1L;
		}
	}
}
