using StaRTS.Main.Models.ValueObjects;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1933 : $UnityType
	{
		public unsafe $UnityType1933()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 755268) = ldftn($Invoke0);
			*(data + 755296) = ldftn($Invoke1);
			*(data + 755324) = ldftn($Invoke2);
			*(data + 755352) = ldftn($Invoke3);
			*(data + 755380) = ldftn($Invoke4);
			*(data + 755408) = ldftn($Invoke5);
			*(data + 755436) = ldftn($Invoke6);
			*(data + 755464) = ldftn($Invoke7);
			*(data + 755492) = ldftn($Invoke8);
			*(data + 755520) = ldftn($Invoke9);
			*(data + 755548) = ldftn($Invoke10);
			*(data + 755576) = ldftn($Invoke11);
			*(data + 755604) = ldftn($Invoke12);
			*(data + 755632) = ldftn($Invoke13);
			*(data + 755660) = ldftn($Invoke14);
			*(data + 755688) = ldftn($Invoke15);
			*(data + 755716) = ldftn($Invoke16);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).Lvl);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).Order);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeContraband);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeCredits);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeGroup);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeMaterials);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).Lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).Order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).Size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((IUpgradeableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime = *(int*)args;
			return -1L;
		}
	}
}
