using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers
{
	public class DeployableShardUnlockController : IEventObserver
	{
		private List<IDeployableVO> deployablesToCelebrate;

		public bool AllowResearchBuildingBadging
		{
			get;
			set;
		}

		public DeployableShardUnlockController()
		{
			Service.Set<DeployableShardUnlockController>(this);
			this.deployablesToCelebrate = new List<IDeployableVO>();
			Service.Get<EventManager>().RegisterObserver(this, EventId.ShardUnitUpgraded);
			this.AllowResearchBuildingBadging = true;
		}

		public void GrantUnlockShards(string shardId, int count)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Dictionary<string, int> shards = currentPlayer.Shards;
			IDataController dataController = Service.Get<IDataController>();
			ShardVO shardVO = dataController.Get<ShardVO>(shardId);
			string targetType = shardVO.TargetType;
			string targetGroupId = shardVO.TargetGroupId;
			int num = count;
			if (shards.ContainsKey(shardId))
			{
				num += shards[shardId];
			}
			currentPlayer.ModifyShardAmount(shardId, num);
			int upgradeLevelOfDeployable = this.GetUpgradeLevelOfDeployable(targetType, targetGroupId);
			if (upgradeLevelOfDeployable == 0)
			{
				this.AttemptToUpgradeUnitWithShards(shardId, 1);
			}
			else
			{
				IDeployableVO deployableVOForLevelInGroup = this.GetDeployableVOForLevelInGroup(upgradeLevelOfDeployable, targetType, targetGroupId);
				if (deployableVOForLevelInGroup == null)
				{
					Service.Get<Logger>().ErrorFormat("No deployableVO found for targetType: {0}, targetGroup: {1}, upgradeLevel: {2}", new object[]
					{
						targetType,
						targetGroupId,
						upgradeLevelOfDeployable
					});
					return;
				}
				if (num - count < deployableVOForLevelInGroup.UpgradeShardCount && num >= deployableVOForLevelInGroup.UpgradeShardCount)
				{
					this.AllowResearchBuildingBadging = true;
					Service.Get<EventManager>().SendEvent(EventId.ShardUnitNowUpgradable, deployableVOForLevelInGroup.Uid);
				}
			}
		}

		public bool IsUIDForAShardUpgradableDeployable(string uid)
		{
			IDataController dataController = Service.Get<IDataController>();
			IDeployableVO optional = dataController.GetOptional<TroopTypeVO>(uid);
			if (optional == null)
			{
				optional = dataController.GetOptional<SpecialAttackTypeVO>(uid);
			}
			return optional != null && !string.IsNullOrEmpty(optional.UpgradeShardUid);
		}

		public int GetUpgradeQualityForDeployableUID(string uid)
		{
			IDataController dataController = Service.Get<IDataController>();
			IDeployableVO optional = dataController.GetOptional<TroopTypeVO>(uid);
			if (optional == null)
			{
				optional = dataController.GetOptional<SpecialAttackTypeVO>(uid);
			}
			if (optional == null)
			{
				return 0;
			}
			return this.GetUpgradeQualityForDeployable(optional);
		}

		public int GetUpgradeQualityForDeployable(IDeployableVO deployable)
		{
			int result = 0;
			string upgradeShardUid = deployable.UpgradeShardUid;
			if (!string.IsNullOrEmpty(upgradeShardUid))
			{
				ShardVO shardVO = Service.Get<IDataController>().Get<ShardVO>(upgradeShardUid);
				result = (int)shardVO.Quality;
			}
			return result;
		}

		private bool IsShardDeployableReadyToUpgrade(string shardId)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Dictionary<string, int> shards = currentPlayer.Shards;
			IDataController dataController = Service.Get<IDataController>();
			ShardVO shardVO = dataController.Get<ShardVO>(shardId);
			bool flag = shardVO.TargetType == "specialAttack";
			int num = 0;
			if (shards.ContainsKey(shardId))
			{
				num += shards[shardId];
			}
			int upgradeLevelOfDeployable = this.GetUpgradeLevelOfDeployable(shardVO.TargetType, shardVO.TargetGroupId);
			IDeployableVO deployableVOForLevelInGroup;
			if (flag)
			{
				deployableVOForLevelInGroup = this.GetDeployableVOForLevelInGroup<SpecialAttackTypeVO>(upgradeLevelOfDeployable + 1, shardVO.TargetGroupId);
			}
			else
			{
				deployableVOForLevelInGroup = this.GetDeployableVOForLevelInGroup<TroopTypeVO>(upgradeLevelOfDeployable + 1, shardVO.TargetGroupId);
			}
			return deployableVOForLevelInGroup != null && this.DoesUserHaveUpgradeShardRequirement(deployableVOForLevelInGroup);
		}

		private void AttemptToUpgradeUnitWithShards(string shardId, int level)
		{
			if (level <= 0)
			{
				return;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Dictionary<string, int> shards = currentPlayer.Shards;
			IDataController dataController = Service.Get<IDataController>();
			ShardVO shardVO = dataController.Get<ShardVO>(shardId);
			string targetType = shardVO.TargetType;
			string targetGroupId = shardVO.TargetGroupId;
			bool flag = targetType == "specialAttack";
			int num = 0;
			if (shards.ContainsKey(shardId))
			{
				num += shards[shardId];
			}
			int startLevel = level - 1;
			int numShardsForDeployableToReachLevelInGroup;
			if (flag)
			{
				numShardsForDeployableToReachLevelInGroup = this.GetNumShardsForDeployableToReachLevelInGroup<SpecialAttackTypeVO>(startLevel, level, targetGroupId);
			}
			else
			{
				numShardsForDeployableToReachLevelInGroup = this.GetNumShardsForDeployableToReachLevelInGroup<TroopTypeVO>(startLevel, level, targetGroupId);
			}
			if (num >= numShardsForDeployableToReachLevelInGroup)
			{
				UnlockedLevelData unlockedLevels = Service.Get<CurrentPlayer>().UnlockedLevels;
				IDeployableVO deployableVOForLevelInGroup;
				if (flag)
				{
					deployableVOForLevelInGroup = this.GetDeployableVOForLevelInGroup<SpecialAttackTypeVO>(1, targetGroupId);
				}
				else
				{
					deployableVOForLevelInGroup = this.GetDeployableVOForLevelInGroup<TroopTypeVO>(1, targetGroupId);
				}
				if (deployableVOForLevelInGroup == null)
				{
					return;
				}
				unlockedLevels.UpgradeTroopsOrStarships(deployableVOForLevelInGroup.Uid, flag);
				currentPlayer.ModifyShardAmount(shardId, num - numShardsForDeployableToReachLevelInGroup);
				Service.Get<EventManager>().SendEvent(EventId.ShardUnitUpgraded, deployableVOForLevelInGroup);
			}
		}

		public void SpendDeployableShard(string shardUID, int amount)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.Shards.ContainsKey(shardUID))
			{
				int num = currentPlayer.Shards[shardUID];
				currentPlayer.ModifyShardAmount(shardUID, num - amount);
			}
		}

		public int GetShardAmount(string shardUID)
		{
			Dictionary<string, int> shards = Service.Get<CurrentPlayer>().Shards;
			int result = 0;
			if (shards.ContainsKey(shardUID))
			{
				result = shards[shardUID];
			}
			return result;
		}

		public bool DoesUserHaveAnyUpgradeableShardUnits()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			TroopUpgradeCatalog troopUpgradeCatalog = Service.Get<TroopUpgradeCatalog>();
			StarshipUpgradeCatalog starshipUpgradeCatalog = Service.Get<StarshipUpgradeCatalog>();
			foreach (string current in troopUpgradeCatalog.AllUpgradeGroups())
			{
				int nextLevel = currentPlayer.UnlockedLevels.Troops.GetNextLevel(current);
				TroopTypeVO byLevel = troopUpgradeCatalog.GetByLevel(current, nextLevel);
				if (byLevel != null)
				{
					if (byLevel.PlayerFacing)
					{
						if (byLevel.Type != TroopType.Champion)
						{
							if (byLevel.Faction == currentPlayer.Faction)
							{
								if (!string.IsNullOrEmpty(byLevel.UpgradeShardUid))
								{
									if (this.DoesUserHaveUpgradeShardRequirement(byLevel))
									{
										bool result = true;
										return result;
									}
								}
							}
						}
					}
				}
			}
			foreach (string current2 in starshipUpgradeCatalog.AllUpgradeGroups())
			{
				int nextLevel2 = currentPlayer.UnlockedLevels.Starships.GetNextLevel(current2);
				SpecialAttackTypeVO byLevel2 = starshipUpgradeCatalog.GetByLevel(current2, nextLevel2);
				if (byLevel2 != null)
				{
					if (byLevel2.PlayerFacing)
					{
						if (byLevel2.Faction == currentPlayer.Faction)
						{
							if (!string.IsNullOrEmpty(byLevel2.UpgradeShardUid))
							{
								if (this.DoesUserHaveUpgradeShardRequirement(byLevel2))
								{
									bool result = true;
									return result;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public bool DoesUserHaveUpgradeShardRequirement(IDeployableVO deployableVO)
		{
			if (deployableVO == null || string.IsNullOrEmpty(deployableVO.UpgradeShardUid))
			{
				return false;
			}
			int shardAmount = this.GetShardAmount(deployableVO.UpgradeShardUid);
			return shardAmount >= deployableVO.UpgradeShardCount;
		}

		public IDeployableVO GetDeployableVOFromShard(ShardVO shard)
		{
			if (shard == null)
			{
				return null;
			}
			int num = this.GetUpgradeLevelOfDeployable(shard.TargetType, shard.TargetGroupId);
			if (num < 1)
			{
				num = 1;
			}
			IDeployableVO deployableVOForLevelInGroup;
			if (shard.TargetType == "specialAttack")
			{
				deployableVOForLevelInGroup = this.GetDeployableVOForLevelInGroup<SpecialAttackTypeVO>(num, shard.TargetGroupId);
			}
			else
			{
				deployableVOForLevelInGroup = this.GetDeployableVOForLevelInGroup<TroopTypeVO>(num, shard.TargetGroupId);
			}
			return deployableVOForLevelInGroup;
		}

		private IDeployableVO GetDeployableVOForLevelInGroup(int level, string targetType, string groupId)
		{
			IDeployableVO deployableVOForLevelInGroup;
			if (targetType == "specialAttack")
			{
				deployableVOForLevelInGroup = this.GetDeployableVOForLevelInGroup<SpecialAttackTypeVO>(level, groupId);
			}
			else
			{
				deployableVOForLevelInGroup = this.GetDeployableVOForLevelInGroup<TroopTypeVO>(level, groupId);
			}
			return deployableVOForLevelInGroup;
		}

		private IDeployableVO GetDeployableVOForLevelInGroup<T>(int level, string groupId) where T : IDeployableVO
		{
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, T>.ValueCollection all = dataController.GetAll<T>();
			foreach (T current in all)
			{
				int lvl = current.Lvl;
				if (current.UpgradeGroup == groupId && lvl == level)
				{
					return current;
				}
			}
			return null;
		}

		public int GetNumShardsForDeployableToReachLevelInGroup<T>(int startLevel, int endLevel, string groupId) where T : IDeployableVO
		{
			int num = 0;
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, T>.ValueCollection all = dataController.GetAll<T>();
			foreach (T current in all)
			{
				int lvl = current.Lvl;
				if (current.UpgradeGroup == groupId && lvl > startLevel && lvl <= endLevel)
				{
					num += current.UpgradeShardCount;
				}
			}
			return num;
		}

		public int GetUpgradeLevelOfDeployable(string type, string groupId)
		{
			int result = 0;
			UnlockedLevelData unlockedLevels = Service.Get<CurrentPlayer>().UnlockedLevels;
			if (type == "hero" || type == "troop")
			{
				if (unlockedLevels.Troops.Has(groupId))
				{
					result = unlockedLevels.Troops.GetLevel(groupId);
				}
			}
			else if (type == "specialAttack")
			{
				if (unlockedLevels.Starships.Has(groupId))
				{
					result = unlockedLevels.Starships.GetLevel(groupId);
				}
			}
			else
			{
				Service.Get<Logger>().Error("GetUpgradeLevelOfDeployable; Unexpected type: " + type + "with group: " + groupId);
			}
			return result;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ShardUnitUpgraded)
			{
				IDeployableVO deployableVO = (IDeployableVO)cookie;
				if (deployableVO == null)
				{
					return EatResponse.NotEaten;
				}
				if (deployableVO.Lvl > 1)
				{
					return EatResponse.NotEaten;
				}
				this.deployablesToCelebrate.Add(deployableVO);
				if (GameUtils.IsUnlockBlockingScreenOpen())
				{
					Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenClosing);
				}
				else
				{
					this.QueueShowDeployableUnlocks();
				}
			}
			else if (id == EventId.ScreenClosing && cookie is InventoryCrateCollectionScreen)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenClosing);
				GameUtils.CloseStoreOrInventoryScreen();
				this.QueueShowDeployableUnlocks();
			}
			return EatResponse.NotEaten;
		}

		private void QueueShowDeployableUnlocks()
		{
			int count = this.deployablesToCelebrate.Count;
			for (int i = 0; i < count; i++)
			{
				IDeployableVO deployableVO = this.deployablesToCelebrate[i];
				bool isSpecialAttack = deployableVO is SpecialAttackTypeVO;
				this.ShowDeployableUnlockedCelebration(deployableVO, isSpecialAttack);
			}
			this.deployablesToCelebrate.Clear();
		}

		private void ShowDeployableUnlockedCelebration(IDeployableVO vo, bool isSpecialAttack)
		{
			bool isUnlock = vo.Lvl == 1;
			Service.Get<ScreenController>().AddScreen(new DeployableUnlockedCelebrationScreen(vo, isSpecialAttack, isUnlock), QueueScreenBehavior.QueueAndDeferTillClosed);
		}
	}
}
