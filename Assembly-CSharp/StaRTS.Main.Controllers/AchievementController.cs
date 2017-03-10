using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class AchievementController : IEventObserver
	{
		private Dictionary<string, string> rebelBuildingLevelAchievements;

		private Dictionary<string, string> empireBuildingLevelAchievements;

		private Dictionary<int, string> pveStarsAchievements;

		private Dictionary<int, string> lootCreditsAchievements;

		private Dictionary<int, string> lootAlloyAchievements;

		private Dictionary<int, string> lootContrabandAchievements;

		private Dictionary<int, string> pvpBattlesWonAchievements;

		public AchievementController()
		{
			Service.Set<AchievementController>(this);
			this.BuildAchievementLists();
			Service.Get<EventManager>().RegisterObserver(this, EventId.GameServicesSignedIn, EventPriority.Default);
		}

		private void BuildAchievementLists()
		{
			this.rebelBuildingLevelAchievements = new Dictionary<string, string>();
			this.empireBuildingLevelAchievements = new Dictionary<string, string>();
			this.pveStarsAchievements = new Dictionary<int, string>();
			this.lootCreditsAchievements = new Dictionary<int, string>();
			this.lootAlloyAchievements = new Dictionary<int, string>();
			this.lootContrabandAchievements = new Dictionary<int, string>();
			this.pvpBattlesWonAchievements = new Dictionary<int, string>();
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, AchievementVO>.ValueCollection all = dataController.GetAll<AchievementVO>();
			foreach (AchievementVO current in all)
			{
				string iosAchievementId = current.IosAchievementId;
				if (current.AchievementType == AchievementType.BuildingLevel)
				{
					this.rebelBuildingLevelAchievements.Add(current.RebelData, iosAchievementId);
					this.empireBuildingLevelAchievements.Add(current.EmpireData, iosAchievementId);
				}
				else
				{
					int key = Convert.ToInt32(current.RebelData, CultureInfo.InvariantCulture);
					switch (current.AchievementType)
					{
					case AchievementType.PveStars:
						this.pveStarsAchievements.Add(key, iosAchievementId);
						break;
					case AchievementType.LootCreditsPvp:
						this.lootCreditsAchievements.Add(key, iosAchievementId);
						break;
					case AchievementType.LootAlloyPvp:
						this.lootAlloyAchievements.Add(key, iosAchievementId);
						break;
					case AchievementType.LootContrabandPvp:
						this.lootContrabandAchievements.Add(key, iosAchievementId);
						break;
					case AchievementType.PvpWon:
						this.pvpBattlesWonAchievements.Add(key, iosAchievementId);
						break;
					}
				}
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.GameServicesSignedIn)
			{
				this.TryRetroactiveAchievements();
			}
			return EatResponse.NotEaten;
		}

		public void TryRetroactiveAchievements()
		{
			Dictionary<string, string>.KeyCollection keyCollection = null;
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			if (faction != FactionType.Empire)
			{
				if (faction == FactionType.Rebel)
				{
					keyCollection = this.rebelBuildingLevelAchievements.Keys;
				}
			}
			else
			{
				keyCollection = this.empireBuildingLevelAchievements.Keys;
			}
			if (keyCollection != null)
			{
				List<Building> list = new List<Building>();
				foreach (string current in keyCollection)
				{
					int indexOfFirstNumericCharacter = StringUtils.GetIndexOfFirstNumericCharacter(current);
					if (indexOfFirstNumericCharacter >= 0)
					{
						int num = Convert.ToInt32(current.Substring(indexOfFirstNumericCharacter), CultureInfo.InvariantCulture);
						string baseUId = current.Substring(0, indexOfFirstNumericCharacter);
						list.Clear();
						Service.Get<CurrentPlayer>().Map.GetAllBuildingsWithBaseUid(baseUId, list);
						for (int i = 0; i < list.Count; i++)
						{
							string uid = list[i].Uid;
							indexOfFirstNumericCharacter = StringUtils.GetIndexOfFirstNumericCharacter(uid);
							if (indexOfFirstNumericCharacter >= 0 && num <= Convert.ToInt32(uid.Substring(indexOfFirstNumericCharacter), CultureInfo.InvariantCulture))
							{
								this.TryUnlockAchievementById(AchievementType.BuildingLevel, current);
							}
						}
					}
				}
			}
			this.TryUnlockAchievementByValue(AchievementType.PveStars, Service.Get<CurrentPlayer>().CampaignProgress.GetTotalCampaignStarsEarnedInAllCampaigns());
			this.TryUnlockAchievementByValue(AchievementType.LootCreditsPvp, Service.Get<CurrentPlayer>().BattleHistory.GetTotalPvpCreditsLooted());
			this.TryUnlockAchievementByValue(AchievementType.LootAlloyPvp, Service.Get<CurrentPlayer>().BattleHistory.GetTotalPvpMaterialLooted());
			this.TryUnlockAchievementByValue(AchievementType.LootContrabandPvp, Service.Get<CurrentPlayer>().BattleHistory.GetTotalPvpContrabandLooted());
			this.TryUnlockAchievementByValue(AchievementType.PvpWon, Service.Get<CurrentPlayer>().BattleHistory.GetTotalPvpWins());
		}

		private Dictionary<string, string> GetAchievementIdListByType(AchievementType achievementType)
		{
			Dictionary<string, string> result = null;
			if (achievementType == AchievementType.BuildingLevel)
			{
				FactionType faction = Service.Get<CurrentPlayer>().Faction;
				if (faction != FactionType.Empire)
				{
					if (faction == FactionType.Rebel)
					{
						result = this.rebelBuildingLevelAchievements;
					}
				}
				else
				{
					result = this.empireBuildingLevelAchievements;
				}
			}
			return result;
		}

		private Dictionary<int, string> GetAchievementValueListByType(AchievementType achievementType)
		{
			Dictionary<int, string> result = null;
			switch (achievementType)
			{
			case AchievementType.PveStars:
				result = this.pveStarsAchievements;
				break;
			case AchievementType.LootCreditsPvp:
				result = this.lootCreditsAchievements;
				break;
			case AchievementType.LootAlloyPvp:
				result = this.lootAlloyAchievements;
				break;
			case AchievementType.LootContrabandPvp:
				result = this.lootContrabandAchievements;
				break;
			case AchievementType.PvpWon:
				result = this.pvpBattlesWonAchievements;
				break;
			}
			return result;
		}

		private void UnlockAchievement(string achievementID)
		{
		}

		public bool TryUnlockAchievementById(AchievementType achievementType, string achievementData)
		{
			bool result = false;
			if (achievementData != null)
			{
				Dictionary<string, string> achievementIdListByType = this.GetAchievementIdListByType(achievementType);
				if (achievementIdListByType != null && achievementIdListByType.ContainsKey(achievementData))
				{
					result = true;
					this.UnlockAchievement(achievementIdListByType[achievementData]);
				}
			}
			return result;
		}

		public bool TryUnlockAchievementByValue(AchievementType achievementType, int achievementData)
		{
			bool result = false;
			Dictionary<int, string> achievementValueListByType = this.GetAchievementValueListByType(achievementType);
			if (achievementValueListByType != null)
			{
				Dictionary<int, string>.KeyCollection keys = achievementValueListByType.Keys;
				foreach (int current in keys)
				{
					int num = -1;
					if (achievementData >= current)
					{
						num = current;
					}
					if (num >= 0)
					{
						result = true;
						this.UnlockAchievement(achievementValueListByType[num]);
					}
				}
			}
			return result;
		}

		protected internal AchievementController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AchievementController)GCHandledObjects.GCHandleToObject(instance)).BuildAchievementLists();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementController)GCHandledObjects.GCHandleToObject(instance)).GetAchievementIdListByType((AchievementType)(*(int*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementController)GCHandledObjects.GCHandleToObject(instance)).GetAchievementValueListByType((AchievementType)(*(int*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AchievementController)GCHandledObjects.GCHandleToObject(instance)).TryRetroactiveAchievements();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementController)GCHandledObjects.GCHandleToObject(instance)).TryUnlockAchievementById((AchievementType)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AchievementController)GCHandledObjects.GCHandleToObject(instance)).TryUnlockAchievementByValue((AchievementType)(*(int*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AchievementController)GCHandledObjects.GCHandleToObject(instance)).UnlockAchievement(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
