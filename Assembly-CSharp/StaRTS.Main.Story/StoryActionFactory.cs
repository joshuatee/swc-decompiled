using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Story.Actions;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story
{
	public static class StoryActionFactory
	{
		public const string ACTIVATE_TRIGGER = "ActivateTrigger";

		public const string ACTIVATE_SAVE_TRIGGER = "ActivateSaveTrigger";

		public const string CLUSTER_AND = "ClusterAND";

		public const string PLAY_AUDIO = "PlayAudio";

		public const string TRANSITION_TO_WORLD = "TransitionToWorld";

		public const string TRANSITION_TO_HOME = "TransitionToHome";

		public const string DELAY = "Delay";

		public const string SHOW_HOLO = "ShowHolo";

		public const string PLAY_HOLO_ANIM = "PlayHoloAnim";

		public const string SHOW_TRANSCRIPT = "ShowTranscript";

		public const string DISPLAY_BUTTON = "DisplayButton";

		public const string HIDE_HOLO = "HideHolo";

		public const string HIDE_TRANSCRIPT = "HideTranscript";

		public const string CONFIGURE_CONTROLS = "ConfigureControls";

		public const string PAUSE_BATTLE = "PauseBattle";

		public const string RESUME_BATTLE = "ResumeBattle";

		public const string ACTIVATE_MISSION = "ActivateMission";

		public const string DEPLOY_BUILDING = "DeployBuilding";

		public const string REMOVE_BUILDING = "RemoveBuilding";

		public const string PRESS_HERE = "PressHere";

		public const string PRESS_HERE_SCREEN = "PressHereScreen";

		public const string CLEAR_PRESS_HERE = "ClearPressHere";

		public const string SHOW_INSTRUCTION = "ShowInstruction";

		public const string MOVE_CAMERA = "MoveCamera";

		public const string DEPLOY_STARFIGHTER = "DeployStarfighter";

		public const string CIRCLE_BUTTON = "CircleButton";

		public const string CLEAR_CIRCLE_BUTTON = "ClearCircleButton";

		public const string DEFEND_BASE = "DefendBase";

		public const string CIRCLE_REGION = "CircleRegion";

		public const string CIRCLE_BUILDING = "CircleBuilding";

		public const string STORE_LOOKUP = "StoreLookup";

		public const string EXIT_EDIT_MODE = "ExitEditMode";

		public const string DESELECT_BUILDING = "DeselectBuilding";

		public const string DISABLE_CLICKS = "DisableClicks";

		public const string DISABLE_GRID_SCROLLING = "DisableGridScrolling";

		public const string ENABLE_CLICKS = "EnableClicks";

		public const string ENABLE_GRID_SCROLLING = "EnableGridScrolling";

		public const string ALLOW_DEPLOY = "AllowDeploy";

		public const string SHOW_CHOOSE_FACTION_SCREEN = "ShowChooseFactionScreen";

		public const string TRAINING_INSTRUCTIONS = "TrainingInstructions";

		public const string HIGHLIGHT_AREA = "HighlightAreaRectangle";

		public const string HIGHLIGHT_BUTTON = "HighlightButton";

		public const string HIGHLIGHT_REGION = "HighlightRegion";

		public const string HIGHLIGHT_BUILDING = "HighlightBuilding";

		public const string CLEAR_HIGHLIGHT = "ClearHighlight";

		public const string SAVE_PROGRESS = "SaveProgress";

		public const string CLEAR_PROGRESS = "ClearProgress";

		public const string DEACTIVATE_TRIGGER = "DeactivateTrigger";

		public const string ZOOM_CAMERA = "ZoomCamera";

		public const string SET_BUILDING_REPAIR_LEVEL_TYPE = "SetBuildingTypeRepairLevel";

		public const string SET_BUILDING_REPAIR_LEVEL_TYPE_AND_AREA = "SetBuildingTypeRepairLevelInArea";

		public const string SET_BUILDING_REPAIR_LEVEL_ALL = "SetAllBuildingsRepairLevel";

		public const string PAUSE_BUILDING_REPAIR = "PauseBuildingRepair";

		public const string UNPAUSE_BUILDING_REPAIR = "UnpauseBuildingRepair";

		public const string HIDE_BUILDING_TOOLTIPS = "HideBuildingTooltips";

		public const string SHOW_BUILDING_TOOLTIPS = "ShowBuildingTooltips";

		public const string DELAY_BLOCKING = "DelayBlocking";

		public const string HIDE_INSTRUCTION = "HideInstruction";

		public const string END_CHAIN = "EndChain";

		public const string SHOW_UI_ELEMENT = "ShowUIElement";

		public const string HIDE_UI_ELEMENT = "HideUIElement";

		public const string SHOW_BUILDING_TOOLTIP_BY_TYPE = "ShowBuildingTooltipByType";

		public const string HIDE_BUILDING_TOOLTIP_BY_TYPE = "HideBuildingTooltipByType";

		public const string START_PLACE_BUILDING = "StartPlaceBuilding";

		public const string SHOW_HOLO_INFO_PANEL = "ShowInfoPanel";

		public const string SHOW_PLANET_INFO_PANEL = "ShowPlanetPanel";

		public const string HIDE_HOLO_INFO_PANEL = "HideInfoPanel";

		public const string SHOW_WHATS_NEXT_SCREEN = "ShowWhatsNextScreen";

		public const string SHOW_SET_CALLSIGN_SCREEN = "ShowSetCallSignScreen";

		public const string SHOW_SET_CALLSIGN_SCREEN_NO_AUTH = "ShowSetCallSignScreenHackNoAuth";

		public const string SET_MUSIC_VOLUME = "SetMusicVolume";

		public const string OPEN_STORE_SCREEN = "OpenStoreScreen";

		public const string DISABLE_CANCEL_BUILDING_PLACEMENT = "DisableCancelBuildingPlacement";

		public const string ENABLE_CANCEL_BUILDING_PLACEMENT = "EnableCancelBuildingPlacement";

		public const string END_FUE = "EndFue";

		public const string SPAWN_DEFENSIVE_TROOP = "SpawnDefensiveTroop";

		public const string PROMPT_PVP = "PromptPvp";

		public const string SHOW_PUSH_NOTIFICATION_SETTINGS_SCREEN = "ShowPushNotificationSettingsScreen";

		public const string SHOW_RATE_MY_APP_SCREEN = "ShowRateMyAppScreen";

		public const string CLEAR_BUILDING_HIGHLIGHT = "ClearBuildingHighlight";

		public const string SHOW_TEXT_CRAWL = "ShowTextCrawl";

		public const string SPIN_PLANET_FORWARD = "SpinPlanetForward";

		public const string REBEL_EMPIRE_FORKING = "RebelEmpireFork";

		public const string SHOW_SCREEN = "ShowScreen";

		public const string CLOSE_SCREEN = "CloseScreen";

		public const string PAN_TO_PLANET = "PanToPlanet";

		public const string EDIT_PREF = "EditPref";

		public const string IF_PREF_GATE = "IfPrefGate";

		public const string IF_MAIN_FUE_GATE = "IfMainFUEGate";

		public const string PLAY_PLANET_INTRO = "PlayPlanetIntro";

		public const string SHOW_WAR_HELP = "ShowWarHelp";

		public static IStoryAction GenerateStoryAction(StoryActionVO vo, IStoryReactor parent)
		{
			string actionType = vo.ActionType;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(actionType);
			if (num <= 2283752179u)
			{
				if (num <= 990447518u)
				{
					if (num <= 591343635u)
					{
						if (num <= 203422667u)
						{
							if (num <= 89752668u)
							{
								if (num != 31010333u)
								{
									if (num != 89752668u)
									{
										goto IL_FC9;
									}
									if (!(actionType == "TransitionToHome"))
									{
										goto IL_FC9;
									}
									return new TransitionToHomeStoryAction(vo, parent);
								}
								else
								{
									if (!(actionType == "HideBuildingTooltipByType"))
									{
										goto IL_FC9;
									}
									return new ShowBuildingTooltipByTypeStoryAction(vo, parent, false);
								}
							}
							else if (num != 122238119u)
							{
								if (num != 143980828u)
								{
									if (num != 203422667u)
									{
										goto IL_FC9;
									}
									if (!(actionType == "HighlightAreaRectangle"))
									{
										goto IL_FC9;
									}
									return new HighlightAreaStoryAction(vo, parent);
								}
								else
								{
									if (!(actionType == "ShowBuildingTooltips"))
									{
										goto IL_FC9;
									}
									return new ShowBuildingTooltipsStoryAction(vo, parent, true);
								}
							}
							else
							{
								if (!(actionType == "EndChain"))
								{
									goto IL_FC9;
								}
								return new EndChainStoryAction(vo, parent);
							}
						}
						else if (num <= 355087249u)
						{
							if (num != 243105477u)
							{
								if (num != 302671759u)
								{
									if (num != 355087249u)
									{
										goto IL_FC9;
									}
									if (!(actionType == "HideBuildingTooltips"))
									{
										goto IL_FC9;
									}
									return new ShowBuildingTooltipsStoryAction(vo, parent, false);
								}
								else if (!(actionType == "HighlightRegion"))
								{
									goto IL_FC9;
								}
							}
							else
							{
								if (!(actionType == "HideUIElement"))
								{
									goto IL_FC9;
								}
								return new ShowUIElementStoryAction(vo, parent, false);
							}
						}
						else if (num != 470960890u)
						{
							if (num != 550270072u)
							{
								if (num != 591343635u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "HideTranscript"))
								{
									goto IL_FC9;
								}
								return new HideTranscriptStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "Delay"))
								{
									goto IL_FC9;
								}
								return new DelayStoryAction(vo, parent);
							}
						}
						else
						{
							if (!(actionType == "ClearBuildingHighlight"))
							{
								goto IL_FC9;
							}
							return new ClearBuildingHighlightStoryAction(vo, parent);
						}
					}
					else if (num <= 763360927u)
					{
						if (num <= 709607536u)
						{
							if (num != 632104463u)
							{
								if (num != 709607536u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "DisableCancelBuildingPlacement"))
								{
									goto IL_FC9;
								}
								return new EnableCancelBuildingPlacementStoryAction(vo, parent, false);
							}
							else
							{
								if (!(actionType == "PromptPvp"))
								{
									goto IL_FC9;
								}
								return new PromptPvpStoryAction(vo, parent);
							}
						}
						else if (num != 757530807u)
						{
							if (num != 760836417u)
							{
								if (num != 763360927u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "ShowChooseFactionScreen"))
								{
									goto IL_FC9;
								}
								return new ShowChooseFactionScreenStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "ShowSetCallSignScreen"))
								{
									goto IL_FC9;
								}
								return new ShowSetCallSignScreenStoryAction(true, vo, parent);
							}
						}
						else
						{
							if (!(actionType == "DelayBlocking"))
							{
								goto IL_FC9;
							}
							return new DelayBlockingStoryAction(vo, parent);
						}
					}
					else if (num <= 892314778u)
					{
						if (num != 878464119u)
						{
							if (num != 887998804u)
							{
								if (num != 892314778u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "IfMainFUEGate"))
								{
									goto IL_FC9;
								}
								return new MainFUEGateStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "ShowInstruction"))
								{
									goto IL_FC9;
								}
								return new ShowInstructionStoryAction(vo, parent);
							}
						}
						else
						{
							if (!(actionType == "HighlightButton"))
							{
								goto IL_FC9;
							}
							goto IL_DAB;
						}
					}
					else if (num != 917229751u)
					{
						if (num != 972608897u)
						{
							if (num != 990447518u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "ShowUIElement"))
							{
								goto IL_FC9;
							}
							return new ShowUIElementStoryAction(vo, parent, true);
						}
						else
						{
							if (!(actionType == "AllowDeploy"))
							{
								goto IL_FC9;
							}
							return new AllowDeployStoryAction(vo, parent);
						}
					}
					else
					{
						if (!(actionType == "ShowRateMyAppScreen"))
						{
							goto IL_FC9;
						}
						return new ShowRateAppScreenStoryAction(vo, parent);
					}
				}
				else if (num <= 1486873892u)
				{
					if (num <= 1229496475u)
					{
						if (num <= 1066278926u)
						{
							if (num != 998551597u)
							{
								if (num != 1066278926u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "ClusterAND"))
								{
									goto IL_FC9;
								}
								return new ClusterANDStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "HideInfoPanel"))
								{
									goto IL_FC9;
								}
								return new HideHoloInfoPanelStoryAction(vo, parent);
							}
						}
						else if (num != 1097608348u)
						{
							if (num != 1163979072u)
							{
								if (num != 1229496475u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "SaveProgress"))
								{
									goto IL_FC9;
								}
								return new SaveProgressStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "ResumeBattle"))
								{
									goto IL_FC9;
								}
								return new ResumeBattleStoryAction(vo, parent);
							}
						}
						else
						{
							if (!(actionType == "PauseBuildingRepair"))
							{
								goto IL_FC9;
							}
							return new PauseBuildingRepairStoryAction(vo, parent);
						}
					}
					else if (num <= 1310760974u)
					{
						if (num != 1247491030u)
						{
							if (num != 1262831148u)
							{
								if (num != 1310760974u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "ExitEditMode"))
								{
									goto IL_FC9;
								}
								return new ExitEditModeStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "ActivateTrigger"))
								{
									goto IL_FC9;
								}
								goto IL_D1B;
							}
						}
						else
						{
							if (!(actionType == "ShowPushNotificationSettingsScreen"))
							{
								goto IL_FC9;
							}
							return new ShowPushNotificationsSettingsScreenStoryAction(vo, parent);
						}
					}
					else if (num != 1332858228u)
					{
						if (num != 1359443283u)
						{
							if (num != 1486873892u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "SpinPlanetForward"))
							{
								goto IL_FC9;
							}
							return new SpinPlanetForwardStoryAction(vo, parent);
						}
						else
						{
							if (!(actionType == "HighlightBuilding"))
							{
								goto IL_FC9;
							}
							goto IL_E05;
						}
					}
					else
					{
						if (!(actionType == "SetBuildingTypeRepairLevel"))
						{
							goto IL_FC9;
						}
						goto IL_E7D;
					}
				}
				else if (num <= 1953087653u)
				{
					if (num <= 1924668370u)
					{
						if (num != 1529769207u)
						{
							if (num != 1886357747u)
							{
								if (num != 1924668370u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "EndFue"))
								{
									goto IL_FC9;
								}
								return new EndFueStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "SpawnDefensiveTroop"))
								{
									goto IL_FC9;
								}
								return new SpawnDefensiveTroopStoryAction(vo, parent);
							}
						}
						else
						{
							if (!(actionType == "EnableClicks"))
							{
								goto IL_FC9;
							}
							return new EnableClicksStoryAction(vo, parent);
						}
					}
					else if (num != 1925637569u)
					{
						if (num != 1936643325u)
						{
							if (num != 1953087653u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "TransitionToWorld"))
							{
								goto IL_FC9;
							}
							return new TransitionToWorldStoryAction(vo, parent);
						}
						else
						{
							if (!(actionType == "ConfigureControls"))
							{
								goto IL_FC9;
							}
							return new ConfigureControlsStoryAction(vo, parent);
						}
					}
					else
					{
						if (!(actionType == "HideHolo"))
						{
							goto IL_FC9;
						}
						return new HideHologramStoryAction(vo, parent);
					}
				}
				else if (num <= 2085162650u)
				{
					if (num != 1959445457u)
					{
						if (num != 2062624914u)
						{
							if (num != 2085162650u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "StartPlaceBuilding"))
							{
								goto IL_FC9;
							}
							return new StartPlaceBuildingStoryAction(vo, parent);
						}
						else
						{
							if (!(actionType == "DisableClicks"))
							{
								goto IL_FC9;
							}
							return new DisableClicksStoryAction(vo, parent);
						}
					}
					else if (!(actionType == "CircleRegion"))
					{
						goto IL_FC9;
					}
				}
				else if (num != 2157143007u)
				{
					if (num != 2282072399u)
					{
						if (num != 2283752179u)
						{
							goto IL_FC9;
						}
						if (!(actionType == "MoveCamera"))
						{
							goto IL_FC9;
						}
						return new MoveCameraStoryAction(vo, parent);
					}
					else
					{
						if (!(actionType == "PlayAudio"))
						{
							goto IL_FC9;
						}
						return new PlayAudioStoryAction(vo, parent);
					}
				}
				else
				{
					if (!(actionType == "DeployStarfighter"))
					{
						goto IL_FC9;
					}
					return new DeployStarshipAttackStoryAction(vo, parent);
				}
				return new CircleRegionStoryAction(vo, parent);
			}
			if (num <= 3398906397u)
			{
				if (num <= 2913054206u)
				{
					if (num <= 2536933583u)
					{
						if (num <= 2383160428u)
						{
							if (num != 2300225815u)
							{
								if (num != 2383160428u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "EditPref"))
								{
									goto IL_FC9;
								}
								return new EditPrefStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "ClearPressHere"))
								{
									goto IL_FC9;
								}
								return new ClearPressHereStoryAction(vo, parent);
							}
						}
						else if (num != 2460641462u)
						{
							if (num != 2489943886u)
							{
								if (num != 2536933583u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "PauseBattle"))
								{
									goto IL_FC9;
								}
								return new PauseBattleStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "ShowInfoPanel"))
								{
									goto IL_FC9;
								}
								return new ShowHologramInfoStoryAction(vo, parent, false);
							}
						}
						else
						{
							if (!(actionType == "SetBuildingTypeRepairLevelInArea"))
							{
								goto IL_FC9;
							}
							goto IL_E7D;
						}
					}
					else if (num <= 2746445930u)
					{
						if (num != 2557306625u)
						{
							if (num != 2591652078u)
							{
								if (num != 2746445930u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "DisableGridScrolling"))
								{
									goto IL_FC9;
								}
								return new DisableGridScrollingStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "PressHereScreen"))
								{
									goto IL_FC9;
								}
								return new PressHereStoryAction(vo, parent, true);
							}
						}
						else
						{
							if (!(actionType == "CircleBuilding"))
							{
								goto IL_FC9;
							}
							goto IL_E05;
						}
					}
					else if (num != 2819298322u)
					{
						if (num != 2848964266u)
						{
							if (num != 2913054206u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "ClearHighlight"))
							{
								goto IL_FC9;
							}
						}
						else
						{
							if (!(actionType == "ShowWhatsNextScreen"))
							{
								goto IL_FC9;
							}
							return new ShowWhatsNextScreenStoryAction(vo, parent);
						}
					}
					else
					{
						if (!(actionType == "TrainingInstructions"))
						{
							goto IL_FC9;
						}
						return new TrainingInstructionsStoryAction(vo, parent);
					}
				}
				else if (num <= 3039121289u)
				{
					if (num <= 2943842346u)
					{
						if (num != 2934331024u)
						{
							if (num != 2943250752u)
							{
								if (num != 2943842346u)
								{
									goto IL_FC9;
								}
								if (!(actionType == "DeployBuilding"))
								{
									goto IL_FC9;
								}
								return new DeployBuildingStoryAction(vo, parent);
							}
							else
							{
								if (!(actionType == "SetMusicVolume"))
								{
									goto IL_FC9;
								}
								return new SetMusicVolumeStoryAction(vo, parent);
							}
						}
						else
						{
							if (!(actionType == "ShowHolo"))
							{
								goto IL_FC9;
							}
							return new ShowHologramStoryAction(vo, parent);
						}
					}
					else if (num != 2978252339u)
					{
						if (num != 3011974379u)
						{
							if (num != 3039121289u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "ShowWarHelp"))
							{
								goto IL_FC9;
							}
							return new OpenWarInfoStoryAction(vo, parent);
						}
						else
						{
							if (!(actionType == "ActivateSaveTrigger"))
							{
								goto IL_FC9;
							}
							goto IL_D1B;
						}
					}
					else
					{
						if (!(actionType == "PanToPlanet"))
						{
							goto IL_FC9;
						}
						return new PanToPlanetStoryAction(vo, parent);
					}
				}
				else if (num <= 3191378542u)
				{
					if (num != 3077559665u)
					{
						if (num != 3116084834u)
						{
							if (num != 3191378542u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "PlayHoloAnim"))
							{
								goto IL_FC9;
							}
							return new PlayHoloAnimationStoryAction(vo, parent);
						}
						else
						{
							if (!(actionType == "ShowBuildingTooltipByType"))
							{
								goto IL_FC9;
							}
							return new ShowBuildingTooltipByTypeStoryAction(vo, parent, true);
						}
					}
					else
					{
						if (!(actionType == "ZoomCamera"))
						{
							goto IL_FC9;
						}
						return new ZoomCameraStoryAction(vo, parent);
					}
				}
				else if (num != 3376906467u)
				{
					if (num != 3395551422u)
					{
						if (num != 3398906397u)
						{
							goto IL_FC9;
						}
						if (!(actionType == "CircleButton"))
						{
							goto IL_FC9;
						}
						goto IL_DAB;
					}
					else
					{
						if (!(actionType == "ActivateMission"))
						{
							goto IL_FC9;
						}
						return new ActivateMissionStoryAction(vo, parent);
					}
				}
				else
				{
					if (!(actionType == "EnableCancelBuildingPlacement"))
					{
						goto IL_FC9;
					}
					return new EnableCancelBuildingPlacementStoryAction(vo, parent, true);
				}
			}
			else if (num <= 3865718972u)
			{
				if (num <= 3562203881u)
				{
					if (num <= 3417656017u)
					{
						if (num != 3399402465u)
						{
							if (num != 3417656017u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "EnableGridScrolling"))
							{
								goto IL_FC9;
							}
							return new EnableGridScrollingStoryAction(vo, parent);
						}
						else
						{
							if (!(actionType == "DisplayButton"))
							{
								goto IL_FC9;
							}
							return new DisplayButtonStoryAction(vo, parent);
						}
					}
					else if (num != 3433966887u)
					{
						if (num != 3542354334u)
						{
							if (num != 3562203881u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "UnpauseBuildingRepair"))
							{
								goto IL_FC9;
							}
							return new UnpauseBuildingRepairStoryAction(vo, parent);
						}
						else
						{
							if (!(actionType == "OpenStoreScreen"))
							{
								goto IL_FC9;
							}
							return new OpenStoreScreenStoryAction(vo, parent);
						}
					}
					else
					{
						if (!(actionType == "HideInstruction"))
						{
							goto IL_FC9;
						}
						return new HideInstructionStoryAction(vo, parent);
					}
				}
				else if (num <= 3654862876u)
				{
					if (num != 3565370429u)
					{
						if (num != 3595237455u)
						{
							if (num != 3654862876u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "IfPrefGate"))
							{
								goto IL_FC9;
							}
							return new IfPrefGateStoryAction(vo, parent);
						}
						else
						{
							if (!(actionType == "RebelEmpireFork"))
							{
								goto IL_FC9;
							}
							return new RebelEmpireForkingStoryAction(vo, parent);
						}
					}
					else
					{
						if (!(actionType == "ShowSetCallSignScreenHackNoAuth"))
						{
							goto IL_FC9;
						}
						return new ShowSetCallSignScreenStoryAction(false, vo, parent);
					}
				}
				else if (num != 3731537560u)
				{
					if (num != 3768924688u)
					{
						if (num != 3865718972u)
						{
							goto IL_FC9;
						}
						if (!(actionType == "ShowPlanetPanel"))
						{
							goto IL_FC9;
						}
						return new ShowHologramInfoStoryAction(vo, parent, true);
					}
					else
					{
						if (!(actionType == "SetAllBuildingsRepairLevel"))
						{
							goto IL_FC9;
						}
						goto IL_E7D;
					}
				}
				else
				{
					if (!(actionType == "ShowScreen"))
					{
						goto IL_FC9;
					}
					return new ShowScreenStoryAction(vo, parent);
				}
			}
			else if (num <= 3982745782u)
			{
				if (num <= 3938612029u)
				{
					if (num != 3920669632u)
					{
						if (num != 3937246294u)
						{
							if (num != 3938612029u)
							{
								goto IL_FC9;
							}
							if (!(actionType == "DeactivateTrigger"))
							{
								goto IL_FC9;
							}
							return new DeactivateTriggerStoryAction(vo, parent);
						}
						else
						{
							if (!(actionType == "ShowTextCrawl"))
							{
								goto IL_FC9;
							}
							return new TextCrawlStoryAction(vo, parent);
						}
					}
					else
					{
						if (!(actionType == "StoreLookup"))
						{
							goto IL_FC9;
						}
						return new StoreLookupStoryAction(vo, parent);
					}
				}
				else if (num != 3948763934u)
				{
					if (num != 3978060781u)
					{
						if (num != 3982745782u)
						{
							goto IL_FC9;
						}
						if (!(actionType == "ShowTranscript"))
						{
							goto IL_FC9;
						}
						return new ShowTranscriptStoryAction(vo, parent);
					}
					else
					{
						if (!(actionType == "RemoveBuilding"))
						{
							goto IL_FC9;
						}
						return new RemoveBuildingStoryAction(vo, parent);
					}
				}
				else
				{
					if (!(actionType == "DeselectBuilding"))
					{
						goto IL_FC9;
					}
					return new DeselectBuildingStoryAction(vo, parent);
				}
			}
			else if (num <= 4206932450u)
			{
				if (num != 4118311817u)
				{
					if (num != 4123384274u)
					{
						if (num != 4206932450u)
						{
							goto IL_FC9;
						}
						if (!(actionType == "ClearCircleButton"))
						{
							goto IL_FC9;
						}
					}
					else
					{
						if (!(actionType == "PressHere"))
						{
							goto IL_FC9;
						}
						return new PressHereStoryAction(vo, parent, false);
					}
				}
				else
				{
					if (!(actionType == "PlayPlanetIntro"))
					{
						goto IL_FC9;
					}
					return new PlayPlanetIntroStoryAction(vo, parent);
				}
			}
			else if (num != 4223297341u)
			{
				if (num != 4260681439u)
				{
					if (num != 4278333676u)
					{
						goto IL_FC9;
					}
					if (!(actionType == "DefendBase"))
					{
						goto IL_FC9;
					}
					return new DefendBaseStoryAction(vo, parent);
				}
				else
				{
					if (!(actionType == "CloseScreen"))
					{
						goto IL_FC9;
					}
					return new CloseScreenStoryAction(vo, parent);
				}
			}
			else
			{
				if (!(actionType == "ClearProgress"))
				{
					goto IL_FC9;
				}
				return new ClearProgressStoryAction(vo, parent);
			}
			return new ClearButtonCircleStoryAction(vo, parent);
			IL_D1B:
			return new ActivateTriggerStoryAction(vo, parent);
			IL_DAB:
			return new CircleButtonStoryAction(vo, parent);
			IL_E05:
			return new CircleBuildingStoryAction(vo, parent);
			IL_E7D:
			return new SetBuildingRepairStateStoryAction(vo, parent);
			IL_FC9:
			Service.Get<StaRTSLogger>().ErrorFormat("There is no entry in the StoryActionFactory for {0}", new object[]
			{
				vo.ActionType
			});
			return new DegenerateStoryAction(vo, parent);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryActionFactory.GenerateStoryAction((StoryActionVO)GCHandledObjects.GCHandleToObject(*args), (IStoryReactor)GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
