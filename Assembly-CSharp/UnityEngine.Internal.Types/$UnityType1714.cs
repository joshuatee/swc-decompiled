using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1714 : $UnityType
	{
		public unsafe $UnityType1714()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 670960) = ldftn($Invoke0);
			*(data + 670988) = ldftn($Invoke1);
			*(data + 671016) = ldftn($Invoke2);
			*(data + 671044) = ldftn($Invoke3);
			*(data + 671072) = ldftn($Invoke4);
			*(data + 671100) = ldftn($Invoke5);
			*(data + 671128) = ldftn($Invoke6);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IHealthComponent)GCHandledObjects.GCHandleToObject(instance)).ArmorType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IHealthComponent)GCHandledObjects.GCHandleToObject(instance)).Health);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IHealthComponent)GCHandledObjects.GCHandleToObject(instance)).MaxHealth);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IHealthComponent)GCHandledObjects.GCHandleToObject(instance)).IsDead());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IHealthComponent)GCHandledObjects.GCHandleToObject(instance)).ArmorType = (ArmorType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IHealthComponent)GCHandledObjects.GCHandleToObject(instance)).Health = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((IHealthComponent)GCHandledObjects.GCHandleToObject(instance)).MaxHealth = *(int*)args;
			return -1L;
		}
	}
}
