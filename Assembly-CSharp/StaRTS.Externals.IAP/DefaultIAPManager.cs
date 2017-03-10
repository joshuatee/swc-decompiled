using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.IAP
{
	public class DefaultIAPManager : IInAppPurchaseManager
	{
		public void Init()
		{
		}

		public void GetProducts()
		{
		}

		public void Purchase(string productID)
		{
		}

		public void Consume(string productID)
		{
		}

		public void RestorePurchases()
		{
		}

		public void OnApplicationResume()
		{
		}

		public DefaultIAPManager()
		{
		}

		protected internal DefaultIAPManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DefaultIAPManager)GCHandledObjects.GCHandleToObject(instance)).Consume(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DefaultIAPManager)GCHandledObjects.GCHandleToObject(instance)).GetProducts();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DefaultIAPManager)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DefaultIAPManager)GCHandledObjects.GCHandleToObject(instance)).OnApplicationResume();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DefaultIAPManager)GCHandledObjects.GCHandleToObject(instance)).Purchase(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DefaultIAPManager)GCHandledObjects.GCHandleToObject(instance)).RestorePurchases();
			return -1L;
		}
	}
}
