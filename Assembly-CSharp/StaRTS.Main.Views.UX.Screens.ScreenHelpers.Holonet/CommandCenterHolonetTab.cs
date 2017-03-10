using StaRTS.Externals.Maker;
using StaRTS.Externals.Maker.Player;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet
{
	public class CommandCenterHolonetTab : AbstractHolonetTab, IViewFrameTimeObserver, IEventObserver, IViewClockTimeObserver
	{
		private const int devNoteLength = 120;

		private const float WARBOARD_TRANSITION_DELAY = 0.002f;

		private const float DAILY_CRATE_PARTICLE_DELAY = 1.5f;

		private const string NOTES_PANEL_TITLE_LABEL = "NotesPanelTitleLabel";

		private const string NOTES_PANEL_FEATURED_TITLE_LABEL = "NotesPanelFeaturedTitleLabel";

		private const string NOTES_PANEL_FEATURED_BODY_LABEL = "NotesPanelFeaturedBodyLabel";

		private const string BTN_LABEL_NOTES_PANEL_READ_MORE = "BtnLabelNotesPanelReadMore";

		private const string BTN_NOTES_PANEL_READ_MORE = "BtnNotesPanelReadMore";

		private const string VIDEO_PANEL_FEATURED_LABEL = "VideoPanelFeaturedLabel";

		private const string VIDEO_PANEL_LABEL_TIME_HOLO = "VideoPanelLabelTimeHolo";

		private const string VIDEO_PANEL_TITLE_LABEL = "VideoPanelTitleLabel";

		private const string VIDEO_PANEL_CREATOR_LABEL = "VideoPanelCreatorLabel";

		private const string VIDEO_PANEL_CLIPPING = "VideoClippingPanel";

		private const string BTN_FEATURED_VIDEO_PLAY = "BtnFeaturedVideoPlay";

		private const string VIDEO_PANEL_IMAGE_TEXTURE = "VideoPanelImageTexture";

		private const string VIDEO_GREEN_SOURCE = "VideoPanelBGUserTypeMoreVideos";

		private const string VIDEO_SOURCE_TEXT = "VideoPanelLabelUserTypeMoreVideos";

		private const string HOLONET_VID_SUMMARY_MESSAGE_CONTAINER = "MakerContainerErrorMsgCC";

		private const string MORE_VID_BUTTON = "BtnMoreVideosCCTab";

		private const string MORE_VID_LABEL = "BtnLabelMoreVideosCCTab";

		private const string LEADER_PANEL_CONTAINTER = "LeadersPanelContainer";

		private const string LEADER_PANEL_GRID = "LeadersGrid";

		private const string LEADER_PANEL_CARD = "LeadersCard";

		private const string LEADER_PANEL_FACTION_SYMBOL = "LeadersSpriteFactionSymbol";

		private const string LEADER_PANEL_MEDAL = "LeadersSpriteIconMedals";

		private const string LEADER_PANEL_NAME = "LeadersLabelName";

		private const string LEADER_PANEL_SQUAD = "LeadersLabelSquad";

		private const string LEADER_PANEL_MEDAL_COUNT = "LeadersLabelMedalCount";

		private const string LEADER_PANEL_BTN = "BtnLeadersPanelGoToLeaderboards";

		private const string LEADER_LABEL_BTN_LEADERBOARD = "BtnLabelLeadersGoToLeaderboards";

		private const string DAILY_CRATE_PANEL_GROUP = "PanelGroupDailyCrate";

		private const string DAILY_CRATE_PANEL_BG = "PanelBgDailyCrate";

		private const string DAILY_CRATE_PROJ_SPRITE = "SpriteDailyCrateImage";

		private const string DAILY_CRATE_BG_TEXTURE = "TextureBgDailyCrate";

		private const string DAILY_CRATE_ROOM_IMAGE = "supplycratefloor";

		private const string DAILY_CRATE_TITLE_LABEL = "LabelTitleDailyCrate";

		private const string DAILY_CRATE_TIMER_LABEL = "LabelCountdownDailyCrate";

		private const string DAILY_CRATE_GET_MORE_BTN = "BtnGetMoreDailyCrate";

		private const string DAILY_CRATE_GET_MORE_LABEL = "LabelBtnGetMoreDailyCrate";

		private const string DAILY_CRATE_OPEN_CRATE_BTN = "BtnPrimaryDailyCrate";

		private const string DAILY_CRATE_OPEN_CRATE_LABEL = "LabelBtnPrimaryDailyCrate";

		private const string DAILY_CRATE_SHADOW_TEXTURE = "TextureShadowDailyCrate";

		private const string DAILY_CRATE_AVAILABLE_OPEN_TITLE_STRINGID = "hn_ui_daily_crate_available";

		private const string DAILY_CRATE_AVAILABLE_OPEN_BTN_STRINGID = "hn_ui_daily_crate_open_cta";

		private const string DAILY_CRATE_AVAILABLE_NEXT_TITLE_STRINGID = "hn_ui_daily_crate_next_available";

		private const string DAILY_CRATE_AVAILABLE_NEXT_BTN_STRINGID = "hn_ui_daily_crate_store_cta";

		private const string DEFAULT_DAILY_CRATE_SHADOW = "shadow_dailycrate";

		private const string PAGE_DOT_GRID = "PageDotGrid";

		private const string PAGE_DOT = "PageDot";

		private const string FEATURE_PANEL_GRID = "FeaturePanelGrid";

		private const string NEWS_ITEM_FULL = "NewsItemFull";

		private const string NEWS_ITEM_HALF = "NewsItemHalf";

		private const string NEWS_ITEM_QUARTER = "NewsItemQuarter";

		private const string NEWS_ITEM_FULL_TITLE_LABEL = "NewsItemFullTitleLabel";

		private const string NEWS_ITEM_FULL_BODY_LABEL = "NewsItemFullBodyLabel";

		private const string NEWS_ITEM_FULL_BTN_ACTION_LABEL = "NewsItemFullBtnActionLabel";

		private const string NEWS_ITEM_FULL_BTN_ACTION = "NewsItemFullBtnAction";

		private const string NEWS_ITEM_FULL_IMAGE = "NewsItemFullImage";

		private const string NEWS_ITEM_HALF_TITLE_LABEL = "NewsItemHalfTitleLabel";

		private const string NEWS_ITEM_HALF_BODY_LABEL = "NewsItemHalfBodyLabel";

		private const string NEWS_ITEM_HALF_BTN_ACTION_LABEL = "NewsItemHalfBtnActionLabel";

		private const string NEWS_ITEM_HALF_BTN_ACTION = "NewsItemHalfBtnAction";

		private const string NEWS_ITEM_HALF_IMAGE = "NewsItemHalfImage";

		private const string NEWS_ITEM_QUARTER_TITLE_LABEL = "NewsItemQuarterTitleLabel";

		private const string NEWS_ITEM_QUARTER_BODY_LABEL = "NewsItemQuarterBodyLabel";

		private const string NEWS_ITEM_QUARTER_BTN_ACTION_LABEL = "NewsItemQuarterBtnActionLabel";

		private const string NEWS_ITEM_QUARTER_BTN_ICON_OPTION_LABEL = "NewsItemQuarterBtnActionIconOptionLabel";

		private const string NEWS_ITEM_QUARTER_BTN_OPTION_SPRITE = "NewsItemQuarterBtnActionIconOptionSprite";

		private const string NEWS_ITEM_QUARTER_BTN_ACTION = "NewsItemQuarterBtnAction";

		private const string NEWS_ITEM_QUARTER_BTN_ACTION_ICON_OPTION = "NewsItemQuarterBtnActionIconOption";

		private const string NEWS_ITEM_QUARTER_IMAGE = "NewsItemQuarterImage";

		private const string NEWS_ITEM_QUARTER_TABLE = "NewsItemQuarterTable";

		private const string NEWS_ITEM_QUARTER_BUTTON1 = "_1_";

		private const string NEWS_ITEM_QUARTER_BUTTON2 = "_2_";

		private const string NEWS_HN_FACEBOOK_CTA = "hn_facebook_cta";

		private const string NEWS_HN_TWITTER_CTA = "hn_twitter_cta";

		private const string FACTION_LEADERS = "hn_faction_leaders";

		private const string FACTION_LEADER = "hn_faction_leader";

		private const string LEADERS_CARD = "LeadersCard";

		private const string LEADERS_CARD_CLONE = "leadersCard #";

		private const string FACTION_SPRITE_EMPIRE = "FactionEmpire";

		private const string FACTION_SPRITE_REBEL = "FactionRebel";

		private const string NO_LEADER_FOUND = "HN_NO_LEADER_FOUND";

		private const string HOLONET_GO_TO_LEADERBOARD = "hn_leaderboard_prompt";

		private const string PLANET_NAME_PREFIX = "planet_name_";

		private const string HOLONET_DEVELOPER_NOTES = "hn_cc_developer_notes";

		private const string HOLONET_READ_MORE = "hn_cc_read_more";

		private const string IMG_TAG = "[img]";

		private const string SOURCE_TAG = "src=";

		private const string ZERO_SCORE = "0";

		private const string HOLONET_FEATURED_VIDEOS = "hn_cc_featured_videos";

		private const string MORE_VIDEOS = "MORE_VIDEOS";

		private const string NOTES_GROUP = "NotesGroup";

		private const string SQUAD_WAR_GROUP = "SquadWarGroup";

		private const string SQUAD_WAR_LABEL_TITLE = "SquadWarTitleLabel";

		private const string SQUAD_WAR_BTN_LABEL = "BtnLabelSquadWar";

		private const string SQUAD_WAR_BTN = "BtnSquadWar";

		private const string SQUAD_WAR_TEXTURE = "SquadWarTexture";

		private const string SQUAD_WAR_START = "WarStart";

		private const string SQUAD_WAR_PREP_ACTION = "WarPrepAction";

		private const string SQUAD_WAR_TIMER_PREP_ACTION = "SquadWarTimerLabelPrepAction";

		private const string SQUAD_WAR_LABEL_LEFT_NAME = "SquadWarLabelLeftName";

		private const string SQUAD_WAR_LABEL_LEFT_SCORE = "SquadWarLabelLeftScore";

		private const string SQUAD_WAR_SPRITE_LEFT = "SpriteSquadWarLeft";

		private const string SQUAD_WAR_LABEL_RIGHT_NAME = "SquadWarLabelRightName";

		private const string SQUAD_WAR_LABEL_RIGHT_SCORE = "SquadWarLabelRightScore";

		private const string SQUAD_WAR_SPRITE_RIGHT = "SpriteSquadWarRight";

		private const string SQUAD_WAR_REWARD = "WarReward";

		private const string SQUAD_WAR_REWARD_LABEL = "SquadWarLabelWinner";

		private const string SQUAD_WAR_REWARD_DIRECTIONS = "SquadWarLabelDirections";

		private const string SQUAD_WAR_TITLE = "WAR_INTERSTITIAL_TITLE";

		private const string SQUAD_WAR_BTN_TEXT = "WAR_START_TITLE";

		private const string SQUAD_WAR_PREP_TIME_REMAINING = "WAR_BASE_PREP_TIME_REMAINING";

		private const string SQUAD_WAR_ACTION_TIME_REMAINING = "WAR_BASE_ACTION_TIME_REMAINING";

		private const string SQUAD_WAR_PREP_GRACE_TIME_REMAINING = "WAR_BOARD_PREP_GRACE_PHASE";

		private const string SQUAD_WAR_ACTION_GRACE_TIME_REMAINING = "WAR_BOARD_ACTION_GRACE_PHASE";

		private const string SQUAD_WAR_PREPARE = "WAR_HOLONET_PREPARE";

		private const string SQUAD_WAR_ACTION = "WAR_HOLONET_ACTION";

		private const string WAR_END_NEWSPAPER_TITLE = "WAR_END_NEWSPAPER_TITLE";

		private const string WAR_END_NEWSPAPER_DESC = "WAR_END_NEWSPAPER_DESC";

		private const string WAR_END_NEWSPAPER_CTA_DESC = "WAR_END_NEWSPAPER_CTA_DESC";

		private const string WAR_END_NEWSPAPER_CTA = "WAR_END_NEWSPAPER_CTA";

		private UXLabel notesPanelTitleLabel;

		private UXLabel notesPanelFeaturedTitleLabel;

		private UXLabel notesPanelFeaturedBodyLabel;

		private UXLabel btnLabelNotesPanelReadMore;

		private UXButton btnNotesPanelReadMore;

		private UXElement notesGroup;

		private UXElement squadWarGroup;

		private UXTexture squadWarTexture;

		private UXElement squadWarStart;

		private UXElement squadWarPrepAction;

		private UXLabel squadWarTimerLabelPrepAction;

		private UXLabel squadWarLabelLeftName;

		private UXLabel squadWarLabelLeftScore;

		private UXLabel squadWarLabelRightName;

		private UXLabel squadWarLabelRightScore;

		private UXSprite squadWarLeftIcon;

		private UXSprite squadWarRightIcon;

		private UXElement squadWarReward;

		private UXLabel videoPanelFeaturedLabel;

		private UXLabel videoPanelLabelTimeHolo;

		private UXLabel videoPanelTitleLabel;

		private UXLabel videoPanelCreatorLabel;

		private UXElement videoClippingPanel;

		private UXButton btnFeaturedVideoPlay;

		private UXTexture videoPanelImageTexture;

		private UXLabel videoSourceLabel;

		private UXElement videoErrorContainer;

		private UXSprite videoGreenSource;

		private UXButton btnMoreVideo;

		private UXLabel lblMoreVideo;

		private UXElement leadersPanelContainer;

		private UXElement leadersCard;

		private UXGrid leadersPanelGrid;

		private UXSprite leadersfactionSymbol;

		private UXSprite leadersMedal;

		private UXLabel leadersName;

		private UXLabel leadersSquad;

		private UXLabel leadersMedalCount;

		private UXLabel btnLabelLeadersGo;

		private UXElement leaderToken;

		private UXButton leaderBoardBtn;

		private bool leaderboardIsSetup;

		private UXGrid pageDotGrid;

		private UXGrid featureGrid;

		private UXElement newsItemFullTemplate;

		private UXElement newsItemHalfTemplate;

		private UXElement newsItemQuarterTemplate;

		private UXLabel newsItemFullTitleLabel;

		private UXLabel newsItemFullBodyLabel;

		private UXLabel newsItemFullBtnActionLabel;

		private UXButton newsItemFullBtnAction;

		private UXTexture newsItemFullImage;

		private float featureSwipeTimer;

		private UXElement nextAutoElement;

		private VideoSummary videoSummary;

		private VideoSummary videoSummaryFeatured;

		private int previousCarouselIndex;

		private Dictionary<string, List<UXButton>> ctaButtonList;

		private Dictionary<string, List<UXLabel>> ctaLabelList;

		private int endActionPhaseTime;

		private int endPrepPhaseTime;

		private int endActionGracePhaseTime;

		private int endPrepGracePhaseTime;

		private UXElement dailyCratePanel;

		private UXElement dailyCratePanelBG;

		private UXLabel nextCrateTimeLabel;

		private CrateData dailyCrate;

		private UXSprite dailyCrateSprite;

		private UXElement dailyCrateParticleFX;

		private uint dailyParticleTimer;

		private GeometryProjector dailyCrateProj;

		private UXTexture crateShadow;

		public CommandCenterHolonetTab(HolonetScreen screen, HolonetControllerType holonetControllerType) : base(screen, holonetControllerType)
		{
			InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
			this.dailyParticleTimer = 0u;
			base.InitializeTab("CommandCenterTab", "hn_commandcenter_tab");
			this.dailyCrate = crates.GetDailyCrateIfAvailable();
			this.ctaButtonList = new Dictionary<string, List<UXButton>>();
			this.ctaLabelList = new Dictionary<string, List<UXLabel>>();
			this.videoPanelFeaturedLabel = screen.GetElement<UXLabel>("VideoPanelFeaturedLabel");
			this.videoPanelLabelTimeHolo = screen.GetElement<UXLabel>("VideoPanelLabelTimeHolo");
			this.videoPanelTitleLabel = screen.GetElement<UXLabel>("VideoPanelTitleLabel");
			this.videoPanelCreatorLabel = screen.GetElement<UXLabel>("VideoPanelCreatorLabel");
			this.videoClippingPanel = screen.GetElement<UXElement>("VideoClippingPanel");
			this.btnFeaturedVideoPlay = screen.GetElement<UXButton>("BtnFeaturedVideoPlay");
			this.videoPanelImageTexture = screen.GetElement<UXTexture>("VideoPanelImageTexture");
			this.videoGreenSource = screen.GetElement<UXSprite>("VideoPanelBGUserTypeMoreVideos");
			this.videoSourceLabel = screen.GetElement<UXLabel>("VideoPanelLabelUserTypeMoreVideos");
			this.videoErrorContainer = screen.GetElement<UXElement>("MakerContainerErrorMsgCC");
			this.videoErrorContainer.Visible = false;
			this.btnMoreVideo = screen.GetElement<UXButton>("BtnMoreVideosCCTab");
			this.btnMoreVideo.OnClicked = new UXButtonClickedDelegate(this.LinkToMoreVideos);
			this.lblMoreVideo = screen.GetElement<UXLabel>("BtnLabelMoreVideosCCTab");
			this.lblMoreVideo.Text = this.lang.Get("MORE_VIDEOS", new object[0]);
			this.leadersPanelContainer = screen.GetElement<UXElement>("LeadersPanelContainer");
			this.leadersCard = screen.GetElement<UXElement>("LeadersCard");
			this.leadersPanelGrid = screen.GetElement<UXGrid>("LeadersGrid");
			this.leaderboardIsSetup = false;
			this.dailyCratePanel = screen.GetElement<UXElement>("PanelGroupDailyCrate");
			this.dailyCratePanelBG = screen.GetElement<UXElement>("PanelBgDailyCrate");
			this.dailyCrateSprite = screen.GetElement<UXSprite>("SpriteDailyCrateImage");
			this.nextCrateTimeLabel = screen.GetElement<UXLabel>("LabelCountdownDailyCrate");
			this.dailyCratePanel.Visible = false;
			this.dailyCratePanelBG.Visible = false;
			this.btnMoreVideo.Visible = false;
			this.leadersPanelContainer.Visible = false;
			this.featureSwipeTimer = 0f;
			this.SetupSquadWarInfo();
			this.notesPanelTitleLabel = screen.GetElement<UXLabel>("NotesPanelTitleLabel");
			this.notesPanelFeaturedTitleLabel = screen.GetElement<UXLabel>("NotesPanelFeaturedTitleLabel");
			this.notesPanelFeaturedBodyLabel = screen.GetElement<UXLabel>("NotesPanelFeaturedBodyLabel");
			this.btnLabelNotesPanelReadMore = screen.GetElement<UXLabel>("BtnLabelNotesPanelReadMore");
			this.btnNotesPanelReadMore = screen.GetElement<UXButton>("BtnNotesPanelReadMore");
			this.btnNotesPanelReadMore.OnClicked = new UXButtonClickedDelegate(this.LinkToDevNotes);
			this.btnLabelNotesPanelReadMore.Text = this.lang.Get("hn_cc_read_more", new object[0]);
			this.notesPanelTitleLabel.Text = this.lang.Get("hn_cc_developer_notes", new object[0]);
			List<DevNoteEntryVO> devNotes = Service.Get<HolonetController>().DevNotesController.DevNotes;
			if (devNotes != null && devNotes.Count > 0)
			{
				string text = this.lang.Get(devNotes[0].BodyText, new object[0]);
				StringBuilder stringBuilder = new StringBuilder();
				string[] array = new string[]
				{
					"[img]"
				};
				string[] array2 = text.Split(array, 0);
				int i = 0;
				int num = array2.Length;
				while (i < num)
				{
					if (!array2[i].StartsWith("src="))
					{
						stringBuilder.Append(array2[i]);
					}
					i++;
				}
				string text2 = stringBuilder.ToString();
				if (text2.get_Length() >= 120)
				{
					int num2 = text2.LastIndexOf(' ', 120);
					if (num2 == -1)
					{
						num2 = 120;
					}
					text2 = text2.Substring(0, num2);
				}
				text2 += "...";
				this.notesPanelFeaturedTitleLabel.Text = this.lang.Get(devNotes[0].TitleText, new object[0]);
				this.notesPanelFeaturedBodyLabel.Text = text2;
			}
			this.pageDotGrid = screen.GetElement<UXGrid>("PageDotGrid");
			this.pageDotGrid.SetTemplateItem("PageDot");
			this.featureGrid = screen.GetElement<UXGrid>("FeaturePanelGrid");
			this.featureGrid.SetCenteredFinishedCallback(new UXGrid.OnCentered(this.OnCenteredFinished));
			this.newsItemFullTemplate = screen.GetElement<UXElement>("NewsItemFull");
			this.newsItemHalfTemplate = screen.GetElement<UXElement>("NewsItemHalf");
			this.newsItemQuarterTemplate = screen.GetElement<UXElement>("NewsItemQuarter");
			screen.GetElement<UXButton>("NewsItemQuarterBtnAction").Visible = false;
			this.newsItemFullTemplate.Visible = false;
			this.newsItemHalfTemplate.Visible = false;
			this.newsItemQuarterTemplate.Visible = false;
			List<CommandCenterVO> featuredItems = Service.Get<HolonetController>().CommandCenterController.FeaturedItems;
			int j = 0;
			int count = featuredItems.Count;
			while (j < count)
			{
				CommandCenterVO commandCenterVO = featuredItems[j];
				UXElement uXElement;
				switch (commandCenterVO.Layout)
				{
				case 1:
				{
					uXElement = this.featureGrid.CloneItem(commandCenterVO.Uid, this.newsItemFullTemplate);
					this.featureGrid.GetSubElement<UXLabel>(commandCenterVO.Uid, "NewsItemFullTitleLabel").Text = this.lang.Get(commandCenterVO.TitleText, new object[0]);
					this.featureGrid.GetSubElement<UXLabel>(commandCenterVO.Uid, "NewsItemFullBodyLabel").Text = this.lang.Get(commandCenterVO.BodyText, new object[0]);
					UXTexture subElement = this.featureGrid.GetSubElement<UXTexture>(commandCenterVO.Uid, "NewsItemFullImage");
					base.DeferTexture(subElement, commandCenterVO.Image);
					this.ctaButtonList.Add(commandCenterVO.Uid, new List<UXButton>());
					this.ctaLabelList.Add(commandCenterVO.Uid, new List<UXLabel>());
					UXButton subElement2 = this.featureGrid.GetSubElement<UXButton>(commandCenterVO.Uid, "NewsItemFullBtnAction");
					UXLabel subElement3 = this.featureGrid.GetSubElement<UXLabel>(commandCenterVO.Uid, "NewsItemFullBtnActionLabel");
					this.ctaButtonList[commandCenterVO.Uid].Add(subElement2);
					this.ctaLabelList[commandCenterVO.Uid].Add(subElement3);
					base.PrepareButton(commandCenterVO, 1, subElement2, subElement3);
					break;
				}
				case 2:
				{
					uXElement = this.featureGrid.CloneItem(commandCenterVO.Uid, this.newsItemHalfTemplate);
					this.featureGrid.GetSubElement<UXLabel>(commandCenterVO.Uid, "NewsItemHalfTitleLabel").Text = this.lang.Get(commandCenterVO.TitleText, new object[0]);
					this.featureGrid.GetSubElement<UXLabel>(commandCenterVO.Uid, "NewsItemHalfBodyLabel").Text = this.lang.Get(commandCenterVO.BodyText, new object[0]);
					this.ctaButtonList.Add(commandCenterVO.Uid, new List<UXButton>());
					this.ctaLabelList.Add(commandCenterVO.Uid, new List<UXLabel>());
					UXButton subElement4 = this.featureGrid.GetSubElement<UXButton>(commandCenterVO.Uid, "NewsItemHalfBtnAction");
					UXLabel subElement5 = this.featureGrid.GetSubElement<UXLabel>(commandCenterVO.Uid, "NewsItemHalfBtnActionLabel");
					this.ctaButtonList[commandCenterVO.Uid].Add(subElement4);
					this.ctaLabelList[commandCenterVO.Uid].Add(subElement5);
					base.PrepareButton(commandCenterVO, 1, subElement4, subElement5);
					UXTexture subElement6 = this.featureGrid.GetSubElement<UXTexture>(commandCenterVO.Uid, "NewsItemHalfImage");
					base.DeferTexture(subElement6, commandCenterVO.Image);
					break;
				}
				case 3:
				{
					uXElement = this.featureGrid.CloneItem(commandCenterVO.Uid, this.newsItemQuarterTemplate);
					this.featureGrid.GetSubElement<UXLabel>(commandCenterVO.Uid, "NewsItemQuarterTitleLabel").Text = this.lang.Get(commandCenterVO.TitleText, new object[0]);
					this.featureGrid.GetSubElement<UXLabel>(commandCenterVO.Uid, "NewsItemQuarterBodyLabel").Text = this.lang.Get(commandCenterVO.BodyText, new object[0]);
					UXTexture subElement7 = this.featureGrid.GetSubElement<UXTexture>(commandCenterVO.Uid, "NewsItemQuarterImage");
					base.DeferTexture(subElement7, commandCenterVO.Image);
					UXTable subElement8 = this.featureGrid.GetSubElement<UXTable>(commandCenterVO.Uid, "NewsItemQuarterTable");
					if (!string.IsNullOrEmpty(commandCenterVO.Btn1))
					{
						this.QuarterButtonSetup(commandCenterVO, subElement8, 1, commandCenterVO.Btn1);
					}
					if (!string.IsNullOrEmpty(commandCenterVO.Btn2))
					{
						this.QuarterButtonSetup(commandCenterVO, subElement8, 2, commandCenterVO.Btn2);
					}
					subElement8.RepositionItemsFrameDelayed();
					break;
				}
				default:
					Service.Get<StaRTSLogger>().Warn("Holonet Command Layout: " + featuredItems[j].Layout + " not supported.");
					uXElement = null;
					break;
				}
				if (uXElement != null)
				{
					this.featureGrid.AddItem(uXElement, j);
					uXElement.Tag = commandCenterVO;
					UXElement uXElement2 = this.pageDotGrid.CloneTemplateItem(commandCenterVO.Uid);
					this.pageDotGrid.AddItem(uXElement2, j);
					UXCheckbox uXCheckbox = (UXCheckbox)uXElement2;
					uXCheckbox.OnSelected = new UXCheckboxSelectedDelegate(this.FeaturedItemDotClicked);
					uXCheckbox.Tag = j;
					if (j == 0)
					{
						string cookie = commandCenterVO.Uid + "|auto";
						this.eventManager.SendEvent(EventId.HolonetCommandCenterFeature, cookie);
					}
				}
				j++;
			}
			screen.GetElement<UXTexture>("TextureBgDailyCrate").LoadTexture("supplycratefloor");
			this.pageDotGrid.RepositionItems();
			this.pageDotGrid.CenterElementsInPanel();
			this.featureGrid.RepositionItemsFrameDelayed();
		}

		private void SetupDailyCratePanel()
		{
			IDataController dataController = Service.Get<IDataController>();
			Service.Get<EventManager>().RegisterObserver(this, EventId.CrateInventoryUpdated);
			this.HideViewUIElements();
			this.HideDailyCrateParticleFX();
			bool flag = this.dailyCrate != null;
			UXLabel element = this.screen.GetElement<UXLabel>("LabelTitleDailyCrate");
			UXButton element2 = this.screen.GetElement<UXButton>("BtnGetMoreDailyCrate");
			UXLabel element3 = this.screen.GetElement<UXLabel>("LabelBtnGetMoreDailyCrate");
			UXButton element4 = this.screen.GetElement<UXButton>("BtnPrimaryDailyCrate");
			UXLabel element5 = this.screen.GetElement<UXLabel>("LabelBtnPrimaryDailyCrate");
			element5.Text = this.lang.Get("hn_ui_daily_crate_open_cta", new object[0]);
			element3.Text = this.lang.Get("hn_ui_daily_crate_store_cta", new object[0]);
			this.nextCrateTimeLabel.Visible = false;
			element2.Visible = false;
			element3.Visible = false;
			element4.Visible = false;
			element5.Visible = false;
			element.Visible = true;
			this.dailyCratePanel.Visible = true;
			this.dailyCratePanelBG.Visible = true;
			this.dailyCrateSprite.Visible = true;
			string text;
			string text2;
			if (!flag)
			{
				InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
				text = crates.GetNextDailyCrateId();
				text2 = this.lang.Get("hn_ui_daily_crate_next_available", new object[0]);
			}
			else
			{
				text2 = this.lang.Get("hn_ui_daily_crate_available", new object[0]);
				text = this.dailyCrate.CrateId;
			}
			element.Text = text2;
			if (string.IsNullOrEmpty(text))
			{
				Service.Get<StaRTSLogger>().Error("CommandCenterHolonetTab.SetupDailyCratePanel Daily Crate Data missing crate CMS UID");
				return;
			}
			CrateVO optional = dataController.GetOptional<CrateVO>(text);
			if (optional == null)
			{
				Service.Get<StaRTSLogger>().Error("CommandCenterHolonetTab.SetupDailyCratePanel Daily Crate Data has invalid crate CMS UID " + text);
				return;
			}
			string text3 = optional.HoloCrateShadowTextureName;
			this.crateShadow = this.screen.GetElement<UXTexture>("TextureShadowDailyCrate");
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "shadow_dailycrate";
			}
			if (flag)
			{
				this.crateShadow.LoadTexture(text3);
			}
			if (this.dailyCrateProj != null)
			{
				this.dailyCrateProj.Destroy();
			}
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(optional, this.dailyCrateSprite);
			projectorConfig.AnimState = AnimState.Closed;
			projectorConfig.AnimPreference = AnimationPreference.AnimationAlways;
			projectorConfig.EnableCrateHoloShaderSwap = !flag;
			projectorConfig.CameraPosition = optional.HoloNetIconCameraPostion;
			projectorConfig.CameraInterest = optional.HoloNetIconLookAtPostion;
			if (projectorConfig.EnableCrateHoloShaderSwap && !string.IsNullOrEmpty(optional.UIColor))
			{
				projectorConfig.Tint = FXUtils.ConvertHexStringToColorObject(optional.UIColor);
			}
			this.dailyCrateProj = ProjectorUtils.GenerateProjector(projectorConfig);
			if (!flag)
			{
				this.nextCrateTimeLabel.Visible = true;
				element2.Visible = true;
				element3.Visible = true;
				this.crateShadow.Visible = false;
				this.UpdateNextDailyCrateTimeLabel();
				element2.OnClicked = new UXButtonClickedDelegate(this.OnGetCrateClicked);
				return;
			}
			element4.Visible = true;
			element5.Visible = true;
			this.crateShadow.Visible = true;
			element4.OnClicked = new UXButtonClickedDelegate(this.OnOpenCrateClicked);
			this.ScheduleDailyCrateParticleActivation(optional);
		}

		private void UpdateNextDailyCrateTimeLabel()
		{
			InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
			uint nextDailyCrateTime = crates.NextDailyCrateTime;
			uint time = ServerTime.Time;
			string text = string.Empty;
			if (nextDailyCrateTime >= time)
			{
				uint num = nextDailyCrateTime - time;
				text = LangUtils.FormatTime((long)((ulong)num));
			}
			this.nextCrateTimeLabel.Text = text;
		}

		private void OnGetCrateClicked(UXButton btn)
		{
			this.screen.Close(null);
			GameUtils.OpenStoreTreasureTab();
		}

		private void OnOpenCrateClicked(UXButton btn)
		{
			this.HideDailyCrateParticleFX();
			GameUtils.OpenInventoryCrateModal(this.dailyCrate, new OnScreenModalResult(this.OnCrateInfoModalClosed));
		}

		private void OnCrateInfoModalClosed(object result, object cookie)
		{
			if (cookie == null && this.dailyCrate != null)
			{
				IDataController dataController = Service.Get<IDataController>();
				CrateVO optional = dataController.GetOptional<CrateVO>(this.dailyCrate.CrateId);
				this.ScheduleDailyCrateParticleActivation(optional);
				return;
			}
			Service.Get<HolonetController>().RegisterForHolonetToReopenAfterCrateReward();
			this.screen.Close(null);
		}

		private void CancelDailyCrateParticleFXTimer()
		{
			if (this.dailyParticleTimer != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.dailyParticleTimer);
				this.dailyParticleTimer = 0u;
			}
		}

		private void ScheduleDailyCrateParticleActivation(CrateVO crateVO)
		{
			this.dailyParticleTimer = Service.Get<ViewTimerManager>().CreateViewTimer(1.5f, false, new TimerDelegate(this.OnCrateParticleTimerDone), crateVO);
		}

		private void OnCrateParticleTimerDone(uint timerId, object cookie)
		{
			if (cookie == null)
			{
				return;
			}
			CrateVO crateVO = (CrateVO)cookie;
			this.dailyParticleTimer = 0u;
			if (crateVO == null)
			{
				Service.Get<StaRTSLogger>().Error("CommandCenterHolonetTab.OnCrateParticleTimerDone Patricle crate timer cookie not a CrateVO");
				return;
			}
			string holoParticleEffectId = crateVO.HoloParticleEffectId;
			if (string.IsNullOrEmpty(holoParticleEffectId))
			{
				Service.Get<StaRTSLogger>().Error("CommandCenterHolonetTab.OnCrateParticleTimerDone Daily Crate missing holonet FX in crate: " + crateVO.Uid);
				return;
			}
			this.dailyCrateParticleFX = this.screen.GetOptionalElement<UXElement>(holoParticleEffectId);
			if (this.dailyCrateParticleFX == null)
			{
				Service.Get<StaRTSLogger>().Error("CommandCenterHolonetTab.OnCrateParticleTimerDone Could not find crate fx id in UI " + holoParticleEffectId + " in " + crateVO.Uid);
				return;
			}
			this.dailyCrateParticleFX.Visible = true;
		}

		private void HideDailyCrateParticleFX()
		{
			if (this.dailyCrateParticleFX != null)
			{
				this.dailyCrateParticleFX.Visible = false;
			}
		}

		private void SetupSquadWarInfo()
		{
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			SquadWarData currentSquadWar = warManager.CurrentSquadWar;
			SquadWarStatusType currentStatus = warManager.GetCurrentStatus();
			bool flag = Service.Get<CurrentPlayer>().Faction == FactionType.Empire;
			this.notesGroup = this.screen.GetElement<UXElement>("NotesGroup");
			this.squadWarGroup = this.screen.GetElement<UXElement>("SquadWarGroup");
			this.squadWarTexture = this.screen.GetElement<UXTexture>("SquadWarTexture");
			UXLabel element = this.screen.GetElement<UXLabel>("SquadWarTitleLabel");
			UXLabel element2 = this.screen.GetElement<UXLabel>("BtnLabelSquadWar");
			UXButton element3 = this.screen.GetElement<UXButton>("BtnSquadWar");
			this.squadWarStart = this.screen.GetElement<UXElement>("WarStart");
			this.squadWarPrepAction = this.screen.GetElement<UXElement>("WarPrepAction");
			this.squadWarTimerLabelPrepAction = this.screen.GetElement<UXLabel>("SquadWarTimerLabelPrepAction");
			this.squadWarLabelLeftName = this.screen.GetElement<UXLabel>("SquadWarLabelLeftName");
			this.squadWarLabelLeftScore = this.screen.GetElement<UXLabel>("SquadWarLabelLeftScore");
			this.squadWarLabelRightName = this.screen.GetElement<UXLabel>("SquadWarLabelRightName");
			this.squadWarLabelRightScore = this.screen.GetElement<UXLabel>("SquadWarLabelRightScore");
			this.squadWarLeftIcon = this.screen.GetElement<UXSprite>("SpriteSquadWarLeft");
			this.squadWarRightIcon = this.screen.GetElement<UXSprite>("SpriteSquadWarRight");
			this.squadWarReward = this.screen.GetElement<UXElement>("WarReward");
			this.notesGroup.Visible = false;
			this.squadWarGroup.Visible = true;
			element.Text = this.lang.Get("WAR_INTERSTITIAL_TITLE", new object[0]);
			element3.OnClicked = new UXButtonClickedDelegate(this.OnSquadWarBtnClicked);
			element3.Visible = true;
			this.squadWarStart.Visible = false;
			this.squadWarPrepAction.Visible = false;
			this.squadWarReward.Visible = false;
			IDataController dataController = Service.Get<IDataController>();
			string text = null;
			switch (currentStatus)
			{
			case SquadWarStatusType.PhaseOpen:
				this.squadWarStart.Visible = true;
				element2.Text = this.lang.Get("WAR_START_TITLE", new object[0]);
				text = (flag ? GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_OPEN : GameConstants.HOLONET_TEXTURE_WAR_REBEL_OPEN);
				break;
			case SquadWarStatusType.PhasePrep:
			case SquadWarStatusType.PhasePrepGrace:
			case SquadWarStatusType.PhaseAction:
			case SquadWarStatusType.PhaseActionGrace:
			{
				uint serverTime = Service.Get<ServerAPI>().ServerTime;
				SquadWarSquadData squadWarSquadData = currentSquadWar.Squads[0];
				SquadWarSquadData squadWarSquadData2 = currentSquadWar.Squads[1];
				this.squadWarPrepAction.Visible = true;
				SquadWarSquadData squadWarSquadData3 = squadWarSquadData;
				SquadWarSquadData squadWarSquadData4 = squadWarSquadData2;
				SquadWarSquadType squadType = SquadWarSquadType.PLAYER_SQUAD;
				SquadWarSquadType squadType2 = SquadWarSquadType.OPPONENT_SQUAD;
				this.squadWarLeftIcon.SpriteName = ((squadWarSquadData.Faction == FactionType.Rebel) ? "FactionRebel" : "FactionEmpire");
				this.squadWarRightIcon.SpriteName = ((squadWarSquadData2.Faction == FactionType.Rebel) ? "FactionRebel" : "FactionEmpire");
				int currentSquadScore = warManager.GetCurrentSquadScore(squadType);
				int currentSquadScore2 = warManager.GetCurrentSquadScore(squadType2);
				this.squadWarLabelLeftName.Text = squadWarSquadData3.SquadName;
				this.squadWarLabelLeftScore.Text = currentSquadScore.ToString();
				this.squadWarLabelRightName.Text = squadWarSquadData4.SquadName;
				this.squadWarLabelRightScore.Text = currentSquadScore2.ToString();
				switch (currentStatus)
				{
				case SquadWarStatusType.PhasePrep:
					this.endPrepPhaseTime = currentSquadWar.PrepGraceStartTimeStamp - (int)serverTime;
					break;
				case SquadWarStatusType.PhasePrepGrace:
					this.endPrepGracePhaseTime = currentSquadWar.PrepEndTimeStamp - (int)serverTime;
					break;
				case SquadWarStatusType.PhaseAction:
					this.endActionPhaseTime = currentSquadWar.ActionGraceStartTimeStamp - (int)serverTime;
					break;
				case SquadWarStatusType.PhaseActionGrace:
					this.endActionGracePhaseTime = currentSquadWar.ActionEndTimeStamp - (int)serverTime;
					break;
				}
				if (currentStatus == SquadWarStatusType.PhasePrep || currentStatus == SquadWarStatusType.PhasePrepGrace)
				{
					element2.Text = this.lang.Get("WAR_HOLONET_PREPARE", new object[0]);
					text = (flag ? GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_PREP : GameConstants.HOLONET_TEXTURE_WAR_REBEL_PREP);
				}
				else
				{
					element2.Text = this.lang.Get("WAR_HOLONET_ACTION", new object[0]);
					text = (flag ? GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_ACTION : GameConstants.HOLONET_TEXTURE_WAR_REBEL_ACTION);
				}
				Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
				break;
			}
			case SquadWarStatusType.PhaseCooldown:
			{
				this.squadWarReward.Visible = true;
				text = (flag ? GameConstants.HOLONET_TEXTURE_WAR_EMPIRE_COOLDOWN : GameConstants.HOLONET_TEXTURE_WAR_REBEL_COOLDOWN);
				element.Text = this.lang.Get("WAR_END_NEWSPAPER_TITLE", new object[0]);
				UXLabel element4 = this.screen.GetElement<UXLabel>("SquadWarLabelWinner");
				element4.Text = this.lang.Get("WAR_END_NEWSPAPER_DESC", new object[0]);
				UXLabel element5 = this.screen.GetElement<UXLabel>("SquadWarLabelDirections");
				element5.Text = this.lang.Get("WAR_END_NEWSPAPER_CTA_DESC", new object[0]);
				element2.Text = this.lang.Get("WAR_END_NEWSPAPER_CTA", new object[0]);
				break;
			}
			}
			if (!string.IsNullOrEmpty(text))
			{
				TextureVO optional = dataController.GetOptional<TextureVO>(text);
				if (optional != null)
				{
					this.squadWarTexture.LoadTexture(optional.AssetName);
				}
			}
		}

		private void OnSquadWarBtnClicked(UXButton btn)
		{
			this.screen.Close(null);
			Service.Get<ViewTimerManager>().CreateViewTimer(0.002f, false, new TimerDelegate(this.TransitionToWarBoard), null);
		}

		private void TransitionToWarBoard(uint timerId, object cookie)
		{
			Service.Get<EventManager>().SendEvent(EventId.WarLaunchFlow, null);
		}

		private void HideViewUIElements()
		{
			this.videoPanelLabelTimeHolo.Visible = false;
			this.videoPanelTitleLabel.Visible = false;
			this.videoPanelCreatorLabel.Visible = false;
			this.videoClippingPanel.Visible = false;
			this.btnFeaturedVideoPlay.Visible = false;
			this.videoPanelImageTexture.Visible = false;
			this.videoGreenSource.Visible = false;
			this.videoSourceLabel.Visible = false;
			this.videoPanelFeaturedLabel.Visible = false;
			this.videoErrorContainer.Visible = false;
		}

		private void ShowLeaderboardInfo()
		{
			this.HideViewUIElements();
			this.leaderBoardBtn = this.screen.GetElement<UXButton>("BtnLeadersPanelGoToLeaderboards");
			this.btnLabelLeadersGo = this.screen.GetElement<UXLabel>("BtnLabelLeadersGoToLeaderboards");
			this.btnLabelLeadersGo.Text = this.lang.Get("hn_leaderboard_prompt", new object[0]);
			this.leaderBoardBtn.OnClicked = new UXButtonClickedDelegate(this.GoToLeaderBoardClicked);
			if (!this.leaderboardIsSetup)
			{
				this.leaderboardIsSetup = true;
				Service.Get<EventManager>().RegisterObserver(this, EventId.HolonetLeaderBoardUpdated);
				Service.Get<LeaderboardController>().TopPlayer(Service.Get<CurrentPlayer>().Planet);
				return;
			}
			this.leadersPanelContainer.Visible = true;
			this.videoPanelFeaturedLabel.Visible = true;
		}

		private void QuarterButtonSetup(CommandCenterVO vo, UXTable buttonsTable, int index, string btn)
		{
			if (!this.ctaButtonList.ContainsKey(vo.Uid))
			{
				this.ctaButtonList.Add(vo.Uid, new List<UXButton>());
			}
			if (!this.ctaLabelList.ContainsKey(vo.Uid))
			{
				this.ctaLabelList.Add(vo.Uid, new List<UXLabel>());
			}
			string text = vo.Uid + index;
			UXButton uXButton;
			string name;
			if (btn == "hn_facebook_cta" || btn == "hn_twitter_cta")
			{
				string templateItem = UXUtils.FormatAppendedName("NewsItemQuarterBtnActionIconOption", vo.Uid);
				buttonsTable.SetTemplateItem(templateItem);
				uXButton = (buttonsTable.CloneTemplateItem(text) as UXButton);
				name = UXUtils.FormatAppendedName("NewsItemQuarterBtnActionIconOptionLabel", vo.Uid);
				string text2 = UXUtils.FormatAppendedName("NewsItemQuarterBtnActionIconOptionSprite", vo.Uid);
				text2 = UXUtils.FormatAppendedName(text2, text);
				UXSprite element = this.screen.GetElement<UXSprite>(text2);
				if (btn == "hn_facebook_cta")
				{
					element.SpriteName = "icoFacebook";
				}
				else if (btn == "hn_twitter_cta")
				{
					element.SpriteName = "icoTwitter";
				}
			}
			else
			{
				string templateItem = UXUtils.FormatAppendedName("NewsItemQuarterBtnAction", vo.Uid);
				buttonsTable.SetTemplateItem(templateItem);
				uXButton = (buttonsTable.CloneTemplateItem(text) as UXButton);
				name = UXUtils.FormatAppendedName("NewsItemQuarterBtnActionLabel", vo.Uid);
			}
			UXLabel subElement = buttonsTable.GetSubElement<UXLabel>(text, name);
			this.ctaButtonList[vo.Uid].Add(uXButton);
			this.ctaLabelList[vo.Uid].Add(subElement);
			buttonsTable.AddItem(uXButton, buttonsTable.Count);
			base.PrepareButton(vo, index, uXButton, subElement);
		}

		public void OnCenteredFinished(UXElement element, int index)
		{
			if (this.nextAutoElement != null)
			{
				this.nextAutoElement = null;
			}
		}

		public void MakeLeaderBoardExcerpt()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.HolonetLeaderBoardUpdated);
			LeaderboardController leaderboardController = Service.Get<LeaderboardController>();
			this.leadersPanelContainer.Visible = true;
			this.videoPanelFeaturedLabel.Visible = true;
			if (leaderboardController.topPlayers.Count == 2)
			{
				this.videoPanelFeaturedLabel.Text = this.lang.Get("hn_faction_leaders", new object[]
				{
					this.lang.Get("planet_name_" + Service.Get<CurrentPlayer>().PlanetId, new object[0])
				});
				int i = 0;
				int count = leaderboardController.topPlayers.Count;
				while (i < count)
				{
					this.MakeLeaderClone(leaderboardController, i);
					i++;
				}
			}
			else if (leaderboardController.topPlayers.Count <= 0)
			{
				this.MakeNoLeaderFoundClone();
			}
			else
			{
				this.MakeLeaderClone(leaderboardController, 0);
				this.videoPanelFeaturedLabel.Text = this.lang.Get("hn_faction_leader", new object[]
				{
					Service.Get<CurrentPlayer>().Planet.PlanetBIName
				});
				this.leadersPanelGrid.CenterElementsInPanel();
			}
			this.leadersPanelGrid.RepositionItems();
			leaderboardController.topPlayers.Clear();
		}

		private void MakeLeaderClone(LeaderboardController leader, int i)
		{
			this.leadersPanelGrid.SetTemplateItem("LeadersCard");
			this.leaderToken = this.leadersPanelGrid.CloneItem("leadersCard #" + i, this.leadersCard);
			this.leadersPanelGrid.AddItem(this.leaderToken, i);
			this.leadersfactionSymbol = this.leadersPanelGrid.GetSubElement<UXSprite>("leadersCard #" + i, "LeadersSpriteFactionSymbol");
			this.leadersName = this.leadersPanelGrid.GetSubElement<UXLabel>("leadersCard #" + i, "LeadersLabelName");
			this.leadersSquad = this.leadersPanelGrid.GetSubElement<UXLabel>("leadersCard #" + i, "LeadersLabelSquad");
			this.leadersMedalCount = this.leadersPanelGrid.GetSubElement<UXLabel>("leadersCard #" + i, "LeadersLabelMedalCount");
			if (leader.topPlayers[i].Faction == FactionType.Empire)
			{
				this.leadersfactionSymbol.SpriteName = "FactionEmpire";
			}
			else
			{
				this.leadersfactionSymbol.SpriteName = "FactionRebel";
			}
			this.leadersMedalCount.Text = leader.topPlayers[i].BattleScore.ToString();
			this.leadersName.Text = leader.topPlayers[i].PlayerName;
			this.leadersSquad.Text = leader.topPlayers[i].SquadName;
		}

		private void MakeNoLeaderFoundClone()
		{
			this.videoPanelFeaturedLabel.Text = this.lang.Get("hn_faction_leaders", new object[]
			{
				Service.Get<CurrentPlayer>().Planet.PlanetBIName
			});
			this.leadersPanelGrid.SetTemplateItem("LeadersCard");
			this.leaderToken = this.leadersPanelGrid.CloneItem("leadersCard #", this.leadersCard);
			this.leadersPanelGrid.AddItem(this.leaderToken, 0);
			this.leadersfactionSymbol = this.leadersPanelGrid.GetSubElement<UXSprite>("leadersCard #", "LeadersSpriteFactionSymbol");
			this.leadersMedal = this.leadersPanelGrid.GetSubElement<UXSprite>("leadersCard #", "LeadersSpriteIconMedals");
			this.leadersName = this.leadersPanelGrid.GetSubElement<UXLabel>("leadersCard #", "LeadersLabelName");
			this.leadersSquad = this.leadersPanelGrid.GetSubElement<UXLabel>("leadersCard #", "LeadersLabelSquad");
			this.leadersMedalCount = this.leadersPanelGrid.GetSubElement<UXLabel>("leadersCard #", "LeadersLabelMedalCount");
			this.leadersName.Text = this.lang.Get("HN_NO_LEADER_FOUND", new object[0]);
			this.leadersSquad.Visible = false;
			this.leadersMedal.Visible = false;
			this.leadersMedalCount.Visible = false;
			this.leadersfactionSymbol.Visible = false;
			this.leadersMedalCount.Text = "0";
		}

		protected override void FeaturedButton1Clicked(UXButton button)
		{
			CommandCenterVO commandCenterVO = (CommandCenterVO)button.Tag;
			Service.Get<HolonetController>().HandleCallToActionButton(commandCenterVO.Btn1Action, commandCenterVO.Btn1Data, commandCenterVO.Uid);
			this.RefreshButtons(commandCenterVO);
			base.SendCallToActionBI(commandCenterVO.Btn1Action, commandCenterVO.Uid, EventId.HolonetCommandCenterTab);
		}

		protected override void FeaturedButton2Clicked(UXButton button)
		{
			CommandCenterVO commandCenterVO = (CommandCenterVO)button.Tag;
			Service.Get<HolonetController>().HandleCallToActionButton(commandCenterVO.Btn2Action, commandCenterVO.Btn2Data, commandCenterVO.Uid);
			this.RefreshButtons(commandCenterVO);
			base.SendCallToActionBI(commandCenterVO.Btn2Action, commandCenterVO.Uid, EventId.HolonetCommandCenterTab);
		}

		private void RefreshButtons(CommandCenterVO vo)
		{
			if (!this.ctaButtonList.ContainsKey(vo.Uid) || this.ctaButtonList[vo.Uid] == null)
			{
				return;
			}
			if (this.ctaButtonList[vo.Uid].Count > 0)
			{
				base.PrepareButton(vo, 1, this.ctaButtonList[vo.Uid][0], this.ctaLabelList[vo.Uid][0]);
			}
			if (this.ctaButtonList[vo.Uid].Count > 1)
			{
				base.PrepareButton(vo, 2, this.ctaButtonList[vo.Uid][1], this.ctaLabelList[vo.Uid][1]);
			}
		}

		private void LinkToMoreVideos(UXButton btn)
		{
			this.screen.ShowMoreVideos();
		}

		private void LinkToDevNotes(UXButton btn)
		{
			this.eventManager.SendEvent(EventId.HolonetDevNotes, "featured");
			this.screen.OpenTab(HolonetControllerType.DevNotes);
		}

		public override void OnDestroyTab()
		{
			if (this.ctaButtonList != null)
			{
				foreach (KeyValuePair<string, List<UXButton>> current in this.ctaButtonList)
				{
					current.get_Value().Clear();
				}
			}
			this.ctaButtonList.Clear();
			this.ctaButtonList = null;
			if (this.ctaLabelList != null)
			{
				foreach (KeyValuePair<string, List<UXLabel>> current2 in this.ctaLabelList)
				{
					current2.get_Value().Clear();
				}
			}
			this.ctaLabelList.Clear();
			this.ctaLabelList = null;
			if (this.videoSummary != null)
			{
				this.videoSummary.Cleanup();
				this.videoSummary = null;
			}
			if (this.featureGrid != null)
			{
				this.featureGrid.Clear();
				this.featureGrid = null;
			}
			if (this.pageDotGrid != null)
			{
				this.pageDotGrid.Clear();
				this.pageDotGrid = null;
			}
			if (this.videoSummaryFeatured != null)
			{
				this.videoSummaryFeatured.Cleanup();
				this.videoSummaryFeatured = null;
			}
			Service.Get<EventManager>().UnregisterObserver(this, EventId.HolonetLeaderBoardUpdated);
			this.leadersPanelGrid.Clear();
			this.leadersPanelGrid.RepositionItems();
			this.leadersPanelGrid = null;
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			this.eventManager.UnregisterObserver(this, EventId.UIVideosViewComplete);
			this.eventManager.UnregisterObserver(this, EventId.UIVideosViewBegin);
			this.eventManager.UnregisterObserver(this, EventId.UIVideosQueryResponse);
			this.eventManager.UnregisterObserver(this, EventId.CrateInventoryUpdated);
			this.eventManager.UnregisterObserver(this, EventId.ScreenLoaded);
			this.eventManager.UnregisterObserver(this, EventId.ScreenClosing);
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			this.CancelDailyCrateParticleFXTimer();
		}

		private void FeaturedItemDotClicked(UXCheckbox checkbox, bool selected)
		{
			if (selected)
			{
				this.featureGrid.SmoothScrollToItem((int)checkbox.Tag);
			}
		}

		private void GoToLeaderBoardClicked(UXButton btn)
		{
			this.screen.Close(null);
			Service.Get<UXController>().HUD.OpenLeaderBoard();
		}

		public override void OnTabOpen()
		{
			base.OnTabOpen();
			this.featureSwipeTimer = 0f;
			this.nextAutoElement = null;
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			this.eventManager.RegisterObserver(this, EventId.UIVideosViewComplete);
			this.eventManager.RegisterObserver(this, EventId.UIVideosViewBegin);
			this.eventManager.RegisterObserver(this, EventId.UIVideosQueryResponse);
			this.eventManager.RegisterObserver(this, EventId.ScreenLoaded);
			this.eventManager.RegisterObserver(this, EventId.ScreenClosing);
			if (this.videoSummary != null)
			{
				this.videoSummary.PopulateDisplay();
			}
			this.eventManager.SendEvent(EventId.HolonetCommandCenterTab, "view");
			this.SetupDailyCratePanel();
		}

		public override void OnTabClose()
		{
			base.OnTabClose();
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			this.eventManager.UnregisterObserver(this, EventId.UIVideosViewComplete);
			this.eventManager.UnregisterObserver(this, EventId.UIVideosViewBegin);
			this.eventManager.UnregisterObserver(this, EventId.UIVideosQueryResponse);
			this.eventManager.UnregisterObserver(this, EventId.ScreenLoaded);
			this.eventManager.UnregisterObserver(this, EventId.ScreenClosing);
			this.CancelDailyCrateParticleFXTimer();
			this.HideDailyCrateParticleFX();
		}

		public void OnViewFrameTime(float dt)
		{
			UXElement centeredElement = this.featureGrid.GetCenteredElement();
			if (centeredElement == null)
			{
				return;
			}
			List<UXElement> elementList = this.pageDotGrid.GetElementList();
			int num = this.featureGrid.GetElementList().IndexOf(centeredElement);
			if (0 <= num && num < elementList.Count)
			{
				UXCheckbox uXCheckbox = (UXCheckbox)elementList[num];
				uXCheckbox.Selected = true;
			}
			if (this.previousCarouselIndex != num)
			{
				this.previousCarouselIndex = num;
				if (this.nextAutoElement == null)
				{
					string cookie = (centeredElement.Tag as CommandCenterVO).Uid + "|manual";
					this.eventManager.SendEvent(EventId.HolonetCommandCenterFeature, cookie);
				}
			}
			if (Service.Get<UserInputManager>().IsPressed() || (GameConstants.IsMakerVideoEnabled() && VideoPlayerKeepAlive.Instance.IsDisplayed()))
			{
				this.featureSwipeTimer = 0f;
				this.nextAutoElement = null;
			}
			else
			{
				this.featureSwipeTimer += dt;
			}
			float carouselAutoSwipe = (centeredElement.Tag as CommandCenterVO).CarouselAutoSwipe;
			float num2 = (carouselAutoSwipe > 0f) ? carouselAutoSwipe : GameConstants.HOLONET_FEATURE_CAROUSEL_AUTO_SWIPE;
			if (this.featureSwipeTimer >= num2)
			{
				this.featureSwipeTimer = 0f;
				this.nextAutoElement = this.featureGrid.ScrollToNextElement();
				if (this.nextAutoElement != null)
				{
					string cookie2 = (this.nextAutoElement.Tag as CommandCenterVO).Uid + "|auto";
					this.eventManager.SendEvent(EventId.HolonetCommandCenterFeature, cookie2);
				}
			}
			if (this.dailyCrate == null)
			{
				this.UpdateNextDailyCrateTimeLabel();
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.ScreenLoaded)
			{
				if (id == EventId.HolonetLeaderBoardUpdated)
				{
					this.MakeLeaderBoardExcerpt();
					return EatResponse.NotEaten;
				}
				if (id != EventId.ScreenClosing)
				{
					if (id != EventId.ScreenLoaded)
					{
						return EatResponse.NotEaten;
					}
				}
				else
				{
					if (this.dailyCrateParticleFX != null)
					{
						this.dailyCrateParticleFX.Visible = true;
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
			}
			else if (id <= EventId.UIVideosViewBegin)
			{
				if (id != EventId.UIVideosQueryResponse)
				{
					if (id != EventId.UIVideosViewBegin)
					{
						return EatResponse.NotEaten;
					}
					this.screen.ShowVideoPreView();
					return EatResponse.NotEaten;
				}
				else
				{
					KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>> keyValuePair = (KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>>)cookie;
					if (keyValuePair.get_Key() != VideoSummaryStyle.HolonetEmpty || this.videoSummary == null)
					{
						return EatResponse.NotEaten;
					}
					using (List<VideoSummaryData>.Enumerator enumerator = keyValuePair.get_Value().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							VideoSummaryData current = enumerator.Current;
							if (current == this.videoSummary.summaryData)
							{
								this.ShowLeaderboardInfo();
							}
						}
						return EatResponse.NotEaten;
					}
				}
			}
			else if (id != EventId.UIVideosViewComplete)
			{
				if (id == EventId.CrateInventoryUpdated)
				{
					InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
					this.dailyCrate = crates.GetDailyCrateIfAvailable();
					this.SetupDailyCratePanel();
					return EatResponse.NotEaten;
				}
				return EatResponse.NotEaten;
			}
			else
			{
				KeyValuePair<bool, string> keyValuePair2 = (KeyValuePair<bool, string>)cookie;
				if (keyValuePair2.get_Key())
				{
					this.screen.ShowVideoPostView(keyValuePair2.get_Value());
					return EatResponse.NotEaten;
				}
				this.screen.HideVideoPreView();
				return EatResponse.NotEaten;
			}
			ScreenBase highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<ScreenBase>();
			if (highestLevelScreen != this.screen)
			{
				this.HideDailyCrateParticleFX();
			}
			return EatResponse.NotEaten;
		}

		public void OnViewClockTime(float dt)
		{
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			SquadWarStatusType currentStatus = warManager.GetCurrentStatus();
			string id = "";
			int num = 0;
			switch (currentStatus)
			{
			case SquadWarStatusType.PhasePrep:
			{
				int num2 = this.endPrepPhaseTime - 1;
				this.endPrepPhaseTime = num2;
				num = num2;
				id = "WAR_BASE_PREP_TIME_REMAINING";
				break;
			}
			case SquadWarStatusType.PhasePrepGrace:
			{
				int num2 = this.endPrepGracePhaseTime - 1;
				this.endPrepGracePhaseTime = num2;
				num = num2;
				id = "WAR_BOARD_PREP_GRACE_PHASE";
				break;
			}
			case SquadWarStatusType.PhaseAction:
			{
				int num2 = this.endActionPhaseTime - 1;
				this.endActionPhaseTime = num2;
				num = num2;
				id = "WAR_BASE_ACTION_TIME_REMAINING";
				break;
			}
			case SquadWarStatusType.PhaseActionGrace:
			{
				int num2 = this.endActionGracePhaseTime - 1;
				this.endActionGracePhaseTime = num2;
				num = num2;
				id = "WAR_BOARD_ACTION_GRACE_PHASE";
				break;
			}
			case SquadWarStatusType.PhaseCooldown:
				this.SetupSquadWarInfo();
				return;
			}
			if (num < 0)
			{
				num = 0;
			}
			this.squadWarTimerLabelPrepAction.Text = this.lang.Get(id, new object[]
			{
				GameUtils.GetTimeLabelFromSeconds(num)
			});
		}

		public override string GetBITabName()
		{
			UXElement centeredElement = this.featureGrid.GetCenteredElement();
			string text = string.Empty;
			if (centeredElement != null && centeredElement.Tag != null)
			{
				text = "|" + (centeredElement.Tag as CommandCenterVO).Uid;
			}
			return "command_center" + text;
		}

		protected internal CommandCenterHolonetTab(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).CancelDailyCrateParticleFXTimer();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).FeaturedButton1Clicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).FeaturedButton2Clicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).FeaturedItemDotClicked((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).GetBITabName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).GoToLeaderBoardClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HideDailyCrateParticleFX();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HideViewUIElements();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).LinkToDevNotes((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).LinkToMoreVideos((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).MakeLeaderBoardExcerpt();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).MakeLeaderClone((LeaderboardController)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).MakeNoLeaderFoundClone();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnCenteredFinished((UXElement)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnCrateInfoModalClosed(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnDestroyTab();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnGetCrateClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnOpenCrateClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnSquadWarBtnClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnTabClose();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnTabOpen();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).QuarterButtonSetup((CommandCenterVO)GCHandledObjects.GCHandleToObject(*args), (UXTable)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).RefreshButtons((CommandCenterVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ScheduleDailyCrateParticleActivation((CrateVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetupDailyCratePanel();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetupSquadWarInfo();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ShowLeaderboardInfo();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((CommandCenterHolonetTab)GCHandledObjects.GCHandleToObject(instance)).UpdateNextDailyCrateTimeLabel();
			return -1L;
		}
	}
}
