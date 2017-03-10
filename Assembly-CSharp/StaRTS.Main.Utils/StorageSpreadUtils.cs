using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class StorageSpreadUtils
	{
		public static int CalculateAssumedCurrencyInStorage(CurrencyType currencyType, Entity targetBuilding)
		{
			int num = 0;
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			switch (currencyType)
			{
			case CurrencyType.Credits:
				num = worldOwner.CurrentCreditsAmount;
				break;
			case CurrencyType.Materials:
				num = worldOwner.CurrentMaterialsAmount;
				break;
			case CurrencyType.Contraband:
				num = worldOwner.CurrentContrabandAmount;
				break;
			}
			List<StorageNode> list = new List<StorageNode>();
			NodeList<StorageNode> storageNodeList = Service.Get<BuildingLookupController>().StorageNodeList;
			for (StorageNode storageNode = storageNodeList.Head; storageNode != null; storageNode = storageNode.Next)
			{
				BuildingTypeVO buildingType = storageNode.BuildingComp.BuildingType;
				if (buildingType.Currency == currencyType)
				{
					list.Add(storageNode);
				}
			}
			list.Sort(new Comparison<StorageNode>(StorageSpreadUtils.CompareStorageNode));
			int num2 = (targetBuilding == null) ? 0 : targetBuilding.Get<BuildingComponent>().BuildingType.Storage;
			int num3 = 0;
			int num4 = -1;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				BuildingTypeVO buildingType2 = list[i].BuildingComp.BuildingType;
				int storage = buildingType2.Storage;
				if (storage != num4)
				{
					num4 = storage;
					num3 = num / (count - i);
				}
				int num5;
				if (num3 <= storage)
				{
					num5 = num3;
				}
				else
				{
					num5 = storage;
				}
				if (targetBuilding != null && (num5 < storage || storage == num2))
				{
					return num5;
				}
				num -= num5;
				i++;
			}
			return 0;
		}

		public static void UpdateAllStarportFullnessMeters()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			Dictionary<string, InventoryEntry> internalStorage = worldOwner.Inventory.Troop.GetInternalStorage();
			IDataController dataController = Service.Get<IDataController>();
			List<TroopTypeVO> list = new List<TroopTypeVO>();
			foreach (string current in internalStorage.Keys)
			{
				list.Add(dataController.Get<TroopTypeVO>(current));
			}
			List<StarportNode> list2 = new List<StarportNode>();
			NodeList<StarportNode> starportNodeList = Service.Get<BuildingLookupController>().StarportNodeList;
			for (StarportNode starportNode = starportNodeList.Head; starportNode != null; starportNode = starportNode.Next)
			{
				if (!ContractUtils.IsBuildingConstructing(starportNode.BuildingComp.Entity))
				{
					list2.Add(starportNode);
				}
			}
			list.Sort(new Comparison<TroopTypeVO>(StorageSpreadUtils.CompareTroop));
			int count = list.Count;
			int[] array = new int[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = internalStorage[list[i].Uid].Amount;
			}
			list2.Sort(new Comparison<StarportNode>(StorageSpreadUtils.CompareStarportNode));
			int num = 0;
			int count2 = list2.Count;
			PriorityList<int> priorityList = new PriorityList<int>();
			int[] array2 = new int[count2];
			for (int j = 0; j < count2; j++)
			{
				int storage = list2[j].BuildingComp.BuildingType.Storage;
				array2[j] = storage;
				num += storage;
				int priority = -j;
				priorityList.Add(j, priority);
			}
			int num2 = 0;
			while (num2 < count && num > 0)
			{
				int size = list[num2].Size;
				int num3 = array[num2];
				while (num3 > 0 && num > 0)
				{
					bool flag = false;
					for (int k = 0; k < count2; k++)
					{
						int element = priorityList.GetElement(k);
						int num4 = array2[element];
						if (size <= num4)
						{
							array2[element] -= size;
							num -= size;
							int num5 = priorityList.GetPriority(k);
							priorityList.RemoveAt(k);
							num5 -= size * count2;
							priorityList.Add(element, num5);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						int num6 = size;
						int num7 = 0;
						while (num7 < count2 && num6 > 0)
						{
							int num8 = array2[num7];
							if (num8 > 0)
							{
								int num9 = (num6 < num8) ? num6 : num8;
								array2[num7] -= num9;
								num -= num9;
								num6 -= num9;
							}
							num7++;
						}
					}
					num3--;
				}
				num2++;
			}
			for (int l = 0; l < count2; l++)
			{
				BuildingComponent buildingComp = list2[l].BuildingComp;
				int storage2 = buildingComp.BuildingType.Storage;
				int num10 = storage2 - array2[l];
				Entity entity = buildingComp.Entity;
				StorageSpreadUtils.SetStarportFullnessPercent(entity, (float)num10 / (float)storage2);
				StorageSpreadUtils.SetStarportFillSize(entity, num10);
			}
		}

		public static Entity FindLeastFullStarport()
		{
			float num = 0f;
			int num2 = 0;
			Entity entity = null;
			NodeList<StarportNode> starportNodeList = Service.Get<BuildingLookupController>().StarportNodeList;
			for (StarportNode starportNode = starportNodeList.Head; starportNode != null; starportNode = starportNode.Next)
			{
				BuildingComponent buildingComp = starportNode.BuildingComp;
				Entity entity2 = buildingComp.Entity;
				if (!ContractUtils.IsBuildingConstructing(entity2))
				{
					int starportFillSize = StorageSpreadUtils.GetStarportFillSize(entity2);
					int storage = buildingComp.BuildingType.Storage;
					if (entity == null || (float)starportFillSize < num || ((float)starportFillSize == num && storage < num2))
					{
						num = (float)starportFillSize;
						num2 = storage;
						entity = entity2;
					}
				}
			}
			return entity;
		}

		public static void AddTroopToStarportVisually(Entity starport, TroopTypeVO troop)
		{
			if (starport != null && starport.Get<BuildingComponent>() != null && troop != null)
			{
				float num = StorageSpreadUtils.GetStarportFullnessPercent(starport);
				int storage = starport.Get<BuildingComponent>().BuildingType.Storage;
				num += (float)troop.Size / (float)storage;
				if (num > 1f)
				{
					StorageSpreadUtils.UpdateAllStarportFullnessMeters();
					return;
				}
				StorageSpreadUtils.SetStarportFullnessPercent(starport, num);
			}
		}

		public static void AddTroopToStarportReserve(Entity starport, TroopTypeVO troop)
		{
			if (starport != null)
			{
				int num = StorageSpreadUtils.GetStarportFillSize(starport);
				num += troop.Size;
				StorageSpreadUtils.SetStarportFillSize(starport, num);
			}
		}

		private static float GetStarportFullnessPercent(Entity starport)
		{
			MeterShaderComponent meterShaderComponent = starport.Get<MeterShaderComponent>();
			if (meterShaderComponent != null)
			{
				return meterShaderComponent.Percentage;
			}
			return 0f;
		}

		private static int GetStarportFillSize(Entity starport)
		{
			MeterShaderComponent meterShaderComponent = starport.Get<MeterShaderComponent>();
			if (meterShaderComponent != null)
			{
				return meterShaderComponent.FillSize;
			}
			return 0;
		}

		private static void SetStarportFullnessPercent(Entity starport, float percent)
		{
			MeterShaderComponent meterShaderComponent = starport.Get<MeterShaderComponent>();
			if (meterShaderComponent != null)
			{
				meterShaderComponent.UpdatePercentage(percent);
				Service.Get<EventManager>().SendEvent(EventId.StarportMeterUpdated, meterShaderComponent);
			}
		}

		private static void SetStarportFillSize(Entity starport, int fillSize)
		{
			MeterShaderComponent meterShaderComponent = starport.Get<MeterShaderComponent>();
			if (meterShaderComponent != null)
			{
				meterShaderComponent.FillSize = fillSize;
			}
		}

		private static int CompareTroop(TroopTypeVO a, TroopTypeVO b)
		{
			int num = b.Size - a.Size;
			if (num == 0)
			{
				num = a.Order - b.Order;
			}
			return num;
		}

		private static int CompareStorageNode(StorageNode a, StorageNode b)
		{
			return StorageSpreadUtils.CompareEntityStorage(a.BuildingComp, b.BuildingComp);
		}

		private static int CompareStarportNode(StarportNode a, StarportNode b)
		{
			return StorageSpreadUtils.CompareEntityStorage(a.BuildingComp, b.BuildingComp);
		}

		private static int CompareEntityStorage(BuildingComponent a, BuildingComponent b)
		{
			if (a == b)
			{
				return 0;
			}
			int num = a.BuildingType.Storage - b.BuildingType.Storage;
			if (num == 0)
			{
				num = ((a.Entity.ID > b.Entity.ID) ? 1 : -1);
			}
			return num;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			StorageSpreadUtils.AddTroopToStarportReserve((Entity)GCHandledObjects.GCHandleToObject(*args), (TroopTypeVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			StorageSpreadUtils.AddTroopToStarportVisually((Entity)GCHandledObjects.GCHandleToObject(*args), (TroopTypeVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StorageSpreadUtils.CalculateAssumedCurrencyInStorage((CurrencyType)(*(int*)args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StorageSpreadUtils.CompareEntityStorage((BuildingComponent)GCHandledObjects.GCHandleToObject(*args), (BuildingComponent)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StorageSpreadUtils.CompareStarportNode((StarportNode)GCHandledObjects.GCHandleToObject(*args), (StarportNode)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StorageSpreadUtils.CompareStorageNode((StorageNode)GCHandledObjects.GCHandleToObject(*args), (StorageNode)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StorageSpreadUtils.CompareTroop((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (TroopTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StorageSpreadUtils.FindLeastFullStarport());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StorageSpreadUtils.GetStarportFillSize((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StorageSpreadUtils.GetStarportFullnessPercent((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			StorageSpreadUtils.SetStarportFillSize((Entity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			StorageSpreadUtils.SetStarportFullnessPercent((Entity)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			StorageSpreadUtils.UpdateAllStarportFullnessMeters();
			return -1L;
		}
	}
}
