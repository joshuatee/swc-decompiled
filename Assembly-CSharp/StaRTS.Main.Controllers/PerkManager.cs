using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Perks;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Perks;
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
	public class PerkManager
	{
		private Logger logger;

		public int SquadLevelMax
		{
			get;
			private set;
		}

		public PerkManager()
		{
			Service.Set<PerkManager>(this);
			this.logger = Service.Get<Logger>();
			this.SquadLevelMax = Service.Get<IDataController>().GetAll<SquadLevelVO>().Count;
		}

		public void UpdatePlayerPerksData(object rawPerksData)
		{
			if (rawPerksData != null)
			{
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				currentPlayer.PerksInfo.UpdatePerksData(rawPerksData);
			}
			Service.Get<EventManager>().SendEvent(EventId.ActivePerksUpdated, null);
		}

		public List<ActivatedPerkData> GetPlayerActivatedPerks()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.PerksInfo == null || currentPlayer.PerksInfo.Perks == null)
			{
				return null;
			}
			return currentPlayer.PerksInfo.Perks.ActivatedPerks;
		}

		public Dictionary<string, uint> GetPlayerPerkGroupCooldowns()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.PerksInfo == null || currentPlayer.PerksInfo.Perks == null)
			{
				return null;
			}
			return currentPlayer.PerksInfo.Perks.Cooldowns;
		}

		public List<ActivatedPerkData> GetPlayerPerksInState(PerkState state)
		{
			List<ActivatedPerkData> playerActivatedPerks = this.GetPlayerActivatedPerks();
			List<ActivatedPerkData> list = new List<ActivatedPerkData>();
			if (playerActivatedPerks != null)
			{
				int i = 0;
				int count = playerActivatedPerks.Count;
				while (i < count)
				{
					ActivatedPerkData activatedPerkData = playerActivatedPerks[i];
					if (state == PerkState.Active && this.IsPerkActive(activatedPerkData))
					{
						list.Add(activatedPerkData);
					}
					else if (state == PerkState.Cooldown && this.IsPerkInCooldown(activatedPerkData))
					{
						list.Add(activatedPerkData);
					}
					else if (state == PerkState.Expired && this.IsPerkExpired(activatedPerkData))
					{
						list.Add(activatedPerkData);
					}
					i++;
				}
			}
			return list;
		}

		public List<string> GetPlayerPerkIdsInState(PerkState state)
		{
			List<ActivatedPerkData> playerPerksInState = this.GetPlayerPerksInState(state);
			List<string> list = new List<string>();
			int i = 0;
			int count = playerPerksInState.Count;
			while (i < count)
			{
				ActivatedPerkData activatedPerkData = playerPerksInState[i];
				list.Add(activatedPerkData.PerkId);
				i++;
			}
			return list;
		}

		public bool HasPlayerActivatedFirstPerk()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return currentPlayer.PerksInfo != null && currentPlayer.PerksInfo.Perks != null && currentPlayer.PerksInfo.Perks.HasActivatedFirstPerk;
		}

		public bool HasPlayerSeenPerkTutorial()
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			string pref = sharedPlayerPrefs.GetPref<string>(GameConstants.SQUADPERK_TUTORIAL_ACTIVE_PREF);
			return "2".Equals(pref) || "3".Equals(pref);
		}

		public bool WillShowPerkTutorial()
		{
			bool flag = !Service.Get<PerkManager>().HasPlayerSeenPerkTutorial();
			return flag && Service.Get<UXController>().HUD.IsSquadScreenOpenOrOpeningAndCloseable();
		}

		public List<ActivatedPerkData> GetPlayerActivePerks()
		{
			return this.GetPlayerPerksInState(PerkState.Active);
		}

		public List<string> GetPlayerActivePerkIds()
		{
			return this.GetPlayerPerkIdsInState(PerkState.Active);
		}

		public List<ActivatedPerkData> GetPlayerCooldownPerks()
		{
			return this.GetPlayerPerksInState(PerkState.Cooldown);
		}

		public List<string> GetPlayerCooldownPerkIds()
		{
			return this.GetPlayerPerkIdsInState(PerkState.Cooldown);
		}

		public List<ActivatedPerkData> GetPlayerExpiredPerks()
		{
			return this.GetPlayerPerksInState(PerkState.Expired);
		}

		public List<string> GetPlayerExpiredPerkIds()
		{
			return this.GetPlayerPerkIdsInState(PerkState.Expired);
		}

		public ActivatedPerkData GetPlayerPerk(string perkId)
		{
			List<ActivatedPerkData> playerActivatedPerks = this.GetPlayerActivatedPerks();
			int i = 0;
			int count = playerActivatedPerks.Count;
			while (i < count)
			{
				ActivatedPerkData activatedPerkData = playerActivatedPerks[i];
				if (activatedPerkData.PerkId == perkId)
				{
					return activatedPerkData;
				}
				i++;
			}
			return null;
		}

		public ActivatedPerkData GetPlayerPerkForGroup(string perkGroupId)
		{
			List<ActivatedPerkData> playerActivatedPerks = this.GetPlayerActivatedPerks();
			if (playerActivatedPerks == null)
			{
				return null;
			}
			IDataController dataController = Service.Get<IDataController>();
			int i = 0;
			int count = playerActivatedPerks.Count;
			while (i < count)
			{
				ActivatedPerkData activatedPerkData = playerActivatedPerks[i];
				PerkVO perkVO = dataController.Get<PerkVO>(activatedPerkData.PerkId);
				if (perkGroupId == perkVO.PerkGroup)
				{
					return activatedPerkData;
				}
				i++;
			}
			return null;
		}

		public List<ActivatedPerkData> GetPerksAppliedToBuilding(BuildingTypeVO vo)
		{
			List<ActivatedPerkData> list = new List<ActivatedPerkData>();
			IDataController dataController = Service.Get<IDataController>();
			List<ActivatedPerkData> playerActivatedPerks = this.GetPlayerActivatedPerks();
			int i = 0;
			int count = playerActivatedPerks.Count;
			while (i < count)
			{
				ActivatedPerkData activatedPerkData = playerActivatedPerks[i];
				PerkVO perkVO = dataController.Get<PerkVO>(activatedPerkData.PerkId);
				if (activatedPerkData.EndTime > ServerTime.Time)
				{
					int j = 0;
					int num = perkVO.PerkEffects.Length;
					while (j < num)
					{
						PerkEffectVO perkEffectVO = dataController.Get<PerkEffectVO>(perkVO.PerkEffects[j]);
						bool flag = this.IsPerkEffectAppliedToBuilding(perkEffectVO, vo);
						if (flag)
						{
							list.Add(activatedPerkData);
							break;
						}
						j++;
					}
				}
				i++;
			}
			return list;
		}

		public bool IsPerkAppliedToBuilding(BuildingTypeVO vo)
		{
			List<string> playerActivePerkEffectIds = this.GetPlayerActivePerkEffectIds();
			if (playerActivePerkEffectIds == null)
			{
				return false;
			}
			IDataController dataController = Service.Get<IDataController>();
			int i = 0;
			int count = playerActivePerkEffectIds.Count;
			while (i < count)
			{
				PerkEffectVO perkEffectVO = dataController.Get<PerkEffectVO>(playerActivePerkEffectIds[i]);
				bool flag = this.IsPerkEffectAppliedToBuilding(perkEffectVO, vo);
				if (flag)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public bool IsPerkEffectAppliedToBuilding(PerkEffectVO perkEffectVO, BuildingTypeVO vo)
		{
			BuildingType type = vo.Type;
			return type == perkEffectVO.PerkBuilding && (perkEffectVO.PerkBuilding != BuildingType.Resource || (perkEffectVO.PerkBuilding == BuildingType.Resource && perkEffectVO.Currency == vo.Currency));
		}

		public List<Entity> GetBuildingsForPerk(PerkVO perkVO)
		{
			List<Entity> list = new List<Entity>();
			string[] perkEffects = perkVO.PerkEffects;
			if (perkEffects != null)
			{
				IDataController dataController = Service.Get<IDataController>();
				int i = 0;
				int num = perkEffects.Length;
				while (i < num)
				{
					PerkEffectVO perkEffectVO = dataController.Get<PerkEffectVO>(perkEffects[i]);
					BuildingType perkBuilding = perkEffectVO.PerkBuilding;
					List<Entity> buildingListByType = Service.Get<BuildingLookupController>().GetBuildingListByType(perkBuilding);
					list.AddRange(buildingListByType);
					i++;
				}
			}
			return list;
		}

		public uint GetMaxActivationTimeRemaining(Entity building)
		{
			BuildingComponent buildingComp = ((SmartEntity)building).BuildingComp;
			BuildingTypeVO buildingType = buildingComp.BuildingType;
			List<ActivatedPerkData> perksAppliedToBuilding = this.GetPerksAppliedToBuilding(buildingType);
			uint num = 0u;
			int i = 0;
			int count = perksAppliedToBuilding.Count;
			while (i < count)
			{
				ActivatedPerkData activatedPerkData = perksAppliedToBuilding[i];
				uint num2 = activatedPerkData.EndTime - ServerTime.Time;
				if (activatedPerkData.EndTime > ServerTime.Time && num < num2)
				{
					num = num2;
				}
				i++;
			}
			return num;
		}

		public List<Entity> GetBuildingsForActivePerks()
		{
			List<ActivatedPerkData> playerActivePerks = this.GetPlayerActivePerks();
			List<Entity> list = new List<Entity>();
			int i = 0;
			int count = playerActivePerks.Count;
			while (i < count)
			{
				PerkVO perkVO = Service.Get<IDataController>().Get<PerkVO>(playerActivePerks[i].PerkId);
				list.AddRange(this.GetBuildingsForPerk(perkVO));
				i++;
			}
			return list;
		}

		public List<ActivatedPerkData> GetActiveNavCenterPerks()
		{
			Entity currentNavigationCenter = Service.Get<BuildingLookupController>().GetCurrentNavigationCenter();
			if (currentNavigationCenter == null)
			{
				return null;
			}
			BuildingTypeVO buildingType = currentNavigationCenter.Get<BuildingComponent>().BuildingType;
			if (buildingType == null)
			{
				return null;
			}
			List<ActivatedPerkData> perksAppliedToBuilding = this.GetPerksAppliedToBuilding(buildingType);
			if (perksAppliedToBuilding.Count == 0)
			{
				return null;
			}
			return perksAppliedToBuilding;
		}

		public bool IsPerkActive(ActivatedPerkData perk)
		{
			uint time = ServerTime.Time;
			return perk != null && perk.StartTime <= time && perk.EndTime > time;
		}

		public bool IsPerkActive(string perkId)
		{
			ActivatedPerkData playerPerk = this.GetPlayerPerk(perkId);
			return this.IsPerkActive(playerPerk);
		}

		public bool IsPerkGroupActive(string perkGroupId)
		{
			ActivatedPerkData playerPerkForGroup = this.GetPlayerPerkForGroup(perkGroupId);
			return this.IsPerkActive(playerPerkForGroup);
		}

		public bool IsPerkInCooldown(ActivatedPerkData playerPerk)
		{
			if (playerPerk == null)
			{
				return false;
			}
			IDataController dataController = Service.Get<IDataController>();
			PerkVO perkVO = dataController.Get<PerkVO>(playerPerk.PerkId);
			string perkGroup = perkVO.PerkGroup;
			Dictionary<string, uint> playerPerkGroupCooldowns = this.GetPlayerPerkGroupCooldowns();
			if (playerPerkGroupCooldowns == null || !playerPerkGroupCooldowns.ContainsKey(perkGroup))
			{
				return false;
			}
			uint endTime = playerPerk.EndTime;
			uint num = playerPerkGroupCooldowns[perkGroup];
			uint time = ServerTime.Time;
			return endTime <= time && num > time;
		}

		public bool IsPerkInCooldown(string perkId)
		{
			ActivatedPerkData playerPerk = this.GetPlayerPerk(perkId);
			return this.IsPerkInCooldown(playerPerk);
		}

		public bool IsPerkGroupInCooldown(string perkGroupId)
		{
			ActivatedPerkData playerPerkForGroup = this.GetPlayerPerkForGroup(perkGroupId);
			return this.IsPerkInCooldown(playerPerkForGroup);
		}

		public bool IsPerkExpired(ActivatedPerkData playerPerk)
		{
			return playerPerk != null && (!this.IsPerkActive(playerPerk) && !this.IsPerkInCooldown(playerPerk));
		}

		public bool IsPerkExpired(string perkId)
		{
			ActivatedPerkData playerPerk = this.GetPlayerPerk(perkId);
			return this.IsPerkExpired(playerPerk);
		}

		public bool IsPerkGroupExpired(string perkGroupId)
		{
			ActivatedPerkData playerPerkForGroup = this.GetPlayerPerkForGroup(perkGroupId);
			return this.IsPerkExpired(playerPerkForGroup);
		}

		public List<string> GetPlayerActivePerkEffectIds()
		{
			List<string> playerActivePerkIds = this.GetPlayerActivePerkIds();
			return this.GetPerkEffectIds(playerActivePerkIds);
		}

		public List<string> GetPerkEffectIds(List<string> perkIds)
		{
			List<string> list = new List<string>();
			if (perkIds != null)
			{
				IDataController dataController = Service.Get<IDataController>();
				int i = 0;
				int count = perkIds.Count;
				while (i < count)
				{
					PerkVO perkVO = dataController.Get<PerkVO>(perkIds[i]);
					list.AddRange(perkVO.PerkEffects);
					i++;
				}
			}
			return list;
		}

		public int GetPerkInvestedProgress(PerkVO perkData, Dictionary<string, int> inProgress)
		{
			int result = 0;
			string uid = perkData.Uid;
			if (inProgress.ContainsKey(uid))
			{
				result = inProgress[uid];
			}
			return result;
		}

		public bool IsPerkLevelLocked(PerkVO perkData, int squadLevel)
		{
			return squadLevel < perkData.SquadLevelUnlock;
		}

		public bool IsPerkReputationLocked(PerkVO perkData, int squadLevel, Dictionary<string, string> availablePerks)
		{
			return !this.IsPerkLevelLocked(perkData, squadLevel) && !availablePerks.ContainsKey(perkData.PerkGroup);
		}

		public bool IsPerkMaxTier(PerkVO perkToCheck)
		{
			IDataController dataController = Service.Get<IDataController>();
			string perkGroup = perkToCheck.PerkGroup;
			int perkTier = perkToCheck.PerkTier;
			foreach (PerkVO current in dataController.GetAll<PerkVO>())
			{
				if (perkGroup == current.PerkGroup && perkTier < current.PerkTier)
				{
					return false;
				}
			}
			return true;
		}

		public List<PerkVO> GetPerksUnlockedAtSquadLevel(int level)
		{
			List<PerkVO> list = new List<PerkVO>();
			IDataController dataController = Service.Get<IDataController>();
			foreach (PerkVO current in dataController.GetAll<PerkVO>())
			{
				if (current.SquadLevelUnlock == level)
				{
					list.Add(current);
				}
			}
			return list;
		}

		public int GetAvailableSlotsCount(string guildLevelUid)
		{
			IDataController dataController = Service.Get<IDataController>();
			SquadLevelVO squadLevelVO = dataController.Get<SquadLevelVO>(guildLevelUid);
			return squadLevelVO.Slots;
		}

		public bool HasEmptyPerkActivationSlot(string guildLevelUid)
		{
			int count = this.GetPlayerActivePerks().Count;
			int availableSlotsCount = this.GetAvailableSlotsCount(guildLevelUid);
			return count < availableSlotsCount;
		}

		public bool HasPrerequistesUnlocked(PerkVO perk, Dictionary<string, string> availablePerks)
		{
			IDataController dataController = Service.Get<IDataController>();
			bool result = false;
			string perkGroup = perk.PerkGroup;
			if (availablePerks.ContainsKey(perkGroup))
			{
				PerkVO perkVO = dataController.Get<PerkVO>(availablePerks[perkGroup]);
				result = (perk.PerkTier == perkVO.PerkTier + 1);
			}
			else if (perk.PerkTier == 1)
			{
				result = true;
			}
			return result;
		}

		private bool IsPerkValidForInvestment(PerkVO perk, Dictionary<string, string> availablePerks, string guildLevelUid)
		{
			bool flag = false;
			IDataController dataController = Service.Get<IDataController>();
			SquadLevelVO squadLevelVO = dataController.Get<SquadLevelVO>(guildLevelUid);
			if (squadLevelVO != null)
			{
				flag = (perk.SquadLevelUnlock <= squadLevelVO.Level && !availablePerks.ContainsValue(perk.Uid));
			}
			if (flag)
			{
				flag = this.HasPrerequistesUnlocked(perk, availablePerks);
			}
			return flag;
		}

		public void InvestInPerk(int repToInvest, string targetPerkId, Dictionary<string, string> availablePerks, string guildLevelUid)
		{
			IDataController dataController = Service.Get<IDataController>();
			PerkVO perkVO = dataController.Get<PerkVO>(targetPerkId);
			if (this.IsPerkValidForInvestment(perkVO, availablePerks, guildLevelUid))
			{
				GameUtils.SpendCurrency(0, 0, 0, repToInvest, 0, true);
				PlayerPerkInvestRequest request = new PlayerPerkInvestRequest(perkVO.Uid, repToInvest);
				PlayerPerkInvestCommand playerPerkInvestCommand = new PlayerPerkInvestCommand(request);
				playerPerkInvestCommand.AddFailureCallback(new AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>.OnFailureCallback(this.OnInvestFailure));
				playerPerkInvestCommand.Context = repToInvest;
				Service.Get<ServerAPI>().Sync(playerPerkInvestCommand);
			}
		}

		private void OnInvestFailure(uint status, object cookie)
		{
			if (!GameUtils.IsPerkCommandStatusFatal(status) && cookie != null)
			{
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				int delta = Convert.ToInt32(cookie);
				currentPlayer.Inventory.ModifyReputation(delta);
			}
		}

		public bool ActivatePlayerPerk(string targetPerkId, Dictionary<string, string> availablePerks, string guildLevelUid)
		{
			IDataController dataController = Service.Get<IDataController>();
			PerkVO perkVO = dataController.Get<PerkVO>(targetPerkId);
			string perkGroup = perkVO.PerkGroup;
			if (!this.HasEmptyPerkActivationSlot(guildLevelUid))
			{
				return false;
			}
			if (availablePerks == null || !availablePerks.ContainsValue(targetPerkId))
			{
				string message = "Perk not activated, not in available perks, perkId: " + targetPerkId;
				this.logger.ErrorFormat(message, new object[0]);
				return false;
			}
			List<ActivatedPerkData> playerActivatedPerks = this.GetPlayerActivatedPerks();
			Dictionary<string, uint> playerPerkGroupCooldowns = this.GetPlayerPerkGroupCooldowns();
			if (this.IsPerkActive(targetPerkId))
			{
				ActivatedPerkData playerPerk = this.GetPlayerPerk(targetPerkId);
				string message2 = string.Concat(new object[]
				{
					"Perk not activated, perk already active, perkId: ",
					targetPerkId,
					" startTime: ",
					playerPerk.StartTime,
					" endTime: ",
					playerPerk.EndTime
				});
				this.logger.ErrorFormat(message2, new object[0]);
				return false;
			}
			if (this.IsPerkInCooldown(targetPerkId))
			{
				ActivatedPerkData playerPerk2 = this.GetPlayerPerk(targetPerkId);
				string message3 = string.Concat(new object[]
				{
					"Perk not activated, perk in cooldown, perkId: ",
					targetPerkId,
					" startTime: ",
					playerPerk2.StartTime,
					" endTime: ",
					playerPerk2.EndTime,
					" cooldownEndTime: ",
					playerPerkGroupCooldowns[perkGroup]
				});
				this.logger.ErrorFormat(message3, new object[0]);
				return false;
			}
			this.CleanupExpiredPlayerPerks();
			if (this.HasPlayerActivatedFirstPerk())
			{
				GameUtils.SpendHQScaledCurrency(perkVO.ActivationCost, false);
			}
			uint time = ServerTime.Time;
			int num = perkVO.ActiveDurationMinutes * 60;
			uint num2 = time + (uint)num;
			int num3 = perkVO.CooldownDurationMinutes * 60;
			uint value = num2 + (uint)num3;
			playerActivatedPerks.Add(new ActivatedPerkData
			{
				PerkId = targetPerkId,
				StartTime = time,
				EndTime = num2
			});
			playerPerkGroupCooldowns[perkGroup] = value;
			Service.Get<EventManager>().SendEvent(EventId.ActivePerksUpdated, null);
			PlayerPerkActivateRequest request = new PlayerPerkActivateRequest(targetPerkId);
			PlayerPerkActivateCommand command = new PlayerPerkActivateCommand(request);
			Service.Get<ServerAPI>().Sync(command);
			return true;
		}

		public bool CancelPlayerPerk(string perkId)
		{
			ActivatedPerkData playerPerk = this.GetPlayerPerk(perkId);
			Dictionary<string, uint> playerPerkGroupCooldowns = this.GetPlayerPerkGroupCooldowns();
			IDataController dataController = Service.Get<IDataController>();
			PerkVO perkVO = dataController.Get<PerkVO>(perkId);
			string perkGroup = perkVO.PerkGroup;
			if (playerPerk == null)
			{
				this.logger.ErrorFormat("Cannot cancel non existant perk, perkId : {0}", new object[]
				{
					perkId
				});
				return false;
			}
			if (!this.IsPerkActive(playerPerk))
			{
				this.logger.ErrorFormat("Cannot cancel non active perk, perkId : {0}", new object[]
				{
					perkId
				});
				return false;
			}
			if (playerPerkGroupCooldowns == null || !playerPerkGroupCooldowns.ContainsKey(perkGroup))
			{
				this.logger.ErrorFormat("Perk Cooldown group doesn't exist for perkId: {0}, PerkGroup: {1}", new object[]
				{
					perkId,
					perkGroup
				});
				return false;
			}
			uint time = ServerTime.Time;
			playerPerk.EndTime = time;
			int num = perkVO.CooldownDurationMinutes * 60;
			uint value = time + (uint)num;
			playerPerkGroupCooldowns[perkGroup] = value;
			Service.Get<EventManager>().SendEvent(EventId.ActivePerksUpdated, null);
			PlayerPerkCancelRequest request = new PlayerPerkCancelRequest(perkId);
			PlayerPerkCancelCommand command = new PlayerPerkCancelCommand(request);
			Service.Get<ServerAPI>().Sync(command);
			return true;
		}

		public void ConfirmPurchaseCooldownSkip(string perkId, string perkTitle, string cooldownTime, string crystalCost)
		{
			string message = Service.Get<Lang>().Get("PERK_COOLDOWN_POPUP_DESC", new object[]
			{
				perkTitle,
				cooldownTime,
				crystalCost
			});
			FinishNowScreen.ShowModalPerk(perkId, new OnScreenModalResult(this.PurchaseCooldownSkipConfirmed), perkId, int.Parse(crystalCost), perkTitle, message, true);
		}

		private void PurchaseCooldownSkipConfirmed(object result, object cookie)
		{
			if (result != null)
			{
				string perkId = (string)cookie;
				this.PurchaseCooldownSkip(perkId);
			}
		}

		public void PurchaseCooldownSkip(string perkId)
		{
			IDataController dataController = Service.Get<IDataController>();
			string perkGroup = dataController.Get<PerkVO>(perkId).PerkGroup;
			ActivatedPerkData playerPerkForGroup = this.GetPlayerPerkForGroup(perkGroup);
			if (this.IsPerkInCooldown(playerPerkForGroup))
			{
				Dictionary<string, uint> playerPerkGroupCooldowns = this.GetPlayerPerkGroupCooldowns();
				uint seconds = playerPerkGroupCooldowns[perkGroup] - ServerTime.Time;
				int crystals = GameUtils.SecondsToCrystalsForPerk((int)seconds);
				if (!GameUtils.SpendCrystals(crystals))
				{
					return;
				}
				ProcessingScreen.Show();
				PlayerPerkSkipCooldownRequest request = new PlayerPerkSkipCooldownRequest(playerPerkForGroup.PerkId);
				PlayerPerkSkipCooldownCommand command = new PlayerPerkSkipCooldownCommand(request);
				Service.Get<ServerAPI>().Sync(command);
			}
			else
			{
				Service.Get<Logger>().WarnFormat("Perk {0} is no longer in cooldown, skipping purchase", new object[]
				{
					playerPerkForGroup.PerkId
				});
			}
		}

		public void PurchaseCooldownSkipResponse(object response)
		{
			this.UpdatePlayerPerksData(response);
			ProcessingScreen.Hide();
		}

		private void CleanupExpiredPlayerPerks()
		{
			IDataController dataController = Service.Get<IDataController>();
			List<ActivatedPerkData> playerActivatedPerks = this.GetPlayerActivatedPerks();
			Dictionary<string, uint> playerPerkGroupCooldowns = this.GetPlayerPerkGroupCooldowns();
			ResourceGenerationPerkUtils.ProcessResouceGenPerkEffectsIntoStorage(this.GetPlayerActivatedPerks());
			List<ActivatedPerkData> list = new List<ActivatedPerkData>();
			int i = 0;
			int count = playerActivatedPerks.Count;
			while (i < count)
			{
				ActivatedPerkData activatedPerkData = playerActivatedPerks[i];
				if (this.IsPerkExpired(activatedPerkData.PerkId))
				{
					PerkVO perkVO = dataController.Get<PerkVO>(activatedPerkData.PerkId);
					string perkGroup = perkVO.PerkGroup;
					if (playerPerkGroupCooldowns.ContainsKey(perkGroup))
					{
						playerPerkGroupCooldowns.Remove(perkGroup);
					}
					list.Add(playerActivatedPerks[i]);
				}
				i++;
			}
			int j = 0;
			int count2 = list.Count;
			while (j < count2)
			{
				playerActivatedPerks.Remove(list[j]);
				j++;
			}
		}

		public int GetAccruedCurrencyIncludingPerkAdjustedRate(BuildingTypeVO buildingVO, uint startTime, uint endTime)
		{
			return ResourceGenerationPerkUtils.GetPerkAdjustedAccruedCurrency(buildingVO, startTime, endTime, this.GetPlayerActivatedPerks());
		}

		public int GetSecondsTillFullIncludingPerkAdjustedRate(BuildingTypeVO buildingVO, float remainingAmountTillFull, uint currentTime)
		{
			return ResourceGenerationPerkUtils.GetPerkAdjustSecondsTillFull(buildingVO, remainingAmountTillFull, currentTime, this.GetPlayerActivatedPerks());
		}

		public float GetContractCostMultiplier(BuildingTypeVO buildingVO)
		{
			List<string> playerActivePerkEffectIds = this.GetPlayerActivePerkEffectIds();
			return ContractCostPerkUtils.GetDiscountedCostMultiplier(buildingVO, playerActivePerkEffectIds);
		}

		public float GetContractCostMultiplierForPerks(BuildingTypeVO buildingVO, List<string> perkIds)
		{
			List<string> perkEffectIds = this.GetPerkEffectIds(perkIds);
			return ContractCostPerkUtils.GetDiscountedCostMultiplier(buildingVO, perkEffectIds);
		}

		public bool IsContractCostMultiplierAppliedToBuilding(BuildingTypeVO buildingVO)
		{
			List<string> playerActivePerkEffectIds = this.GetPlayerActivePerkEffectIds();
			if (playerActivePerkEffectIds != null)
			{
				IDataController dataController = Service.Get<IDataController>();
				int i = 0;
				int count = playerActivePerkEffectIds.Count;
				while (i < count)
				{
					PerkEffectVO perkEffectVO = dataController.Get<PerkEffectVO>(playerActivePerkEffectIds[i]);
					if (ContractCostPerkUtils.CanApplyEffect(perkEffectVO, buildingVO))
					{
						return true;
					}
					i++;
				}
			}
			return false;
		}

		public float GetContractTimeReductionMultiplier(IUpgradeableVO deployableVO)
		{
			List<string> playerActivePerkIds = this.GetPlayerActivePerkIds();
			return this.GetContractTimeReductionMultiplierForPerks(deployableVO, playerActivePerkIds);
		}

		public float GetContractTimeReductionMultiplierForPerks(IUpgradeableVO deployableVO, List<string> perkIds)
		{
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			BuildingTypeVO minBuildingRequirement = buildingLookupController.GetMinBuildingRequirement(deployableVO);
			List<string> perkEffectIds = this.GetPerkEffectIds(perkIds);
			return ContractTimePerkUtils.GetTimeReductionMultiplier(minBuildingRequirement, perkEffectIds);
		}

		public int GetTroopRequestCooldownTime()
		{
			List<string> playerActivePerkEffectIds = this.GetPlayerActivePerkEffectIds();
			return TroopRequestPerkUtils.GetTroopRequestPerkTimeReduction(playerActivePerkEffectIds);
		}

		public int GetRelocationCostDiscount()
		{
			List<string> playerActivePerkIds = this.GetPlayerActivePerkIds();
			return this.GetRelocationCostDiscountForPerks(playerActivePerkIds);
		}

		public int GetRelocationCostDiscountForPerks(List<string> perkIds)
		{
			List<string> perkEffectIds = this.GetPerkEffectIds(perkIds);
			return RelocationCostPerkUtils.GetRelocationCostDiscount(perkEffectIds);
		}

		public string GetPurchaseContextForActivationCost(PerkVO perkVO)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return string.Format("{0}|{1}|{2}||{3}", new object[]
			{
				"perk",
				perkVO.Uid,
				perkVO.PerkTier,
				currentPlayer.Planet.PlanetBIName
			});
		}
	}
}
