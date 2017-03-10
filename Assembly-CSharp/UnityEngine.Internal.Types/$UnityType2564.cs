using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2564 : $UnityType
	{
		public unsafe $UnityType2564()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 996152) = ldftn($Invoke0);
			*(data + 996180) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISerializable)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISerializable)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
