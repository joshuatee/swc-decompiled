using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class ArmorySortUtils
	{
		public delegate int SortEquipmentDelegate(SortableEquipment a, SortableEquipment b);

		public static void SortWithPriorityList(List<SortableEquipment> list, List<EquipmentSortMethod> priorityList)
		{
			list.Sort((SortableEquipment a, SortableEquipment b) => ArmorySortUtils.SortWithList(a, b, priorityList));
		}

		public static List<EquipmentVO> RemoveWrapper(List<SortableEquipment> sortableEquipmentList)
		{
			List<EquipmentVO> list = new List<EquipmentVO>();
			int i = 0;
			int count = sortableEquipmentList.Count;
			while (i < count)
			{
				list.Add(sortableEquipmentList[i].Equipment);
				i++;
			}
			return list;
		}

		public static int SortWithList(SortableEquipment a, SortableEquipment b, List<EquipmentSortMethod> sortingPriority)
		{
			Dictionary<EquipmentSortMethod, ArmorySortUtils.SortEquipmentDelegate> dictionary = ArmorySortUtils.CreateSortDictionary();
			int i = 0;
			int count = sortingPriority.Count;
			while (i < count)
			{
				ArmorySortUtils.SortEquipmentDelegate sortEquipmentDelegate = dictionary[sortingPriority[i]];
				int num = sortEquipmentDelegate(a, b);
				if (num != 0)
				{
					return num;
				}
				i++;
			}
			return 0;
		}

		private static Dictionary<EquipmentSortMethod, ArmorySortUtils.SortEquipmentDelegate> CreateSortDictionary()
		{
			return new Dictionary<EquipmentSortMethod, ArmorySortUtils.SortEquipmentDelegate>
			{
				{
					EquipmentSortMethod.UnlockedEquipment,
					new ArmorySortUtils.SortEquipmentDelegate(ArmorySortUtils.SortByUnlockedEquipment)
				},
				{
					EquipmentSortMethod.RequirementsMet,
					new ArmorySortUtils.SortEquipmentDelegate(ArmorySortUtils.SortByRequirementsMet)
				},
				{
					EquipmentSortMethod.CurrentPlanet,
					new ArmorySortUtils.SortEquipmentDelegate(ArmorySortUtils.SortByCurrentPlanet)
				},
				{
					EquipmentSortMethod.CapacitySize,
					new ArmorySortUtils.SortEquipmentDelegate(ArmorySortUtils.SortByCapacitySize)
				},
				{
					EquipmentSortMethod.Quality,
					new ArmorySortUtils.SortEquipmentDelegate(ArmorySortUtils.SortByQuality)
				},
				{
					EquipmentSortMethod.Alphabetical,
					new ArmorySortUtils.SortEquipmentDelegate(ArmorySortUtils.SortAlphabetically)
				},
				{
					EquipmentSortMethod.EmptyEquipment,
					new ArmorySortUtils.SortEquipmentDelegate(ArmorySortUtils.SortByEmptyEquipment)
				},
				{
					EquipmentSortMethod.DecrementingIndex,
					new ArmorySortUtils.SortEquipmentDelegate(ArmorySortUtils.SortByDecrementingIndex)
				},
				{
					EquipmentSortMethod.IncrementingEmptyIndex,
					new ArmorySortUtils.SortEquipmentDelegate(ArmorySortUtils.SortByIncrementingEmptyIndex)
				}
			};
		}

		private static int SortByUnlockedEquipment(SortableEquipment a, SortableEquipment b)
		{
			bool flag = a.Player.UnlockedLevels.Equipment.Levels.ContainsKey(a.Equipment.EquipmentID);
			bool flag2 = b.Player.UnlockedLevels.Equipment.Levels.ContainsKey(b.Equipment.EquipmentID);
			if (flag && !flag2)
			{
				return -1;
			}
			if (flag2 && !flag)
			{
				return 1;
			}
			return 0;
		}

		private static int SortByRequirementsMet(SortableEquipment a, SortableEquipment b)
		{
			bool flag = ArmoryUtils.IsBuildingRequirementMet(a.Equipment);
			bool flag2 = ArmoryUtils.IsBuildingRequirementMet(b.Equipment);
			if (flag && !flag2)
			{
				return -1;
			}
			if (flag2 && !flag)
			{
				return 1;
			}
			return 0;
		}

		private static int SortByCurrentPlanet(SortableEquipment a, SortableEquipment b)
		{
			bool flag = ArmoryUtils.IsEquipmentOnValidPlanet(a.Player, a.Equipment);
			bool flag2 = ArmoryUtils.IsEquipmentOnValidPlanet(b.Player, b.Equipment);
			if (flag && !flag2)
			{
				return -1;
			}
			if (flag2 && !flag)
			{
				return 1;
			}
			return 0;
		}

		private static int SortByCapacitySize(SortableEquipment a, SortableEquipment b)
		{
			int num = a.Equipment.Size - b.Equipment.Size;
			if (num != 0)
			{
				return num;
			}
			return 0;
		}

		private static int SortByQuality(SortableEquipment a, SortableEquipment b)
		{
			int num = b.Equipment.Quality - a.Equipment.Quality;
			if (num != 0)
			{
				return num;
			}
			return 0;
		}

		private static int SortAlphabetically(SortableEquipment a, SortableEquipment b)
		{
			return string.Compare(a.Equipment.EquipmentID, b.Equipment.EquipmentID);
		}

		private static int SortByEmptyEquipment(SortableEquipment a, SortableEquipment b)
		{
			if (a.HasEquipment() && !b.HasEquipment())
			{
				return -1;
			}
			if (b.HasEquipment() && !a.HasEquipment())
			{
				return 1;
			}
			return 0;
		}

		private static int SortByDecrementingIndex(SortableEquipment a, SortableEquipment b)
		{
			int num = b.IncrementingIndex - a.IncrementingIndex;
			if (num != 0)
			{
				return num;
			}
			return 0;
		}

		private static int SortByIncrementingEmptyIndex(SortableEquipment a, SortableEquipment b)
		{
			int num = a.EmptyIndex - b.EmptyIndex;
			if (num != 0)
			{
				return num;
			}
			return 0;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.CreateSortDictionary());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.RemoveWrapper((List<SortableEquipment>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortAlphabetically((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortByCapacitySize((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortByCurrentPlanet((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortByDecrementingIndex((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortByEmptyEquipment((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortByIncrementingEmptyIndex((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortByQuality((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortByRequirementsMet((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortByUnlockedEquipment((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmorySortUtils.SortWithList((SortableEquipment)GCHandledObjects.GCHandleToObject(*args), (SortableEquipment)GCHandledObjects.GCHandleToObject(args[1]), (List<EquipmentSortMethod>)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			ArmorySortUtils.SortWithPriorityList((List<SortableEquipment>)GCHandledObjects.GCHandleToObject(*args), (List<EquipmentSortMethod>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
