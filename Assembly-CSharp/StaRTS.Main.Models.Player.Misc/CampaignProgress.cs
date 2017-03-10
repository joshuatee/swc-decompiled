using StaRTS.Externals.EnvironmentManager;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.VictoryConditions;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Commands.Missions;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public class CampaignProgress : ISerializable
	{
		private Dictionary<string, Campaign> campaigns;

		private Dictionary<string, Mission> missions;

		private float[] starsToPortion;

		private EnvironmentController env;

		public bool FueInProgress
		{
			get;
			set;
		}

		public IDictionary<string, Mission> Missions
		{
			get
			{
				return this.missions;
			}
		}

		public IDictionary<string, Campaign> Campaigns
		{
			get
			{
				return this.campaigns;
			}
		}

		public CampaignProgress()
		{
			this.campaigns = new Dictionary<string, Campaign>();
			this.missions = new Dictionary<string, Mission>();
			this.FueInProgress = true;
			this.env = Service.Get<EnvironmentController>();
			this.starsToPortion = new float[]
			{
				0f,
				0.3f,
				0.6f,
				1f
			};
		}

		public void CheckForNewMissions(ref bool newChapterMission)
		{
			IDataController dataController = Service.Get<IDataController>();
			foreach (string current in this.missions.Keys)
			{
				Mission mission = this.missions[current];
				if (mission.Status == MissionStatus.Default)
				{
					CampaignMissionVO optional = dataController.GetOptional<CampaignMissionVO>(mission.Uid);
					if (optional != null)
					{
						CampaignVO optional2 = dataController.GetOptional<CampaignVO>(optional.CampaignUid);
						if (optional2 != null)
						{
							if (!optional2.Timed)
							{
								newChapterMission = true;
							}
							if (newChapterMission)
							{
								break;
							}
						}
					}
				}
			}
		}

		public bool HasCampaign(CampaignVO campaignType)
		{
			return this.campaigns.ContainsKey(campaignType.Uid);
		}

		public int GetOffsetSeconds(ITimedEventVO vo)
		{
			if (!vo.UseTimeZoneOffset)
			{
				return 0;
			}
			if (this.campaigns.ContainsKey(vo.Uid))
			{
				return (int)(this.campaigns[vo.Uid].TimeZone * 3600f);
			}
			return this.env.GetTimezoneOffsetSeconds();
		}

		public bool IsNewSpecOp(CampaignVO vo)
		{
			if (!this.HasCampaign(vo))
			{
				return true;
			}
			IDataController dataController = Service.Get<IDataController>();
			foreach (Mission current in this.missions.Values)
			{
				CampaignMissionVO optional = dataController.GetOptional<CampaignMissionVO>(current.Uid);
				if (current.CampaignUid == vo.Uid && optional != null && optional.UnlockOrder == 1)
				{
					return false;
				}
			}
			return true;
		}

		public bool IsGrindComplete(CampaignMissionVO vo)
		{
			if (!this.missions.ContainsKey(vo.Uid))
			{
				return false;
			}
			Mission mission = this.missions[vo.Uid];
			return mission.GrindMissionRetries >= GameConstants.GRIND_MISSION_MAXIMUM;
		}

		public int GetRetriesLeft(CampaignMissionVO vo)
		{
			if (!this.missions.ContainsKey(vo.Uid))
			{
				return GameConstants.GRIND_MISSION_MAXIMUM;
			}
			Mission mission = this.missions[vo.Uid];
			return GameConstants.GRIND_MISSION_MAXIMUM - mission.GrindMissionRetries;
		}

		public bool HasSeenIntro(string campaignUid)
		{
			return Service.Get<CurrentPlayer>().SpecOpIntros.Contains(campaignUid);
		}

		public bool IsCampaignCollected(string campaignUid)
		{
			return this.campaigns.ContainsKey(campaignUid) && this.campaigns[campaignUid].Collected;
		}

		public bool CanReplay(CampaignMissionVO missionType)
		{
			return missionType.Replayable || missionType.Grind || this.GetMissionEarnedStars(missionType) < missionType.MasteryStars;
		}

		public int GetTotalCampaignStarsEarned(CampaignVO campaignType)
		{
			if (!this.campaigns.ContainsKey(campaignType.Uid))
			{
				return 0;
			}
			int num = 0;
			foreach (Mission current in this.missions.Values)
			{
				if (current.CampaignUid == campaignType.Uid)
				{
					num += current.EarnedStars;
				}
			}
			return num;
		}

		public int GetTotalAttackDefendCampaignStarsEarned(CampaignVO campaignType)
		{
			if (!this.campaigns.ContainsKey(campaignType.Uid))
			{
				return 0;
			}
			int num = 0;
			foreach (Mission current in this.missions.Values)
			{
				if (current.CampaignUid == campaignType.Uid)
				{
					CampaignMissionVO campaignMissionVO = Service.Get<IDataController>().Get<CampaignMissionVO>(current.Uid);
					if (campaignMissionVO.MissionType == MissionType.Attack || campaignMissionVO.MissionType == MissionType.Defend)
					{
						num += current.EarnedStars;
					}
				}
			}
			return num;
		}

		public int GetTotalCampaignStarsEarnedInAllCampaigns()
		{
			int num = 0;
			Dictionary<string, CampaignVO>.ValueCollection all = Service.Get<IDataController>().GetAll<CampaignVO>();
			foreach (CampaignVO current in all)
			{
				if (current.Faction != FactionType.Smuggler)
				{
					num += this.GetTotalAttackDefendCampaignStarsEarned(current);
				}
			}
			return num;
		}

		public List<CampaignMissionVO> GetAllMissionsInProgress()
		{
			IDataController dataController = Service.Get<IDataController>();
			List<CampaignMissionVO> list = new List<CampaignMissionVO>();
			foreach (Mission current in this.missions.Values)
			{
				CampaignMissionVO optional = dataController.GetOptional<CampaignMissionVO>(current.Uid);
				if (optional != null && this.IsMissionInProgress(optional) && !optional.IsCombatMission())
				{
					list.Add(optional);
				}
			}
			return list;
		}

		public int GetTotalCampaignMissionsCompleted(CampaignVO campaignType)
		{
			if (!this.campaigns.ContainsKey(campaignType.Uid))
			{
				return 0;
			}
			int num = 0;
			foreach (Mission current in this.missions.Values)
			{
				if (current.CampaignUid == campaignType.Uid && current.Completed)
				{
					num++;
				}
			}
			return num;
		}

		public void AddMission(string uid, Mission mission)
		{
			this.missions.Add(uid, mission);
		}

		public void StartMission(CampaignMissionVO vo)
		{
			if (!string.IsNullOrEmpty(vo.Uid) && this.missions.ContainsKey(vo.Uid))
			{
				Mission mission = this.missions[vo.Uid];
				if (vo.Grind)
				{
					mission.GrindMissionRetries++;
				}
				if (!mission.Activated)
				{
					MissionIdRequest request = new MissionIdRequest(vo.Uid);
					Service.Get<ServerAPI>().Sync(new ActivateMissionCommand(request));
					mission.Activated = true;
				}
			}
		}

		public bool CompleteMission(CampaignMissionVO vo, int starsEarned)
		{
			if (vo.Uid != null && this.missions.ContainsKey(vo.Uid))
			{
				Mission mission = this.missions[vo.Uid];
				int earnedStars = mission.EarnedStars;
				if (starsEarned > earnedStars)
				{
					float[] array = this.starsToPortion;
					if (vo.StarsToPortion != null)
					{
						array = vo.StarsToPortion;
					}
					if (!vo.Grind)
					{
						mission.EarnedStars = starsEarned;
					}
					int campaignPoints = vo.CampaignPoints;
					if (campaignPoints > 0)
					{
						int num = (int)(array[earnedStars] * (float)campaignPoints);
						int num2 = (int)(array[starsEarned] * (float)campaignPoints);
						uint num3 = (uint)(num2 - num);
						if (this.campaigns.ContainsKey(vo.CampaignUid))
						{
							this.campaigns[vo.CampaignUid].Points += num3;
						}
						CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
						currentBattle.CampaignPointsEarn = num3;
					}
					Service.Get<AchievementController>().TryUnlockAchievementByValue(AchievementType.PveStars, this.GetTotalCampaignStarsEarnedInAllCampaigns());
				}
				if (!mission.Completed)
				{
					mission.Completed = true;
					return true;
				}
			}
			return false;
		}

		public int RemainingCampaignPointsForMission(CampaignMissionVO vo)
		{
			if (this.missions.ContainsKey(vo.Uid))
			{
				float[] array = this.starsToPortion;
				if (vo.StarsToPortion != null)
				{
					array = vo.StarsToPortion;
				}
				int earnedStars = this.missions[vo.Uid].EarnedStars;
				int campaignPoints = vo.CampaignPoints;
				if (campaignPoints > 0)
				{
					int num = campaignPoints - (int)(array[earnedStars] * (float)campaignPoints);
					if (num > 0)
					{
						return num;
					}
					return 0;
				}
			}
			return vo.CampaignPoints;
		}

		public bool CollectMission(string uid)
		{
			if (this.missions.ContainsKey(uid))
			{
				Mission mission = this.missions[uid];
				if (!mission.Collected)
				{
					mission.Collected = true;
					return true;
				}
			}
			return false;
		}

		public bool UpdateMissionLoot(string uid, CurrentBattle battle)
		{
			if (this.missions.ContainsKey(uid))
			{
				this.missions[uid].SetLootRemaining(battle.LootCreditsAvailable - battle.LootCreditsEarned, battle.LootMaterialsAvailable - battle.LootMaterialsEarned, battle.LootContrabandAvailable - battle.LootContrabandEarned);
				return true;
			}
			return false;
		}

		public void AddCampaign(string uid, Campaign campaign)
		{
			if (!this.campaigns.ContainsKey(uid))
			{
				this.campaigns.Add(uid, campaign);
			}
		}

		public bool CompleteCampaign(string uid)
		{
			if (!string.IsNullOrEmpty(uid) && this.campaigns.ContainsKey(uid) && !this.campaigns[uid].Completed)
			{
				this.campaigns[uid].Completed = true;
				return true;
			}
			return false;
		}

		public bool CollectCampaign(string uid)
		{
			if (this.campaigns.ContainsKey(uid) && !this.campaigns[uid].Collected)
			{
				this.campaigns[uid].Collected = true;
				return true;
			}
			return false;
		}

		public int GetMissionLootCreditsRemaining(CampaignMissionVO missionType)
		{
			return this.GetMissionLootCurrencyRemaining(CurrencyType.Credits, missionType);
		}

		public int GetMissionLootMaterialsRemaining(CampaignMissionVO missionType)
		{
			return this.GetMissionLootCurrencyRemaining(CurrencyType.Materials, missionType);
		}

		public int GetMissionLootContrabandRemaining(CampaignMissionVO missionType)
		{
			return this.GetMissionLootCurrencyRemaining(CurrencyType.Contraband, missionType);
		}

		private int GetMissionLootCurrencyRemaining(CurrencyType type, CampaignMissionVO missionType)
		{
			if (this.missions.ContainsKey(missionType.Uid))
			{
				Mission mission = this.missions[missionType.Uid];
				if (mission.LootRemaining != null && mission.LootRemaining[(int)type] >= 0)
				{
					return mission.LootRemaining[(int)type];
				}
				if (missionType.TotalLoot != null && missionType.TotalLoot[(int)type] >= 0)
				{
					return missionType.TotalLoot[(int)type];
				}
			}
			return 0;
		}

		public int GetMissionEarnedStars(CampaignMissionVO missionType)
		{
			if (!this.missions.ContainsKey(missionType.Uid))
			{
				return 0;
			}
			return this.missions[missionType.Uid].EarnedStars;
		}

		public Dictionary<string, int> GetMissionCounters(CampaignMissionVO missionType)
		{
			if (!this.missions.ContainsKey(missionType.Uid))
			{
				return null;
			}
			return this.missions[missionType.Uid].Counters;
		}

		public bool UpdateMissionCounter(CampaignMissionVO missionType, string key, int delta)
		{
			if (!this.missions.ContainsKey(missionType.Uid))
			{
				return false;
			}
			this.missions[missionType.Uid].AddToCounter(key, delta);
			return true;
		}

		public void GetMissionProgress(CampaignMissionVO mission, out int current, out int total)
		{
			if (!this.missions.ContainsKey(mission.Uid))
			{
				current = 0;
				total = 1;
			}
			current = 0;
			total = 0;
			for (int i = 0; i < mission.Conditions.Count; i++)
			{
				Dictionary<string, int> counters = this.missions[mission.Uid].Counters;
				ConditionVO conditionVO = mission.Conditions[i];
				int startingValue = (counters != null && counters.ContainsKey(conditionVO.Uid)) ? counters[conditionVO.Uid] : 0;
				AbstractCondition abstractCondition = ConditionFactory.GenerateCondition(conditionVO, null, startingValue);
				int num;
				int num2;
				abstractCondition.GetProgress(out num, out num2);
				current += num;
				total += num2;
				abstractCondition.Destroy();
			}
		}

		public bool IsMissionLocked(CampaignMissionVO missionType)
		{
			return !this.missions.ContainsKey(missionType.Uid) || this.missions[missionType.Uid].Locked;
		}

		public bool IsMissionCompleted(CampaignMissionVO missionType)
		{
			return this.missions.ContainsKey(missionType.Uid) && this.missions[missionType.Uid].Completed;
		}

		public bool IsMissionCollected(CampaignMissionVO missionType)
		{
			return this.missions.ContainsKey(missionType.Uid) && this.missions[missionType.Uid].Collected;
		}

		public bool IsMissionInProgress(CampaignMissionVO missionType)
		{
			if (missionType.IsCombatMission())
			{
				return false;
			}
			if (this.missions.ContainsKey(missionType.Uid))
			{
				Mission mission = this.missions[missionType.Uid];
				if (!mission.Completed && !mission.Collected)
				{
					return mission.Activated;
				}
			}
			return false;
		}

		public Campaign GetTimedEvent(string eventUid)
		{
			if (this.campaigns.ContainsKey(eventUid))
			{
				return this.campaigns[eventUid];
			}
			return null;
		}

		public string ToJson()
		{
			return "{}";
		}

		public void RemoveMissingMissionData()
		{
			IDataController dataController = Service.Get<IDataController>();
			List<string> list = new List<string>();
			foreach (string current in this.campaigns.Keys)
			{
				if (dataController.GetOptional<CampaignVO>(current) == null)
				{
					list.Add(current);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.campaigns.Remove(list[i]);
			}
			list.Clear();
			foreach (string current2 in this.missions.Keys)
			{
				if (dataController.GetOptional<CampaignMissionVO>(current2) == null)
				{
					list.Add(current2);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				this.missions.Remove(list[j]);
			}
			list.Clear();
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("campaigns"))
			{
				Dictionary<string, object> dictionary2 = dictionary["campaigns"] as Dictionary<string, object>;
				foreach (KeyValuePair<string, object> current in dictionary2)
				{
					Campaign campaign = new Campaign();
					campaign.FromObject(current.get_Value());
					this.campaigns.Add(current.get_Key(), campaign);
				}
			}
			if (dictionary.ContainsKey("missions"))
			{
				Dictionary<string, object> dictionary3 = dictionary["missions"] as Dictionary<string, object>;
				foreach (KeyValuePair<string, object> current2 in dictionary3)
				{
					Mission mission = new Mission();
					mission.FromObject(current2.get_Value());
					this.missions.Add(current2.get_Key(), mission);
				}
			}
			if (dictionary.ContainsKey("isFueInProgress"))
			{
				this.FueInProgress = (bool)dictionary["isFueInProgress"];
			}
			return this;
		}

		public void EraseMission(CampaignMissionVO missionType)
		{
			if (this.missions.ContainsKey(missionType.Uid))
			{
				this.missions.Remove(missionType.Uid);
			}
		}

		protected internal CampaignProgress(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).AddCampaign(Marshal.PtrToStringUni(*(IntPtr*)args), (Campaign)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).AddMission(Marshal.PtrToStringUni(*(IntPtr*)args), (Mission)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).CanReplay((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).CollectCampaign(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).CollectMission(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).CompleteCampaign(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).CompleteMission((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).EraseMission((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).Campaigns);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).FueInProgress);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).Missions);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetAllMissionsInProgress());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetMissionCounters((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetMissionEarnedStars((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetMissionLootContrabandRemaining((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetMissionLootCreditsRemaining((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetMissionLootCurrencyRemaining((CurrencyType)(*(int*)args), (CampaignMissionVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetMissionLootMaterialsRemaining((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetOffsetSeconds((ITimedEventVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetRetriesLeft((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetTimedEvent(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetTotalAttackDefendCampaignStarsEarned((CampaignVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetTotalCampaignMissionsCompleted((CampaignVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetTotalCampaignStarsEarned((CampaignVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).GetTotalCampaignStarsEarnedInAllCampaigns());
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).HasCampaign((CampaignVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).HasSeenIntro(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).IsCampaignCollected(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).IsGrindComplete((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).IsMissionCollected((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).IsMissionCompleted((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).IsMissionInProgress((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).IsMissionLocked((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).IsNewSpecOp((CampaignVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).RemainingCampaignPointsForMission((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).RemoveMissingMissionData();
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).FueInProgress = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).StartMission((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).UpdateMissionCounter((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignProgress)GCHandledObjects.GCHandleToObject(instance)).UpdateMissionLoot(Marshal.PtrToStringUni(*(IntPtr*)args), (CurrentBattle)GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
