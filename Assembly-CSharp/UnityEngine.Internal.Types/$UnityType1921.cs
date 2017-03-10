using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1921 : $UnityType
	{
		public unsafe $UnityType1921()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 752328) = ldftn($Invoke0);
			*(data + 752356) = ldftn($Invoke1);
			*(data + 752384) = ldftn($Invoke2);
			*(data + 752412) = ldftn($Invoke3);
			*(data + 752440) = ldftn($Invoke4);
			*(data + 752468) = ldftn($Invoke5);
			*(data + 752496) = ldftn($Invoke6);
			*(data + 752524) = ldftn($Invoke7);
			*(data + 752552) = ldftn($Invoke8);
			*(data + 752580) = ldftn($Invoke9);
			*(data + 752608) = ldftn($Invoke10);
			*(data + 752636) = ldftn($Invoke11);
			*(data + 752664) = ldftn($Invoke12);
			*(data + 752692) = ldftn($Invoke13);
			*(data + 752720) = ldftn($Invoke14);
			*(data + 752748) = ldftn($Invoke15);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((IAudioVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
