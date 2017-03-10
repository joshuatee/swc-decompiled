using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace com.amazon.device.iap.cpt.json
{
	public static class Json
	{
		public static object Deserialize(string json)
		{
			return null;
		}

		public static string Serialize(object obj)
		{
			return null;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Json.Deserialize(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Json.Serialize(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
