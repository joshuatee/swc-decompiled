using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Utils.Json
{
	[AttributeUsage]
	public class JsonPropertyAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public JsonPropertyAttribute(string name)
		{
			this.Name = name;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((JsonPropertyAttribute)GCHandledObjects.GCHandleToObject(instance)).Name);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((JsonPropertyAttribute)GCHandledObjects.GCHandleToObject(instance)).Name = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
