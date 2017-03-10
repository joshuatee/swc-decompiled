using StaRTS.Main.Views.UX.Elements;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class EventTickerObject
	{
		public string message
		{
			get;
			set;
		}

		public UXButtonClickedDelegate onClickFunction
		{
			get;
			set;
		}

		public string planet
		{
			get;
			set;
		}

		public Color textColor
		{
			get;
			set;
		}

		public Color bgColor
		{
			get;
			set;
		}

		public EventTickerObject()
		{
		}

		protected internal EventTickerObject(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).bgColor);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).message);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).onClickFunction);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).planet);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).textColor);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).bgColor = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).message = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).onClickFunction = (UXButtonClickedDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).planet = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((EventTickerObject)GCHandledObjects.GCHandleToObject(instance)).textColor = *(*(IntPtr*)args);
			return -1L;
		}
	}
}
