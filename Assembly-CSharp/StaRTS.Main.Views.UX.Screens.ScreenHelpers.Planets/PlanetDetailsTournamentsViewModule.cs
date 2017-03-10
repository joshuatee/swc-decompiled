using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Tournament;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.Leaderboard;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets
{
	public class PlanetDetailsTournamentsViewModule : AbstractPlanetDetailsViewModule, IEventObserver, IViewClockTimeObserver
	{
		private const float DELAY = 0.05f;

		private const string CONFLICT_GROUP = "TournamentSelection";

		private const string ANIM_SHOW_IN_PROGRESS = "ShowInProgress";

		private const string ANIM_SHOW_NEXT = "ShowNext";

		private const string ANIM_SHOW_COMING_SOON = "ShowComingSoon";

		private const string ANIM_SHOW_ENDED = "ShowEnded";

		private const string ANIM_NO_INFO = "NoConflict";

		private const string CONFLICT_COMING_SOON_GROUP = "ComingSoon";

		private const string CONFLICT_COMING_SOON_DESCRIPTION_LABEL = "LabelComingSoonDescription";

		private const string CONFLICT_COMING_SOON_EVENT_TITLE_LABEL = "LabelComingSoonTitle";

		private const string CONFLICT_COMING_SOON_EVENT_TIMER_LABEL = "LabelComingSoonExpiration";

		private const string CONFLICT_COMING_SOON_VIEW_TIERS_BUTTON = "BtnComingSoonViewLeagues";

		private const string CONFLICT_COMING_SOON_VIEW_TIERS_LABEL = "LabelComingSoonViewLeagues";

		private const string CONFLICT_TOP_TIER_LABEL = "LabelTopLeagueComingSoon";

		private const string CONFLICT_TOP_TIER_ICON = "SpriteTopLeagueComingSoonIcon";

		private const string CONFLICT_TOP_TIER_NAME = "LabelTopLeagueComingSoonLevel";

		private const string CONFLICT_TOP_TIER_PERCENT = "LabelTopLeagueComingSoonPercent";

		private const string CONFLICT_COMING_SOON_REWARD_TITLE_LABEL = "LabelTopPrizeComingSoon";

		private const string CONFLICT_COMING_SOON_REWARD_LABEL = "LabelPrizeNameComingSoon";

		private const string CONFLICT_COMING_SOON_REWARD_SPRITE = "SpriteTopLeagueComingSoonItemImage";

		private const string CONFLICT_COMING_SOON_REWARD_CRATE_SPRITE = "SpriteTopLeagueComingSoonCrate";

		private const string CONFLICT_COMING_SOON_REWARD_ITEM_CARD = "TopLeagueComingSoonItemCardQ{0}";

		private const string CONFLICT_COMING_SOON_REWARD_DATA_FRAG_ICON_SPRITE = "SpriteTopPrizeDataFragIcon";

		private const string CONFLICT_COMING_SOON_REWARD_ITEM_COUNT = "LabelTopPrizeAmt";

		private const string CONFLICT_COMING_SOON_CRATE_COUNT = "LabelTopPrizeCrateCount";

		private const string CONFLICT_COMING_SOON_OUTLINE = "SpriteComingSoonOutline";

		private const string CONFLICT_COMING_SOON_OUTLINE_BG_GLOW = "SpriteComingSoonOutlineBgGlow";

		private const string CONFLICT_COMING_SOON_DIVIDER_LEFT = "SpriteComingSoonDividerLeft";

		private const string CONFLICT_COMING_SOON_DIVIDER_CENTER = "SpriteComingSoonDividerCenter";

		private const string CONFLICT_IN_PROGRESS_GROUP = "InProgress";

		private const string CONFLICT_IN_PROGRESS_EVENT_TIMER_LABEL = "LabelInProgressEventTimer";

		private const string CONFLICT_IN_PROGRESS_VIEW_TIERS_LABEL = "LabelInProgressViewLeagues";

		private const string CONFLICT_IN_PROGRESS_VIEW_TIERS_BUTTON = "BtnInProgressViewLeagues";

		private const string CONFLICT_IN_PROGRESS_CURRENT_TIER_TOP_TITLE_LABEL = "LabelCurrentLeagueTitle";

		private const string CONFLICT_IN_PROGRESS_CURRENT_TIER_TITLE_LABEL = "LabelCurrentLeagueLevel";

		private const string CONFLICT_IN_PROGRESS_CURRENT_TIER_PERCENT_LABEL = "LabelCurrentLeaguePercent";

		private const string CONFLICT_IN_PROGRESS_CURRENT_TIER_ICON_SPRITE = "SpriteCurrentLeagueIcon";

		private const string CONFLICT_IN_PROGRESS_CURRENT_TIER_CRATE_COUNT = "LabelCurrentLeagueCrateCount";

		private const string CONFLICT_IN_PROGRESS_CURRENT_REWARD_LABEL = "LabelCurrentLeagueCount";

		private const string CONFLICT_IN_PROGRESS_CURRENT_REWARD_SPRITE = "SpriteCurrentLeagueItemImage";

		private const string CONFLICT_IN_PROGRESS_CURRENT_REWARD_CRATE_SPRITE = "SpriteCurrentLeagueCrate";

		private const string CONFLICT_IN_PROGRESS_CURRENT_REWARD_ITEM_CARD = "CurrentLeagueItemCardQ{0}";

		private const string CONFLICT_IN_PROGRESS_CURRENT_REWARD_DATA_FRAG_ICON_SPRITE = "SpriteCurrentLeagueDataFragIcon";

		private const string CONFLICT_IN_PROGRESS_NEXT_TIER_TOP_TITLE_LABEL = "LabelNextLeagueTitle";

		private const string CONFLICT_IN_PROGRESS_NEXT_TIER_TITLE_LABEL = "LabelNextLeagueLevel";

		private const string CONFLICT_IN_PROGRESS_NEXT_TIER_PERCENT_LABEL = "LabelNextLeaguePercent";

		private const string CONFLICT_IN_PROGRESS_NEXT_TIER_ICON_SPRITE = "SpriteNextLeagueIcon";

		private const string CONFLICT_IN_PROGRESS_NEXT_TIER_CRATE_COUNT = "LabelNextLeagueCrateCount";

		private const string CONFLICT_IN_PROGRESS_NEXT_REWARD_LABEL = "LabelNextLeagueCount";

		private const string CONFLICT_IN_PROGRESS_NEXT_REWARD_SPRITE = "SpriteNextLeagueItemImage";

		private const string CONFLICT_IN_PROGRESS_NEXT_REWARD_CRATE_SPRITE = "SpriteNextLeagueCrate";

		private const string CONFLICT_IN_PROGRESS_NEXT_REWARD_ITEM_CARD = "NextLeagueItemCardQ{0}";

		private const string CONFLICT_IN_PROGRESS_NEXT_REWARD_DATA_FRAG_ICON_SPRITE = "SpriteNextLeagueDataFragIcon";

		private const string CONFLICT_IN_PROGRESS_TOP_TIER_TOP_TITLE_LABEL = "LabelTopLeagueTitle";

		private const string CONFLICT_IN_PROGRESS_TOP_TIER_TITLE_LABEL = "LabelTopLeagueLevel";

		private const string CONFLICT_IN_PROGRESS_TOP_TIER_PERCENT_LABEL = "LabelTopLeaguePercent";

		private const string CONFLICT_IN_PROGRESS_TOP_TIER_ICON_SPRITE = "SpriteTopLeagueIcon";

		private const string CONFLICT_IN_PROGRESS_TOP_TIER_CRATE_COUNT = "LabelTopLeagueCrateCount";

		private const string CONFLICT_IN_PROGRESS_TOP_REWARD_LABEL = "LabelTopLeagueCount";

		private const string CONFLICT_IN_PROGRESS_TOP_REWARD_SPRITE = "SpriteTopLeagueItemImage";

		private const string CONFLICT_IN_PROGRESS_TOP_REWARD_CRATE_SPRITE = "SpriteTopLeagueCrate";

		private const string CONFLICT_IN_PROGRESS_TOP_REWARD_ITEM_CARD = "TopLeagueItemCardQ{0}";

		private const string CONFLICT_IN_PROGRESS_TOP_REWARD_DATA_FRAG_ICON_SPRITE = "SpriteTopLeagueDataFragIcon";

		private const string CONFLICT_IN_PROGRESS_BG = "SpriteTournamentInProgressBg";

		private const string CONFLICT_IN_PROGRESS_DIVIDER = "SpriteInProgressDivider";

		private const string CONFLICT_ENDED_GROUP = "Ended";

		private const string CONFLICT_ENDED_VIEW_TIERS_LABEL = "LabelEndedViewTiers";

		private const string CONFLICT_ENDED_VIEW_TIERS_BUTTON = "BtnEndedViewTiers";

		private const string CONFLICT_ENDED_LEADERBOARDS_LABEL = "LabelBtnEndedLeaderboards";

		private const string CONFLICT_ENDED_LEADERBOARDS_BUTTON = "BtnEndedLeaderboards";

		private const string CONFLICT_ENDED_TIER_TITLE_LABEL = "LabelEndedTierTitle";

		private const string CONFLICT_ENDED_TIER_LABEL = "LabelEndedTier";

		private const string CONFLICT_ENDED_PERCENT_LABEL = "LabelPercentTier";

		private const string CONFLICT_ENDED_TIER_ICON_SPRITE = "SpriteEndedTierIcon";

		private const string CAMPAIGN_BEGINS_IN_STRING = "CAMPAIGN_BEGINS_IN";

		private const string CAMPAIGN_ENDS_IN_STRING = "CAMPAIGN_ENDS_IN";

		private const string CONFLICT_TIER_LOADING_STRING = "CONFLICT_TIER_LOADING";

		private const string CONFLICT_TIER_PERCENTILE_STRING = "CONFLICT_TIER_PERCENTILE";

		private const string VIEW_TIERS_STRING = "s_ViewTiers";

		private const string VIEW_LEADERS_STRING = "CONFLICT_VIEW_LEADERS";

		private const string EVENT_INFO_GROUP = "EventInfo";

		private const string CONFLICT_TOP_LEAGUE = "CONFLICT_TOP_LEAGUE";

		private const string CONFLICT_YOUR_LEAGUE = "CONFLICT_YOUR_LEAGUE";

		private const string CONFLICT_TOP_REWARD = "CONFLICT_TOP_REWARD";

		private const string CONFLICT_IN_PROGRESS_TIER_REWARD = "CONFLICT_IN_PROGRESS_TIER_REWARD";

		private const string CONFLICT_DESC_PRE = "CONFLICT_DESC_PRE";

		private const string CONFLICT_DESC_PRE_LOCKED = "CONFLICT_DESC_PRE_LOCKED";

		private const string CONFLICT_DESC_PRE_RELOCATE = "CONFLICT_DESC_PRE_RELOCATE";

		private const string CONFLICT_DESC_NOT_PLAYED = "CONFLICT_DESC_NOT_PLAYED";

		private const string CONFLICT_DESC_NOT_PLAYED_LOCKED = "CONFLICT_DESC_NOT_PLAYED_LOCKED";

		private const string CONFLICT_DESC_NOT_PLAYED_RELOCATE = "CONFLICT_DESC_NOT_PLAYED_RELOCATE";

		private const string CONFLICT_NEXT_LEAGUE = "CONFLICT_NEXT_LEAGUE";

		private const string CONFLICT_LEAGUE_AND_DIVISION = "CONFLICT_LEAGUE_AND_DIVISION";

		private const string LOADING_TEXT = "s_Loading";

		private const string CONFLICT_PROMOFLAG = "CONFLICT_PROMOFLAG";

		private const float ANIM_DELAY = 0.1f;

		private UXElement tournamentGroup;

		private UXElement comingSoonGroup;

		private UXLabel comingSoonEventTimerLabel;

		private UXLabel comingSoonEventTitleLabel;

		private UXLabel comingSoonDescriptionLabel;

		private UXButton comingSoonTiersButton;

		private UXLabel comingSoonTiersButtonLabel;

		private UXLabel comingSoonTopTierTitle;

		private UXSprite comingSoonTopTierIcon;

		private UXLabel comingSoonTopTierName;

		private UXLabel comingSoonTopTierPercent;

		private UXLabel comingSoonTopRewardTitleLabel;

		private UXLabel comingSoonTopRewardLabel;

		private UXSprite comingSoonTopRewardSprite;

		private UXSprite comingSoonTopRewardCrateSprite;

		private UXElement comingSoonTopRewardItemCardBasic;

		private UXElement comingSoonTopRewardItemCardAdvanced;

		private UXElement comingSoonTopRewardItemCardElite;

		private UXSprite comingSoonBg;

		private UXSprite comingSoonBgGlow;

		private UXSprite comingSoonDividerLeft;

		private UXSprite comingSoonDividerCenter;

		private UXSprite comingSoonTopRewardDataFragIcon;

		private UXLabel comingSoonCrateCount;

		private UXLabel comingSoonTopRewardIconLabel;

		private UXElement inProgressGroup;

		private UXButton inProgressTiersButton;

		private UXLabel inProgressTiersButtonLabel;

		private UXLabel inProgressEventTimerLabel;

		private UXLabel inProgressCurrentTierLabel;

		private UXLabel inProgressCurrentTierPercentLabel;

		private UXSprite inProgressCurrentTierIconSprite;

		private UXLabel inProgressCurrentRewardLabel;

		private UXSprite inProgressCurrentRewardSprite;

		private UXSprite inProgressCurrentRewardCrateSprite;

		private UXElement inProgressCurrentRewardItemCardBasic;

		private UXElement inProgressCurrentRewardItemCardAdvanced;

		private UXElement inProgressCurrentRewardItemCardElite;

		private UXSprite inProgressCurrentRewardDataFragSprite;

		private UXLabel currentCrateCount;

		private UXLabel inProgressNextTierLabel;

		private UXLabel inProgressNextTierPercentLabel;

		private UXSprite inProgressNextTierIconSprite;

		private UXLabel inProgressNextRewardLabel;

		private UXSprite inProgressNextRewardSprite;

		private UXSprite inProgressNextRewardCrateSprite;

		private UXElement inProgressNextRewardItemCardBasic;

		private UXElement inProgressNextRewardItemCardAdvanced;

		private UXElement inProgressNextRewardItemCardElite;

		private UXSprite inProgressNextRewardDataFragSprite;

		private UXLabel nextCrateCount;

		private UXLabel inProgressTopTierLabel;

		private UXLabel inProgressTopTierPercentLabel;

		private UXSprite inProgressTopTierIconSprite;

		private UXLabel inProgressTopRewardLabel;

		private UXSprite inProgressTopRewardSprite;

		private UXSprite inProgressTopRewardCrateSprite;

		private UXElement inProgressTopRewardItemCardBasic;

		private UXElement inProgressTopRewardItemCardAdvanced;

		private UXElement inProgressTopRewardItemCardElite;

		private UXSprite inProgressTopRewardDataFragSprite;

		private UXLabel topCrateCount;

		private UXElement endedGroup;

		private UXLabel endedTierTitleLabel;

		private UXLabel endedTierLabel;

		private UXLabel endedTierPercentLabel;

		private UXLabel endedViewTiersButtonLabel;

		private UXButton endedViewTiersButton;

		private UXLabel endedLeaderboardButtonLabel;

		private UXButton endedLeaderboardButton;

		private UXSprite endedTierIconSprite;

		private TournamentVO currentTournamentVO;

		private TimedEventState currentEventState;

		private bool observingClockTime;

		private bool isLoaded;

		private bool isAnimatingShowNext;

		private TournamentController tournamentController;

		private Color planetUpcomingTextColor;

		private Color planetUpcomingTextGlowColor;

		private Color planetActiveTextColor;

		private Color planetActiveTextGlowColor;

		public PlanetDetailsTournamentsViewModule(PlanetDetailsScreen screen)
		{
			this.currentEventState = TimedEventState.Hidden;
			base..ctor(screen);
			base.EvtManager.RegisterObserver(this, EventId.TournamentRedeemed, EventPriority.Default);
			base.EvtManager.RegisterObserver(this, EventId.GalaxyPlanetInfoButton, EventPriority.Default);
			base.EvtManager.RegisterObserver(this, EventId.ScreenClosing, EventPriority.Default);
			this.tournamentController = Service.Get<TournamentController>();
			this.isLoaded = false;
			this.planetUpcomingTextColor = new Color(1f, 0.753f, 0f);
			this.planetUpcomingTextGlowColor = new Color(0.471f, 0.353f, 0f);
			this.planetActiveTextColor = new Color(0.733f, 0.075f, 0.075f);
			this.planetActiveTextGlowColor = new Color(0.733f, 0.075f, 0.075f, 0.588f);
		}

		public void OnScreenLoaded()
		{
			this.eventInfoGroup = this.screen.GetElement<UXElement>("EventInfo");
			this.tournamentGroup = this.screen.GetElement<UXElement>("TournamentSelection");
			this.comingSoonGroup = this.screen.GetElement<UXElement>("ComingSoon");
			this.comingSoonEventTitleLabel = this.screen.GetElement<UXLabel>("LabelComingSoonTitle");
			this.comingSoonEventTimerLabel = this.screen.GetElement<UXLabel>("LabelComingSoonExpiration");
			this.comingSoonDescriptionLabel = this.screen.GetElement<UXLabel>("LabelComingSoonDescription");
			this.comingSoonTiersButton = this.screen.GetElement<UXButton>("BtnComingSoonViewLeagues");
			this.comingSoonTiersButton.OnClicked = new UXButtonClickedDelegate(this.OnTournamentTiersButtonClicked);
			this.comingSoonTiersButtonLabel = this.screen.GetElement<UXLabel>("LabelComingSoonViewLeagues");
			this.comingSoonTiersButtonLabel.Text = base.LangController.Get("s_ViewTiers", new object[0]);
			this.comingSoonTopTierTitle = this.screen.GetElement<UXLabel>("LabelTopLeagueComingSoon");
			this.comingSoonTopTierIcon = this.screen.GetElement<UXSprite>("SpriteTopLeagueComingSoonIcon");
			this.comingSoonTopTierName = this.screen.GetElement<UXLabel>("LabelTopLeagueComingSoonLevel");
			this.comingSoonTopTierPercent = this.screen.GetElement<UXLabel>("LabelTopLeagueComingSoonPercent");
			this.comingSoonTopRewardTitleLabel = this.screen.GetElement<UXLabel>("LabelTopPrizeComingSoon");
			this.comingSoonTopRewardLabel = this.screen.GetElement<UXLabel>("LabelPrizeNameComingSoon");
			this.comingSoonTopRewardSprite = this.screen.GetElement<UXSprite>("SpriteTopLeagueComingSoonItemImage");
			this.comingSoonTopRewardCrateSprite = this.screen.GetElement<UXSprite>("SpriteTopLeagueComingSoonCrate");
			this.comingSoonTopRewardItemCardBasic = this.screen.GetElement<UXElement>(string.Format("TopLeagueComingSoonItemCardQ{0}", new object[]
			{
				1
			}));
			this.comingSoonTopRewardItemCardAdvanced = this.screen.GetElement<UXElement>(string.Format("TopLeagueComingSoonItemCardQ{0}", new object[]
			{
				2
			}));
			this.comingSoonTopRewardItemCardElite = this.screen.GetElement<UXElement>(string.Format("TopLeagueComingSoonItemCardQ{0}", new object[]
			{
				3
			}));
			this.comingSoonBg = this.screen.GetElement<UXSprite>("SpriteComingSoonOutline");
			this.comingSoonBgGlow = this.screen.GetElement<UXSprite>("SpriteComingSoonOutlineBgGlow");
			this.comingSoonDividerLeft = this.screen.GetElement<UXSprite>("SpriteComingSoonDividerLeft");
			this.comingSoonDividerCenter = this.screen.GetElement<UXSprite>("SpriteComingSoonDividerCenter");
			this.comingSoonTopRewardDataFragIcon = this.screen.GetElement<UXSprite>("SpriteTopPrizeDataFragIcon");
			this.comingSoonCrateCount = this.screen.GetElement<UXLabel>("LabelTopPrizeCrateCount");
			this.comingSoonTopRewardIconLabel = this.screen.GetElement<UXLabel>("LabelTopPrizeAmt");
			this.inProgressGroup = this.screen.GetElement<UXElement>("InProgress");
			this.inProgressEventTimerLabel = this.screen.GetElement<UXLabel>("LabelInProgressEventTimer");
			this.inProgressTiersButtonLabel = this.screen.GetElement<UXLabel>("LabelInProgressViewLeagues");
			this.inProgressTiersButtonLabel.Text = base.LangController.Get("s_ViewTiers", new object[0]);
			this.inProgressTiersButton = this.screen.GetElement<UXButton>("BtnInProgressViewLeagues");
			this.inProgressTiersButton.OnClicked = new UXButtonClickedDelegate(this.OnTournamentTiersButtonClicked);
			this.screen.GetElement<UXLabel>("LabelCurrentLeagueTitle").Text = base.LangController.Get("CONFLICT_YOUR_LEAGUE", new object[0]);
			this.screen.GetElement<UXLabel>("LabelNextLeagueTitle").Text = base.LangController.Get("CONFLICT_NEXT_LEAGUE", new object[0]);
			this.screen.GetElement<UXLabel>("LabelTopLeagueTitle").Text = base.LangController.Get("CONFLICT_TOP_LEAGUE", new object[0]);
			this.currentCrateCount = this.screen.GetElement<UXLabel>("LabelCurrentLeagueCrateCount");
			this.nextCrateCount = this.screen.GetElement<UXLabel>("LabelNextLeagueCrateCount");
			this.topCrateCount = this.screen.GetElement<UXLabel>("LabelTopLeagueCrateCount");
			this.inProgressCurrentTierLabel = this.screen.GetElement<UXLabel>("LabelCurrentLeagueLevel");
			this.inProgressCurrentTierPercentLabel = this.screen.GetElement<UXLabel>("LabelCurrentLeaguePercent");
			this.inProgressCurrentTierIconSprite = this.screen.GetElement<UXSprite>("SpriteCurrentLeagueIcon");
			this.inProgressCurrentRewardLabel = this.screen.GetElement<UXLabel>("LabelCurrentLeagueCount");
			this.inProgressCurrentRewardSprite = this.screen.GetElement<UXSprite>("SpriteCurrentLeagueItemImage");
			this.inProgressCurrentRewardCrateSprite = this.screen.GetElement<UXSprite>("SpriteCurrentLeagueCrate");
			this.inProgressCurrentRewardItemCardBasic = this.screen.GetElement<UXElement>(string.Format("CurrentLeagueItemCardQ{0}", new object[]
			{
				1
			}));
			this.inProgressCurrentRewardItemCardAdvanced = this.screen.GetElement<UXElement>(string.Format("CurrentLeagueItemCardQ{0}", new object[]
			{
				2
			}));
			this.inProgressCurrentRewardItemCardElite = this.screen.GetElement<UXElement>(string.Format("CurrentLeagueItemCardQ{0}", new object[]
			{
				3
			}));
			this.inProgressCurrentRewardDataFragSprite = this.screen.GetElement<UXSprite>("SpriteCurrentLeagueDataFragIcon");
			this.inProgressNextTierLabel = this.screen.GetElement<UXLabel>("LabelNextLeagueLevel");
			this.inProgressNextTierPercentLabel = this.screen.GetElement<UXLabel>("LabelNextLeaguePercent");
			this.inProgressNextTierIconSprite = this.screen.GetElement<UXSprite>("SpriteNextLeagueIcon");
			this.inProgressNextRewardLabel = this.screen.GetElement<UXLabel>("LabelNextLeagueCount");
			this.inProgressNextRewardSprite = this.screen.GetElement<UXSprite>("SpriteNextLeagueItemImage");
			this.inProgressNextRewardCrateSprite = this.screen.GetElement<UXSprite>("SpriteNextLeagueCrate");
			this.inProgressNextRewardItemCardBasic = this.screen.GetElement<UXElement>(string.Format("NextLeagueItemCardQ{0}", new object[]
			{
				1
			}));
			this.inProgressNextRewardItemCardAdvanced = this.screen.GetElement<UXElement>(string.Format("NextLeagueItemCardQ{0}", new object[]
			{
				2
			}));
			this.inProgressNextRewardItemCardElite = this.screen.GetElement<UXElement>(string.Format("NextLeagueItemCardQ{0}", new object[]
			{
				3
			}));
			this.inProgressNextRewardDataFragSprite = this.screen.GetElement<UXSprite>("SpriteNextLeagueDataFragIcon");
			this.inProgressTopTierLabel = this.screen.GetElement<UXLabel>("LabelTopLeagueLevel");
			this.inProgressTopTierPercentLabel = this.screen.GetElement<UXLabel>("LabelTopLeaguePercent");
			this.inProgressTopTierIconSprite = this.screen.GetElement<UXSprite>("SpriteTopLeagueIcon");
			this.inProgressTopRewardLabel = this.screen.GetElement<UXLabel>("LabelTopLeagueCount");
			this.inProgressTopRewardSprite = this.screen.GetElement<UXSprite>("SpriteTopLeagueItemImage");
			this.inProgressTopRewardCrateSprite = this.screen.GetElement<UXSprite>("SpriteTopLeagueCrate");
			this.inProgressTopRewardItemCardBasic = this.screen.GetElement<UXElement>(string.Format("TopLeagueItemCardQ{0}", new object[]
			{
				1
			}));
			this.inProgressTopRewardItemCardAdvanced = this.screen.GetElement<UXElement>(string.Format("TopLeagueItemCardQ{0}", new object[]
			{
				2
			}));
			this.inProgressTopRewardItemCardElite = this.screen.GetElement<UXElement>(string.Format("TopLeagueItemCardQ{0}", new object[]
			{
				3
			}));
			this.inProgressTopRewardDataFragSprite = this.screen.GetElement<UXSprite>("SpriteTopLeagueDataFragIcon");
			this.endedGroup = this.screen.GetElement<UXElement>("Ended");
			this.endedTierTitleLabel = this.screen.GetElement<UXLabel>("LabelEndedTierTitle");
			this.endedTierLabel = this.screen.GetElement<UXLabel>("LabelEndedTier");
			this.endedTierPercentLabel = this.screen.GetElement<UXLabel>("LabelPercentTier");
			this.endedViewTiersButtonLabel = this.screen.GetElement<UXLabel>("LabelEndedViewTiers");
			this.endedViewTiersButtonLabel.Text = base.LangController.Get("s_ViewTiers", new object[0]);
			this.endedViewTiersButton = this.screen.GetElement<UXButton>("BtnEndedViewTiers");
			this.endedViewTiersButton.OnClicked = new UXButtonClickedDelegate(this.OnTournamentTiersButtonClicked);
			this.endedTierIconSprite = this.screen.GetElement<UXSprite>("SpriteEndedTierIcon");
			this.endedLeaderboardButtonLabel = this.screen.GetElement<UXLabel>("LabelBtnEndedLeaderboards");
			this.endedLeaderboardButtonLabel.Text = base.LangController.Get("CONFLICT_VIEW_LEADERS", new object[0]);
			this.endedLeaderboardButton = this.screen.GetElement<UXButton>("BtnEndedLeaderboards");
			this.endedLeaderboardButton.OnClicked = new UXButtonClickedDelegate(this.OnLeaderboardButtonClicked);
			this.isLoaded = true;
		}

		public void RefreshScreenForPlanetChange()
		{
		}

		public void OnAnimateShowUI()
		{
			if (this.isAnimatingShowNext)
			{
				this.AnimateShowNext();
			}
		}

		private void UpdateCurrentTournamentTier()
		{
			TournamentRank tournamentFinalRank = base.Player.TournamentProgress.GetTournamentFinalRank(this.currentTournamentVO.Uid);
			if (tournamentFinalRank != null && !string.IsNullOrEmpty(tournamentFinalRank.TierUid))
			{
				this.OnTournamentRankUpdated(null, tournamentFinalRank, this.currentTournamentVO.Uid);
				return;
			}
			this.endedTierLabel.Text = (this.inProgressCurrentTierLabel.Text = (this.inProgressNextTierLabel.Text = (this.inProgressTopTierLabel.Text = base.LangController.Get("CONFLICT_TIER_LOADING", new object[0]))));
			this.inProgressCurrentRewardLabel.Text = (this.inProgressNextRewardLabel.Text = (this.inProgressTopRewardLabel.Text = base.LangController.Get("s_Loading", new object[0])));
			this.inProgressNextTierIconSprite.SpriteName = (this.inProgressCurrentTierIconSprite.SpriteName = (this.inProgressTopTierIconSprite.SpriteName = (this.inProgressCurrentRewardSprite.SpriteName = (this.inProgressNextRewardSprite.SpriteName = (this.inProgressTopRewardSprite.SpriteName = (this.endedTierIconSprite.SpriteName = string.Empty))))));
			this.endedTierPercentLabel.Text = (this.inProgressCurrentTierPercentLabel.Text = (this.inProgressNextTierPercentLabel.Text = (this.inProgressTopTierPercentLabel.Text = string.Empty)));
			this.inProgressCurrentRewardItemCardBasic.Visible = false;
			this.inProgressCurrentRewardItemCardAdvanced.Visible = false;
			this.inProgressCurrentRewardItemCardElite.Visible = false;
			this.inProgressNextRewardItemCardBasic.Visible = false;
			this.inProgressNextRewardItemCardAdvanced.Visible = false;
			this.inProgressNextRewardItemCardElite.Visible = false;
			this.inProgressTopRewardItemCardBasic.Visible = false;
			this.inProgressTopRewardItemCardAdvanced.Visible = false;
			this.inProgressTopRewardItemCardElite.Visible = false;
			this.tournamentController.UpdatePlayerRank(new TournamentController.PlayerRankUpdatedCallback(this.OnTournamentRankUpdated), this.currentTournamentVO);
		}

		private void OnTournamentRankUpdated(TournamentRank oldRank, TournamentRank rank, string tournamentUID)
		{
			if (!this.isLoaded)
			{
				return;
			}
			if (this.currentEventState == TimedEventState.Closing)
			{
				this.endedTierTitleLabel.Text = base.LangController.Get("CONFLICT_YOUR_LEAGUE", new object[0]);
				this.UpdateEndedUI();
				return;
			}
			if (this.currentTournamentVO == null || this.currentTournamentVO.Uid != tournamentUID)
			{
				return;
			}
			TournamentTierVO optional = base.Sdc.GetOptional<TournamentTierVO>(rank.TierUid);
			TournamentTierVO vOForNextTier = TournamentController.GetVOForNextTier(optional);
			TournamentTierVO idForTopTier = TournamentController.GetIdForTopTier();
			Dictionary<string, TournamentRewardsVO> tierRewardMap = TimedEventPrizeUtils.GetTierRewardMap(this.currentTournamentVO.RewardGroupId);
			TournamentRewardsVO rewardGroup = null;
			TournamentRewardsVO rewardGroup2 = null;
			TournamentRewardsVO rewardGroup3 = null;
			if (optional != null && vOForNextTier != null && idForTopTier != null && tierRewardMap.TryGetValue(optional.Uid, out rewardGroup) && tierRewardMap.TryGetValue(vOForNextTier.Uid, out rewardGroup2) && tierRewardMap.TryGetValue(idForTopTier.Uid, out rewardGroup3))
			{
				this.UpdateTierInfoUI(optional, optional.Percentage, this.inProgressCurrentTierIconSprite, this.inProgressCurrentTierLabel, this.inProgressCurrentTierPercentLabel);
				this.inProgressCurrentTierPercentLabel.Text = base.LangController.Get("CONFLICT_TIER_PERCENTILE", new object[]
				{
					Math.Round(rank.Percentile, 2)
				});
				this.UpdateTierInfoUI(vOForNextTier, vOForNextTier.Percentage, this.inProgressNextTierIconSprite, this.inProgressNextTierLabel, this.inProgressNextTierPercentLabel);
				this.UpdateTierInfoUI(idForTopTier, idForTopTier.Percentage, this.inProgressTopTierIconSprite, this.inProgressTopTierLabel, this.inProgressTopTierPercentLabel);
				TimedEventPrizeUtils.TrySetupConflictItemRewardView(rewardGroup, this.inProgressCurrentRewardLabel, this.inProgressCurrentRewardSprite, this.inProgressCurrentRewardCrateSprite, this.inProgressCurrentRewardItemCardBasic, this.inProgressCurrentRewardItemCardAdvanced, this.inProgressCurrentRewardItemCardElite, this.currentCrateCount, this.inProgressCurrentRewardDataFragSprite, null);
				TimedEventPrizeUtils.TrySetupConflictItemRewardView(rewardGroup2, this.inProgressNextRewardLabel, this.inProgressNextRewardSprite, this.inProgressNextRewardCrateSprite, this.inProgressNextRewardItemCardBasic, this.inProgressNextRewardItemCardAdvanced, this.inProgressNextRewardItemCardElite, this.nextCrateCount, this.inProgressNextRewardDataFragSprite, null);
				TimedEventPrizeUtils.TrySetupConflictItemRewardView(rewardGroup3, this.inProgressTopRewardLabel, this.inProgressTopRewardSprite, this.inProgressTopRewardCrateSprite, this.inProgressTopRewardItemCardBasic, this.inProgressTopRewardItemCardAdvanced, this.inProgressTopRewardItemCardElite, this.topCrateCount, this.inProgressTopRewardDataFragSprite, null);
				this.AnimateShowNext();
				return;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("Unable to load InProgress UI, invalid tiers meta data for tournamentUID: " + tournamentUID, new object[0]);
		}

		public void OnViewClockTime(float dt)
		{
			if (!this.isLoaded)
			{
				return;
			}
			TimedEventState state = TimedEventUtils.GetState(this.currentTournamentVO);
			if (state != this.currentEventState)
			{
				this.RefreshView();
			}
			if (this.currentTournamentVO != null)
			{
				this.UpdateTimeRemaining();
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.ScreenClosing)
			{
				if (id != EventId.GalaxyPlanetInfoButton)
				{
					if (id == EventId.TournamentRedeemed)
					{
						if (this.currentTournamentVO != null)
						{
							TimedEventState state = TimedEventUtils.GetState(this.currentTournamentVO);
							if (state == TimedEventState.Closing)
							{
								this.UpdateEndedUI();
							}
						}
					}
				}
				else if (this.eventInfoGroup.Visible)
				{
					this.UpdatePlanetViewDetailWithAnimation();
				}
			}
			else
			{
				if (cookie is LeaderboardsScreen)
				{
					this.screen.AnimateShowUI();
				}
				if ((cookie is TournamentTiersScreen || cookie is LeaderboardsScreen) && this.eventInfoGroup.Visible)
				{
					this.UpdatePlanetViewDetailWithAnimation();
				}
			}
			return EatResponse.NotEaten;
		}

		private void StartObservingTime()
		{
			if (!this.observingClockTime)
			{
				this.UpdateTimeRemaining();
				this.observingClockTime = true;
				Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
			}
		}

		private void UpdateTimeRemaining()
		{
			if (this.currentTournamentVO == null)
			{
				return;
			}
			int secondsRemaining = TimedEventUtils.GetSecondsRemaining(this.currentTournamentVO);
			string text = LangUtils.FormatTime((long)secondsRemaining);
			TimedEventState timedEventState = this.currentEventState;
			if (timedEventState == TimedEventState.Upcoming)
			{
				this.comingSoonEventTimerLabel.Text = base.LangController.Get("CAMPAIGN_BEGINS_IN", new object[]
				{
					text
				});
				return;
			}
			if (timedEventState != TimedEventState.Live)
			{
				return;
			}
			this.inProgressEventTimerLabel.Text = base.LangController.Get("CAMPAIGN_ENDS_IN", new object[]
			{
				text
			});
			this.comingSoonEventTimerLabel.Text = base.LangController.Get("CAMPAIGN_ENDS_IN", new object[]
			{
				text
			});
		}

		public void RefreshView()
		{
			this.currentTournamentVO = TournamentController.GetActiveTournamentOnPlanet(this.screen.viewingPlanetVO.Uid);
			if (this.currentTournamentVO != null)
			{
				this.comingSoonEventTitleLabel.Text = LangUtils.GetTournamentTitle(this.currentTournamentVO);
				this.currentEventState = TimedEventUtils.GetState(this.currentTournamentVO);
				this.screen.UpdatePvpPanel(this.currentEventState == TimedEventState.Live, this.currentTournamentVO);
				bool isPlayerInTournament = this.tournamentController.IsPlayerInTournament(this.currentTournamentVO);
				this.ShowTimedEventDetails(isPlayerInTournament);
			}
			else
			{
				this.observingClockTime = false;
				Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
				this.currentEventState = TimedEventState.Invalid;
				this.screen.UpdatePvpPanel(false, null);
			}
			TimedEventState timedEventState = this.currentEventState;
			if (timedEventState != TimedEventState.Upcoming)
			{
				if (timedEventState == TimedEventState.Live)
				{
					this.inProgressEventTimerLabel.TextColor = this.planetActiveTextColor;
					this.comingSoonEventTimerLabel.TextColor = this.planetActiveTextColor;
					this.comingSoonBg.Color = this.planetActiveTextColor;
					this.comingSoonBgGlow.Color = this.planetActiveTextGlowColor;
					this.comingSoonDividerLeft.Color = this.planetActiveTextColor;
					this.comingSoonDividerCenter.Color = this.planetActiveTextColor;
				}
			}
			else
			{
				this.comingSoonEventTimerLabel.TextColor = this.planetUpcomingTextColor;
				this.comingSoonBg.Color = this.planetUpcomingTextColor;
				this.comingSoonBgGlow.Color = this.planetUpcomingTextGlowColor;
				this.comingSoonDividerLeft.Color = this.planetUpcomingTextColor;
				this.comingSoonDividerCenter.Color = this.planetUpcomingTextColor;
			}
			this.UpdateTimeRemaining();
		}

		private void UpdateComingSoonUI()
		{
			TournamentTierVO idForTopTier = TournamentController.GetIdForTopTier();
			if (idForTopTier == null)
			{
				return;
			}
			this.comingSoonTopTierTitle.Text = base.LangController.Get("CONFLICT_TOP_LEAGUE", new object[0]);
			this.comingSoonTopRewardTitleLabel.Text = base.LangController.Get("CONFLICT_TOP_REWARD", new object[0]);
			this.UpdateTierInfoUI(idForTopTier, idForTopTier.Percentage, this.comingSoonTopTierIcon, this.comingSoonTopTierName, this.comingSoonTopTierPercent);
			Dictionary<string, TournamentRewardsVO> tierRewardMap = TimedEventPrizeUtils.GetTierRewardMap(this.currentTournamentVO.RewardGroupId);
			if (tierRewardMap.ContainsKey(idForTopTier.Uid))
			{
				TournamentRewardsVO rewardGroup = tierRewardMap[idForTopTier.Uid];
				TimedEventPrizeUtils.TrySetupConflictItemRewardView(rewardGroup, this.comingSoonTopRewardIconLabel, this.comingSoonTopRewardSprite, this.comingSoonTopRewardCrateSprite, this.comingSoonTopRewardItemCardBasic, this.comingSoonTopRewardItemCardAdvanced, this.comingSoonTopRewardItemCardElite, this.comingSoonCrateCount, this.comingSoonTopRewardDataFragIcon, this.comingSoonTopRewardLabel);
				this.AnimateShowNext();
				return;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("No Tournament to display reward on planet " + this.screen.viewingPlanetVO.Uid, new object[0]);
		}

		private void UpdatePlanetViewDetailWithAnimation()
		{
			Service.Get<UXController>().MiscElementsManager.SetEventTickerViewVisible(false);
			Animator component = this.eventInfoGroup.Root.GetComponent<Animator>();
			if (component != null)
			{
				component.ResetTrigger("ShowInProgress");
				component.ResetTrigger("ShowComingSoon");
				component.ResetTrigger("ShowEnded");
				Service.Get<ViewTimerManager>().CreateViewTimer(0.05f, false, new TimerDelegate(this.UpdatePlanetView), null);
			}
		}

		private void AnimateShowNext()
		{
			Animator component = this.eventInfoGroup.Root.GetComponent<Animator>();
			if (component != null && component.isActiveAndEnabled)
			{
				component.SetTrigger("ShowNext");
			}
			this.isAnimatingShowNext = true;
		}

		private void UpdatePlanetView(uint id, object cookie)
		{
			Animator component = this.eventInfoGroup.Root.GetComponent<Animator>();
			if (component != null && component.isActiveAndEnabled)
			{
				if (this.inProgressGroup.Visible)
				{
					component.SetTrigger("ShowInProgress");
					return;
				}
				if (this.endedGroup.Visible)
				{
					component.SetTrigger("ShowEnded");
					return;
				}
				if (this.comingSoonGroup.Visible)
				{
					component.SetTrigger("ShowComingSoon");
				}
			}
		}

		private void ShowTimedEventDetails(bool IsPlayerInTournament)
		{
			this.tournamentGroup.Visible = true;
			this.comingSoonGroup.Visible = false;
			this.inProgressGroup.Visible = false;
			this.endedGroup.Visible = false;
			if (IsPlayerInTournament)
			{
				this.UpdateCurrentTournamentTier();
				this.comingSoonGroup.Visible = false;
				if (this.currentEventState == TimedEventState.Live)
				{
					this.inProgressGroup.Visible = true;
				}
				else if (this.currentEventState == TimedEventState.Closing)
				{
					this.endedGroup.Visible = true;
					this.UpdateEndedUI();
				}
			}
			else
			{
				bool flag = !Service.Get<CurrentPlayer>().IsPlanetUnlocked(this.currentTournamentVO.PlanetId);
				if (this.currentEventState == TimedEventState.Upcoming)
				{
					this.comingSoonGroup.Visible = true;
					this.UpdateComingSoonUI();
					if (flag)
					{
						this.comingSoonDescriptionLabel.Text = base.LangController.Get("CONFLICT_DESC_PRE_LOCKED", new object[0]);
					}
					else if (this.currentTournamentVO.PlanetId != Service.Get<CurrentPlayer>().PlanetId)
					{
						this.comingSoonDescriptionLabel.Text = base.LangController.Get("CONFLICT_DESC_PRE_RELOCATE", new object[0]);
					}
					else
					{
						this.comingSoonDescriptionLabel.Text = base.LangController.Get("CONFLICT_DESC_PRE", new object[0]);
					}
				}
				else if (this.currentEventState == TimedEventState.Live)
				{
					this.comingSoonGroup.Visible = true;
					this.UpdateComingSoonUI();
					if (flag)
					{
						this.comingSoonDescriptionLabel.Text = base.LangController.Get("CONFLICT_DESC_NOT_PLAYED_LOCKED", new object[0]);
					}
					else if (this.currentTournamentVO.PlanetId != Service.Get<CurrentPlayer>().PlanetId)
					{
						this.comingSoonDescriptionLabel.Text = base.LangController.Get("CONFLICT_DESC_NOT_PLAYED_RELOCATE", new object[0]);
					}
					else
					{
						this.comingSoonDescriptionLabel.Text = base.LangController.Get("CONFLICT_DESC_NOT_PLAYED", new object[0]);
					}
				}
			}
			this.UpdatePlanetViewDetailWithAnimation();
			this.StartObservingTime();
		}

		private void UpdateEndedUI()
		{
			if (this.currentTournamentVO == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("There is no Tournament to display reward on planet " + this.screen.viewingPlanetVO.Uid, new object[0]);
				return;
			}
			Tournament tournament = Service.Get<CurrentPlayer>().TournamentProgress.GetTournament(this.currentTournamentVO.Uid);
			if (tournament.FinalRank.TierUid != null)
			{
				TournamentTierVO optional = base.Sdc.GetOptional<TournamentTierVO>(tournament.FinalRank.TierUid);
				if (optional != null)
				{
					float percentile = (float)tournament.FinalRank.Percentile;
					this.UpdateTierInfoUI(optional, percentile, this.endedTierIconSprite, this.endedTierLabel, this.endedTierPercentLabel);
				}
			}
		}

		private void UpdateTierInfoUI(TournamentTierVO tierVO, float percentile, UXSprite sprite, UXLabel tierName, UXLabel tierPercent)
		{
			if (tierVO != null)
			{
				sprite.SpriteName = this.tournamentController.GetTierIconName(tierVO);
				tierName.Text = this.GetTierTitleString(tierVO);
				tierPercent.Text = base.LangController.Get("CONFLICT_TIER_PERCENTILE", new object[]
				{
					Math.Round((double)percentile, 2)
				});
			}
		}

		private void OnTournamentTiersButtonClicked(UXButton button)
		{
			Service.Get<UserInputInhibitor>().DenyAll();
			this.screen.AnimateHideUI();
			Service.Get<ViewTimerManager>().CreateViewTimer(0.1f, false, new TimerDelegate(this.OnTierButtonTimerCallback), null);
		}

		private void OnLeaderboardButtonClicked(UXButton button)
		{
			Service.Get<UserInputInhibitor>().DenyAll();
			this.screen.AnimateHideUI();
			Service.Get<ViewTimerManager>().CreateViewTimer(0.1f, false, new TimerDelegate(this.OnLeaderboardButtonTimerCallback), null);
		}

		private void OnTierButtonTimerCallback(uint id, object cookie)
		{
			Tournament tournament = base.Player.TournamentProgress.GetTournament(this.currentTournamentVO.Uid);
			base.ScrController.AddScreen(new TournamentTiersScreen(this.currentTournamentVO, tournament));
			base.EvtManager.SendEvent(EventId.UIAttackScreenSelection, new ActionMessageBIData("prizes", this.currentTournamentVO.Uid));
			Service.Get<UserInputInhibitor>().AllowAll();
		}

		private void OnLeaderboardButtonTimerCallback(uint id, object cookie)
		{
			Service.Get<UXController>().HUD.OpenConflictLeaderBoardWithPlanet(this.currentTournamentVO.PlanetId);
			Service.Get<UserInputInhibitor>().AllowAll();
		}

		public void Destroy()
		{
			base.EvtManager.UnregisterObserver(this, EventId.StoryChainCompleted);
			base.EvtManager.UnregisterObserver(this, EventId.ScreenClosing);
			if (this.observingClockTime)
			{
				Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
				this.observingClockTime = false;
			}
			base.EvtManager.UnregisterObserver(this, EventId.TournamentRedeemed);
			this.isLoaded = false;
		}

		private string GetTierTitleString(TournamentTierVO tierVO)
		{
			return base.LangController.Get("CONFLICT_LEAGUE_AND_DIVISION", new object[]
			{
				base.LangController.Get(tierVO.RankName, new object[0]),
				base.LangController.Get(tierVO.DivisionSmall, new object[0])
			});
		}

		protected internal PlanetDetailsTournamentsViewModule(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).AnimateShowNext();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).GetTierTitleString((TournamentTierVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).OnAnimateShowUI();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).OnLeaderboardButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).OnTournamentRankUpdated((TournamentRank)GCHandledObjects.GCHandleToObject(*args), (TournamentRank)GCHandledObjects.GCHandleToObject(args[1]), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).OnTournamentTiersButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).RefreshScreenForPlanetChange();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).ShowTimedEventDetails(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).StartObservingTime();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).UpdateComingSoonUI();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentTournamentTier();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).UpdateEndedUI();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).UpdatePlanetViewDetailWithAnimation();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).UpdateTierInfoUI((TournamentTierVO)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), (UXSprite)GCHandledObjects.GCHandleToObject(args[2]), (UXLabel)GCHandledObjects.GCHandleToObject(args[3]), (UXLabel)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((PlanetDetailsTournamentsViewModule)GCHandledObjects.GCHandleToObject(instance)).UpdateTimeRemaining();
			return -1L;
		}
	}
}
