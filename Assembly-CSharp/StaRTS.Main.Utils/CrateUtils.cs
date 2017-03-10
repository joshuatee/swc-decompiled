using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class CrateUtils
	{
		private const string CONDITION_ARMORY_REQUIRED = "ownsArmory";

		private const string CONDITION_EQUIPMENT_AVAILABLE = "hasAvailableEquipment";

		private const string CONDITION_HQ_PREFIX = "hq";

		public static bool HasVisibleCrateStoreItems()
		{
			IDataController dataController = Service.Get<IDataController>();
			foreach (CrateVO current in dataController.GetAll<CrateVO>())
			{
				if (CrateUtils.IsVisibleInStore(current))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsVisibleInStore(CrateVO crateVO)
		{
			return CrateUtils.AllConditionsMet(crateVO.StoreVisibilityConditions);
		}

		public static bool IsPurchasableInStore(CrateVO crateTier)
		{
			return CrateUtils.AllConditionsMet(crateTier.StorePurchasableConditions);
		}

		public static bool AllConditionsMet(string[] conditions)
		{
			if (conditions == null || conditions.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < conditions.Length; i++)
			{
				if (!CrateUtils.ConditionMet(conditions[i]))
				{
					return false;
				}
			}
			return true;
		}

		private static bool ConditionMet(string condition)
		{
			if (string.IsNullOrEmpty(condition))
			{
				return false;
			}
			if (condition == "ownsArmory")
			{
				return ArmoryUtils.PlayerHasArmory();
			}
			if (condition == "hasAvailableEquipment")
			{
				return CrateUtils.PlayerHasEquipmentAvailable();
			}
			if (condition.StartsWith("hq"))
			{
				string value = condition.Substring("hq".get_Length());
				int level = Convert.ToInt32(value, CultureInfo.InvariantCulture);
				return CrateUtils.PlayerHasAtLeastHqLevel(level);
			}
			return false;
		}

		private static bool PlayerHasAtLeastHqLevel(int level)
		{
			return Service.Get<CurrentPlayer>().Map.FindHighestHqLevel() >= level;
		}

		private static bool PlayerHasEquipmentAvailable()
		{
			CurrentPlayer player = Service.Get<CurrentPlayer>();
			EquipmentUpgradeCatalog equipmentUpgradeCatalog = Service.Get<EquipmentUpgradeCatalog>();
			foreach (string current in equipmentUpgradeCatalog.AllUpgradeGroups())
			{
				if (!ArmoryUtils.HasReachedMaxEquipmentShards(player, equipmentUpgradeCatalog, current))
				{
					return true;
				}
			}
			return false;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateUtils.AllConditionsMet((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateUtils.ConditionMet(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateUtils.HasVisibleCrateStoreItems());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateUtils.IsPurchasableInStore((CrateVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateUtils.IsVisibleInStore((CrateVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateUtils.PlayerHasAtLeastHqLevel(*(int*)args));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateUtils.PlayerHasEquipmentAvailable());
		}
	}
}
