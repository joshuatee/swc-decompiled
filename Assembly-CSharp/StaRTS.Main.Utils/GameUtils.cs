using Net.RichardLord.Ash.Core;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.EnvironmentManager;
using StaRTS.Externals.FileManagement;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.GameBoard;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Crates;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Commands.Player.Store;
using StaRTS.Main.Models.Commands.Pvp;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Leaderboard;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class GameUtils
	{
		private const int INFINITE_TROOP_CAPACITY = 9999;

		private const uint FRAME_COUNT_OFFSET_FOR_NEXT_TARGETING = 30u;

		private const string PERK_VO_PREFIX = "perk_";

		public static GamePlayer GetWorldOwner()
		{
			if (GameUtils.IsVisitingNeighbor())
			{
				return Service.Get<NeighborVisitManager>().NeighborPlayer;
			}
			return Service.Get<CurrentPlayer>();
		}

		public static bool IsVisitingNeighbor()
		{
			return Service.Get<GameStateMachine>().CurrentState is NeighborVisitState;
		}

		public static bool IsVisitingBase()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			return currentState is NeighborVisitState || (currentState is BattleStartState && Service.Get<SquadController>().WarManager.GetCurrentStatus() == SquadWarStatusType.PhasePrep);
		}

		public static Dictionary<string, int> ListToMap(string[] list)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			if (list != null)
			{
				int i = 0;
				int num = list.Length;
				while (i < num)
				{
					string item = list[i];
					string key;
					int value;
					if (GameUtils.ParseMapString(item, out key, out value))
					{
						dictionary[key] = value;
					}
					i++;
				}
			}
			return dictionary;
		}

		public static int CalculatePlayerVictoryRating(GamePlayer player)
		{
			int result = 0;
			if (player != null)
			{
				result = GameUtils.CalculateVictoryRating(player.AttacksWon, player.DefensesWon);
			}
			return result;
		}

		public static int CalculatePvpTargetVictoryRating(PvpTarget target)
		{
			int result = 0;
			if (target != null)
			{
				result = GameUtils.CalculateVictoryRating(target.PlayerAttacksWon, target.PlayerDefensesWon);
			}
			return result;
		}

		public static int CalculateBattleHistoryVictoryRating(LeaderboardBattleHistory history)
		{
			int result = 0;
			if (history != null)
			{
				result = GameUtils.CalculateVictoryRating(history.AttacksWon, history.DefensesWon);
			}
			return result;
		}

		public static int CalculateVictoryRating(int attacksWon, int defensesWon)
		{
			return attacksWon + defensesWon;
		}

		public static bool ParseCurrencyCostString(string costString, out CurrencyType type, out int amount)
		{
			type = CurrencyType.None;
			amount = -1;
			if (string.IsNullOrEmpty(costString))
			{
				Service.Get<StaRTSLogger>().Error("ParseCurrencyCostString failed becuase cost string was null or empty");
				return false;
			}
			string[] array = costString.Split(new char[]
			{
				':'
			});
			if (array.Length <= 1)
			{
				Service.Get<StaRTSLogger>().Error("ParseCurrencyCostString failed becuase cost string was invalid: " + costString);
				return false;
			}
			type = StringUtils.ParseEnum<CurrencyType>(array[0]);
			amount = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
			return true;
		}

		private static bool ParseMapString(string item, out string key, out int val)
		{
			val = 0;
			key = null;
			if (item != null)
			{
				int num = item.IndexOf(':');
				if (num >= 0)
				{
					int.TryParse(StringUtils.Substring(item, num + 1), ref val);
					key = StringUtils.Substring(item, 0, num);
					return true;
				}
			}
			return false;
		}

		public static void ListToAdditiveMap(string[] list, Dictionary<string, int> map)
		{
			if (list != null && map != null)
			{
				int i = 0;
				int num = list.Length;
				while (i < num)
				{
					string item = list[i];
					string key;
					int num2;
					if (GameUtils.ParseMapString(item, out key, out num2))
					{
						int num3 = 0;
						if (map.ContainsKey(key))
						{
							num3 = map[key];
						}
						map[key] = num3 + num2;
					}
					i++;
				}
			}
		}

		public static string GetTransmissionHoloId(FactionType faction, string planetUId)
		{
			IDataController dataController = Service.Get<IDataController>();
			foreach (TransmissionCharacterVO current in dataController.GetAll<TransmissionCharacterVO>())
			{
				if (current.Faction == faction && current.PlanetId == planetUId)
				{
					return current.CharacterId;
				}
			}
			string result = "";
			switch (faction)
			{
			case FactionType.Empire:
				result = "kosh_1";
				return result;
			case FactionType.Rebel:
				result = "jennica_1";
				return result;
			case FactionType.Smuggler:
				result = "";
				return result;
			}
			Service.Get<StaRTSLogger>().Error("Unknown Faction: " + faction.ToString() + " GameUtils::GetBattleLogHoloId");
			return result;
		}

		public static string GetServerTransmissionMessageImage(FactionType faction, string planetUId)
		{
			string result = "";
			IDataController dataController = Service.Get<IDataController>();
			foreach (TransmissionCharacterVO current in dataController.GetAll<TransmissionCharacterVO>())
			{
				if (current.Faction == faction && current.PlanetId == planetUId)
				{
					result = current.Image;
					break;
				}
			}
			return result;
		}

		public static int SquaredDistance(int fromX, int fromZ, int toX, int toZ)
		{
			return (toX - fromX) * (toX - fromX) + (toZ - fromZ) * (toZ - fromZ);
		}

		public static int GetSquaredDistanceToTarget(ShooterComponent shooterComp, SmartEntity target)
		{
			SmartEntity smartEntity = (SmartEntity)shooterComp.Entity;
			if (target == null || smartEntity == null)
			{
				return 2147483647;
			}
			TransformComponent transformComp = smartEntity.TransformComp;
			if (transformComp == null)
			{
				return 2147483647;
			}
			TransformComponent transformComp2 = target.TransformComp;
			if (transformComp2 == null)
			{
				return 2147483647;
			}
			int num = transformComp.CenterGridX();
			int num2 = transformComp.CenterGridZ();
			int fromX;
			int fromZ;
			if (shooterComp.IsMelee)
			{
				fromX = GameUtils.NearestPointOnRect(num, transformComp2.MinX(), transformComp2.MaxX());
				fromZ = GameUtils.NearestPointOnRect(num2, transformComp2.MinZ(), transformComp2.MaxZ());
			}
			else
			{
				fromX = transformComp2.CenterGridX();
				fromZ = transformComp2.CenterGridZ();
			}
			return GameUtils.SquaredDistance(fromX, fromZ, num, num2);
		}

		public static int NearestPointOnRect(int k, int minK, int maxK)
		{
			if (k < minK)
			{
				return minK;
			}
			if (k > maxK)
			{
				return maxK;
			}
			return k;
		}

		public static bool IsBuildingUpgradable(BuildingTypeVO buildingInfo)
		{
			return buildingInfo.Lvl > 0 && buildingInfo.Lvl < Service.Get<BuildingUpgradeCatalog>().GetMaxLevel(buildingInfo.UpgradeGroup).Lvl;
		}

		public static int GetBuildingEffectiveLevel(Entity building)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			BuildingTypeVO buildingType = buildingComponent.BuildingType;
			if (ContractUtils.IsBuildingConstructing(building))
			{
				return 0;
			}
			return buildingType.Lvl;
		}

		public static bool IsBuildingTypeValidForBattleConditions(BuildingType type)
		{
			bool flag = type == BuildingType.Wall;
			bool flag2 = type == BuildingType.Rubble;
			bool flag3 = type == BuildingType.Blocker;
			bool flag4 = type == BuildingType.Clearable;
			return !flag && !flag2 && !flag3 && !flag4;
		}

		public static bool IsBuildingMovable(Entity building)
		{
			if (building == null)
			{
				return false;
			}
			BuildingType type = building.Get<BuildingComponent>().BuildingType.Type;
			return type != BuildingType.Clearable;
		}

		public static string GetEquivalentSlow(BuildingTypeVO currentType, FactionType faction)
		{
			if (currentType.Faction == faction)
			{
				return currentType.Uid;
			}
			IDataController dataController = Service.Get<IDataController>();
			List<BuildingTypeVO> list = new List<BuildingTypeVO>();
			foreach (BuildingTypeVO current in dataController.GetAll<BuildingTypeVO>())
			{
				list.Add(current);
			}
			list.Sort(new Comparison<BuildingTypeVO>(GameUtils.SortBuildingByUID));
			return GameUtils.GetEquivalentFromPreSortedList(list, currentType, faction);
		}

		public static string GetEquivalentFromPreSortedList(List<BuildingTypeVO> sortedBuildings, BuildingTypeVO currentType, FactionType faction)
		{
			int count = sortedBuildings.Count;
			for (int i = 0; i < count; i++)
			{
				BuildingTypeVO buildingTypeVO = sortedBuildings[i];
				if (buildingTypeVO.Faction == faction && buildingTypeVO.Lvl == currentType.Lvl && buildingTypeVO.Type == currentType.Type && buildingTypeVO.SubType == currentType.SubType && ((buildingTypeVO.Type != BuildingType.Resource && buildingTypeVO.Type != BuildingType.Storage && buildingTypeVO.Type != BuildingType.Turret) || (buildingTypeVO.Type == BuildingType.Resource && buildingTypeVO.Currency == currentType.Currency) || (buildingTypeVO.Type == BuildingType.Storage && buildingTypeVO.Currency == currentType.Currency) || (buildingTypeVO.Type == BuildingType.Turret && buildingTypeVO.SubType == currentType.SubType)))
				{
					return buildingTypeVO.Uid;
				}
			}
			Service.Get<StaRTSLogger>().WarnFormat("No equivalent building for {0} in faction {1}", new object[]
			{
				currentType.Uid,
				faction
			});
			return currentType.Uid;
		}

		public static int SortBuildingByUID(BuildingTypeVO bldg1, BuildingTypeVO bldg2)
		{
			return bldg2.Uid.CompareTo(bldg1.Uid);
		}

		public static int GetWorldOwnerTroopCount(string uid)
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			return GameUtils.GetDeployableCount(uid, worldOwner.Inventory.Troop);
		}

		public static int GetWorldOwnerSpecialAttackCount(string uid)
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			return GameUtils.GetDeployableCount(uid, worldOwner.Inventory.SpecialAttack);
		}

		public static int GetWorldOwnerHeroCount(string uid)
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			return GameUtils.GetDeployableCount(uid, worldOwner.Inventory.Hero);
		}

		public static int GetWorldOwnerChampionCount(string uid)
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			return GameUtils.GetDeployableCount(uid, worldOwner.Inventory.Champion);
		}

		public static int GetDeployableCountForUpgradeGroupSpecialAttack(SpecialAttackTypeVO specialAttack)
		{
			int num = 0;
			List<SpecialAttackTypeVO> upgradeGroupLevels = Service.Get<StarshipUpgradeCatalog>().GetUpgradeGroupLevels(specialAttack.UpgradeGroup);
			InventoryStorage specialAttack2 = Service.Get<CurrentPlayer>().Inventory.SpecialAttack;
			for (int i = 0; i < upgradeGroupLevels.Count; i++)
			{
				num += GameUtils.GetDeployableCount(upgradeGroupLevels[i].Uid, specialAttack2);
			}
			return num;
		}

		public static int GetDeployableCountForUpgradeGroupTroop(TroopTypeVO troop)
		{
			int num = 0;
			List<TroopTypeVO> upgradeGroupLevels = Service.Get<TroopUpgradeCatalog>().GetUpgradeGroupLevels(troop.TroopID);
			TroopType type = troop.Type;
			InventoryStorage storage;
			if (type != TroopType.Hero)
			{
				if (type != TroopType.Champion)
				{
					storage = Service.Get<CurrentPlayer>().Inventory.Troop;
				}
				else
				{
					storage = Service.Get<CurrentPlayer>().Inventory.Champion;
				}
			}
			else
			{
				storage = Service.Get<CurrentPlayer>().Inventory.Hero;
			}
			for (int i = 0; i < upgradeGroupLevels.Count; i++)
			{
				num += GameUtils.GetDeployableCount(upgradeGroupLevels[i].Uid, storage);
			}
			return num;
		}

		public static int GetDeployableCount(string uid, InventoryStorage storage)
		{
			return storage.GetItemAmount(uid);
		}

		public static void GetStarportTroopCounts(out int housingSpace, out int housingSpaceTotal)
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			housingSpace = worldOwner.Inventory.Troop.GetTotalStorageAmount();
			housingSpaceTotal = worldOwner.Inventory.Troop.GetTotalStorageCapacity();
			if (housingSpaceTotal == -1)
			{
				housingSpaceTotal = 9999;
			}
		}

		public static void LogComponentsAsError(string message, Entity entity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<ComponentBase> all = entity.GetAll();
			bool flag = false;
			int i = 0;
			int count = all.Count;
			while (i < count)
			{
				ComponentBase componentBase = all[i];
				bool flag2 = componentBase is AssetComponent;
				if (!(flag2 & flag))
				{
					stringBuilder.Append(componentBase.GetType().get_Name());
					if (flag2)
					{
						flag = true;
						stringBuilder.Append('=');
						stringBuilder.Append(((AssetComponent)componentBase).AssetName);
					}
					if (i < count - 1)
					{
						stringBuilder.Append(',');
					}
				}
				i++;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("{0} ({1}): {2}", new object[]
			{
				message,
				entity.ID,
				stringBuilder
			});
		}

		public static bool RectsIntersect(int left1, int right1, int top1, int bottom1, int left2, int right2, int top2, int bottom2)
		{
			return left1 < right2 && right1 > left2 && top1 < bottom2 && bottom1 > top2;
		}

		public static bool RectContainsRect(int left1, int right1, int top1, int bottom1, int left2, int right2, int top2, int bottom2)
		{
			return left2 >= left1 && right2 <= right1 && top2 >= top1 && bottom2 <= bottom1;
		}

		public static int CalcuateMedals(int AttackRating, int DefenseRating)
		{
			return AttackRating + DefenseRating;
		}

		public static string GetTimeLabelFromSeconds(int totalSeconds)
		{
			Lang lang = Service.Get<Lang>();
			int num = totalSeconds / 60;
			int num2 = totalSeconds - num * 60;
			int num3 = num / 60;
			num -= num3 * 60;
			int num4 = num3 / 24;
			num3 -= num4 * 24;
			int num5 = num4 / 7;
			num4 -= num5 * 7;
			if (num5 > 0)
			{
				return lang.Get("WEEKS_DAYS", new object[]
				{
					num5,
					num4
				});
			}
			if (num4 > 0)
			{
				return lang.Get("DAYS_HOURS", new object[]
				{
					num4,
					num3
				});
			}
			if (num3 > 0)
			{
				return lang.Get("HOURS_MINUTES", new object[]
				{
					num3,
					num
				});
			}
			if (num > 0)
			{
				return lang.Get("MINUTES_SECONDS", new object[]
				{
					num,
					num2
				});
			}
			return lang.Get("SECONDS", new object[]
			{
				num2
			});
		}

		public static CurrencyType GetCurrencyType(int credits, int materials, int contraband)
		{
			if (credits != 0)
			{
				return CurrencyType.Credits;
			}
			if (materials != 0)
			{
				return CurrencyType.Materials;
			}
			if (contraband != 0)
			{
				return CurrencyType.Contraband;
			}
			return CurrencyType.None;
		}

		public static bool CanAffordCrystals(int crystals)
		{
			return crystals <= Service.Get<CurrentPlayer>().CurrentCrystalsAmount;
		}

		public static bool CanAffordCredits(int credits)
		{
			return credits <= Service.Get<CurrentPlayer>().CurrentCreditsAmount;
		}

		public static bool CanAffordMaterials(int materials)
		{
			return materials <= Service.Get<CurrentPlayer>().CurrentMaterialsAmount;
		}

		public static bool CanAffordContraband(int contraband)
		{
			return contraband <= Service.Get<CurrentPlayer>().CurrentContrabandAmount;
		}

		public static bool CanAffordReputation(int reputation)
		{
			return reputation <= Service.Get<CurrentPlayer>().CurrentReputationAmount;
		}

		public static bool CanAffordCosts(int credits, int materials, int contraband, int crystals)
		{
			return GameUtils.CanAffordCrystals(crystals) && GameUtils.CanAffordCredits(credits) && GameUtils.CanAffordMaterials(materials) && GameUtils.CanAffordContraband(contraband);
		}

		public static void SpendCurrency(int credits, int materials, int contraband, int reputation, int crystals, bool playSound)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			EventManager eventManager = Service.Get<EventManager>();
			if (credits > 0)
			{
				currentPlayer.Inventory.ModifyCredits(-credits);
				if (playSound)
				{
					eventManager.SendEvent(EventId.AudibleCurrencySpent, CurrencyType.Credits);
				}
			}
			if (materials > 0)
			{
				currentPlayer.Inventory.ModifyMaterials(-materials);
				if (playSound)
				{
					eventManager.SendEvent(EventId.AudibleCurrencySpent, CurrencyType.Materials);
				}
			}
			if (contraband > 0)
			{
				currentPlayer.Inventory.ModifyContraband(-contraband);
				if (playSound)
				{
					eventManager.SendEvent(EventId.AudibleCurrencySpent, CurrencyType.Contraband);
				}
			}
			if (reputation > 0)
			{
				currentPlayer.Inventory.ModifyReputation(-reputation);
				if (playSound)
				{
					eventManager.SendEvent(EventId.AudibleCurrencySpent, CurrencyType.Reputation);
				}
			}
			if (crystals > 0)
			{
				currentPlayer.Inventory.ModifyCrystals(-crystals);
				if (playSound)
				{
					eventManager.SendEvent(EventId.AudibleCurrencySpent, CurrencyType.Crystals);
				}
			}
		}

		public static void SpendCurrencyWithMultiplier(int credits, int materials, int contraband, float multiplier, bool playSound)
		{
			GameUtils.MultiplyCurrency(multiplier, ref credits, ref materials, ref contraband);
			GameUtils.SpendCurrency(credits, materials, contraband, 0, 0, playSound);
		}

		public static void SpendCurrency(int credits, int materials, int contraband, bool playSound)
		{
			GameUtils.SpendCurrency(credits, materials, contraband, 0, 0, playSound);
		}

		public static void SpendHQScaledCurrency(string[] cost, bool playSound)
		{
			int credits;
			int materials;
			int contraband;
			int reputation;
			GameUtils.GetHQScaledCurrency(cost, out credits, out materials, out contraband, out reputation);
			GameUtils.SpendCurrency(credits, materials, contraband, reputation, 0, playSound);
		}

		public static void SpendCurrency(string[] cost, bool playSound)
		{
			int credits;
			int materials;
			int contraband;
			int reputation;
			int crystals;
			GameUtils.GetCurrencyCost(cost, out credits, out materials, out contraband, out reputation, out crystals);
			GameUtils.SpendCurrency(credits, materials, contraband, reputation, crystals, playSound);
		}

		public static void GetHQScaledCurrency(string[] cost, out int credits, out int materials, out int contraband, out int reputation)
		{
			int num = 0;
			credits = 0;
			materials = 0;
			contraband = 0;
			reputation = 0;
			Dictionary<string, int> hQScaledCostForPlayer = GameUtils.GetHQScaledCostForPlayer(cost);
			if (hQScaledCostForPlayer.TryGetValue("credits", out num))
			{
				credits = num;
			}
			if (hQScaledCostForPlayer.TryGetValue("materials", out num))
			{
				materials = num;
			}
			if (hQScaledCostForPlayer.TryGetValue("contraband", out num))
			{
				contraband = num;
			}
			if (hQScaledCostForPlayer.TryGetValue("reputation", out num))
			{
				reputation = num;
			}
		}

		public static void GetCurrencyCost(string[] cost, out int credits, out int materials, out int contraband, out int reputation, out int crystals)
		{
			credits = 0;
			materials = 0;
			contraband = 0;
			reputation = 0;
			crystals = 0;
			Dictionary<string, int> dictionary = GameUtils.ListToMap(cost);
			foreach (string current in dictionary.Keys)
			{
				if (!(current == "credits"))
				{
					if (!(current == "materials"))
					{
						if (!(current == "contraband"))
						{
							if (!(current == "reputation"))
							{
								if (current == "crystals")
								{
									crystals = dictionary[current];
								}
							}
							else
							{
								reputation = dictionary[current];
							}
						}
						else
						{
							contraband = dictionary[current];
						}
					}
					else
					{
						materials = dictionary[current];
					}
				}
				else
				{
					credits = dictionary[current];
				}
			}
		}

		public static bool SpendCrystals(int crystals)
		{
			if (crystals <= 0)
			{
				return false;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (crystals > currentPlayer.CurrentCrystalsAmount)
			{
				GameUtils.PromptToBuyCrystals();
				return false;
			}
			currentPlayer.Inventory.ModifyCrystals(-crystals);
			return true;
		}

		public static void PromptToBuyCrystals()
		{
			Lang lang = Service.Get<Lang>();
			string title = lang.Get("NOT_ENOUGH_CRYSTALS", new object[0]);
			string message = lang.Get("NOT_ENOUGH_CRYSTALS_BUY_MORE", new object[0]);
			AlertScreen.ShowModal(false, title, message, new OnScreenModalResult(GameUtils.OnBuyMoreCrystals), null);
		}

		private static void OnBuyMoreCrystals(object result, object cookie)
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is GalaxyState)
			{
				ScreenController screenController = Service.Get<ScreenController>();
				screenController.CloseAll();
				Service.Get<GalaxyViewController>().GoToHome(true, new WipeCompleteDelegate(GameUtils.ReturnToHomeCompleteNowBuyMoreCrystals), result);
				return;
			}
			if (result != null)
			{
				ScreenController screenController2 = Service.Get<ScreenController>();
				StoreScreen storeScreen = screenController2.GetHighestLevelScreen<StoreScreen>();
				if (storeScreen == null)
				{
					storeScreen = new StoreScreen();
					screenController2.AddScreen(storeScreen);
				}
				storeScreen.SetTab(StoreTab.Treasure);
				Service.Get<EventManager>().SendEvent(EventId.UINotEnoughHardCurrencyBuy, null);
				return;
			}
			Service.Get<EventManager>().SendEvent(EventId.UINotEnoughHardCurrencyClose, null);
		}

		public static void OpenStoreTreasureTab()
		{
			StoreScreen storeScreen = new StoreScreen();
			storeScreen.SetTab(StoreTab.Treasure);
			Service.Get<ScreenController>().AddScreen(storeScreen);
		}

		public static void OpenInventoryCrateModal(CrateData crateData, OnScreenModalResult modalResult)
		{
			CrateInfoModalScreen crateInfoModalScreen = CrateInfoModalScreen.CreateForInventory(crateData);
			crateInfoModalScreen.OnModalResult = modalResult;
			crateInfoModalScreen.IsAlwaysOnTop = true;
			Service.Get<ScreenController>().AddScreen(crateInfoModalScreen, true, false);
		}

		public static void ReturnToHomeCompleteNowBuyMoreCrystals(object cookie)
		{
			GameUtils.OnBuyMoreCrystals(cookie, null);
		}

		public static int DroidCrystalCost(int droidIndex)
		{
			string[] array = GameConstants.DROID_CRYSTAL_COSTS.Split(new char[]
			{
				' '
			});
			int result;
			if (droidIndex < array.Length && int.TryParse(array[droidIndex], ref result))
			{
				return result;
			}
			return -1;
		}

		public static int SecondsToCrystals(int seconds)
		{
			float baseValue = (float)seconds / 3600f;
			int cRYSTALS_SPEED_UP_COEFFICIENT = GameConstants.CRYSTALS_SPEED_UP_COEFFICIENT;
			int cRYSTALS_SPEED_UP_EXPONENT = GameConstants.CRYSTALS_SPEED_UP_EXPONENT;
			return GameUtils.CurrencyPow(baseValue, cRYSTALS_SPEED_UP_COEFFICIENT, cRYSTALS_SPEED_UP_EXPONENT);
		}

		public static int SecondsToCrystalsForPerk(int seconds)
		{
			float baseValue = (float)seconds / 3600f;
			int sQUADPERK_CRYSTALS_SPEED_UP_COEFFICIENT = GameConstants.SQUADPERK_CRYSTALS_SPEED_UP_COEFFICIENT;
			int sQUADPERK_CRYSTALS_SPEED_UP_EXPONENT = GameConstants.SQUADPERK_CRYSTALS_SPEED_UP_EXPONENT;
			return GameUtils.CurrencyPow(baseValue, sQUADPERK_CRYSTALS_SPEED_UP_COEFFICIENT, sQUADPERK_CRYSTALS_SPEED_UP_EXPONENT);
		}

		public static int MultiCurrencyCrystalCost(Dictionary<CurrencyType, int> costMap)
		{
			int num = 0;
			int num2 = 0;
			if (costMap.TryGetValue(CurrencyType.Credits, out num2))
			{
				num += GameUtils.CreditsCrystalCost(num2);
			}
			if (costMap.TryGetValue(CurrencyType.Materials, out num2))
			{
				num += GameUtils.MaterialsCrystalCost(num2);
			}
			if (costMap.TryGetValue(CurrencyType.Contraband, out num2))
			{
				num += GameUtils.ContrabandCrystalCost(num2);
			}
			return num;
		}

		public static int CreditsCrystalCost(int credits)
		{
			int cREDITS_COEFFICIENT = GameConstants.CREDITS_COEFFICIENT;
			int cREDITS_EXPONENT = GameConstants.CREDITS_EXPONENT;
			return GameUtils.CurrencyPow((float)credits, cREDITS_COEFFICIENT, cREDITS_EXPONENT);
		}

		public static int MaterialsCrystalCost(int materials)
		{
			int aLLOY_COEFFICIENT = GameConstants.ALLOY_COEFFICIENT;
			int aLLOY_EXPONENT = GameConstants.ALLOY_EXPONENT;
			return GameUtils.CurrencyPow((float)materials, aLLOY_COEFFICIENT, aLLOY_EXPONENT);
		}

		public static int ContrabandCrystalCost(int contraband)
		{
			int cONTRABAND_COEFFICIENT = GameConstants.CONTRABAND_COEFFICIENT;
			int cONTRABAND_EXPONENT = GameConstants.CONTRABAND_EXPONENT;
			return GameUtils.CurrencyPow((float)contraband, cONTRABAND_COEFFICIENT, cONTRABAND_EXPONENT);
		}

		private static int CurrencyPow(float baseValue, int coefficient, int exponent)
		{
			int cOEF_EXP_ACCURACY = GameConstants.COEF_EXP_ACCURACY;
			if (baseValue < 0f || coefficient <= 0 || exponent <= 0 || cOEF_EXP_ACCURACY <= 0)
			{
				return -1;
			}
			float num = (float)coefficient / (float)cOEF_EXP_ACCURACY;
			float p = (float)exponent / (float)cOEF_EXP_ACCURACY;
			return (int)Mathf.Ceil(num * Mathf.Pow(baseValue, p));
		}

		public static int CrystalCostToUpgradeAllWalls(int oneWallCost, int numWalls)
		{
			int num = oneWallCost * numWalls;
			int uPGRADE_ALL_WALLS_COEFFICIENT = GameConstants.UPGRADE_ALL_WALLS_COEFFICIENT;
			int uPGRADE_ALL_WALL_EXPONENT = GameConstants.UPGRADE_ALL_WALL_EXPONENT;
			int num2 = GameUtils.CurrencyPow((float)num, uPGRADE_ALL_WALLS_COEFFICIENT, uPGRADE_ALL_WALL_EXPONENT);
			return (int)Mathf.Ceil((float)num2 * GameConstants.UPGRADE_ALL_WALLS_CONVENIENCE_TAX);
		}

		public static int CrystalCostToInstantUpgrade(BuildingTypeVO nextBuildingInfo)
		{
			return GameUtils.CreditsCrystalCost(nextBuildingInfo.UpgradeCredits) + GameUtils.MaterialsCrystalCost(nextBuildingInfo.UpgradeMaterials) + GameUtils.ContrabandCrystalCost(nextBuildingInfo.UpgradeContraband) + GameUtils.SecondsToCrystals(nextBuildingInfo.Time);
		}

		public static void GetCrystalPacks(out int[] amounts, out int[] prices)
		{
			string[] array = GameConstants.CRYSTAL_PACK_AMOUNT.Split(new char[]
			{
				' '
			});
			string[] array2 = GameConstants.CRYSTAL_PACK_COST_USD.Split(new char[]
			{
				' '
			});
			int num = array.Length;
			if (num != array2.Length)
			{
				amounts = new int[0];
				prices = new int[0];
				return;
			}
			amounts = new int[num];
			prices = new int[num];
			for (int i = 0; i < num; i++)
			{
				int num2;
				int num3;
				if (!int.TryParse(array[i], ref num2) || !int.TryParse(array2[i], ref num3))
				{
					amounts = new int[0];
					prices = new int[0];
					return;
				}
				amounts[i] = num2;
				prices[i] = num3;
			}
		}

		public static void GetProtectionPacks(out int[] durations, out int[] crystals)
		{
			string[] array = GameConstants.PROTECTION_DURATION.Split(new char[]
			{
				' '
			});
			string[] array2 = GameConstants.PROTECTION_CRYSTAL_COSTS.Split(new char[]
			{
				' '
			});
			int num = array.Length;
			if (num != array2.Length)
			{
				durations = new int[0];
				crystals = new int[0];
				return;
			}
			durations = new int[num];
			crystals = new int[num];
			for (int i = 0; i < num; i++)
			{
				int num2;
				int num3;
				if (!int.TryParse(array[i], ref num2) || !int.TryParse(array2[i], ref num3))
				{
					durations = new int[0];
					crystals = new int[0];
					return;
				}
				durations[i] = num2;
				crystals[i] = num3;
			}
		}

		public static bool HasEnoughCurrencyStorage(CurrencyType currency, int amount)
		{
			bool flag = true;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			switch (currency)
			{
			case CurrencyType.Credits:
				flag = (currentPlayer.CurrentCreditsAmount + amount <= currentPlayer.MaxCreditsAmount);
				break;
			case CurrencyType.Materials:
				flag = (currentPlayer.CurrentMaterialsAmount + amount <= currentPlayer.MaxMaterialsAmount);
				break;
			case CurrencyType.Contraband:
				flag = (currentPlayer.CurrentContrabandAmount + amount <= currentPlayer.MaxContrabandAmount);
				break;
			}
			if (!flag)
			{
				GameUtils.ShowNotEnoughStorageMessage(currency);
			}
			return flag;
		}

		public static void ShowNotEnoughStorageMessage(CurrencyType currency)
		{
			Lang lang = Service.Get<Lang>();
			string instructions = lang.Get("NOT_ENOUGH_STORAGE", new object[]
			{
				lang.Get(currency.ToString().ToUpper(), new object[0])
			});
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(instructions);
		}

		public static int GetProtectionTimeRemaining()
		{
			uint protectedUntil = Service.Get<CurrentPlayer>().ProtectedUntil;
			uint serverTime = Service.Get<ServerAPI>().ServerTime;
			if (protectedUntil > serverTime)
			{
				return (int)(protectedUntil - serverTime);
			}
			return 0;
		}

		public static bool BuyLEI(CurrentPlayer player, LimitedEditionItemVO itemVO)
		{
			bool flag = false;
			if (itemVO.Crystals > 0)
			{
				flag = GameUtils.SpendCrystals(itemVO.Crystals);
			}
			else
			{
				bool flag2 = player.CurrentCreditsAmount > itemVO.Credits && player.CurrentMaterialsAmount > itemVO.Materials && player.CurrentContrabandAmount > itemVO.Contraband;
				if (flag2)
				{
					GameUtils.SpendCurrency(itemVO.Credits, itemVO.Materials, itemVO.Contraband, true);
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			ProcessingScreen.Show();
			BuyLimitedEditionItemRequest request = new BuyLimitedEditionItemRequest(itemVO.Uid);
			BuyLimitedEditionItemCommand buyLimitedEditionItemCommand = new BuyLimitedEditionItemCommand(request);
			buyLimitedEditionItemCommand.AddSuccessCallback(new AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>.OnSuccessCallback(GameUtils.HandleCratePurchaseResponse));
			Service.Get<ServerAPI>().Sync(buyLimitedEditionItemCommand);
			return true;
		}

		public static bool BuyCrate(CurrentPlayer player, CrateVO crateVO)
		{
			if (!crateVO.Purchasable)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Crate '{0}' is not purchasable", new object[]
				{
					crateVO.Uid
				});
				return false;
			}
			int num = player.ArmoryInfo.FirstCratePurchased ? crateVO.Crystals : 0;
			if (num > 0 && !GameUtils.SpendCrystals(num))
			{
				Service.Get<EventManager>().SendEvent(EventId.CrateStoreNotEnoughCurrency, crateVO.Uid);
				return false;
			}
			ProcessingScreen.Show();
			player.ArmoryInfo.FirstCratePurchased = true;
			BuyCrateRequest request = new BuyCrateRequest(crateVO.Uid);
			BuyCrateCommand buyCrateCommand = new BuyCrateCommand(request);
			buyCrateCommand.AddSuccessCallback(new AbstractCommand<BuyCrateRequest, CrateDataResponse>.OnSuccessCallback(GameUtils.HandleCratePurchaseResponse));
			Service.Get<ServerAPI>().Sync(buyCrateCommand);
			Service.Get<EventManager>().SendEvent(EventId.CrateStorePurchase, crateVO.Uid);
			return true;
		}

		private static void HandleCratePurchaseResponse(CrateDataResponse response, object cookie)
		{
			if (response.CrateDataTO != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.OpeningPurchasedCrate, null);
				CrateData crateDataTO = response.CrateDataTO;
				List<string> resolvedSupplyIdList = GameUtils.GetResolvedSupplyIdList(crateDataTO);
				Service.Get<InventoryCrateRewardController>().GrantInventoryCrateReward(resolvedSupplyIdList, response.CrateDataTO);
			}
		}

		public static void OpenCrate(CrateData crateData)
		{
			if (crateData != null)
			{
				crateData.Claimed = true;
				OpenCrateRequest request = new OpenCrateRequest(crateData.UId);
				OpenCrateCommand openCrateCommand = new OpenCrateCommand(request);
				openCrateCommand.AddSuccessCallback(new AbstractCommand<OpenCrateRequest, OpenCrateResponse>.OnSuccessCallback(GameUtils.CrateOpenSuccessCallback));
				openCrateCommand.AddFailureCallback(new AbstractCommand<OpenCrateRequest, OpenCrateResponse>.OnFailureCallback(GameUtils.CrateOpenFailureCallback));
				openCrateCommand.Context = crateData;
				ProcessingScreen.Show();
				Service.Get<ServerAPI>().Sync(openCrateCommand);
				Service.Get<EventManager>().SendEvent(EventId.InventoryCrateOpened, crateData.CrateId + "|" + crateData.UId);
			}
		}

		public static void CrateOpenSuccessCallback(OpenCrateResponse response, object cookie)
		{
			if (response.SupplyIDs != null)
			{
				List<string> supplyIDs = response.SupplyIDs;
				Service.Get<InventoryCrateRewardController>().GrantInventoryCrateReward(supplyIDs, (CrateData)cookie);
			}
		}

		public static void CrateOpenFailureCallback(uint status, object cookie)
		{
			ProcessingScreen.Hide();
			Service.Get<StaRTSLogger>().Error("Failed to inventory open crate");
		}

		public static List<string> GetResolvedSupplyIdList(CrateData crateData)
		{
			List<string> list = new List<string>();
			List<SupplyData> resolvedSupplies = crateData.ResolvedSupplies;
			int count = resolvedSupplies.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					list.Add(resolvedSupplies[i].SupplyId);
				}
			}
			return list;
		}

		public static void ShowCrateAwardModal(string awardedCrateUid)
		{
			IDataController dataController = Service.Get<IDataController>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Dictionary<string, CrateData> available = currentPlayer.Prizes.Crates.Available;
			if (string.IsNullOrEmpty(awardedCrateUid) || !available.ContainsKey(awardedCrateUid))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Cannot show crate reward modal, crate Id: {0} doesn't exist in inventory", new object[]
				{
					awardedCrateUid
				});
				return;
			}
			CrateData crateData = available[awardedCrateUid];
			CrateVO optional = dataController.GetOptional<CrateVO>(crateData.CrateId);
			if (optional == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Cannot show crate reward modal, static data not found for crate Id: {0}", new object[]
				{
					awardedCrateUid
				});
				return;
			}
			CrateRewardModalScreen crateRewardModalScreen = new CrateRewardModalScreen(optional);
			crateRewardModalScreen.IsAlwaysOnTop = true;
			Service.Get<ScreenController>().AddScreen(crateRewardModalScreen, true, true);
		}

		public static bool BuyProtectionPackWithCrystals(int packNumber)
		{
			int[] array;
			int[] array2;
			GameUtils.GetProtectionPacks(out array, out array2);
			if (packNumber - 1 >= array.Length || packNumber - 1 >= array2.Length)
			{
				return false;
			}
			if (!GameUtils.SpendCrystals(array2[packNumber - 1]))
			{
				return false;
			}
			BuyResourceRequest request = BuyResourceRequest.MakeBuyProtectionRequest(packNumber);
			Service.Get<ServerAPI>().Enqueue(new BuyResourceCommand(request));
			uint time = ServerTime.Time;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.ProtectedUntil == 0u)
			{
				currentPlayer.ProtectedUntil = time;
			}
			uint num = (uint)array[packNumber - 1];
			currentPlayer.ProtectedUntil += num * 60u;
			currentPlayer.AddProtectionCooldownUntil(packNumber, time + num * 60u * 6u);
			int currencyAmount = -array2[packNumber - 1];
			string itemType = "protection";
			string itemId = array[packNumber - 1].ToString();
			int itemCount = 1;
			string type = "damage_protection";
			string subType = "consumable";
			Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, itemId, itemCount, type, subType);
			return true;
		}

		public static bool HandleSoftCurrencyFlow(object result, object cookie)
		{
			bool flag = result != null;
			if (flag)
			{
				CurrencyTag currencyTag = (CurrencyTag)cookie;
				CurrencyType currency = currencyTag.Currency;
				int amount = currencyTag.Amount;
				int crystals = currencyTag.Crystals;
				string purchaseContext = currencyTag.PurchaseContext;
				if (!GameUtils.BuySoftCurrencyWithCrystals(currency, amount, crystals, purchaseContext, true))
				{
					flag = false;
				}
			}
			return flag;
		}

		public static bool BuySoftCurrencyWithCrystals(CurrencyType currency, int amount, int crystals, string purchaseContext, bool softCurrencyFlow)
		{
			if (!GameUtils.SpendCrystals(crystals))
			{
				return false;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			switch (currency)
			{
			case CurrencyType.Credits:
				currentPlayer.Inventory.ModifyCredits(amount);
				break;
			case CurrencyType.Materials:
				currentPlayer.Inventory.ModifyMaterials(amount);
				break;
			case CurrencyType.Contraband:
				currentPlayer.Inventory.ModifyContraband(amount);
				break;
			}
			BuyResourceRequest buyResourceRequest = BuyResourceRequest.MakeBuyResourceRequest(currency, amount);
			if (!string.IsNullOrEmpty(purchaseContext))
			{
				buyResourceRequest.setPurchaseContext(purchaseContext);
			}
			Service.Get<ServerAPI>().Enqueue(new BuyResourceCommand(buyResourceRequest));
			int currencyAmount = -crystals;
			string itemType = "";
			string itemId = purchaseContext;
			if (softCurrencyFlow)
			{
				itemType = "soft_currency_flow";
				switch (currency)
				{
				case CurrencyType.Credits:
					itemId = "credits";
					break;
				case CurrencyType.Materials:
					itemId = "materials";
					break;
				case CurrencyType.Contraband:
					itemId = "contraband";
					break;
				}
			}
			string type = "currency_purchase";
			string subType = "durable";
			Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, itemId, amount, type, subType);
			return true;
		}

		public static bool BuySoftCurrenciesWithCrystals(int credits, int materials, int contraband, int crystals, string purchaseContext, string itemId)
		{
			if (!GameUtils.SpendCrystals(crystals))
			{
				return false;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (credits > 0)
			{
				currentPlayer.Inventory.ModifyCredits(credits);
			}
			if (materials > 0)
			{
				currentPlayer.Inventory.ModifyMaterials(materials);
			}
			if (contraband > 0)
			{
				currentPlayer.Inventory.ModifyContraband(contraband);
			}
			BuyMultiResourceRequest request = new BuyMultiResourceRequest(credits, materials, contraband, purchaseContext);
			Service.Get<ServerAPI>().Enqueue(new BuyMultiResourceCommand(request));
			int currencyAmount = -crystals;
			string itemType = "";
			int itemCount = 1;
			string type = "currency_purchase";
			string subType = "durable";
			Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, itemId, itemCount, type, subType);
			return true;
		}

		public static bool BuyNextDroid(bool allDroidsWereBusy)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.CurrentDroidsAmount >= currentPlayer.MaxDroidsAmount)
			{
				return false;
			}
			int num = GameUtils.DroidCrystalCost(currentPlayer.CurrentDroidsAmount);
			if (!GameUtils.SpendCrystals(num))
			{
				return false;
			}
			currentPlayer.Inventory.ModifyDroids(1);
			BuyResourceRequest buyResourceRequest = BuyResourceRequest.MakeBuyDroidRequest(1);
			buyResourceRequest.setPurchaseContext(allDroidsWereBusy ? "allDroidsBusy" : "droidHutUpgrade");
			BuyResourceCommand command = new BuyResourceCommand(buyResourceRequest);
			Service.Get<ServerAPI>().Enqueue(command);
			int currencyAmount = -num;
			string itemType = "droid_hut";
			string analyticsDroidHutType = GameUtils.GetAnalyticsDroidHutType();
			int itemCount = 1;
			string type = currentPlayer.CampaignProgress.FueInProgress ? "FUE_droid_upgrade" : "droid_upgrade";
			string subType = "durable";
			Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, analyticsDroidHutType, itemCount, type, subType);
			Service.Get<EventManager>().SendEvent(EventId.DroidPurchaseCompleted, null);
			return true;
		}

		private static string GetAnalyticsDroidHutType()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return currentPlayer.Faction.ToString().ToLower() + "DroidHut";
		}

		public static string GetBuildingPurchaseContext(BuildingTypeVO nextBuildingVO, BuildingTypeVO currentBuildingVO, bool isUpgrade, bool isSwap)
		{
			return GameUtils.GetBuildingPurchaseContext(nextBuildingVO, currentBuildingVO, isUpgrade, isSwap, null);
		}

		public static string GetBuildingPurchaseContext(BuildingTypeVO nextBuildingVO, BuildingTypeVO currentBuildingVO, bool isUpgrade, bool isSwap, PlanetVO selectedPlanet)
		{
			string text = StringUtils.ToLowerCaseUnderscoreSeperated(nextBuildingVO.Type.ToString());
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(text);
			if (selectedPlanet != null)
			{
				stringBuilder.Append("|");
				stringBuilder.Append(selectedPlanet.PlanetBIName);
			}
			if (isUpgrade)
			{
				stringBuilder.Append("|");
				stringBuilder.Append("upgrade");
			}
			if (isSwap)
			{
				stringBuilder.Append("|");
				stringBuilder.Append("cross");
			}
			stringBuilder.Append("|");
			stringBuilder.Append(nextBuildingVO.BuildingID);
			stringBuilder.Append("|");
			stringBuilder.Append(nextBuildingVO.Lvl);
			if (isSwap && currentBuildingVO != null)
			{
				stringBuilder.Append("|");
				stringBuilder.Append(currentBuildingVO.BuildingID);
			}
			return stringBuilder.ToString();
		}

		public static void OpenURL(string url)
		{
			if (Service.Get<EnvironmentController>().IsRestrictedProfile())
			{
				string message = Service.Get<Lang>().Get("RESTRICTED_WEB_ACCESS_WARNING", new object[0]);
				AlertScreen.ShowModal(false, null, message, null, null);
				return;
			}
			string message2 = Service.Get<Lang>().Get("EXIT_WARNING", new object[0]);
			AlertScreen.ShowModal(false, null, message2, new OnScreenModalResult(GameUtils.OnOpenURLModalResult), url);
		}

		private static void OnOpenURLModalResult(object result, object cookie)
		{
			if (result != null)
			{
				Application.OpenURL((string)cookie);
			}
		}

		public static void ToggleGameObjectViewVisibility(GameObjectViewComponent viewComp, bool visible)
		{
			if (viewComp != null && viewComp.MainTransform != null)
			{
				Transform mainTransform = viewComp.MainTransform;
				using (IEnumerator enumerator = mainTransform.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Transform transform = (Transform)enumerator.get_Current();
						transform.gameObject.SetActive(visible);
					}
				}
			}
		}

		public static string GetDeviceInfo()
		{
			return string.Format("Device: {0}-{1}, OS: {2}, Processor: {3}x{4}, Memory: {5}, Graphics: {6}-{7}-{8}", new object[]
			{
				SystemInfo.deviceModel,
				SystemInfo.deviceType,
				SystemInfo.operatingSystem,
				SystemInfo.processorType,
				SystemInfo.processorCount,
				SystemInfo.systemMemorySize,
				SystemInfo.graphicsDeviceVendor,
				SystemInfo.graphicsDeviceName,
				SystemInfo.graphicsDeviceVersion
			}).Trim();
		}

		public static long GetJavaEpochTime(DateTime time)
		{
			return Convert.ToInt64((time - new DateTime(1970, 1, 1, 0, 0, 0, 1)).get_TotalMilliseconds(), CultureInfo.InvariantCulture);
		}

		public static long GetNowJavaEpochTime()
		{
			return Convert.ToInt64((DateTime.get_UtcNow() - new DateTime(1970, 1, 1, 0, 0, 0, 1)).get_TotalMilliseconds(), CultureInfo.InvariantCulture);
		}

		public static int CalculateDamagePercentage(HealthComponent healthComp)
		{
			return GameUtils.CalculateDamagePercentage(healthComp.Health, healthComp.MaxHealth);
		}

		public static int CalculateDamagePercentage(int currentHealth, int maxHealth)
		{
			float num = (float)currentHealth / (float)maxHealth;
			return (int)((1f - num) * 100f);
		}

		public static long CalculateResourceChecksum(int credits, int materials, int contraband, int crystals)
		{
			return 31L * (long)credits ^ 31L * (long)materials << 10 ^ 31L * (long)contraband << 20 ^ 31L * (long)crystals << 30;
		}

		public static string CreateInfoStringForChecksum(Contract additionalContract, bool instantContract, bool simulateTroopContractUpdate, ref int crystals)
		{
			ISupportController supportController = Service.Get<ISupportController>();
			List<Contract> list = null;
			List<Contract> list2 = null;
			supportController.GetEstimatedUpdatedContractListsForChecksum(simulateTroopContractUpdate, out list, out list2);
			int num = (list2 == null) ? 0 : list2.Count;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Map map = currentPlayer.Map;
			IDataController dataController = Service.Get<IDataController>();
			StringBuilder stringBuilder = new StringBuilder();
			List<Building> list3 = new List<Building>(map.Buildings);
			BaseLayoutToolController baseLayoutToolController = Service.Get<BaseLayoutToolController>();
			bool flag = baseLayoutToolController != null && baseLayoutToolController.ShouldChecksumLastSaveData();
			if (flag)
			{
				list3.Sort(new Comparison<Building>(GameUtils.CompareLastSavedBuildingLocation));
			}
			else
			{
				list3.Sort(new Comparison<Building>(GameUtils.CompareBuildingsByPosition));
			}
			stringBuilder.Append("--Buildings--\n");
			int i = 0;
			int count = list3.Count;
			while (i < count)
			{
				Building building = list3[i];
				string uidOverride = null;
				uint timeOverride = 0u;
				bool flag2 = false;
				int j = 0;
				while (j < num)
				{
					Contract contract = list2[j];
					if (contract.ContractTO.BuildingKey == building.Key && ContractUtils.IsBuildingType(contract.ContractTO.ContractType))
					{
						if (contract.DeliveryType == DeliveryType.UpgradeBuilding || contract.DeliveryType == DeliveryType.SwapBuilding)
						{
							uidOverride = contract.ProductUid;
						}
						if (contract.DeliveryType == DeliveryType.Building || contract.DeliveryType == DeliveryType.UpgradeBuilding || contract.DeliveryType == DeliveryType.SwapBuilding)
						{
							BuildingTypeVO buildingTypeVO = dataController.Get<BuildingTypeVO>(contract.ProductUid);
							if (buildingTypeVO.Type == BuildingType.Resource)
							{
								timeOverride = contract.ContractTO.EndTime;
							}
						}
						if (contract.DeliveryType == DeliveryType.ClearClearable)
						{
							flag2 = true;
							crystals += building.CurrentStorage;
							break;
						}
						break;
					}
					else
					{
						j++;
					}
				}
				if (!flag2)
				{
					if (instantContract && additionalContract != null && additionalContract.DeliveryType == DeliveryType.UpgradeBuilding && additionalContract.ContractTO.BuildingKey == building.Key)
					{
						uidOverride = additionalContract.ProductUid;
						timeOverride = 0u;
						additionalContract = null;
					}
					if (flag)
					{
						int buildingLastSavedX = baseLayoutToolController.GetBuildingLastSavedX(building.Key);
						int buildingLastSavedZ = baseLayoutToolController.GetBuildingLastSavedZ(building.Key);
						building.AddString(stringBuilder, uidOverride, timeOverride, buildingLastSavedX, buildingLastSavedZ);
					}
					else
					{
						building.AddString(stringBuilder, uidOverride, timeOverride);
					}
				}
				i++;
			}
			stringBuilder.Append("--Contracts--\n");
			if (list != null)
			{
				if (additionalContract != null)
				{
					list.Add(additionalContract);
				}
				list.Sort(new Comparison<Contract>(supportController.SortByEndTime));
				int k = 0;
				int count2 = list.Count;
				while (k < count2)
				{
					list[k].AddString(stringBuilder);
					k++;
				}
				List<ContractTO> uninitializedContractData = supportController.GetUninitializedContractData();
				if (uninitializedContractData != null)
				{
					uninitializedContractData.Sort(new Comparison<ContractTO>(supportController.SortContractTOByEndTime));
					int l = 0;
					int count3 = uninitializedContractData.Count;
					while (l < count3)
					{
						uninitializedContractData[l].AddString(stringBuilder);
						l++;
					}
				}
			}
			return stringBuilder.ToString();
		}

		public static long StringHash(string str)
		{
			int num = 7;
			for (int i = 0; i < str.get_Length(); i++)
			{
				num = num * 31 + (int)str.get_Chars(i);
			}
			return (long)num;
		}

		public static int CompareBuildingsByPosition(Building a, Building b)
		{
			int num = a.X - b.X;
			if (num == 0)
			{
				return a.Z - b.Z;
			}
			return num;
		}

		public static int CompareLastSavedBuildingLocation(Building a, Building b)
		{
			BaseLayoutToolController baseLayoutToolController = Service.Get<BaseLayoutToolController>();
			int buildingLastSavedX = baseLayoutToolController.GetBuildingLastSavedX(a.Key);
			int buildingLastSavedX2 = baseLayoutToolController.GetBuildingLastSavedX(b.Key);
			int num = buildingLastSavedX - buildingLastSavedX2;
			if (num == 0)
			{
				int buildingLastSavedZ = baseLayoutToolController.GetBuildingLastSavedZ(a.Key);
				int buildingLastSavedZ2 = baseLayoutToolController.GetBuildingLastSavedZ(b.Key);
				return buildingLastSavedZ - buildingLastSavedZ2;
			}
			return num;
		}

		public static int GetTimeDifferenceSafe(uint timeA, uint timeB)
		{
			uint num;
			if (timeA > timeB)
			{
				num = timeA - timeB;
			}
			else
			{
				num = timeB - timeA;
			}
			if (num > 2147483647u)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Attempted to get time difference but delta time {1} is too large.", new object[]
				{
					num
				});
				return 0;
			}
			return (int)(timeA - timeB);
		}

		public static uint GetModifiedTimeSafe(uint time, int modifier)
		{
			uint result;
			if (modifier >= 0)
			{
				result = time + (uint)modifier;
			}
			else
			{
				modifier = -modifier;
				if (time < (uint)modifier)
				{
					result = 0u;
				}
				else
				{
					result = time - (uint)modifier;
				}
			}
			return result;
		}

		public static FactionType GetOppositeFaction(FactionType faction)
		{
			if (faction == FactionType.Empire)
			{
				return FactionType.Rebel;
			}
			if (faction != FactionType.Rebel)
			{
				return FactionType.Invalid;
			}
			return FactionType.Empire;
		}

		public static bool IsPlanetCurrentOne(string uid)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			PlanetVO planet = currentPlayer.Map.Planet;
			return planet.Uid.Equals(uid);
		}

		public static bool IsMissionSpecOps(string missionId)
		{
			if (!string.IsNullOrEmpty(missionId))
			{
				IDataController dataController = Service.Get<IDataController>();
				CampaignMissionVO campaignMissionVO = dataController.Get<CampaignMissionVO>(missionId);
				return campaignMissionVO.BIContext == "campaign";
			}
			return false;
		}

		public static bool IsMissionRaidDefense(string missionId)
		{
			if (!string.IsNullOrEmpty(missionId))
			{
				IDataController dataController = Service.Get<IDataController>();
				CampaignMissionVO campaignMissionVO = dataController.Get<CampaignMissionVO>(missionId);
				return campaignMissionVO.MissionType == MissionType.RaidDefend;
			}
			return false;
		}

		public static bool IsMissionDefense(string missionId)
		{
			if (!string.IsNullOrEmpty(missionId))
			{
				IDataController dataController = Service.Get<IDataController>();
				CampaignMissionVO campaignMissionVO = dataController.Get<CampaignMissionVO>(missionId);
				return campaignMissionVO.MissionType == MissionType.Defend || campaignMissionVO.MissionType == MissionType.RaidDefend;
			}
			return false;
		}

		public static CampaignVO GetHighestUnlockedCampaign()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			IDataController dataController = Service.Get<IDataController>();
			int num = -1;
			CampaignVO campaignVO = null;
			foreach (CampaignVO current in dataController.GetAll<CampaignVO>())
			{
				if (!current.Timed && current.Faction == currentPlayer.Faction && currentPlayer.CampaignProgress.HasCampaign(current) && current.UnlockOrder > num)
				{
					campaignVO = current;
					num = campaignVO.UnlockOrder;
				}
			}
			return campaignVO;
		}

		public static bool HasAvailableTroops(bool isPvE, BattleTypeVO battle)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Inventory inventory = currentPlayer.Inventory;
			if (inventory.Troop.GetTotalStorageAmount() > 0 || inventory.Champion.GetTotalStorageAmount() > 0 || inventory.SpecialAttack.GetTotalStorageAmount() > 0)
			{
				return true;
			}
			if (isPvE)
			{
				return battle != null && battle.OverridePlayerUnits;
			}
			return inventory.Hero.GetTotalStorageAmount() > 0 || SquadUtils.GetDonatedTroopStorageUsedByCurrentPlayer() > 0;
		}

		public static void ExitEditState()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is EditBaseState)
			{
				HomeState.GoToHomeState(null, false);
			}
		}

		public static bool IsDeviceCountryInList(string countryList)
		{
			if (string.IsNullOrEmpty(countryList))
			{
				return false;
			}
			countryList = countryList.Replace(" ", "");
			countryList = countryList.ToLower();
			string[] array = countryList.Split(new char[]
			{
				'|'
			});
			string text = Service.Get<EnvironmentController>().GetDeviceCountryCode();
			text = text.ToLower();
			for (int i = 0; i < array.Length; i++)
			{
				if (text.Equals(array[i]))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsBattleVersionSupported(string cmsVersion, string battleVersion)
		{
			return cmsVersion == Service.Get<FMS>().GetFileVersion("patches/base.json").ToString() && battleVersion == "21.0";
		}

		public static bool IsVideoShareSupported(string videoId)
		{
			return !string.IsNullOrEmpty(videoId) && GameConstants.IsMakerVideoEnabled();
		}

		public static List<BoardCell<Entity>> TraverseSpiral(int radius, int centerX, int centerZ)
		{
			BoardController boardController = Service.Get<BoardController>();
			List<BoardCell<Entity>> list = new List<BoardCell<Entity>>();
			int num3;
			int num2;
			int num = num2 = (num3 = 0);
			int num4 = -1;
			int num5 = radius * 2 + 1;
			int num6 = num5 * num5;
			for (int i = 0; i < num6; i++)
			{
				if (-radius <= num2 && num2 <= radius && -radius <= num && num <= radius)
				{
					BoardCell<Entity> clampedToBoardCellAt = boardController.Board.GetClampedToBoardCellAt(centerX + num2, centerZ + num, 1);
					if (clampedToBoardCellAt != null)
					{
						list.Add(clampedToBoardCellAt);
					}
				}
				if (num2 == num || (num2 < 0 && num2 == -num) || (num2 > 0 && num2 == 1 - num))
				{
					num5 = num3;
					num3 = -num4;
					num4 = num5;
				}
				num2 += num3;
				num += num4;
			}
			return list;
		}

		public static uint GetCurrentSimFrame()
		{
			return Service.Get<SimTimeEngine>().GetFrameCount();
		}

		public static void UpdateMinimumFrameCountForNextTargeting(ShooterComponent shooterComponent)
		{
			shooterComponent.MinimumFrameCountForNextTargeting = GameUtils.GetCurrentSimFrame() + 30u;
		}

		public static bool IsEligibleToFindTarget(ShooterComponent shooterComponent)
		{
			return shooterComponent != null && shooterComponent.MinimumFrameCountForNextTargeting <= GameUtils.GetCurrentSimFrame();
		}

		public static bool IsEntityDead(SmartEntity entity)
		{
			return entity.HealthComp == null || entity.HealthComp.IsDead();
		}

		public static Quaternion FindRelativeRotation(Quaternion a, Quaternion b)
		{
			return Quaternion.Inverse(b) * a;
		}

		public static Quaternion ApplyRelativeRotation(Quaternion a, Quaternion b)
		{
			return b * a;
		}

		public static bool IsEntityShieldGenerator(SmartEntity entity)
		{
			return entity.ShieldGeneratorComp != null;
		}

		public static void TryAndOpenAppropriateStorePage()
		{
		}

		public static string GetTournamentPointIconName(string planetId)
		{
			if (string.IsNullOrEmpty(planetId))
			{
				return null;
			}
			PlanetVO optional = Service.Get<IDataController>().GetOptional<PlanetVO>(planetId);
			if (optional != null)
			{
				return optional.MedalIconName;
			}
			return null;
		}

		public static bool ConflictStartsInBadgePeriod(TournamentVO tournamentVO)
		{
			return tournamentVO.StartTimestamp - (int)ServerTime.Time < GameConstants.TOURNAMENT_HOURS_SHOW_BADGE * 3600;
		}

		public static string GetCurrencyIconName(string currencyName)
		{
			string result = null;
			if (currencyName == "contraband")
			{
				result = "icoContraband";
			}
			else if (currencyName == "crystals")
			{
				result = "icoCrystals";
			}
			else if (currencyName == "credits")
			{
				result = "icoCollectCredit";
			}
			else if (currencyName == "materials")
			{
				result = "icoMaterials";
			}
			else if (currencyName == "reputation")
			{
				result = "icoReputation";
			}
			return result;
		}

		public static string GetClearableAssetName(BuildingTypeVO buildingVO, PlanetVO planetVO)
		{
			string text = buildingVO.AssetName.Substring(0, buildingVO.AssetName.LastIndexOf("-") + 1);
			return text + planetVO.Abbreviation;
		}

		public static bool IsPvpTargetSearchFailureRequiresReload(uint commandStatus)
		{
			bool result = true;
			switch (commandStatus)
			{
			case 2100u:
			case 2101u:
			case 2102u:
			case 2103u:
			case 2104u:
			case 2105u:
			case 2107u:
				result = false;
				break;
			}
			return result;
		}

		public static void IndicateNewInventoryItems(RewardVO vo)
		{
			if (vo.CurrencyRewards != null)
			{
				Service.Get<ServerPlayerPrefs>().SetPref(ServerPref.NumInventoryCurrencyNotViewed, "1");
			}
			if (vo.TroopRewards != null || vo.SpecialAttackRewards != null || vo.HeroRewards != null)
			{
				Service.Get<ServerPlayerPrefs>().SetPref(ServerPref.NumInventoryTroopsNotViewed, "1");
			}
			Service.Get<ServerAPI>().Sync(new SetPrefsCommand(false));
			if (!GameUtils.IsAppLoading())
			{
				Service.Get<EventManager>().SendEvent(EventId.NumInventoryItemsNotViewedUpdated, null);
			}
		}

		public static void UpdateInventoryCrateBadgeCount(int delta)
		{
			if (delta != 0)
			{
				ServerPlayerPrefs serverPlayerPrefs = Service.Get<ServerPlayerPrefs>();
				int val = Convert.ToInt32(serverPlayerPrefs.GetPref(ServerPref.NumInventoryCratesNotViewed), CultureInfo.InvariantCulture) + delta;
				serverPlayerPrefs.SetPref(ServerPref.NumInventoryCratesNotViewed, Math.Max(0, val).ToString());
				Service.Get<ServerAPI>().Enqueue(new SetPrefsCommand(false));
			}
			if (!GameUtils.IsAppLoading())
			{
				Service.Get<EventManager>().SendEvent(EventId.NumInventoryItemsNotViewedUpdated, null);
			}
		}

		public static void AddRewardToInventory(RewardVO vo)
		{
			PrizeInventory prizes = Service.Get<CurrentPlayer>().Prizes;
			IDataController dataController = Service.Get<IDataController>();
			if (vo.CurrencyRewards != null)
			{
				int i = 0;
				int num = vo.CurrencyRewards.Length;
				while (i < num)
				{
					string[] array = vo.CurrencyRewards[i].Split(new char[]
					{
						':'
					});
					prizes.ModifyResourceAmount(array[0], Convert.ToInt32(array[1], CultureInfo.InvariantCulture));
					i++;
				}
			}
			if (vo.TroopRewards != null)
			{
				int j = 0;
				int num2 = vo.TroopRewards.Length;
				while (j < num2)
				{
					string[] array = vo.TroopRewards[j].Split(new char[]
					{
						':'
					});
					IUpgradeableVO upgradeableVO = dataController.Get<TroopTypeVO>(array[0]);
					prizes.ModifyTroopAmount(upgradeableVO.UpgradeGroup, Convert.ToInt32(array[1], CultureInfo.InvariantCulture));
					j++;
				}
			}
			if (vo.SpecialAttackRewards != null)
			{
				int k = 0;
				int num3 = vo.SpecialAttackRewards.Length;
				while (k < num3)
				{
					string[] array = vo.SpecialAttackRewards[k].Split(new char[]
					{
						':'
					});
					IUpgradeableVO upgradeableVO = dataController.Get<SpecialAttackTypeVO>(array[0]);
					prizes.ModifySpecialAttackAmount(upgradeableVO.UpgradeGroup, Convert.ToInt32(array[1], CultureInfo.InvariantCulture));
					k++;
				}
			}
			if (vo.HeroRewards != null)
			{
				int l = 0;
				int num4 = vo.HeroRewards.Length;
				while (l < num4)
				{
					string[] array = vo.HeroRewards[l].Split(new char[]
					{
						':'
					});
					IUpgradeableVO upgradeableVO = dataController.Get<TroopTypeVO>(array[0]);
					prizes.ModifyTroopAmount(upgradeableVO.UpgradeGroup, Convert.ToInt32(array[1], CultureInfo.InvariantCulture));
					l++;
				}
			}
			GameUtils.IndicateNewInventoryItems(vo);
		}

		public static int GetNumInventoryItemsNotViewed()
		{
			ServerPlayerPrefs serverPlayerPrefs = Service.Get<ServerPlayerPrefs>();
			int num = 0;
			num += Convert.ToInt32(serverPlayerPrefs.GetPref(ServerPref.NumInventoryItemsNotViewed), CultureInfo.InvariantCulture);
			num += Convert.ToInt32(serverPlayerPrefs.GetPref(ServerPref.NumInventoryCratesNotViewed), CultureInfo.InvariantCulture);
			num += Convert.ToInt32(serverPlayerPrefs.GetPref(ServerPref.NumInventoryTroopsNotViewed), CultureInfo.InvariantCulture);
			return num + Convert.ToInt32(serverPlayerPrefs.GetPref(ServerPref.NumInventoryCurrencyNotViewed), CultureInfo.InvariantCulture);
		}

		public static Transform FindAssetMetaDataTransform(GameObject gameObj, string searchName)
		{
			Transform result = null;
			if (gameObj != null && !string.IsNullOrEmpty(searchName))
			{
				AssetMeshDataMonoBehaviour component = gameObj.GetComponent<AssetMeshDataMonoBehaviour>();
				if (component != null)
				{
					int count = component.OtherGameObjects.Count;
					for (int i = 0; i < count; i++)
					{
						GameObject gameObject = component.OtherGameObjects[i];
						if (gameObject.name.Contains(searchName))
						{
							result = gameObject.transform;
							break;
						}
					}
				}
			}
			return result;
		}

		public static bool IsAppLoading()
		{
			return Service.Get<GameStateMachine>().CurrentState is ApplicationLoadState;
		}

		public static string GetSingleCurrencyItemAssetName(string currencyType)
		{
			CurrencyType currencyType2 = StringUtils.ParseEnum<CurrencyType>(currencyType);
			return GameUtils.GetSingleCurrencyItemAssetName(currencyType2);
		}

		public static string GetSingleCurrencyItemAssetName(CurrencyType currencyType)
		{
			switch (currencyType)
			{
			case CurrencyType.Credits:
				return "collectcredit-ani";
			case CurrencyType.Materials:
				return "collectmaterial-ani";
			case CurrencyType.Contraband:
				return "collectcontraband-ani";
			default:
				return "collectcredit-ani";
			}
		}

		public static string GetSingleCurrencyIconAssetName(string currencyType)
		{
			CurrencyType currencyType2 = StringUtils.ParseEnum<CurrencyType>(currencyType);
			return GameUtils.GetSingleCurrencyIconAssetName(currencyType2);
		}

		public static string GetSingleCurrencyIconAssetName(CurrencyType currencyType)
		{
			switch (currencyType)
			{
			case CurrencyType.Credits:
				return "currencyicon_neu-mod_credit";
			case CurrencyType.Materials:
				return "currencyicon_neu-mod_alloy";
			case CurrencyType.Contraband:
				return "currencyicon_neu-mod_contraband";
			case CurrencyType.Crystals:
				return "currencyicon_neu-mod_crystal";
			}
			return "currencyicon_neu-mod_credit";
		}

		public static string GetInventorySupplyIconAssetName(CrateSupplyVO crateSupply, out IGeometryVO config, out string supplyName, int hqLevel)
		{
			config = null;
			supplyName = null;
			if (crateSupply == null)
			{
				return null;
			}
			string rewardUid = crateSupply.RewardUid;
			if (rewardUid == null || rewardUid.get_Length() <= 0)
			{
				return null;
			}
			InventoryCrateRewardController inventoryCrateRewardController = Service.Get<InventoryCrateRewardController>();
			RewardVO vo = inventoryCrateRewardController.GenerateRewardFromSupply(crateSupply, hqLevel);
			supplyName = Service.Get<RewardManager>().GetRewardString(vo, Service.Get<Lang>().Get("SupplyRewardFormat", new object[0]));
			SupplyType type = crateSupply.Type;
			if (type == SupplyType.Currency)
			{
				config = Service.Get<IDataController>().Get<CurrencyIconVO>(rewardUid);
				return GameUtils.GetSingleCurrencyIconAssetName(rewardUid);
			}
			if (type == SupplyType.Shard)
			{
				config = ArmoryUtils.GetCurrentEquipmentDataByID(crateSupply.RewardUid);
				return config.IconAssetName;
			}
			if (type == SupplyType.Troop || type == SupplyType.Hero)
			{
				TroopTypeVO troopTypeVO = Service.Get<IDataController>().Get<TroopTypeVO>(rewardUid);
				config = troopTypeVO;
				return troopTypeVO.AssetName;
			}
			if (type == SupplyType.SpecialAttack)
			{
				SpecialAttackTypeVO specialAttackTypeVO = Service.Get<IDataController>().Get<SpecialAttackTypeVO>(rewardUid);
				config = specialAttackTypeVO;
				return specialAttackTypeVO.AssetName;
			}
			config = null;
			supplyName = null;
			return null;
		}

		public static IGeometryVO GetIconVOFromObjective(ObjectiveVO obj, int objectiveHq)
		{
			IGeometryVO geometryVO = null;
			IDataController dataController = Service.Get<IDataController>();
			switch (obj.ObjectiveType)
			{
			case ObjectiveType.Loot:
				geometryVO = UXUtils.GetDefaultCurrencyIconVO(obj.ObjIcon);
				break;
			case ObjectiveType.DestroyBuildingType:
			case ObjectiveType.DestroyBuildingID:
			case ObjectiveType.ReceiveDonatedTroops:
			case ObjectiveType.DonateTroopType:
			case ObjectiveType.DonateTroopID:
			case ObjectiveType.DonateTroop:
				geometryVO = dataController.GetOptional<BuildingTypeVO>(obj.ObjIcon + objectiveHq);
				break;
			case ObjectiveType.DeployTroopType:
			case ObjectiveType.DeployTroopID:
			case ObjectiveType.TrainTroopType:
			case ObjectiveType.TrainTroopID:
				geometryVO = dataController.GetOptional<TroopTypeVO>(obj.ObjIcon + objectiveHq);
				break;
			case ObjectiveType.DeploySpecialAttack:
			case ObjectiveType.DeploySpecialAttackID:
			case ObjectiveType.TrainSpecialAttack:
			case ObjectiveType.TrainSpecialAttackID:
				geometryVO = dataController.GetOptional<SpecialAttackTypeVO>(obj.ObjIcon + objectiveHq);
				break;
			}
			if (geometryVO == null && objectiveHq != 1)
			{
				geometryVO = GameUtils.GetIconVOFromObjective(obj, 1);
			}
			if (geometryVO == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Could not find icon for objective {0}, type {1}, objIcon {2}, lvl {3}", new object[]
				{
					obj.Uid,
					obj.ObjectiveType,
					obj.ObjIcon,
					objectiveHq
				});
			}
			return geometryVO;
		}

		public static int GetShardQualityNumeric(CrateSupplyVO supplyVO)
		{
			IDataController dataController = Service.Get<IDataController>();
			SupplyType type = supplyVO.Type;
			if (type == SupplyType.Shard)
			{
				EquipmentVO currentEquipmentDataByID = ArmoryUtils.GetCurrentEquipmentDataByID(supplyVO.RewardUid);
				return (int)currentEquipmentDataByID.Quality;
			}
			if (type == SupplyType.ShardTroop || type == SupplyType.ShardSpecialAttack)
			{
				ShardVO shardVO = dataController.Get<ShardVO>(supplyVO.RewardUid);
				return (int)shardVO.Quality;
			}
			return -1;
		}

		public static string GetRewardSupplyName(CrateSupplyVO supply, int hqLevel)
		{
			InventoryCrateRewardController inventoryCrateRewardController = Service.Get<InventoryCrateRewardController>();
			SupplyType type = supply.Type;
			string result;
			if (type == SupplyType.ShardSpecialAttack || type == SupplyType.ShardTroop)
			{
				IDataController dataController = Service.Get<IDataController>();
				ShardVO optional = dataController.GetOptional<ShardVO>(supply.RewardUid);
				IDeployableVO deployableVOFromShard = Service.Get<DeployableShardUnlockController>().GetDeployableVOFromShard(optional);
				int rewardAmount = Service.Get<InventoryCrateRewardController>().GetRewardAmount(supply, hqLevel);
				result = GameUtils.GetShardUnlockSupplyName(deployableVOFromShard, optional, rewardAmount);
			}
			else
			{
				RewardVO vo = inventoryCrateRewardController.GenerateRewardFromSupply(supply, hqLevel);
				result = Service.Get<RewardManager>().GetRewardStringWithLevel(vo, Service.Get<Lang>().Get("SupplyRewardFormat", new object[0]));
			}
			return result;
		}

		public static IGeometryVO GetIconVOFromCrateSupply(CrateSupplyVO supply, int playerHq)
		{
			IGeometryVO geometryVO = null;
			string text = null;
			GameUtils.GetInventorySupplyIconAssetName(supply, out geometryVO, out text, playerHq);
			if (geometryVO != null)
			{
				return geometryVO;
			}
			return GameUtils.GetIconVOFromShardCrateSupply(supply, out text);
		}

		private static string GetShardUnlockSupplyName(IDeployableVO config, ShardVO shard, int amount)
		{
			Lang lang = Service.Get<Lang>();
			string text = null;
			if (config != null && shard != null)
			{
				if (shard.TargetType == "specialAttack")
				{
					text = LangUtils.GetStarshipDisplayName((SpecialAttackTypeVO)config);
				}
				else
				{
					text = LangUtils.GetTroopDisplayName((TroopTypeVO)config);
				}
			}
			if (amount <= 0)
			{
				return text;
			}
			string text2 = lang.Get("SupplyRewardFormat", new object[0]);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(text2, new object[]
			{
				text,
				lang.ThousandsSeparated(Convert.ToInt32(amount, CultureInfo.InvariantCulture))
			});
			return stringBuilder.ToString();
		}

		private static IGeometryVO GetIconVOFromShardCrateSupply(CrateSupplyVO supply, out string supplyName)
		{
			IDataController dataController = Service.Get<IDataController>();
			ShardVO optional = dataController.GetOptional<ShardVO>(supply.RewardUid);
			IDeployableVO deployableVOFromShard = Service.Get<DeployableShardUnlockController>().GetDeployableVOFromShard(optional);
			supplyName = GameUtils.GetShardUnlockSupplyName(deployableVOFromShard, optional, 0);
			return deployableVOFromShard;
		}

		public static bool HasUserFactionFlipped(CurrentPlayer player)
		{
			return player.NumIdentities > 1;
		}

		public static void SwapShaderIfNeeded(string[] swapList, Shader swapSrc, Material sharedMaterial)
		{
			if (sharedMaterial.shader != null)
			{
				string name = sharedMaterial.shader.name;
				int num = swapList.Length;
				for (int i = 0; i < num; i++)
				{
					if (swapList[i] == name)
					{
						sharedMaterial.shader = swapSrc;
						return;
					}
				}
				return;
			}
			Service.Get<StaRTSLogger>().Warn("Material Shader NULL: " + sharedMaterial.name);
		}

		public static bool SafeVOEqualityValidation(IValueObject lhs, IValueObject rhs)
		{
			return lhs == rhs || (rhs != null && lhs != null && lhs.Uid == rhs.Uid);
		}

		public static void MultiplyCurrency(float multiplier, ref int credits, ref int materials, ref int contraband)
		{
			if (multiplier != 1f)
			{
				credits = Mathf.FloorToInt((float)credits * multiplier);
				materials = Mathf.FloorToInt((float)materials * multiplier);
				contraband = Mathf.FloorToInt((float)contraband * multiplier);
			}
		}

		public static Dictionary<string, int> GetHQScaledCostForPlayer(string[] cost)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int hqLevel = currentPlayer.Map.FindHighestHqLevel();
			return GameUtils.GetHQScaledCost(cost, hqLevel);
		}

		public static Dictionary<string, int> GetHQScaledCost(string[] cost, int hqLevel)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			IDataController sdc = Service.Get<IDataController>();
			int num = cost.Length;
			for (int i = 0; i < num; i++)
			{
				string[] array = cost[i].Split(new char[]
				{
					':'
				});
				if (array.Length == 2)
				{
					string key = array[0];
					string scalingUid = array[1];
					int hQScaledValue = GameUtils.GetHQScaledValue(sdc, scalingUid, hqLevel);
					if (hQScaledValue > 0)
					{
						dictionary[key] = hQScaledValue;
					}
				}
			}
			return dictionary;
		}

		private static int GetHQScaledValue(IDataController sdc, string scalingUid, int hqLevel)
		{
			CrateSupplyScaleVO crateSupplyScaleVO = sdc.Get<CrateSupplyScaleVO>(scalingUid);
			return crateSupplyScaleVO.GetHQScaling(hqLevel);
		}

		public static string GetSquadLevelUIDFromLevel(int level)
		{
			string text = "SquadLevel";
			return text + level.ToString();
		}

		public static string GetSquadLevelUIDFromSquad(Squad squad)
		{
			int level = 1;
			if (squad != null)
			{
				level = squad.Level;
			}
			else
			{
				Service.Get<StaRTSLogger>().Warn("GameUtils.GetSquadLevelUIDFromSquad called with null Squad");
			}
			return GameUtils.GetSquadLevelUIDFromLevel(level);
		}

		public static int GetReputationCapacityForLevel(int squadCenterLevel)
		{
			string sQUADPERK_REPUTATION_MAX_LIMIT = GameConstants.SQUADPERK_REPUTATION_MAX_LIMIT;
			if (!string.IsNullOrEmpty(sQUADPERK_REPUTATION_MAX_LIMIT) && squadCenterLevel > 0)
			{
				string[] array = sQUADPERK_REPUTATION_MAX_LIMIT.Split(new char[]
				{
					' '
				});
				if (array.Length > squadCenterLevel - 1)
				{
					return Convert.ToInt32(array[squadCenterLevel - 1], CultureInfo.InvariantCulture);
				}
			}
			return 0;
		}

		public static int GetSquadLevelFromInvestedRep(int investedRep)
		{
			int num;
			if (Service.IsSet<PerkManager>())
			{
				num = Service.Get<PerkManager>().SquadLevelMax;
			}
			else
			{
				num = Service.Get<IDataController>().GetAll<SquadLevelVO>().Count;
			}
			IDataController dataController = Service.Get<IDataController>();
			int result = 0;
			for (int i = 1; i <= num; i++)
			{
				string squadLevelUIDFromLevel = GameUtils.GetSquadLevelUIDFromLevel(i);
				SquadLevelVO squadLevelVO = dataController.Get<SquadLevelVO>(squadLevelUIDFromLevel);
				if (squadLevelVO.RepThreshold > investedRep)
				{
					break;
				}
				result = squadLevelVO.Level;
			}
			return result;
		}

		public static bool IsPerkCommandStatusFatal(uint status)
		{
			return status != 2504u && status != 2502u;
		}

		public static PerkVO GetPerkByGroupAndTier(string perkGroup, int perkTier)
		{
			IDataController dataController = Service.Get<IDataController>();
			return dataController.Get<PerkVO>("perk_" + perkGroup + perkTier.ToString());
		}

		public static CrateData GetNextInventoryCrateToExpire(InventoryCrates crates, uint minThresholdTime)
		{
			Dictionary<string, CrateData> available = crates.Available;
			CrateData crateData = null;
			foreach (CrateData current in available.Values)
			{
				if (current.DoesExpire)
				{
					uint expiresTimeStamp = current.ExpiresTimeStamp;
					if (expiresTimeStamp != 0u && expiresTimeStamp >= minThresholdTime)
					{
						if (crateData == null)
						{
							crateData = current;
						}
						else if (crateData.ExpiresTimeStamp > expiresTimeStamp)
						{
							crateData = current;
						}
					}
				}
			}
			return crateData;
		}

		public static bool IsUnlockBlockingScreenOpen()
		{
			StoreScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<StoreScreen>();
			if (highestLevelScreen != null)
			{
				return true;
			}
			PrizeInventoryScreen highestLevelScreen2 = Service.Get<ScreenController>().GetHighestLevelScreen<PrizeInventoryScreen>();
			return highestLevelScreen2 != null || Service.Get<InventoryCrateRewardController>().IsCrateAnimationShowingOrPending;
		}

		public static void CloseStoreOrInventoryScreen()
		{
			StoreScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<StoreScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.Close(null);
				return;
			}
			PrizeInventoryScreen highestLevelScreen2 = Service.Get<ScreenController>().GetHighestLevelScreen<PrizeInventoryScreen>();
			if (highestLevelScreen2 != null)
			{
				highestLevelScreen2.Close(null);
			}
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			GameUtils.AddRewardToInventory((RewardVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.ApplyRelativeRotation(*(*(IntPtr*)args), *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.BuyCrate((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (CrateVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.BuyLEI((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.BuyNextDroid(*(sbyte*)args != 0));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.BuyProtectionPackWithCrystals(*(int*)args));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.BuySoftCurrenciesWithCrystals(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5))));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.BuySoftCurrencyWithCrystals((CurrencyType)(*(int*)args), *(int*)(args + 1), *(int*)(args + 2), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), *(sbyte*)(args + 4) != 0));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CalcuateMedals(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CalculateBattleHistoryVictoryRating((LeaderboardBattleHistory)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CalculateDamagePercentage((HealthComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CalculateDamagePercentage(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CalculatePlayerVictoryRating((GamePlayer)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CalculatePvpTargetVictoryRating((PvpTarget)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CalculateResourceChecksum(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CalculateVictoryRating(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CanAffordContraband(*(int*)args));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CanAffordCosts(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CanAffordCredits(*(int*)args));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CanAffordCrystals(*(int*)args));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CanAffordMaterials(*(int*)args));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CanAffordReputation(*(int*)args));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			GameUtils.CloseStoreOrInventoryScreen();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CompareBuildingsByPosition((Building)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CompareLastSavedBuildingLocation((Building)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.ConflictStartsInBadgePeriod((TournamentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.ContrabandCrystalCost(*(int*)args));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			GameUtils.CrateOpenSuccessCallback((OpenCrateResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CreditsCrystalCost(*(int*)args));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CrystalCostToInstantUpgrade((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CrystalCostToUpgradeAllWalls(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.CurrencyPow(*(float*)args, *(int*)(args + 1), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.DroidCrystalCost(*(int*)args));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			GameUtils.ExitEditState();
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.FindAssetMetaDataTransform((GameObject)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.FindRelativeRotation(*(*(IntPtr*)args), *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetAnalyticsDroidHutType());
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetBuildingEffectiveLevel((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetBuildingPurchaseContext((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0));
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetBuildingPurchaseContext((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0, (PlanetVO)GCHandledObjects.GCHandleToObject(args[4])));
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetClearableAssetName((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (PlanetVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetCurrencyIconName(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetCurrencyType(*(int*)args, *(int*)(args + 1), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetDeployableCount(Marshal.PtrToStringUni(*(IntPtr*)args), (InventoryStorage)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetDeployableCountForUpgradeGroupSpecialAttack((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetDeployableCountForUpgradeGroupTroop((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetDeviceInfo());
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetEquivalentFromPreSortedList((List<BuildingTypeVO>)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]), (FactionType)(*(int*)(args + 2))));
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetEquivalentSlow((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (FactionType)(*(int*)(args + 1))));
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetHighestUnlockedCampaign());
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetHQScaledCost((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetHQScaledCostForPlayer((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args)));
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetHQScaledValue((IDataController)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetIconVOFromCrateSupply((CrateSupplyVO)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetIconVOFromObjective((ObjectiveVO)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetJavaEpochTime(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetNowJavaEpochTime());
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetNumInventoryItemsNotViewed());
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetOppositeFaction((FactionType)(*(int*)args)));
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetPerkByGroupAndTier(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetProtectionTimeRemaining());
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetReputationCapacityForLevel(*(int*)args));
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetResolvedSupplyIdList((CrateData)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetRewardSupplyName((CrateSupplyVO)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetServerTransmissionMessageImage((FactionType)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetShardQualityNumeric((CrateSupplyVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetShardUnlockSupplyName((IDeployableVO)GCHandledObjects.GCHandleToObject(*args), (ShardVO)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetSingleCurrencyIconAssetName((CurrencyType)(*(int*)args)));
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetSingleCurrencyIconAssetName(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetSingleCurrencyItemAssetName((CurrencyType)(*(int*)args)));
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetSingleCurrencyItemAssetName(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetSquadLevelFromInvestedRep(*(int*)args));
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetSquadLevelUIDFromLevel(*(int*)args));
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetSquadLevelUIDFromSquad((Squad)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetSquaredDistanceToTarget((ShooterComponent)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetTimeLabelFromSeconds(*(int*)args));
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetTournamentPointIconName(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetTransmissionHoloId((FactionType)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetWorldOwner());
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetWorldOwnerChampionCount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetWorldOwnerHeroCount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetWorldOwnerSpecialAttackCount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.GetWorldOwnerTroopCount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			GameUtils.HandleCratePurchaseResponse((CrateDataResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.HandleSoftCurrencyFlow(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.HasAvailableTroops(*(sbyte*)args != 0, (BattleTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.HasEnoughCurrencyStorage((CurrencyType)(*(int*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.HasUserFactionFlipped((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			GameUtils.IndicateNewInventoryItems((RewardVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsAppLoading());
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsBattleVersionSupported(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsBuildingMovable((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsBuildingTypeValidForBattleConditions((BuildingType)(*(int*)args)));
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsBuildingUpgradable((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsDeviceCountryInList(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsEligibleToFindTarget((ShooterComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsEntityDead((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsEntityShieldGenerator((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsMissionDefense(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsMissionRaidDefense(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsMissionSpecOps(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsPlanetCurrentOne(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsUnlockBlockingScreenOpen());
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsVideoShareSupported(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsVisitingBase());
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.IsVisitingNeighbor());
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			GameUtils.ListToAdditiveMap((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.ListToMap((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args)));
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			GameUtils.LogComponentsAsError(Marshal.PtrToStringUni(*(IntPtr*)args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.MaterialsCrystalCost(*(int*)args));
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.MultiCurrencyCrystalCost((Dictionary<CurrencyType, int>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.NearestPointOnRect(*(int*)args, *(int*)(args + 1), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			GameUtils.OnBuyMoreCrystals(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			GameUtils.OnOpenURLModalResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			GameUtils.OpenCrate((CrateData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			GameUtils.OpenInventoryCrateModal((CrateData)GCHandledObjects.GCHandleToObject(*args), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			GameUtils.OpenStoreTreasureTab();
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			GameUtils.OpenURL(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			GameUtils.PromptToBuyCrystals();
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.RectContainsRect(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), *(int*)(args + 5), *(int*)(args + 6), *(int*)(args + 7)));
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.RectsIntersect(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), *(int*)(args + 5), *(int*)(args + 6), *(int*)(args + 7)));
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			GameUtils.ReturnToHomeCompleteNowBuyMoreCrystals(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.SafeVOEqualityValidation((IValueObject)GCHandledObjects.GCHandleToObject(*args), (IValueObject)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.SecondsToCrystals(*(int*)args));
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.SecondsToCrystalsForPerk(*(int*)args));
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			GameUtils.ShowCrateAwardModal(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			GameUtils.ShowNotEnoughStorageMessage((CurrencyType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.SortBuildingByUID((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.SpendCrystals(*(int*)args));
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			GameUtils.SpendCurrency((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			GameUtils.SpendCurrency(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			GameUtils.SpendCurrency(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), *(sbyte*)(args + 5) != 0);
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			GameUtils.SpendCurrencyWithMultiplier(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(float*)(args + 3), *(sbyte*)(args + 4) != 0);
			return -1L;
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			GameUtils.SpendHQScaledCurrency((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.SquaredDistance(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.StringHash(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			GameUtils.SwapShaderIfNeeded((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), (Shader)GCHandledObjects.GCHandleToObject(args[1]), (Material)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			GameUtils.ToggleGameObjectViewVisibility((GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameUtils.TraverseSpiral(*(int*)args, *(int*)(args + 1), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			GameUtils.TryAndOpenAppropriateStorePage();
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			GameUtils.UpdateInventoryCrateBadgeCount(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			GameUtils.UpdateMinimumFrameCountForNextTargeting((ShooterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
