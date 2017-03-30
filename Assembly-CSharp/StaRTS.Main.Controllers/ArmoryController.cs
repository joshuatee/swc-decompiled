using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Equipment;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Player;
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
	public class ArmoryController : IEventObserver
	{
		private const string ARMORY_FULL = "ARMORY_FULL";

		private const string BASE_ON_INCORRECT_PLANET = "BASE_ON_INCORRECT_PLANET";

		private const int EQUIPMENT_AUTO_UPGRADE_LEVEL = 1;

		private EquipmentVO pendingCelebrationEquipment;

		public bool AllowUnlockCelebration = true;

		public bool AllowShowEquipmentTabBadge
		{
			get;
			set;
		}

		public ArmoryController()
		{
			Service.Set<ArmoryController>(this);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.PlanetConfirmRelocate);
			eventManager.RegisterObserver(this, EventId.BattleLoadStart);
			eventManager.RegisterObserver(this, EventId.BattleLoadedForDefend);
			eventManager.RegisterObserver(this, EventId.BattleReplaySetup);
			eventManager.RegisterObserver(this, EventId.BattleRecordRetrieved);
			eventManager.RegisterObserver(this, EventId.BattleEndFullyProcessed);
			eventManager.RegisterObserver(this, EventId.BattleLeftBeforeStarting);
			eventManager.RegisterObserver(this, EventId.BattleReplayEnded);
			eventManager.RegisterObserver(this, EventId.EquipmentUpgraded);
			eventManager.RegisterObserver(this, EventId.EquipmentUnlocked);
			eventManager.RegisterObserver(this, EventId.ScreenLoaded);
			eventManager.RegisterObserver(this, EventId.EquipmentActivated);
			eventManager.RegisterObserver(this, EventId.EquipmentDeactivated);
			this.AllowShowEquipmentTabBadge = true;
		}

		public void ActivateEquipment(string equipmentId)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			EquipmentVO currentEquipmentDataByID = ArmoryUtils.GetCurrentEquipmentDataByID(equipmentId);
			if (currentEquipmentDataByID == null)
			{
				Service.Get<Logger>().Warn("Invalid EquipmentID: " + equipmentId);
				return;
			}
			if (currentPlayer.ActiveArmory.Equipment.Contains(currentEquipmentDataByID.Uid))
			{
				return;
			}
			if (ArmoryUtils.GetCurrentActiveEquipmentCapacity(currentPlayer.ActiveArmory) + currentEquipmentDataByID.Size > currentPlayer.ActiveArmory.MaxCapacity)
			{
				string instructions = Service.Get<Lang>().Get("ARMORY_FULL", new object[0]);
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(instructions);
				return;
			}
			if (!ArmoryUtils.IsEquipmentOnValidPlanet(currentPlayer, currentEquipmentDataByID))
			{
				string instructions2 = Service.Get<Lang>().Get("BASE_ON_INCORRECT_PLANET", new object[0]);
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(instructions2);
				return;
			}
			currentPlayer.ActiveArmory.Equipment.Add(currentEquipmentDataByID.Uid);
			Service.Get<EventManager>().SendEvent(EventId.EquipmentActivated, currentEquipmentDataByID);
			EquipmentIdRequest request = new EquipmentIdRequest(equipmentId);
			ActivateEquipmentCommand activateEquipmentCommand = new ActivateEquipmentCommand(request);
			activateEquipmentCommand.Context = equipmentId;
			activateEquipmentCommand.AddFailureCallback(new AbstractCommand<EquipmentIdRequest, DefaultResponse>.OnFailureCallback(this.OnActivateEquipmentFailure));
			Service.Get<ServerAPI>().Enqueue(activateEquipmentCommand);
		}

		public void DeactivateEquipment(string equipmentId)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			EquipmentVO currentEquipmentDataByID = ArmoryUtils.GetCurrentEquipmentDataByID(equipmentId);
			if (!currentPlayer.ActiveArmory.Equipment.Contains(currentEquipmentDataByID.Uid))
			{
				return;
			}
			this.DeactivateEquipmentOnClient(currentPlayer, currentEquipmentDataByID);
			EquipmentIdRequest request = new EquipmentIdRequest(equipmentId);
			DeactivateEquipmentCommand deactivateEquipmentCommand = new DeactivateEquipmentCommand(request);
			deactivateEquipmentCommand.Context = equipmentId;
			deactivateEquipmentCommand.AddFailureCallback(new AbstractCommand<EquipmentIdRequest, DefaultResponse>.OnFailureCallback(this.OnDeactivateEquipmentFailure));
			Service.Get<ServerAPI>().Enqueue(deactivateEquipmentCommand);
		}

		public void DeactivateEquipmentOnClient(CurrentPlayer player, EquipmentVO equipment)
		{
			player.ActiveArmory.Equipment.Remove(equipment.Uid);
			Service.Get<EventManager>().SendEvent(EventId.EquipmentDeactivated, equipment);
		}

		public void HandleEarnedShardReward(string equipmentId, int count)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Dictionary<string, int> shards = currentPlayer.Shards;
			EquipmentVO equipmentDataByID = ArmoryUtils.GetEquipmentDataByID(equipmentId, 1);
			if (equipmentDataByID == null)
			{
				return;
			}
			int num = count;
			if (shards.ContainsKey(equipmentId))
			{
				num += shards[equipmentId];
			}
			currentPlayer.ModifyShardAmount(equipmentId, num);
			EquipmentVO nextEquipmentVOUpgrade = this.GetNextEquipmentVOUpgrade(equipmentId);
			if (nextEquipmentVOUpgrade != null && shards[equipmentId] - count < nextEquipmentVOUpgrade.UpgradeShards && nextEquipmentVOUpgrade.UpgradeShards < shards[equipmentId])
			{
				this.AllowShowEquipmentTabBadge = true;
				Service.Get<EventManager>().SendEvent(EventId.EquipmentNowUpgradable, equipmentId);
			}
			Service.Get<EventManager>().SendEvent(EventId.ShardsEarned, null);
			if (!ArmoryUtils.IsEquipmentOwned(currentPlayer, equipmentDataByID))
			{
				this.TryToUnlockPlayerEquipment(equipmentDataByID);
			}
		}

		public bool ChargeEquipmentUpgradeCost(EquipmentVO equipmentVO)
		{
			if (equipmentVO == null)
			{
				return false;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Dictionary<string, int> shards = currentPlayer.Shards;
			int upgradeShards = equipmentVO.UpgradeShards;
			string equipmentID = equipmentVO.EquipmentID;
			int num = (!shards.ContainsKey(equipmentID)) ? 0 : shards[equipmentID];
			if (!this.IsEquipmentUnlockableOrUpgradeable(equipmentVO))
			{
				return false;
			}
			currentPlayer.ModifyShardAmount(equipmentID, num - upgradeShards);
			return true;
		}

		private bool TryToUnlockPlayerEquipment(EquipmentVO equipmentVO)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			string equipmentID = equipmentVO.EquipmentID;
			if (currentPlayer.UnlockedLevels.Equipment.Levels.ContainsKey(equipmentID))
			{
				Service.Get<Logger>().Warn("Tried to unlock equipment that is already unlocked.");
				return false;
			}
			if (!this.ChargeEquipmentUpgradeCost(equipmentVO))
			{
				Service.Get<Logger>().Debug("Tried to unlock equipment with insufficient shards.");
				return false;
			}
			currentPlayer.UnlockedLevels.Equipment.Levels.Add(equipmentID, equipmentVO.Lvl);
			Service.Get<EventManager>().SendEvent(EventId.EquipmentUnlocked, equipmentVO);
			return true;
		}

		private void UpdateActiveArmoryLevel(EquipmentVO equipmentVO)
		{
			if (equipmentVO == null)
			{
				Service.Get<Logger>().Warn("Equipment is null");
				return;
			}
			IDataController dataController = Service.Get<IDataController>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			List<string> equipment = currentPlayer.ActiveArmory.Equipment;
			int i = 0;
			int count = equipment.Count;
			while (i < count)
			{
				string text = equipment[i];
				EquipmentVO equipmentVO2 = dataController.Get<EquipmentVO>(text);
				if (equipmentVO2.EquipmentID == equipmentVO.EquipmentID)
				{
					equipment.Remove(text);
					equipment.Add(equipmentVO.Uid);
					break;
				}
				i++;
			}
		}

		public bool DoesUserHaveAnyUpgradableEquipment()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			foreach (string current in currentPlayer.UnlockedLevels.Equipment.Levels.Keys)
			{
				EquipmentVO nextEquipmentVOUpgrade = this.GetNextEquipmentVOUpgrade(current);
				if (nextEquipmentVOUpgrade != null)
				{
					if (nextEquipmentVOUpgrade.PlayerFacing)
					{
						if (nextEquipmentVOUpgrade.Faction == currentPlayer.Faction)
						{
							if (this.IsEquipmentUnlockableOrUpgradeable(nextEquipmentVOUpgrade))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool IsEquipmentUnlockableOrUpgradeable(EquipmentVO equipment)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			bool flag = ArmoryUtils.CanAffordEquipment(currentPlayer, equipment);
			bool flag2 = ArmoryUtils.IsBuildingRequirementMet(equipment);
			return flag && flag2;
		}

		public EquipmentVO GetNextEquipmentVOUpgrade(string equipmentID)
		{
			IDataController dataController = Service.Get<IDataController>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int num = 0;
			if (currentPlayer.UnlockedLevels.Equipment.Levels.ContainsKey(equipmentID))
			{
				num = currentPlayer.UnlockedLevels.Equipment.Levels[equipmentID];
			}
			num++;
			foreach (EquipmentVO current in dataController.GetAll<EquipmentVO>())
			{
				if (current.EquipmentID == equipmentID && current.Lvl == num)
				{
					return current;
				}
			}
			return null;
		}

		private void OnActivateEquipmentFailure(uint status, object cookie)
		{
			Service.Get<Logger>().ErrorFormat("Activate equipmentID '{0}' failed!", new object[]
			{
				cookie as string
			});
		}

		private void OnDeactivateEquipmentFailure(uint status, object cookie)
		{
			Service.Get<Logger>().ErrorFormat("Deactivate equipmentID '{0}' failed!", new object[]
			{
				cookie as string
			});
		}

		public static void AddEquipmentBuffs(List<string> attackerEquipment, List<string> defenderEquipment)
		{
			BuffController buffController = Service.Get<BuffController>();
			buffController.ClearEquipmentBuffs();
			IDataController dataController = Service.Get<IDataController>();
			if (attackerEquipment != null)
			{
				int i = 0;
				int count = attackerEquipment.Count;
				while (i < count)
				{
					EquipmentVO equipmentVO = dataController.Get<EquipmentVO>(attackerEquipment[i]);
					string[] effectUids = equipmentVO.EffectUids;
					if (effectUids != null)
					{
						int j = 0;
						int num = effectUids.Length;
						while (j < num)
						{
							EquipmentEffectVO effectVO = dataController.Get<EquipmentEffectVO>(effectUids[j]);
							buffController.AddAttackerEquipmentBuff(effectVO);
							j++;
						}
					}
					i++;
				}
			}
			if (defenderEquipment != null)
			{
				int k = 0;
				int count2 = defenderEquipment.Count;
				while (k < count2)
				{
					EquipmentVO equipmentVO2 = dataController.Get<EquipmentVO>(defenderEquipment[k]);
					string[] effectUids2 = equipmentVO2.EffectUids;
					if (effectUids2 != null)
					{
						int l = 0;
						int num2 = effectUids2.Length;
						while (l < num2)
						{
							EquipmentEffectVO effectVO2 = dataController.Get<EquipmentEffectVO>(effectUids2[l]);
							buffController.AddDefenderEquipmentBuff(effectVO2);
							l++;
						}
					}
					k++;
				}
			}
		}

		public bool IsEquipmentUpgradeable(EquipmentVO equipmentVO, EquipmentVO nextEquipmentVO)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			bool flag = ArmoryUtils.IsEquipmentOwned(currentPlayer, equipmentVO);
			bool flag2 = this.IsEquipmentUnlockableOrUpgradeable(nextEquipmentVO);
			return flag && flag2;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			switch (id)
			{
			case EventId.BattleLoadStart:
			case EventId.BattleLoadedForDefend:
			{
				CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
				ArmoryController.AddEquipmentBuffs(currentBattle.AttackerEquipment, currentBattle.DefenderEquipment);
				return EatResponse.NotEaten;
			}
			case EventId.BattleLoadEnd:
			case EventId.BattleEndProcessing:
			case EventId.BattleEndRecorded:
			case EventId.BattleCancelRequested:
			case EventId.BattleCanceled:
			case EventId.BattleNextRequested:
			case EventId.BattleReplayRequested:
				IL_48:
				switch (id)
				{
				case EventId.EquipmentUnlocked:
					this.pendingCelebrationEquipment = (cookie as EquipmentVO);
					this.UpdateLastEquipmentUnlocked(this.pendingCelebrationEquipment.Uid);
					if (this.AllowUnlockCelebration)
					{
						if (GameUtils.IsUnlockBlockingScreenOpen())
						{
							Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenClosing);
						}
						else
						{
							this.ShowEquipmentUnlockedCelebration();
						}
					}
					return EatResponse.NotEaten;
				case EventId.EquipmentUpgraded:
				{
					ContractEventData contractEventData = cookie as ContractEventData;
					IDataController dataController = Service.Get<IDataController>();
					EquipmentVO equipmentVO = dataController.Get<EquipmentVO>(contractEventData.Contract.ProductUid);
					this.UpdateActiveArmoryLevel(equipmentVO);
					return EatResponse.NotEaten;
				}
				case EventId.EquipmentActivated:
				case EventId.EquipmentDeactivated:
					this.UpdateArmoryBuildingTooltip();
					return EatResponse.NotEaten;
				default:
				{
					if (id == EventId.ScreenClosing)
					{
						if (cookie is InventoryCrateCollectionScreen)
						{
							GameUtils.CloseStoreOrInventoryScreen();
							Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenClosing);
							if (this.AllowUnlockCelebration)
							{
								this.ShowEquipmentUnlockedCelebration();
							}
						}
						return EatResponse.NotEaten;
					}
					if (id == EventId.ScreenLoaded)
					{
						if (cookie is ArmoryScreen)
						{
							this.UpdateLastEquipmentUnlocked("false");
						}
						return EatResponse.NotEaten;
					}
					if (id != EventId.PlanetConfirmRelocate)
					{
						return EatResponse.NotEaten;
					}
					CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
					IDataController dataController2 = Service.Get<IDataController>();
					for (int i = currentPlayer.ActiveArmory.Equipment.Count - 1; i >= 0; i--)
					{
						EquipmentVO equipment = dataController2.Get<EquipmentVO>(currentPlayer.ActiveArmory.Equipment[i]);
						if (!ArmoryUtils.IsEquipmentValidForPlanet(equipment, (string)cookie))
						{
							this.DeactivateEquipmentOnClient(currentPlayer, equipment);
						}
					}
					return EatResponse.NotEaten;
				}
				}
				break;
			case EventId.BattleEndFullyProcessed:
			case EventId.BattleLeftBeforeStarting:
			case EventId.BattleReplayEnded:
				Service.Get<BuffController>().ClearEquipmentBuffs();
				return EatResponse.NotEaten;
			case EventId.BattleReplaySetup:
			{
				BattleRecord battleRecord = (BattleRecord)cookie;
				ArmoryController.AddEquipmentBuffs(battleRecord.AttackerEquipment, battleRecord.DefenderEquipment);
				return EatResponse.NotEaten;
			}
			case EventId.BattleRecordRetrieved:
			{
				GetReplayResponse getReplayResponse = (GetReplayResponse)cookie;
				BattleRecord replayData = getReplayResponse.ReplayData;
				ArmoryController.AddEquipmentBuffs(replayData.AttackerEquipment, replayData.DefenderEquipment);
				return EatResponse.NotEaten;
			}
			}
			goto IL_48;
		}

		private void UpdateLastEquipmentUnlocked(string equipmentUid)
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			if (sharedPlayerPrefs.GetPref<string>("NewEqp") != equipmentUid)
			{
				sharedPlayerPrefs.SetPref("NewEqp", equipmentUid);
				this.UpdateArmoryBuildingTooltip();
			}
		}

		private void UpdateArmoryBuildingTooltip()
		{
			SmartEntity smartEntity = (SmartEntity)Service.Get<BuildingLookupController>().GetCurrentArmory();
			if (smartEntity != null)
			{
				Service.Get<BuildingTooltipController>().EnsureBuildingTooltip(smartEntity);
			}
		}

		private void ShowEquipmentUnlockedCelebration()
		{
			Service.Get<ScreenController>().AddScreen(new EquipmentUnlockedCelebrationScreen(this.pendingCelebrationEquipment), QueueScreenBehavior.QueueAndDeferTillClosed);
			this.pendingCelebrationEquipment = null;
		}
	}
}
