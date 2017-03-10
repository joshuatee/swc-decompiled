using StaRTS.Main.Models;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Tags
{
	public class CurrencyTag
	{
		public CurrencyType Currency
		{
			get;
			private set;
		}

		public int Amount
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

		public CurrencyTag(CurrencyType currency, int amount, int crystals, string purchaseContext)
		{
			this.Currency = currency;
			this.Amount = amount;
			this.Crystals = crystals;
			this.PurchaseContext = purchaseContext;
		}

		protected internal CurrencyTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Amount);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Crystals);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Currency);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyTag)GCHandledObjects.GCHandleToObject(instance)).PurchaseContext);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((CurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Amount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((CurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Crystals = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((CurrencyTag)GCHandledObjects.GCHandleToObject(instance)).Currency = (CurrencyType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((CurrencyTag)GCHandledObjects.GCHandleToObject(instance)).PurchaseContext = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
