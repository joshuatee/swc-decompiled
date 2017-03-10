using StaRTS.Main.Models.Cee.Serializables;
using StaRTS.Utils;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle.Replay
{
	public class BattleRecord : ISerializable
	{
		public const string ACTION_ID_KEY = "actionId";

		private const string COMBAT_ENCOUNTER_KEY = "combatEncounter";

		private const string BATTLE_ATTRIBUTES_KEY = "battleAttributes";

		private const string BATTLE_ACTIONS_KEY = "battleActions";

		private const string BATTLE_TYPE_KEY = "battleType";

		private const string BATTLE_LENGTH_KEY = "battleLength";

		private const string ATTACKER_DEPLOYMENT_DATA_KEY = "attackerDeploymentData";

		private const string DEFENDER_DEPLOYMENT_DATA_KEY = "defenderDeploymentData";

		private const string PLANET_ID_KEY = "planetId";

		private const string LOOT_CREDITS_AVAILABLE_KEY = "lootCreditsAvailable";

		private const string LOOT_MATERIALS_AVAILABLE_KEY = "lootMaterialsAvailable";

		private const string LOOT_CONTRABAND_AVAILABLE_KEY = "lootContrabandAvailable";

		private const string LOOT_BUILDING_CREDITS_MAP_KEY = "lootBuildingCreditsMap";

		private const string LOOT_BUILDING_MATERIALS_MAP_KEY = "lootBuildingMaterialsMap";

		private const string LOOT_BUILDING_CONTRABAND_MAP_KEY = "lootBuildingContrabandMap";

		private const string BATTLE_VERSION_KEY = "battleVersion";

		private const string BATTLE_PLANET_ID_KEY = "planetId";

		private const string CMS_VERSION_KEY = "manifestVersion";

		private const string LOWEST_FPS_KEY = "lowFPS";

		private const string LOWEST_FPS_TIME_KEY = "lowFPSTime";

		private const string VICTORY_CONDITIONS = "victoryConditions";

		private const string FAILURE_CONDITION = "failureCondition";

		private const string DEFENDER_GUILD_TROOPS = "donatedTroops";

		private const string ATTACKER_GUILD_TROOPS = "donatedTroopsAttacker";

		private const string DEFENDER_CHAMPIONS = "champions";

		private const string DEFENSE_ENCOUNTER_PROFILE = "defenseEncounterProfile";

		private const string BATTLE_SCRIPT = "battleScript";

		private const string DISABLED_BUILDINGS = "disabledBuildings";

		private const string VIEW_TIME_PRE_BATTLE = "viewTimePreBattle";

		private List<IBattleAction> battleActions;

		public List<string> victoryConditionsUids;

		public string failureConditionUid;

		public string DefenseEncounterProfile;

		public string BattleScript;

		public RandSimSeed SimSeed;

		public string RecordId
		{
			get;
			set;
		}

		public CombatEncounter CombatEncounter
		{
			get;
			set;
		}

		public BattleDeploymentData AttackerDeploymentData
		{
			get;
			set;
		}

		public BattleDeploymentData DefenderDeploymentData
		{
			get;
			set;
		}

		public Dictionary<string, int> DefenderGuildTroops
		{
			get;
			set;
		}

		public Dictionary<string, int> AttackerGuildTroops
		{
			get;
			set;
		}

		public Dictionary<string, int> DefenderChampions
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

		public Dictionary<string, int> BuildingLootCreditsMap
		{
			get;
			set;
		}

		public Dictionary<string, int> BuildingLootMaterialsMap
		{
			get;
			set;
		}

		public Dictionary<string, int> BuildingLootContrabandMap
		{
			get;
			set;
		}

		public BattleType BattleType
		{
			get;
			set;
		}

		public List<IBattleAction> BattleActions
		{
			get
			{
				return this.battleActions;
			}
		}

		public string MissionId
		{
			get;
			set;
		}

		public string PlanetId
		{
			get;
			set;
		}

		public string BattleVersion
		{
			get;
			set;
		}

		public string CmsVersion
		{
			get;
			set;
		}

		public BattleAttributes BattleAttributes
		{
			get;
			set;
		}

		public int BattleLength
		{
			get;
			set;
		}

		public List<string> DisabledBuildings
		{
			get;
			set;
		}

		public float LowestFPS
		{
			get;
			set;
		}

		public uint LowestFPSTime
		{
			get;
			set;
		}

		public float ViewTimePassedPreBattle
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

		public BattleRecord()
		{
			this.battleActions = new List<IBattleAction>();
			this.BattleAttributes = new BattleAttributes();
		}

		public void Add(IBattleAction battleAction)
		{
			this.battleActions.Add(battleAction);
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.CombatEncounter = (new CombatEncounter().FromObject(dictionary["combatEncounter"]) as CombatEncounter);
			List<object> list = dictionary["battleActions"] as List<object>;
			foreach (object current in list)
			{
				Dictionary<string, object> dictionary2 = current as Dictionary<string, object>;
				string actionId = dictionary2["actionId"] as string;
				this.BattleActions.Add(BattleRecord.CreateBattleAction(actionId, current));
			}
			if (dictionary.ContainsKey("attackerDeploymentData"))
			{
				object obj2 = dictionary["attackerDeploymentData"];
				if (obj2 != null)
				{
					this.AttackerDeploymentData = (new BattleDeploymentData().FromObject(obj2) as BattleDeploymentData);
				}
			}
			if (dictionary.ContainsKey("defenderDeploymentData"))
			{
				object obj3 = dictionary["defenderDeploymentData"];
				if (obj3 != null)
				{
					this.DefenderDeploymentData = (new BattleDeploymentData().FromObject(obj3) as BattleDeploymentData);
				}
			}
			if (dictionary.ContainsKey("battleType"))
			{
				this.BattleType = StringUtils.ParseEnum<BattleType>(dictionary["battleType"] as string);
			}
			else
			{
				this.BattleType = BattleType.Pvp;
			}
			this.BattleLength = Convert.ToInt32(dictionary["battleLength"], CultureInfo.InvariantCulture);
			if (dictionary.ContainsKey("lowFPS"))
			{
				this.LowestFPS = (float)Convert.ToInt32(dictionary["lowFPS"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("lowFPSTime"))
			{
				this.LowestFPSTime = (uint)Convert.ToInt32(dictionary["lowFPSTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("missionId"))
			{
				this.MissionId = (dictionary["missionId"] as string);
			}
			if (dictionary.ContainsKey("planetId"))
			{
				this.PlanetId = (dictionary["planetId"] as string);
			}
			if (this.PlanetId == null && this.CombatEncounter.map.Planet != null)
			{
				this.PlanetId = this.CombatEncounter.map.Planet.Uid;
			}
			if (dictionary.ContainsKey("battleVersion"))
			{
				this.BattleVersion = (dictionary["battleVersion"] as string);
			}
			this.CmsVersion = (dictionary["manifestVersion"] as string);
			if (dictionary.ContainsKey("battleAttributes"))
			{
				this.BattleAttributes = new BattleAttributes();
				this.BattleAttributes.FromObject(dictionary["battleAttributes"]);
			}
			this.LootCreditsAvailable = Convert.ToInt32(dictionary["lootCreditsAvailable"], CultureInfo.InvariantCulture);
			this.LootMaterialsAvailable = Convert.ToInt32(dictionary["lootMaterialsAvailable"], CultureInfo.InvariantCulture);
			if (dictionary.ContainsKey("lootContrabandAvailable"))
			{
				this.LootContrabandAvailable = Convert.ToInt32(dictionary["lootContrabandAvailable"], CultureInfo.InvariantCulture);
			}
			this.BuildingLootCreditsMap = new Dictionary<string, int>();
			if (dictionary.ContainsKey("lootBuildingCreditsMap"))
			{
				Dictionary<string, object> dictionary3 = dictionary["lootBuildingCreditsMap"] as Dictionary<string, object>;
				if (dictionary3 != null)
				{
					foreach (KeyValuePair<string, object> current2 in dictionary3)
					{
						this.BuildingLootCreditsMap.Add(current2.get_Key(), Convert.ToInt32(current2.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			this.BuildingLootMaterialsMap = new Dictionary<string, int>();
			if (dictionary.ContainsKey("lootBuildingMaterialsMap"))
			{
				Dictionary<string, object> dictionary4 = dictionary["lootBuildingMaterialsMap"] as Dictionary<string, object>;
				if (dictionary4 != null)
				{
					foreach (KeyValuePair<string, object> current3 in dictionary4)
					{
						this.BuildingLootMaterialsMap.Add(current3.get_Key(), Convert.ToInt32(current3.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			this.BuildingLootContrabandMap = new Dictionary<string, int>();
			if (dictionary.ContainsKey("lootBuildingContrabandMap"))
			{
				Dictionary<string, object> dictionary5 = dictionary["lootBuildingContrabandMap"] as Dictionary<string, object>;
				if (dictionary5 != null)
				{
					foreach (KeyValuePair<string, object> current4 in dictionary5)
					{
						this.BuildingLootContrabandMap.Add(current4.get_Key(), Convert.ToInt32(current4.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			this.victoryConditionsUids = new List<string>();
			List<object> list2 = dictionary["victoryConditions"] as List<object>;
			int i = 0;
			int count = list2.Count;
			while (i < count)
			{
				this.victoryConditionsUids.Add(list2[i] as string);
				i++;
			}
			this.failureConditionUid = (dictionary["failureCondition"] as string);
			this.DefenderGuildTroops = new Dictionary<string, int>();
			if (dictionary.ContainsKey("donatedTroops"))
			{
				Dictionary<string, object> dictionary6 = dictionary["donatedTroops"] as Dictionary<string, object>;
				if (dictionary6 != null)
				{
					foreach (KeyValuePair<string, object> current5 in dictionary6)
					{
						this.DefenderGuildTroops.Add(current5.get_Key(), Convert.ToInt32(current5.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			this.AttackerGuildTroops = new Dictionary<string, int>();
			if (dictionary.ContainsKey("donatedTroopsAttacker"))
			{
				Dictionary<string, object> dictionary7 = dictionary["donatedTroopsAttacker"] as Dictionary<string, object>;
				if (dictionary7 != null)
				{
					foreach (KeyValuePair<string, object> current6 in dictionary7)
					{
						this.AttackerGuildTroops.Add(current6.get_Key(), Convert.ToInt32(current6.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			this.DefenderChampions = new Dictionary<string, int>();
			if (dictionary.ContainsKey("champions"))
			{
				Dictionary<string, object> dictionary8 = dictionary["champions"] as Dictionary<string, object>;
				if (dictionary8 != null)
				{
					foreach (KeyValuePair<string, object> current7 in dictionary8)
					{
						this.DefenderChampions.Add(current7.get_Key(), Convert.ToInt32(current7.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			if (dictionary.ContainsKey("defenseEncounterProfile"))
			{
				this.DefenseEncounterProfile = (dictionary["defenseEncounterProfile"] as string);
			}
			if (dictionary.ContainsKey("battleScript"))
			{
				this.BattleScript = (dictionary["battleScript"] as string);
			}
			this.DisabledBuildings = new List<string>();
			if (dictionary.ContainsKey("disabledBuildings"))
			{
				List<object> list3 = dictionary["disabledBuildings"] as List<object>;
				for (int j = 0; j < list3.Count; j++)
				{
					this.DisabledBuildings.Add((string)list3[j]);
				}
			}
			if (dictionary.ContainsKey("simSeedA"))
			{
				this.SimSeed.SimSeedA = uint.Parse(dictionary["simSeedA"] as string);
			}
			if (dictionary.ContainsKey("simSeedB"))
			{
				this.SimSeed.SimSeedB = uint.Parse(dictionary["simSeedB"] as string);
			}
			if (dictionary.ContainsKey("viewTimePreBattle"))
			{
				this.ViewTimePassedPreBattle = float.Parse(dictionary["viewTimePreBattle"] as string, CultureInfo.InvariantCulture);
			}
			this.AttackerWarBuffs = this.ParseStringList(dictionary, "attackerWarBuffs");
			this.DefenderWarBuffs = this.ParseStringList(dictionary, "defenderWarBuffs");
			this.AttackerEquipment = this.ParseStringList(dictionary, "attackerEquipment");
			this.DefenderEquipment = this.ParseStringList(dictionary, "defenderEquipment");
			return this;
		}

		private List<string> ParseStringList(Dictionary<string, object> dictionary, string key)
		{
			List<string> list = null;
			if (dictionary.ContainsKey(key))
			{
				List<object> list2 = dictionary[key] as List<object>;
				if (list2 != null)
				{
					list = new List<string>();
					int i = 0;
					int count = list2.Count;
					while (i < count)
					{
						list.Add(Convert.ToString(list2[i], CultureInfo.InvariantCulture));
						i++;
					}
				}
			}
			return list;
		}

		private static IBattleAction CreateBattleAction(string actionId, object action)
		{
			IBattleAction result = null;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(actionId);
			if (num <= 1033960932u)
			{
				if (num != 255476488u)
				{
					if (num != 619276938u)
					{
						if (num == 1033960932u)
						{
							if (actionId == "HeroAbilityActivated")
							{
								result = (new HeroAbilityAction().FromObject(action) as HeroAbilityAction);
							}
						}
					}
					else if (actionId == "TroopPlaced")
					{
						result = (new TroopPlacedAction().FromObject(action) as TroopPlacedAction);
					}
				}
				else if (actionId == "BattleCanceled")
				{
					result = (new BattleCanceledAction().FromObject(action) as BattleCanceledAction);
				}
			}
			else if (num <= 2120365278u)
			{
				if (num != 1186883982u)
				{
					if (num == 2120365278u)
					{
						if (actionId == "SpecialAttackDeployed")
						{
							result = (new SpecialAttackDeployedAction().FromObject(action) as SpecialAttackDeployedAction);
						}
					}
				}
				else if (actionId == "SquadTroopPlaced")
				{
					result = (new SquadTroopPlacedAction().FromObject(action) as SquadTroopPlacedAction);
				}
			}
			else if (num != 3440613923u)
			{
				if (num == 3805918252u)
				{
					if (actionId == "ChampionDeployed")
					{
						result = (new ChampionDeployedAction().FromObject(action) as ChampionDeployedAction);
					}
				}
			}
			else if (actionId == "HeroDeployed")
			{
				result = (new HeroDeployedAction().FromObject(action) as HeroDeployedAction);
			}
			return result;
		}

		public string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddObject<CombatEncounter>("combatEncounter", this.CombatEncounter);
			serializer.AddArray<IBattleAction>("battleActions", this.battleActions);
			serializer.AddObject<BattleDeploymentData>("attackerDeploymentData", this.AttackerDeploymentData);
			serializer.AddObject<BattleDeploymentData>("defenderDeploymentData", this.DefenderDeploymentData);
			serializer.Add<int>("lootCreditsAvailable", this.LootCreditsAvailable);
			serializer.Add<int>("lootMaterialsAvailable", this.LootMaterialsAvailable);
			serializer.Add<int>("lootContrabandAvailable", this.LootContrabandAvailable);
			if (this.BuildingLootCreditsMap != null)
			{
				serializer.AddDictionary<int>("lootBuildingCreditsMap", this.BuildingLootCreditsMap);
			}
			if (this.BuildingLootMaterialsMap != null)
			{
				serializer.AddDictionary<int>("lootBuildingMaterialsMap", this.BuildingLootMaterialsMap);
			}
			if (this.BuildingLootContrabandMap != null)
			{
				serializer.AddDictionary<int>("lootBuildingContrabandMap", this.BuildingLootContrabandMap);
			}
			serializer.AddString("battleType", this.BattleType.ToString());
			serializer.Add<int>("battleLength", this.BattleLength);
			serializer.Add<int>("lowFPS", (int)this.LowestFPS);
			serializer.Add<int>("lowFPSTime", (int)this.LowestFPSTime);
			serializer.AddString("battleVersion", this.BattleVersion);
			serializer.AddString("planetId", this.PlanetId);
			serializer.AddString("manifestVersion", this.CmsVersion);
			serializer.AddObject<BattleAttributes>("battleAttributes", this.BattleAttributes);
			if (this.victoryConditionsUids != null)
			{
				serializer.AddArrayOfPrimitives<string>("victoryConditions", this.victoryConditionsUids);
			}
			serializer.AddString("failureCondition", this.failureConditionUid);
			if (this.DefenderGuildTroops != null)
			{
				serializer.AddDictionary<int>("donatedTroops", this.DefenderGuildTroops);
			}
			if (this.AttackerGuildTroops != null)
			{
				serializer.AddDictionary<int>("donatedTroopsAttacker", this.AttackerGuildTroops);
			}
			if (this.DefenderChampions != null)
			{
				serializer.AddDictionary<int>("champions", this.DefenderChampions);
			}
			if (!string.IsNullOrEmpty(this.DefenseEncounterProfile))
			{
				serializer.AddString("defenseEncounterProfile", this.DefenseEncounterProfile);
			}
			if (!string.IsNullOrEmpty(this.BattleScript))
			{
				serializer.AddString("battleScript", this.BattleScript);
			}
			if (this.DisabledBuildings != null)
			{
				serializer.AddArrayOfPrimitives<string>("disabledBuildings", this.DisabledBuildings);
			}
			serializer.Add<uint>("simSeedA", this.SimSeed.SimSeedA);
			serializer.Add<uint>("simSeedB", this.SimSeed.SimSeedB);
			serializer.Add<float>("viewTimePreBattle", this.ViewTimePassedPreBattle);
			if (this.AttackerWarBuffs != null)
			{
				serializer.AddArrayOfPrimitives<string>("attackerWarBuffs", this.AttackerWarBuffs);
			}
			if (this.DefenderWarBuffs != null)
			{
				serializer.AddArrayOfPrimitives<string>("defenderWarBuffs", this.DefenderWarBuffs);
			}
			if (this.AttackerEquipment != null)
			{
				serializer.AddArrayOfPrimitives<string>("attackerEquipment", this.AttackerEquipment);
			}
			if (this.DefenderEquipment != null)
			{
				serializer.AddArrayOfPrimitives<string>("defenderEquipment", this.DefenderEquipment);
			}
			return serializer.End().ToString();
		}

		protected internal BattleRecord(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).Add((IBattleAction)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleRecord.CreateBattleAction(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).AttackerDeploymentData);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).AttackerEquipment);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildTroops);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).AttackerWarBuffs);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BattleActions);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BattleAttributes);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BattleLength);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BattleType);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BattleVersion);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BuildingLootContrabandMap);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BuildingLootCreditsMap);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BuildingLootMaterialsMap);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).CmsVersion);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).CombatEncounter);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderChampions);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderDeploymentData);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderEquipment);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildTroops);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderWarBuffs);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DisabledBuildings);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).LootContrabandAvailable);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).LootCreditsAvailable);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsAvailable);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).LowestFPS);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).MissionId);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).RecordId);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).ViewTimePassedPreBattle);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).ParseStringList((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).AttackerDeploymentData = (BattleDeploymentData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).AttackerEquipment = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildTroops = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).AttackerWarBuffs = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BattleAttributes = (BattleAttributes)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BattleLength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BattleType = (BattleType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BattleVersion = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BuildingLootContrabandMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BuildingLootCreditsMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).BuildingLootMaterialsMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).CmsVersion = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).CombatEncounter = (CombatEncounter)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderChampions = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderDeploymentData = (BattleDeploymentData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderEquipment = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildTroops = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DefenderWarBuffs = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).DisabledBuildings = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).LootContrabandAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).LootCreditsAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).LowestFPS = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).MissionId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).RecordId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).ViewTimePassedPreBattle = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleRecord)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
