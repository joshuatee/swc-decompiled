using StaRTS.Main.Models;
using StaRTS.Main.Views.UX.Elements;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Tags
{
	public class PrizeInventoryItemTag
	{
		public string PrizeID
		{
			get;
			set;
		}

		public PrizeType PrizeType
		{
			get;
			set;
		}

		public UXElement TileElement
		{
			get;
			set;
		}

		public UXElement MainElement
		{
			get;
			set;
		}

		public UXLabel InfoLabel
		{
			get;
			set;
		}

		public UXLabel CountLabel
		{
			get;
			set;
		}

		public string IconAssetName
		{
			get;
			set;
		}

		public PrizeInventoryItemTag()
		{
		}

		protected internal PrizeInventoryItemTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).CountLabel);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).InfoLabel);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).MainElement);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).PrizeID);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).PrizeType);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).TileElement);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).CountLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).InfoLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).MainElement = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).PrizeID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).PrizeType = (PrizeType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((PrizeInventoryItemTag)GCHandledObjects.GCHandleToObject(instance)).TileElement = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
