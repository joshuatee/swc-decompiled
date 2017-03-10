using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public static class RewardUtils
	{
		public const string NOT_ENOUGH_HOUSING = "NOT_ENOUGH_HOUSING";

		public const string NOT_ENOUGH_SPACE = "NOT_ENOUGH_SPACE";

		public const string INVENTORY_NO_ROOM = "INVENTORY_NO_ROOM";

		private const string TROOP_MULTIPLIER = "TROOP_MULTIPLIER";

		private const string REWARD_UPGRADE = "LABEL_REWARD_UPGRADE";

		private const string REWARD_UNLOCK = "LABEL_REWARD_UNLOCKED";

		private const string DROID_UID = "troopWorkerDroid";

		public static List<RewardComponentTag> GetRewardComponents(RewardVO rewardVO)
		{
			List<RewardComponentTag> list = new List<RewardComponentTag>();
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, int> dictionary = GameUtils.ListToMap(rewardVO.TroopRewards);
			foreach (string current in dictionary.Keys)
			{
				RewardComponentTag item = default(RewardComponentTag);
				TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(current);
				item.RewardName = LangUtils.GetTroopDisplayName(troopTypeVO);
				item.Quantity = Service.Get<Lang>().Get("TROOP_MULTIPLIER", new object[]
				{
					dictionary[current]
				});
				item.RewardAssetName = troopTypeVO.AssetName;
				item.RewardGeometryConfig = troopTypeVO;
				item.Type = RewardType.Troop;
				list.Add(item);
			}
			dictionary = GameUtils.ListToMap(rewardVO.HeroRewards);
			foreach (string current2 in dictionary.Keys)
			{
				RewardComponentTag item2 = default(RewardComponentTag);
				TroopTypeVO troopTypeVO2 = dataController.Get<TroopTypeVO>(current2);
				item2.RewardName = LangUtils.GetTroopDisplayName(troopTypeVO2);
				item2.Quantity = Service.Get<Lang>().Get("TROOP_MULTIPLIER", new object[]
				{
					dictionary[current2]
				});
				item2.RewardAssetName = troopTypeVO2.AssetName;
				item2.RewardGeometryConfig = troopTypeVO2;
				item2.Type = RewardType.Troop;
				list.Add(item2);
			}
			dictionary = GameUtils.ListToMap(rewardVO.SpecialAttackRewards);
			foreach (string current3 in dictionary.Keys)
			{
				RewardComponentTag item3 = default(RewardComponentTag);
				SpecialAttackTypeVO specialAttackTypeVO = dataController.Get<SpecialAttackTypeVO>(current3);
				item3.RewardName = LangUtils.GetStarshipDisplayName(specialAttackTypeVO);
				item3.Quantity = Service.Get<Lang>().Get("TROOP_MULTIPLIER", new object[]
				{
					dictionary[current3]
				});
				item3.RewardAssetName = specialAttackTypeVO.AssetName;
				item3.RewardGeometryConfig = specialAttackTypeVO;
				item3.Type = RewardType.Troop;
				list.Add(item3);
			}
			dictionary = GameUtils.ListToMap(rewardVO.CurrencyRewards);
			foreach (string current4 in dictionary.Keys)
			{
				RewardComponentTag item4 = default(RewardComponentTag);
				item4.RewardName = Service.Get<Lang>().Get(current4.ToUpper(), new object[0]);
				int num = dictionary[current4];
				item4.Quantity = Service.Get<Lang>().ThousandsSeparated(num);
				int num2 = 0;
				int num3 = 0;
				if (current4 == "credits")
				{
					num2 = GameConstants.CREDITS_3_THRESHOLD;
					num3 = GameConstants.CREDITS_2_THRESHOLD;
				}
				else if (current4 == "materials")
				{
					num2 = GameConstants.MATERIALS_3_THRESHOLD;
					num3 = GameConstants.MATERIALS_2_THRESHOLD;
				}
				else if (current4 == "contraband")
				{
					num2 = GameConstants.CONTRABAND_3_THRESHOLD;
					num3 = GameConstants.CONTRABAND_2_THRESHOLD;
				}
				else if (current4 == "crystals")
				{
					num2 = GameConstants.CRYSTALS_3_THRESHOLD;
					num3 = GameConstants.CRYSTALS_2_THRESHOLD;
				}
				if (num >= num2)
				{
					item4.RewardAssetName = current4 + 3;
				}
				else if (num >= num3)
				{
					item4.RewardAssetName = current4 + 2;
				}
				else
				{
					item4.RewardAssetName = current4 + 1;
				}
				item4.RewardGeometryConfig = dataController.Get<CurrencyIconVO>(item4.RewardAssetName);
				item4.Type = RewardType.Currency;
				list.Add(item4);
			}
			if (rewardVO.HeroUnlocks != null)
			{
				int i = 0;
				int num4 = rewardVO.HeroUnlocks.Length;
				while (i < num4)
				{
					RewardComponentTag item5 = default(RewardComponentTag);
					TroopTypeVO troopTypeVO3 = dataController.Get<TroopTypeVO>(rewardVO.HeroUnlocks[i]);
					item5.RewardName = LangUtils.GetTroopDisplayName(troopTypeVO3);
					item5.Quantity = Service.Get<Lang>().Get("LABEL_REWARD_UNLOCKED", new object[0]);
					item5.RewardAssetName = troopTypeVO3.AssetName;
					item5.RewardGeometryConfig = troopTypeVO3;
					item5.Type = RewardType.Troop;
					list.Add(item5);
					i++;
				}
			}
			if (rewardVO.TroopUnlocks != null)
			{
				int j = 0;
				int num5 = rewardVO.TroopUnlocks.Length;
				while (j < num5)
				{
					RewardComponentTag item6 = default(RewardComponentTag);
					TroopTypeVO troopTypeVO4 = dataController.Get<TroopTypeVO>(rewardVO.TroopUnlocks[j]);
					item6.RewardName = LangUtils.GetTroopDisplayName(troopTypeVO4);
					item6.Quantity = Service.Get<Lang>().Get("LABEL_REWARD_UNLOCKED", new object[0]);
					item6.RewardAssetName = troopTypeVO4.AssetName;
					item6.RewardGeometryConfig = troopTypeVO4;
					item6.Type = RewardType.Troop;
					list.Add(item6);
					j++;
				}
			}
			if (rewardVO.SpecialAttackUnlocks != null)
			{
				int k = 0;
				int num6 = rewardVO.SpecialAttackUnlocks.Length;
				while (k < num6)
				{
					RewardComponentTag item7 = default(RewardComponentTag);
					SpecialAttackTypeVO specialAttackTypeVO2 = dataController.Get<SpecialAttackTypeVO>(rewardVO.SpecialAttackUnlocks[k]);
					item7.RewardName = LangUtils.GetStarshipDisplayName(specialAttackTypeVO2);
					item7.Quantity = Service.Get<Lang>().Get("LABEL_REWARD_UNLOCKED", new object[0]);
					item7.RewardAssetName = specialAttackTypeVO2.AssetName;
					item7.RewardGeometryConfig = specialAttackTypeVO2;
					item7.Type = RewardType.Troop;
					list.Add(item7);
					k++;
				}
			}
			if (rewardVO.BuildingUnlocks != null)
			{
				int l = 0;
				int num7 = rewardVO.BuildingUnlocks.Length;
				while (l < num7)
				{
					RewardComponentTag item8 = default(RewardComponentTag);
					BuildingTypeVO buildingTypeVO = dataController.Get<BuildingTypeVO>(rewardVO.BuildingUnlocks[l]);
					item8.RewardName = LangUtils.GetBuildingDisplayName(buildingTypeVO);
					item8.Quantity = Service.Get<Lang>().Get("LABEL_REWARD_UNLOCKED", new object[0]);
					item8.RewardAssetName = buildingTypeVO.AssetName;
					item8.RewardGeometryConfig = buildingTypeVO;
					item8.Type = RewardType.Building;
					list.Add(item8);
					l++;
				}
			}
			if (!string.IsNullOrEmpty(rewardVO.DroidRewards))
			{
				RewardComponentTag item9 = default(RewardComponentTag);
				TroopTypeVO troopTypeVO5 = dataController.Get<TroopTypeVO>("troopWorkerDroid");
				item9.RewardName = LangUtils.GetTroopDisplayName(troopTypeVO5);
				item9.Quantity = rewardVO.DroidRewards;
				item9.RewardAssetName = troopTypeVO5.AssetName;
				item9.RewardGeometryConfig = troopTypeVO5;
				item9.Type = RewardType.BuilderDroid;
				list.Add(item9);
			}
			dictionary = GameUtils.ListToMap(rewardVO.BuildingInstantRewards);
			foreach (string current5 in dictionary.Keys)
			{
				RewardComponentTag item10 = default(RewardComponentTag);
				RewardUtils.GetBuildingReward(current5, ref item10);
				item10.Quantity = Service.Get<Lang>().Get("TROOP_MULTIPLIER", new object[]
				{
					dictionary[current5]
				});
				list.Add(item10);
			}
			if (rewardVO.BuildingInstantUpgrades != null)
			{
				int m = 0;
				int num8 = rewardVO.BuildingInstantUpgrades.Length;
				while (m < num8)
				{
					RewardComponentTag item11 = default(RewardComponentTag);
					string[] array = RewardUtils.ParsePairedStrings(rewardVO.BuildingInstantUpgrades[m], ':');
					RewardUtils.GetBuildingReward(array[0], ref item11);
					item11.Quantity = Service.Get<Lang>().Get("LABEL_REWARD_UPGRADE", new object[0]);
					list.Add(item11);
					m++;
				}
			}
			if (rewardVO.HeroResearchInstantUpgrades != null)
			{
				int n = 0;
				int num9 = rewardVO.HeroResearchInstantUpgrades.Length;
				while (n < num9)
				{
					RewardComponentTag item12 = default(RewardComponentTag);
					RewardUtils.GetInstantTroopReward(rewardVO.HeroResearchInstantUpgrades[n], ref item12);
					list.Add(item12);
					n++;
				}
			}
			if (rewardVO.TroopResearchInstantUpgrades != null)
			{
				int num10 = 0;
				int num11 = rewardVO.TroopResearchInstantUpgrades.Length;
				while (num10 < num11)
				{
					RewardComponentTag item13 = default(RewardComponentTag);
					RewardUtils.GetInstantTroopReward(rewardVO.TroopResearchInstantUpgrades[num10], ref item13);
					list.Add(item13);
					num10++;
				}
			}
			if (rewardVO.SpecAttackResearchInstantUpgrades != null)
			{
				int num12 = 0;
				int num13 = rewardVO.SpecAttackResearchInstantUpgrades.Length;
				while (num12 < num13)
				{
					RewardComponentTag item14 = default(RewardComponentTag);
					RewardUtils.GetInstantSpecialAttackReward(rewardVO.SpecAttackResearchInstantUpgrades[num12], ref item14);
					list.Add(item14);
					num12++;
				}
			}
			return list;
		}

		public static void GetInstantTroopReward(string rewardString, ref RewardComponentTag rct)
		{
			IDataController dataController = Service.Get<IDataController>();
			string[] array = RewardUtils.ParsePairedStrings(rewardString, ':');
			string uid = array[0];
			TroopTypeVO optional = dataController.GetOptional<TroopTypeVO>(uid);
			if (optional != null)
			{
				rct.RewardName = LangUtils.GetTroopDisplayName(optional);
				rct.Quantity = Service.Get<Lang>().Get("LABEL_REWARD_UPGRADE", new object[0]);
				rct.RewardAssetName = optional.AssetName;
				rct.RewardGeometryConfig = optional;
				rct.Type = RewardType.Troop;
			}
		}

		public static void GetInstantSpecialAttackReward(string rewardString, ref RewardComponentTag rct)
		{
			IDataController dataController = Service.Get<IDataController>();
			string[] array = RewardUtils.ParsePairedStrings(rewardString, ':');
			string uid = array[0];
			SpecialAttackTypeVO optional = dataController.GetOptional<SpecialAttackTypeVO>(uid);
			if (optional != null)
			{
				rct.RewardName = LangUtils.GetStarshipDisplayName(optional);
				rct.Quantity = Service.Get<Lang>().Get("LABEL_REWARD_UPGRADE", new object[0]);
				rct.RewardAssetName = optional.AssetName;
				rct.RewardGeometryConfig = optional;
				rct.Type = RewardType.Troop;
			}
		}

		public static void GetBuildingReward(string buildindId, ref RewardComponentTag rct)
		{
			IDataController dataController = Service.Get<IDataController>();
			BuildingTypeVO buildingTypeVO = dataController.Get<BuildingTypeVO>(buildindId);
			if (buildingTypeVO.LinkedUnit != null)
			{
				TroopTypeVO troopTypeVO = Service.Get<ChampionController>().FindChampionTypeIfPlatform(buildingTypeVO);
				rct.RewardName = LangUtils.GetTroopDisplayName(troopTypeVO);
				rct.RewardAssetName = troopTypeVO.AssetName;
				rct.RewardGeometryConfig = troopTypeVO;
				rct.Type = RewardType.Troop;
				return;
			}
			rct.RewardName = LangUtils.GetBuildingDisplayName(buildingTypeVO);
			rct.RewardAssetName = buildingTypeVO.AssetName;
			rct.RewardGeometryConfig = buildingTypeVO;
			rct.Type = RewardType.Building;
		}

		public static bool SetupTargetedOfferCrateRewardDisplay(RewardVO rewardVO, UXLabel itemLabel, UXSprite itemSprite)
		{
			if (string.IsNullOrEmpty(rewardVO.CrateReward))
			{
				return false;
			}
			CrateVO optional = Service.Get<IDataController>().GetOptional<CrateVO>(rewardVO.CrateReward);
			if (optional != null)
			{
				itemLabel.Text = LangUtils.GetCrateDisplayName(optional);
				RewardUtils.SetCrateIcon(itemSprite, optional, AnimState.Closed);
				return true;
			}
			return false;
		}

		public static void SetRewardIcon(UXSprite sprite, IGeometryVO config, AnimationPreference animPreference = AnimationPreference.NoAnimation)
		{
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(config, sprite);
			projectorConfig.AnimPreference = animPreference;
			ProjectorUtils.GenerateProjector(projectorConfig);
		}

		public static void SetCrateIcon(UXSprite sprite, CrateVO crateVO, AnimState animState)
		{
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(crateVO, sprite);
			if (GameConstants.CRATE_SHOW_VFX && !string.IsNullOrEmpty(crateVO.VfxAssetName) && animState == AnimState.Idle)
			{
				projectorConfig.AttachmentAssets = new string[]
				{
					crateVO.VfxAssetName
				};
				projectorConfig.AnimPreference = AnimationPreference.AnimationAlways;
			}
			else
			{
				projectorConfig.AnimPreference = AnimationPreference.NoAnimation;
			}
			projectorConfig.AnimState = animState;
			ProjectorUtils.GenerateProjector(projectorConfig);
		}

		public static RewardabilityResult CanPlayerHandleReward(CurrentPlayer cp, RewardVO reward, bool checkCurrencyCapacity)
		{
			RewardabilityResult rewardabilityResult = new RewardabilityResult();
			IDataController dataController = Service.Get<IDataController>();
			if (reward.TroopRewards != null)
			{
				int num = 0;
				int num2 = cp.Inventory.Troop.GetTotalStorageCapacity() - cp.Inventory.Troop.GetTotalStorageAmount();
				for (int i = 0; i < reward.TroopRewards.Length; i++)
				{
					string[] array = reward.TroopRewards[i].Split(new char[]
					{
						':'
					});
					TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(array[0]);
					num += troopTypeVO.Size * Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
				}
				if (num > num2)
				{
					rewardabilityResult.CanAward = false;
					rewardabilityResult.Reason = "NOT_ENOUGH_HOUSING";
				}
			}
			if (reward.HeroRewards != null)
			{
				int num = 0;
				int num2 = cp.Inventory.Hero.GetTotalStorageCapacity() - cp.Inventory.Hero.GetTotalStorageAmount();
				for (int i = 0; i < reward.HeroRewards.Length; i++)
				{
					string[] array = reward.HeroRewards[i].Split(new char[]
					{
						':'
					});
					TroopTypeVO troopTypeVO2 = dataController.Get<TroopTypeVO>(array[0]);
					num += troopTypeVO2.Size * Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
				}
				if (num > num2)
				{
					rewardabilityResult.CanAward = false;
					rewardabilityResult.Reason = "NOT_ENOUGH_SPACE";
				}
			}
			if (reward.SpecialAttackRewards != null)
			{
				int num = 0;
				int num2 = cp.Inventory.SpecialAttack.GetTotalStorageCapacity() - cp.Inventory.SpecialAttack.GetTotalStorageAmount();
				for (int i = 0; i < reward.SpecialAttackRewards.Length; i++)
				{
					string[] array = reward.SpecialAttackRewards[i].Split(new char[]
					{
						':'
					});
					SpecialAttackTypeVO specialAttackTypeVO = dataController.Get<SpecialAttackTypeVO>(array[0]);
					num += specialAttackTypeVO.Size * Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
				}
				if (num > num2)
				{
					rewardabilityResult.CanAward = false;
					rewardabilityResult.Reason = "NOT_ENOUGH_SPACE";
				}
			}
			if (reward.CurrencyRewards != null & checkCurrencyCapacity)
			{
				int num3 = cp.CurrentReputationAmount;
				int maxReputationAmount = cp.MaxReputationAmount;
				int num4 = cp.CurrentContrabandAmount;
				int maxContrabandAmount = cp.MaxContrabandAmount;
				int num5 = cp.CurrentMaterialsAmount;
				int maxMaterialsAmount = cp.MaxMaterialsAmount;
				int num6 = cp.CurrentCreditsAmount;
				int maxCreditsAmount = cp.MaxCreditsAmount;
				for (int i = 0; i < reward.CurrencyRewards.Length; i++)
				{
					string[] array = reward.CurrencyRewards[i].Split(new char[]
					{
						':'
					});
					if (array[0] == "contraband")
					{
						num4 += int.Parse(array[1]);
						if (num4 > maxContrabandAmount)
						{
							rewardabilityResult.CanAward = false;
							rewardabilityResult.Reason = "INVENTORY_NO_ROOM";
							return rewardabilityResult;
						}
					}
					if (array[0] == "reputation")
					{
						num3 += int.Parse(array[1]);
						if (num3 > maxReputationAmount)
						{
							rewardabilityResult.CanAward = false;
							rewardabilityResult.Reason = "INVENTORY_NO_ROOM";
							return rewardabilityResult;
						}
					}
					if (array[0] == "materials")
					{
						num5 += int.Parse(array[1]);
						if (num5 > maxMaterialsAmount)
						{
							rewardabilityResult.CanAward = false;
							rewardabilityResult.Reason = "INVENTORY_NO_ROOM";
							return rewardabilityResult;
						}
					}
					if (array[0] == "credits")
					{
						num6 += int.Parse(array[1]);
						if (num6 > maxCreditsAmount)
						{
							rewardabilityResult.CanAward = false;
							rewardabilityResult.Reason = "INVENTORY_NO_ROOM";
							return rewardabilityResult;
						}
					}
				}
			}
			return rewardabilityResult;
		}

		public static void GrantInAppPurchaseRewardToHQInventory(RewardVO reward)
		{
			UnlockController unlockController = Service.Get<UnlockController>();
			unlockController.GrantBuildingUnlockReward(reward.BuildingUnlocks);
			unlockController.GrantTroopUnlockReward(reward.TroopUnlocks);
			unlockController.GrantTroopUnlockReward(reward.HeroUnlocks);
			unlockController.GrantSpecialAttackUnlockReward(reward.SpecialAttackUnlocks);
			GameUtils.AddRewardToInventory(reward);
		}

		public static void GrantReward(CurrentPlayer cp, RewardVO reward, double saleBonusMultiplier)
		{
			UnlockController unlockController = Service.Get<UnlockController>();
			unlockController.GrantBuildingUnlockReward(reward.BuildingUnlocks);
			unlockController.GrantTroopUnlockReward(reward.TroopUnlocks);
			unlockController.GrantTroopUnlockReward(reward.HeroUnlocks);
			unlockController.GrantSpecialAttackUnlockReward(reward.SpecialAttackUnlocks);
			if (reward.CurrencyRewards != null)
			{
				int i = 0;
				int num = reward.CurrencyRewards.Length;
				while (i < num)
				{
					string[] array = reward.CurrencyRewards[i].Split(new char[]
					{
						':'
					});
					int itemCapacity = cp.Inventory.GetItemCapacity(array[0]);
					int itemAmount = cp.Inventory.GetItemAmount(array[0]);
					int num2 = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
					if (saleBonusMultiplier > 1.0)
					{
						num2 = (int)Math.Floor((double)num2 * saleBonusMultiplier);
					}
					if (itemCapacity != -1)
					{
						int val = itemCapacity - itemAmount;
						int delta = Math.Min(val, num2);
						cp.Inventory.ModifyItemAmount(array[0], delta);
					}
					else
					{
						cp.Inventory.ModifyItemAmount(array[0], num2);
					}
					i++;
				}
			}
			if (reward.ShardRewards != null)
			{
				ArmoryController armoryController = Service.Get<ArmoryController>();
				int i = 0;
				int num = reward.ShardRewards.Length;
				while (i < num)
				{
					string[] array2 = reward.ShardRewards[i].Split(new char[]
					{
						':'
					});
					int count = Convert.ToInt32(array2[1], CultureInfo.InvariantCulture);
					armoryController.HandleEarnedShardReward(array2[0], count);
					i++;
				}
			}
			if (reward.TroopRewards != null)
			{
				int i = 0;
				int num = reward.TroopRewards.Length;
				while (i < num)
				{
					string[] array3 = reward.TroopRewards[i].Split(new char[]
					{
						':'
					});
					int delta2 = Convert.ToInt32(array3[1], CultureInfo.InvariantCulture);
					cp.Inventory.Troop.ModifyItemAmount(array3[0], delta2);
					i++;
				}
			}
			if (reward.HeroRewards != null)
			{
				int i = 0;
				int num = reward.HeroRewards.Length;
				while (i < num)
				{
					string[] array4 = reward.HeroRewards[i].Split(new char[]
					{
						':'
					});
					int delta3 = Convert.ToInt32(array4[1], CultureInfo.InvariantCulture);
					cp.Inventory.Hero.ModifyItemAmount(array4[0], delta3);
					i++;
				}
			}
			if (reward.SpecialAttackRewards != null)
			{
				int i = 0;
				int num = reward.SpecialAttackRewards.Length;
				while (i < num)
				{
					string[] array5 = reward.SpecialAttackRewards[i].Split(new char[]
					{
						':'
					});
					int delta4 = Convert.ToInt32(array5[1], CultureInfo.InvariantCulture);
					cp.Inventory.SpecialAttack.ModifyItemAmount(array5[0], delta4);
					i++;
				}
			}
			if (!string.IsNullOrEmpty(reward.DroidRewards))
			{
				int delta5 = Convert.ToInt32(reward.DroidRewards, CultureInfo.InvariantCulture);
				cp.Inventory.ModifyDroids(delta5);
			}
			if (reward.BuildingInstantRewards != null)
			{
				int i = 0;
				int num = reward.BuildingInstantRewards.Length;
				while (i < num)
				{
					string[] array6 = reward.BuildingInstantRewards[i].Split(new char[]
					{
						':'
					});
					int num3 = Convert.ToInt32(array6[1], CultureInfo.InvariantCulture);
					string text = array6[0];
					BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(text);
					if (buildingTypeVO == null)
					{
						Service.Get<StaRTSLogger>().WarnFormat("buildingUiD {0} does not exist", new object[]
						{
							text
						});
					}
					else
					{
						for (int j = 0; j < num3; j++)
						{
							Service.Get<BuildingController>().PlaceRewardedBuilding(buildingTypeVO);
						}
					}
					i++;
				}
			}
			if (reward.BuildingInstantUpgrades != null)
			{
				RewardUtils.GrantInstantBuildingUpgrade(reward, cp);
			}
			if (reward.HeroResearchInstantUpgrades != null)
			{
				int i = 0;
				int num = reward.HeroResearchInstantUpgrades.Length;
				while (i < num)
				{
					RewardUtils.GrantInstantTroopHeroUpgrade(reward.HeroResearchInstantUpgrades[i]);
					i++;
				}
			}
			if (reward.TroopResearchInstantUpgrades != null)
			{
				int i = 0;
				int num = reward.TroopResearchInstantUpgrades.Length;
				while (i < num)
				{
					RewardUtils.GrantInstantTroopHeroUpgrade(reward.TroopResearchInstantUpgrades[i]);
					i++;
				}
			}
			if (reward.SpecAttackResearchInstantUpgrades != null)
			{
				int i = 0;
				int num = reward.SpecAttackResearchInstantUpgrades.Length;
				while (i < num)
				{
					RewardUtils.GrantInstantSpecialAttackUpgrade(reward.SpecAttackResearchInstantUpgrades[i]);
					i++;
				}
			}
		}

		public static void GrantReward(CurrentPlayer cp, RewardVO reward)
		{
			RewardUtils.GrantReward(cp, reward, 1.0);
		}

		public static bool IsUnlockReward(RewardVO rewardVO)
		{
			return rewardVO.BuildingUnlocks != null || rewardVO.TroopUnlocks != null || rewardVO.SpecialAttackUnlocks != null || rewardVO.HeroUnlocks != null;
		}

		public static string[] ParsePairedStrings(string fullstring, char delimiter)
		{
			return fullstring.Split(new char[]
			{
				delimiter
			});
		}

		public static void GrantInstantTroopHeroUpgrade(string rewardString)
		{
			IDataController dataController = Service.Get<IDataController>();
			UnlockedLevelData unlockedLevels = Service.Get<CurrentPlayer>().UnlockedLevels;
			string[] array = RewardUtils.ParsePairedStrings(rewardString, ':');
			string uid = array[0];
			int level = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
			TroopTypeVO optional = dataController.GetOptional<TroopTypeVO>(uid);
			if (optional != null)
			{
				TroopUpgradeCatalog troopUpgradeCatalog = Service.Get<TroopUpgradeCatalog>();
				TroopTypeVO byLevel = troopUpgradeCatalog.GetByLevel(optional, level);
				if (byLevel != null)
				{
					unlockedLevels.Troops.SetLevel(byLevel);
					return;
				}
			}
			else
			{
				Service.Get<StaRTSLogger>().WarnFormat("Instant unit upgrade {0} does not exist", new object[]
				{
					rewardString
				});
			}
		}

		public static void GrantInstantSpecialAttackUpgrade(string rewardString)
		{
			IDataController dataController = Service.Get<IDataController>();
			UnlockedLevelData unlockedLevels = Service.Get<CurrentPlayer>().UnlockedLevels;
			string[] array = RewardUtils.ParsePairedStrings(rewardString, ':');
			string uid = array[0];
			int level = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
			SpecialAttackTypeVO optional = dataController.GetOptional<SpecialAttackTypeVO>(uid);
			if (optional != null)
			{
				StarshipUpgradeCatalog starshipUpgradeCatalog = Service.Get<StarshipUpgradeCatalog>();
				SpecialAttackTypeVO byLevel = starshipUpgradeCatalog.GetByLevel(optional, level);
				if (byLevel != null)
				{
					unlockedLevels.Starships.SetLevel(byLevel);
					return;
				}
			}
			else
			{
				Service.Get<StaRTSLogger>().WarnFormat("Instant spec Attack upgrade {0} does not exist", new object[]
				{
					rewardString
				});
			}
		}

		public static void GrantInstantBuildingUpgrade(RewardVO reward, CurrentPlayer cp)
		{
			for (int i = 0; i < reward.BuildingInstantUpgrades.Length; i++)
			{
				string[] array = reward.BuildingInstantUpgrades[i].Split(new char[]
				{
					':'
				});
				int num = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
				string text = array[0];
				BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(text);
				if (buildingTypeVO == null)
				{
					Service.Get<StaRTSLogger>().WarnFormat("buildingUiD {0} does not exist", new object[]
					{
						text
					});
				}
				else
				{
					BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
					ISupportController supportController = Service.Get<ISupportController>();
					NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
					for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
					{
						BuildingTypeVO buildingType = buildingNode.BuildingComp.BuildingType;
						if (buildingType.Lvl < num && buildingType.Type == buildingTypeVO.Type && buildingType.Type != BuildingType.Clearable)
						{
							BuildingTypeVO byLevel = buildingUpgradeCatalog.GetByLevel(buildingType.UpgradeGroup, num);
							if (byLevel != null && byLevel.PlayerFacing)
							{
								if (!string.IsNullOrEmpty(buildingTypeVO.LinkedUnit))
								{
									if (ContractUtils.IsChampionRepairing(buildingNode.Entity))
									{
										supportController.FinishCurrentContract(buildingNode.Entity, true);
									}
									if (cp.Inventory.Champion.GetItemAmount(buildingTypeVO.LinkedUnit) == 0)
									{
										cp.OnChampionRepaired(buildingTypeVO.LinkedUnit);
									}
								}
								supportController.StartBuildingUpgrade(byLevel, buildingNode.Entity, true);
								int boardX = buildingNode.Entity.Get<BoardItemComponent>().BoardItem.BoardX;
								int boardZ = buildingNode.Entity.Get<BoardItemComponent>().BoardItem.BoardZ;
								float x;
								float z;
								EditBaseController.BuildingBoardToWorld(buildingNode.Entity, boardX, boardZ, out x, out z);
								Vector3 worldLocation = new Vector3(x, 0f, z);
								worldLocation.x = Units.BoardToWorldX(boardX);
								worldLocation.z = Units.BoardToWorldX(boardZ);
								Service.Get<WorldInitializer>().View.PanToLocation(worldLocation);
							}
						}
					}
				}
			}
		}

		public static int GetShardsRewarded(RewardVO reward)
		{
			int result = 0;
			if (reward.ShardRewards != null && reward.ShardRewards.Length != 0)
			{
				string[] array = reward.ShardRewards[0].Split(new char[]
				{
					':'
				});
				result = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
			}
			return result;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardUtils.CanPlayerHandleReward((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (RewardVO)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardUtils.GetRewardComponents((RewardVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardUtils.GetShardsRewarded((RewardVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			RewardUtils.GrantInAppPurchaseRewardToHQInventory((RewardVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			RewardUtils.GrantInstantBuildingUpgrade((RewardVO)GCHandledObjects.GCHandleToObject(*args), (CurrentPlayer)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			RewardUtils.GrantInstantSpecialAttackUpgrade(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			RewardUtils.GrantInstantTroopHeroUpgrade(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			RewardUtils.GrantReward((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (RewardVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			RewardUtils.GrantReward((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (RewardVO)GCHandledObjects.GCHandleToObject(args[1]), *(double*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardUtils.IsUnlockReward((RewardVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			RewardUtils.SetCrateIcon((UXSprite)GCHandledObjects.GCHandleToObject(*args), (CrateVO)GCHandledObjects.GCHandleToObject(args[1]), (AnimState)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			RewardUtils.SetRewardIcon((UXSprite)GCHandledObjects.GCHandleToObject(*args), (IGeometryVO)GCHandledObjects.GCHandleToObject(args[1]), (AnimationPreference)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardUtils.SetupTargetedOfferCrateRewardDisplay((RewardVO)GCHandledObjects.GCHandleToObject(*args), (UXLabel)GCHandledObjects.GCHandleToObject(args[1]), (UXSprite)GCHandledObjects.GCHandleToObject(args[2])));
		}
	}
}
