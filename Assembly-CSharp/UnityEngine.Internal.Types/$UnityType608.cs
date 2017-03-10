using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType608 : $UnityType
	{
		public unsafe $UnityType608()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 505256) = ldftn($Invoke0);
			*(data + 505284) = ldftn($Invoke1);
			*(data + 505312) = ldftn($Invoke2);
			*(data + 505340) = ldftn($Invoke3);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICommand)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICommand)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICommand)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICommand)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}
	}
}
