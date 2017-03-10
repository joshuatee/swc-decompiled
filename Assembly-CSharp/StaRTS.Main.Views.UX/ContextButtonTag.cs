using StaRTS.Main.Views.UX.Elements;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX
{
	public class ContextButtonTag
	{
		public string ContextId
		{
			get;
			set;
		}

		public string SpriteName
		{
			get;
			set;
		}

		public UXButton ContextButton
		{
			get;
			set;
		}

		public UXButton ContextDimButton
		{
			get;
			set;
		}

		public UXSprite ContextIconSprite
		{
			get;
			set;
		}

		public UXSprite ContextBackground
		{
			get;
			set;
		}

		public UXLabel ContextLabel
		{
			get;
			set;
		}

		public UXLabel HardCostLabel
		{
			get;
			set;
		}

		public UXLabel TimerLabel
		{
			get;
			set;
		}

		public GetTimerSecondsDelegate TimerSecondsDelegate
		{
			get;
			set;
		}

		public ContextButtonTag()
		{
		}

		protected internal ContextButtonTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextBackground);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextButton);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextDimButton);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextIconSprite);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextId);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextLabel);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).HardCostLabel);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).SpriteName);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).TimerLabel);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).TimerSecondsDelegate);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextBackground = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextButton = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextDimButton = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextIconSprite = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).ContextLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).HardCostLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).SpriteName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).TimerLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((ContextButtonTag)GCHandledObjects.GCHandleToObject(instance)).TimerSecondsDelegate = (GetTimerSecondsDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
