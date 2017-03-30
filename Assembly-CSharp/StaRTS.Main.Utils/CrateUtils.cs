using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

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
			if (condition != null)
			{
				if (CrateUtils.<>f__switch$map12 == null)
				{
					CrateUtils.<>f__switch$map12 = new Dictionary<string, int>(2)
					{
						{
							"ownsArmory",
							0
						},
						{
							"hasAvailableEquipment",
							1
						}
					};
				}
				int num;
				if (CrateUtils.<>f__switch$map12.TryGetValue(condition, out num))
				{
					if (num == 0)
					{
						return ArmoryUtils.PlayerHasArmory();
					}
					if (num == 1)
					{
						return CrateUtils.PlayerHasEquipmentAvailable();
					}
				}
			}
			if (condition.StartsWith("hq"))
			{
				string value = condition.Substring("hq".Length);
				int level = Convert.ToInt32(value);
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
	}
}
