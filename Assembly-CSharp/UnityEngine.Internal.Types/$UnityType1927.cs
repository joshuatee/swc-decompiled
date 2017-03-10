using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1927 : $UnityType
	{
		public unsafe $UnityType1927()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 754204) = ldftn($Invoke0);
			*(data + 754232) = ldftn($Invoke1);
			*(data + 754260) = ldftn($Invoke2);
			*(data + 754288) = ldftn($Invoke3);
			*(data + 754316) = ldftn($Invoke4);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISplashVO)GCHandledObjects.GCHandleToObject(instance)).SplashDamagePercentages);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISplashVO)GCHandledObjects.GCHandleToObject(instance)).SplashRadius);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISplashVO)GCHandledObjects.GCHandleToObject(instance)).GetSplashDamagePercent(*(int*)args));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ISplashVO)GCHandledObjects.GCHandleToObject(instance)).SplashDamagePercentages = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ISplashVO)GCHandledObjects.GCHandleToObject(instance)).SplashRadius = *(int*)args;
			return -1L;
		}
	}
}
