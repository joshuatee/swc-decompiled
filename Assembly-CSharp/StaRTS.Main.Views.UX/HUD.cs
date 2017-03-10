using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.Externals.BI;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Controllers.Objectives;
using StaRTS.Main.Controllers.Performance;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Commands.Pvp;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Anchors;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.UX.Screens.Leaderboard;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using StaRTS.Main.Views.UX.Screens.Squads;
using StaRTS.Utils;
using StaRTS.Utils.Animation;
using StaRTS.Utils.Animation.Anims;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX
{
	public class HUD : UXFactory, IEventObserver, IPerformanceObserver, IViewClockTimeObserver, IViewFrameTimeObserver
	{
		public const string CURRENCY_GROUP = "Currency";

		public const string OPPONENT_GROUP = "OpponentInfo";

		public const string OPPONENT_SYMBOL = "SpriteSymbolOpponent";

		public const string OPPONENT_SYMBOL_UPGRADE_REBEL = "SpriteSymbolOpponentFactionUp";

		public const string OPPONENT_SYMBOL_UPGRADE_EMPIRE = "SpriteSymbolOpponentFactionUpEmp";

		public const string PREBATTLE_MEDALS_GROUP = "MedalInfo";

		public const string PLAYER_GROUP = "PlayerInfo";

		public const string SHIELD_GROUP = "Shield";

		public const string SPECIAL_PROMOTION_GROUP = "SpecialPromo";

		public const string BASE_RATING_BUTTON = "BaseRating";

		public const string MEDALS_BUTTON = "Medals";

		private const string MEDALS_LABEL = "LabelMedals";

		private const string CREDITS_SLIDER = "PBarCurrency";

		private const string MATERIAL_SLIDER = "PBarMaterial";

		private const string CONTRABAND_SLIDER = "PBarContraband";

		public const string CREDITS_LABEL = "LabelCurrencyValueHome";

		public const string MATERIAL_LABEL = "LabelMaterialValueHome";

		public const string CONTRABAND_LABEL = "LabelContrabandValueHome";

		public const string CRYSTALS_DROIDS_GROUP = "CrystalsDroids";

		public const string CRYSTAL_BUTTON = "Crystals";

		private const string CRYSTAL_LABEL = "LabelCrystalsValueHome";

		private const string CREDITS_ICON_ANIMATOR = "Credits";

		private const string MATERIALS_ICON_ANIMATOR = "Materials";

		private const string CONTRABAND_ICON_ANIMATOR = "Contraband";

		private const string CRYSTALS_ICON_ANIMATOR = "Crystals";

		public const string DROID_BUTTON = "Droids";

		private const string DROID_ADD_LABEL = "LabelDroidsAdd";

		private const string DROID_MAX_LABEL = "LabelDroidsMax";

		private const string DROID_ADD_GROUP = "DroidsAdd";

		private const string DROID_MAX_GROUP = "DroidsMax";

		private const string SHIELD_LABEL = "LabelShield";

		public const string NEXT_BATTLE_BUTTON = "ButtonNextBattle";

		public const string NEXT_BATTLE_COST_GROUP = "CostNextBattle";

		public const string HOME_BUTTON = "ButtonHome";

		public const string EDIT_BUTTON = "ButtonEdit";

		public const string EXIT_EDIT_BUTTON = "ButtonExitEdit";

		public const string EXIT_EDIT_ANIMATION = "ButtonExitEditHolder";

		public const string STORE_BUTTON = "ButtonStore";

		private const string CONTEXT_BUTTON_PARENT = "ButtonContextParent";

		public const string CONTEXT_BUTTON = "ButtonContext";

		private const string CONTEXT_BUTTON_DIM = "ButtonContextDim";

		private const string CONTEXT_BUTTON_LABEL = "LabelContext";

		private const string CONTEXT_BUTTON_BACKGROUND = "BackgroundContext";

		private const string CONTEXT_BUTTON_BACKGROUND_SPRITE = "context_button";

		private const string CONTEXT_DESC_LABEL = "LabelContextDescription";

		private const string STASH_CONTEXT_LOCATOR = "StashContextLocator";

		private const string CONTEXT_COST_GROUP = "Cost";

		private const string CONTEXT_HARDCOST_LABEL = "LabelHardCost";

		private const string CONTEXT_ICON_SPRITE = "SpriteContextIcon";

		private const string CONTEXT_JEWEL_CONTAINER = "ContainerJewelContext";

		private const string CONTEXT_JEWEL_COUNT_LABEL = "LabelMessageCountContext";

		public const string BATTLE_BUTTON = "ButtonBattle";

		public const string WAR_BUTTON = "ButtonWar";

		public const string WAR_BUTTON_LABEL = "LabelWar";

		public const string WAR_BUTTON_PHASE_PREP = "WarPrep";

		public const string WAR_BUTTON_PHASE_ACTION = "WarAction";

		public const string WAR_BUTTON_PHASE_COOLDOWN = "WarReward";

		public const string WAR_BUTTON_TUTORIAL = "WarTutorial";

		public const string LOG_BUTTON = "ButtonLog";

		public const string LEADERBOARD_BUTTON = "ButtonLeaderboard";

		public const string HOLONET_BUTTON = "Newspaper";

		public const string SETTINGS_BUTTON = "ButtonSettings";

		public const string SQUADS_BUTTON = "ButtonClans";

		public const string SHIELD_BUTTON = "Shield";

		public const string SQUADS_BUTTON_LABEL = "LabelClans";

		public const string END_BATTLE_BUTTON = "ButtonEndBattle";

		public const string TARGETED_BUNDLE_BUTTON = "SpecialPromo";

		public const string TARGETED_BUNDLE_TEXTURE = "TextureSpecialPromo";

		public const string TROOP_GRID = "TroopsGrid";

		private const string TROOP_TEMPLATE = "TroopTemplate";

		public const string TROOP_CHECKBOX = "CheckboxTroop";

		public const string ABILITY_PREPARED_ELEMENT = "HeroAbilityActive";

		public const string ABILITY_READY_FX_ELEMENT = "HeroReady";

		private const string TROOP_LEVEL = "LabelTroopLevel";

		private const string TROOP_QUALITY_DEFAULT = "TroopFrameBgDefault";

		private const string TROOP_QUALITY_PREFIX = "TroopFrameBgQ{0}";

		private const string PROVIDED_TROOP_LEVEL = "LabelProvidedTroopLevel";

		private const string TROOP_SPRITE_DIM = "SpriteTroopDim";

		private const string PROVIDED_TROOP_SPRITE_DIM = "SpriteProvidedTroopDim";

		private const string TROOP_PROVIDED_FRAME_GROUP = "ProvidedFrame";

		private const string TROOP_PROVIDED_FRAME_DEFAULT = "ProvidedFrameDefault";

		private const string TROOP_PROVIDED_FRAME_QUAILTY_FORMAT = "ProvidedFrameQ{0}";

		private const string TROOP_STANDARD_FRAME = "StandardFrame";

		private const string TROOP_SPRITE_PROVIDED_SELECTED = "SpriteProvidedSelected";

		private const string TROOP_GLINT_BOTTOM = "SpriteTroopGlintBottom";

		private const string TROOP_ICON_SPRITE = "SpriteTroop";

		private const string SQUAD_ICON_SPRITE = "SpriteSquad";

		private const string TROOP_COUNT_LABEL = "LabelQuantity";

		private const string PROVIDED_TROOP_COUNT_LABEL = "LabelProvidedQuantity";

		public const string TIME_LEFT_LABEL = "LabelTimeLeft";

		private const string DAMAGE_STAR_ANCHOR = "BattleStarsRewards";

		private const string DAMAGE_STAR_ANIMATOR = "RewardStarHolder";

		private const string DAMAGE_STAR_LABEL = "LabelRewardStar";

		public const string DAMAGE_STAR_GROUP = "DamageStars";

		private const string DAMAGE_STAR_1 = "SpriteStar1";

		private const string DAMAGE_STAR_2 = "SpriteStar2";

		private const string DAMAGE_STAR_3 = "SpriteStar3";

		private const string DAMAGE_VALUE_LABEL = "LabelPercent";

		public const string BASE_NAME_LABEL = "LabelBaseNameOpponent";

		private const string LOOT_GROUP = "AvailableLoot";

		public const string LOOT_CREDIT_LABEL = "LabelCurrencyValueOpponent";

		private const string LOOT_MATERIAL_LABEL = "LabelMaterialsValueOpponent";

		private const string LOOT_CONTRABAND_ICON = "SpriteOpponentContraband";

		private const string LOOT_CONTRABAND_LABEL = "LabelContrabandValueOpponent";

		public const string MEDALS_GAIN_LABEL = "LabelMedalsValueOpponent";

		public const string MEDALS_LOSE_LABEL = "LabelDefeatMedals";

		private const string TOURNAMENT_RATING_GAIN_GROUP = "TournamentMedals";

		private const string TOURNAMENT_RATING_GAIN_LABEL = "LabelDefeatTournament";

		private const string TOURNAMENT_RATING_GAIN_SPRITE = "SpriteIcoTournamentMedals";

		private const string TOURNAMENT_RATING_LOSE_GROUP = "TournamentMedalsDefeat";

		private const string TOURNAMENT_RATING_LOSE_LABEL = "LabelDefeatTournamentMedals";

		private const string TOURNAMENT_RATING_LOSE_SPRITE = "SpriteIcoTournamentMedalsDefeat";

		private const string RANK_LABEL = "LabelTrophies";

		private const string RANK_LABEL_OPPONENT = "LabelTrophiesOpponent";

		private const string RANK_SPRITE_BG = "BaseScoreBkgOpponent";

		private const string PRE_COMBAT_COUNTDOWN_GROUP = "PrecombatCountdown";

		private const string PRE_COMBAT_TIME_LABEL = "LabelCount";

		private const string PRE_COMBAT_COUNTDOWN_FILL = "CountdownFill";

		private const string PRE_COMBAT_BEGINS_LABEL = "LabelBattleBegins";

		private const string PRE_COMBAT_GOAL_LABEL = "LabelGoal";

		public const string DEPLOY_INSTRUCTIONS_LABEL = "LabelDeployInstructions";

		public const string REPLAY_CONTROLS_GROUP = "ReplayControls";

		private const string REPLAY_CHANGE_SPEED_BUTTON = "ButtonReplaySpeed";

		private const string REPLAY_CURRENT_SPEED_LABEL = "LabelReplaySpeed";

		private const string REPLAY_ENDS_IN_LABEL = "LabelReplayEndsIn";

		private const string REPLAY_TIME_LABEL = "LabelReplayTime";

		private const string PLAYER_NAME_LABEL = "LabelPlayerName";

		private const string PLAYER_CLAN_LABEL = "LabelClanName";

		private const string PLAYER_BASE_SCORE_JEWEL = "ContainerJewelBaseRating";

		private const string TARGETED_BUNDLE_LABEL = "LabelSpecialPromo";

		private const string TARGETED_BUNDLE_LABEL_TIMER = "LabelSpecialPromoTimer";

		private const string EXPIRES_IN = "expires_in";

		private const string EQUIPMENT_FX = "EquipmentFX";

		public const string PLAYER_BATTLE_WARBUFF = "BuffsYoursSquadWars";

		public const string OPPONENENT_BATTLE_WARBUFF = "BuffsOpponentsSquadWars";

		private const string LABEL_YOUR_WAR_BUFFS = "LabelBuffsYoursSquadWars";

		private const string GROUP_YOUR_WAR_BUFFS = "PanelBuffsYoursSquadWars";

		private const string GRID_YOUR_WAR_BUFFS = "GridBuffsYoursSquadWars";

		private const string TEMPLATE_SPRITE_YOUR_WAR_BUFFS = "SpriteBuffsYoursSquadWars";

		private const string GROUP_OPPONENT_WAR_BUFFS = "PanelBuffsOpponentSquadWars";

		private const string GRID_OPPONENT_WAR_BUFFS = "GridBuffsOpponentSquadWars";

		private const string TEMPLATE_SPRITE_OPPONENT_WAR_BUFFS = "SpriteBuffOpponentSquadWars";

		private const string YOUR_BUFFS_TITLE = "WAR_BATTLE_CURRENT_ADVANTAGES";

		private const string YOUR_BUFFS_BUTTON = "ContainerBuffsYoursSquadWars";

		private const string OPPONENTS_BUFFS_BUTTON = "ContainerBuffsOpponentSquadWars";

		private const string FACTION_BACKGROUND = "TrophiesBkg";

		private const string FACTION_BACKGROUND_UPGRADE_REBEL = "TrophiesBkgFactionUp";

		private const string FACTION_BACKGROUND_UPGRADE_EMPIRE = "TrophiesBkgFactionUpEmp";

		private const string FACTION_SPRITE_DEFAULT = "HudXpBg";

		private const string BUTTON_NEXT_BATTLE_HOLDER = "ButtonNextBattleHolder";

		private const string DAMAGE_STARS_HOLDER = "DamageStarsHolder";

		public const string NEIGHBOR_GROUP = "FriendInfo";

		private const string NEIGHBOR_MEDALS_LABEL = "LabelFriendMedals";

		private const string NEIGHBOR_SQUAD_NAME_LABEL = "LabelFriendClanName";

		private const string NEIGHBOR_NAME_LABEL = "LabelFriendName";

		private const string NEIGHBOR_TROPHIES_LABEL = "LabelFriendTrophies";

		private const string NEIGHBOR_FACTION_BACKGROUND = "TrophiesBkgFriend";

		private const string NEIGHBOR_FACTION_BACKGROUND_UPGRADE_REBEL = "TrophiesBkgFriendFactionUp";

		private const string NEIGHBOR_FACTION_BACKGROUND_UPGRADE_EMPIRE = "TrophiesBkgFriendFactionUpEmp";

		private const string DEPLOYABLE_BKG_SPRITE = "SpriteTroopBkg";

		private const string PROVIDED_DEPLOYABLE_BKG_SPRITE = "SpriteProvidedTroopBkg";

		private const string TROOP_BKG = "troop_bkg";

		private const string STARSHIP_BKG = "starship_bkg";

		private const string HERO_BKG = "hero_bkg";

		private const string CHAMPION_BKG = "champion_bkg";

		private const string FPS_NAME = "FPS";

		private const string FRAMETIME_NAME = "FrameTime";

		private const string FPEAK_NAME = "FPeak";

		private const string MEM_RSVD_NAME = "Rsvd";

		private const string MEM_USED_NAME = "Used";

		private const string MEM_MESH_NAME = "Mesh";

		private const string MEM_TEXTURE_NAME = "Texture";

		private const string MEM_AUDIO_NAME = "Audio";

		private const string MEM_ANIMATION_NAME = "Animation";

		private const string MEM_MATERIALS_NAME = "Material";

		private const string MEM_DEVICE_NAME = "DeviceMem";

		private const float LOOT_ICON_DEFAULT_SCALE = 32f;

		private const float LOOT_ICON_BUMP_SCALE = 40f;

		private const float LOOT_ICON_BUMP_TIME = 0.05f;

		private const float LOOT_ICON_BUMP_TIME_RESET = 0.2f;

		private const float CONTROL_ANIMATION_SPEED_DEFAULT = 1f;

		private const float CONTROL_ANIMATION_SPEED_IMMEDIATE = 100f;

		private const float CONTEXT_ANIMATION_DURATION_IN = 0.5f;

		private const float CONTEXT_ANIMATION_DURATION_OUT = 0.1f;

		private const string SHOW_PROMO_BUTTON_GLOW = "ShowEffect";

		private const string STOP_PROMO_BUTTON_GLOW = "StopEffect";

		private const string CLEAR_PROMO_BUTTON_GLOW = "ClearEffect";

		private const string SQUAD_INTRO_VIEWED_PREF = "1";

		public const string SQUAD_TROOPS_ID = "squadTroops";

		public const string SQUAD_SCREEN = "SquadScreen";

		public const string WAR_ATTACK = "WarAttack";

		public const string WAR_ATTACK_OPPONENT = "WarAttackOpponent";

		public const string WAR_ATTACK_STARTED = "WarAttackStarted";

		private const string WAR_ATTACK_BUTTON = "BtnSquadwarAttack";

		private const string WAR_ATTACK_LABEL = "LabelBtnSquadwarAttack";

		private const string WAR_ATTACK_TEXT = "WAR_START_ATTACK";

		private const string WAR_DONE_BUTTON = "BtnSquadwarBack";

		private const string WAR_DONE_LABEL = "LabelBtnSquadwarBack";

		private const string WAR_DONE_TEXT = "WAR_SCOUT_CANCEL";

		private const string WAR_UPLINKS_LABEL = "LabelAvailableUplinks";

		private const string WAR_UPLINKS_CONTAINER = "AvailableUplinks";

		private const string WAR_UPLINKS_ICON_PREFIX = "SpriteUplink";

		private const string WAR_UPLINKS_TEXT = "WAR_SCOUT_POINTS_LEFT";

		private const string WAR_EXCLAMATION = "WAR_EXCLAMATION";

		private const string WAR_BUTTON_OPEN_PHASE = "WAR_BUTTON_OPEN_PHASE";

		private const string WAR_BUTTON_PREP_PHASE = "WAR_BUTTON_PREP_PHASE";

		private const string WAR_BUTTON_ACTION_PHASE = "WAR_BUTTON_ACTION_PHASE";

		private const string WAR_BUTTON_COOLDOWN_PHASE = "WAR_BUTTON_COOLDOWN_PHASE";

		private const string WAR_ATTACK_BUFF_BASE_NOT_UNLOCKED_TITLE = "WAR_ATTACK_BUFF_BASE_NOT_UNLOCKED_TITLE";

		private const string WAR_ATTACK_BUFF_BASE_NOT_UNLOCKED_MESSAGE = "WAR_ATTACK_BUFF_BASE_NOT_UNLOCKED_MESSAGE";

		private const int MAX_COST_LABEL_WIDTH = 108;

		private const int MAX_TIMER_LABEL_WIDTH = 108;

		private string FACTION_FLIP_ALERT_TITLE;

		private string FACTION_FLIP_ALERT_DESC;

		private const string RAID_ICON_DEFEND = "icoDefend";

		private const string RAID_DEFEND_BG = "BtnTroopBg_Gold";

		private const string RAID_ICON_INFO = "context_NightRaid";

		private const string SQUAD_ACCEPTED_MESSAGE = "SQUAD_ACCEPTED_MESSAGE";

		private const string TROOPS_RECEIVED_FROM = "TROOPS_RECEIVED_FROM";

		private const string WAR_INTERSTITIAL_TITLE = "WAR_INTERSTITIAL_TITLE";

		private const string WAR_INTERSTITIAL_MESSAGE = "WAR_INTERSTITIAL_MESSAGE";

		private const string LIMITED_EDITION_JEWEL = "LIMITED_EDITION_JEWEL";

		private const string ACTION_TYPE_BUYOUT_RESEARCH = "speed_up_research";

		private const string ACTION_TYPE_BUYOUT_BUILDING = "speed_up_building";

		private const string ACTION_TYPE_BUYOUT_BUILDING_FUE = "FUE_speed_up_building";

		private readonly string[] ANIMATION_TRANSITION_WHITE_LIST;

		private UXElement currencyGroup;

		private UXElement opponentGroup;

		private UXSprite opponentSymbol;

		private UXSprite opponentSymbolUpgradeRebel;

		private UXSprite opponentSymbolUpgradeEmpire;

		private UXElement prebattleMedalsGroup;

		private UXElement playerGroup;

		private UXElement shieldGroup;

		private UXElement specialPromotionGroup;

		private UXButton baseRatingButton;

		private UXButton medalsButton;

		private UXSprite factionBackground;

		private UXSprite factionBackgroundUpgradeRebel;

		private UXLabel playerNameLabel;

		private UXLabel playerClanLabel;

		private UXElement crystalsDroidsGroup;

		private UXButton crystalButton;

		private UXButton droidButton;

		private UXLabel droidAddLabel;

		private UXLabel droidMaxLabel;

		private UXElement droidAddGroup;

		private UXElement droidMaxGroup;

		private UXLabel playerRankLabel;

		private UXLabel playerMedalLabel;

		private UXLabel opponentRankLabel;

		private UXSprite opponentRankBG;

		private UXLabel protectionLabel;

		private UXButton protectionButton;

		private UXButton nextBattleButton;

		private UXButton startBattleButton;

		private UXButton battleButton;

		private UXButton warButton;

		private UXButton warAttackButton;

		private UXButton targetedBundleButton;

		private bool targetedBundleGlowShown;

		private Animator targetedBundleButtonGlowAnim;

		private uint targetedBundleGlowTimerID;

		private UXLabel warAttackLabel;

		private UXButton warDoneButton;

		private UXElement warUplinks;

		private UXLabel battleButtonLabel;

		private UXButton logButton;

		private UXButton leaderboardButton;

		private UXButton holonetButton;

		private UXButton settingsButton;

		private UXButton joinSquadButton;

		private UXLabel squadsButtonLabel;

		private UXButton homeButton;

		private UXButton editButton;

		private UXButton exitEditButton;

		private Animation exitEditAnimation;

		private UXButton storeButton;

		private UXLabel storeButtonLabel;

		private UXButton contextButtonTemplate;

		private UXElement contextButtonParentTemplate;

		private List<Anim> curAnims;

		private List<UXElement> contextButtons;

		private Dictionary<string, UXElement> contextButtonPool;

		private UXLabel contextDescLabel;

		private UXButton endBattleButton;

		private UXLabel timeLeftLabel;

		private UXLabel deployInstructionsLabel;

		private UXLabel baseNameLabel;

		private UXGrid troopGrid;

		private UXElement neighborGroup;

		private UXLabel neighborNameLabel;

		private UXLabel neighborSquadLabel;

		private UXLabel neighborMedalsLabel;

		private UXLabel neighborTrophiesLabel;

		private UXSprite neighborFactionBackground;

		private UXSprite neighborFactionBackgroundUpgradeRebel;

		private JewelControl clansJewel;

		private JewelControl logJewel;

		private JewelControl storeJewel;

		private JewelControl leiJewel;

		private JewelControl battleJewel;

		private JewelControl holonetJewel;

		private JewelControl warJewel;

		private JewelControl warPrepJewel;

		private UXElement preCombatGroup;

		private UXLabel preCombatBeginsLabel;

		private UXLabel preCombatGoalLabel;

		private UXLabel preCombatTimeLabel;

		private UXSprite preCombatCountdownFill;

		private UXElement damageStarAnchor;

		private UXElement damageStarAnimator;

		private UXLabel damageStarLabel;

		private UXElement damageStarGroup;

		private UXSprite damageStar1;

		private UXSprite damageStar2;

		private UXSprite damageStar3;

		private UXLabel damageValueLabel;

		private UXElement lootGroup;

		private UXLabel lootCreditsLabel;

		private UXLabel lootMaterialLabel;

		private UXLabel lootContrabandLabel;

		private UXSprite lootContrabandIcon;

		private UXLabel medalsGainLabel;

		private UXLabel medalsLoseLabel;

		private UXElement tournamentRatingGainGroup;

		private UXSprite tournamentRatingGainSprite;

		private UXLabel tournamentRatingGainLabel;

		private UXElement tournamentRatingLoseGroup;

		private UXSprite tournamentRatingLoseSprite;

		private UXLabel tournamentRatingLoseLabel;

		private Dictionary<string, DeployableTroopControl> deployableTroops;

		private int battleControlsSelectedGroup;

		private DeployableTroopControl battleControlsSelectedCheckbox;

		private UXElement replayControlsGroup;

		private UXLabel replaySpeedLabel;

		private UXButton replayChangeSpeedButton;

		private UXLabel replayTimeLeftLabel;

		public HudConfig CurrentHudConfig;

		private List<UXElement> genericConfigElements;

		private Lang lang;

		private CurrentPlayer player;

		private DroidMoment droidMoment;

		private AssetHandle assetHandle;

		private Animation[] animations;

		private bool broughtIn;

		private bool isAnimating;

		private bool setInvisibleAfterAnimating;

		private UXAnchor performanceFPSAnchor;

		private UXAnchor performanceMemAnchor;

		private UXLabel fpsLabel;

		private UXLabel frameTimeLabel;

		private UXLabel fpeakLabel;

		private UXLabel deviceMemUsedLabel;

		private UXLabel memUsedLabel;

		private UXLabel memRsvdLabel;

		private UXLabel memMeshLabel;

		private UXLabel memTextureLabel;

		private UXLabel memAudioLabel;

		private UXLabel memAnimationLabel;

		private UXLabel memMaterialsLabel;

		private HUDResourceView creditsView;

		private HUDResourceView materialsView;

		private HUDResourceView contrabandView;

		private HUDResourceView crystalsView;

		private bool registeredFrameTimeObserver;

		private SquadSlidingScreen persistentSquadScreen;

		private bool logVisited;

		private UXSprite factionBackgroundUpgradeEmpire;

		private UXSprite neighborFactionBackgroundUpgradeEmpire;

		private bool readyToToggleVisibility;

		public HUDBaseLayoutToolView BaseLayoutToolView
		{
			get;
			private set;
		}

		public bool ReadyToToggleVisiblity
		{
			get
			{
				return this.readyToToggleVisibility && (this.persistentSquadScreen == null || this.persistentSquadScreen.IsClosed());
			}
			set
			{
				this.readyToToggleVisibility = value;
			}
		}

		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				if (this.isAnimating)
				{
					this.setInvisibleAfterAnimating = !value;
					return;
				}
				EventManager eventManager = Service.Get<EventManager>();
				if (value != base.Visible)
				{
					eventManager.SendEvent(EventId.HUDVisibilityChanging, value);
				}
				base.Visible = value;
				eventManager.SendEvent(EventId.HUDVisibilityChanged, null);
				if (!value)
				{
					if (this.registeredFrameTimeObserver)
					{
						this.RefreshAllResourceViews(false);
						this.registeredFrameTimeObserver = false;
						Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
					}
					Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
				}
				else
				{
					Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
				}
				Service.Get<UXController>().MiscElementsManager.SetEventTickerViewVisible(value);
				eventManager.RegisterObserver(this, EventId.NumInventoryItemsNotViewedUpdated);
				eventManager.RegisterObserver(this, EventId.HQCelebrationPlayed);
				eventManager.RegisterObserver(this, EventId.MissionActionButtonClicked);
				eventManager.RegisterObserver(this, EventId.SquadJoinInviteReceived);
				eventManager.RegisterObserver(this, EventId.SquadJoinInvitesReceived);
				eventManager.RegisterObserver(this, EventId.ContractStarted);
				eventManager.RegisterObserver(this, EventId.BuildingConstructed);
				eventManager.RegisterObserver(this, EventId.BuildingLevelUpgraded);
				eventManager.RegisterObserver(this, EventId.BuildingReplaced);
				eventManager.RegisterObserver(this, EventId.SquadUpdated);
				eventManager.RegisterObserver(this, EventId.SquadTroopsReceived);
				eventManager.RegisterObserver(this, EventId.WarPhaseChanged);
				if (value)
				{
					eventManager.RegisterObserver(this, EventId.TroopDeployed);
					eventManager.RegisterObserver(this, EventId.SpecialAttackDeployed);
					eventManager.RegisterObserver(this, EventId.HeroDeployed);
					eventManager.RegisterObserver(this, EventId.ChampionDeployed);
					eventManager.RegisterObserver(this, EventId.SquadTroopsDeployedByPlayer);
					eventManager.RegisterObserver(this, EventId.WorldLoadComplete);
					eventManager.RegisterObserver(this, EventId.GameStateChanged);
					eventManager.RegisterObserver(this, EventId.InventoryResourceUpdated);
					eventManager.RegisterObserver(this, EventId.LootEarnedUpdated);
					eventManager.RegisterObserver(this, EventId.TroopLevelUpgraded);
					eventManager.RegisterObserver(this, EventId.StarshipLevelUpgraded);
					eventManager.RegisterObserver(this, EventId.EquipmentUpgraded);
					eventManager.RegisterObserver(this, EventId.ChampionRepaired);
					eventManager.RegisterObserver(this, EventId.InventoryUnlockUpdated);
					eventManager.RegisterObserver(this, EventId.PlayerNameChanged);
					eventManager.RegisterObserver(this, EventId.PvpRatingChanged);
					eventManager.RegisterObserver(this, EventId.ScreenClosing);
					eventManager.RegisterObserver(this, EventId.SquadTroopsRequestedByCurrentPlayer);
					eventManager.RegisterObserver(this, EventId.SquadTroopsReceivedFromDonor);
					eventManager.RegisterObserver(this, EventId.SquadJoinApplicationAccepted);
					eventManager.RegisterObserver(this, EventId.SquadLeft);
					eventManager.RegisterObserver(this, EventId.WarAttackPlayerStarted);
					eventManager.RegisterObserver(this, EventId.WarAttackPlayerCompleted);
					eventManager.RegisterObserver(this, EventId.WarAttackBuffBaseStarted);
					eventManager.RegisterObserver(this, EventId.WarAttackBuffBaseCompleted);
					eventManager.RegisterObserver(this, EventId.TargetedBundleContentPrepared);
					eventManager.RegisterObserver(this, EventId.TargetedBundleRewardRedeemed);
					this.BaseLayoutToolView.RegisterObservers();
					return;
				}
				eventManager.UnregisterObserver(this, EventId.TroopDeployed);
				eventManager.UnregisterObserver(this, EventId.SpecialAttackDeployed);
				eventManager.UnregisterObserver(this, EventId.HeroDeployed);
				eventManager.UnregisterObserver(this, EventId.ChampionDeployed);
				eventManager.UnregisterObserver(this, EventId.SquadTroopsDeployedByPlayer);
				eventManager.UnregisterObserver(this, EventId.GameStateChanged);
				eventManager.UnregisterObserver(this, EventId.InventoryResourceUpdated);
				eventManager.UnregisterObserver(this, EventId.LootEarnedUpdated);
				eventManager.UnregisterObserver(this, EventId.TroopLevelUpgraded);
				eventManager.UnregisterObserver(this, EventId.StarshipLevelUpgraded);
				eventManager.UnregisterObserver(this, EventId.EquipmentUpgraded);
				eventManager.UnregisterObserver(this, EventId.ChampionRepaired);
				eventManager.UnregisterObserver(this, EventId.ContractStarted);
				eventManager.UnregisterObserver(this, EventId.BuildingConstructed);
				eventManager.UnregisterObserver(this, EventId.BuildingReplaced);
				eventManager.UnregisterObserver(this, EventId.BuildingLevelUpgraded);
				eventManager.UnregisterObserver(this, EventId.InventoryUnlockUpdated);
				eventManager.UnregisterObserver(this, EventId.PlayerNameChanged);
				eventManager.UnregisterObserver(this, EventId.PvpRatingChanged);
				eventManager.UnregisterObserver(this, EventId.ScreenClosing);
				eventManager.UnregisterObserver(this, EventId.SquadTroopsRequestedByCurrentPlayer);
				eventManager.UnregisterObserver(this, EventId.SquadTroopsReceivedFromDonor);
				eventManager.UnregisterObserver(this, EventId.SquadJoinApplicationAccepted);
				eventManager.UnregisterObserver(this, EventId.SquadLeft);
				eventManager.UnregisterObserver(this, EventId.WarAttackPlayerStarted);
				eventManager.UnregisterObserver(this, EventId.WarAttackPlayerCompleted);
				eventManager.UnregisterObserver(this, EventId.WarAttackBuffBaseStarted);
				eventManager.UnregisterObserver(this, EventId.WarAttackBuffBaseCompleted);
				eventManager.UnregisterObserver(this, EventId.TargetedBundleContentPrepared);
				eventManager.UnregisterObserver(this, EventId.TargetedBundleRewardRedeemed);
			}
		}

		public override bool HiddenInQueue
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public HUD()
		{
			this.FACTION_FLIP_ALERT_TITLE = "FACTION_ICON_FACTION_FLIP_ALERT_TITLE";
			this.FACTION_FLIP_ALERT_DESC = "FACTION_ICON_FACTION_FLIP_ALERT_DESC";
			this.ANIMATION_TRANSITION_WHITE_LIST = new string[]
			{
				"PlayerBottomLeft",
				"PlayerBottomRight",
				"PlayerInfo",
				"Currency"
			};
			base..ctor(Service.Get<CameraManager>().UXCamera);
			this.ReadyToToggleVisiblity = false;
			this.lang = Service.Get<Lang>();
			this.player = Service.Get<CurrentPlayer>();
			this.contextButtons = new List<UXElement>();
			this.contextButtonPool = new Dictionary<string, UXElement>();
			this.deployableTroops = null;
			this.animations = null;
			this.broughtIn = true;
			this.isAnimating = false;
			this.setInvisibleAfterAnimating = false;
			this.performanceFPSAnchor = null;
			this.performanceMemAnchor = null;
			this.fpsLabel = null;
			this.frameTimeLabel = null;
			this.deviceMemUsedLabel = null;
			this.fpeakLabel = null;
			this.memUsedLabel = null;
			this.memRsvdLabel = null;
			this.memMeshLabel = null;
			this.memTextureLabel = null;
			this.memAudioLabel = null;
			this.memAnimationLabel = null;
			this.memMaterialsLabel = null;
			this.registeredFrameTimeObserver = false;
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.HolonetContentPrepared);
			eventManager.RegisterObserver(this, EventId.HolonetContentPrepareStarted);
			eventManager.RegisterObserver(this, EventId.TargetedBundleContentPrepared);
			eventManager.RegisterObserver(this, EventId.HUDVisibilityChanged);
			this.logVisited = false;
			this.persistentSquadScreen = null;
			this.BaseLayoutToolView = new HUDBaseLayoutToolView(this);
			base.Load(ref this.assetHandle, "gui_hud", new UXFactoryLoadDelegate(this.LoadSuccess), new UXFactoryLoadDelegate(this.LoadFailure), null);
		}

		public override void OnDestroyElement()
		{
			if (this.assetHandle != AssetHandle.Invalid)
			{
				base.Unload(this.assetHandle, "gui_hud");
				this.assetHandle = AssetHandle.Invalid;
			}
			base.OnDestroyElement();
		}

		private void LoadSuccess(object cookie)
		{
			base.GetElement<UXElement>("ButtonNextBattleHolder").Visible = true;
			base.GetElement<UXElement>("DamageStarsHolder").Visible = true;
			this.InitSliders();
			this.InitGrids();
			this.InitLabels();
			this.InitButtons();
			this.InitLoot();
			this.InitDamageGroup();
			this.InitReplayControls();
			this.InitCountdownGroup();
			this.InitNeighborGroup();
			this.InitTournamentRatingGroup();
			this.BaseLayoutToolView.Initialize();
			if (Service.Get<CameraManager>() != null)
			{
				Service.Get<CameraManager>().CalculateScale((float)Screen.width, (float)Screen.height);
			}
			this.creditsView = new HUDResourceView("credits", base.GetElement<UXSlider>("PBarCurrency"), base.GetElement<UXLabel>("LabelCurrencyValueHome"), base.GetElement<UXElement>("Credits"));
			this.materialsView = new HUDResourceView("materials", base.GetElement<UXSlider>("PBarMaterial"), base.GetElement<UXLabel>("LabelMaterialValueHome"), base.GetElement<UXElement>("Materials"));
			this.contrabandView = new HUDResourceView("contraband", base.GetElement<UXSlider>("PBarContraband"), base.GetElement<UXLabel>("LabelContrabandValueHome"), base.GetElement<UXElement>("Contraband"));
			this.crystalsView = new HUDResourceView("crystals", null, base.GetElement<UXLabel>("LabelCrystalsValueHome"), base.GetElement<UXElement>("Crystals"));
			this.AnimateControls(false, 100f);
			Service.Get<EventManager>().SendEvent(EventId.HudComplete, this);
		}

		private void LoadFailure(object cookie)
		{
			Service.Get<EventManager>().SendEvent(EventId.HudComplete, null);
		}

		private void InitGrids()
		{
			this.troopGrid = base.GetElement<UXGrid>("TroopsGrid");
			UXElement element = base.GetElement<UXElement>("HeroAbilityActive");
			element.Visible = false;
		}

		private void InitSliders()
		{
			this.currencyGroup = base.GetElement<UXElement>("Currency");
			this.opponentGroup = base.GetElement<UXElement>("OpponentInfo");
			this.prebattleMedalsGroup = base.GetElement<UXElement>("MedalInfo");
			this.playerGroup = base.GetElement<UXElement>("PlayerInfo");
			this.shieldGroup = base.GetElement<UXElement>("Shield");
			this.specialPromotionGroup = base.GetElement<UXElement>("SpecialPromo");
			this.droidAddLabel = base.GetElement<UXLabel>("LabelDroidsAdd");
			this.droidMaxLabel = base.GetElement<UXLabel>("LabelDroidsMax");
			this.droidAddGroup = base.GetElement<UXElement>("DroidsAdd");
			this.droidMaxGroup = base.GetElement<UXElement>("DroidsMax");
			this.protectionLabel = base.GetElement<UXLabel>("LabelShield");
			this.opponentSymbol = base.GetElement<UXSprite>("SpriteSymbolOpponent");
			this.opponentSymbolUpgradeRebel = base.GetElement<UXSprite>("SpriteSymbolOpponentFactionUp");
			this.opponentSymbolUpgradeEmpire = base.GetElement<UXSprite>("SpriteSymbolOpponentFactionUpEmp");
		}

		private void InitButtons()
		{
			this.medalsButton = base.GetElement<UXButton>("Medals");
			this.medalsButton.OnClicked = new UXButtonClickedDelegate(this.OnTooltipButtonClicked);
			this.baseRatingButton = base.GetElement<UXButton>("BaseRating");
			this.baseRatingButton.OnClicked = new UXButtonClickedDelegate(this.OnBaseScoreButtonClicked);
			UXElement element = base.GetElement<UXElement>("ContainerJewelBaseRating");
			element.Visible = false;
			this.factionBackground = base.GetElement<UXSprite>("TrophiesBkg");
			this.factionBackgroundUpgradeRebel = base.GetElement<UXSprite>("TrophiesBkgFactionUp");
			this.factionBackgroundUpgradeEmpire = base.GetElement<UXSprite>("TrophiesBkgFactionUpEmp");
			this.endBattleButton = base.GetElement<UXButton>("ButtonEndBattle");
			this.endBattleButton.OnClicked = new UXButtonClickedDelegate(this.OnEndBattleButtonClicked);
			this.nextBattleButton = base.GetElement<UXButton>("ButtonNextBattle");
			this.nextBattleButton.OnClicked = new UXButtonClickedDelegate(this.OnNextBattleButtonClicked);
			this.battleButton = base.GetElement<UXButton>("ButtonBattle");
			this.battleButton.OnClicked = new UXButtonClickedDelegate(this.OnBattleButtonClicked);
			this.warButton = base.GetElement<UXButton>("ButtonWar");
			this.warButton.OnClicked = new UXButtonClickedDelegate(this.OnWarButtonClicked);
			this.logButton = base.GetElement<UXButton>("ButtonLog");
			this.logButton.OnClicked = new UXButtonClickedDelegate(this.OnLogButtonClicked);
			this.leaderboardButton = base.GetElement<UXButton>("ButtonLeaderboard");
			this.leaderboardButton.OnClicked = new UXButtonClickedDelegate(this.OnLeaderboardButtonClicked);
			this.holonetButton = base.GetElement<UXButton>("Newspaper");
			this.holonetButton.OnClicked = new UXButtonClickedDelegate(this.OnHolonetButtonClicked);
			this.settingsButton = base.GetElement<UXButton>("ButtonSettings");
			this.settingsButton.OnClicked = new UXButtonClickedDelegate(this.OnSettingsButtonClicked);
			this.joinSquadButton = base.GetElement<UXButton>("ButtonClans");
			this.joinSquadButton.OnClicked = new UXButtonClickedDelegate(this.OnSquadsButtonClicked);
			this.squadsButtonLabel = base.GetElement<UXLabel>("LabelClans");
			this.squadsButtonLabel.Text = this.lang.Get("s_Clans", new object[0]);
			if (Service.Get<SquadController>().StateManager.GetCurrentSquad() != null)
			{
				this.UpdateSquadJewelCount();
			}
			else if (GameConstants.SQUAD_INVITES_ENABLED)
			{
				this.UpdateSquadJewelCount();
			}
			this.homeButton = base.GetElement<UXButton>("ButtonHome");
			this.homeButton.OnClicked = new UXButtonClickedDelegate(this.OnHomeButtonClicked);
			this.crystalsDroidsGroup = base.GetElement<UXElement>("CrystalsDroids");
			this.crystalButton = base.GetElement<UXButton>("Crystals");
			this.crystalButton.OnClicked = new UXButtonClickedDelegate(this.OnCrystalButtonClicked);
			this.droidButton = base.GetElement<UXButton>("Droids");
			this.droidButton.OnClicked = new UXButtonClickedDelegate(this.OnDroidButtonClicked);
			this.protectionButton = base.GetElement<UXButton>("Shield");
			this.protectionButton.OnClicked = new UXButtonClickedDelegate(this.OnShieldButtonClicked);
			this.targetedBundleButton = base.GetElement<UXButton>("SpecialPromo");
			this.targetedBundleButton.OnClicked = new UXButtonClickedDelegate(this.OnSpecialPromotionButtonClicked);
			this.targetedBundleGlowShown = false;
			this.targetedBundleButtonGlowAnim = this.targetedBundleButton.Root.GetComponent<Animator>();
			this.targetedBundleButtonGlowAnim.Rebind();
			this.editButton = base.GetElement<UXButton>("ButtonEdit");
			this.editButton.OnClicked = new UXButtonClickedDelegate(this.OnEditButtonClicked);
			this.editButton.Visible = false;
			this.exitEditButton = base.GetElement<UXButton>("ButtonExitEdit");
			this.exitEditButton.OnClicked = new UXButtonClickedDelegate(this.OnHomeButtonClicked);
			this.exitEditAnimation = base.GetElement<UXElement>("ButtonExitEditHolder").Root.GetComponent<Animation>();
			this.storeButton = base.GetElement<UXButton>("ButtonStore");
			this.storeButton.OnClicked = new UXButtonClickedDelegate(this.OnStoreButtonClicked);
			this.animations = this.root.GetComponentsInChildren<Animation>(true);
			this.contextButtonTemplate = base.GetElement<UXButton>("ButtonContext");
			this.contextButtonParentTemplate = base.GetElement<UXElement>("ButtonContextParent");
			this.contextButtonParentTemplate.Visible = false;
			this.curAnims = new List<Anim>();
			base.GetElement<UXButton>("ContainerBuffsYoursSquadWars").OnClicked = new UXButtonClickedDelegate(this.OnYourBuffsButtonClicked);
			base.GetElement<UXButton>("ContainerBuffsOpponentSquadWars").OnClicked = new UXButtonClickedDelegate(this.OnOpponentBuffsButtonClicked);
		}

		private void InitLabels()
		{
			this.playerNameLabel = base.GetElement<UXLabel>("LabelPlayerName");
			this.playerClanLabel = base.GetElement<UXLabel>("LabelClanName");
			this.timeLeftLabel = base.GetElement<UXLabel>("LabelTimeLeft");
			this.baseNameLabel = base.GetElement<UXLabel>("LabelBaseNameOpponent");
			this.contextDescLabel = base.GetElement<UXLabel>("LabelContextDescription");
			this.contextDescLabel.Text = "";
			this.playerRankLabel = base.GetElement<UXLabel>("LabelTrophies");
			this.playerMedalLabel = base.GetElement<UXLabel>("LabelMedals");
			this.opponentRankLabel = base.GetElement<UXLabel>("LabelTrophiesOpponent");
			this.opponentRankBG = base.GetElement<UXSprite>("BaseScoreBkgOpponent");
			this.deployInstructionsLabel = base.GetElement<UXLabel>("LabelDeployInstructions");
			this.deployInstructionsLabel.Visible = false;
			this.clansJewel = JewelControl.Create(this, "Clans");
			this.logJewel = JewelControl.Create(this, "Log");
			JewelControl.Create(this, "Leaderboard");
			JewelControl.Create(this, "Settings");
			this.storeJewel = JewelControl.Create(this, "Store");
			this.leiJewel = JewelControl.Create(this, "StoreSpecial");
			this.battleJewel = JewelControl.Create(this, "Battle");
			this.holonetJewel = JewelControl.Create(this, "NewsHolo");
			this.warJewel = JewelControl.Create(this, "War");
			this.warPrepJewel = JewelControl.Create(this, "Prep");
			this.medalsGainLabel = base.GetElement<UXLabel>("LabelMedalsValueOpponent");
			this.medalsLoseLabel = base.GetElement<UXLabel>("LabelDefeatMedals");
		}

		private void InitLoot()
		{
			this.lootGroup = base.GetElement<UXElement>("AvailableLoot");
			this.lootCreditsLabel = base.GetElement<UXLabel>("LabelCurrencyValueOpponent");
			this.lootMaterialLabel = base.GetElement<UXLabel>("LabelMaterialsValueOpponent");
			this.lootContrabandIcon = base.GetElement<UXSprite>("SpriteOpponentContraband");
			this.lootContrabandLabel = base.GetElement<UXLabel>("LabelContrabandValueOpponent");
			this.lootGroup.Visible = false;
		}

		private void InitDamageGroup()
		{
			this.damageStarAnchor = base.GetElement<UXElement>("BattleStarsRewards");
			this.damageStarAnimator = base.GetElement<UXElement>("RewardStarHolder");
			this.damageStarLabel = base.GetElement<UXLabel>("LabelRewardStar");
			this.damageStarGroup = base.GetElement<UXElement>("DamageStars");
			this.damageStar1 = base.GetElement<UXSprite>("SpriteStar1");
			this.damageStar2 = base.GetElement<UXSprite>("SpriteStar2");
			this.damageStar3 = base.GetElement<UXSprite>("SpriteStar3");
			this.damageValueLabel = base.GetElement<UXLabel>("LabelPercent");
		}

		private void InitReplayControls()
		{
			base.GetElement<UXLabel>("LabelReplayEndsIn").Text = this.lang.Get("replay_ends_in", new object[0]);
			this.replayControlsGroup = base.GetElement<UXElement>("ReplayControls");
			this.replayChangeSpeedButton = base.GetElement<UXButton>("ButtonReplaySpeed");
			this.replaySpeedLabel = base.GetElement<UXLabel>("LabelReplaySpeed");
			this.replayTimeLeftLabel = base.GetElement<UXLabel>("LabelReplayTime");
			this.replayChangeSpeedButton.OnClicked = new UXButtonClickedDelegate(this.OnReplaySpeedChangeButtonClicked);
		}

		private void InitNeighborGroup()
		{
			this.neighborGroup = base.GetElement<UXElement>("FriendInfo");
			this.neighborNameLabel = base.GetElement<UXLabel>("LabelFriendName");
			this.neighborSquadLabel = base.GetElement<UXLabel>("LabelFriendClanName");
			this.neighborMedalsLabel = base.GetElement<UXLabel>("LabelFriendMedals");
			this.neighborTrophiesLabel = base.GetElement<UXLabel>("LabelFriendTrophies");
			this.neighborFactionBackground = base.GetElement<UXSprite>("TrophiesBkgFriend");
			this.neighborFactionBackgroundUpgradeRebel = base.GetElement<UXSprite>("TrophiesBkgFriendFactionUp");
			this.neighborFactionBackgroundUpgradeEmpire = base.GetElement<UXSprite>("TrophiesBkgFriendFactionUpEmp");
		}

		private void InitTournamentRatingGroup()
		{
			this.tournamentRatingGainGroup = base.GetElement<UXElement>("TournamentMedals");
			this.tournamentRatingGainLabel = base.GetElement<UXLabel>("LabelDefeatTournament");
			this.tournamentRatingGainSprite = base.GetElement<UXSprite>("SpriteIcoTournamentMedals");
			this.tournamentRatingLoseGroup = base.GetElement<UXElement>("TournamentMedalsDefeat");
			this.tournamentRatingLoseLabel = base.GetElement<UXLabel>("LabelDefeatTournamentMedals");
			this.tournamentRatingLoseSprite = base.GetElement<UXSprite>("SpriteIcoTournamentMedalsDefeat");
		}

		private void InitCountdownGroup()
		{
			this.preCombatGroup = base.GetElement<UXElement>("PrecombatCountdown");
			this.preCombatTimeLabel = base.GetElement<UXLabel>("LabelCount");
			this.preCombatCountdownFill = base.GetElement<UXSprite>("CountdownFill");
			this.preCombatTimeLabel.Text = Convert.ToInt32(GameConstants.PVP_MATCH_COUNTDOWN, CultureInfo.InvariantCulture).ToString();
			UXLabel element = base.GetElement<UXLabel>("LabelBattleBegins");
			element.Text = this.lang.Get("PRECOMBAT_BATTLE_BEGINS_IN", new object[0]);
			this.preCombatGroup.Visible = false;
			this.preCombatGoalLabel = base.GetElement<UXLabel>("LabelGoal");
			this.preCombatGoalLabel.Visible = false;
		}

		public void ShowCountdown(bool enable)
		{
			this.preCombatGroup.Visible = enable;
		}

		public void SetCountdownTime(float remaining, float duration)
		{
			this.preCombatCountdownFill.FillAmount = remaining / duration;
			int num = Mathf.CeilToInt(remaining);
			this.preCombatTimeLabel.Text = num.ToString();
		}

		public void TogglePerformanceDisplay(bool isFPS)
		{
			PerformanceMonitor performanceMonitor = Service.Get<PerformanceMonitor>();
			UXController uXController = Service.Get<UXController>();
			MiscElementsManager miscElementsManager = uXController.MiscElementsManager;
			if (isFPS)
			{
				if (this.performanceFPSAnchor == null)
				{
					this.performanceFPSAnchor = uXController.PerformanceAnchor;
					float d = 0f;
					float num = (float)(Screen.height / 2);
					this.fpsLabel = miscElementsManager.CreateGameBoardLabel("FPS", this.performanceFPSAnchor);
					this.fpsLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.fpsLabel.LocalPosition = Vector3.right * d + Vector3.up * num;
					num += this.fpsLabel.LineHeight;
					this.frameTimeLabel = miscElementsManager.CreateGameBoardLabel("FrameTime", this.performanceFPSAnchor);
					this.frameTimeLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.frameTimeLabel.LocalPosition = Vector3.right * d + Vector3.up * num;
					num += this.frameTimeLabel.LineHeight;
					this.fpeakLabel = miscElementsManager.CreateGameBoardLabel("FPeak", this.performanceFPSAnchor);
					this.fpeakLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.fpeakLabel.LocalPosition = Vector3.right * d + Vector3.up * num;
					performanceMonitor.RegisterFPSObserver(this);
					return;
				}
				performanceMonitor.UnregisterFPSObserver(this);
				miscElementsManager.DestroyMiscElement(this.fpeakLabel);
				this.fpeakLabel = null;
				miscElementsManager.DestroyMiscElement(this.fpsLabel);
				this.fpsLabel = null;
				miscElementsManager.DestroyMiscElement(this.frameTimeLabel);
				this.frameTimeLabel = null;
				this.performanceFPSAnchor = null;
				return;
			}
			else
			{
				if (this.performanceMemAnchor == null)
				{
					this.performanceMemAnchor = uXController.PerformanceAnchor;
					float d2 = 0f;
					float num2 = (float)Screen.height / 2f + 56f;
					this.deviceMemUsedLabel = miscElementsManager.CreateGameBoardLabel("DeviceMem", this.performanceMemAnchor);
					this.deviceMemUsedLabel.TextColor = Color.red;
					this.deviceMemUsedLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.deviceMemUsedLabel.LocalPosition = Vector3.right * d2 + Vector3.up * num2;
					num2 += this.deviceMemUsedLabel.LineHeight;
					this.memRsvdLabel = miscElementsManager.CreateGameBoardLabel("Rsvd", this.performanceMemAnchor);
					this.memRsvdLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.memRsvdLabel.LocalPosition = Vector3.right * d2 + Vector3.up * num2;
					num2 += this.memRsvdLabel.LineHeight;
					this.memUsedLabel = miscElementsManager.CreateGameBoardLabel("Used", this.performanceMemAnchor);
					this.memUsedLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.memUsedLabel.LocalPosition = Vector3.right * d2 + Vector3.up * num2;
					num2 += this.memUsedLabel.LineHeight;
					this.memTextureLabel = miscElementsManager.CreateGameBoardLabel("Texture", this.performanceMemAnchor);
					this.memTextureLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.memTextureLabel.LocalPosition = Vector3.right * d2 + Vector3.up * num2;
					num2 += this.memTextureLabel.LineHeight;
					this.memMeshLabel = miscElementsManager.CreateGameBoardLabel("Mesh", this.performanceMemAnchor);
					this.memMeshLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.memMeshLabel.LocalPosition = Vector3.right * d2 + Vector3.up * num2;
					num2 += this.memMeshLabel.LineHeight;
					this.memAudioLabel = miscElementsManager.CreateGameBoardLabel("Audio", this.performanceMemAnchor);
					this.memAudioLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.memAudioLabel.LocalPosition = Vector3.right * d2 + Vector3.up * num2;
					num2 += this.memAudioLabel.LineHeight;
					this.memAnimationLabel = miscElementsManager.CreateGameBoardLabel("Animation", this.performanceMemAnchor);
					this.memAnimationLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.memAnimationLabel.LocalPosition = Vector3.right * d2 + Vector3.up * num2;
					num2 += this.memAnimationLabel.LineHeight;
					this.memMaterialsLabel = miscElementsManager.CreateGameBoardLabel("Material", this.performanceMemAnchor);
					this.memMaterialsLabel.Pivot = UIWidget.Pivot.BottomRight;
					this.memMaterialsLabel.LocalPosition = Vector3.right * d2 + Vector3.up * num2;
					performanceMonitor.RegisterMemObserver(this);
					return;
				}
				performanceMonitor.UnregisterMemObserver(this);
				miscElementsManager.DestroyMiscElement(this.memRsvdLabel);
				this.memRsvdLabel = null;
				miscElementsManager.DestroyMiscElement(this.memUsedLabel);
				this.memUsedLabel = null;
				miscElementsManager.DestroyMiscElement(this.memTextureLabel);
				this.memTextureLabel = null;
				miscElementsManager.DestroyMiscElement(this.memMeshLabel);
				this.memMeshLabel = null;
				miscElementsManager.DestroyMiscElement(this.memAudioLabel);
				this.memAudioLabel = null;
				miscElementsManager.DestroyMiscElement(this.memAnimationLabel);
				this.memAnimationLabel = null;
				miscElementsManager.DestroyMiscElement(this.memMaterialsLabel);
				this.memMaterialsLabel = null;
				miscElementsManager.DestroyMiscElement(this.deviceMemUsedLabel);
				this.deviceMemUsedLabel = null;
				this.performanceMemAnchor = null;
				return;
			}
		}

		public void OnPerformanceFPS(float fps)
		{
			this.fpsLabel.Text = string.Format("{0:F2} FPS ", new object[]
			{
				fps
			});
			this.frameTimeLabel.Text = string.Format("{0:F2} ms", new object[]
			{
				1000.0 / (double)fps
			});
		}

		public void OnPerformanceFPeak(uint fpeak)
		{
			this.fpeakLabel.Text = string.Format("{0} FPeak ", new object[]
			{
				fpeak
			});
		}

		public void OnPerformanceMemRsvd(uint memRsvd)
		{
			this.memRsvdLabel.Text = string.Format("{0} Rsvd", new object[]
			{
				memRsvd / 1000000u
			});
		}

		public void OnPerformanceMemUsed(uint memUsd)
		{
			this.memUsedLabel.Text = string.Format("{0} Used", new object[]
			{
				memUsd / 1000000u
			});
		}

		public void OnPerformanceMemTexture(uint mem)
		{
			this.memTextureLabel.Text = string.Format("{0} Texture", new object[]
			{
				mem / 1000000u
			});
		}

		public void OnPerformanceMemMesh(uint mem)
		{
			this.memMeshLabel.Text = string.Format("{0} Mesh", new object[]
			{
				mem / 1000000u
			});
		}

		public void OnPerformanceMemAudio(uint mem)
		{
			this.memAudioLabel.Text = string.Format("{0} Audio", new object[]
			{
				mem / 1000000u
			});
		}

		public void OnPerformanceMemAnimation(uint mem)
		{
			this.memAnimationLabel.Text = string.Format("{0:F1} Animation", new object[]
			{
				mem / 1000000u
			});
		}

		public void OnPerformanceMemMaterials(uint mem)
		{
			this.memMaterialsLabel.Text = string.Format("{0:F1} Material", new object[]
			{
				mem / 1000000u
			});
		}

		public void OnPerformanceDeviceMemUsage(long memory)
		{
			this.deviceMemUsedLabel.Text = string.Format("{0:F2} MB", new object[]
			{
				memory / 1048576L
			});
		}

		private void SetupDeployableTroops()
		{
			GameStateMachine gameStateMachine = Service.Get<GameStateMachine>();
			if (gameStateMachine.PreviousStateType == typeof(BattleStartState) && gameStateMachine.PreviousStateType != typeof(FueBattleStartState) && gameStateMachine.CurrentState is BattlePlayState)
			{
				return;
			}
			if (this.troopGrid.Count != 0)
			{
				this.troopGrid.RepositionItems();
				return;
			}
			this.troopGrid.Visible = true;
			this.deployableTroops = new Dictionary<string, DeployableTroopControl>();
			BattleController battleController = Service.Get<BattleController>();
			Dictionary<string, int> allPlayerDeployableTroops = battleController.GetAllPlayerDeployableTroops();
			Dictionary<string, int> allPlayerDeployableSpecialAttacks = battleController.GetAllPlayerDeployableSpecialAttacks();
			Dictionary<string, int> allPlayerDeployableHeroes = battleController.GetAllPlayerDeployableHeroes();
			Dictionary<string, int> allPlayerDeployableChampions = battleController.GetAllPlayerDeployableChampions();
			CurrentBattle currentBattle = battleController.GetCurrentBattle();
			BattleDeploymentData seededPlayerDeployableData = currentBattle.SeededPlayerDeployableData;
			if (allPlayerDeployableTroops != null)
			{
				this.CreateDeployableControls(allPlayerDeployableTroops, seededPlayerDeployableData.TroopData, new UXCheckboxSelectedDelegate(this.OnTroopCheckboxSelected), currentBattle);
			}
			if (allPlayerDeployableSpecialAttacks != null)
			{
				this.CreateDeployableControls(allPlayerDeployableSpecialAttacks, seededPlayerDeployableData.SpecialAttackData, new UXCheckboxSelectedDelegate(this.OnSpecialAttackCheckboxSelected), currentBattle);
			}
			if (allPlayerDeployableHeroes != null)
			{
				this.CreateDeployableControls(allPlayerDeployableHeroes, seededPlayerDeployableData.HeroData, new UXCheckboxSelectedDelegate(this.OnHeroCheckboxSelected), currentBattle);
			}
			if (allPlayerDeployableChampions != null)
			{
				this.CreateDeployableControls(allPlayerDeployableChampions, seededPlayerDeployableData.ChampionData, new UXCheckboxSelectedDelegate(this.OnChampionCheckboxSelected), currentBattle);
			}
			if (battleController.CanPlayerDeploySquadTroops())
			{
				this.CreateSquadDeployableControl();
			}
			int highestLevelHQ = Service.Get<BuildingLookupController>().GetHighestLevelHQ();
			int aUTOSELECT_DISABLE_HQTHRESHOLD = GameConstants.AUTOSELECT_DISABLE_HQTHRESHOLD;
			if ((highestLevelHQ < aUTOSELECT_DISABLE_HQTHRESHOLD || aUTOSELECT_DISABLE_HQTHRESHOLD < 1) && this.troopGrid.Count != 0)
			{
				UXCheckbox uXCheckbox = this.troopGrid.GetItem(0).Tag as UXCheckbox;
				uXCheckbox.Selected = true;
				uXCheckbox.OnSelected(uXCheckbox, true);
			}
			this.troopGrid.RepositionItems();
		}

		public bool IsElementProvidedTroop(UXElement element)
		{
			if (element is UXCheckbox && element.Tag is string)
			{
				UXElement optionalSubElement = this.troopGrid.GetOptionalSubElement<UXElement>((string)element.Tag, "ProvidedFrame");
				return optionalSubElement != null && optionalSubElement.Visible;
			}
			return false;
		}

		private void CreateDeployableControls(Dictionary<string, int> deployables, Dictionary<string, int> seededDeployables, UXCheckboxSelectedDelegate onSelected, CurrentBattle battle)
		{
			this.troopGrid.SetTemplateItem("TroopTemplate");
			IDataController dataController = Service.Get<IDataController>();
			ActiveArmory activeArmory = Service.Get<CurrentPlayer>().ActiveArmory;
			bool flag = onSelected == new UXCheckboxSelectedDelegate(this.OnSpecialAttackCheckboxSelected);
			bool flag2 = onSelected == new UXCheckboxSelectedDelegate(this.OnHeroCheckboxSelected);
			bool flag3 = onSelected == new UXCheckboxSelectedDelegate(this.OnChampionCheckboxSelected);
			foreach (KeyValuePair<string, int> current in deployables)
			{
				string key = current.get_Key();
				int value = current.get_Value();
				if (value > 0)
				{
					IDeployableVO arg_A9_0;
					if (!flag)
					{
						IDeployableVO optional = dataController.GetOptional<TroopTypeVO>(key);
						arg_A9_0 = optional;
					}
					else
					{
						IDeployableVO optional = dataController.GetOptional<SpecialAttackTypeVO>(key);
						arg_A9_0 = optional;
					}
					IDeployableVO deployableVO = arg_A9_0;
					if (deployableVO != null)
					{
						bool flag4 = seededDeployables != null && seededDeployables.ContainsKey(current.get_Key());
						string text = key;
						UXElement uXElement = this.troopGrid.CloneTemplateItem(text);
						UXCheckbox subElement = this.troopGrid.GetSubElement<UXCheckbox>(text, "CheckboxTroop");
						subElement.Tag = key;
						subElement.OnSelected = onSelected;
						subElement.Selected = false;
						uXElement.Tag = subElement;
						UXSprite subElement2 = this.troopGrid.GetSubElement<UXSprite>(text, "SpriteTroop");
						UXSprite subElement3 = this.troopGrid.GetSubElement<UXSprite>(text, "SpriteTroopDim");
						UXElement subElement4 = this.troopGrid.GetSubElement<UXElement>(text, "HeroAbilityActive");
						UXElement subElement5 = this.troopGrid.GetSubElement<UXElement>(text, "HeroReady");
						UXElement subElement6 = this.troopGrid.GetSubElement<UXElement>(text, "EquipmentFX");
						UXSprite subElement7 = this.troopGrid.GetSubElement<UXSprite>(text, "SpriteSquad");
						UXSprite subElement8 = this.troopGrid.GetSubElement<UXSprite>(text, "SpriteTroopBkg");
						string defaultCardName = "TroopFrameBgDefault";
						string cardName = "TroopFrameBgQ{0}";
						if (flag4)
						{
							defaultCardName = "ProvidedFrameDefault";
							cardName = "ProvidedFrameQ{0}";
							this.troopGrid.GetSubElement<UXElement>(text, "StandardFrame").Visible = false;
							this.troopGrid.GetSubElement<UXElement>(text, "ProvidedFrame").Visible = true;
							subElement3 = this.troopGrid.GetSubElement<UXSprite>(text, "SpriteProvidedTroopDim");
							subElement8 = this.troopGrid.GetSubElement<UXSprite>(text, "SpriteProvidedTroopBkg");
							UXSprite subElement9 = this.troopGrid.GetSubElement<UXSprite>(text, "SpriteProvidedSelected");
							subElement.SetAnimationAndSprite(subElement9);
						}
						base.RevertToOriginalNameRecursively(subElement4.Root, text);
						base.RevertToOriginalNameRecursively(subElement5.Root, text);
						base.RevertToOriginalNameRecursively(subElement6.Root, text);
						subElement4.Visible = false;
						subElement5.Visible = false;
						subElement6.Visible = false;
						ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(deployableVO, subElement2);
						Service.Get<EventManager>().SendEvent(EventId.ButtonCreated, new GeometryTag(deployableVO, projectorConfig, battle));
						projectorConfig.AnimPreference = AnimationPreference.AnimationPreferred;
						GeometryProjector optionalGeometry = ProjectorUtils.GenerateProjector(projectorConfig);
						if (flag3)
						{
							FactionDecal.UpdateDeployableDecal(text, this.troopGrid, deployableVO);
							subElement8.SpriteName = "champion_bkg";
						}
						else if (flag2)
						{
							FactionDecal.UpdateDeployableDecal(text, this.troopGrid, deployableVO);
							subElement8.SpriteName = "hero_bkg";
						}
						else if (flag)
						{
							subElement8.SpriteName = "starship_bkg";
						}
						else
						{
							subElement8.SpriteName = "troop_bkg";
						}
						subElement7.Visible = false;
						UXLabel uXLabel = flag4 ? this.troopGrid.GetSubElement<UXLabel>(text, "LabelProvidedQuantity") : this.troopGrid.GetSubElement<UXLabel>(text, "LabelQuantity");
						uXLabel.Tag = key;
						uXLabel.Text = value.ToString();
						uXLabel.TextColor = UXUtils.GetCostColor(uXLabel, value != 0, false);
						UXLabel uXLabel2 = flag4 ? this.troopGrid.GetSubElement<UXLabel>(text, "LabelProvidedTroopLevel") : this.troopGrid.GetSubElement<UXLabel>(text, "LabelTroopLevel");
						uXLabel2.Text = LangUtils.GetLevelText(deployableVO.Lvl);
						string text2 = null;
						if (deployableVO is TroopTypeVO)
						{
							TroopTypeVO troop = deployableVO as TroopTypeVO;
							Service.Get<SkinController>().GetApplicableSkin(troop, activeArmory.Equipment, out text2);
						}
						int qualityInt;
						if (!string.IsNullOrEmpty(text2))
						{
							EquipmentVO equipmentVO = dataController.Get<EquipmentVO>(text2);
							qualityInt = (int)equipmentVO.Quality;
						}
						else
						{
							qualityInt = Service.Get<DeployableShardUnlockController>().GetUpgradeQualityForDeployable(deployableVO);
						}
						UXUtils.SetCardQuality(this, this.troopGrid, text, qualityInt, cardName, defaultCardName);
						DeployableTroopControl value2 = new DeployableTroopControl(subElement, uXLabel, subElement3, subElement4, subElement5, optionalGeometry, flag2, flag, key, subElement6);
						this.deployableTroops.Add(key, value2);
						this.troopGrid.AddItem(uXElement, deployableVO.Order);
					}
				}
			}
			this.troopGrid.Scroll(0f);
		}

		private void CreateSquadDeployableControl()
		{
			this.troopGrid.SetTemplateItem("TroopTemplate");
			UXElement uXElement = this.troopGrid.CloneTemplateItem("squadTroops");
			UXCheckbox subElement = this.troopGrid.GetSubElement<UXCheckbox>("squadTroops", "CheckboxTroop");
			subElement.Tag = "squadTroops";
			subElement.OnSelected = new UXCheckboxSelectedDelegate(this.OnSquadTroopsCheckboxSelected);
			subElement.Selected = false;
			uXElement.Tag = subElement;
			UXSprite subElement2 = this.troopGrid.GetSubElement<UXSprite>("squadTroops", "SpriteTroop");
			UXSprite subElement3 = this.troopGrid.GetSubElement<UXSprite>("squadTroops", "SpriteTroopDim");
			UXSprite subElement4 = this.troopGrid.GetSubElement<UXSprite>("squadTroops", "SpriteSquad");
			UXSprite subElement5 = this.troopGrid.GetSubElement<UXSprite>("squadTroops", "SpriteTroopBkg");
			UXElement subElement6 = this.troopGrid.GetSubElement<UXElement>("squadTroops", "HeroAbilityActive");
			UXElement subElement7 = this.troopGrid.GetSubElement<UXElement>("squadTroops", "HeroReady");
			UXElement subElement8 = this.troopGrid.GetSubElement<UXElement>("squadTroops", "EquipmentFX");
			subElement6.Visible = false;
			subElement7.Visible = false;
			subElement8.Visible = false;
			subElement.SetTweenTarget(subElement4);
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad != null)
			{
				subElement4.SpriteName = currentSquad.Symbol;
			}
			subElement5.SpriteName = "troop_bkg";
			subElement2.Visible = false;
			subElement4.Visible = true;
			UXLabel subElement9 = this.troopGrid.GetSubElement<UXLabel>("squadTroops", "LabelQuantity");
			subElement9.Tag = "squadTroops";
			subElement9.Text = "1";
			subElement9.TextColor = UXUtils.GetCostColor(subElement9, true, false);
			DeployableTroopControl value = new DeployableTroopControl(subElement, subElement9, subElement3, null, null, null, false, false, "squadTroops", null);
			this.deployableTroops.Add("squadTroops", value);
			this.troopGrid.AddItem(uXElement, 99999999);
		}

		private void CleanupDeployableTroops()
		{
			this.troopGrid.Clear();
			this.troopGrid.HideScrollArrows();
			this.troopGrid.Visible = false;
			if (this.deployableTroops != null)
			{
				foreach (DeployableTroopControl current in this.deployableTroops.Values)
				{
					current.StopObserving();
				}
				this.deployableTroops = null;
			}
		}

		private void UpdateDeployInstructionLabel()
		{
			if (!this.player.CampaignProgress.FueInProgress)
			{
				this.deployInstructionsLabel.Visible = false;
				return;
			}
			bool visible = false;
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			if (currentBattle != null && this.deployableTroops != null && this.deployableTroops.Count > 0)
			{
				if (currentBattle.Type != BattleType.PveDefend)
				{
					this.deployInstructionsLabel.Text = this.lang.Get("DEPLOY_INSTRUCTIONS", new object[0]);
				}
				else
				{
					this.deployInstructionsLabel.Text = this.lang.Get("DEPLOY_DEFENSE_INSTRUCTIONS", new object[0]);
				}
				visible = true;
			}
			this.deployInstructionsLabel.Visible = visible;
		}

		public void RefreshPlayerSocialInformation()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			this.playerNameLabel.Text = worldOwner.PlayerName;
			this.neighborNameLabel.Text = worldOwner.PlayerName;
			if (worldOwner.Squad != null)
			{
				this.playerClanLabel.Text = worldOwner.Squad.SquadName;
				this.neighborSquadLabel.Text = worldOwner.Squad.SquadName;
			}
			else
			{
				this.playerClanLabel.Text = "";
				this.neighborSquadLabel.Text = "";
			}
			int rating = GameUtils.CalculatePlayerVictoryRating(worldOwner);
			FactionType faction = worldOwner.Faction;
			string factionUpgradeIcon;
			if (faction != FactionType.Empire)
			{
				if (faction != FactionType.Rebel)
				{
					factionUpgradeIcon = "HudXpBg";
				}
				else
				{
					factionUpgradeIcon = Service.Get<FactionIconUpgradeController>().GetIcon(FactionType.Rebel, rating);
				}
			}
			else
			{
				factionUpgradeIcon = Service.Get<FactionIconUpgradeController>().GetIcon(FactionType.Empire, rating);
			}
			this.RefreshFactionIconVisibility(rating, factionUpgradeIcon, worldOwner);
		}

		private void RefreshFactionIconVisibility(int rating, string factionUpgradeIcon, GamePlayer worldOwner)
		{
			bool flag = worldOwner.Faction == FactionType.Empire;
			bool flag2 = Service.Get<FactionIconUpgradeController>().UseUpgradeImage(rating);
			this.factionBackground.Visible = !flag2;
			this.neighborFactionBackground.Visible = !flag2;
			this.factionBackgroundUpgradeRebel.Visible = (flag2 && !flag);
			this.neighborFactionBackgroundUpgradeRebel.Visible = (flag2 && !flag);
			this.factionBackgroundUpgradeEmpire.Visible = (flag2 & flag);
			this.neighborFactionBackgroundUpgradeEmpire.Visible = (flag2 & flag);
			if (!flag2)
			{
				this.neighborFactionBackground.SpriteName = factionUpgradeIcon;
				this.factionBackground.SpriteName = factionUpgradeIcon;
				return;
			}
			if (!flag)
			{
				this.neighborFactionBackgroundUpgradeRebel.SpriteName = factionUpgradeIcon;
				this.factionBackgroundUpgradeRebel.SpriteName = factionUpgradeIcon;
				return;
			}
			this.factionBackgroundUpgradeEmpire.SpriteName = factionUpgradeIcon;
			this.neighborFactionBackgroundUpgradeEmpire.SpriteName = factionUpgradeIcon;
		}

		private void RefreshAllResourceViews(bool animate)
		{
			this.RefreshResourceView("credits", animate);
			this.RefreshResourceView("materials", animate);
			this.RefreshResourceView("contraband", animate);
			this.RefreshResourceView("crystals", animate);
		}

		private void RefreshResourceView(string resourceKey, bool animate)
		{
			if (this.creditsView == null || this.materialsView == null || this.contrabandView == null || this.crystalsView == null)
			{
				return;
			}
			if (resourceKey == "credits")
			{
				this.creditsView.SetAmount(this.player.CurrentCreditsAmount, this.player.MaxCreditsAmount, animate);
			}
			else if (resourceKey == "materials")
			{
				this.materialsView.SetAmount(this.player.CurrentMaterialsAmount, this.player.MaxMaterialsAmount, animate);
			}
			else if (resourceKey == "contraband")
			{
				this.contrabandView.SetAmount(this.player.CurrentContrabandAmount, this.player.MaxContrabandAmount, animate);
			}
			else if (resourceKey == "crystals")
			{
				this.crystalsView.SetAmount(this.player.CurrentCrystalsAmount, -1, animate);
			}
			if (!this.registeredFrameTimeObserver && (this.creditsView.NeedsUpdate || this.materialsView.NeedsUpdate || this.contrabandView.NeedsUpdate || this.crystalsView.NeedsUpdate))
			{
				this.registeredFrameTimeObserver = true;
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			}
		}

		public void RefreshTimerView(int seconds, bool warning)
		{
			int num = seconds / 60;
			seconds -= num * 60;
			string text = Service.Get<Lang>().Get("MINUTES_SECONDS", new object[]
			{
				num,
				seconds
			});
			this.timeLeftLabel.Text = text;
			this.timeLeftLabel.TextColor = UXUtils.GetCostColor(this.timeLeftLabel, !warning || (seconds & 1) == 1, false);
		}

		public void RefreshReplayTimerView(int seconds)
		{
			BattleRecord currentBattleRecord = Service.Get<BattlePlaybackController>().CurrentBattleRecord;
			if (currentBattleRecord == null || currentBattleRecord.BattleAttributes == null)
			{
				return;
			}
			seconds -= currentBattleRecord.BattleAttributes.TimeLeft;
			int num = seconds / 60;
			seconds -= num * 60;
			string text = Service.Get<Lang>().Get("MINUTES_SECONDS", new object[]
			{
				num,
				seconds
			});
			this.replayTimeLeftLabel.Text = text;
		}

		private void SetOpponentLevelVisibility(bool vis)
		{
			this.opponentRankLabel.Visible = vis;
			this.opponentRankBG.Visible = vis;
		}

		private void RefreshBaseName()
		{
			WorldTransitioner worldTransitioner = Service.Get<WorldTransitioner>();
			this.baseNameLabel.Text = worldTransitioner.GetCurrentWorldName();
			if (!worldTransitioner.IsCurrentWorldHome())
			{
				bool flag = false;
				PvpTarget currentPvpTarget = Service.Get<PvpManager>().CurrentPvpTarget;
				bool flag2 = true;
				string spriteName;
				if (currentPvpTarget != null)
				{
					this.SetOpponentLevelVisibility(true);
					flag2 = (currentPvpTarget.PlayerFaction == FactionType.Empire);
					FactionIconUpgradeController factionIconUpgradeController = Service.Get<FactionIconUpgradeController>();
					int rating = currentPvpTarget.PlayerAttacksWon + currentPvpTarget.PlayerDefensesWon;
					spriteName = factionIconUpgradeController.GetIcon(currentPvpTarget.PlayerFaction, rating);
					flag = factionIconUpgradeController.UseUpgradeImage(rating);
					string text = factionIconUpgradeController.RatingToDisplayLevel(rating).ToString();
					this.opponentRankLabel.Text = text;
				}
				else
				{
					this.SetOpponentLevelVisibility(false);
					spriteName = worldTransitioner.GetCurrentWorldFactionAssetName();
				}
				this.opponentSymbol.Visible = !flag;
				this.opponentSymbolUpgradeRebel.Visible = (flag && !flag2);
				this.opponentSymbolUpgradeEmpire.Visible = (flag & flag2);
				if (flag)
				{
					if (!flag2)
					{
						this.opponentSymbolUpgradeRebel.SpriteName = spriteName;
						return;
					}
					this.opponentSymbolUpgradeEmpire.SpriteName = spriteName;
					return;
				}
				else
				{
					this.opponentSymbol.SpriteName = spriteName;
				}
			}
		}

		private void RefreshPlayerMedals()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			string text = Service.Get<Lang>().ThousandsSeparated(worldOwner.PlayerMedals);
			this.playerMedalLabel.Text = text;
			this.neighborMedalsLabel.Text = text;
		}

		private void RefreshCurrentPlayerLevel()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			int rating = GameUtils.CalculatePlayerVictoryRating(worldOwner);
			FactionIconUpgradeController factionIconUpgradeController = Service.Get<FactionIconUpgradeController>();
			string text = factionIconUpgradeController.RatingToDisplayLevel(rating).ToString();
			this.playerRankLabel.Text = text;
			this.neighborTrophiesLabel.Text = text;
		}

		public UXElement GetDamageStarAnchor()
		{
			return this.damageStarAnchor;
		}

		public UXElement GetDamageStarAnimator()
		{
			return this.damageStarAnimator;
		}

		public UXLabel GetDamageStarLabel()
		{
			return this.damageStarLabel;
		}

		public void UpdateDamageValue(int percentage)
		{
			this.damageValueLabel.Text = this.lang.Get("PERCENTAGE", new object[]
			{
				percentage
			});
		}

		public void UpdateDamageStars(int numEarnedStars)
		{
			this.damageStar1.Color = ((numEarnedStars > 0) ? Color.white : Color.black);
			this.damageStar2.Color = ((numEarnedStars > 1) ? Color.white : Color.black);
			this.damageStar3.Color = ((numEarnedStars > 2) ? Color.white : Color.black);
		}

		public void ResetDamageStars()
		{
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			if (currentBattle != null && currentBattle.Type == BattleType.PveDefend)
			{
				this.UpdateDamageStars(3);
				return;
			}
			this.UpdateDamageStars(0);
		}

		public void ShowContextButtons(Entity selectedBuilding)
		{
			AnimController animController = Service.Get<AnimController>();
			int i = 0;
			int count = this.curAnims.Count;
			while (i < count)
			{
				Anim anim = this.curAnims[i];
				animController.CompleteAnim(anim);
				UXElement uXElement = (UXElement)anim.Tag;
				uXElement.Root.SetActive(false);
				this.contextButtonPool.Add(uXElement.Root.name, uXElement);
				i++;
			}
			this.curAnims.Clear();
			this.contextDescLabel.Text = "";
			this.BaseLayoutToolView.SelectedBuildingLabel.Text = "";
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			bool flag = currentState is HomeState;
			bool flag2 = GameUtils.IsVisitingBase();
			bool flag3 = currentState is WarBoardState;
			bool flag4 = currentState is EditBaseState;
			bool flag5 = Service.Get<BaseLayoutToolController>().IsActive();
			BuildingController buildingController = Service.Get<BuildingController>();
			UXLabel selectedBuildingLabel = this.contextDescLabel;
			if (flag5)
			{
				selectedBuildingLabel = this.BaseLayoutToolView.SelectedBuildingLabel;
			}
			if (selectedBuilding == null || buildingController.IsPurchasing || (!flag && !flag2 && !flag4 && !flag5 && !flag3) || this.isAnimating)
			{
				selectedBuildingLabel.Text = "";
				return;
			}
			bool isLifted = buildingController.IsLifted(selectedBuilding);
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			if (buildingComponent == null)
			{
				return;
			}
			BuildingTypeVO buildingType = buildingComponent.BuildingType;
			if (buildingType.Type == BuildingType.Clearable)
			{
				selectedBuildingLabel.Text = this.lang.Get("CLEARABLE_INFO", new object[]
				{
					LangUtils.GetClearableDisplayName(buildingType),
					buildingType.Lvl
				});
			}
			else
			{
				selectedBuildingLabel.Text = this.lang.Get("BUILDING_INFO", new object[]
				{
					LangUtils.GetBuildingDisplayName(buildingType),
					buildingType.Lvl
				});
			}
			bool visible = this.Visible;
			if (!visible)
			{
				this.Visible = true;
			}
			this.contextButtonParentTemplate.Visible = true;
			this.AddContextButtons(selectedBuilding, buildingType, flag, flag2, flag5, isLifted);
			int count2 = this.contextButtons.Count;
			float colliderWidth = this.contextButtonTemplate.ColliderWidth;
			float num = (float)(1 - count2) * 0.5f * colliderWidth;
			for (int j = 0; j < count2; j++)
			{
				UXElement uXElement2 = this.contextButtons[j];
				UXButton element = base.GetElement<UXButton>(UXUtils.FormatAppendedName("ButtonContext", uXElement2.Root.name));
				if (!element.Enabled)
				{
					element = base.GetElement<UXButton>(UXUtils.FormatAppendedName("ButtonContextDim", uXElement2.Root.name));
				}
				Vector3 localPosition = this.contextButtonParentTemplate.LocalPosition;
				localPosition.x = (float)j * colliderWidth + num;
				Vector3 vector = localPosition;
				vector.y = -vector.y;
				uXElement2.LocalPosition = vector;
				if (flag5)
				{
					UXElement element2 = base.GetElement<UXElement>("StashContextLocator");
					localPosition.y = element2.LocalPosition.y;
				}
				AnimUXPosition animUXPosition = new AnimUXPosition(uXElement2, 0.5f, localPosition);
				animUXPosition.EaseFunctionX = new Easing.EasingDelegate(Easing.AlwaysStart);
				animUXPosition.EaseFunctionY = new Easing.EasingDelegate(Easing.ExpoEaseOut);
				animUXPosition.EaseFunctionZ = new Easing.EasingDelegate(Easing.AlwaysStart);
				animUXPosition.Tag = uXElement2;
				this.curAnims.Add(animUXPosition);
			}
			this.contextButtonParentTemplate.Visible = false;
			if (!visible)
			{
				this.Visible = false;
			}
			int k = 0;
			int count3 = this.curAnims.Count;
			while (k < count3)
			{
				animController.Animate(this.curAnims[k]);
				k++;
			}
		}

		private void AddContextButtons(Entity selectedBuilding, BuildingTypeVO buildingInfo, bool inHomeMode, bool inVisitMode, bool isBaseLayoutToolMode, bool isLifted)
		{
			RaidDefenseController @object = Service.Get<RaidDefenseController>();
			int numSelectedBuildings = Service.Get<BuildingController>().NumSelectedBuildings;
			bool flag = Service.Get<GameStateMachine>().CurrentState is WarBaseEditorState;
			this.contextButtons.Clear();
			if (buildingInfo.Type == BuildingType.Clearable)
			{
				if (!inVisitMode)
				{
					if (ContractUtils.IsBuildingClearing(selectedBuilding))
					{
						this.AddContextButton("Cancel", 0, 0, 0, 0);
						return;
					}
					this.AddContextButton("Clear", buildingInfo.Credits, buildingInfo.Materials, buildingInfo.Contraband, 0);
				}
				return;
			}
			if (!isLifted && numSelectedBuildings <= 1)
			{
				if (!isBaseLayoutToolMode)
				{
					this.AddContextButton("Info", 0, 0, 0, 0);
				}
				if (inHomeMode)
				{
					this.AddContextButton("Move", 0, 0, 0, 0);
				}
			}
			if (Service.Get<PostBattleRepairController>().IsEntityInRepair(selectedBuilding))
			{
				return;
			}
			if (isBaseLayoutToolMode && !isLifted)
			{
				this.AddContextButton("Stash", 0, 0, 0, 0);
			}
			if (numSelectedBuildings > 1 && buildingInfo.Type == BuildingType.Wall)
			{
				this.AddContextButton("RotateWall", 0, 0, 0, 0);
			}
			if (!inVisitMode && !isLifted && numSelectedBuildings <= 1)
			{
				if (buildingInfo.Type == BuildingType.Squad)
				{
					this.AddSquadContextButtons(selectedBuilding, buildingInfo, inHomeMode, isBaseLayoutToolMode);
				}
				if (buildingInfo.Type == BuildingType.Wall && !inHomeMode)
				{
					this.AddContextButton("SelectRow", 0, 0, 0, 0);
				}
				if (buildingInfo.Type == BuildingType.DroidHut && !flag)
				{
					if (this.player.CurrentDroidsAmount < this.player.MaxDroidsAmount)
					{
						int crystals = GameUtils.DroidCrystalCost(this.player.CurrentDroidsAmount);
						this.AddContextButton("Buy_Droid", 0, 0, 0, crystals);
						return;
					}
				}
				else
				{
					if (!isBaseLayoutToolMode && ContractUtils.IsBuildingConstructing(selectedBuilding))
					{
						this.AddFinishContextButton(selectedBuilding);
						return;
					}
					if (!isBaseLayoutToolMode && (ContractUtils.IsBuildingUpgrading(selectedBuilding) || ContractUtils.IsBuildingSwapping(selectedBuilding) || ContractUtils.IsChampionRepairing(selectedBuilding)))
					{
						this.AddContextButton("Cancel", 0, 0, 0, 0);
						this.AddFinishContextButton(selectedBuilding);
						BuildingType type = buildingInfo.Type;
						if (type == BuildingType.HQ)
						{
							this.AddHQInventoryContextButtonIfProper();
							return;
						}
						if (type == BuildingType.NavigationCenter && inHomeMode)
						{
							this.AddContextButton("Navigate", 0, 0, 0, 0, "context_Galaxy");
							return;
						}
					}
					else
					{
						bool flag2 = buildingInfo.Type == BuildingType.ChampionPlatform && !Service.Get<ChampionController>().IsChampionAvailable((SmartEntity)selectedBuilding);
						if (GameUtils.IsBuildingUpgradable(buildingInfo) && !flag2 && !isBaseLayoutToolMode)
						{
							BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
							if (Service.Get<ISupportController>().FindBuildingContract(buildingComponent.BuildingTO.Key) == null)
							{
								BuildingTypeVO nextLevel = Service.Get<BuildingUpgradeCatalog>().GetNextLevel(buildingInfo);
								this.AddContextButton("Upgrade", nextLevel.UpgradeCredits, nextLevel.UpgradeMaterials, nextLevel.UpgradeContraband, 0);
							}
						}
						if (buildingInfo.Type == BuildingType.Trap && !isBaseLayoutToolMode)
						{
							if (selectedBuilding.Get<TrapComponent>().CurrentState == TrapState.Spent)
							{
								TrapTypeVO trapTypeVO = Service.Get<IDataController>().Get<TrapTypeVO>(buildingInfo.TrapUid);
								this.AddContextButton("Trap_Rearm", trapTypeVO.RearmCreditsCost, trapTypeVO.RearmMaterialsCost, trapTypeVO.RearmContrabandCost, 0);
							}
							List<Entity> rearmableTraps = TrapUtils.GetRearmableTraps();
							if (rearmableTraps.Count > 1 || rearmableTraps.Count == 0 || rearmableTraps[0] != selectedBuilding)
							{
								int credits;
								int materials;
								int contraband;
								TrapUtils.GetRearmAllTrapsCost(out credits, out materials, out contraband);
								this.AddContextButton("Trap_RearmAll", credits, materials, contraband, 0);
							}
						}
						if (inHomeMode)
						{
							switch (buildingInfo.Type)
							{
							case BuildingType.HQ:
								this.AddHQInventoryContextButtonIfProper();
								return;
							case BuildingType.Barracks:
								this.AddContextButton("Train", 0, 0, 0, 0);
								return;
							case BuildingType.Factory:
								this.AddContextButton("Build", 0, 0, 0, 0);
								return;
							case BuildingType.FleetCommand:
								this.AddContextButton("Commission", 0, 0, 0, 0);
								return;
							case BuildingType.HeroMobilizer:
								this.AddContextButton("Mobilize", 0, 0, 0, 0);
								return;
							case BuildingType.ChampionPlatform:
								if (flag2)
								{
									this.AddContextButton("Repair", 0, 0, 0, 0);
									return;
								}
								break;
							case BuildingType.Housing:
							case BuildingType.Squad:
							case BuildingType.Starport:
							case BuildingType.DroidHut:
							case BuildingType.Wall:
							case BuildingType.Storage:
							case BuildingType.ShieldGenerator:
							case BuildingType.Rubble:
							case BuildingType.Clearable:
							case BuildingType.Blocker:
							case BuildingType.Trap:
								break;
							case BuildingType.Turret:
								if (Service.Get<BuildingLookupController>().IsTurretSwappingUnlocked())
								{
									this.AddContextButton("Swap", 0, 0, 0, 0);
									return;
								}
								break;
							case BuildingType.TroopResearch:
								if (ContractUtils.IsArmyUpgrading(selectedBuilding))
								{
									if (ContractUtils.CanCancelDeployableContract(selectedBuilding))
									{
										this.AddContextButton("Cancel", 0, 0, 0, 0);
									}
									this.AddFinishContextButton(selectedBuilding);
								}
								else if (ContractUtils.IsEquipmentUpgrading(selectedBuilding))
								{
									this.AddFinishContextButton(selectedBuilding);
								}
								this.AddContextButton("Upgrade_Troops", 0, 0, 0, 0);
								return;
							case BuildingType.DefenseResearch:
								this.AddContextButton("Upgrade_Defense", 0, 0, 0, 0);
								return;
							case BuildingType.Resource:
							{
								string contextId = null;
								switch (buildingInfo.Currency)
								{
								case CurrencyType.Credits:
									contextId = "Credits";
									break;
								case CurrencyType.Materials:
									contextId = "Materials";
									break;
								case CurrencyType.Contraband:
									contextId = "Contraband";
									break;
								}
								this.AddContextButton(contextId, 0, 0, 0, 0);
								return;
							}
							case BuildingType.Cantina:
								this.AddContextButton("Hire", 0, 0, 0, 0);
								return;
							case BuildingType.NavigationCenter:
								if (!isLifted)
								{
									this.AddContextButton("Navigate", 0, 0, 0, 0, "context_Galaxy");
								}
								break;
							case BuildingType.ScoutTower:
								if (!Service.Get<RaidDefenseController>().IsRaidAvailable())
								{
									this.AddContextButtonWithTimer("NextRaid", 0, 0, 0, 0, "context_NightRaid", new GetTimerSecondsDelegate(@object.GetRaidTimeSeconds));
									return;
								}
								this.AddContextButton("RaidBriefing", 0, 0, 0, 0, "context_NightRaid");
								this.AddContextButton("RaidDefend", 0, 0, 0, 0, "icoDefend", "BtnTroopBg_Gold");
								return;
							case BuildingType.Armory:
								this.AddContextButton("Armory", 0, 0, 0, 0);
								return;
							default:
								return;
							}
						}
					}
				}
			}
		}

		private void AddHQInventoryContextButtonIfProper()
		{
			if (!this.player.CampaignProgress.FueInProgress)
			{
				int numInventoryItemsNotViewed = GameUtils.GetNumInventoryItemsNotViewed();
				this.AddContextButton("Inventory", 0, 0, 0, 0, numInventoryItemsNotViewed, null, null);
			}
		}

		private void AddSquadContextButtons(Entity selectedBuilding, BuildingTypeVO buildingInfo, bool inHomeMode, bool isBaseLayoutToolMode)
		{
			if (isBaseLayoutToolMode)
			{
				return;
			}
			if (Service.Get<SquadController>().StateManager.GetCurrentSquad() != null)
			{
				SquadController squadController = Service.Get<SquadController>();
				if (SquadUtils.GetDonatedTroopStorageUsedByCurrentPlayer() < buildingInfo.Storage)
				{
					uint serverTime = Service.Get<ServerAPI>().ServerTime;
					uint troopRequestDate = squadController.StateManager.TroopRequestDate;
					if (SquadUtils.CanSendFreeTroopRequest(serverTime, troopRequestDate))
					{
						this.AddContextButton("RequestTroops", 0, 0, 0, 0);
						return;
					}
					int troopRequestCrystalCost = SquadUtils.GetTroopRequestCrystalCost(serverTime, troopRequestDate);
					this.AddContextButton("RequestTroopsPaid", 0, 0, 0, troopRequestCrystalCost, "context_Finish_Now");
					return;
				}
			}
			else if (inHomeMode)
			{
				this.AddContextButton("Join", 0, 0, 0, 0, "context_Squad");
			}
		}

		private void AddFinishContextButton(Entity selectedBuilding)
		{
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(selectedBuilding.Get<BuildingComponent>().BuildingTO.Key);
			int crystalCostToFinishContract = ContractUtils.GetCrystalCostToFinishContract(contract);
			this.AddContextButton("Finish_Now", 0, 0, 0, crystalCostToFinishContract);
		}

		public void AddCustomContextButton(string contextId)
		{
			this.AddContextButton(contextId, 0, 0, 0, 0, 0, null, null);
		}

		private void AddContextButton(string contextId, int credits, int materials, int contraband, int crystals)
		{
			this.AddContextButton(contextId, credits, materials, contraband, crystals, 0, null, null);
		}

		private void AddContextButton(string contextId, int credits, int materials, int contraband, int crystals, string spriteName)
		{
			this.AddContextButton(contextId, credits, materials, contraband, crystals, 0, spriteName, null);
		}

		private void AddContextButtonWithTimer(string contextId, int credits, int materials, int contraband, int crystals, string spriteName, GetTimerSecondsDelegate getTimeSeconds)
		{
			ContextButtonTag contextButtonTag = this.AddContextButton(contextId, credits, materials, contraband, crystals, 0, spriteName, null);
			contextButtonTag.TimerLabel = contextButtonTag.HardCostLabel;
			contextButtonTag.TimerSecondsDelegate = getTimeSeconds;
			this.UpdateContextTimerLabel(contextButtonTag);
		}

		private void AddContextButton(string contextId, int credits, int materials, int contraband, int crystals, string spriteName, string bgSpriteName)
		{
			this.AddContextButton(contextId, credits, materials, contraband, crystals, 0, spriteName, bgSpriteName);
		}

		private UXElement GetContextButtonFromPool(UXElement template, string name, GameObject parent)
		{
			if (this.contextButtonPool.ContainsKey(name))
			{
				UXElement uXElement = this.contextButtonPool[name];
				this.contextButtonPool.Remove(name);
				uXElement.Root.SetActive(true);
				return uXElement;
			}
			return base.CloneElement<UXElement>(template, name, parent);
		}

		private ContextButtonTag AddContextButton(string contextId, int credits, int materials, int contraband, int crystals, int badgeCount, string spriteName, string bgSpriteName)
		{
			string text = string.Format("{0}_{1}", new object[]
			{
				this.contextButtonParentTemplate.Root.name,
				contextId
			});
			UXElement contextButtonFromPool = this.GetContextButtonFromPool(this.contextButtonParentTemplate, text, this.contextButtonParentTemplate.Root.transform.parent.gameObject);
			ContextButtonTag contextButtonTag = new ContextButtonTag();
			contextButtonFromPool.Tag = contextButtonTag;
			contextButtonTag.ContextButton = base.GetElement<UXButton>(UXUtils.FormatAppendedName("ButtonContext", text));
			contextButtonTag.ContextButton.OnClicked = new UXButtonClickedDelegate(this.OnContextButtonClicked);
			contextButtonTag.ContextButton.Tag = contextId;
			contextButtonTag.ContextDimButton = base.GetElement<UXButton>(UXUtils.FormatAppendedName("ButtonContextDim", text));
			contextButtonTag.ContextDimButton.Enabled = false;
			contextButtonTag.ContextDimButton.Tag = contextId;
			contextButtonTag.ContextDimButton.OnClicked = new UXButtonClickedDelegate(this.OnDisabledContextButtonClicked);
			contextButtonTag.ContextId = contextId;
			contextButtonTag.ContextBackground = base.GetElement<UXSprite>(UXUtils.FormatAppendedName("BackgroundContext", text));
			if (!string.IsNullOrEmpty(bgSpriteName))
			{
				contextButtonTag.ContextBackground.SpriteName = bgSpriteName;
			}
			if (spriteName != null)
			{
				contextButtonTag.SpriteName = spriteName;
			}
			else
			{
				contextButtonTag.SpriteName = "context_" + contextButtonTag.ContextId;
			}
			contextButtonTag.ContextIconSprite = base.GetElement<UXSprite>(UXUtils.FormatAppendedName("SpriteContextIcon", text));
			contextButtonTag.ContextIconSprite.SpriteName = contextButtonTag.SpriteName;
			contextButtonTag.ContextLabel = base.GetElement<UXLabel>(UXUtils.FormatAppendedName("LabelContext", text));
			contextButtonTag.ContextLabel.Text = LangUtils.GetContextButtonText(contextButtonTag.ContextId);
			contextButtonTag.HardCostLabel = base.GetElement<UXLabel>(UXUtils.FormatAppendedName("LabelHardCost", text));
			contextButtonTag.HardCostLabel.Text = ((crystals == 0) ? "" : this.lang.ThousandsSeparated(crystals));
			contextButtonTag.HardCostLabel.TextColor = UXUtils.GetCostColor(contextButtonTag.HardCostLabel, GameUtils.CanAffordCrystals(crystals), false);
			UXElement element = base.GetElement<UXElement>(UXUtils.FormatAppendedName("ContainerJewelContext", text));
			if (badgeCount > 0)
			{
				element.Visible = true;
				base.GetElement<UXLabel>(UXUtils.FormatAppendedName("LabelMessageCountContext", text)).Text = badgeCount.ToString();
			}
			else
			{
				element.Visible = false;
			}
			UXUtils.SetupCostElements(this, "Cost", text, credits, materials, contraband, 0, false, null, 108);
			this.ToggleContextButton(contextButtonFromPool, !this.ShouldDisableContextButton(contextId, credits, materials, contraband, crystals));
			this.contextButtons.Add(contextButtonFromPool);
			return contextButtonTag;
		}

		private bool ShouldDisableContextButton(string contextId, int credits, int materials, int contraband, int crystals)
		{
			if (contextId == "Credits" || contextId == "Materials" || contextId == "Contraband")
			{
				return !Service.Get<ICurrencyController>().IsGeneratorCollectable(Service.Get<BuildingController>().SelectedBuilding);
			}
			return contextId == "Trap_RearmAll" && (credits == 0 && materials == 0) && crystals == 0;
		}

		public void ToggleContextButton(string contextId, bool enabled)
		{
			if (this.contextButtons == null)
			{
				return;
			}
			UXElement button = null;
			for (int i = 0; i < this.contextButtons.Count; i++)
			{
				if ((this.contextButtons[i].Tag as ContextButtonTag).ContextId == contextId)
				{
					button = this.contextButtons[i];
					break;
				}
			}
			this.ToggleContextButton(button, enabled);
		}

		public void ToggleContextButton(UXElement button, bool enabled)
		{
			if (button == null)
			{
				return;
			}
			ContextButtonTag contextButtonTag = button.Tag as ContextButtonTag;
			contextButtonTag.ContextDimButton.Enabled = !enabled;
			contextButtonTag.ContextDimButton.Visible = !enabled;
			contextButtonTag.ContextButton.Enabled = enabled;
			contextButtonTag.ContextLabel.TextColor = UXUtils.GetCostColor(contextButtonTag.ContextLabel, true, !enabled);
		}

		private bool IsAnimationWhiteListed(GameObject gameObject)
		{
			bool result = false;
			if (gameObject != null)
			{
				string name = gameObject.name;
				int num = this.ANIMATION_TRANSITION_WHITE_LIST.Length;
				for (int i = 0; i < num; i++)
				{
					if (name == this.ANIMATION_TRANSITION_WHITE_LIST[i])
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private void AnimateControls(bool bringIn, float speed)
		{
			if (bringIn == this.broughtIn)
			{
				return;
			}
			float num = 0f;
			int i = 0;
			int num2 = this.animations.Length;
			while (i < num2)
			{
				Animation animation = this.animations[i];
				GameObject gameObject = animation.gameObject;
				if (this.IsAnimationWhiteListed(gameObject))
				{
					using (IEnumerator enumerator = animation.gameObject.GetComponent<Animation>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AnimationState animationState = (AnimationState)enumerator.get_Current();
							if (bringIn)
							{
								animationState.speed = speed;
								animationState.time = 0f;
							}
							else
							{
								animationState.speed = -speed;
								animationState.time = animationState.length;
							}
							if (animationState.length > num & bringIn)
							{
								num = animationState.length;
							}
						}
					}
					this.animations[i].Play();
				}
				i++;
			}
			if (num > 0f)
			{
				this.isAnimating = true;
				Service.Get<ViewTimerManager>().CreateViewTimer(num, false, new TimerDelegate(this.OnAnimationComplete), null);
			}
			this.broughtIn = bringIn;
		}

		private void OnAnimationComplete(uint timerId, object cookie)
		{
			this.isAnimating = false;
			this.Visible = !this.setInvisibleAfterAnimating;
			this.setInvisibleAfterAnimating = false;
		}

		public void UpdateDroidCount()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is HomeState || currentState is EditBaseState)
			{
				HomeModeController homeModeController = Service.Get<HomeModeController>();
				if (homeModeController != null && homeModeController.Enabled)
				{
					int num = ContractUtils.CalculateDroidsInUse();
					int currentDroidsAmount = this.player.CurrentDroidsAmount;
					int num2 = currentDroidsAmount - num;
					string text = this.lang.Get("FRACTION", new object[]
					{
						num2,
						currentDroidsAmount
					});
					this.droidAddLabel.Text = text;
					this.droidMaxLabel.Text = text;
					bool flag = this.player.CurrentDroidsAmount < this.player.MaxDroidsAmount;
					this.droidAddGroup.Visible = flag;
					this.droidMaxGroup.Visible = !flag;
				}
			}
		}

		public void ToggleExitEditModeButton(bool show)
		{
			if (show)
			{
				this.CurrentHudConfig.Add("ButtonExitEdit");
			}
			else
			{
				this.CurrentHudConfig.Remove("ButtonExitEdit");
			}
			this.exitEditButton.Visible = show;
			if (show && this.exitEditAnimation != null)
			{
				this.exitEditAnimation.Play();
			}
		}

		public void ConfigureControls(HudConfig config)
		{
			if (!base.IsLoaded())
			{
				return;
			}
			this.CurrentHudConfig = config;
			if (this.genericConfigElements == null)
			{
				this.genericConfigElements = new List<UXElement>();
				this.genericConfigElements.Add(this.currencyGroup);
				this.genericConfigElements.Add(this.droidButton);
				this.genericConfigElements.Add(this.crystalButton);
				this.genericConfigElements.Add(this.opponentGroup);
				this.genericConfigElements.Add(this.prebattleMedalsGroup);
				this.genericConfigElements.Add(this.playerGroup);
				this.genericConfigElements.Add(this.shieldGroup);
				this.genericConfigElements.Add(this.specialPromotionGroup);
				this.genericConfigElements.Add(this.baseNameLabel);
				this.genericConfigElements.Add(this.homeButton);
				this.genericConfigElements.Add(this.editButton);
				this.genericConfigElements.Add(this.exitEditButton);
				this.genericConfigElements.Add(this.storeButton);
				this.genericConfigElements.Add(this.battleButton);
				this.genericConfigElements.Add(this.warButton);
				this.genericConfigElements.Add(this.logButton);
				this.genericConfigElements.Add(this.leaderboardButton);
				this.genericConfigElements.Add(this.settingsButton);
				this.genericConfigElements.Add(this.joinSquadButton);
				this.genericConfigElements.Add(this.endBattleButton);
				this.genericConfigElements.Add(this.damageStarGroup);
				this.genericConfigElements.Add(this.replayControlsGroup);
				this.genericConfigElements.Add(this.nextBattleButton);
				this.genericConfigElements.Add(this.deployInstructionsLabel);
				this.genericConfigElements.Add(this.neighborGroup);
				this.genericConfigElements.Add(this.holonetButton);
				this.BaseLayoutToolView.AddHUDBaseLayoutToolElements(this.genericConfigElements);
			}
			int i = 0;
			int count = this.genericConfigElements.Count;
			while (i < count)
			{
				UXElement uXElement = this.genericConfigElements[i];
				if (uXElement != null)
				{
					if (uXElement.Root != null && config.Has(uXElement.Root.name))
					{
						uXElement.Visible = true;
						uXElement.Enabled = true;
					}
					else
					{
						uXElement.Visible = false;
					}
				}
				i++;
			}
			if (this.currencyGroup.Visible)
			{
				bool isContrabandUnlocked = this.player.IsContrabandUnlocked;
				this.contrabandView.Visible = isContrabandUnlocked;
				Vector3 a = base.GetElement<UXElement>("Materials").LocalPosition - base.GetElement<UXElement>("Credits").LocalPosition;
				this.crystalsDroidsGroup.LocalPosition = (isContrabandUnlocked ? (a * 3f) : (a * 2f));
			}
			if (this.exitEditButton.Visible && this.exitEditAnimation != null)
			{
				this.exitEditAnimation.Play();
			}
			if (config.Has("LabelTimeLeft"))
			{
				int timeLeft = Service.Get<BattleController>().GetCurrentBattle().TimeLeft;
				if (timeLeft > 0)
				{
					this.timeLeftLabel.Visible = true;
					this.RefreshTimerView(timeLeft, false);
				}
				else
				{
					this.timeLeftLabel.Visible = false;
				}
			}
			else
			{
				this.timeLeftLabel.Visible = false;
			}
			if (config.Has("LabelCurrencyValueOpponent"))
			{
				this.ShowLootElements();
				this.RefreshLoot();
			}
			else
			{
				this.HideLootElements();
			}
			if (config.Has("TroopsGrid"))
			{
				this.SetupDeployableTroops();
			}
			else
			{
				this.CleanupDeployableTroops();
			}
			if (config.Has("LabelDeployInstructions"))
			{
				this.UpdateDeployInstructionLabel();
			}
			if (config.Has("ReplayControls"))
			{
				this.UpdateCurrentReplaySpeedUI();
				this.replayTimeLeftLabel.Visible = false;
			}
			if (config.Has("ButtonNextBattle"))
			{
				this.nextBattleButton.Enabled = false;
				int pvpMatchCost = Service.Get<PvpManager>().GetPvpMatchCost();
				UXUtils.SetupSingleCostElement(this, "CostNextBattle", pvpMatchCost, 0, 0, 0, 0, false, "");
			}
			if (config.Has("ButtonBattle") && !this.player.CampaignProgress.FueInProgress)
			{
				int num = 0;
				num += Service.Get<TournamentController>().NumberOfTournamentsNotViewed();
				num += Service.Get<ObjectiveManager>().GetCompletedObjectivesCount();
				this.battleJewel.Value = num;
			}
			if (Service.Get<SquadController>().StateManager.GetCurrentSquad() != null)
			{
				this.joinSquadButton.Visible = false;
				IState currentState = Service.Get<GameStateMachine>().CurrentState;
				if (currentState is HomeState || config.Has("SquadScreen"))
				{
					this.CreateSquadScreen();
				}
			}
			else
			{
				this.DestroySquadScreen();
			}
			if (this.player.CampaignProgress.FueInProgress)
			{
				this.warButton.Visible = false;
			}
			else
			{
				this.UpdateWarButton();
			}
			if (config.Has("ButtonStore"))
			{
				this.UpdateStoreJewel();
			}
			this.warAttackButton = base.GetElement<UXButton>("BtnSquadwarAttack");
			this.warAttackButton.Visible = false;
			this.warAttackLabel = base.GetElement<UXLabel>("LabelBtnSquadwarAttack");
			this.warAttackLabel.Text = this.lang.Get("WAR_START_ATTACK", new object[0]);
			this.warDoneButton = base.GetElement<UXButton>("BtnSquadwarBack");
			this.warDoneButton.Visible = false;
			this.warUplinks = base.GetElement<UXElement>("AvailableUplinks");
			this.warUplinks.Visible = false;
			Service.Get<EventManager>().UnregisterObserver(this, EventId.WarAttackCommandFailed);
			if (config.Has("WarAttack"))
			{
				if (config.Has("WarAttackOpponent"))
				{
					this.warAttackButton.Visible = true;
					this.warAttackButton.OnClicked = new UXButtonClickedDelegate(this.OnWarAttackClicked);
				}
				this.warDoneButton.Visible = true;
				this.warDoneButton.Enabled = true;
				this.warDoneButton.OnClicked = new UXButtonClickedDelegate(this.OnHomeButtonClicked);
				base.GetElement<UXLabel>("LabelBtnSquadwarBack").Text = this.lang.Get("WAR_SCOUT_CANCEL", new object[0]);
				CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
				if (currentBattle.Type == BattleType.PvpAttackSquadWar)
				{
					this.warUplinks.Visible = true;
					base.GetElement<UXLabel>("LabelAvailableUplinks").Text = this.lang.Get("WAR_SCOUT_POINTS_LEFT", new object[0]);
					this.ShowScoutingUplinksAvailable();
				}
				this.UpdateWarAttackState();
				if (this.deployableTroops != null)
				{
					this.FullyDisableDeployableControls(false);
				}
			}
			else if (config.Has("WarAttackStarted") && this.deployableTroops != null)
			{
				this.FullyEnableDeployablControls();
			}
			BuffController buffController = Service.Get<BuffController>();
			UXElement element = base.GetElement<UXElement>("PanelBuffsOpponentSquadWars");
			UXLabel element2 = base.GetElement<UXLabel>("LabelBuffsYoursSquadWars");
			UXElement element3 = base.GetElement<UXElement>("PanelBuffsYoursSquadWars");
			element3.Visible = false;
			element2.Visible = false;
			element.Visible = false;
			if (config.Has("BuffsYoursSquadWars"))
			{
				List<WarBuffVO> listOfWarBuffsBasedOnTeam = buffController.GetListOfWarBuffsBasedOnTeam(TeamType.Attacker);
				int count2 = listOfWarBuffsBasedOnTeam.Count;
				if (count2 > 0)
				{
					element3.Visible = true;
					element2.Visible = true;
					element2.Text = this.lang.Get("WAR_BATTLE_CURRENT_ADVANTAGES", new object[0]);
					UXGrid element4 = base.GetElement<UXGrid>("GridBuffsYoursSquadWars");
					element4.Clear();
					element4.SetTemplateItem("SpriteBuffsYoursSquadWars");
					for (int j = 0; j < count2; j++)
					{
						UXSprite uXSprite = (UXSprite)element4.CloneTemplateItem("SpriteBuffsYoursSquadWars" + j);
						uXSprite.SpriteName = listOfWarBuffsBasedOnTeam[j].BuffIcon;
						element4.AddItem(uXSprite, j);
					}
					element4.RepositionItems();
				}
			}
			if (config.Has("BuffsOpponentsSquadWars"))
			{
				List<WarBuffVO> listOfWarBuffsBasedOnTeam2 = buffController.GetListOfWarBuffsBasedOnTeam(TeamType.Defender);
				int count3 = listOfWarBuffsBasedOnTeam2.Count;
				if (count3 > 0)
				{
					element.Visible = true;
					UXGrid element5 = base.GetElement<UXGrid>("GridBuffsOpponentSquadWars");
					element5.Clear();
					element5.SetTemplateItem("SpriteBuffOpponentSquadWars");
					for (int k = 0; k < count3; k++)
					{
						UXSprite uXSprite2 = (UXSprite)element5.CloneTemplateItem("SpriteBuffOpponentSquadWars" + k);
						uXSprite2.SpriteName = listOfWarBuffsBasedOnTeam2[k].BuffIcon;
						element5.AddItem(uXSprite2, k);
					}
					element5.RepositionItems();
				}
			}
			if (config.Has("SpecialPromo"))
			{
				this.UpdateTargetedBundleButtonVisibility();
			}
			this.RefreshAllResourceViews(true);
			this.RefreshBaseName();
			this.RefreshPlayerSocialInformation();
			this.RefreshCurrentPlayerLevel();
			this.RefreshPlayerMedals();
			this.UpdateProtectionTimeLabel();
			if (this.persistentSquadScreen != null)
			{
				this.persistentSquadScreen.RefreshVisibility();
			}
			this.AnimateControls(true, 1f);
		}

		private void UpdateWarAttackState()
		{
			if (this.warAttackButton == null || this.warAttackLabel == null || !this.warAttackButton.Visible)
			{
				return;
			}
			SquadWarScoutState squadWarScoutState = SquadWarScoutState.Invalid;
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			if (currentBattle.Type == BattleType.PvpAttackSquadWar)
			{
				squadWarScoutState = Service.Get<SquadController>().WarManager.CanAttackCurrentlyScoutedOpponent();
			}
			else if (currentBattle.Type == BattleType.PveBuffBase)
			{
				squadWarScoutState = Service.Get<SquadController>().WarManager.CanAttackCurrentlyScoutedBuffBase();
			}
			if (squadWarScoutState != SquadWarScoutState.Invalid)
			{
				this.warAttackButton.Enabled = true;
				this.warAttackButton.Tag = squadWarScoutState;
				if (squadWarScoutState != SquadWarScoutState.AttackAvailable)
				{
					this.DisableWarAttacksUI();
					return;
				}
				this.warAttackButton.VisuallyEnableButton();
				this.warAttackLabel.TextColor = UXUtils.COLOR_ENABLED;
			}
		}

		private void ShowScoutingUplinksAvailable()
		{
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			SquadWarParticipantState currentOpponentState = warManager.GetCurrentOpponentState();
			if (currentOpponentState == null)
			{
				Service.Get<StaRTSLogger>().Warn("Could not find opponent's squad war data");
				return;
			}
			int wAR_VICTORY_POINTS = GameConstants.WAR_VICTORY_POINTS;
			int victoryPointsLeft = currentOpponentState.VictoryPointsLeft;
			if (victoryPointsLeft > wAR_VICTORY_POINTS || victoryPointsLeft < 0)
			{
				Service.Get<StaRTSLogger>().Warn("Invalid number of uplinks available");
			}
			int num = wAR_VICTORY_POINTS - victoryPointsLeft;
			for (int i = 1; i <= wAR_VICTORY_POINTS; i++)
			{
				UXUtils.UpdateUplinkHelper(base.GetElement<UXSprite>("SpriteUplink" + i), i > num, false);
			}
		}

		private void OnYourBuffsButtonClicked(UXButton btn)
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is BattlePlayState)
			{
				return;
			}
			BuffController buffController = Service.Get<BuffController>();
			List<WarBuffVO> listOfWarBuffsBasedOnTeam = buffController.GetListOfWarBuffsBasedOnTeam(TeamType.Attacker);
			int count = listOfWarBuffsBasedOnTeam.Count;
			if (count > 0)
			{
				Service.Get<UXController>().MiscElementsManager.ShowHudBuffToolTip(btn, listOfWarBuffsBasedOnTeam, true);
			}
		}

		private void OnOpponentBuffsButtonClicked(UXButton btn)
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is BattlePlayState)
			{
				return;
			}
			BuffController buffController = Service.Get<BuffController>();
			List<WarBuffVO> listOfWarBuffsBasedOnTeam = buffController.GetListOfWarBuffsBasedOnTeam(TeamType.Defender);
			int count = listOfWarBuffsBasedOnTeam.Count;
			if (count > 0)
			{
				Service.Get<UXController>().MiscElementsManager.ShowHudBuffToolTip(btn, listOfWarBuffsBasedOnTeam, false);
			}
		}

		public override void RefreshView()
		{
			if (this.CurrentHudConfig != null)
			{
				this.ConfigureControls(this.CurrentHudConfig);
			}
		}

		private void OnWarAttackClicked(UXButton button)
		{
			SquadWarScoutState squadWarScoutState = (SquadWarScoutState)button.Tag;
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			if (squadWarScoutState == SquadWarScoutState.AttackAvailable)
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.WarAttackCommandFailed);
				button.Enabled = false;
				this.warDoneButton.Enabled = false;
				if (currentBattle.Type == BattleType.PvpAttackSquadWar)
				{
					Service.Get<SquadController>().WarManager.StartAttackOnScoutedWarMember();
					return;
				}
				if (currentBattle.Type == BattleType.PveBuffBase)
				{
					Service.Get<SquadController>().WarManager.StartAttackOnScoutedBuffBase();
					return;
				}
			}
			else
			{
				this.ShowScoutAttackFailureMessage(squadWarScoutState, currentBattle);
			}
		}

		private void ShowScoutAttackFailureMessage(SquadWarScoutState state, CurrentBattle battle)
		{
			if (state == SquadWarScoutState.Invalid)
			{
				Service.Get<StaRTSLogger>().Error("Attempting to start squad war battle when in invalid scouting state. Only happens if data is bad/nonexistent");
			}
			bool flag = battle.Type == BattleType.PvpAttackSquadWar;
			if (!flag && state == SquadWarScoutState.DestinationUnavailable)
			{
				string currentlyScoutedBuffBaseId = Service.Get<SquadController>().WarManager.GetCurrentlyScoutedBuffBaseId();
				WarBuffVO warBuffVO = Service.Get<IDataController>().Get<WarBuffVO>(currentlyScoutedBuffBaseId);
				string planetId = warBuffVO.PlanetId;
				string planetDisplayName = LangUtils.GetPlanetDisplayName(planetId);
				AlertScreen.ShowModal(false, this.lang.Get("WAR_ATTACK_BUFF_BASE_NOT_UNLOCKED_TITLE", new object[]
				{
					planetDisplayName
				}), this.lang.Get("WAR_ATTACK_BUFF_BASE_NOT_UNLOCKED_MESSAGE", new object[]
				{
					planetDisplayName
				}), null, null);
				return;
			}
			string failureStringIdByScoutState = SquadUtils.GetFailureStringIdByScoutState(state, flag);
			string instructions = this.lang.Get(failureStringIdByScoutState, new object[0]);
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(instructions);
		}

		public void OnSquadWarAttackResultCallback(object result, object cookie)
		{
			if (this.warAttackButton != null)
			{
				this.warAttackButton.Enabled = true;
			}
			if (this.warDoneButton != null)
			{
				this.warDoneButton.Enabled = true;
			}
		}

		public void SetSquadScreenVisibility(bool visible)
		{
			if (this.persistentSquadScreen != null)
			{
				this.persistentSquadScreen.Visible = visible;
			}
		}

		public void SetSquadScreenAlwaysOnTop(bool onTop)
		{
			if (this.persistentSquadScreen != null)
			{
				this.persistentSquadScreen.IsAlwaysOnTop = onTop;
			}
		}

		public void SlideSquadScreenOpen()
		{
			if (this.persistentSquadScreen != null)
			{
				this.persistentSquadScreen.AnimateOpen();
			}
		}

		private void CreateSquadScreen()
		{
			if (this.persistentSquadScreen == null)
			{
				this.persistentSquadScreen = new SquadSlidingScreen();
				Service.Get<ScreenController>().AddScreen(this.persistentSquadScreen, false, true);
			}
		}

		public void PrepForSquadScreenCreate()
		{
			this.persistentSquadScreen = null;
		}

		public void DestroySquadScreen()
		{
			if (this.persistentSquadScreen != null)
			{
				bool flag = this.persistentSquadScreen.IsOpen();
				if (flag)
				{
					this.persistentSquadScreen.AnimateClosed(true, null);
					return;
				}
				this.persistentSquadScreen.Close(null);
				this.persistentSquadScreen = null;
			}
		}

		public bool IsSquadScreenOpenAndCloseable()
		{
			bool result = false;
			if (this.persistentSquadScreen != null)
			{
				result = this.persistentSquadScreen.IsOpen();
			}
			return result;
		}

		public bool IsSquadScreenOpenOrOpeningAndCloseable()
		{
			bool result = false;
			if (this.persistentSquadScreen != null)
			{
				result = (this.persistentSquadScreen.IsOpen() || this.persistentSquadScreen.IsOpening());
			}
			return result;
		}

		public void SlideSquadScreenClosed()
		{
			if (this.persistentSquadScreen != null)
			{
				this.persistentSquadScreen.AnimateClosed(false, null);
			}
		}

		public void SlideSquadScreenClosedInstantly()
		{
			if (this.persistentSquadScreen != null)
			{
				this.persistentSquadScreen.InstantClose(false, null);
			}
		}

		private bool ShouldDestroyOrHideHomeStateUI(IState state)
		{
			return state is BattleStartState || state is BattlePlaybackState || state is EditBaseState || state is WarBaseEditorState;
		}

		private void UpdateHolonetButtonVisibility(bool visible)
		{
			if (this.holonetButton != null && this.CurrentHudConfig != null && this.CurrentHudConfig.Has("Newspaper"))
			{
				this.holonetButton.Visible = visible;
				if (visible)
				{
					this.genericConfigElements.Add(this.holonetButton);
					return;
				}
				this.genericConfigElements.Remove(this.holonetButton);
			}
		}

		private void UpdateTargetedBundleViewTimer()
		{
			if (this.targetedBundleButton == null)
			{
				return;
			}
			TargetedBundleController targetedBundleController = Service.Get<TargetedBundleController>();
			if (targetedBundleController.CurrentTargetedOffer != null)
			{
				uint serverTime = Service.Get<ServerAPI>().ServerTime;
				int num = (int)(targetedBundleController.OfferExpiresAt - serverTime);
				if (num <= 0)
				{
					if (!targetedBundleController.FetchingNewOffer)
					{
						targetedBundleController.GetNewOffer();
						return;
					}
				}
				else
				{
					UXLabel element = base.GetElement<UXLabel>("LabelSpecialPromoTimer");
					element.Text = this.lang.Get("expires_in", new object[]
					{
						LangUtils.FormatTime((long)num)
					});
				}
			}
		}

		private void UpdateTargetedBundleButtonVisibility()
		{
			if (this.targetedBundleButton != null)
			{
				TargetedBundleController targetedBundleController = Service.Get<TargetedBundleController>();
				this.targetedBundleButton.Visible = targetedBundleController.CanDisplaySPDButton();
				if (UXUtils.IsVisibleInHierarchy(this.targetedBundleButton))
				{
					UXLabel element = base.GetElement<UXLabel>("LabelSpecialPromo");
					TargetedBundleVO currentTargetedOffer = targetedBundleController.CurrentTargetedOffer;
					element.Text = this.lang.Get(currentTargetedOffer.IconString, new object[0]);
					this.UpdateTargetedBundleViewTimer();
					IDataController dataController = Service.Get<IDataController>();
					string iconImage = currentTargetedOffer.IconImage;
					TextureVO optional = dataController.GetOptional<TextureVO>(iconImage);
					if (optional != null)
					{
						UXTexture element2 = base.GetElement<UXTexture>("TextureSpecialPromo");
						element2.LoadTexture(optional.AssetName);
					}
					else
					{
						Service.Get<StaRTSLogger>().Error("HUD::UpdateTargetedBundleButtonVisibility Could Not find texture vo for " + iconImage + " in offer " + currentTargetedOffer.Uid);
					}
					this.targetedBundleButtonGlowAnim.SetTrigger("ClearEffect");
					if (GameConstants.PROMO_BUTTON_RESHOW_GLOW || !this.targetedBundleGlowShown)
					{
						this.ShowPromoButtonGlowEffect();
					}
				}
			}
		}

		private void ShowPromoButtonGlowEffect()
		{
			if (GameConstants.PROMO_BUTTON_GLOW_DURATION != 0)
			{
				this.targetedBundleButtonGlowAnim.SetTrigger("ShowEffect");
				this.targetedBundleGlowShown = true;
			}
			if (GameConstants.PROMO_BUTTON_GLOW_DURATION > 0)
			{
				ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
				viewTimerManager.KillViewTimer(this.targetedBundleGlowTimerID);
				this.targetedBundleGlowTimerID = 0u;
				this.targetedBundleGlowTimerID = viewTimerManager.CreateViewTimer((float)GameConstants.PROMO_BUTTON_GLOW_DURATION, false, new TimerDelegate(this.StopPromoButtonGlowEffect), null);
				this.targetedBundleGlowShown = true;
			}
		}

		private void StopPromoButtonGlowEffect(uint id, object cookie)
		{
			if (this.Visible)
			{
				this.targetedBundleButtonGlowAnim.SetTrigger("StopEffect");
			}
			else
			{
				this.targetedBundleButtonGlowAnim.StopPlayback();
			}
			Service.Get<ViewTimerManager>().KillViewTimer(this.targetedBundleGlowTimerID);
			this.targetedBundleGlowTimerID = 0u;
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (!base.IsLoaded())
			{
				return EatResponse.NotEaten;
			}
			if (id <= EventId.InventoryResourceUpdated)
			{
				if (id <= EventId.SquadTroopsDeployedByPlayer)
				{
					if (id <= EventId.TroopDeployed)
					{
						if (id == EventId.BuildingPurchaseSuccess)
						{
							this.RefreshAllResourceViews(true);
							goto IL_72C;
						}
						if (id != EventId.TroopDeployed)
						{
							goto IL_72C;
						}
						this.OnTroopPlaced(cookie as SmartEntity);
						goto IL_72C;
					}
					else
					{
						if (id == EventId.ChampionRepaired)
						{
							goto IL_518;
						}
						if (id == EventId.SquadTroopsReceived)
						{
							goto IL_5D5;
						}
						if (id != EventId.SquadTroopsDeployedByPlayer)
						{
							goto IL_72C;
						}
						this.OnSquadTroopsDeployed();
						goto IL_72C;
					}
				}
				else if (id <= EventId.WorldLoadComplete)
				{
					switch (id)
					{
					case EventId.TroopLevelUpgraded:
					case EventId.StarshipLevelUpgraded:
						goto IL_518;
					case EventId.BuildingLevelUpgraded:
					case EventId.BuildingReplaced:
						break;
					case EventId.BuildingSwapped:
					case EventId.SpecialAttackSpawned:
						goto IL_72C;
					case EventId.BuildingConstructed:
					{
						Entity selectedBuilding = Service.Get<BuildingController>().SelectedBuilding;
						ContractEventData contractEventData = (ContractEventData)cookie;
						if (contractEventData.BuildingVO.Currency == CurrencyType.Contraband)
						{
							this.RefreshView();
						}
						if (selectedBuilding != null && selectedBuilding == contractEventData.Entity)
						{
							this.ShowContextButtons(selectedBuilding);
						}
						this.UpdateStoreJewel();
						goto IL_72C;
					}
					case EventId.SpecialAttackDeployed:
						this.OnSpecialAttackDeployed((SpecialAttack)cookie);
						goto IL_72C;
					default:
						if (id != EventId.WorldLoadComplete)
						{
							goto IL_72C;
						}
						Service.Get<EventManager>().UnregisterObserver(this, EventId.HolonetContentPrepared);
						this.storeButton.Enabled = true;
						if (this.CurrentHudConfig != null)
						{
							this.ConfigureControls(this.CurrentHudConfig);
						}
						this.UpdateStoreJewel();
						this.UpdateLogJewel();
						goto IL_72C;
					}
				}
				else if (id != EventId.GameStateChanged)
				{
					if (id != EventId.ContractStarted)
					{
						if (id != EventId.InventoryResourceUpdated)
						{
							goto IL_72C;
						}
						this.RefreshResourceView((string)cookie, !Service.Get<BattleController>().BattleInProgress);
						this.RefreshCurrentPlayerLevel();
						goto IL_72C;
					}
					else
					{
						ContractEventData contractEventData2 = (ContractEventData)cookie;
						if (contractEventData2.Contract.DeliveryType == DeliveryType.Building)
						{
							this.UpdateStoreJewel();
							goto IL_72C;
						}
						goto IL_72C;
					}
				}
				else
				{
					IState currentState = Service.Get<GameStateMachine>().CurrentState;
					if (currentState is HomeState)
					{
						HomeState homeState = currentState as HomeState;
						if (homeState.ForceReloadMap)
						{
							this.storeButton.Enabled = false;
						}
						this.UpdateHolonetJewel();
						this.UpdateStoreJewel();
						this.UpdateLogJewel();
						Service.Get<UXController>().MiscElementsManager.SetEventTickerViewVisible(this.Visible);
						goto IL_72C;
					}
					if (currentState is WarBaseEditorState)
					{
						Service.Get<UXController>().MiscElementsManager.SetEventTickerViewVisible(this.Visible);
						goto IL_72C;
					}
					if (this.ShouldDestroyOrHideHomeStateUI(currentState))
					{
						Service.Get<UXController>().MiscElementsManager.SetEventTickerViewVisible(false);
						this.DestroySquadScreen();
						goto IL_72C;
					}
					goto IL_72C;
				}
			}
			else
			{
				if (id > EventId.PvpRatingChanged)
				{
					if (id <= EventId.WarAttackBuffBaseCompleted)
					{
						switch (id)
						{
						case EventId.HolonetContentPrepareStarted:
							Service.Get<EventManager>().RegisterObserver(this, EventId.AllHolonetContentPrepared);
							this.UpdateHolonetButtonVisibility(false);
							goto IL_72C;
						case EventId.AllHolonetContentPrepared:
							this.UpdateHolonetButtonVisibility(true);
							Service.Get<EventManager>().UnregisterObserver(this, EventId.AllHolonetContentPrepared);
							goto IL_72C;
						case EventId.HolonetContentPrepared:
							this.UpdateHolonetJewel();
							goto IL_72C;
						case EventId.TargetedBundleContentPrepared:
							break;
						default:
							switch (id)
							{
							case EventId.SquadUpdated:
								goto IL_5D5;
							case EventId.SquadLeft:
								this.joinSquadButton.Visible = true;
								goto IL_72C;
							case EventId.SquadChatFilterUpdated:
							case EventId.SquadJoinedByCurrentPlayer:
							case EventId.SquadJoinApplicationAcceptedByCurrentPlayer:
							case EventId.SquadJoinInviteAcceptedByCurrentPlayer:
							case EventId.SquadWarTroopsRequestStartedByCurrentPlayer:
							case EventId.SquadWarTroopsRequestedByCurrentPlayer:
							case EventId.SquadTroopsDonatedByCurrentPlayer:
							case EventId.SquadReplaySharedByCurrentPlayer:
							case EventId.CurrentPlayerMemberDataUpdated:
							case EventId.SquadJoinInviteRemoved:
							case EventId.SquadServerMessage:
								goto IL_72C;
							case EventId.SquadTroopsRequestedByCurrentPlayer:
							{
								SmartEntity smartEntity = (SmartEntity)Service.Get<BuildingController>().SelectedBuilding;
								if (smartEntity != null && smartEntity.SquadBuildingComp != null)
								{
									this.ShowContextButtons(smartEntity);
									goto IL_72C;
								}
								goto IL_72C;
							}
							case EventId.SquadJoinApplicationAccepted:
							{
								if (ScreenUtils.IsAnySquadScreenOpen())
								{
									Service.Get<ScreenController>().CloseAll();
								}
								string text = (string)cookie;
								if (!string.IsNullOrEmpty(text))
								{
									string instructions = Service.Get<Lang>().Get("SQUAD_ACCEPTED_MESSAGE", new object[]
									{
										text
									});
									Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(instructions);
									goto IL_72C;
								}
								goto IL_72C;
							}
							case EventId.SquadTroopsReceivedFromDonor:
							{
								string text2 = (string)cookie;
								if (!string.IsNullOrEmpty(text2))
								{
									string instructions2 = Service.Get<Lang>().Get("TROOPS_RECEIVED_FROM", new object[]
									{
										text2
									});
									Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(instructions2);
									goto IL_72C;
								}
								goto IL_72C;
							}
							case EventId.SquadJoinInviteReceived:
							case EventId.SquadJoinInvitesReceived:
								this.UpdateSquadJewelCount();
								goto IL_72C;
							case EventId.WarPhaseChanged:
								this.UpdateWarButton();
								goto IL_72C;
							case EventId.WarAttackPlayerStarted:
							case EventId.WarAttackPlayerCompleted:
							case EventId.WarAttackBuffBaseStarted:
							case EventId.WarAttackBuffBaseCompleted:
								this.UpdateWarAttackState();
								goto IL_72C;
							default:
								goto IL_72C;
							}
							break;
						}
					}
					else
					{
						if (id == EventId.WarAttackCommandFailed)
						{
							if (this.warAttackButton != null)
							{
								this.warAttackButton.Enabled = true;
								this.warDoneButton.Enabled = true;
								this.DisableWarAttacksUI();
							}
							Service.Get<EventManager>().UnregisterObserver(this, EventId.WarAttackCommandFailed);
							goto IL_72C;
						}
						if (id != EventId.TargetedBundleRewardRedeemed)
						{
							if (id != EventId.EquipmentUpgraded)
							{
								goto IL_72C;
							}
							goto IL_518;
						}
					}
					this.UpdateTargetedBundleButtonVisibility();
					goto IL_72C;
				}
				if (id <= EventId.HUDVisibilityChanged)
				{
					switch (id)
					{
					case EventId.InventoryUnlockUpdated:
						break;
					case EventId.InventoryPrizeUpdated:
					case EventId.LootCollected:
						goto IL_72C;
					case EventId.NumInventoryItemsNotViewedUpdated:
					{
						Entity selectedBuilding2 = Service.Get<BuildingController>().SelectedBuilding;
						if (selectedBuilding2 != null)
						{
							this.ShowContextButtons(selectedBuilding2);
							goto IL_72C;
						}
						goto IL_72C;
					}
					case EventId.LootEarnedUpdated:
						this.RefreshLoot();
						goto IL_72C;
					case EventId.ScreenClosing:
						if (cookie is StoreScreen)
						{
							if (!this.player.CampaignProgress.FueInProgress)
							{
								this.UpdateStoreJewel();
								goto IL_72C;
							}
							goto IL_72C;
						}
						else
						{
							if (cookie is HolonetScreen)
							{
								this.UpdateHolonetJewel();
								goto IL_72C;
							}
							goto IL_72C;
						}
						break;
					default:
					{
						if (id != EventId.HUDVisibilityChanged)
						{
							goto IL_72C;
						}
						bool flag = false;
						if (Service.IsSet<TargetedBundleController>())
						{
							flag = (Service.Get<TargetedBundleController>().CurrentTargetedOffer != null);
						}
						if (flag && this.Visible && UXUtils.IsVisibleInHierarchy(this.targetedBundleButton))
						{
							this.targetedBundleButtonGlowAnim.SetTrigger("ClearEffect");
							if (GameConstants.PROMO_BUTTON_RESHOW_GLOW || !this.targetedBundleGlowShown)
							{
								this.ShowPromoButtonGlowEffect();
							}
							Service.Get<TargetedBundleController>().LogTargetedBundleBI("icon_display");
							goto IL_72C;
						}
						if (UXUtils.IsVisibleInHierarchy(this.targetedBundleButton) && this.targetedBundleGlowShown)
						{
							this.StopPromoButtonGlowEffect(0u, null);
							goto IL_72C;
						}
						goto IL_72C;
					}
					}
				}
				else
				{
					if (id == EventId.HeroDeployed)
					{
						this.OnHeroDeployed(cookie as SmartEntity);
						goto IL_72C;
					}
					if (id == EventId.ChampionDeployed)
					{
						this.OnChampionDeployed(cookie as SmartEntity);
						goto IL_72C;
					}
					switch (id)
					{
					case EventId.MissionActionButtonClicked:
					{
						if (cookie == null)
						{
							goto IL_72C;
						}
						CampaignMissionVO campaignMissionVO = (CampaignMissionVO)cookie;
						if (campaignMissionVO.MissionType == MissionType.Defend || campaignMissionVO.MissionType == MissionType.RaidDefend)
						{
							this.DestroySquadScreen();
							goto IL_72C;
						}
						goto IL_72C;
					}
					case EventId.PlayerNameChanged:
						this.RefreshPlayerSocialInformation();
						goto IL_72C;
					case EventId.PlayerFactionChanged:
						goto IL_72C;
					case EventId.PvpRatingChanged:
						this.RefreshPlayerMedals();
						goto IL_72C;
					default:
						goto IL_72C;
					}
				}
			}
			this.UpdateStoreJewel();
			this.UpdateLogJewel();
			goto IL_72C;
			IL_518:
			Entity selectedBuilding3 = Service.Get<BuildingController>().SelectedBuilding;
			if (selectedBuilding3 != null && selectedBuilding3 == ((ContractEventData)cookie).Entity)
			{
				this.ShowContextButtons(selectedBuilding3);
				goto IL_72C;
			}
			goto IL_72C;
			IL_5D5:
			IState currentState2 = Service.Get<GameStateMachine>().CurrentState;
			if (currentState2 is HomeState || currentState2 is EditBaseState)
			{
				this.RefreshView();
				SmartEntity smartEntity2 = (SmartEntity)Service.Get<BuildingController>().SelectedBuilding;
				if (smartEntity2 != null && smartEntity2.SquadBuildingComp != null)
				{
					this.ShowContextButtons(smartEntity2);
				}
				BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
				SmartEntity smartEntity3 = (SmartEntity)buildingLookupController.GetCurrentSquadBuilding();
				if (smartEntity3 != null)
				{
					Service.Get<BuildingTooltipController>().EnsureBuildingTooltip(smartEntity3);
				}
			}
			IL_72C:
			return base.OnEvent(id, cookie);
		}

		private void DisableWarAttacksUI()
		{
			this.warAttackButton.VisuallyDisableButton();
			this.warAttackLabel.TextColor = UXUtils.COLOR_LABEL_DISABLED;
			this.FullyDisableDeployableControls(false);
		}

		private void FullyDisableDeployableControls(bool hideTroopCounts)
		{
			if (this.deployableTroops == null)
			{
				return;
			}
			foreach (KeyValuePair<string, DeployableTroopControl> current in this.deployableTroops)
			{
				DeployableTroopControl value = current.get_Value();
				value.Disable(hideTroopCounts);
				value.TroopCheckbox.Enabled = false;
			}
		}

		private void FullyEnableDeployablControls()
		{
			if (this.deployableTroops == null)
			{
				return;
			}
			foreach (KeyValuePair<string, DeployableTroopControl> current in this.deployableTroops)
			{
				DeployableTroopControl value = current.get_Value();
				value.TroopCheckbox.Enabled = true;
				value.Enable();
			}
		}

		private void OnTroopPlaced(SmartEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			TroopComponent troopComp = entity.TroopComp;
			if (troopComp == null)
			{
				return;
			}
			this.UpdateTroopCount(troopComp.TroopType.Uid);
		}

		private void OnSpecialAttackDeployed(SpecialAttack specialAttack)
		{
			this.UpdateSpecialAttackCount(specialAttack.VO.Uid);
		}

		private void OnHeroDeployed(SmartEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			TroopComponent troopComp = entity.TroopComp;
			if (troopComp == null)
			{
				return;
			}
			string uid = troopComp.TroopType.Uid;
			TroopAbilityVO abilityVO = troopComp.AbilityVO;
			this.UpdateHeroCount(uid);
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			bool multipleHeroDeploysAllowed = currentBattle.MultipleHeroDeploysAllowed;
			if (this.deployableTroops != null)
			{
				foreach (KeyValuePair<string, DeployableTroopControl> current in this.deployableTroops)
				{
					DeployableTroopControl value = current.get_Value();
					if (value.IsHero)
					{
						if (current.get_Key() == uid && abilityVO != null && !abilityVO.Auto)
						{
							value.PrepareHeroAbility();
						}
						else if (value.Enabled && !multipleHeroDeploysAllowed)
						{
							value.Disable();
						}
					}
				}
			}
			this.battleControlsSelectedCheckbox = null;
		}

		private void OnChampionDeployed(SmartEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			TroopComponent troopComp = entity.TroopComp;
			if (troopComp == null)
			{
				return;
			}
			this.UpdateChampionCount(troopComp.TroopType.Uid);
		}

		private void OnSquadTroopsDeployed()
		{
			this.UpdateDeployableCount("squadTroops", 0);
		}

		private void UpdateTroopCount(string uid)
		{
			this.UpdateDeployableCount(uid, Service.Get<BattleController>().GetPlayerDeployableTroopCount(uid));
		}

		private void UpdateSpecialAttackCount(string uid)
		{
			this.UpdateDeployableCount(uid, Service.Get<BattleController>().GetPlayerDeployableSpecialAttackCount(uid));
		}

		private void UpdateHeroCount(string uid)
		{
			this.UpdateDeployableCount(uid, Service.Get<BattleController>().GetPlayerDeployableHeroCount(uid));
		}

		private void UpdateChampionCount(string uid)
		{
			this.UpdateDeployableCount(uid, Service.Get<BattleController>().GetPlayerDeployableChampionCount(uid));
		}

		private void UpdateDeployableCount(string uid, int count)
		{
			if (this.deployableTroops != null && this.deployableTroops.ContainsKey(uid))
			{
				DeployableTroopControl deployableTroopControl = this.deployableTroops[uid];
				UXLabel troopCountLabel = deployableTroopControl.TroopCountLabel;
				troopCountLabel.Text = count.ToString();
				troopCountLabel.TextColor = UXUtils.GetCostColor(troopCountLabel, count != 0, false);
				if (count == 0)
				{
					deployableTroopControl.Disable();
				}
			}
		}

		public void DisableHeroDeploys()
		{
			foreach (DeployableTroopControl current in this.deployableTroops.Values)
			{
				if (current.IsHero && current.AbilityState == HeroAbilityState.Dormant)
				{
					current.DisableDueToBuildingDestruction = true;
					current.Disable();
				}
			}
		}

		public void DisableSquadDeploy()
		{
			if (this.deployableTroops.ContainsKey("squadTroops"))
			{
				this.deployableTroops["squadTroops"].DisableDueToBuildingDestruction = true;
				this.deployableTroops["squadTroops"].Disable();
			}
		}

		public void DisableSpecialAttacks()
		{
			foreach (DeployableTroopControl current in this.deployableTroops.Values)
			{
				if (current.IsStarship)
				{
					current.DisableDueToBuildingDestruction = true;
					current.Disable();
				}
			}
		}

		public void EnableNextBattleButton()
		{
			if (this.nextBattleButton.Visible)
			{
				this.nextBattleButton.Enabled = true;
			}
		}

		private void DeselectAllDeployableControlers()
		{
			foreach (DeployableTroopControl current in this.deployableTroops.Values)
			{
				current.TroopCheckbox.Selected = false;
			}
		}

		private void OnNextBattleButtonClicked(UXButton button)
		{
			this.DeselectAllDeployableControlers();
			PvpManager pvpManager = Service.Get<PvpManager>();
			if (Service.IsSet<PvpManager>())
			{
				if (GameUtils.CanAffordCredits(pvpManager.GetPvpMatchCost()))
				{
					button.Enabled = false;
					Service.Get<EventManager>().SendEvent(EventId.PvpBattleSkipped, null);
					Service.Get<CombatTriggerManager>().UnregisterAllTriggers();
					pvpManager.PurchaseNextBattle();
					return;
				}
				pvpManager.HandleNotEnoughCreditsForNextBattle();
			}
		}

		private void OnEndBattleButtonClicked(UXButton button)
		{
			Service.Get<EventManager>().SendEvent(EventId.BattleCancelRequested, null);
		}

		private void OnDroidButtonClicked(UXButton button)
		{
			this.ShowDroidPurchaseScreen();
			Service.Get<EventManager>().SendEvent(EventId.HUDDroidButtonClicked, null);
		}

		private void OnCrystalButtonClicked(UXButton button)
		{
			StoreScreen storeScreen = new StoreScreen();
			storeScreen.SetTab(StoreTab.Treasure);
			Service.Get<ScreenController>().AddScreen(storeScreen);
			Service.Get<EventManager>().SendEvent(EventId.HUDCrystalButtonClicked, null);
		}

		private void OnShieldButtonClicked(UXButton button)
		{
			StoreScreen storeScreen = new StoreScreen();
			storeScreen.SetTab(StoreTab.Protection);
			Service.Get<ScreenController>().AddScreen(storeScreen);
			Service.Get<EventManager>().SendEvent(EventId.HUDShieldButtonClicked, null);
		}

		private void OnSpecialPromotionButtonClicked(UXButton button)
		{
			TargetedBundleController targetedBundleController = Service.Get<TargetedBundleController>();
			TargetedBundleVO currentTargetedOffer = targetedBundleController.CurrentTargetedOffer;
			if (currentTargetedOffer == null)
			{
				return;
			}
			CrateVO crateVOFromTargetedOffer = targetedBundleController.GetCrateVOFromTargetedOffer(currentTargetedOffer);
			ScreenBase screen;
			if (crateVOFromTargetedOffer != null)
			{
				screen = CrateInfoModalScreen.CreateForTargetedOffer(currentTargetedOffer, crateVOFromTargetedOffer);
			}
			else
			{
				screen = new TargetedBundleScreen();
			}
			Service.Get<ScreenController>().AddScreen(screen);
			Service.Get<EventManager>().SendEvent(EventId.HUDSpecialPromotionButtonClicked, null);
			Service.Get<TargetedBundleController>().LogTargetedBundleBI("icon_tap");
		}

		public void OnViewFrameTime(float dt)
		{
			bool flag = false;
			if (this.creditsView.NeedsUpdate)
			{
				this.creditsView.Update(dt);
				flag = true;
			}
			if (this.materialsView.NeedsUpdate)
			{
				this.materialsView.Update(dt);
				flag = true;
			}
			if (this.contrabandView.NeedsUpdate)
			{
				this.contrabandView.Update(dt);
				flag = true;
			}
			if (this.crystalsView.NeedsUpdate)
			{
				this.crystalsView.Update(dt);
				flag = true;
			}
			if (!flag)
			{
				this.registeredFrameTimeObserver = false;
				Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			}
		}

		public void OnViewClockTime(float dt)
		{
			this.UpdateProtectionTimeLabel();
			this.UpdateTargetedBundleViewTimer();
			if (!Service.IsSet<BuildingController>())
			{
				return;
			}
			SmartEntity smartEntity = (SmartEntity)Service.Get<BuildingController>().SelectedBuilding;
			if (smartEntity != null && smartEntity.BuildingComp != null && smartEntity.BuildingComp.BuildingTO != null && !string.IsNullOrEmpty(smartEntity.BuildingComp.BuildingTO.Key))
			{
				BuildingComponent buildingComp = smartEntity.BuildingComp;
				int i = 0;
				int count = this.contextButtons.Count;
				while (i < count)
				{
					if (this.contextButtons[i] != null && this.contextButtons[i].Tag != null)
					{
						ContextButtonTag contextButtonTag = this.contextButtons[i].Tag as ContextButtonTag;
						if (contextButtonTag != null)
						{
							if (contextButtonTag.ContextId == "Finish_Now")
							{
								int num = -1;
								if (contextButtonTag.ContextId == "Finish_Now")
								{
									Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComp.BuildingTO.Key);
									if (contract != null)
									{
										num = ContractUtils.GetCrystalCostToFinishContract(contract);
									}
								}
								else if (contextButtonTag.ContextId == "RequestTroopsPaid")
								{
									uint serverTime = Service.Get<ServerAPI>().ServerTime;
									uint troopRequestDate = Service.Get<SquadController>().StateManager.TroopRequestDate;
									num = SquadUtils.GetTroopRequestCrystalCost(serverTime, troopRequestDate);
								}
								if (num >= 0 && contextButtonTag.HardCostLabel != null)
								{
									contextButtonTag.HardCostLabel.Text = this.lang.ThousandsSeparated(num);
									contextButtonTag.HardCostLabel.TextColor = UXUtils.GetCostColor(contextButtonTag.HardCostLabel, GameUtils.CanAffordCrystals(num), false);
								}
							}
							this.UpdateContextTimerLabel(contextButtonTag);
						}
					}
					i++;
				}
			}
		}

		private void UpdateContextTimerLabel(ContextButtonTag tag)
		{
			if (tag.TimerLabel != null && tag.TimerSecondsDelegate != null)
			{
				tag.TimerLabel.Text = LangUtils.FormatTime((long)tag.TimerSecondsDelegate());
				UXUtils.ClampUILabelWidth(tag.TimerLabel, 108, 0);
			}
		}

		private void UpdateProtectionTimeLabel()
		{
			if (this.protectionLabel == null)
			{
				return;
			}
			uint protectedUntil = this.player.ProtectedUntil;
			uint serverTime = Service.Get<ServerAPI>().ServerTime;
			if (protectedUntil <= serverTime)
			{
				this.protectionLabel.Text = Service.Get<Lang>().Get("PROTECTION_NONE", new object[0]);
				return;
			}
			this.protectionLabel.Text = GameUtils.GetTimeLabelFromSeconds(Convert.ToInt32(protectedUntil - serverTime, CultureInfo.InvariantCulture));
		}

		private void OnHomeButtonClicked(UXButton button)
		{
			button.Enabled = false;
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is BattlePlaybackState)
			{
				Service.Get<BattleController>().EndBattleRightAway();
				return;
			}
			if (currentState is BattleStartState)
			{
				Service.Get<CombatTriggerManager>().UnregisterAllTriggers();
				BattleType type = Service.Get<BattleController>().GetCurrentBattle().Type;
				Service.Get<EventManager>().SendEvent(EventId.BattleLeftBeforeStarting, null);
				if (type == BattleType.Pvp)
				{
					Service.Get<PvpManager>().ReleasePvpTarget();
				}
				else if (type == BattleType.PveBuffBase)
				{
					Service.Get<SquadController>().WarManager.ReleaseCurrentlyScoutedBuffBase();
				}
				Service.Get<ShieldController>().StopAllEffects();
				if (type == BattleType.PvpAttackSquadWar || type == BattleType.PveBuffBase)
				{
					Service.Get<SquadController>().WarManager.StartTranstionFromWarBaseToWarBoard();
					return;
				}
			}
			HomeState.GoToHomeState(null, false);
		}

		private void OnEditButtonClicked(UXButton button)
		{
			button.Enabled = false;
			Service.Get<GameStateMachine>().SetState(new EditBaseState(false));
		}

		private void OnStoreButtonClicked(UXButton button)
		{
			Service.Get<ScreenController>().AddScreen(new StoreScreen());
			Service.Get<EventManager>().SendEvent(EventId.HUDStoreButtonClicked, null);
		}

		private BuildingType GetBuildingType(BuildingTypeVO building, bool simpleInfoScreen)
		{
			BuildingType result = building.Type;
			if (simpleInfoScreen)
			{
				switch (result)
				{
				case BuildingType.Barracks:
				case BuildingType.Factory:
				case BuildingType.FleetCommand:
				case BuildingType.Squad:
				case BuildingType.Starport:
					break;
				case BuildingType.HeroMobilizer:
				case BuildingType.ChampionPlatform:
				case BuildingType.Housing:
					return result;
				default:
					switch (result)
					{
					case BuildingType.Cantina:
					case BuildingType.NavigationCenter:
					case BuildingType.Armory:
						break;
					case BuildingType.ScoutTower:
						return result;
					default:
						return result;
					}
					break;
				}
				result = BuildingType.Invalid;
			}
			return result;
		}

		private void OnDisabledContextButtonClicked(UXButton button)
		{
			if (button.Tag as string == "Trap_RearmAll")
			{
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(this.lang.Get("traps_error_all_rearmed", new object[0]));
			}
		}

		private void OnContextButtonClicked(UXButton button)
		{
			Entity selectedBuilding = Service.Get<BuildingController>().SelectedBuilding;
			if (selectedBuilding == null)
			{
				UXElement buttonHighlight = Service.Get<UXController>().MiscElementsManager.GetButtonHighlight();
				string text = "null";
				if (buttonHighlight != null)
				{
					text = buttonHighlight.Visible.ToString();
				}
				Service.Get<StaRTSLogger>().ErrorFormat("HUD.OnContextButtonClicked: SelectedBuilding is null. button.Tag is {0}. MiscElementManager.buttonHighlight visiblity is {1}.", new object[]
				{
					button.Tag,
					text
				});
				return;
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			BuildingTypeVO buildingType = buildingComponent.BuildingType;
			bool simpleInfoScreen = GameUtils.IsVisitingBase();
			BuildingType buildingType2 = this.GetBuildingType(buildingType, simpleInfoScreen);
			ScreenBase screenBase = null;
			string text2 = button.Tag as string;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 2210818019u)
			{
				if (num <= 805458841u)
				{
					if (num <= 261807733u)
					{
						if (num <= 127659816u)
						{
							if (num != 77955010u)
							{
								if (num != 127659816u)
								{
									goto IL_B27;
								}
								if (!(text2 == "NextRaid"))
								{
									goto IL_B27;
								}
							}
							else
							{
								if (!(text2 == "Clear"))
								{
									goto IL_B27;
								}
								Service.Get<BuildingController>().StartClearingSelectedBuilding();
								goto IL_B27;
							}
						}
						else if (num != 222444311u)
						{
							if (num != 261807733u)
							{
								goto IL_B27;
							}
							if (!(text2 == "Credits"))
							{
								goto IL_B27;
							}
							goto IL_949;
						}
						else
						{
							if (!(text2 == "Upgrade"))
							{
								goto IL_B27;
							}
							switch (buildingType.Type)
							{
							case BuildingType.HQ:
								screenBase = new HQUpgradeScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Barracks:
							case BuildingType.Factory:
							case BuildingType.FleetCommand:
							case BuildingType.HeroMobilizer:
							case BuildingType.Cantina:
								screenBase = new TrainingUpgradeScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.ChampionPlatform:
								screenBase = new ChampionInfoScreen(selectedBuilding, Service.Get<ChampionController>().FindChampionTypeIfPlatform(buildingType), true);
								goto IL_B27;
							case BuildingType.Squad:
								screenBase = new SquadUpgradeScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Starport:
								screenBase = new StarportUpgradeScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Wall:
								screenBase = new WallUpgradeScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Turret:
								screenBase = new TurretUpgradeScreen(selectedBuilding, false);
								goto IL_B27;
							case BuildingType.Resource:
								screenBase = new GeneratorUpgradeScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Storage:
								screenBase = new StorageUpgradeScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.ShieldGenerator:
								screenBase = new ShieldGeneratorUpgradeScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Trap:
								screenBase = new TrapInfoScreen(selectedBuilding, true);
								goto IL_B27;
							case BuildingType.NavigationCenter:
								screenBase = new NavigationCenterUpgradeScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Armory:
								screenBase = new ArmoryUpgradeScreen(selectedBuilding, true);
								goto IL_B27;
							}
							screenBase = new BuildingInfoScreen(selectedBuilding, true);
							goto IL_B27;
						}
					}
					else if (num <= 504525065u)
					{
						if (num != 449344883u)
						{
							if (num != 504525065u)
							{
								goto IL_B27;
							}
							if (!(text2 == "Train"))
							{
								goto IL_B27;
							}
							goto IL_78D;
						}
						else
						{
							if (!(text2 == "Contraband"))
							{
								goto IL_B27;
							}
							goto IL_949;
						}
					}
					else if (num != 686057250u)
					{
						if (num != 805458841u)
						{
							goto IL_B27;
						}
						if (!(text2 == "Join"))
						{
							goto IL_B27;
						}
						this.OpenJoinSquadPanel();
						goto IL_B27;
					}
					else
					{
						if (!(text2 == "Stash"))
						{
							goto IL_B27;
						}
						BuildingController buildingController = Service.Get<BuildingController>();
						Service.Get<BuildingController>().EnsureLoweredLiftedBuilding();
						List<string> list = new List<string>();
						if (buildingController.NumSelectedBuildings > 1)
						{
							List<Entity> additionalSelectedBuildings = buildingController.GetAdditionalSelectedBuildings();
							int i = 0;
							int count = additionalSelectedBuildings.Count;
							while (i < count)
							{
								Entity entity = additionalSelectedBuildings[i];
								string uid = entity.Get<BuildingComponent>().BuildingType.Uid;
								if (!list.Contains(uid))
								{
									list.Add(uid);
								}
								Service.Get<BaseLayoutToolController>().StashBuilding(additionalSelectedBuildings[i]);
								i++;
							}
						}
						Service.Get<BaseLayoutToolController>().StashBuilding(selectedBuilding);
						string uid2 = selectedBuilding.Get<BuildingComponent>().BuildingTO.Uid;
						if (!list.Contains(uid2))
						{
							list.Add(uid2);
						}
						int j = 0;
						int count2 = list.Count;
						while (j < count2)
						{
							this.BaseLayoutToolView.RefreshStashedBuildingCount(list[j]);
							j++;
						}
						Service.Get<BuildingController>().EnsureDeselectSelectedBuilding();
						goto IL_B27;
					}
				}
				else if (num <= 1444466900u)
				{
					if (num <= 1254971714u)
					{
						if (num != 900713019u)
						{
							if (num != 1254971714u)
							{
								goto IL_B27;
							}
							if (!(text2 == "RotateWall"))
							{
								goto IL_B27;
							}
							this.RotateCurrentSelection(selectedBuilding);
							goto IL_B27;
						}
						else
						{
							if (!(text2 == "Cancel"))
							{
								goto IL_B27;
							}
							this.CancelContractOnBuilding(selectedBuilding);
							goto IL_B27;
						}
					}
					else if (num != 1389223987u)
					{
						if (num != 1444466900u)
						{
							goto IL_B27;
						}
						if (!(text2 == "Repair"))
						{
							goto IL_B27;
						}
						Service.Get<ChampionController>().StartChampionRepair((SmartEntity)selectedBuilding);
						goto IL_B27;
					}
					else
					{
						if (!(text2 == "Trap_RearmAll"))
						{
							goto IL_B27;
						}
						TrapUtils.RearmAllTraps();
						goto IL_B27;
					}
				}
				else if (num <= 2045844646u)
				{
					if (num != 1759713184u)
					{
						if (num != 2045844646u)
						{
							goto IL_B27;
						}
						if (!(text2 == "Navigate"))
						{
							goto IL_B27;
						}
						Service.Get<GalaxyViewController>().GoToGalaxyView();
						Service.Get<EventManager>().SendEvent(EventId.GalaxyOpenByContextButton, null);
						goto IL_B27;
					}
					else
					{
						if (!(text2 == "Trap_Rearm"))
						{
							goto IL_B27;
						}
						TrapUtils.RearmSingleTrap(selectedBuilding);
						goto IL_B27;
					}
				}
				else if (num != 2078673637u)
				{
					if (num != 2133600820u)
					{
						if (num != 2210818019u)
						{
							goto IL_B27;
						}
						if (!(text2 == "SelectRow"))
						{
							goto IL_B27;
						}
						this.SelectWallGroup(selectedBuilding);
						goto IL_B27;
					}
					else
					{
						if (!(text2 == "Move"))
						{
							goto IL_B27;
						}
						Service.Get<EventManager>().SendEvent(EventId.UserWantedEditBaseState, false);
						goto IL_B27;
					}
				}
				else if (!(text2 == "RaidBriefing"))
				{
					goto IL_B27;
				}
				Service.Get<BILoggingController>().TrackGameAction("UI_raid_briefing", "open", "context", "", 1);
				Service.Get<RaidDefenseController>().ShowRaidInfo();
				goto IL_B27;
			}
			if (num <= 3094495885u)
			{
				if (num <= 2627229077u)
				{
					if (num <= 2464763265u)
					{
						if (num != 2413946405u)
						{
							if (num != 2464763265u)
							{
								goto IL_B27;
							}
							if (!(text2 == "Upgrade_Troops"))
							{
								goto IL_B27;
							}
							screenBase = new TroopUpgradeScreen(selectedBuilding);
							goto IL_B27;
						}
						else
						{
							if (!(text2 == "Info"))
							{
								goto IL_B27;
							}
							switch (buildingType2)
							{
							case BuildingType.Barracks:
							case BuildingType.Factory:
							case BuildingType.Cantina:
								screenBase = new TrainingInfoScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.FleetCommand:
								screenBase = new StarshipInfoScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.ChampionPlatform:
								screenBase = new ChampionInfoScreen(selectedBuilding, Service.Get<ChampionController>().FindChampionTypeIfPlatform(buildingType), false);
								goto IL_B27;
							case BuildingType.Squad:
								this.OpenSquadBuildingInfoScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Starport:
								screenBase = new StarportInfoScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Turret:
								screenBase = new TurretInfoScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Resource:
								screenBase = new GeneratorInfoScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Storage:
								screenBase = new StorageInfoScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.ShieldGenerator:
								screenBase = new ShieldGeneratorInfoScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Trap:
								screenBase = new TrapInfoScreen(selectedBuilding, false);
								goto IL_B27;
							case BuildingType.NavigationCenter:
								screenBase = new NavigationCenterInfoScreen(selectedBuilding);
								goto IL_B27;
							case BuildingType.Armory:
								screenBase = new ArmoryUpgradeScreen(selectedBuilding, false);
								goto IL_B27;
							}
							screenBase = new BuildingInfoScreen(selectedBuilding);
							goto IL_B27;
						}
					}
					else if (num != 2509665603u)
					{
						if (num != 2627229077u)
						{
							goto IL_B27;
						}
						if (!(text2 == "RequestTroopsPaid"))
						{
							goto IL_B27;
						}
					}
					else
					{
						if (!(text2 == "Build"))
						{
							goto IL_B27;
						}
						goto IL_78D;
					}
				}
				else if (num <= 2919601252u)
				{
					if (num != 2877021875u)
					{
						if (num != 2919601252u)
						{
							goto IL_B27;
						}
						if (!(text2 == "Upgrade_Defense"))
						{
							goto IL_B27;
						}
						goto IL_B27;
					}
					else if (!(text2 == "RequestTroops"))
					{
						goto IL_B27;
					}
				}
				else if (num != 2922987018u)
				{
					if (num != 3094495885u)
					{
						goto IL_B27;
					}
					if (!(text2 == "RaidDefend"))
					{
						goto IL_B27;
					}
					Service.Get<BILoggingController>().TrackGameAction("UI_raid", "start", "context", "", 1);
					Service.Get<RaidDefenseController>().StartCurrentRaidDefense();
					goto IL_B27;
				}
				else
				{
					if (!(text2 == "Mobilize"))
					{
						goto IL_B27;
					}
					screenBase = new TroopTrainingScreen(selectedBuilding);
					goto IL_B27;
				}
				Service.Get<SquadController>().ShowTroopRequestScreen(null, false);
				goto IL_B27;
			}
			if (num <= 3553418121u)
			{
				if (num <= 3249042673u)
				{
					if (num != 3170576990u)
					{
						if (num != 3249042673u)
						{
							goto IL_B27;
						}
						if (!(text2 == "Finish_Now"))
						{
							goto IL_B27;
						}
						this.MaybeShowFinishContractScreen(selectedBuilding);
						goto IL_B27;
					}
					else
					{
						if (!(text2 == "Buy_Droid"))
						{
							goto IL_B27;
						}
						this.ShowDroidPurchaseScreen();
						goto IL_B27;
					}
				}
				else if (num != 3369262303u)
				{
					if (num != 3553418121u)
					{
						goto IL_B27;
					}
					if (!(text2 == "Materials"))
					{
						goto IL_B27;
					}
					goto IL_949;
				}
				else
				{
					if (!(text2 == "Inventory"))
					{
						goto IL_B27;
					}
					screenBase = this.CreatePrizeInventoryScreen();
					goto IL_B27;
				}
			}
			else if (num <= 3730279344u)
			{
				if (num != 3705035655u)
				{
					if (num != 3730279344u)
					{
						goto IL_B27;
					}
					if (!(text2 == "Commission"))
					{
						goto IL_B27;
					}
					screenBase = new TroopTrainingScreen(selectedBuilding);
					goto IL_B27;
				}
				else if (!(text2 == "Hire"))
				{
					goto IL_B27;
				}
			}
			else if (num != 3843674990u)
			{
				if (num != 4212025207u)
				{
					if (num != 4219141457u)
					{
						goto IL_B27;
					}
					if (!(text2 == "Squad"))
					{
						goto IL_B27;
					}
					this.SlideSquadScreenOpen();
					goto IL_B27;
				}
				else
				{
					if (!(text2 == "Armory"))
					{
						goto IL_B27;
					}
					screenBase = new ArmoryScreen(selectedBuilding);
					goto IL_B27;
				}
			}
			else
			{
				if (!(text2 == "Swap"))
				{
					goto IL_B27;
				}
				screenBase = new TurretUpgradeScreen(selectedBuilding, true);
				Service.Get<BILoggingController>().TrackGameAction("UI_base", "turret_swap", buildingType.Uid + "|" + this.player.PlanetId, "", 1);
				goto IL_B27;
			}
			IL_78D:
			screenBase = new TroopTrainingScreen(selectedBuilding);
			goto IL_B27;
			IL_949:
			Service.Get<ICurrencyController>().CollectCurrency(selectedBuilding);
			IL_B27:
			Service.Get<EventManager>().SendEvent(EventId.ContextButtonClicked, button.Tag);
			if (screenBase != null)
			{
				Service.Get<ScreenController>().AddScreen(screenBase);
			}
		}

		public ScreenBase CreatePrizeInventoryScreen()
		{
			ServerPlayerPrefs serverPlayerPrefs = Service.Get<ServerPlayerPrefs>();
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			sharedPlayerPrefs.SetPref("HQInvLastViewTime", ServerTime.Time.ToString());
			serverPlayerPrefs.SetPref(ServerPref.NumInventoryItemsNotViewed, "0");
			Service.Get<ServerAPI>().Sync(new SetPrefsCommand(false));
			Service.Get<EventManager>().SendEvent(EventId.HUDInventoryScreenOpened, null);
			Service.Get<EventManager>().SendEvent(EventId.NumInventoryItemsNotViewedUpdated, null);
			return new PrizeInventoryScreen();
		}

		public void InitialNavigationCenterPlanetSelect(Entity entity, BuildingTypeVO buildingTypeVO, OnScreenModalResult callback)
		{
			Service.Get<ScreenController>().AddScreen(new NavigationCenterUpgradeScreen(entity, buildingTypeVO, callback));
			Service.Get<CurrentPlayer>().SetFreeRelocation();
		}

		private void SelectWallGroup(Entity selectedBuilding)
		{
			Service.Get<BuildingController>().SelectAdjacentWalls(selectedBuilding);
			this.ShowContextButtons(selectedBuilding);
		}

		private void RotateCurrentSelection(Entity selectedBuilding)
		{
			Service.Get<BuildingController>().RotateCurrentSelection(selectedBuilding);
		}

		private void CancelContractOnBuilding(Entity selectedBuilding)
		{
			CancelScreen.ShowModal(selectedBuilding, new OnScreenModalResult(this.OnCancelModalResult), null);
		}

		private void OnCancelModalResult(object result, object cookie)
		{
			Entity entity = result as Entity;
			if (entity == null)
			{
				return;
			}
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(entity.Get<BuildingComponent>().BuildingTO.Key);
			if (contract != null)
			{
				if (ContractUtils.IsTroopType(ContractUtils.GetContractType(contract.DeliveryType)))
				{
					Service.Get<ISupportController>().CancelTroopTrainContract(contract.ProductUid, entity);
				}
				else
				{
					Service.Get<ISupportController>().CancelCurrentBuildingContract(contract, entity);
				}
				this.ShowContextButtons(entity);
			}
		}

		private void ShowDroidPurchaseScreen()
		{
			if (this.player.CurrentDroidsAmount >= this.player.MaxDroidsAmount)
			{
				return;
			}
			DroidMoment droidMoment = this.droidMoment;
			this.droidMoment = new DroidMoment();
			if (droidMoment != null)
			{
				droidMoment.DestroyDroidMoment();
			}
			YesNoScreen.ShowModal(LangUtils.GetBuildingVerb(BuildingType.DroidHut), this.lang.Get("PURCHASE_DROID", new object[]
			{
				GameUtils.DroidCrystalCost(this.player.CurrentDroidsAmount)
			}), true, true, false, new OnScreenModalResult(this.OnBuyDroid), null, false);
		}

		private void OnBuyDroid(object result, object cookie)
		{
			bool flag = result != null;
			bool happy = false;
			if (flag)
			{
				happy = GameUtils.BuyNextDroid(false);
			}
			if (this.droidMoment != null)
			{
				this.droidMoment.HideDroidMoment(happy);
			}
			if (!flag)
			{
				Service.Get<EventManager>().SendEvent(EventId.DroidPurchaseCancelled, null);
				return;
			}
			Entity selectedBuilding = Service.Get<BuildingController>().SelectedBuilding;
			if (selectedBuilding != null)
			{
				BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
				if (buildingComponent.BuildingType.Type == BuildingType.DroidHut)
				{
					this.ShowContextButtons(selectedBuilding);
				}
			}
		}

		private void MaybeShowFinishContractScreen(Entity selectedBuilding)
		{
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(selectedBuilding.Get<BuildingComponent>().BuildingTO.Key);
			int crystalCostToFinishContract = ContractUtils.GetCrystalCostToFinishContract(contract);
			if (crystalCostToFinishContract >= GameConstants.CRYSTAL_SPEND_WARNING_MINIMUM)
			{
				FinishNowScreen.ShowModal(selectedBuilding, new OnScreenModalResult(this.FinishContractOnBuilding), null);
				return;
			}
			this.FinishContractOnBuilding(selectedBuilding, null);
		}

		private void FinishContractOnBuilding(object result, object cookie)
		{
			if (result == null)
			{
				return;
			}
			Entity entity = (Entity)result;
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComponent.BuildingTO.Key);
			if (contract == null)
			{
				return;
			}
			int crystalCostToFinishContract = ContractUtils.GetCrystalCostToFinishContract(contract);
			if (!GameUtils.SpendCrystals(crystalCostToFinishContract))
			{
				return;
			}
			if (ContractUtils.IsTroopType(ContractUtils.GetContractType(contract.DeliveryType)))
			{
				Service.Get<ISupportController>().BuyoutAllTroopTrainContracts(entity, true);
			}
			else
			{
				Service.Get<ISupportController>().BuyOutCurrentBuildingContract(entity, true);
			}
			if (entity == Service.Get<BuildingController>().SelectedBuilding)
			{
				this.ShowContextButtons(entity);
			}
			if (buildingComponent != null)
			{
				BuildingTypeVO buildingType = buildingComponent.BuildingType;
				if (buildingType != null)
				{
					int currencyAmount = -crystalCostToFinishContract;
					int itemCount = 1;
					string context = "";
					IDataController dataController = Service.Get<IDataController>();
					string type;
					string itemType;
					string itemId;
					switch (contract.DeliveryType)
					{
					case DeliveryType.UpgradeTroop:
					{
						TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(contract.ProductUid);
						type = "speed_up_research";
						itemType = StringUtils.ToLowerCaseUnderscoreSeperated(troopTypeVO.Type.ToString());
						itemId = troopTypeVO.TroopID;
						context = troopTypeVO.Lvl.ToString();
						break;
					}
					case DeliveryType.UpgradeStarship:
					{
						SpecialAttackTypeVO specialAttackTypeVO = dataController.Get<SpecialAttackTypeVO>(contract.ProductUid);
						type = "speed_up_research";
						itemType = StringUtils.ToLowerCaseUnderscoreSeperated(specialAttackTypeVO.SpecialAttackName);
						itemId = specialAttackTypeVO.SpecialAttackID;
						context = specialAttackTypeVO.Lvl.ToString();
						break;
					}
					case DeliveryType.UpgradeEquipment:
					{
						EquipmentVO equipmentVO = dataController.Get<EquipmentVO>(contract.ProductUid);
						type = "speed_up_research";
						itemType = StringUtils.ToLowerCaseUnderscoreSeperated(equipmentVO.GetType().ToString());
						itemId = equipmentVO.EquipmentID;
						context = equipmentVO.Lvl.ToString();
						break;
					}
					default:
						itemType = StringUtils.ToLowerCaseUnderscoreSeperated(buildingType.Type.ToString());
						itemId = buildingType.BuildingID;
						type = (this.player.CampaignProgress.FueInProgress ? "FUE_speed_up_building" : "speed_up_building");
						break;
					}
					string subType = "consumable";
					Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, itemId, itemCount, type, subType, context);
				}
			}
		}

		private void OnTooltipButtonClicked(UXButton button)
		{
			Service.Get<UXController>().MiscElementsManager.ShowHudTooltip(button);
		}

		private bool AttemptToShowFactionFlipInfo()
		{
			bool result = false;
			if (GameUtils.HasUserFactionFlipped(Service.Get<CurrentPlayer>()) && Service.Get<CurrentPlayer>().NumIdentities > 1)
			{
				ServerPlayerPrefs serverPlayerPrefs = Service.Get<ServerPlayerPrefs>();
				if (serverPlayerPrefs.GetPref(ServerPref.FactionFlippingViewed) == "1")
				{
					result = true;
					serverPlayerPrefs.SetPref(ServerPref.FactionFlippingViewed, "0");
					Service.Get<ServerAPI>().Enqueue(new SetPrefsCommand(false));
					string title = this.lang.Get(this.FACTION_FLIP_ALERT_TITLE, new object[0]);
					string message = this.lang.Get(this.FACTION_FLIP_ALERT_DESC, new object[0]);
					AlertScreen.ShowModal(false, title, message, null, null);
				}
			}
			return result;
		}

		private void OnBaseScoreButtonClicked(UXButton button)
		{
			if (GameConstants.ENABLE_FACTION_ICON_UPGRADES)
			{
				if (!this.AttemptToShowFactionFlipInfo())
				{
					Service.Get<UXController>().MiscElementsManager.ShowHudFactionIconTooltip(button);
					return;
				}
			}
			else if (GameUtils.HasUserFactionFlipped(Service.Get<CurrentPlayer>()) && Service.Get<CurrentPlayer>().NumIdentities > 1)
			{
				IState currentState = Service.Get<GameStateMachine>().CurrentState;
				if (currentState is HomeState || currentState is EditBaseState)
				{
					Service.Get<ScreenController>().AddScreen(new FactionFlipScreen());
					Service.Get<EventManager>().SendEvent(EventId.UIFactionFlipOpened, "hud");
					return;
				}
			}
			else
			{
				Service.Get<UXController>().MiscElementsManager.ShowHudTooltip(button);
			}
		}

		private void OnBattleButtonClicked(UXButton button)
		{
			this.OpenPlanetViewScreen();
		}

		private void OnWarButtonClicked(UXButton button)
		{
			this.warButton.Enabled = false;
			SquadController squadController = Service.Get<SquadController>();
			SquadWarStatusType currentStatus = squadController.WarManager.GetCurrentStatus();
			string text = null;
			Squad currentSquad = squadController.StateManager.GetCurrentSquad();
			if (currentSquad != null)
			{
				text = currentSquad.SquadID;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "NULL";
			}
			if (currentStatus == SquadWarStatusType.PhasePrep && squadController.WarManager.CurrentSquadWar != null)
			{
				string warId = squadController.WarManager.CurrentSquadWar.WarId;
				Service.Get<SharedPlayerPrefs>().SetPref("WarPrepBadge", warId);
			}
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			int highestLevelHQ = buildingLookupController.GetHighestLevelHQ();
			Service.Get<BILoggingController>().TrackGameAction(highestLevelHQ.ToString(), "UI_squadwar_HUD", text + "|" + ServerTime.Time.ToString(), null);
			Service.Get<EventManager>().SendEvent(EventId.WarLaunchFlow, null);
		}

		public void OpenPlanetViewScreen()
		{
			this.OpenPlanetViewScreen(CampaignScreenSection.Main);
		}

		public void OpenPlanetViewScreen(CampaignScreenSection setSection)
		{
			Service.Get<ScreenController>().CloseAll();
			Service.Get<GalaxyViewController>().GoToPlanetView(this.player.Planet.Uid, setSection);
			Service.Get<EventManager>().SendEvent(EventId.HUDBattleButtonClicked, null);
		}

		public void OpenBattleLog()
		{
			this.HideLogJewel();
			Service.Get<ScreenController>().AddScreen(new BattleLogScreen());
		}

		public void OpenLeaderBoard()
		{
			Service.Get<ScreenController>().AddScreen(new LeaderboardsScreen(true, null));
		}

		public void OpenSquadMessageScreen()
		{
			Service.Get<SquadController>().StateManager.SquadScreenState = SquadScreenState.Chat;
			this.SlideSquadScreenOpen();
		}

		public void OpenSquadAdvancementScreen()
		{
			Service.Get<SquadController>().StateManager.SquadScreenState = SquadScreenState.Advancement;
			this.SlideSquadScreenOpen();
		}

		public void OpenConflictLeaderBoardWithPlanet(string planetId)
		{
			Service.Get<ScreenController>().AddScreen(new LeaderboardsScreen(true, planetId));
		}

		private void OnLogButtonClicked(UXButton button)
		{
			this.OpenBattleLog();
			Service.Get<EventManager>().SendEvent(EventId.HUDBattleLogButtonClicked, null);
		}

		private void OnLeaderboardButtonClicked(UXButton button)
		{
			Service.Get<ScreenController>().AddScreen(new LeaderboardsScreen(true, null));
			Service.Get<EventManager>().SendEvent(EventId.HUDLeaderboardButtonClicked, null);
		}

		private void OnHolonetButtonClicked(UXButton button)
		{
			Service.Get<HolonetController>().OpenHolonet();
			Service.Get<EventManager>().SendEvent(EventId.HUDHolonetButtonClicked, null);
		}

		private void OnSettingsButtonClicked(UXButton button)
		{
			Service.Get<ScreenController>().AddScreen(new SettingsScreen());
			Service.Get<EventManager>().SendEvent(EventId.HUDSettingsButtonClicked, null);
		}

		private void OnSquadsButtonClicked(UXButton button)
		{
			this.OpenJoinSquadPanel();
			Service.Get<EventManager>().SendEvent(EventId.HUDSquadsButtonClicked, null);
		}

		public void OpenJoinSquadPanelAfterDelay()
		{
			this.Visible = false;
			Service.Get<ViewTimerManager>().CreateViewTimer(1f, false, new TimerDelegate(this.OpenJoinSquadPanelCallback), null);
		}

		private void OpenJoinSquadPanelCallback(uint timerId, object cookie)
		{
			this.OpenJoinSquadPanel();
		}

		public void OpenJoinSquadPanel()
		{
			Service.Get<ScreenController>().AddScreen(new SquadJoinScreen());
		}

		private void OpenSquadBuildingInfoScreen(Entity building)
		{
			ServerPlayerPrefs serverPlayerPrefs = Service.Get<ServerPlayerPrefs>();
			string pref = serverPlayerPrefs.GetPref(ServerPref.SquadIntroViewed);
			bool flag = pref == "1";
			Service.Get<ScreenController>().AddScreen((Service.Get<SquadController>().StateManager.GetCurrentSquad() != null | flag) ? new SquadBuildingScreen(building) : new SquadIntroScreen());
			if (!flag)
			{
				serverPlayerPrefs.SetPref(ServerPref.SquadIntroViewed, "1");
				SetPrefsCommand command = new SetPrefsCommand(false);
				Service.Get<ServerAPI>().Enqueue(command);
			}
		}

		public void UpdateSquadJewelCount()
		{
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (GameConstants.SQUAD_INVITES_ENABLED && currentSquad == null && this.clansJewel != null)
			{
				List<SquadInvite> squadInvites = Service.Get<SquadController>().StateManager.SquadInvites;
				this.clansJewel.Value = ((squadInvites != null) ? squadInvites.Count : 0);
			}
		}

		private void UpdateLogJewel()
		{
			if (this.logJewel != null && !this.logVisited)
			{
				this.logJewel.Value = Service.Get<PvpManager>().GetBattlesThatHappenOffline().Count;
				this.logVisited = true;
			}
		}

		private void HideLogJewel()
		{
			if (this.logJewel != null)
			{
				this.logJewel.Value = 0;
			}
		}

		private int GetBadgeCount()
		{
			return StoreScreen.CountUnlockedUnbuiltBuildings();
		}

		private void UpdateStoreJewel()
		{
			if (this.leiJewel != null)
			{
				this.leiJewel.Value = 0;
			}
			LimitedEditionItemController limitedEditionItemController = Service.Get<LimitedEditionItemController>();
			if (limitedEditionItemController.ValidLEIs.Count > 0)
			{
				this.HideStoreJewel();
				if (this.leiJewel != null)
				{
					this.leiJewel.Text = this.lang.Get("LIMITED_EDITION_JEWEL", new object[0]);
				}
				return;
			}
			if (this.storeJewel != null)
			{
				IState currentState = Service.Get<GameStateMachine>().CurrentState;
				if (((currentState is HomeState && !((HomeState)currentState).ForceReloadMap) || currentState is EditBaseState) && Service.Get<BuildingLookupController>().GetCurrentHQ() != null)
				{
					int badgeCount = this.GetBadgeCount();
					this.storeJewel.Value = badgeCount;
				}
			}
		}

		private void HideStoreJewel()
		{
			if (this.storeJewel != null)
			{
				this.storeJewel.Value = 0;
			}
		}

		private void UpdateHolonetJewel()
		{
			if (this.holonetJewel != null)
			{
				int value = Service.Get<HolonetController>().CalculateBadgeCount();
				this.holonetJewel.Value = value;
			}
		}

		private void UpdateWarButton()
		{
			if (this.warButton != null)
			{
				UXElement element = base.GetElement<UXElement>("WarAction");
				UXElement element2 = base.GetElement<UXElement>("WarPrep");
				UXElement element3 = base.GetElement<UXElement>("WarReward");
				UXElement element4 = base.GetElement<UXElement>("WarTutorial");
				UXLabel element5 = base.GetElement<UXLabel>("LabelWar");
				bool flag = false;
				bool flag2 = Service.Get<BuildingLookupController>() != null;
				if (flag2)
				{
					flag = (Service.Get<BuildingLookupController>().GetHighestLevelHQ() >= GameConstants.WAR_PARTICIPANT_MIN_LEVEL);
				}
				int pref = Service.Get<SharedPlayerPrefs>().GetPref<int>("WarTut");
				element4.Visible = (pref < 1 & flag);
				this.warButton.Enabled = true;
				element.Visible = false;
				element2.Visible = false;
				element3.Visible = false;
				this.warJewel.Value = 0;
				this.warPrepJewel.Value = 0;
				string id = string.Empty;
				SquadWarManager warManager = Service.Get<SquadController>().WarManager;
				SquadWarStatusType currentStatus = warManager.GetCurrentStatus();
				SquadWarParticipantState currentParticipantState = warManager.GetCurrentParticipantState();
				switch (currentStatus)
				{
				case SquadWarStatusType.PhaseOpen:
					id = "WAR_BUTTON_OPEN_PHASE";
					break;
				case SquadWarStatusType.PhasePrep:
				{
					SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
					string pref2 = sharedPlayerPrefs.GetPref<string>("WarPrepBadge");
					if (pref2 == warManager.CurrentSquadWar.WarId)
					{
						this.warPrepJewel.Value = 0;
					}
					else
					{
						this.warPrepJewel.Text = this.lang.Get("WAR_EXCLAMATION", new object[0]);
					}
					element2.Visible = true;
					id = "WAR_BUTTON_PREP_PHASE";
					break;
				}
				case SquadWarStatusType.PhasePrepGrace:
					element2.Visible = true;
					id = "WAR_BUTTON_PREP_PHASE";
					break;
				case SquadWarStatusType.PhaseAction:
					this.warJewel.Value = ((currentParticipantState != null) ? currentParticipantState.TurnsLeft : 0);
					element.Visible = true;
					id = "WAR_BUTTON_ACTION_PHASE";
					break;
				case SquadWarStatusType.PhaseActionGrace:
					element.Visible = true;
					id = "WAR_BUTTON_ACTION_PHASE";
					break;
				case SquadWarStatusType.PhaseCooldown:
					this.warPrepJewel.Text = ((warManager.GetCurrentPlayerCurrentWarReward() != null) ? this.lang.Get("WAR_EXCLAMATION", new object[0]) : "");
					element3.Visible = true;
					id = "WAR_BUTTON_COOLDOWN_PHASE";
					break;
				}
				element5.Text = this.lang.Get(id, new object[0]);
			}
		}

		private void OnDisabledDeployableControlSelected(DeployableTroopControl control, string errorStringId)
		{
			if (this.battleControlsSelectedCheckbox != null && this.battleControlsSelectedCheckbox.Enabled)
			{
				this.battleControlsSelectedCheckbox.TroopCheckbox.Selected = true;
			}
			control.TroopCheckbox.Selected = false;
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(this.lang.Get(errorStringId, new object[0]));
		}

		private void OnTroopCheckboxSelected(UXCheckbox checkbox, bool selected)
		{
			if (!selected)
			{
				return;
			}
			string text = checkbox.Tag as string;
			if (!string.IsNullOrEmpty(text) && this.deployableTroops != null && this.deployableTroops.ContainsKey(text))
			{
				TroopTypeVO troopType = Service.Get<IDataController>().Get<TroopTypeVO>(text);
				DeployableTroopControl deployableTroopControl = this.deployableTroops[text];
				if (deployableTroopControl.Enabled)
				{
					this.SelectCheckbox(deployableTroopControl);
					Service.Get<DeployerController>().EnterTroopPlacementMode(troopType);
					return;
				}
				this.OnDisabledDeployableControlSelected(deployableTroopControl, "TROOP_INVALID");
			}
		}

		private void OnSpecialAttackCheckboxSelected(UXCheckbox checkbox, bool selected)
		{
			if (!selected)
			{
				return;
			}
			string text = checkbox.Tag as string;
			if (!string.IsNullOrEmpty(text) && this.deployableTroops != null && this.deployableTroops.ContainsKey(text))
			{
				SpecialAttackTypeVO specialAttackType = Service.Get<IDataController>().Get<SpecialAttackTypeVO>(text);
				DeployableTroopControl deployableTroopControl = this.deployableTroops[text];
				if (deployableTroopControl.Enabled)
				{
					this.SelectCheckbox(deployableTroopControl);
					Service.Get<DeployerController>().EnterSpecialAttackPlacementMode(specialAttackType);
					return;
				}
				if (deployableTroopControl.DisableDueToBuildingDestruction)
				{
					this.OnDisabledDeployableControlSelected(deployableTroopControl, "STARSHIP_TRAINER_DESTROYED");
					return;
				}
				this.OnDisabledDeployableControlSelected(deployableTroopControl, "TROOP_INVALID");
			}
		}

		private void OnHeroCheckboxSelected(UXCheckbox checkbox, bool selected)
		{
			if (!selected)
			{
				return;
			}
			string text = checkbox.Tag as string;
			if (!string.IsNullOrEmpty(text) && this.deployableTroops != null && this.deployableTroops.ContainsKey(text))
			{
				TroopTypeVO troopTypeVO = Service.Get<IDataController>().Get<TroopTypeVO>(text);
				DeployableTroopControl deployableTroopControl = this.deployableTroops[text];
				if (deployableTroopControl.Enabled)
				{
					if (deployableTroopControl.AbilityState == HeroAbilityState.Prepared)
					{
						deployableTroopControl.UseHeroAbility();
						Service.Get<TroopAbilityController>().UserActivateAbility(deployableTroopControl.HeroEntityID);
						return;
					}
					this.SelectCheckbox(deployableTroopControl);
					Service.Get<DeployerController>().EnterHeroDeployMode(troopTypeVO);
					return;
				}
				else
				{
					string errorStringId;
					if (deployableTroopControl.AbilityState == HeroAbilityState.InUse)
					{
						errorStringId = "HERO_ABILITY_ACTIVE";
					}
					else if (deployableTroopControl.AbilityState == HeroAbilityState.CoolingDown)
					{
						errorStringId = "HERO_ABILITY_COOLDOWN";
					}
					else if (deployableTroopControl.DisableDueToBuildingDestruction)
					{
						errorStringId = "HERO_TRAINER_DESTROYED";
					}
					else
					{
						int playerDeployableHeroCount = Service.Get<BattleController>().GetPlayerDeployableHeroCount(text);
						if (playerDeployableHeroCount > 0)
						{
							errorStringId = "CANNOT_DEPLOY_MULTIPLE_HEROES";
						}
						else
						{
							errorStringId = "CANNOT_DEPLOY_THIS_HERO";
						}
					}
					this.OnDisabledDeployableControlSelected(deployableTroopControl, errorStringId);
					Service.Get<EventManager>().SendEvent(EventId.HeroNotActivated, troopTypeVO);
				}
			}
		}

		private void OnChampionCheckboxSelected(UXCheckbox checkbox, bool selected)
		{
			if (!selected)
			{
				return;
			}
			string text = checkbox.Tag as string;
			if (!string.IsNullOrEmpty(text) && this.deployableTroops != null && this.deployableTroops.ContainsKey(text))
			{
				TroopTypeVO troopType = Service.Get<IDataController>().Get<TroopTypeVO>(text);
				DeployableTroopControl deployableTroopControl = this.deployableTroops[text];
				if (deployableTroopControl.Enabled)
				{
					this.SelectCheckbox(deployableTroopControl);
					Service.Get<DeployerController>().EnterChampionDeployMode(troopType);
					return;
				}
				if (deployableTroopControl.DisableDueToBuildingDestruction)
				{
					this.OnDisabledDeployableControlSelected(deployableTroopControl, "SQUAD_CENTER_DESTROYED");
					return;
				}
				this.OnDisabledDeployableControlSelected(deployableTroopControl, "TROOP_INVALID");
			}
		}

		private void OnSquadTroopsCheckboxSelected(UXCheckbox checkbox, bool selected)
		{
			if (!selected)
			{
				return;
			}
			if (this.deployableTroops != null && this.deployableTroops.ContainsKey("squadTroops"))
			{
				DeployableTroopControl deployableTroopControl = this.deployableTroops["squadTroops"];
				if (deployableTroopControl.Enabled)
				{
					this.SelectCheckbox(deployableTroopControl);
					Service.Get<DeployerController>().EnterSquadTroopPlacementMode();
					return;
				}
				this.OnDisabledDeployableControlSelected(deployableTroopControl, "TROOP_INVALID");
			}
		}

		private void SelectCheckbox(DeployableTroopControl control)
		{
			if (control.TroopCheckbox.RadioGroup != this.battleControlsSelectedGroup && this.battleControlsSelectedCheckbox != null)
			{
				this.battleControlsSelectedCheckbox.TroopCheckbox.Selected = false;
			}
			this.battleControlsSelectedGroup = control.TroopCheckbox.RadioGroup;
			this.battleControlsSelectedCheckbox = control;
		}

		public void UpdateMedalsAvailable(int gain, int lose)
		{
			this.medalsGainLabel.Text = this.lang.ThousandsSeparated(gain);
			this.medalsLoseLabel.Text = this.lang.ThousandsSeparated(lose);
		}

		public void UpdateTournamentRatingBattleDelta(int gain, int lose, string planetId)
		{
			TournamentController tournamentController = Service.Get<TournamentController>();
			bool flag = tournamentController.IsThisTournamentLive(tournamentController.CurrentPlanetActiveTournament);
			if (flag)
			{
				this.tournamentRatingGainSprite.SpriteName = GameUtils.GetTournamentPointIconName(planetId);
				this.tournamentRatingLoseSprite.SpriteName = GameUtils.GetTournamentPointIconName(planetId);
				this.tournamentRatingGainLabel.Text = gain.ToString();
				this.tournamentRatingLoseLabel.Text = lose.ToString();
			}
			this.tournamentRatingGainGroup.Visible = flag;
			this.tournamentRatingLoseGroup.Visible = flag;
		}

		private void ShowLootElements()
		{
			this.lootGroup.Visible = true;
			this.lootContrabandIcon.Visible = this.player.IsContrabandUnlocked;
			this.lootContrabandLabel.Visible = this.player.IsContrabandUnlocked;
			LootController lootController = Service.Get<BattleController>().LootController;
			if (lootController != null)
			{
				string text = this.lang.ThousandsSeparated(lootController.GetTotalLootAvailable(CurrencyType.Credits));
				string text2 = this.lang.ThousandsSeparated(lootController.GetTotalLootAvailable(CurrencyType.Materials));
				string text3 = this.lang.ThousandsSeparated(lootController.GetTotalLootAvailable(CurrencyType.Contraband));
				this.lootCreditsLabel.Text = text;
				this.lootMaterialLabel.Text = text2;
				this.lootContrabandLabel.Text = text3;
			}
		}

		private void HideLootElements()
		{
			this.lootGroup.Visible = false;
			this.lootCreditsLabel.Text = "0";
			this.lootMaterialLabel.Text = "0";
			this.lootContrabandLabel.Text = "0";
		}

		private void RefreshLoot()
		{
			LootController lootController = Service.Get<BattleController>().LootController;
			if (lootController != null)
			{
				string text = this.lang.ThousandsSeparated((int)lootController.GetRemainingLoot(CurrencyType.Credits));
				string text2 = this.lang.ThousandsSeparated((int)lootController.GetRemainingLoot(CurrencyType.Materials));
				string text3 = this.lang.ThousandsSeparated((int)lootController.GetRemainingLoot(CurrencyType.Contraband));
				this.lootCreditsLabel.Text = text;
				this.lootMaterialLabel.Text = text2;
				this.lootContrabandLabel.Text = text3;
			}
		}

		private void OnReplaySpeedChangeButtonClicked(UXButton button)
		{
			Service.Get<BattlePlaybackController>().FastForward();
			this.UpdateCurrentReplaySpeedUI();
		}

		public void UpdateCurrentReplaySpeedUI()
		{
			int currentPlaybackScale = (int)Service.Get<BattlePlaybackController>().CurrentPlaybackScale;
			this.replaySpeedLabel.Text = this.lang.Get("replay_playback_speed", new object[]
			{
				currentPlaybackScale
			});
		}

		public void ShowReplayTimer()
		{
			this.replayTimeLeftLabel.Visible = true;
			this.RefreshReplayTimerView(Service.Get<BattleController>().GetCurrentBattle().TimeLeft);
		}

		public bool AreBattleStarsVisible()
		{
			return this.damageStarGroup.Visible;
		}

		protected internal HUD(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AddContextButton(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AddContextButton(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AddContextButton(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), Marshal.PtrToStringUni(*(IntPtr*)(args + 6)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).AddContextButton(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), *(int*)(args + 5), Marshal.PtrToStringUni(*(IntPtr*)(args + 6)), Marshal.PtrToStringUni(*(IntPtr*)(args + 7))));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AddContextButtons((Entity)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0, *(sbyte*)(args + 4) != 0, *(sbyte*)(args + 5) != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AddContextButtonWithTimer(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), (GetTimerSecondsDelegate)GCHandledObjects.GCHandleToObject(args[6]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AddCustomContextButton(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AddFinishContextButton((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AddHQInventoryContextButtonIfProper();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AddSquadContextButtons((Entity)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).AnimateControls(*(sbyte*)args != 0, *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).AreBattleStarsVisible());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).AttemptToShowFactionFlipInfo());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).CancelContractOnBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).CleanupDeployableTroops();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ConfigureControls((HudConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).CreateDeployableControls((Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]), (UXCheckboxSelectedDelegate)GCHandledObjects.GCHandleToObject(args[2]), (CurrentBattle)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).CreatePrizeInventoryScreen());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).CreateSquadDeployableControl();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).CreateSquadScreen();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).DeselectAllDeployableControlers();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).DestroySquadScreen();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).DisableHeroDeploys();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).DisableSpecialAttacks();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).DisableSquadDeploy();
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).DisableWarAttacksUI();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).EnableNextBattleButton();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).FinishContractOnBuilding(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).FullyDisableDeployableControls(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).FullyEnableDeployablControls();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).BaseLayoutToolView);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).HiddenInQueue);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).ReadyToToggleVisiblity);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).Visible);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).GetBadgeCount());
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).GetBuildingType((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).GetContextButtonFromPool((UXElement)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (GameObject)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).GetDamageStarAnchor());
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).GetDamageStarAnimator());
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).GetDamageStarLabel());
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).HideLogJewel();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).HideLootElements();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).HideStoreJewel();
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitCountdownGroup();
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitDamageGroup();
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitGrids();
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitialNavigationCenterPlanetSelect((Entity)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitLoot();
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitNeighborGroup();
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitReplayControls();
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitSliders();
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).InitTournamentRatingGroup();
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).IsAnimationWhiteListed((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).IsElementProvidedTroop((UXElement)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).IsSquadScreenOpenAndCloseable());
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).IsSquadScreenOpenOrOpeningAndCloseable());
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).LoadFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).LoadSuccess(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).MaybeShowFinishContractScreen((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnBaseScoreButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnBattleButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnBuyDroid(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnCancelModalResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnChampionCheckboxSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnChampionDeployed((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnContextButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnCrystalButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnDisabledContextButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnDisabledDeployableControlSelected((DeployableTroopControl)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnDroidButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnEditButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnEndBattleButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnHeroCheckboxSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnHeroDeployed((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnHolonetButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnHomeButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnLeaderboardButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnLogButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnNextBattleButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnOpponentBuffsButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnPerformanceDeviceMemUsage(*args);
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnPerformanceFPS(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnReplaySpeedChangeButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnSettingsButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnShieldButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnSpecialAttackCheckboxSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnSpecialAttackDeployed((SpecialAttack)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnSpecialPromotionButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnSquadsButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnSquadTroopsCheckboxSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnSquadTroopsDeployed();
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnSquadWarAttackResultCallback(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnStoreButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnTooltipButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnTroopCheckboxSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnTroopPlaced((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnWarAttackClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnWarButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OnYourBuffsButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenBattleLog();
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenConflictLeaderBoardWithPlanet(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenJoinSquadPanel();
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenJoinSquadPanelAfterDelay();
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenLeaderBoard();
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenPlanetViewScreen();
			return -1L;
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenPlanetViewScreen((CampaignScreenSection)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenSquadAdvancementScreen();
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenSquadBuildingInfoScreen((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).OpenSquadMessageScreen();
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).PrepForSquadScreenCreate();
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshAllResourceViews(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshBaseName();
			return -1L;
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshCurrentPlayerLevel();
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshFactionIconVisibility(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (GamePlayer)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshLoot();
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshPlayerMedals();
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshPlayerSocialInformation();
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshReplayTimerView(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshResourceView(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshTimerView(*(int*)args, *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ResetDamageStars();
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).RotateCurrentSelection((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SelectCheckbox((DeployableTroopControl)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SelectWallGroup((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).BaseLayoutToolView = (HUDBaseLayoutToolView)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).HiddenInQueue = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ReadyToToggleVisiblity = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).Visible = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SetCountdownTime(*(float*)args, *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SetOpponentLevelVisibility(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SetSquadScreenAlwaysOnTop(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SetSquadScreenVisibility(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SetupDeployableTroops();
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).ShouldDestroyOrHideHomeStateUI((IState)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HUD)GCHandledObjects.GCHandleToObject(instance)).ShouldDisableContextButton(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4)));
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ShowContextButtons((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ShowCountdown(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ShowDroidPurchaseScreen();
			return -1L;
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ShowLootElements();
			return -1L;
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ShowPromoButtonGlowEffect();
			return -1L;
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ShowReplayTimer();
			return -1L;
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ShowScoutAttackFailureMessage((SquadWarScoutState)(*(int*)args), (CurrentBattle)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ShowScoutingUplinksAvailable();
			return -1L;
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SlideSquadScreenClosed();
			return -1L;
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SlideSquadScreenClosedInstantly();
			return -1L;
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).SlideSquadScreenOpen();
			return -1L;
		}

		public unsafe static long $Invoke153(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ToggleContextButton((UXElement)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke154(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ToggleContextButton(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke155(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).ToggleExitEditModeButton(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke156(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).TogglePerformanceDisplay(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke157(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateChampionCount(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke158(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateContextTimerLabel((ContextButtonTag)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke159(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentReplaySpeedUI();
			return -1L;
		}

		public unsafe static long $Invoke160(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateDamageStars(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke161(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateDamageValue(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke162(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateDeployableCount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke163(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateDeployInstructionLabel();
			return -1L;
		}

		public unsafe static long $Invoke164(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateDroidCount();
			return -1L;
		}

		public unsafe static long $Invoke165(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateHeroCount(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke166(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateHolonetButtonVisibility(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke167(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateHolonetJewel();
			return -1L;
		}

		public unsafe static long $Invoke168(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateLogJewel();
			return -1L;
		}

		public unsafe static long $Invoke169(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateMedalsAvailable(*(int*)args, *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke170(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateProtectionTimeLabel();
			return -1L;
		}

		public unsafe static long $Invoke171(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateSpecialAttackCount(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke172(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateSquadJewelCount();
			return -1L;
		}

		public unsafe static long $Invoke173(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateStoreJewel();
			return -1L;
		}

		public unsafe static long $Invoke174(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateTargetedBundleButtonVisibility();
			return -1L;
		}

		public unsafe static long $Invoke175(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateTargetedBundleViewTimer();
			return -1L;
		}

		public unsafe static long $Invoke176(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateTournamentRatingBattleDelta(*(int*)args, *(int*)(args + 1), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke177(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateTroopCount(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke178(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateWarAttackState();
			return -1L;
		}

		public unsafe static long $Invoke179(long instance, long* args)
		{
			((HUD)GCHandledObjects.GCHandleToObject(instance)).UpdateWarButton();
			return -1L;
		}
	}
}
