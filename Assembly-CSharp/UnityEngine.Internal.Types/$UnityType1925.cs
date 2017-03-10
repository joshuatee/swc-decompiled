using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1925 : $UnityType
	{
		public unsafe $UnityType1925()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 753784) = ldftn($Invoke0);
			*(data + 753812) = ldftn($Invoke1);
			*(data + 753840) = ldftn($Invoke2);
			*(data + 753868) = ldftn($Invoke3);
			*(data + 753896) = ldftn($Invoke4);
			*(data + 753924) = ldftn($Invoke5);
			*(data + 753952) = ldftn($Invoke6);
			*(data + 753980) = ldftn($Invoke7);
			*(data + 754008) = ldftn($Invoke8);
			*(data + 754036) = ldftn($Invoke9);
			*(data + 754064) = ldftn($Invoke10);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).ClipRetargeting);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).Damage);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).DPS);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).GunSequence);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).NearnessPercentile);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).OverWalls);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).Preference);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).PreferencePercentile);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).Sequences);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IShooterVO)GCHandledObjects.GCHandleToObject(instance)).StrictCooldown);
		}
	}
}
