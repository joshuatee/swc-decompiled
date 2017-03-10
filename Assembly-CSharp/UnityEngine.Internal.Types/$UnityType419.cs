using StaRTS.Externals.IAP;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType419 : $UnityType
	{
		public unsafe $UnityType419()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 445336) = ldftn($Invoke0);
			*(data + 445364) = ldftn($Invoke1);
			*(data + 445392) = ldftn($Invoke2);
			*(data + 445420) = ldftn($Invoke3);
			*(data + 445448) = ldftn($Invoke4);
			*(data + 445476) = ldftn($Invoke5);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IInAppPurchaseManager)GCHandledObjects.GCHandleToObject(instance)).Consume(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IInAppPurchaseManager)GCHandledObjects.GCHandleToObject(instance)).GetProducts();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IInAppPurchaseManager)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((IInAppPurchaseManager)GCHandledObjects.GCHandleToObject(instance)).OnApplicationResume();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IInAppPurchaseManager)GCHandledObjects.GCHandleToObject(instance)).Purchase(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IInAppPurchaseManager)GCHandledObjects.GCHandleToObject(instance)).RestorePurchases();
			return -1L;
		}
	}
}
