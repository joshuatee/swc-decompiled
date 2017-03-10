using StaRTS.Externals.FileManagement;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public class BattleEntry : ISerializable
	{
		private Dictionary<string, object> troopsExpended;

		private Dictionary<string, object> attackerGuildTroopsDeployed;

		public string RecordID
		{
			get;
			set;
		}

		public BattleParticipant Attacker
		{
			get;
			set;
		}

		public BattleParticipant Defender
		{
			get;
			set;
		}

		public int LootCreditsAvailable
		{
			get;
			set;
		}

		public int LootMaterialsAvailable
		{
			get;
			set;
		}

		public int LootContrabandAvailable
		{
			get;
			set;
		}

		public int LootCreditsEarned
		{
			get;
			set;
		}

		public int LootMaterialsEarned
		{
			get;
			set;
		}

		public int LootContrabandEarned
		{
			get;
			set;
		}

		public int LootCreditsDeducted
		{
			get;
			set;
		}

		public int LootMaterialsDeducted
		{
			get;
			set;
		}

		public int LootContrabandDeducted
		{
			get;
			set;
		}

		public int PotentialMedalsToGain
		{
			get;
			set;
		}

		public uint CampaignPointsEarn
		{
			get;
			set;
		}

		public int EarnedStars
		{
			get;
			set;
		}

		public int DamagePercent
		{
			get;
			set;
		}

		public string MissionId
		{
			get;
			set;
		}

		public string BattleVersion
		{
			get;
			set;
		}

		public string PlanetId
		{
			get;
			set;
		}

		public string ManifestVersion
		{
			get;
			set;
		}

		public string CmsVersion
		{
			get;
			set;
		}

		public List<ConditionVO> VictoryConditions
		{
			get;
			set;
		}

		public ConditionVO FailureCondition
		{
			get;
			set;
		}

		public BattleDeploymentData AttackerDeployedData
		{
			get;
			set;
		}

		public BattleDeploymentData DefenderDeployedData
		{
			get;
			set;
		}

		public string DefenseEncounterProfile
		{
			get;
			set;
		}

		public string BattleScript
		{
			get;
			set;
		}

		public bool AllowReplay
		{
			get;
			set;
		}

		public int WarVictoryPointsAvailable
		{
			get;
			set;
		}

		public List<string> AttackerWarBuffs
		{
			get;
			set;
		}

		public List<string> DefenderWarBuffs
		{
			get;
			set;
		}

		public List<string> AttackerEquipment
		{
			get;
			set;
		}

		public List<string> DefenderEquipment
		{
			get;
			set;
		}

		public string FailedConditionUid
		{
			get;
			set;
		}

		public bool Revenged
		{
			get;
			set;
		}

		public uint EndBattleServerTime
		{
			get;
			set;
		}

		public bool Won
		{
			get;
			set;
		}

		public string SharerPlayerId
		{
			get;
			set;
		}

		public string AttackerID
		{
			get
			{
				if (this.Attacker != null)
				{
					return this.Attacker.PlayerId;
				}
				return "";
			}
		}

		public string DefenderID
		{
			get
			{
				if (this.Defender != null)
				{
					return this.Defender.PlayerId;
				}
				return "";
			}
		}

		public string ToJson()
		{
			return Serializer.Start().End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.BattleVersion = (dictionary.ContainsKey("battleVersion") ? Convert.ToString(dictionary["battleVersion"], CultureInfo.InvariantCulture) : "21.0".ToString());
			this.CmsVersion = (dictionary.ContainsKey("cmsVersion") ? Convert.ToString(dictionary["cmsVersion"], CultureInfo.InvariantCulture) : Service.Get<FMS>().GetFileVersion("patches/base.json").ToString());
			this.MissionId = (dictionary.ContainsKey("missionId") ? Convert.ToString(dictionary["missionId"], CultureInfo.InvariantCulture) : null);
			this.EarnedStars = Convert.ToInt32(dictionary["stars"], CultureInfo.InvariantCulture);
			this.DamagePercent = Convert.ToInt32(dictionary["baseDamagePercent"], CultureInfo.InvariantCulture);
			this.ManifestVersion = Convert.ToString(dictionary["manifestVersion"], CultureInfo.InvariantCulture);
			this.RecordID = Convert.ToString(dictionary["battleId"], CultureInfo.InvariantCulture);
			this.Attacker = BattleParticipant.CreateFromObject(dictionary["attacker"]);
			this.Defender = BattleParticipant.CreateFromObject(dictionary["defender"]);
			this.EndBattleServerTime = Convert.ToUInt32(dictionary["attackDate"], CultureInfo.InvariantCulture);
			if (dictionary.ContainsKey("planetId"))
			{
				this.PlanetId = Convert.ToString(dictionary["planetId"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("potentialMedalGain"))
			{
				this.PotentialMedalsToGain = Convert.ToInt32(dictionary["potentialMedalGain"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("troopsExpended"))
			{
				this.troopsExpended = (dictionary["troopsExpended"] as Dictionary<string, object>);
			}
			if (dictionary.ContainsKey("attackerGuildTroopsExpended"))
			{
				this.attackerGuildTroopsDeployed = (dictionary["attackerGuildTroopsExpended"] as Dictionary<string, object>);
			}
			this.DefenderDeployedData = null;
			if (dictionary.ContainsKey("looted"))
			{
				Dictionary<string, object> dictionary2 = dictionary["looted"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					if (dictionary2.ContainsKey("credits"))
					{
						this.LootCreditsDeducted = Convert.ToInt32(dictionary2["credits"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("materials"))
					{
						this.LootMaterialsDeducted = Convert.ToInt32(dictionary2["materials"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("contraband"))
					{
						this.LootContrabandDeducted = Convert.ToInt32(dictionary2["contraband"], CultureInfo.InvariantCulture);
					}
				}
			}
			bool flag = false;
			if (dictionary.ContainsKey("earned"))
			{
				Dictionary<string, object> dictionary3 = dictionary["earned"] as Dictionary<string, object>;
				if (dictionary3 != null)
				{
					flag = true;
					if (dictionary3.ContainsKey("credits"))
					{
						this.LootCreditsEarned = Convert.ToInt32(dictionary3["credits"], CultureInfo.InvariantCulture);
					}
					if (dictionary3.ContainsKey("materials"))
					{
						this.LootMaterialsEarned = Convert.ToInt32(dictionary3["materials"], CultureInfo.InvariantCulture);
					}
					if (dictionary3.ContainsKey("contraband"))
					{
						this.LootContrabandEarned = Convert.ToInt32(dictionary3["contraband"], CultureInfo.InvariantCulture);
					}
				}
			}
			if (!flag)
			{
				this.LootCreditsEarned = this.LootCreditsDeducted;
				this.LootMaterialsEarned = this.LootMaterialsDeducted;
				this.LootContrabandEarned = this.LootContrabandDeducted;
			}
			if (dictionary.ContainsKey("maxLootable"))
			{
				Dictionary<string, object> dictionary4 = dictionary["maxLootable"] as Dictionary<string, object>;
				if (dictionary4 != null)
				{
					if (dictionary4.ContainsKey("credits"))
					{
						this.LootCreditsAvailable = Convert.ToInt32(dictionary4["credits"], CultureInfo.InvariantCulture);
					}
					if (dictionary4.ContainsKey("materials"))
					{
						this.LootMaterialsAvailable = Convert.ToInt32(dictionary4["materials"], CultureInfo.InvariantCulture);
					}
					if (dictionary4.ContainsKey("contraband"))
					{
						this.LootContrabandAvailable = Convert.ToInt32(dictionary4["contraband"], CultureInfo.InvariantCulture);
					}
				}
			}
			this.Revenged = false;
			if (dictionary.ContainsKey("revenged"))
			{
				this.Revenged = Convert.ToBoolean(dictionary["revenged"], CultureInfo.InvariantCulture);
			}
			string playerId = Service.Get<CurrentPlayer>().PlayerId;
			this.Won = ((this.AttackerID == playerId && this.EarnedStars > 0) || (this.DefenderID == playerId && this.EarnedStars == 0));
			if (dictionary.ContainsKey("defenseEncounterProfile"))
			{
				this.DefenseEncounterProfile = (dictionary["defenseEncounterProfile"] as string);
			}
			if (dictionary.ContainsKey("battleScript"))
			{
				this.BattleScript = (dictionary["battleScript"] as string);
			}
			if (dictionary.ContainsKey("attackerEquipment"))
			{
				this.AttackerEquipment = new List<string>();
				List<object> list = dictionary["attackerEquipment"] as List<object>;
				if (list != null)
				{
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						this.AttackerEquipment.Add((string)list[i]);
						i++;
					}
				}
			}
			if (dictionary.ContainsKey("defenderEquipment"))
			{
				this.DefenderEquipment = new List<string>();
				List<object> list2 = dictionary["defenderEquipment"] as List<object>;
				if (list2 != null)
				{
					int j = 0;
					int count2 = list2.Count;
					while (j < count2)
					{
						this.DefenderEquipment.Add((string)list2[j]);
						j++;
					}
				}
			}
			return this;
		}

		public void SetupExpendedTroops()
		{
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
			Dictionary<string, int> dictionary4 = new Dictionary<string, int>();
			Dictionary<string, int> dictionary5 = new Dictionary<string, int>();
			if (this.troopsExpended != null)
			{
				foreach (string current in this.troopsExpended.Keys)
				{
					int value = Convert.ToInt32(this.troopsExpended[current], CultureInfo.InvariantCulture);
					TroopTypeVO optional = dataController.GetOptional<TroopTypeVO>(current);
					if (optional != null)
					{
						TroopType type = optional.Type;
						if (type != TroopType.Hero)
						{
							if (type != TroopType.Champion)
							{
								dictionary.Add(current, value);
							}
							else
							{
								dictionary4.Add(current, value);
							}
						}
						else
						{
							dictionary3.Add(current, value);
						}
					}
					else
					{
						SpecialAttackTypeVO optional2 = dataController.GetOptional<SpecialAttackTypeVO>(current);
						if (optional2 != null)
						{
							dictionary2.Add(current, value);
						}
					}
				}
			}
			if (this.attackerGuildTroopsDeployed != null)
			{
				foreach (string current2 in this.attackerGuildTroopsDeployed.Keys)
				{
					int value2 = Convert.ToInt32(this.attackerGuildTroopsDeployed[current2], CultureInfo.InvariantCulture);
					TroopTypeVO optional3 = dataController.GetOptional<TroopTypeVO>(current2);
					if (optional3 != null)
					{
						dictionary5.Add(current2, value2);
					}
				}
			}
			this.AttackerDeployedData = BattleDeploymentData.CreateEmpty();
			this.AttackerDeployedData.TroopData = dictionary;
			this.AttackerDeployedData.SpecialAttackData = dictionary2;
			this.AttackerDeployedData.HeroData = dictionary3;
			this.AttackerDeployedData.ChampionData = dictionary4;
			this.AttackerDeployedData.SquadData = dictionary5;
		}

		public bool IsPvP()
		{
			return string.IsNullOrEmpty(this.MissionId);
		}

		public bool IsSpecOps()
		{
			return GameUtils.IsMissionSpecOps(this.MissionId);
		}

		public bool IsSquadDeployAllowedInRaid()
		{
			return this.IsRaidDefense() && Service.Get<RaidDefenseController>().SquadTroopDeployAllowed();
		}

		public bool IsRaidDefense()
		{
			return GameUtils.IsMissionRaidDefense(this.MissionId);
		}

		public bool IsDefense()
		{
			return GameUtils.IsMissionDefense(this.MissionId);
		}

		public BattleEntry Clone()
		{
			return (BattleEntry)base.MemberwiseClone();
		}

		public BattleEntry()
		{
		}

		protected internal BattleEntry(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).Clone());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).AllowReplay);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).Attacker);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).AttackerDeployedData);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).AttackerEquipment);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).AttackerID);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).AttackerWarBuffs);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).BattleScript);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).BattleVersion);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).CmsVersion);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DamagePercent);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).Defender);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DefenderDeployedData);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DefenderEquipment);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DefenderID);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DefenderWarBuffs);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DefenseEncounterProfile);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).EarnedStars);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).FailedConditionUid);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).FailureCondition);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootContrabandAvailable);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootContrabandDeducted);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootContrabandEarned);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootCreditsAvailable);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootCreditsDeducted);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootCreditsEarned);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsAvailable);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsDeducted);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsEarned);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).ManifestVersion);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).MissionId);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).PotentialMedalsToGain);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).RecordID);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).Revenged);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).SharerPlayerId);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).VictoryConditions);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).WarVictoryPointsAvailable);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).Won);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).IsDefense());
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).IsPvP());
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).IsRaidDefense());
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).IsSpecOps());
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).IsSquadDeployAllowedInRaid());
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).AllowReplay = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).Attacker = (BattleParticipant)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).AttackerDeployedData = (BattleDeploymentData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).AttackerEquipment = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).AttackerWarBuffs = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).BattleScript = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).BattleVersion = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).CmsVersion = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DamagePercent = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).Defender = (BattleParticipant)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DefenderDeployedData = (BattleDeploymentData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DefenderEquipment = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DefenderWarBuffs = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).DefenseEncounterProfile = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).EarnedStars = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).FailedConditionUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).FailureCondition = (ConditionVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootContrabandAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootContrabandDeducted = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootContrabandEarned = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootCreditsAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootCreditsDeducted = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootCreditsEarned = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsDeducted = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsEarned = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).ManifestVersion = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).MissionId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).PotentialMedalsToGain = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).RecordID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).Revenged = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).SharerPlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).VictoryConditions = (List<ConditionVO>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).WarVictoryPointsAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).Won = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).SetupExpendedTroops();
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEntry)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
