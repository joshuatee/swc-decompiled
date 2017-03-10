using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Assets
{
	public class InternalLoadCookie
	{
		public string AssetName
		{
			get;
			private set;
		}

		public InternalLoadCookie(string assetName)
		{
			this.AssetName = assetName;
		}

		protected internal InternalLoadCookie(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InternalLoadCookie)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((InternalLoadCookie)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
