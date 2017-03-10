using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1922 : $UnityType
	{
		public unsafe $UnityType1922()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 752776) = ldftn($Invoke0);
			*(data + 752804) = ldftn($Invoke1);
			*(data + 752832) = ldftn($Invoke2);
			*(data + 752860) = ldftn($Invoke3);
			*(data + 752888) = ldftn($Invoke4);
			*(data + 752916) = ldftn($Invoke5);
			*(data + 752944) = ldftn($Invoke6);
			*(data + 752972) = ldftn($Invoke7);
			*(data + 753000) = ldftn($Invoke8);
			*(data + 753028) = ldftn($Invoke9);
			*(data + 753056) = ldftn($Invoke10);
			*(data + 753084) = ldftn($Invoke11);
			*(data + 753112) = ldftn($Invoke12);
			*(data + 753140) = ldftn($Invoke13);
			*(data + 753168) = ldftn($Invoke14);
			*(data + 753196) = ldftn($Invoke15);
			*(data + 753224) = ldftn($Invoke16);
			*(data + 753252) = ldftn($Invoke17);
			*(data + 753280) = ldftn($Invoke18);
			*(data + 753308) = ldftn($Invoke19);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).Contraband);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).Credits);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonAction);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonData);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonString);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).EventFeaturesString);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).Materials);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardCount);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardUid);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).Contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).Credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonAction = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonData = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).EventFeaturesString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).Materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((IDeployableVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
