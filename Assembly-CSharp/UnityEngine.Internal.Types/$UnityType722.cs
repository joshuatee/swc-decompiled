using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType722 : $UnityType
	{
		public unsafe $UnityType722()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 538016) = ldftn($Invoke0);
			*(data + 538044) = ldftn($Invoke1);
			*(data + 538072) = ldftn($Invoke2);
			*(data + 538100) = ldftn($Invoke3);
			*(data + 538128) = ldftn($Invoke4);
			*(data + 538156) = ldftn($Invoke5);
			*(data + 538184) = ldftn($Invoke6);
			*(data + 538212) = ldftn($Invoke7);
			*(data + 538240) = ldftn($Invoke8);
			*(data + 538268) = ldftn($Invoke9);
			*(data + 538296) = ldftn($Invoke10);
			*(data + 538324) = ldftn($Invoke11);
			*(data + 538352) = ldftn($Invoke12);
			*(data + 538380) = ldftn($Invoke13);
			*(data + 538408) = ldftn($Invoke14);
			*(data + 538436) = ldftn($Invoke15);
			*(data + 538464) = ldftn($Invoke16);
			*(data + 538492) = ldftn($Invoke17);
			*(data + 538520) = ldftn($Invoke18);
			*(data + 538548) = ldftn($Invoke19);
			*(data + 538576) = ldftn($Invoke20);
			*(data + 538604) = ldftn($Invoke21);
			*(data + 538632) = ldftn($Invoke22);
			*(data + 538660) = ldftn($Invoke23);
			*(data + 538688) = ldftn($Invoke24);
			*(data + 538716) = ldftn($Invoke25);
			*(data + 538744) = ldftn($Invoke26);
			*(data + 538772) = ldftn($Invoke27);
			*(data + 538800) = ldftn($Invoke28);
			*(data + 538828) = ldftn($Invoke29);
			*(data + 538856) = ldftn($Invoke30);
			*(data + 538884) = ldftn($Invoke31);
			*(data + 538912) = ldftn($Invoke32);
			*(data + 538940) = ldftn($Invoke33);
			*(data + 538968) = ldftn($Invoke34);
			*(data + 538996) = ldftn($Invoke35);
			*(data + 539024) = ldftn($Invoke36);
			*(data + 539052) = ldftn($Invoke37);
			*(data + 539080) = ldftn($Invoke38);
			*(data + 539108) = ldftn($Invoke39);
			*(data + 539136) = ldftn($Invoke40);
			*(data + 539164) = ldftn($Invoke41);
			*(data + 539192) = ldftn($Invoke42);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).BuyoutAllTroopTrainContracts((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).BuyoutAllTroopTrainContracts((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).BuyOutCurrentBuildingContract((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).CancelCurrentBuildingContract((Contract)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).CancelTroopTrainContract(Marshal.PtrToStringUni(*(IntPtr*)args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).CheatForceUpdateAllContracts();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).DisableBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).FindAllContractsThatConsumeDroids());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).FindAllTroopContractsForBuilding(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).FindBuildingContract(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).FindCurrentContract(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).FindFirstContractWithProductUid(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).FinishCurrentContract((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).FinishCurrentContract((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).GetContractEventsThatHappenedOffline());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).GetUninitializedContractData());
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).HasTroopContractForBuilding(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).InstantBuildingConstruct((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).IsBuildingFrozen(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).IsContractValidForStorage((Contract)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).PauseBuilding(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).ReleaseContractEventsThatHappnedOffline();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).SimulateCheckAllContractsWithCurrentTime();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).SortByEndTime((Contract)GCHandledObjects.GCHandleToObject(*args), (Contract)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).SortContractTOByEndTime((ContractTO)GCHandledObjects.GCHandleToObject(*args), (ContractTO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartAllWallPartBuildingUpgrade((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartBuildingConstruct((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartBuildingUpgrade((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartBuildingUpgrade((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartChampionRepair((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartClearingBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartEquipmentUpgrade((EquipmentVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartHeroMobilization((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartStarshipMobilization((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartStarshipUpgrade((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartTroopTrainContract((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartTroopUpgrade((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).StartTurretCrossgrade((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).SyncCurrentPlayerInventoryWithServer((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).UnpauseAllBuildings();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentContractsListFromServer(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((ISupportController)GCHandledObjects.GCHandleToObject(instance)).UpdateFrozenBuildingsListFromServer(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
