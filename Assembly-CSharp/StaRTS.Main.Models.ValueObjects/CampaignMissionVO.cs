using StaRTS.Main.Controllers;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CampaignMissionVO : IValueObject
	{
		public static int COLUMN_missionType
		{
			get;
			private set;
		}

		public static int COLUMN_waves
		{
			get;
			private set;
		}

		public static int COLUMN_map
		{
			get;
			private set;
		}

		public static int COLUMN_campaignUid
		{
			get;
			private set;
		}

		public static int COLUMN_title
		{
			get;
			private set;
		}

		public static int COLUMN_unlockOrder
		{
			get;
			private set;
		}

		public static int COLUMN_description
		{
			get;
			private set;
		}

		public static int COLUMN_rewards
		{
			get;
			private set;
		}

		public static int COLUMN_introStory
		{
			get;
			private set;
		}

		public static int COLUMN_winStory
		{
			get;
			private set;
		}

		public static int COLUMN_loseStory
		{
			get;
			private set;
		}

		public static int COLUMN_goalFailStory
		{
			get;
			private set;
		}

		public static int COLUMN_opponentName
		{
			get;
			private set;
		}

		public static int COLUMN_goalString
		{
			get;
			private set;
		}

		public static int COLUMN_goalFailString
		{
			get;
			private set;
		}

		public static int COLUMN_progressString
		{
			get;
			private set;
		}

		public static int COLUMN_replay
		{
			get;
			private set;
		}

		public static int COLUMN_grind
		{
			get;
			private set;
		}

		public static int COLUMN_battleMusic
		{
			get;
			private set;
		}

		public static int COLUMN_ambientMusic
		{
			get;
			private set;
		}

		public static int COLUMN_campaignPoints
		{
			get;
			private set;
		}

		public static int COLUMN_fixedWaves
		{
			get;
			private set;
		}

		public static int COLUMN_totalLoot
		{
			get;
			private set;
		}

		public static int COLUMN_victoryConditions
		{
			get;
			private set;
		}

		public static int COLUMN_failureCondition
		{
			get;
			private set;
		}

		public static int COLUMN_bi_chap_id
		{
			get;
			private set;
		}

		public static int COLUMN_bi_context
		{
			get;
			private set;
		}

		public static int COLUMN_bi_enemy_tier
		{
			get;
			private set;
		}

		public static int COLUMN_bi_mission_id
		{
			get;
			private set;
		}

		public static int COLUMN_bi_mission_name
		{
			get;
			private set;
		}

		public static int COLUMN_raidDesc
		{
			get;
			private set;
		}

		public static int COLUMN_raidImage
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string CampaignUid
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public int UnlockOrder
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Rewards
		{
			get;
			set;
		}

		public int MasteryStars
		{
			get;
			set;
		}

		public int[] TotalLoot
		{
			get;
			set;
		}

		public MissionType MissionType
		{
			get;
			set;
		}

		public string Waves
		{
			get;
			set;
		}

		public string Map
		{
			get;
			set;
		}

		public List<ConditionVO> Conditions
		{
			get;
			set;
		}

		public string FailureCondition
		{
			get;
			set;
		}

		public string IntroStory
		{
			get;
			set;
		}

		public string SuccessStory
		{
			get;
			set;
		}

		public string FailureStory
		{
			get;
			set;
		}

		public string GoalFailureStory
		{
			get;
			set;
		}

		public string OpponentName
		{
			get;
			set;
		}

		public string GoalString
		{
			get;
			set;
		}

		public string GoalFailureString
		{
			get;
			set;
		}

		public string ProgressString
		{
			get;
			set;
		}

		public bool Replayable
		{
			get;
			set;
		}

		public bool Grind
		{
			get;
			set;
		}

		public string BattleMusic
		{
			get;
			set;
		}

		public string AmbientMusic
		{
			get;
			set;
		}

		public int CampaignPoints
		{
			get;
			set;
		}

		public float[] StarsToPortion
		{
			get;
			set;
		}

		public bool FixedWaves
		{
			get;
			set;
		}

		public string RaidDescriptionID
		{
			get;
			private set;
		}

		public string RaidBriefingBGTextureName
		{
			get;
			private set;
		}

		public string BIContext
		{
			get;
			set;
		}

		public string BIChapterId
		{
			get;
			set;
		}

		public string BIMissionId
		{
			get;
			set;
		}

		public string BIMissionName
		{
			get;
			set;
		}

		public int BIEnemyTier
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.MissionType = StringUtils.ParseEnum<MissionType>(row.TryGetString(CampaignMissionVO.COLUMN_missionType));
			this.Waves = row.TryGetString(CampaignMissionVO.COLUMN_waves);
			this.Map = row.TryGetString(CampaignMissionVO.COLUMN_map);
			this.CampaignUid = row.TryGetString(CampaignMissionVO.COLUMN_campaignUid);
			this.Title = row.TryGetString(CampaignMissionVO.COLUMN_title);
			this.UnlockOrder = row.TryGetInt(CampaignMissionVO.COLUMN_unlockOrder);
			this.Description = row.TryGetString(CampaignMissionVO.COLUMN_description);
			this.Rewards = row.TryGetString(CampaignMissionVO.COLUMN_rewards);
			this.MasteryStars = 3;
			this.IntroStory = row.TryGetString(CampaignMissionVO.COLUMN_introStory);
			this.SuccessStory = row.TryGetString(CampaignMissionVO.COLUMN_winStory);
			this.FailureStory = row.TryGetString(CampaignMissionVO.COLUMN_loseStory);
			this.GoalFailureStory = row.TryGetString(CampaignMissionVO.COLUMN_goalFailStory);
			this.OpponentName = row.TryGetString(CampaignMissionVO.COLUMN_opponentName);
			this.GoalString = row.TryGetString(CampaignMissionVO.COLUMN_goalString);
			this.GoalFailureString = row.TryGetString(CampaignMissionVO.COLUMN_goalFailString);
			this.ProgressString = row.TryGetString(CampaignMissionVO.COLUMN_progressString);
			this.Replayable = row.TryGetBool(CampaignMissionVO.COLUMN_replay);
			this.Grind = row.TryGetBool(CampaignMissionVO.COLUMN_grind);
			this.BattleMusic = row.TryGetString(CampaignMissionVO.COLUMN_battleMusic);
			this.AmbientMusic = row.TryGetString(CampaignMissionVO.COLUMN_ambientMusic);
			this.CampaignPoints = this.ParseCampaignPoints(row.TryGetString(CampaignMissionVO.COLUMN_campaignPoints));
			this.FixedWaves = row.TryGetBool(CampaignMissionVO.COLUMN_fixedWaves);
			this.RaidDescriptionID = row.TryGetString(CampaignMissionVO.COLUMN_raidDesc);
			this.RaidBriefingBGTextureName = row.TryGetString(CampaignMissionVO.COLUMN_raidImage);
			this.TotalLoot = null;
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			List<StrIntPair> strIntPairs = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(CampaignMissionVO.COLUMN_totalLoot));
			if (strIntPairs != null)
			{
				int num = 6;
				this.TotalLoot = new int[num];
				for (int i = 0; i < num; i++)
				{
					this.TotalLoot[i] = -1;
				}
				int j = 0;
				int count = strIntPairs.Count;
				while (j < count)
				{
					StrIntPair strIntPair = strIntPairs[j];
					this.TotalLoot[(int)StringUtils.ParseEnum<CurrencyType>(strIntPair.StrKey)] = strIntPair.IntVal;
					j++;
				}
			}
			IDataController dataController = Service.Get<IDataController>();
			this.Conditions = new List<ConditionVO>();
			string[] array = row.TryGetStringArray(CampaignMissionVO.COLUMN_victoryConditions);
			for (int k = 0; k < array.Length; k++)
			{
				this.Conditions.Add(dataController.Get<ConditionVO>(array[k]));
			}
			if (!string.IsNullOrEmpty(this.CampaignUid))
			{
				CampaignVO optional = dataController.GetOptional<CampaignVO>(this.CampaignUid);
				if (optional != null)
				{
					CampaignVO expr_2AB = optional;
					int totalMissions = expr_2AB.TotalMissions;
					expr_2AB.TotalMissions = totalMissions + 1;
					optional.TotalMasteryStars += this.MasteryStars;
				}
				else
				{
					Service.Get<StaRTSLogger>().ErrorFormat("CampaignMissionVO {0} that references a CampaignVO Uid {1} that doesn't exist", new object[]
					{
						this.Uid,
						this.CampaignUid
					});
				}
			}
			this.FailureCondition = row.TryGetString(CampaignMissionVO.COLUMN_failureCondition);
			this.BIChapterId = row.TryGetString(CampaignMissionVO.COLUMN_bi_chap_id);
			this.BIContext = row.TryGetString(CampaignMissionVO.COLUMN_bi_context);
			this.BIEnemyTier = row.TryGetInt(CampaignMissionVO.COLUMN_bi_enemy_tier);
			this.BIMissionId = row.TryGetString(CampaignMissionVO.COLUMN_bi_mission_id);
			this.BIMissionName = row.TryGetString(CampaignMissionVO.COLUMN_bi_mission_name);
		}

		public bool IsRaidDefense()
		{
			return this.MissionType == MissionType.RaidDefend;
		}

		public bool IsCombatMission()
		{
			return this.MissionType == MissionType.Attack || this.MissionType == MissionType.Defend || this.MissionType == MissionType.RaidDefend;
		}

		public bool HasPvpCondition()
		{
			int i = 0;
			int count = this.Conditions.Count;
			while (i < count)
			{
				if (this.Conditions[i].IsPvpType())
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public bool IsChallengeMission()
		{
			if (this.Map != null && !this.Grind && this.MissionType != MissionType.RaidDefend)
			{
				BattleTypeVO battleTypeVO = Service.Get<IDataController>().Get<BattleTypeVO>(this.Map);
				return battleTypeVO.OverridePlayerUnits;
			}
			return false;
		}

		private int ParseCampaignPoints(string raw)
		{
			if (string.IsNullOrEmpty(raw))
			{
				return 0;
			}
			if (!raw.Contains("|"))
			{
				return Convert.ToInt32(raw, CultureInfo.InvariantCulture);
			}
			string[] array = raw.Split(new char[]
			{
				'|'
			});
			string[] array2 = array[1].Split(new char[]
			{
				','
			});
			this.StarsToPortion = new float[4];
			this.StarsToPortion[0] = 0f;
			float num = 0f;
			int i = 0;
			int num2 = array2.Length;
			while (i < num2)
			{
				num += Convert.ToSingle(array2[i], CultureInfo.InvariantCulture) / 100f;
				this.StarsToPortion[i + 1] = num;
				i++;
			}
			if (num != 1f)
			{
				Service.Get<StaRTSLogger>().WarnFormat("The campaign point distribution for mission {0} does not add up to 100: {1}", new object[]
				{
					this.Uid,
					raw
				});
			}
			return Convert.ToInt32(array[0], CultureInfo.InvariantCulture);
		}

		public CampaignMissionVO()
		{
		}

		protected internal CampaignMissionVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).AmbientMusic);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BattleMusic);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIChapterId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIContext);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIEnemyTier);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIMissionId);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIMissionName);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).CampaignPoints);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).CampaignUid);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_ambientMusic);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_battleMusic);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_bi_chap_id);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_bi_context);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_bi_enemy_tier);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_bi_mission_id);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_bi_mission_name);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_campaignPoints);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_campaignUid);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_description);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_failureCondition);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_fixedWaves);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_goalFailStory);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_goalFailString);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_goalString);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_grind);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_introStory);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_loseStory);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_map);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_missionType);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_opponentName);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_progressString);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_raidDesc);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_raidImage);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_replay);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_rewards);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_title);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_totalLoot);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_unlockOrder);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_victoryConditions);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_waves);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CampaignMissionVO.COLUMN_winStory);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Conditions);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).FailureCondition);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).FailureStory);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).FixedWaves);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).GoalFailureStory);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).GoalFailureString);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).GoalString);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Grind);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).IntroStory);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Map);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).MasteryStars);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).MissionType);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).OpponentName);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).ProgressString);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).RaidBriefingBGTextureName);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).RaidDescriptionID);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Replayable);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Rewards);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).StarsToPortion);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).SuccessStory);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Title);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).TotalLoot);
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).UnlockOrder);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Waves);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).HasPvpCondition());
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).IsChallengeMission());
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).IsCombatMission());
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).IsRaidDefense());
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).ParseCampaignPoints(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).AmbientMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BattleMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIChapterId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIContext = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIEnemyTier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIMissionId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).BIMissionName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).CampaignPoints = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).CampaignUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_ambientMusic = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_battleMusic = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_bi_chap_id = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_bi_context = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_bi_enemy_tier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_bi_mission_id = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_bi_mission_name = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_campaignPoints = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_campaignUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_description = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_failureCondition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_fixedWaves = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_goalFailStory = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_goalFailString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_goalString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_grind = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_introStory = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_loseStory = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_map = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_missionType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_opponentName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_progressString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_raidDesc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_raidImage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_replay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_rewards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_title = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_totalLoot = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_unlockOrder = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_victoryConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_waves = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			CampaignMissionVO.COLUMN_winStory = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Conditions = (List<ConditionVO>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Description = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).FailureCondition = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).FailureStory = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).FixedWaves = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).GoalFailureStory = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).GoalFailureString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).GoalString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Grind = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).IntroStory = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Map = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).MasteryStars = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).MissionType = (MissionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).OpponentName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).ProgressString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).RaidBriefingBGTextureName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).RaidDescriptionID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Replayable = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Rewards = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).StarsToPortion = (float[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).SuccessStory = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Title = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).TotalLoot = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).UnlockOrder = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			((CampaignMissionVO)GCHandledObjects.GCHandleToObject(instance)).Waves = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
