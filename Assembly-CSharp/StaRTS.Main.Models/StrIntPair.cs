using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class StrIntPair
	{
		public string StrKey
		{
			get;
			private set;
		}

		public int IntVal
		{
			get;
			set;
		}

		public StrIntPair(string strKey, int intVal)
		{
			this.StrKey = strKey;
			this.IntVal = intVal;
		}

		protected internal StrIntPair(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StrIntPair)GCHandledObjects.GCHandleToObject(instance)).IntVal);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StrIntPair)GCHandledObjects.GCHandleToObject(instance)).StrKey);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StrIntPair)GCHandledObjects.GCHandleToObject(instance)).IntVal = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StrIntPair)GCHandledObjects.GCHandleToObject(instance)).StrKey = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
