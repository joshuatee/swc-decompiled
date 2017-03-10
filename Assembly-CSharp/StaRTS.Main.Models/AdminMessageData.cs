using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class AdminMessageData
	{
		public string Uid
		{
			get;
			private set;
		}

		public string Message
		{
			get;
			private set;
		}

		public bool IsCritical
		{
			get;
			private set;
		}

		public AdminMessageData(string uid, string message, bool isCritical)
		{
			this.Uid = uid;
			this.Message = message;
			this.IsCritical = isCritical;
		}

		protected internal AdminMessageData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AdminMessageData)GCHandledObjects.GCHandleToObject(instance)).IsCritical);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AdminMessageData)GCHandledObjects.GCHandleToObject(instance)).Message);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AdminMessageData)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AdminMessageData)GCHandledObjects.GCHandleToObject(instance)).IsCritical = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AdminMessageData)GCHandledObjects.GCHandleToObject(instance)).Message = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AdminMessageData)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
