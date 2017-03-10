using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Story.Trigger;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Story
{
	public class StoryTriggerFactory
	{
		private const string AUTO = "Auto";

		private const string BATTLE_COMPLETE = "BattleComplete";

		private const string CLUSTER_AND = "ClusterAND";

		private const string CLUSTER_ORDER = "ClusterORDER";

		private const string EVENT_COUNTER = "EventCounter";

		private const string FACTION_CHOICE_COMPLETE = "FactionChoiceComplete";

		private const string GALAXY_MAP_OPEN = "GalaxyMapOpen";

		private const string GAME_STATE = "GameState";

		private const string HQ_LEVEL = "HQLevel";

		private const string MISSION_VICTORY = "MissionVictory";

		private const string PLANET_RELOCATE = "PlanetRelocate";

		private const string REPAIR_ALL_COMPLETE = "RepairAllComplete";

		private const string REPAIR_BUILDING_COMPLETE = "RepairBuildingComplete";

		private const string SCREEN_APPEARS = "ScreenAppears";

		private const string SCOUT_PLANET = "ScoutPlanet";

		private const string TIMER = "Timer";

		private const string UNLOCK_PLANET = "UnlockPlanet";

		private const string WORLD_LOAD = "WorldLoad";

		private const string WORLD_TRANSITION_COMPLETE = "WorldTransitionComplete";

		private const string SQUAD_MEMBER = "SquadMember";

		private const string BLOCKING_WAR_CLICK = "BlockingInitWarClick";

		private const string SQUAD_SIZE = "SquadSize";

		private const string SQUAD_ACTIVE_MEMBERS = "squadActiveMembers";

		private const string SQUAD_UI_OPEN = "squadUIOpen";

		public static IStoryTrigger GenerateStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent)
		{
			string triggerType = vo.TriggerType;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(triggerType);
			if (num <= 2057951039u)
			{
				if (num <= 880974561u)
				{
					if (num <= 325412241u)
					{
						if (num != 75439796u)
						{
							if (num != 309063606u)
							{
								if (num != 325412241u)
								{
									goto IL_426;
								}
								if (!(triggerType == "GalaxyMapOpen"))
								{
									goto IL_426;
								}
								return new GalaxyMapOpenStoryTrigger(vo, parent);
							}
							else
							{
								if (!(triggerType == "Auto"))
								{
									goto IL_426;
								}
								return new AutoStoryTrigger(vo, parent);
							}
						}
						else
						{
							if (!(triggerType == "BattleComplete"))
							{
								goto IL_426;
							}
							return new BattleCompleteStoryTrigger(vo, parent);
						}
					}
					else if (num != 738717363u)
					{
						if (num != 809982735u)
						{
							if (num != 880974561u)
							{
								goto IL_426;
							}
							if (!(triggerType == "ScreenAppears"))
							{
								goto IL_426;
							}
							return new ScreenAppearStoryTrigger(vo, parent);
						}
						else
						{
							if (!(triggerType == "SquadMember"))
							{
								goto IL_426;
							}
							return new SquadMemberStoryTrigger(vo, parent);
						}
					}
					else
					{
						if (!(triggerType == "ScoutPlanet"))
						{
							goto IL_426;
						}
						return new ScoutPlanetTrigger(vo, parent);
					}
				}
				else if (num <= 1166570171u)
				{
					if (num != 959338504u)
					{
						if (num != 1066278926u)
						{
							if (num != 1166570171u)
							{
								goto IL_426;
							}
							if (!(triggerType == "RepairBuildingComplete"))
							{
								goto IL_426;
							}
						}
						else
						{
							if (!(triggerType == "ClusterAND"))
							{
								goto IL_426;
							}
							return new ClusterANDStoryTrigger(vo, parent);
						}
					}
					else
					{
						if (!(triggerType == "GameState"))
						{
							goto IL_426;
						}
						return new GameStateStoryTrigger(vo, parent);
					}
				}
				else if (num != 1575734819u)
				{
					if (num != 1610766953u)
					{
						if (num != 2057951039u)
						{
							goto IL_426;
						}
						if (!(triggerType == "FactionChoiceComplete"))
						{
							goto IL_426;
						}
						return new FactionChangedStoryTrigger(vo, parent);
					}
					else
					{
						if (!(triggerType == "MissionVictory"))
						{
							goto IL_426;
						}
						return new MissionVictoryStoryTrigger(vo, parent);
					}
				}
				else
				{
					if (!(triggerType == "squadUIOpen"))
					{
						goto IL_426;
					}
					return new SquadUIOpenStoryTrigger(vo, parent);
				}
			}
			else if (num <= 3573432780u)
			{
				if (num <= 3174207744u)
				{
					if (num != 2275058931u)
					{
						if (num != 2581225745u)
						{
							if (num != 3174207744u)
							{
								goto IL_426;
							}
							if (!(triggerType == "SquadSize"))
							{
								goto IL_426;
							}
							return new SquadSizeStoryTrigger(vo, parent);
						}
						else
						{
							if (!(triggerType == "EventCounter"))
							{
								goto IL_426;
							}
							return new EventCounterStoryTrigger(vo, parent);
						}
					}
					else
					{
						if (!(triggerType == "WorldLoad"))
						{
							goto IL_426;
						}
						return new WorldLoadStoryTrigger(vo, parent);
					}
				}
				else if (num != 3324871712u)
				{
					if (num != 3358627246u)
					{
						if (num != 3573432780u)
						{
							goto IL_426;
						}
						if (!(triggerType == "PlanetRelocate"))
						{
							goto IL_426;
						}
						return new PlanetRelocateStoryTrigger(vo, parent);
					}
					else
					{
						if (!(triggerType == "BlockingInitWarClick"))
						{
							goto IL_426;
						}
						return new BlockingWarStoryTrigger(vo, parent);
					}
				}
				else
				{
					if (!(triggerType == "HQLevel"))
					{
						goto IL_426;
					}
					return new HQLevelStoryTrigger(vo, parent);
				}
			}
			else if (num <= 3759229686u)
			{
				if (num != 3574966019u)
				{
					if (num != 3713188019u)
					{
						if (num != 3759229686u)
						{
							goto IL_426;
						}
						if (!(triggerType == "squadActiveMembers"))
						{
							goto IL_426;
						}
						return new SquadActiveMembersStoryTrigger(vo, parent);
					}
					else
					{
						if (!(triggerType == "WorldTransitionComplete"))
						{
							goto IL_426;
						}
						return new WorldTransitionCompleteStoryTrigger(vo, parent);
					}
				}
				else
				{
					if (!(triggerType == "UnlockPlanet"))
					{
						goto IL_426;
					}
					return new UnlockPlanetTrigger(vo, parent);
				}
			}
			else if (num != 3776171487u)
			{
				if (num != 3828256924u)
				{
					if (num != 3948127682u)
					{
						goto IL_426;
					}
					if (!(triggerType == "Timer"))
					{
						goto IL_426;
					}
					return new TimerTrigger(vo, parent);
				}
				else if (!(triggerType == "RepairAllComplete"))
				{
					goto IL_426;
				}
			}
			else
			{
				if (!(triggerType == "ClusterORDER"))
				{
					goto IL_426;
				}
				return new ClusterORDERStoryTrigger(vo, parent);
			}
			return new BuildingRepairStoryTrigger(vo, parent);
			IL_426:
			Service.Get<StaRTSLogger>().ErrorFormat("There is no entry in the StoryTriggerFactory for {0}", new object[]
			{
				vo.TriggerType
			});
			return new AbstractStoryTrigger(vo, parent);
		}

		public static IStoryTrigger DeserializeStoryTrigger(object data, ITriggerReactor parent)
		{
			Dictionary<string, object> dictionary = data as Dictionary<string, object>;
			if (!dictionary.ContainsKey("uid"))
			{
				Service.Get<StaRTSLogger>().Error("Quest Deserialization Error: Trigger Uid not found.");
				return null;
			}
			string uid = dictionary["uid"] as string;
			StoryTriggerVO vo = Service.Get<IDataController>().Get<StoryTriggerVO>(uid);
			AbstractStoryTrigger abstractStoryTrigger = StoryTriggerFactory.GenerateStoryTrigger(vo, parent) as AbstractStoryTrigger;
			return abstractStoryTrigger.FromObject(dictionary) as AbstractStoryTrigger;
		}

		public StoryTriggerFactory()
		{
		}

		protected internal StoryTriggerFactory(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryTriggerFactory.DeserializeStoryTrigger(GCHandledObjects.GCHandleToObject(*args), (ITriggerReactor)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryTriggerFactory.GenerateStoryTrigger((StoryTriggerVO)GCHandledObjects.GCHandleToObject(*args), (ITriggerReactor)GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
