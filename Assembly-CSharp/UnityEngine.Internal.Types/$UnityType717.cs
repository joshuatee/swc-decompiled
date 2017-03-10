using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType717 : $UnityType
	{
		public unsafe $UnityType717()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 536756) = ldftn($Invoke0);
			*(data + 536784) = ldftn($Invoke1);
			*(data + 536812) = ldftn($Invoke2);
			*(data + 536840) = ldftn($Invoke3);
			*(data + 536868) = ldftn($Invoke4);
			*(data + 536896) = ldftn($Invoke5);
			*(data + 536924) = ldftn($Invoke6);
			*(data + 536952) = ldftn($Invoke7);
			*(data + 536980) = ldftn($Invoke8);
			*(data + 537008) = ldftn($Invoke9);
			*(data + 537036) = ldftn($Invoke10);
			*(data + 537064) = ldftn($Invoke11);
			*(data + 537092) = ldftn($Invoke12);
			*(data + 537120) = ldftn($Invoke13);
			*(data + 537148) = ldftn($Invoke14);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).CalculateAccruedCurrency((Building)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).CalculateGeneratorFillTimeRemaining((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).CalculateTimeUntilAllGeneratorsFull());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).CanStoreCollectionAmountFromGenerator((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).CollectCurrency((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).CurrencyPerHour((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).CurrencyPerSecond((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).ForceCollectAccruedCurrencyForUpgrade((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).HandleUnableToCollect((CurrencyType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).IsGeneratorCollectable((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).IsGeneratorThresholdMet((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).TryCollectCurrencyOnSelection((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).UpdateAllStorageEffects();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ICurrencyController)GCHandledObjects.GCHandleToObject(instance)).UpdateGeneratorAccruedCurrency((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
