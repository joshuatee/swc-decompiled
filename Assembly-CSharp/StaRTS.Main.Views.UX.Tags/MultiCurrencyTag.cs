using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Tags
{
	public class MultiCurrencyTag
	{
		public int Credits
		{
			get;
			private set;
		}

		public int Materials
		{
			get;
			private set;
		}

		public int Contraband
		{
			get;
			private set;
		}

		public int Crystals
		{
			get;
			private set;
		}

		public string PurchaseContext
		{
			get;
			private set;
		}

		public MultiCurrencyTag(int credits, int materials, int contraband, int crystals, string purchaseContext)
		{
			this.Credits = credits;
			this.Materials = materials;
			this.Contraband = contraband;
			this.Crystals = crystals;
			this.PurchaseContext = purchaseContext;
		}

		protected internal MultiCurrencyTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Contraband);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Credits);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Crystals);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Materials);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).PurchaseContext);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Crystals = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((MultiCurrencyTag)GCHandledObjects.GCHandleToObject(instance)).PurchaseContext = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
