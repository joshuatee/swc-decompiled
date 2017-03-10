using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public static class GameConstants
	{
		private const global::BindingFlags bindingFlags = (global::BindingFlags)51;

		public static string ALL_LOCALES
		{
			get;
			private set;
		}

		public static int COEF_EXP_ACCURACY
		{
			get;
			private set;
		}

		public static int CREDITS_COEFFICIENT
		{
			get;
			private set;
		}

		public static int CREDITS_EXPONENT
		{
			get;
			private set;
		}

		public static int ALLOY_COEFFICIENT
		{
			get;
			private set;
		}

		public static int ALLOY_EXPONENT
		{
			get;
			private set;
		}

		public static int CONTRABAND_COEFFICIENT
		{
			get;
			private set;
		}

		public static int CONTRABAND_EXPONENT
		{
			get;
			private set;
		}

		public static int CRYSTALS_SPEED_UP_COEFFICIENT
		{
			get;
			private set;
		}

		public static int CRYSTALS_SPEED_UP_EXPONENT
		{
			get;
			private set;
		}

		public static int SQUADPERK_CRYSTALS_SPEED_UP_COEFFICIENT
		{
			get;
			private set;
		}

		public static int SQUADPERK_CRYSTALS_SPEED_UP_EXPONENT
		{
			get;
			private set;
		}

		public static string CRYSTAL_PACK_AMOUNT
		{
			get;
			private set;
		}

		public static string CRYSTAL_PACK_COST_USD
		{
			get;
			private set;
		}

		public static string PROTECTION_DURATION
		{
			get;
			private set;
		}

		public static string PROTECTION_CRYSTAL_COSTS
		{
			get;
			private set;
		}

		public static int RESPAWN_TIME_NATURAL_RESOURCE
		{
			get;
			private set;
		}

		public static int INVENTORY_INITIAL_CAP
		{
			get;
			private set;
		}

		public static int INVENTORY_LIMIT
		{
			get;
			private set;
		}

		public static int SECONDS_PER_CRYSTAL
		{
			get;
			private set;
		}

		public static string DROID_CRYSTAL_COSTS
		{
			get;
			private set;
		}

		public static string FUE_BATTLE
		{
			get;
			private set;
		}

		public static float EDIT_LONG_PRESS_PRE_FADE
		{
			get;
			private set;
		}

		public static float EDIT_LONG_PRESS_FADE
		{
			get;
			private set;
		}

		public static string POST_FUE_REBEL_BASE
		{
			get;
			private set;
		}

		public static string POST_FUE_EMPIRE_BASE
		{
			get;
			private set;
		}

		public static string NEW_PLAYER_FACTION
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_CREDITS_AMOUNT
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_MATERIALS_AMOUNT
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_CONTRABAND_AMOUNT
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_CREDITS_CAPACITY
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_MATERIALS_CAPACITY
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_CONTRABAND_CAPACITY
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_REPUTATION_AMOUNT
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_CRYSTALS_AMOUNT
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_XP_AMOUNT
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_DROIDS_AMOUNT
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_DROIDS_CAPACITY
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_TROOP_CAPACITY
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_HERO_CAPACITY
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_CHAMPION_CAPACITY
		{
			get;
			private set;
		}

		public static int NEW_PLAYER_STARSHIP_CAPACITY
		{
			get;
			private set;
		}

		public static string NEW_PLAYER_INITIAL_MISSION_REBEL
		{
			get;
			private set;
		}

		public static string NEW_PLAYER_INITIAL_MISSION_EMPIRE
		{
			get;
			private set;
		}

		public static string FUE_QUEST_UID
		{
			get;
			private set;
		}

		public static int PVP_MATCH_DURATION
		{
			get;
			private set;
		}

		public static float PVP_MATCH_COUNTDOWN
		{
			get;
			private set;
		}

		public static string PVP_SEARCH_COST_BY_HQ_LEVEL
		{
			get;
			private set;
		}

		public static int PVP_SEARCH_TIMEOUT_DURATION
		{
			get;
			private set;
		}

		public static int HQ_LOOTABLE_CREDITS
		{
			get;
			private set;
		}

		public static int HQ_LOOTABLE_MATERIALS
		{
			get;
			private set;
		}

		public static int HQ_LOOTABLE_CONTRABAND
		{
			get;
			private set;
		}

		public static int TURRET_SWAP_HQ_UNLOCK
		{
			get;
			private set;
		}

		public static float CAMPAIGN_STORY_INTRO_DELAY
		{
			get;
			private set;
		}

		public static float CAMPAIGN_STORY_SUCCESS_DELAY
		{
			get;
			private set;
		}

		public static float CAMPAIGN_STORY_FAILURE_DELAY
		{
			get;
			private set;
		}

		public static float CAMPAIGN_STORY_GoalFailure_DELAY
		{
			get;
			private set;
		}

		public static string SHIELD_HEALTH_PER_POINT
		{
			get;
			private set;
		}

		public static string SHIELD_RANGE_PER_POINT
		{
			get;
			private set;
		}

		public static int CRYSTAL_SPEND_WARNING_MINIMUM
		{
			get;
			private set;
		}

		public static int DEFAULT_BATTLE_LENGTH
		{
			get;
			private set;
		}

		public static int BATTLE_WARNING_TIME
		{
			get;
			private set;
		}

		public static int BATTLE_END_DELAY
		{
			get;
			private set;
		}

		public static int IDLE_RELOAD_TIME
		{
			get;
			private set;
		}

		public static int PAUSED_RELOAD_TIME
		{
			get;
			private set;
		}

		public static int MAX_TROOP_DONATIONS
		{
			get;
			private set;
		}

		public static int MAX_PER_USER_TROOP_DONATION
		{
			get;
			private set;
		}

		public static int SQUAD_MEMBER_LIMIT
		{
			get;
			private set;
		}

		public static int SQUAD_CREATE_MIN_TROPHY_REQ
		{
			get;
			private set;
		}

		public static int SQUAD_CREATE_MAX_TROPHY_REQ
		{
			get;
			private set;
		}

		public static int SQUAD_CREATE_COST
		{
			get;
			private set;
		}

		public static uint SQUAD_TROOP_REQUEST_THROTTLE_MINUTES
		{
			get;
			private set;
		}

		public static int SQUAD_NAME_LENGTH_MIN
		{
			get;
			private set;
		}

		public static int SQUAD_NAME_LENGTH_MAX
		{
			get;
			private set;
		}

		public static bool SQUAD_INVITES_ENABLED
		{
			get;
			private set;
		}

		public static bool SQUAD_INVITES_TO_LEADERS_ENABLED
		{
			get;
			private set;
		}

		public static int CONTRACT_REFUND_PERCENTAGE_BUILDINGS
		{
			get;
			private set;
		}

		public static int CONTRACT_REFUND_PERCENTAGE_TROOPS
		{
			get;
			private set;
		}

		public static int ATTACK_RATING_WEIGHT
		{
			get;
			private set;
		}

		public static int DEFENSE_RATING_WEIGHT
		{
			get;
			private set;
		}

		public static float PVP_MATCH_BONUS_ATTACKER_SLOPE
		{
			get;
			private set;
		}

		public static float PVP_MATCH_BONUS_ATTACKER_Y_INTERCEPT
		{
			get;
			private set;
		}

		public static bool START_FUE_IN_BATTLE_MODE
		{
			get;
			private set;
		}

		public static int USER_NAME_MAX_CHARACTERS
		{
			get;
			private set;
		}

		public static int USER_NAME_MIN_CHARACTERS
		{
			get;
			private set;
		}

		public static float KEEP_ALIVE_DISPATCH_WAIT_TIME
		{
			get;
			private set;
		}

		public static int UNDER_ATTACK_STATUS_CHECK_INTERVAL
		{
			get;
			private set;
		}

		public static int CAMPAIGN_HOURS_UPCOMING
		{
			get;
			private set;
		}

		public static int CAMPAIGN_HOURS_CLOSING
		{
			get;
			private set;
		}

		public static int TOURNAMENT_HOURS_UPCOMING
		{
			get;
			private set;
		}

		public static int TOURNAMENT_HOURS_SHOW_BADGE
		{
			get;
			private set;
		}

		public static int TOURNAMENT_HOURS_CLOSING
		{
			get;
			private set;
		}

		public static string TOURNAMENT_RATING_DELTAS_ATTACKER
		{
			get;
			private set;
		}

		public static uint TOURNAMENT_TIER_CHANGE_VIEW_THROTTLE
		{
			get;
			private set;
		}

		public static uint CONFLICT_REWARD_PREVIEW_MULT
		{
			get;
			private set;
		}

		public static uint CONFLICT_SHOW_MULTIPLIER
		{
			get;
			private set;
		}

		public static bool REFUND_SURVIVORS
		{
			get;
			private set;
		}

		public static uint SQUAD_TROOP_DEPLOY_STAGGER
		{
			get;
			private set;
		}

		public static string SQUAD_TROOP_DEPLOY_FLAG_EMPIRE_ASSET
		{
			get;
			private set;
		}

		public static string SQUAD_TROOP_DEPLOY_FLAG_REBEL_ASSET
		{
			get;
			private set;
		}

		public static bool TAPJOY_ENABLED
		{
			get;
			private set;
		}

		public static string TAPJOY_BLACKLIST
		{
			get;
			private set;
		}

		public static bool TAPJOY_AFTER_IAP
		{
			get;
			private set;
		}

		public static bool RATE_MY_APP_ENABLED
		{
			get;
			private set;
		}

		public static int FB_CONNECT_REWARD
		{
			get;
			private set;
		}

		public static int CREDITS_2_THRESHOLD
		{
			get;
			private set;
		}

		public static int CREDITS_3_THRESHOLD
		{
			get;
			private set;
		}

		public static int MATERIALS_2_THRESHOLD
		{
			get;
			private set;
		}

		public static int MATERIALS_3_THRESHOLD
		{
			get;
			private set;
		}

		public static int CONTRABAND_2_THRESHOLD
		{
			get;
			private set;
		}

		public static int CONTRABAND_3_THRESHOLD
		{
			get;
			private set;
		}

		public static int CRYSTALS_2_THRESHOLD
		{
			get;
			private set;
		}

		public static int CRYSTALS_3_THRESHOLD
		{
			get;
			private set;
		}

		public static string VO_BLACKLIST
		{
			get;
			private set;
		}

		public static int SPAWN_HEALTH_PERCENT
		{
			get;
			private set;
		}

		public static uint SPAWN_DELAY
		{
			get;
			private set;
		}

		public static int GRIND_MISSION_MAXIMUM
		{
			get;
			private set;
		}

		public static bool FORUMS_ENABLED
		{
			get;
			private set;
		}

		public static bool IAP_DISABLED_ANDROID
		{
			get;
			private set;
		}

		public static string IAP_DISCLAIMER_WHITELIST
		{
			get;
			private set;
		}

		public static bool PROMO_UNIT_TEST_ENABLED
		{
			get;
			private set;
		}

		public static bool IAP_FORCE_POPUP_ENABLED
		{
			get;
			private set;
		}

		public static bool FACTION_CHOICE_CONFIRM_SCREEN_ENABLED
		{
			get;
			private set;
		}

		public static bool SET_CALLSIGN_CONFIRM_SCREEN_ENABLED
		{
			get;
			private set;
		}

		public static float HUD_RESOURCE_TICKER_MAX_DURATION
		{
			get;
			private set;
		}

		public static float HUD_RESOURCE_TICKER_MIN_DURATION
		{
			get;
			private set;
		}

		public static int HUD_RESOURCE_TICKER_CRYSTAL_THRESHOLD
		{
			get;
			private set;
		}

		public static bool PROMO_BUTTON_RESHOW_GLOW
		{
			get;
			private set;
		}

		public static int PROMO_BUTTON_GLOW_DURATION
		{
			get;
			private set;
		}

		public static bool TARGETED_OFFERS_ENABLED
		{
			get;
			private set;
		}

		public static int TARGETED_OFFERS_FREQUENCY_LIMIT
		{
			get;
			private set;
		}

		public static int RATE_APP_INCENTIVE_CRYSTALS
		{
			get;
			private set;
		}

		public static bool PVP_LOSE_ON_PAUSE
		{
			get;
			private set;
		}

		public static bool PVP_LOSE_ON_QUIT
		{
			get;
			private set;
		}

		public static bool NO_FB_FACTION_CHOICE_ANDROID
		{
			get;
			private set;
		}

		public static bool RATE_APP_INCENTIVE_SHOW_IOS
		{
			get;
			private set;
		}

		public static bool RATE_APP_INCENTIVE_GRANT_IOS
		{
			get;
			private set;
		}

		public static bool RATE_APP_INCENTIVE_SHOW_ANDROID
		{
			get;
			private set;
		}

		public static bool RATE_APP_INCENTIVE_GRANT_ANDROID
		{
			get;
			private set;
		}

		public static bool RATE_APP_INCENTIVE_SHOW_WINDOWS
		{
			get;
			private set;
		}

		public static bool RATE_APP_INCENTIVE_GRANT_WINDOWS
		{
			get;
			private set;
		}

		public static bool QUIET_CORRECTION_ENABLED
		{
			get;
			private set;
		}

		public static string QUIET_CORRECTION_WHITELIST
		{
			get;
			private set;
		}

		public static bool ENABLE_INSTANT_BUY
		{
			get;
			private set;
		}

		public static bool ENABLE_UPGRADE_ALL_WALLS
		{
			get;
			private set;
		}

		public static float UPGRADE_ALL_WALLS_CONVENIENCE_TAX
		{
			get;
			private set;
		}

		public static int UPGRADE_ALL_WALLS_COEFFICIENT
		{
			get;
			private set;
		}

		public static int UPGRADE_ALL_WALL_EXPONENT
		{
			get;
			private set;
		}

		public static bool MESH_COMBINE_DISABLED
		{
			get;
			private set;
		}

		public static bool ASSET_BUNDLE_CACHE_CLEAN_DISABLED
		{
			get;
			private set;
		}

		public static int ASSET_BUNDLE_CACHE_CLEAN_VERSION
		{
			get;
			private set;
		}

		public static int MAX_CONCURRENT_ASSET_LOADS
		{
			get;
			private set;
		}

		public static int DEFLECTION_VELOCITY_PERCENT
		{
			get;
			private set;
		}

		public static int DEFLECTION_DURATION_MS
		{
			get;
			private set;
		}

		public static int FACTION_FLIPPING_UNLOCK_LEVEL
		{
			get;
			private set;
		}

		public static int CONTRABAND_UNLOCK_LEVEL
		{
			get;
			private set;
		}

		public static int PERFORMANCE_SAMPLE_DELAY_HOME
		{
			get;
			private set;
		}

		public static int PERFORMANCE_SAMPLE_DELAY_BATTLE
		{
			get;
			private set;
		}

		public static int FPS_THRESHOLD
		{
			get;
			private set;
		}

		public static string RAID_DEFENSE_TRAINER_BINDINGS
		{
			get;
			private set;
		}

		public static int AUTOSELECT_DISABLE_HQTHRESHOLD
		{
			get;
			private set;
		}

		public static float PUSH_NOTIFICATION_SQUAD_JOIN_COOLDOWN
		{
			get;
			private set;
		}

		public static float PUSH_NOTIFICATIONS_TROOP_REQUEST_COOLDOWN
		{
			get;
			private set;
		}

		public static bool FACEBOOK_INVITES_ENABLED
		{
			get;
			private set;
		}

		public static string PLANET_RELOCATED_TUTORIAL_ID
		{
			get;
			private set;
		}

		public static float GALAXY_AUTO_ROTATE_SPEED
		{
			get;
			private set;
		}

		public static float GALAXY_AUTO_ROTATE_DELAY
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_FOREGROUND_THRESHOLD
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_FOREGROUND_PLATEAU_THRESHOLD
		{
			get;
			private set;
		}

		public static float GALAXY_CAMERA_HEIGHT_OFFSET
		{
			get;
			private set;
		}

		public static float GALAXY_CAMERA_DISTANCE_OFFSET
		{
			get;
			private set;
		}

		public static float GALAXY_EASE_ROTATION_TIME
		{
			get;
			private set;
		}

		public static float GALAXY_EASE_ROTATION_TRANSITION_TIME
		{
			get;
			private set;
		}

		public static float GALAXY_INITIAL_GALAXY_ZOOM_DIST
		{
			get;
			private set;
		}

		public static float GALAXY_INITIAL_GALAXY_ZOOM_TIME
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_VIEW_HEIGHT
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_GALAXY_ZOOM_TIME
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_POPULATION_COUNT_PERCENTAGE
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_POPULATION_UPDATE_TIME
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_FOREGROUND_UI_THRESHOLD
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_SWIPE_MIN_MOVE
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_SWIPE_MAX_TIME
		{
			get;
			private set;
		}

		public static float GALAXY_PLANET_SWIPE_TIME
		{
			get;
			private set;
		}

		private static string CRYSTALS_PER_RELOCATION_STAR
		{
			get;
			set;
		}

		public static int[] CrystalsPerRelocationStar
		{
			get;
			private set;
		}

		private static string STARS_PER_RELOCATION
		{
			get;
			set;
		}

		public static int[] StarsPerRelocation
		{
			get;
			private set;
		}

		public static bool PUSH_NOTIFICATION_ENABLE_INCENTIVE
		{
			get;
			private set;
		}

		public static int PUSH_NOTIFICATION_CRYSTAL_REWARD_AMOUNT
		{
			get;
			private set;
		}

		public static int PUSH_NOTIFICATION_MAX_REACHED
		{
			get;
			private set;
		}

		public static float FADE_OUT_CONSTANT_LENGTH
		{
			get;
			private set;
		}

		public static float GALAXY_UI_PLANET_FOCUS_THROTTLE
		{
			get;
			private set;
		}

		public static bool MAKER_VIDEO_ENABLED
		{
			get;
			private set;
		}

		public static string MAKER_VIDEO_LOCALES
		{
			get;
			private set;
		}

		public static string[] MakerVideoLocales
		{
			get;
			private set;
		}

		public static string HOLONET_MAKER_VIDEO_URL
		{
			get;
			private set;
		}

		public static float MIN_MAKER_VID_LOAD
		{
			get;
			private set;
		}

		public static bool TIME_OF_DAY_ENABLED
		{
			get;
			private set;
		}

		public static double TOD_MID_DAY_PERCENTAGE
		{
			get;
			private set;
		}

		public static int RAIDS_HQ_UNLOCK_LEVEL
		{
			get;
			private set;
		}

		public static int RAIDS_UPCOMING_TICKER_THROTTLE
		{
			get;
			private set;
		}

		public static bool LOG_STACK_TRACE_TO_BI
		{
			get;
			private set;
		}

		public static bool PLAYDOM_BI_ENABLED
		{
			get;
			private set;
		}

		public static bool EVENT_2_BI_ENABLED
		{
			get;
			private set;
		}

		public static bool DISABLE_BASE_LAYOUT_TOOL
		{
			get;
			private set;
		}

		public static int WAR_MAX_BUFF_BASES
		{
			get;
			private set;
		}

		public static int SQUADPERK_MAX_SQUAD_LEVEL_CELEBRATIONS_SHOWN
		{
			get;
			private set;
		}

		public static string WAR_HELP_OVERVIEW_REBEL
		{
			get;
			private set;
		}

		public static string WAR_HELP_OVERVIEW_EMPIRE
		{
			get;
			private set;
		}

		public static string WAR_HELP_BASEEDIT_REBEL
		{
			get;
			private set;
		}

		public static string WAR_HELP_BASEEDIT_EMPIRE
		{
			get;
			private set;
		}

		public static string WAR_HELP_PREPARATION_REBEL
		{
			get;
			private set;
		}

		public static string WAR_HELP_PREPARATION_EMPIRE
		{
			get;
			private set;
		}

		public static string WAR_HELP_WAR_REBEL
		{
			get;
			private set;
		}

		public static string WAR_HELP_WAR_EMPIRE
		{
			get;
			private set;
		}

		public static string WAR_HELP_REWARD_REBEL
		{
			get;
			private set;
		}

		public static string WAR_HELP_REWARD_EMPIRE
		{
			get;
			private set;
		}

		public static int WAR_NOTIF_ACTION_TURNS_REMINDER
		{
			get;
			private set;
		}

		public static string HOLONET_TEXTURE_WAR_REBEL_OPEN
		{
			get;
			private set;
		}

		public static string HOLONET_TEXTURE_WAR_REBEL_PREP
		{
			get;
			private set;
		}

		public static string HOLONET_TEXTURE_WAR_REBEL_ACTION
		{
			get;
			private set;
		}

		public static string HOLONET_TEXTURE_WAR_REBEL_COOLDOWN
		{
			get;
			private set;
		}

		public static string HOLONET_TEXTURE_WAR_EMPIRE_OPEN
		{
			get;
			private set;
		}

		public static string HOLONET_TEXTURE_WAR_EMPIRE_PREP
		{
			get;
			private set;
		}

		public static string HOLONET_TEXTURE_WAR_EMPIRE_ACTION
		{
			get;
			private set;
		}

		public static string HOLONET_TEXTURE_WAR_EMPIRE_COOLDOWN
		{
			get;
			private set;
		}

		public static string TFA_PLANET_UID
		{
			get;
			private set;
		}

		public static string HOTH_PLANET_UID
		{
			get;
			private set;
		}

		public static string ERKIT_PLANET_UID
		{
			get;
			private set;
		}

		public static string YAVIN_PLANET_UID
		{
			get;
			private set;
		}

		public static string DANDORAN_PLANET_UID
		{
			get;
			private set;
		}

		public static string TATOOINE_PLANET_UID
		{
			get;
			private set;
		}

		public static int WWW_MAX_RETRY
		{
			get;
			private set;
		}

		public static int PUBLISH_TIMER_DELAY
		{
			get;
			private set;
		}

		public static float PULL_FREQUENCY_CHAT_OPEN
		{
			get;
			private set;
		}

		public static float PULL_FREQUENCY_CHAT_CLOSED
		{
			get;
			private set;
		}

		public static int SQUAD_MESSAGE_LIMIT
		{
			get;
			private set;
		}

		public static int SQUADPERK_MAX_PERK_CARD_BADGES
		{
			get;
			private set;
		}

		public static int CRATE_EXPIRATION_WARNING_NOTIF
		{
			get;
			private set;
		}

		public static int CRATE_EXPIRATION_WARNING_NOTIF_MINIMUM
		{
			get;
			private set;
		}

		public static int CRATE_EXPIRATION_WARNING_TOAST
		{
			get;
			private set;
		}

		public static bool CRATE_DAILY_CRATE_ENABLED
		{
			get;
			private set;
		}

		public static int CRATE_DAILY_CRATE_NOTIF_OFFSET
		{
			get;
			private set;
		}

		public static int CRATE_INVENTORY_TO_STORE_LINK_SORT
		{
			get;
			private set;
		}

		public static string CRATE_INVENTORY_TO_STORE_LINK_CRATE_ASSET
		{
			get;
			private set;
		}

		public static float CRATE_FLYOUT_ITEM_AUTO_SELECT_DURATION
		{
			get;
			private set;
		}

		public static float CRATE_FLYOUT_ITEM_AUTO_SELECT_RESUME
		{
			get;
			private set;
		}

		public static int PLANET_REWARDS_ITEM_THROTTLE
		{
			get;
			private set;
		}

		public static string CRATE_DAY_OF_THE_WEEK_REWARD
		{
			get;
			private set;
		}

		public static int HOLONET_MAX_INCOMING_TRANSMISSIONS
		{
			get;
			private set;
		}

		public static int HOLONET_EVENT_MESSAGE_MAX_COUNT
		{
			get;
			private set;
		}

		public static float HOLONET_FEATURE_CAROUSEL_AUTO_SWIPE
		{
			get;
			private set;
		}

		public static bool HOLONET_FEATURE_SHARE_ENABLED
		{
			get;
			private set;
		}

		public static string IOS_PROMO_END_DATE
		{
			get;
			private set;
		}

		public static bool ENABLE_FACTION_ICON_UPGRADES
		{
			get;
			private set;
		}

		public static int OBJECTIVES_UNLOCKED
		{
			get;
			private set;
		}

		public static float CRATE_OUTLINE_OUTER
		{
			get;
			private set;
		}

		public static float CRATE_OUTLINE_INNER
		{
			get;
			private set;
		}

		public static int WAR_PREP_DURATION
		{
			get;
			private set;
		}

		public static int WAR_ACTION_DURATION
		{
			get;
			private set;
		}

		public static int WAR_ATTACK_COUNT
		{
			get;
			private set;
		}

		public static int WAR_PARTICIPANT_COUNT
		{
			get;
			private set;
		}

		public static int WAR_PARTICIPANT_MIN_LEVEL
		{
			get;
			private set;
		}

		public static int WAR_VICTORY_POINTS
		{
			get;
			private set;
		}

		public static string WARBOARD_LABEL_OFFSET
		{
			get;
			private set;
		}

		public static bool WAR_ALLOW_MATCHMAKING
		{
			get;
			private set;
		}

		public static bool WAR_ALLOW_SAME_FACTION_MATCHMAKING
		{
			get;
			private set;
		}

		private static string WAR_BUFF_BASE_HQ_LEVEL_MAP
		{
			get;
			set;
		}

		public static int[] WAR_BUFF_BASE_HQ_TO_LEVEL_MAPPING
		{
			get;
			private set;
		}

		public static string SQUADPERK_TUTORIAL_ACTIVE_PREF
		{
			get;
			set;
		}

		public static int SQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD
		{
			get;
			set;
		}

		public static int SQUADPERK_DONATION_REPUTATION_AWARD
		{
			get;
			set;
		}

		public static string SQUADPERK_REPUTATION_MAX_LIMIT
		{
			get;
			set;
		}

		public static int NEW_PLAYER_REPUTATION_CAPACITY
		{
			get;
			set;
		}

		public static int CRATE_INVENTORY_EXPIRATION_TIMER_WARNING
		{
			get;
			private set;
		}

		public static int CRATE_INVENTORY_LEI_EXPIRATION_TIMER_WARNING
		{
			get;
			private set;
		}

		public static bool CRATE_SHOW_VFX
		{
			get;
			set;
		}

		public static int EQUIPMENT_SHADER_DELAY
		{
			get;
			set;
		}

		public static int EQUIPMENT_SHADER_DELAY_REPLAY
		{
			get;
			set;
		}

		public static int EQUIPMENT_SHADER_DELAY_DEFENSE
		{
			get;
			set;
		}

		public static bool SAME_FACTION_MATCHMAKING_DEFAULT
		{
			get;
			set;
		}

		public static bool IsMakerVideoEnabled()
		{
			return false;
		}

		public static void Initialize()
		{
			IDataController dataController = Service.Get<IDataController>();
			PropertyInfo[] properties = typeof(GameConstants).GetProperties((global::BindingFlags)51);
			int i = 0;
			int num = properties.Length;
			while (i < num)
			{
				PropertyInfo propertyInfo = properties[i];
				string uid = propertyInfo.Name.ToLower();
				GameConstantsVO optional = dataController.GetOptional<GameConstantsVO>(uid);
				if (optional != null)
				{
					string value = optional.Value;
					propertyInfo.SetValue(null, Convert.ChangeType(value, propertyInfo.PropertyType, CultureInfo.InvariantCulture), null);
				}
				i++;
			}
			dataController.Unload<GameConstantsVO>();
			GameConstants.InitRelocationCrystalsAndStarCost();
			GameConstants.InitMakerVideo();
			GameConstants.InitWarBaseLevelMapping();
		}

		private static void InitWarBaseLevelMapping()
		{
			if (string.IsNullOrEmpty(GameConstants.WAR_BUFF_BASE_HQ_LEVEL_MAP))
			{
				Service.Get<StaRTSLogger>().Error("GameConstants.WAR_BUFF_BASE_HQ_LEVEL_MAP is null or empty");
				return;
			}
			string[] array = GameConstants.WAR_BUFF_BASE_HQ_LEVEL_MAP.Split(new char[]
			{
				','
			});
			int num = array.Length;
			GameConstants.WAR_BUFF_BASE_HQ_TO_LEVEL_MAPPING = new int[num];
			for (int i = 0; i < num; i++)
			{
				GameConstants.WAR_BUFF_BASE_HQ_TO_LEVEL_MAPPING[i] = Convert.ToInt32(array[i], CultureInfo.InvariantCulture);
			}
		}

		private static void InitRelocationCrystalsAndStarCost()
		{
			if (string.IsNullOrEmpty(GameConstants.CRYSTALS_PER_RELOCATION_STAR))
			{
				Service.Get<StaRTSLogger>().Error("GameConstants.CRYSTALS_PER_RELOCATION_STAR is null or empty");
				return;
			}
			string[] array = GameConstants.CRYSTALS_PER_RELOCATION_STAR.Split(new char[]
			{
				','
			});
			string[] array2 = GameConstants.STARS_PER_RELOCATION.Split(new char[]
			{
				','
			});
			int num = array.Length;
			if (num != array2.Length)
			{
				Service.Get<StaRTSLogger>().Error("RelocationCrystalsAndStarCost Invalid Lengths");
				GameConstants.CrystalsPerRelocationStar = new int[0];
				GameConstants.StarsPerRelocation = new int[0];
				return;
			}
			GameConstants.CrystalsPerRelocationStar = new int[num];
			GameConstants.StarsPerRelocation = new int[num];
			for (int i = 0; i < num; i++)
			{
				int num2;
				int num3;
				if (!int.TryParse(array[i], ref num2) || !int.TryParse(array2[i], ref num3))
				{
					Service.Get<StaRTSLogger>().Error("RelocationCrystalsAndStarCost Parse Error");
					GameConstants.CrystalsPerRelocationStar = new int[0];
					GameConstants.StarsPerRelocation = new int[0];
					return;
				}
				GameConstants.CrystalsPerRelocationStar[i] = num2;
				GameConstants.StarsPerRelocation[i] = num3;
			}
		}

		private static void InitMakerVideo()
		{
			if (string.IsNullOrEmpty(GameConstants.MAKER_VIDEO_LOCALES))
			{
				Service.Get<StaRTSLogger>().Error("GameConstants.MAKER_VIDEO_LOCALES is null or empty");
				return;
			}
			GameConstants.MakerVideoLocales = GameConstants.MAKER_VIDEO_LOCALES.Split(new char[]
			{
				','
			});
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ALL_LOCALES);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ALLOY_COEFFICIENT);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ALLOY_EXPONENT);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ASSET_BUNDLE_CACHE_CLEAN_DISABLED);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ASSET_BUNDLE_CACHE_CLEAN_VERSION);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ATTACK_RATING_WEIGHT);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.AUTOSELECT_DISABLE_HQTHRESHOLD);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.BATTLE_END_DELAY);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.BATTLE_WARNING_TIME);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CAMPAIGN_HOURS_CLOSING);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CAMPAIGN_HOURS_UPCOMING);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CAMPAIGN_STORY_FAILURE_DELAY);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CAMPAIGN_STORY_GoalFailure_DELAY);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CAMPAIGN_STORY_INTRO_DELAY);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CAMPAIGN_STORY_SUCCESS_DELAY);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.COEF_EXP_ACCURACY);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CONTRABAND_2_THRESHOLD);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CONTRABAND_3_THRESHOLD);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CONTRABAND_COEFFICIENT);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CONTRABAND_EXPONENT);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CONTRABAND_UNLOCK_LEVEL);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CONTRACT_REFUND_PERCENTAGE_BUILDINGS);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CONTRACT_REFUND_PERCENTAGE_TROOPS);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_DAILY_CRATE_ENABLED);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_DAILY_CRATE_NOTIF_OFFSET);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_DAY_OF_THE_WEEK_REWARD);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_EXPIRATION_WARNING_NOTIF);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_EXPIRATION_WARNING_NOTIF_MINIMUM);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_EXPIRATION_WARNING_TOAST);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_FLYOUT_ITEM_AUTO_SELECT_DURATION);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_FLYOUT_ITEM_AUTO_SELECT_RESUME);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_INVENTORY_EXPIRATION_TIMER_WARNING);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_INVENTORY_LEI_EXPIRATION_TIMER_WARNING);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_INVENTORY_TO_STORE_LINK_CRATE_ASSET);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_INVENTORY_TO_STORE_LINK_SORT);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_OUTLINE_INNER);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_OUTLINE_OUTER);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRATE_SHOW_VFX);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CREDITS_2_THRESHOLD);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CREDITS_3_THRESHOLD);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CREDITS_COEFFICIENT);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CREDITS_EXPONENT);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRYSTAL_PACK_AMOUNT);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRYSTAL_PACK_COST_USD);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRYSTAL_SPEND_WARNING_MINIMUM);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRYSTALS_2_THRESHOLD);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRYSTALS_3_THRESHOLD);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRYSTALS_PER_RELOCATION_STAR);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRYSTALS_SPEED_UP_COEFFICIENT);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CRYSTALS_SPEED_UP_EXPONENT);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.CrystalsPerRelocationStar);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.DANDORAN_PLANET_UID);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.DEFAULT_BATTLE_LENGTH);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.DEFENSE_RATING_WEIGHT);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.DEFLECTION_DURATION_MS);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.DEFLECTION_VELOCITY_PERCENT);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.DISABLE_BASE_LAYOUT_TOOL);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.DROID_CRYSTAL_COSTS);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.EDIT_LONG_PRESS_FADE);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.EDIT_LONG_PRESS_PRE_FADE);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ENABLE_FACTION_ICON_UPGRADES);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ENABLE_INSTANT_BUY);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ENABLE_UPGRADE_ALL_WALLS);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.EQUIPMENT_SHADER_DELAY);
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.EQUIPMENT_SHADER_DELAY_DEFENSE);
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.EQUIPMENT_SHADER_DELAY_REPLAY);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.ERKIT_PLANET_UID);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.EVENT_2_BI_ENABLED);
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.FACEBOOK_INVITES_ENABLED);
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.FACTION_CHOICE_CONFIRM_SCREEN_ENABLED);
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.FACTION_FLIPPING_UNLOCK_LEVEL);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.FADE_OUT_CONSTANT_LENGTH);
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.FB_CONNECT_REWARD);
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.FORUMS_ENABLED);
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.FPS_THRESHOLD);
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.FUE_BATTLE);
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.FUE_QUEST_UID);
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_AUTO_ROTATE_DELAY);
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_AUTO_ROTATE_SPEED);
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_CAMERA_DISTANCE_OFFSET);
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_CAMERA_HEIGHT_OFFSET);
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_EASE_ROTATION_TIME);
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_EASE_ROTATION_TRANSITION_TIME);
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_INITIAL_GALAXY_ZOOM_DIST);
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_INITIAL_GALAXY_ZOOM_TIME);
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_FOREGROUND_PLATEAU_THRESHOLD);
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_FOREGROUND_THRESHOLD);
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_FOREGROUND_UI_THRESHOLD);
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_GALAXY_ZOOM_TIME);
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_POPULATION_COUNT_PERCENTAGE);
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_POPULATION_UPDATE_TIME);
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_SWIPE_MAX_TIME);
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_SWIPE_MIN_MOVE);
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_SWIPE_TIME);
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_PLANET_VIEW_HEIGHT);
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GALAXY_UI_PLANET_FOCUS_THROTTLE);
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.GRIND_MISSION_MAXIMUM);
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_EVENT_MESSAGE_MAX_COUNT);
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_FEATURE_CAROUSEL_AUTO_SWIPE);
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_FEATURE_SHARE_ENABLED);
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_MAKER_VIDEO_URL);
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_MAX_INCOMING_TRANSMISSIONS);
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_ACTION);
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_COOLDOWN);
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_OPEN);
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_PREP);
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_TEXTURE_WAR_REBEL_ACTION);
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_TEXTURE_WAR_REBEL_COOLDOWN);
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_TEXTURE_WAR_REBEL_OPEN);
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOLONET_TEXTURE_WAR_REBEL_PREP);
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HOTH_PLANET_UID);
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HQ_LOOTABLE_CONTRABAND);
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HQ_LOOTABLE_CREDITS);
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HQ_LOOTABLE_MATERIALS);
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HUD_RESOURCE_TICKER_CRYSTAL_THRESHOLD);
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HUD_RESOURCE_TICKER_MAX_DURATION);
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.HUD_RESOURCE_TICKER_MIN_DURATION);
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.IAP_DISABLED_ANDROID);
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.IAP_DISCLAIMER_WHITELIST);
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.IAP_FORCE_POPUP_ENABLED);
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.IDLE_RELOAD_TIME);
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.INVENTORY_INITIAL_CAP);
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.INVENTORY_LIMIT);
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.IOS_PROMO_END_DATE);
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.KEEP_ALIVE_DISPATCH_WAIT_TIME);
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.LOG_STACK_TRACE_TO_BI);
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MAKER_VIDEO_ENABLED);
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MAKER_VIDEO_LOCALES);
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MakerVideoLocales);
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MATERIALS_2_THRESHOLD);
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MATERIALS_3_THRESHOLD);
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MAX_CONCURRENT_ASSET_LOADS);
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MAX_PER_USER_TROOP_DONATION);
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MAX_TROOP_DONATIONS);
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MESH_COMBINE_DISABLED);
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.MIN_MAKER_VID_LOAD);
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_CHAMPION_CAPACITY);
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_CONTRABAND_AMOUNT);
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_CONTRABAND_CAPACITY);
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_CREDITS_AMOUNT);
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_CREDITS_CAPACITY);
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_CRYSTALS_AMOUNT);
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_DROIDS_AMOUNT);
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_DROIDS_CAPACITY);
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_FACTION);
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_HERO_CAPACITY);
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_INITIAL_MISSION_EMPIRE);
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_INITIAL_MISSION_REBEL);
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_MATERIALS_AMOUNT);
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_MATERIALS_CAPACITY);
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_REPUTATION_AMOUNT);
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_REPUTATION_CAPACITY);
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_STARSHIP_CAPACITY);
		}

		public unsafe static long $Invoke153(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_TROOP_CAPACITY);
		}

		public unsafe static long $Invoke154(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NEW_PLAYER_XP_AMOUNT);
		}

		public unsafe static long $Invoke155(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.NO_FB_FACTION_CHOICE_ANDROID);
		}

		public unsafe static long $Invoke156(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.OBJECTIVES_UNLOCKED);
		}

		public unsafe static long $Invoke157(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PAUSED_RELOAD_TIME);
		}

		public unsafe static long $Invoke158(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PERFORMANCE_SAMPLE_DELAY_BATTLE);
		}

		public unsafe static long $Invoke159(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PERFORMANCE_SAMPLE_DELAY_HOME);
		}

		public unsafe static long $Invoke160(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PLANET_RELOCATED_TUTORIAL_ID);
		}

		public unsafe static long $Invoke161(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PLANET_REWARDS_ITEM_THROTTLE);
		}

		public unsafe static long $Invoke162(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PLAYDOM_BI_ENABLED);
		}

		public unsafe static long $Invoke163(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.POST_FUE_EMPIRE_BASE);
		}

		public unsafe static long $Invoke164(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.POST_FUE_REBEL_BASE);
		}

		public unsafe static long $Invoke165(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PROMO_BUTTON_GLOW_DURATION);
		}

		public unsafe static long $Invoke166(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PROMO_BUTTON_RESHOW_GLOW);
		}

		public unsafe static long $Invoke167(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PROMO_UNIT_TEST_ENABLED);
		}

		public unsafe static long $Invoke168(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PROTECTION_CRYSTAL_COSTS);
		}

		public unsafe static long $Invoke169(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PROTECTION_DURATION);
		}

		public unsafe static long $Invoke170(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PUBLISH_TIMER_DELAY);
		}

		public unsafe static long $Invoke171(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PULL_FREQUENCY_CHAT_CLOSED);
		}

		public unsafe static long $Invoke172(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PULL_FREQUENCY_CHAT_OPEN);
		}

		public unsafe static long $Invoke173(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PUSH_NOTIFICATION_CRYSTAL_REWARD_AMOUNT);
		}

		public unsafe static long $Invoke174(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PUSH_NOTIFICATION_ENABLE_INCENTIVE);
		}

		public unsafe static long $Invoke175(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PUSH_NOTIFICATION_MAX_REACHED);
		}

		public unsafe static long $Invoke176(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PUSH_NOTIFICATION_SQUAD_JOIN_COOLDOWN);
		}

		public unsafe static long $Invoke177(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PUSH_NOTIFICATIONS_TROOP_REQUEST_COOLDOWN);
		}

		public unsafe static long $Invoke178(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PVP_LOSE_ON_PAUSE);
		}

		public unsafe static long $Invoke179(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PVP_LOSE_ON_QUIT);
		}

		public unsafe static long $Invoke180(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PVP_MATCH_BONUS_ATTACKER_SLOPE);
		}

		public unsafe static long $Invoke181(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PVP_MATCH_BONUS_ATTACKER_Y_INTERCEPT);
		}

		public unsafe static long $Invoke182(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PVP_MATCH_COUNTDOWN);
		}

		public unsafe static long $Invoke183(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PVP_MATCH_DURATION);
		}

		public unsafe static long $Invoke184(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PVP_SEARCH_COST_BY_HQ_LEVEL);
		}

		public unsafe static long $Invoke185(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.PVP_SEARCH_TIMEOUT_DURATION);
		}

		public unsafe static long $Invoke186(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.QUIET_CORRECTION_ENABLED);
		}

		public unsafe static long $Invoke187(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.QUIET_CORRECTION_WHITELIST);
		}

		public unsafe static long $Invoke188(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RAID_DEFENSE_TRAINER_BINDINGS);
		}

		public unsafe static long $Invoke189(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RAIDS_HQ_UNLOCK_LEVEL);
		}

		public unsafe static long $Invoke190(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RAIDS_UPCOMING_TICKER_THROTTLE);
		}

		public unsafe static long $Invoke191(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RATE_APP_INCENTIVE_CRYSTALS);
		}

		public unsafe static long $Invoke192(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RATE_APP_INCENTIVE_GRANT_ANDROID);
		}

		public unsafe static long $Invoke193(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RATE_APP_INCENTIVE_GRANT_IOS);
		}

		public unsafe static long $Invoke194(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RATE_APP_INCENTIVE_GRANT_WINDOWS);
		}

		public unsafe static long $Invoke195(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RATE_APP_INCENTIVE_SHOW_ANDROID);
		}

		public unsafe static long $Invoke196(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RATE_APP_INCENTIVE_SHOW_IOS);
		}

		public unsafe static long $Invoke197(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RATE_APP_INCENTIVE_SHOW_WINDOWS);
		}

		public unsafe static long $Invoke198(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RATE_MY_APP_ENABLED);
		}

		public unsafe static long $Invoke199(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.REFUND_SURVIVORS);
		}

		public unsafe static long $Invoke200(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.RESPAWN_TIME_NATURAL_RESOURCE);
		}

		public unsafe static long $Invoke201(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SAME_FACTION_MATCHMAKING_DEFAULT);
		}

		public unsafe static long $Invoke202(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SECONDS_PER_CRYSTAL);
		}

		public unsafe static long $Invoke203(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SET_CALLSIGN_CONFIRM_SCREEN_ENABLED);
		}

		public unsafe static long $Invoke204(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SHIELD_HEALTH_PER_POINT);
		}

		public unsafe static long $Invoke205(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SHIELD_RANGE_PER_POINT);
		}

		public unsafe static long $Invoke206(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SPAWN_HEALTH_PERCENT);
		}

		public unsafe static long $Invoke207(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_CREATE_COST);
		}

		public unsafe static long $Invoke208(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_CREATE_MAX_TROPHY_REQ);
		}

		public unsafe static long $Invoke209(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_CREATE_MIN_TROPHY_REQ);
		}

		public unsafe static long $Invoke210(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_INVITES_ENABLED);
		}

		public unsafe static long $Invoke211(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_INVITES_TO_LEADERS_ENABLED);
		}

		public unsafe static long $Invoke212(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_MEMBER_LIMIT);
		}

		public unsafe static long $Invoke213(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_MESSAGE_LIMIT);
		}

		public unsafe static long $Invoke214(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_NAME_LENGTH_MAX);
		}

		public unsafe static long $Invoke215(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_NAME_LENGTH_MIN);
		}

		public unsafe static long $Invoke216(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_TROOP_DEPLOY_FLAG_EMPIRE_ASSET);
		}

		public unsafe static long $Invoke217(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUAD_TROOP_DEPLOY_FLAG_REBEL_ASSET);
		}

		public unsafe static long $Invoke218(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUADPERK_CRYSTALS_SPEED_UP_COEFFICIENT);
		}

		public unsafe static long $Invoke219(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUADPERK_CRYSTALS_SPEED_UP_EXPONENT);
		}

		public unsafe static long $Invoke220(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD);
		}

		public unsafe static long $Invoke221(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD);
		}

		public unsafe static long $Invoke222(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUADPERK_MAX_PERK_CARD_BADGES);
		}

		public unsafe static long $Invoke223(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUADPERK_MAX_SQUAD_LEVEL_CELEBRATIONS_SHOWN);
		}

		public unsafe static long $Invoke224(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUADPERK_REPUTATION_MAX_LIMIT);
		}

		public unsafe static long $Invoke225(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.SQUADPERK_TUTORIAL_ACTIVE_PREF);
		}

		public unsafe static long $Invoke226(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.STARS_PER_RELOCATION);
		}

		public unsafe static long $Invoke227(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.StarsPerRelocation);
		}

		public unsafe static long $Invoke228(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.START_FUE_IN_BATTLE_MODE);
		}

		public unsafe static long $Invoke229(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TAPJOY_AFTER_IAP);
		}

		public unsafe static long $Invoke230(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TAPJOY_BLACKLIST);
		}

		public unsafe static long $Invoke231(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TAPJOY_ENABLED);
		}

		public unsafe static long $Invoke232(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TARGETED_OFFERS_ENABLED);
		}

		public unsafe static long $Invoke233(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TARGETED_OFFERS_FREQUENCY_LIMIT);
		}

		public unsafe static long $Invoke234(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TATOOINE_PLANET_UID);
		}

		public unsafe static long $Invoke235(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TFA_PLANET_UID);
		}

		public unsafe static long $Invoke236(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TIME_OF_DAY_ENABLED);
		}

		public unsafe static long $Invoke237(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TOURNAMENT_HOURS_CLOSING);
		}

		public unsafe static long $Invoke238(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TOURNAMENT_HOURS_SHOW_BADGE);
		}

		public unsafe static long $Invoke239(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TOURNAMENT_HOURS_UPCOMING);
		}

		public unsafe static long $Invoke240(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TOURNAMENT_RATING_DELTAS_ATTACKER);
		}

		public unsafe static long $Invoke241(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.TURRET_SWAP_HQ_UNLOCK);
		}

		public unsafe static long $Invoke242(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.UNDER_ATTACK_STATUS_CHECK_INTERVAL);
		}

		public unsafe static long $Invoke243(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.UPGRADE_ALL_WALL_EXPONENT);
		}

		public unsafe static long $Invoke244(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.UPGRADE_ALL_WALLS_COEFFICIENT);
		}

		public unsafe static long $Invoke245(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.UPGRADE_ALL_WALLS_CONVENIENCE_TAX);
		}

		public unsafe static long $Invoke246(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.USER_NAME_MAX_CHARACTERS);
		}

		public unsafe static long $Invoke247(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.USER_NAME_MIN_CHARACTERS);
		}

		public unsafe static long $Invoke248(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.VO_BLACKLIST);
		}

		public unsafe static long $Invoke249(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_ACTION_DURATION);
		}

		public unsafe static long $Invoke250(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_ALLOW_MATCHMAKING);
		}

		public unsafe static long $Invoke251(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_ALLOW_SAME_FACTION_MATCHMAKING);
		}

		public unsafe static long $Invoke252(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_ATTACK_COUNT);
		}

		public unsafe static long $Invoke253(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_BUFF_BASE_HQ_LEVEL_MAP);
		}

		public unsafe static long $Invoke254(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_BUFF_BASE_HQ_TO_LEVEL_MAPPING);
		}

		public unsafe static long $Invoke255(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_BASEEDIT_EMPIRE);
		}

		public unsafe static long $Invoke256(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_BASEEDIT_REBEL);
		}

		public unsafe static long $Invoke257(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_OVERVIEW_EMPIRE);
		}

		public unsafe static long $Invoke258(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_OVERVIEW_REBEL);
		}

		public unsafe static long $Invoke259(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_PREPARATION_EMPIRE);
		}

		public unsafe static long $Invoke260(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_PREPARATION_REBEL);
		}

		public unsafe static long $Invoke261(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_REWARD_EMPIRE);
		}

		public unsafe static long $Invoke262(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_REWARD_REBEL);
		}

		public unsafe static long $Invoke263(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_WAR_EMPIRE);
		}

		public unsafe static long $Invoke264(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_HELP_WAR_REBEL);
		}

		public unsafe static long $Invoke265(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_MAX_BUFF_BASES);
		}

		public unsafe static long $Invoke266(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_NOTIF_ACTION_TURNS_REMINDER);
		}

		public unsafe static long $Invoke267(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_PARTICIPANT_COUNT);
		}

		public unsafe static long $Invoke268(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_PARTICIPANT_MIN_LEVEL);
		}

		public unsafe static long $Invoke269(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_PREP_DURATION);
		}

		public unsafe static long $Invoke270(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WAR_VICTORY_POINTS);
		}

		public unsafe static long $Invoke271(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WARBOARD_LABEL_OFFSET);
		}

		public unsafe static long $Invoke272(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.WWW_MAX_RETRY);
		}

		public unsafe static long $Invoke273(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.YAVIN_PLANET_UID);
		}

		public unsafe static long $Invoke274(long instance, long* args)
		{
			GameConstants.Initialize();
			return -1L;
		}

		public unsafe static long $Invoke275(long instance, long* args)
		{
			GameConstants.InitMakerVideo();
			return -1L;
		}

		public unsafe static long $Invoke276(long instance, long* args)
		{
			GameConstants.InitRelocationCrystalsAndStarCost();
			return -1L;
		}

		public unsafe static long $Invoke277(long instance, long* args)
		{
			GameConstants.InitWarBaseLevelMapping();
			return -1L;
		}

		public unsafe static long $Invoke278(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstants.IsMakerVideoEnabled());
		}

		public unsafe static long $Invoke279(long instance, long* args)
		{
			GameConstants.ALL_LOCALES = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke280(long instance, long* args)
		{
			GameConstants.ALLOY_COEFFICIENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke281(long instance, long* args)
		{
			GameConstants.ALLOY_EXPONENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke282(long instance, long* args)
		{
			GameConstants.ASSET_BUNDLE_CACHE_CLEAN_DISABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke283(long instance, long* args)
		{
			GameConstants.ASSET_BUNDLE_CACHE_CLEAN_VERSION = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke284(long instance, long* args)
		{
			GameConstants.ATTACK_RATING_WEIGHT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke285(long instance, long* args)
		{
			GameConstants.AUTOSELECT_DISABLE_HQTHRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke286(long instance, long* args)
		{
			GameConstants.BATTLE_END_DELAY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke287(long instance, long* args)
		{
			GameConstants.BATTLE_WARNING_TIME = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke288(long instance, long* args)
		{
			GameConstants.CAMPAIGN_HOURS_CLOSING = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke289(long instance, long* args)
		{
			GameConstants.CAMPAIGN_HOURS_UPCOMING = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke290(long instance, long* args)
		{
			GameConstants.CAMPAIGN_STORY_FAILURE_DELAY = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke291(long instance, long* args)
		{
			GameConstants.CAMPAIGN_STORY_GoalFailure_DELAY = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke292(long instance, long* args)
		{
			GameConstants.CAMPAIGN_STORY_INTRO_DELAY = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke293(long instance, long* args)
		{
			GameConstants.CAMPAIGN_STORY_SUCCESS_DELAY = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke294(long instance, long* args)
		{
			GameConstants.COEF_EXP_ACCURACY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke295(long instance, long* args)
		{
			GameConstants.CONTRABAND_2_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke296(long instance, long* args)
		{
			GameConstants.CONTRABAND_3_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke297(long instance, long* args)
		{
			GameConstants.CONTRABAND_COEFFICIENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke298(long instance, long* args)
		{
			GameConstants.CONTRABAND_EXPONENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke299(long instance, long* args)
		{
			GameConstants.CONTRABAND_UNLOCK_LEVEL = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke300(long instance, long* args)
		{
			GameConstants.CONTRACT_REFUND_PERCENTAGE_BUILDINGS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke301(long instance, long* args)
		{
			GameConstants.CONTRACT_REFUND_PERCENTAGE_TROOPS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke302(long instance, long* args)
		{
			GameConstants.CRATE_DAILY_CRATE_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke303(long instance, long* args)
		{
			GameConstants.CRATE_DAILY_CRATE_NOTIF_OFFSET = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke304(long instance, long* args)
		{
			GameConstants.CRATE_DAY_OF_THE_WEEK_REWARD = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke305(long instance, long* args)
		{
			GameConstants.CRATE_EXPIRATION_WARNING_NOTIF = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke306(long instance, long* args)
		{
			GameConstants.CRATE_EXPIRATION_WARNING_NOTIF_MINIMUM = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke307(long instance, long* args)
		{
			GameConstants.CRATE_EXPIRATION_WARNING_TOAST = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke308(long instance, long* args)
		{
			GameConstants.CRATE_FLYOUT_ITEM_AUTO_SELECT_DURATION = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke309(long instance, long* args)
		{
			GameConstants.CRATE_FLYOUT_ITEM_AUTO_SELECT_RESUME = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke310(long instance, long* args)
		{
			GameConstants.CRATE_INVENTORY_EXPIRATION_TIMER_WARNING = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke311(long instance, long* args)
		{
			GameConstants.CRATE_INVENTORY_LEI_EXPIRATION_TIMER_WARNING = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke312(long instance, long* args)
		{
			GameConstants.CRATE_INVENTORY_TO_STORE_LINK_CRATE_ASSET = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke313(long instance, long* args)
		{
			GameConstants.CRATE_INVENTORY_TO_STORE_LINK_SORT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke314(long instance, long* args)
		{
			GameConstants.CRATE_OUTLINE_INNER = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke315(long instance, long* args)
		{
			GameConstants.CRATE_OUTLINE_OUTER = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke316(long instance, long* args)
		{
			GameConstants.CRATE_SHOW_VFX = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke317(long instance, long* args)
		{
			GameConstants.CREDITS_2_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke318(long instance, long* args)
		{
			GameConstants.CREDITS_3_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke319(long instance, long* args)
		{
			GameConstants.CREDITS_COEFFICIENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke320(long instance, long* args)
		{
			GameConstants.CREDITS_EXPONENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke321(long instance, long* args)
		{
			GameConstants.CRYSTAL_PACK_AMOUNT = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke322(long instance, long* args)
		{
			GameConstants.CRYSTAL_PACK_COST_USD = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke323(long instance, long* args)
		{
			GameConstants.CRYSTAL_SPEND_WARNING_MINIMUM = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke324(long instance, long* args)
		{
			GameConstants.CRYSTALS_2_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke325(long instance, long* args)
		{
			GameConstants.CRYSTALS_3_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke326(long instance, long* args)
		{
			GameConstants.CRYSTALS_PER_RELOCATION_STAR = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke327(long instance, long* args)
		{
			GameConstants.CRYSTALS_SPEED_UP_COEFFICIENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke328(long instance, long* args)
		{
			GameConstants.CRYSTALS_SPEED_UP_EXPONENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke329(long instance, long* args)
		{
			GameConstants.CrystalsPerRelocationStar = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke330(long instance, long* args)
		{
			GameConstants.DANDORAN_PLANET_UID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke331(long instance, long* args)
		{
			GameConstants.DEFAULT_BATTLE_LENGTH = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke332(long instance, long* args)
		{
			GameConstants.DEFENSE_RATING_WEIGHT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke333(long instance, long* args)
		{
			GameConstants.DEFLECTION_DURATION_MS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke334(long instance, long* args)
		{
			GameConstants.DEFLECTION_VELOCITY_PERCENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke335(long instance, long* args)
		{
			GameConstants.DISABLE_BASE_LAYOUT_TOOL = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke336(long instance, long* args)
		{
			GameConstants.DROID_CRYSTAL_COSTS = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke337(long instance, long* args)
		{
			GameConstants.EDIT_LONG_PRESS_FADE = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke338(long instance, long* args)
		{
			GameConstants.EDIT_LONG_PRESS_PRE_FADE = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke339(long instance, long* args)
		{
			GameConstants.ENABLE_FACTION_ICON_UPGRADES = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke340(long instance, long* args)
		{
			GameConstants.ENABLE_INSTANT_BUY = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke341(long instance, long* args)
		{
			GameConstants.ENABLE_UPGRADE_ALL_WALLS = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke342(long instance, long* args)
		{
			GameConstants.EQUIPMENT_SHADER_DELAY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke343(long instance, long* args)
		{
			GameConstants.EQUIPMENT_SHADER_DELAY_DEFENSE = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke344(long instance, long* args)
		{
			GameConstants.EQUIPMENT_SHADER_DELAY_REPLAY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke345(long instance, long* args)
		{
			GameConstants.ERKIT_PLANET_UID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke346(long instance, long* args)
		{
			GameConstants.EVENT_2_BI_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke347(long instance, long* args)
		{
			GameConstants.FACEBOOK_INVITES_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke348(long instance, long* args)
		{
			GameConstants.FACTION_CHOICE_CONFIRM_SCREEN_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke349(long instance, long* args)
		{
			GameConstants.FACTION_FLIPPING_UNLOCK_LEVEL = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke350(long instance, long* args)
		{
			GameConstants.FADE_OUT_CONSTANT_LENGTH = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke351(long instance, long* args)
		{
			GameConstants.FB_CONNECT_REWARD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke352(long instance, long* args)
		{
			GameConstants.FORUMS_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke353(long instance, long* args)
		{
			GameConstants.FPS_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke354(long instance, long* args)
		{
			GameConstants.FUE_BATTLE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke355(long instance, long* args)
		{
			GameConstants.FUE_QUEST_UID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke356(long instance, long* args)
		{
			GameConstants.GALAXY_AUTO_ROTATE_DELAY = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke357(long instance, long* args)
		{
			GameConstants.GALAXY_AUTO_ROTATE_SPEED = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke358(long instance, long* args)
		{
			GameConstants.GALAXY_CAMERA_DISTANCE_OFFSET = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke359(long instance, long* args)
		{
			GameConstants.GALAXY_CAMERA_HEIGHT_OFFSET = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke360(long instance, long* args)
		{
			GameConstants.GALAXY_EASE_ROTATION_TIME = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke361(long instance, long* args)
		{
			GameConstants.GALAXY_EASE_ROTATION_TRANSITION_TIME = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke362(long instance, long* args)
		{
			GameConstants.GALAXY_INITIAL_GALAXY_ZOOM_DIST = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke363(long instance, long* args)
		{
			GameConstants.GALAXY_INITIAL_GALAXY_ZOOM_TIME = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke364(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_FOREGROUND_PLATEAU_THRESHOLD = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke365(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_FOREGROUND_THRESHOLD = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke366(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_FOREGROUND_UI_THRESHOLD = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke367(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_GALAXY_ZOOM_TIME = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke368(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_POPULATION_COUNT_PERCENTAGE = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke369(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_POPULATION_UPDATE_TIME = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke370(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_SWIPE_MAX_TIME = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke371(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_SWIPE_MIN_MOVE = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke372(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_SWIPE_TIME = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke373(long instance, long* args)
		{
			GameConstants.GALAXY_PLANET_VIEW_HEIGHT = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke374(long instance, long* args)
		{
			GameConstants.GALAXY_UI_PLANET_FOCUS_THROTTLE = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke375(long instance, long* args)
		{
			GameConstants.GRIND_MISSION_MAXIMUM = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke376(long instance, long* args)
		{
			GameConstants.HOLONET_EVENT_MESSAGE_MAX_COUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke377(long instance, long* args)
		{
			GameConstants.HOLONET_FEATURE_CAROUSEL_AUTO_SWIPE = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke378(long instance, long* args)
		{
			GameConstants.HOLONET_FEATURE_SHARE_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke379(long instance, long* args)
		{
			GameConstants.HOLONET_MAKER_VIDEO_URL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke380(long instance, long* args)
		{
			GameConstants.HOLONET_MAX_INCOMING_TRANSMISSIONS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke381(long instance, long* args)
		{
			GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_ACTION = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke382(long instance, long* args)
		{
			GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_COOLDOWN = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke383(long instance, long* args)
		{
			GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_OPEN = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke384(long instance, long* args)
		{
			GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_PREP = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke385(long instance, long* args)
		{
			GameConstants.HOLONET_TEXTURE_WAR_REBEL_ACTION = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke386(long instance, long* args)
		{
			GameConstants.HOLONET_TEXTURE_WAR_REBEL_COOLDOWN = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke387(long instance, long* args)
		{
			GameConstants.HOLONET_TEXTURE_WAR_REBEL_OPEN = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke388(long instance, long* args)
		{
			GameConstants.HOLONET_TEXTURE_WAR_REBEL_PREP = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke389(long instance, long* args)
		{
			GameConstants.HOTH_PLANET_UID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke390(long instance, long* args)
		{
			GameConstants.HQ_LOOTABLE_CONTRABAND = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke391(long instance, long* args)
		{
			GameConstants.HQ_LOOTABLE_CREDITS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke392(long instance, long* args)
		{
			GameConstants.HQ_LOOTABLE_MATERIALS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke393(long instance, long* args)
		{
			GameConstants.HUD_RESOURCE_TICKER_CRYSTAL_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke394(long instance, long* args)
		{
			GameConstants.HUD_RESOURCE_TICKER_MAX_DURATION = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke395(long instance, long* args)
		{
			GameConstants.HUD_RESOURCE_TICKER_MIN_DURATION = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke396(long instance, long* args)
		{
			GameConstants.IAP_DISABLED_ANDROID = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke397(long instance, long* args)
		{
			GameConstants.IAP_DISCLAIMER_WHITELIST = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke398(long instance, long* args)
		{
			GameConstants.IAP_FORCE_POPUP_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke399(long instance, long* args)
		{
			GameConstants.IDLE_RELOAD_TIME = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke400(long instance, long* args)
		{
			GameConstants.INVENTORY_INITIAL_CAP = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke401(long instance, long* args)
		{
			GameConstants.INVENTORY_LIMIT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke402(long instance, long* args)
		{
			GameConstants.IOS_PROMO_END_DATE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke403(long instance, long* args)
		{
			GameConstants.KEEP_ALIVE_DISPATCH_WAIT_TIME = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke404(long instance, long* args)
		{
			GameConstants.LOG_STACK_TRACE_TO_BI = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke405(long instance, long* args)
		{
			GameConstants.MAKER_VIDEO_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke406(long instance, long* args)
		{
			GameConstants.MAKER_VIDEO_LOCALES = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke407(long instance, long* args)
		{
			GameConstants.MakerVideoLocales = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke408(long instance, long* args)
		{
			GameConstants.MATERIALS_2_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke409(long instance, long* args)
		{
			GameConstants.MATERIALS_3_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke410(long instance, long* args)
		{
			GameConstants.MAX_CONCURRENT_ASSET_LOADS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke411(long instance, long* args)
		{
			GameConstants.MAX_PER_USER_TROOP_DONATION = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke412(long instance, long* args)
		{
			GameConstants.MAX_TROOP_DONATIONS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke413(long instance, long* args)
		{
			GameConstants.MESH_COMBINE_DISABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke414(long instance, long* args)
		{
			GameConstants.MIN_MAKER_VID_LOAD = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke415(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_CHAMPION_CAPACITY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke416(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_CONTRABAND_AMOUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke417(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_CONTRABAND_CAPACITY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke418(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_CREDITS_AMOUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke419(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_CREDITS_CAPACITY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke420(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_CRYSTALS_AMOUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke421(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_DROIDS_AMOUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke422(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_DROIDS_CAPACITY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke423(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_FACTION = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke424(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_HERO_CAPACITY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke425(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_INITIAL_MISSION_EMPIRE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke426(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_INITIAL_MISSION_REBEL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke427(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_MATERIALS_AMOUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke428(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_MATERIALS_CAPACITY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke429(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_REPUTATION_AMOUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke430(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_REPUTATION_CAPACITY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke431(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_STARSHIP_CAPACITY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke432(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_TROOP_CAPACITY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke433(long instance, long* args)
		{
			GameConstants.NEW_PLAYER_XP_AMOUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke434(long instance, long* args)
		{
			GameConstants.NO_FB_FACTION_CHOICE_ANDROID = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke435(long instance, long* args)
		{
			GameConstants.OBJECTIVES_UNLOCKED = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke436(long instance, long* args)
		{
			GameConstants.PAUSED_RELOAD_TIME = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke437(long instance, long* args)
		{
			GameConstants.PERFORMANCE_SAMPLE_DELAY_BATTLE = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke438(long instance, long* args)
		{
			GameConstants.PERFORMANCE_SAMPLE_DELAY_HOME = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke439(long instance, long* args)
		{
			GameConstants.PLANET_RELOCATED_TUTORIAL_ID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke440(long instance, long* args)
		{
			GameConstants.PLANET_REWARDS_ITEM_THROTTLE = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke441(long instance, long* args)
		{
			GameConstants.PLAYDOM_BI_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke442(long instance, long* args)
		{
			GameConstants.POST_FUE_EMPIRE_BASE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke443(long instance, long* args)
		{
			GameConstants.POST_FUE_REBEL_BASE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke444(long instance, long* args)
		{
			GameConstants.PROMO_BUTTON_GLOW_DURATION = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke445(long instance, long* args)
		{
			GameConstants.PROMO_BUTTON_RESHOW_GLOW = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke446(long instance, long* args)
		{
			GameConstants.PROMO_UNIT_TEST_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke447(long instance, long* args)
		{
			GameConstants.PROTECTION_CRYSTAL_COSTS = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke448(long instance, long* args)
		{
			GameConstants.PROTECTION_DURATION = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke449(long instance, long* args)
		{
			GameConstants.PUBLISH_TIMER_DELAY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke450(long instance, long* args)
		{
			GameConstants.PULL_FREQUENCY_CHAT_CLOSED = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke451(long instance, long* args)
		{
			GameConstants.PULL_FREQUENCY_CHAT_OPEN = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke452(long instance, long* args)
		{
			GameConstants.PUSH_NOTIFICATION_CRYSTAL_REWARD_AMOUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke453(long instance, long* args)
		{
			GameConstants.PUSH_NOTIFICATION_ENABLE_INCENTIVE = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke454(long instance, long* args)
		{
			GameConstants.PUSH_NOTIFICATION_MAX_REACHED = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke455(long instance, long* args)
		{
			GameConstants.PUSH_NOTIFICATION_SQUAD_JOIN_COOLDOWN = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke456(long instance, long* args)
		{
			GameConstants.PUSH_NOTIFICATIONS_TROOP_REQUEST_COOLDOWN = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke457(long instance, long* args)
		{
			GameConstants.PVP_LOSE_ON_PAUSE = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke458(long instance, long* args)
		{
			GameConstants.PVP_LOSE_ON_QUIT = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke459(long instance, long* args)
		{
			GameConstants.PVP_MATCH_BONUS_ATTACKER_SLOPE = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke460(long instance, long* args)
		{
			GameConstants.PVP_MATCH_BONUS_ATTACKER_Y_INTERCEPT = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke461(long instance, long* args)
		{
			GameConstants.PVP_MATCH_COUNTDOWN = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke462(long instance, long* args)
		{
			GameConstants.PVP_MATCH_DURATION = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke463(long instance, long* args)
		{
			GameConstants.PVP_SEARCH_COST_BY_HQ_LEVEL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke464(long instance, long* args)
		{
			GameConstants.PVP_SEARCH_TIMEOUT_DURATION = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke465(long instance, long* args)
		{
			GameConstants.QUIET_CORRECTION_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke466(long instance, long* args)
		{
			GameConstants.QUIET_CORRECTION_WHITELIST = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke467(long instance, long* args)
		{
			GameConstants.RAID_DEFENSE_TRAINER_BINDINGS = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke468(long instance, long* args)
		{
			GameConstants.RAIDS_HQ_UNLOCK_LEVEL = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke469(long instance, long* args)
		{
			GameConstants.RAIDS_UPCOMING_TICKER_THROTTLE = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke470(long instance, long* args)
		{
			GameConstants.RATE_APP_INCENTIVE_CRYSTALS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke471(long instance, long* args)
		{
			GameConstants.RATE_APP_INCENTIVE_GRANT_ANDROID = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke472(long instance, long* args)
		{
			GameConstants.RATE_APP_INCENTIVE_GRANT_IOS = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke473(long instance, long* args)
		{
			GameConstants.RATE_APP_INCENTIVE_GRANT_WINDOWS = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke474(long instance, long* args)
		{
			GameConstants.RATE_APP_INCENTIVE_SHOW_ANDROID = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke475(long instance, long* args)
		{
			GameConstants.RATE_APP_INCENTIVE_SHOW_IOS = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke476(long instance, long* args)
		{
			GameConstants.RATE_APP_INCENTIVE_SHOW_WINDOWS = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke477(long instance, long* args)
		{
			GameConstants.RATE_MY_APP_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke478(long instance, long* args)
		{
			GameConstants.REFUND_SURVIVORS = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke479(long instance, long* args)
		{
			GameConstants.RESPAWN_TIME_NATURAL_RESOURCE = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke480(long instance, long* args)
		{
			GameConstants.SAME_FACTION_MATCHMAKING_DEFAULT = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke481(long instance, long* args)
		{
			GameConstants.SECONDS_PER_CRYSTAL = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke482(long instance, long* args)
		{
			GameConstants.SET_CALLSIGN_CONFIRM_SCREEN_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke483(long instance, long* args)
		{
			GameConstants.SHIELD_HEALTH_PER_POINT = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke484(long instance, long* args)
		{
			GameConstants.SHIELD_RANGE_PER_POINT = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke485(long instance, long* args)
		{
			GameConstants.SPAWN_HEALTH_PERCENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke486(long instance, long* args)
		{
			GameConstants.SQUAD_CREATE_COST = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke487(long instance, long* args)
		{
			GameConstants.SQUAD_CREATE_MAX_TROPHY_REQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke488(long instance, long* args)
		{
			GameConstants.SQUAD_CREATE_MIN_TROPHY_REQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke489(long instance, long* args)
		{
			GameConstants.SQUAD_INVITES_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke490(long instance, long* args)
		{
			GameConstants.SQUAD_INVITES_TO_LEADERS_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke491(long instance, long* args)
		{
			GameConstants.SQUAD_MEMBER_LIMIT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke492(long instance, long* args)
		{
			GameConstants.SQUAD_MESSAGE_LIMIT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke493(long instance, long* args)
		{
			GameConstants.SQUAD_NAME_LENGTH_MAX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke494(long instance, long* args)
		{
			GameConstants.SQUAD_NAME_LENGTH_MIN = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke495(long instance, long* args)
		{
			GameConstants.SQUAD_TROOP_DEPLOY_FLAG_EMPIRE_ASSET = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke496(long instance, long* args)
		{
			GameConstants.SQUAD_TROOP_DEPLOY_FLAG_REBEL_ASSET = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke497(long instance, long* args)
		{
			GameConstants.SQUADPERK_CRYSTALS_SPEED_UP_COEFFICIENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke498(long instance, long* args)
		{
			GameConstants.SQUADPERK_CRYSTALS_SPEED_UP_EXPONENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke499(long instance, long* args)
		{
			GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke500(long instance, long* args)
		{
			GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke501(long instance, long* args)
		{
			GameConstants.SQUADPERK_MAX_PERK_CARD_BADGES = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke502(long instance, long* args)
		{
			GameConstants.SQUADPERK_MAX_SQUAD_LEVEL_CELEBRATIONS_SHOWN = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke503(long instance, long* args)
		{
			GameConstants.SQUADPERK_REPUTATION_MAX_LIMIT = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke504(long instance, long* args)
		{
			GameConstants.SQUADPERK_TUTORIAL_ACTIVE_PREF = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke505(long instance, long* args)
		{
			GameConstants.STARS_PER_RELOCATION = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke506(long instance, long* args)
		{
			GameConstants.StarsPerRelocation = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke507(long instance, long* args)
		{
			GameConstants.START_FUE_IN_BATTLE_MODE = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke508(long instance, long* args)
		{
			GameConstants.TAPJOY_AFTER_IAP = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke509(long instance, long* args)
		{
			GameConstants.TAPJOY_BLACKLIST = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke510(long instance, long* args)
		{
			GameConstants.TAPJOY_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke511(long instance, long* args)
		{
			GameConstants.TARGETED_OFFERS_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke512(long instance, long* args)
		{
			GameConstants.TARGETED_OFFERS_FREQUENCY_LIMIT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke513(long instance, long* args)
		{
			GameConstants.TATOOINE_PLANET_UID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke514(long instance, long* args)
		{
			GameConstants.TFA_PLANET_UID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke515(long instance, long* args)
		{
			GameConstants.TIME_OF_DAY_ENABLED = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke516(long instance, long* args)
		{
			GameConstants.TOD_MID_DAY_PERCENTAGE = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke517(long instance, long* args)
		{
			GameConstants.TOURNAMENT_HOURS_CLOSING = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke518(long instance, long* args)
		{
			GameConstants.TOURNAMENT_HOURS_SHOW_BADGE = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke519(long instance, long* args)
		{
			GameConstants.TOURNAMENT_HOURS_UPCOMING = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke520(long instance, long* args)
		{
			GameConstants.TOURNAMENT_RATING_DELTAS_ATTACKER = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke521(long instance, long* args)
		{
			GameConstants.TURRET_SWAP_HQ_UNLOCK = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke522(long instance, long* args)
		{
			GameConstants.UNDER_ATTACK_STATUS_CHECK_INTERVAL = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke523(long instance, long* args)
		{
			GameConstants.UPGRADE_ALL_WALL_EXPONENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke524(long instance, long* args)
		{
			GameConstants.UPGRADE_ALL_WALLS_COEFFICIENT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke525(long instance, long* args)
		{
			GameConstants.UPGRADE_ALL_WALLS_CONVENIENCE_TAX = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke526(long instance, long* args)
		{
			GameConstants.USER_NAME_MAX_CHARACTERS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke527(long instance, long* args)
		{
			GameConstants.USER_NAME_MIN_CHARACTERS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke528(long instance, long* args)
		{
			GameConstants.VO_BLACKLIST = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke529(long instance, long* args)
		{
			GameConstants.WAR_ACTION_DURATION = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke530(long instance, long* args)
		{
			GameConstants.WAR_ALLOW_MATCHMAKING = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke531(long instance, long* args)
		{
			GameConstants.WAR_ALLOW_SAME_FACTION_MATCHMAKING = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke532(long instance, long* args)
		{
			GameConstants.WAR_ATTACK_COUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke533(long instance, long* args)
		{
			GameConstants.WAR_BUFF_BASE_HQ_LEVEL_MAP = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke534(long instance, long* args)
		{
			GameConstants.WAR_BUFF_BASE_HQ_TO_LEVEL_MAPPING = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke535(long instance, long* args)
		{
			GameConstants.WAR_HELP_BASEEDIT_EMPIRE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke536(long instance, long* args)
		{
			GameConstants.WAR_HELP_BASEEDIT_REBEL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke537(long instance, long* args)
		{
			GameConstants.WAR_HELP_OVERVIEW_EMPIRE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke538(long instance, long* args)
		{
			GameConstants.WAR_HELP_OVERVIEW_REBEL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke539(long instance, long* args)
		{
			GameConstants.WAR_HELP_PREPARATION_EMPIRE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke540(long instance, long* args)
		{
			GameConstants.WAR_HELP_PREPARATION_REBEL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke541(long instance, long* args)
		{
			GameConstants.WAR_HELP_REWARD_EMPIRE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke542(long instance, long* args)
		{
			GameConstants.WAR_HELP_REWARD_REBEL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke543(long instance, long* args)
		{
			GameConstants.WAR_HELP_WAR_EMPIRE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke544(long instance, long* args)
		{
			GameConstants.WAR_HELP_WAR_REBEL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke545(long instance, long* args)
		{
			GameConstants.WAR_MAX_BUFF_BASES = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke546(long instance, long* args)
		{
			GameConstants.WAR_NOTIF_ACTION_TURNS_REMINDER = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke547(long instance, long* args)
		{
			GameConstants.WAR_PARTICIPANT_COUNT = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke548(long instance, long* args)
		{
			GameConstants.WAR_PARTICIPANT_MIN_LEVEL = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke549(long instance, long* args)
		{
			GameConstants.WAR_PREP_DURATION = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke550(long instance, long* args)
		{
			GameConstants.WAR_VICTORY_POINTS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke551(long instance, long* args)
		{
			GameConstants.WARBOARD_LABEL_OFFSET = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke552(long instance, long* args)
		{
			GameConstants.WWW_MAX_RETRY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke553(long instance, long* args)
		{
			GameConstants.YAVIN_PLANET_UID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
