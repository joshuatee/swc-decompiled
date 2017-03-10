using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1932 : $UnityType
	{
		public unsafe $UnityType1932()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 755100) = ldftn($Invoke0);
			*(data + 755128) = ldftn($Invoke1);
			*(data + 755156) = ldftn($Invoke2);
			*(data + 755184) = ldftn($Invoke3);
			*(data + 755212) = ldftn($Invoke4);
			*(data + 755240) = ldftn($Invoke5);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUnlockableVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUnlockableVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUnlockableVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((IUnlockableVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IUnlockableVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IUnlockableVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale = *(*(IntPtr*)args);
			return -1L;
		}
	}
}
