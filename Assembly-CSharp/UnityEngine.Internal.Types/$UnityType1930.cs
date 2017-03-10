using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1930 : $UnityType
	{
		public unsafe $UnityType1930()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 754596) = ldftn($Invoke0);
			*(data + 754624) = ldftn($Invoke1);
			*(data + 754652) = ldftn($Invoke2);
			*(data + 754680) = ldftn($Invoke3);
			*(data + 754708) = ldftn($Invoke4);
			*(data + 754736) = ldftn($Invoke5);
			*(data + 754764) = ldftn($Invoke6);
			*(data + 754792) = ldftn($Invoke7);
			*(data + 754820) = ldftn($Invoke8);
			*(data + 754848) = ldftn($Invoke9);
			*(data + 754876) = ldftn($Invoke10);
			*(data + 754904) = ldftn($Invoke11);
			*(data + 754932) = ldftn($Invoke12);
			*(data + 754960) = ldftn($Invoke13);
			*(data + 754988) = ldftn($Invoke14);
			*(data + 755016) = ldftn($Invoke15);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).ArmorType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).AttackShieldBorder);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetBaseOffset);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetOffset);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).CrushesWalls);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).IsFlying);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).IsHealer);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).RunSpeed);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).RunThreshold);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).SizeX);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).SizeY);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).SpawnApplyBuffs);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).TroopID);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetBaseOffset = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ITroopDeployableVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetOffset = *(*(IntPtr*)args);
			return -1L;
		}
	}
}
