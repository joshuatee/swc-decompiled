using StaRTS.Externals.BI;
using StaRTS.Externals.Maker;
using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.UX.Screens.Leaderboard;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Controls
{
	public class VideoSummaryDisplay : IEventObserver
	{
		private const string FEATURED_SUMMARY_LABEL = "MakerLabelFeaturedVideo";

		private const string FEATURED_SUMMARY_TITLE = "MakerLabelVideoTitle";

		private const string FEATURED_SUMMARY_PLAY = "MakerButtonPlayFeatured";

		private const string FEATURED_SUMMARY_TIME_PARENT = "MakerTime";

		private const string FEATURED_SUMMARY_TIME = "MakerLabelTime";

		private const string FEATURED_SUMMARY_COUNT = "MakerLabelViewCount";

		private const string FEATURED_SUMMARY_DATE = "MakerLabelUploadDate";

		private const string FEATURED_SUMMARY_AUTHOR = "MakerLabelUserName";

		private const string FEATURED_SUMMARY_AUTHOR_TYPE_LABEL = "MakerLabelUserType";

		private const string FEATURED_SUMMARY_AUTHOR_TYPE_BG = "MakerSpriteBGUserType";

		private const string FEATURED_SUMMARY_THUMB = "MakerVideoImageThumb";

		private const string FEATURED_MESSAGE_LABEL = "MakerLabelErrorMsgFeatured";

		private const string FEATURED_MESSAGE_BUTTON = "MakerButtonErrorRetryFeatured";

		private const string FEATURED_SUMMARY_SHARE_SQUAD = "MakerButtonSquad";

		private const string FEATURED_SUMMARY_SHARE_FB = "MakerButtonFB";

		private const string FEATURED_SUMMARY_SHARE_G = "MakerButtonGoogle";

		private const string FEATURED_SUMMARY_SHARE_ICON = "MakerSpriteIconBGSquadShareIcon";

		private const string SEARCH_SUMMARY_PLAY = "MakerButtonPlayMoreVideos";

		private const string SEARCH_SUMMARY_TIME = "MakerLabelTimeMoreVideos";

		private const string SEARCH_SUMMARY_TITLE = "MakerLabelVideoTitleMoreVideos";

		private const string SEARCH_SUMMARY_COUNT_DATE = "MakerLabelViewsDateMoreVideos";

		private const string SEARCH_SUMMARY_AUTHOR = "MakerLabelUserNameMoreVideos";

		private const string SEARCH_SUMMARY_AUTHOR_TYPE_LABEL = "MakerLabelUserTypeMoreVideos";

		private const string SEARCH_SUMMARY_AUTHOR_TYPE_BG = "MakerSpriteBGUserTypeMoreVideos";

		private const string SEARCH_SUMMARY_THUMB = "MakerImageThumbMoreVideos";

		private const string SEARCH_MESSAGE_LABEL = "MakerLabelErrorMsgMoreVideos";

		private const string SEARCH_MESSAGE_BUTTON = "MakerButtonErrorRetryMoreVideos";

		private const string SEARCH_SUMMARY_SHARE_SQUAD = "MakerButtonSquadMoreVideos";

		private const string SEARCH_SUMMARY_SHARE_FB = "MakerButtonFBMoreVideos";

		private const string SEARCH_SUMMARY_SHARE_G = "MakerButtonGoogleMoreVideos";

		private const string SEARCH_SUMMARY_SHARE_ICON = "MakerButtonSquadMoreVideos";

		private const string SEARCH_SUMMARY_BUTTON = "MakerButtonSlideOutMoreVideos";

		private const string SEARCH_SUMMARY_BUTTON_CONTAINER = "MakerContainerSlideOutShareMoreVideos";

		private const string SEARCH_SUMMARY_SOCIAL_HIDE_BUTTON = "MakerButtonSlideBackMoreVideos";

		private const string POST_VID_SUMMARY_REPLAY = "MakerButtonWatchAgain";

		private const string POST_VID_SUMMARY_TITLE = "MakerLabelTitlePostView";

		private const string POST_VID_SUMMARY_AUTHOR = "MakerLabelUsernamePostView";

		private const string POST_VID_SUMMARY_AUTHOR_TYPE_LABEL = "MakerLabelUserTypePostView";

		private const string POST_VID_SUMMARY_AUTHOR_TYPE_BG = "MakerBGUserTypePostView";

		private const string POST_VID_SHARE_SQUAD = "BMakerButtonSquadPostView";

		private const string POST_VID_SHARE_FACEBOOK = "CMakerButtonFBPostView";

		private const string POST_VID_SHARE_GOOGLE = "DMakerButtonGooglePostView";

		private const string POST_VID_SHARE_TABLE = "MakerTableShareButtonsPostView";

		private const string POST_VID_SHARE_ICON = "MakerSpriteIconSquadShareIcon";

		private const string CHAT_VID_SUMMARY_PLAY = "MakerButtonPlayChatBottom";

		private const string CHAT_VID_SUMMARY_TITLE = "MakerLabelVideoTitleChatBottom";

		private const string CHAT_VID_SUMMARY_AUTHOR = "MakerLabelUserNameChatBottom";

		private const string CHAT_VID_SUMMARY_COUNT_DATE = "MakerLabelViewsDateChatBottom";

		private const string CHAT_VID_SUMMARY_TIME_LABEL = "MakerLabelTimeChatBottom";

		private const string CHAT_VID_SUMMARY_THUMB_TEXTURE = "MakerImageThumbChatBottom";

		private const string CHAT_VID_SUMMARY_TIME = "MakerTimeChatBottom";

		private const string CHAT_VID_SUMMARY_AUTHOR_TYPE_LABEL = "MakerLabelUserTypeChatBottom";

		private const string CHAT_VID_SUMMARY_AUTHOR_TYPE_BG = "MakerSpriteBGUserTypeChatBottom";

		private const string CHAT_VID_SUMMARY_MESSAGE_LABEL = "MakerLabelErrorMsgChat";

		private const string CHAT_VID_SUMMARY_MESSAGE_BUTTON = "MakerButtonErrorRetryChat";

		private const string HOLONET_VID_SUMMARY_PLAY = "BtnFeaturedVideoPlay";

		private const string HOLONET_VID_SUMMARY_TITLE = "VideoPanelTitleLabel";

		private const string HOLONET_VID_SUMMARY_AUTHOR = "VideoPanelCreatorLabel";

		private const string HOLONET_VID_SUMMARY_AUTHOR_TYPE_LABEL = "VideoPanelLabelUserTypeMoreVideos";

		private const string HOLONET_VID_SUMMARY_AUTHOR_TYPE_BG = "VideoPanelBGUserTypeMoreVideos";

		private const string HOLONET_VID_SUMMARY_TIME = "VideoPanelLabelTimeHolo";

		private const string HOLONET_VID_SUMMARY_THUMB_TEXTURE = "VideoPanelImageTexture";

		private const string HOLONET_VID_SUMMARY_THUMB = "VideoClippingPanel";

		private const string HOLONET_VID_SUMMARY_MESSAGE_CONTAINER = "MakerContainerErrorMsgCC";

		private const string HOLONET_VID_SUMMARY_MESSAGE_BUTTON = "MakerButtonErrorRetryCC";

		private const string HOLONET_VID_SUMMARY_MESSAGE_LABEL = "MakerLabelErrorMsgCC";

		private const string HOLONET_FEATURED_VID_SUMMARY_PLAY = "NewsItemHalfBtnAction";

		private const string HOLONET_FEATURED_VID_SUMMARY_TITLE = "NewsItemHalfTitleLabel";

		private const string HOLONET_FEATURED_VID_SUMMARY_AUTHOR = "NewsItemHalfBodyLabel";

		private const string HOLONET_FEATURED_VID_SUMMARY_THUMB_TEXTURE = "NewsItemHalfImage";

		private const string LOADING_SPINNER = "WidgetLoadingSpinner";

		private const string LOADING_THUMB = "THUMB_SPINNER_";

		private const string HOLONET_SPINNER_POSTFIX = "holonet";

		private const string HN_UI_VIEWS = "hn_ui_views";

		private const string HN_UI_UPLOADED = "hn_ui_uploaded";

		private const string HN_UI_AUTHOR = "hn_ui_author";

		private const string HN_UI_AUTHOR_LINEBREAK = "hn_ui_author_linebreak";

		private const string HN_UI_DISPLAY_ERROR = "hn_ui_display_error";

		private const string HN_UI_NO_VIDEO = "hn_ui_no_video";

		private const string HN_UI_NO_RESULTS = "hn_ui_no_results";

		private const string SHARE_ICON_SPRITE_NAME = "icoShareIOS";

		private const float SPINNER_SCALE_X_LARGE = 2f;

		private const float SPINNER_SCALE_Y_LARGE = 2f;

		private const float SPINNER_SCALE_Z = 1f;

		private const float SPINNER_POS_X_LARGE = -175f;

		private const float SPINNER_POS_Y_LARGE = 75f;

		private const float SPINNER_POS_Z = 0f;

		private const float SPINNER_SCALE_X_MED = 1f;

		private const float SPINNER_SCALE_Y_MED = 1f;

		private const float SPINNER_POS_X_MED = -85f;

		private const float SPINNER_POS_Y_MED = 40f;

		private const float SPINNER_SCALE_X_SMALL = 0.75f;

		private const float SPINNER_SCALE_Y_SMALL = 0.75f;

		private const float SPINNER_POS_X_SMALL = -48f;

		private const float SPINNER_POS_Y_SMALL = 48f;

		private bool showViewCounts;

		private Dictionary<string, string> sourceBackgrounds;

		private ScreenBase parentScreen;

		private UXElement thumbSpinnerUI;

		private bool thumbSpinnerUIVisibile;

		private VideoSummaryData summaryData;

		private uint thumbSpinnerTimerId;

		private string playAction;

		private string sourceType;

		private bool inResetMode;

		private bool cleanedUp;

		public VideoSummaryDisplay(ScreenBase parentScreen, VideoSummaryData summaryData)
		{
			this.sourceBackgrounds = new Dictionary<string, string>
			{
				{
					"official",
					"MakerBgOfficial"
				}
			};
			base..ctor();
			this.parentScreen = parentScreen;
			this.summaryData = summaryData;
			this.thumbSpinnerUIVisibile = false;
			this.inResetMode = false;
			this.thumbSpinnerTimerId = 0u;
			this.cleanedUp = false;
		}

		private string GetUniqueNamePostfix()
		{
			if (!string.IsNullOrEmpty(this.summaryData.UILabel))
			{
				return " (" + this.summaryData.UILabel + ")";
			}
			return string.Empty;
		}

		private void ShowOrHideThumbnail(bool show)
		{
			string uniqueNamePostfix = this.GetUniqueNamePostfix();
			if (this.summaryData.Style == VideoSummaryStyle.Holonet)
			{
				this.parentScreen.GetElement<UXElement>("VideoClippingPanel").Visible = show;
				return;
			}
			if (this.summaryData.Style == VideoSummaryStyle.SquadChat)
			{
				this.parentScreen.GetElement<UXElement>("MakerTimeChatBottom" + uniqueNamePostfix).Visible = show;
			}
		}

		public void ReceivedThumbnail()
		{
			UXTexture uXTexture = null;
			switch (this.summaryData.Style)
			{
			case VideoSummaryStyle.Featured:
				uXTexture = this.parentScreen.GetElement<UXTexture>("MakerVideoImageThumb" + this.GetUniqueNamePostfix());
				break;
			case VideoSummaryStyle.Search:
				uXTexture = this.parentScreen.GetElement<UXTexture>("MakerImageThumbMoreVideos" + this.GetUniqueNamePostfix());
				break;
			case VideoSummaryStyle.PostView:
				uXTexture = this.parentScreen.GetElement<UXTexture>(this.summaryData.UILabel);
				break;
			case VideoSummaryStyle.SquadChat:
				uXTexture = this.parentScreen.GetElement<UXTexture>("MakerImageThumbChatBottom" + this.GetUniqueNamePostfix());
				break;
			case VideoSummaryStyle.Holonet:
				uXTexture = this.parentScreen.GetElement<UXTexture>("VideoPanelImageTexture");
				break;
			case VideoSummaryStyle.HolonetFeatured:
				uXTexture = this.parentScreen.GetElement<UXTexture>("NewsItemHalfImage" + this.GetUniqueNamePostfix());
				break;
			}
			if (uXTexture != null)
			{
				this.ShowOrHideThumbnail(true);
				uXTexture.MainTexture = this.summaryData.GetThumbnailImage();
				uXTexture.Visible = true;
				this.HideSearchSpinnerUI();
			}
			Service.Get<EventManager>().UnregisterObserver(this, EventId.UIVideosThumbnailResponse);
		}

		private UXElement CreateLoadingUI(UXElement parent, string name)
		{
			UXSprite holonetLoader = Service.Get<UXController>().MiscElementsManager.GetHolonetLoader(parent);
			switch (this.summaryData.Style)
			{
			case VideoSummaryStyle.Featured:
				holonetLoader.Root.transform.localPosition = Vector3.zero;
				break;
			case VideoSummaryStyle.Search:
			case VideoSummaryStyle.SquadChat:
			case VideoSummaryStyle.HolonetFeatured:
				holonetLoader.Root.transform.localPosition = Vector3.zero;
				break;
			case VideoSummaryStyle.Holonet:
				holonetLoader.Root.transform.localPosition = Vector3.zero;
				break;
			}
			return holonetLoader;
		}

		private void OnShowThumbSpinner(uint id, object cookie)
		{
			if (this.thumbSpinnerUI == null)
			{
				string name = "";
				string name2 = "";
				switch (this.summaryData.Style)
				{
				case VideoSummaryStyle.Featured:
					name = "MakerVideoImageThumb" + this.GetUniqueNamePostfix();
					name2 = this.summaryData.UILabel;
					break;
				case VideoSummaryStyle.Search:
					name = "MakerImageThumbMoreVideos" + this.GetUniqueNamePostfix();
					name2 = this.summaryData.UILabel;
					break;
				case VideoSummaryStyle.SquadChat:
					name = "MakerImageThumbChatBottom" + this.GetUniqueNamePostfix();
					name2 = this.summaryData.UILabel;
					break;
				case VideoSummaryStyle.Holonet:
					name = "VideoPanelImageTexture";
					name2 = "holonet";
					break;
				case VideoSummaryStyle.HolonetFeatured:
					name = "NewsItemHalfImage" + this.GetUniqueNamePostfix();
					name2 = this.summaryData.UILabel;
					break;
				}
				UXElement element = this.parentScreen.GetElement<UXElement>(name);
				this.thumbSpinnerUI = this.CreateLoadingUI(element, name2);
			}
			this.thumbSpinnerUI.Visible = this.thumbSpinnerUIVisibile;
			Service.Get<ViewTimerManager>().KillViewTimer(this.thumbSpinnerTimerId);
			this.thumbSpinnerTimerId = 0u;
		}

		private void ShowSearchSpinnerUI()
		{
			this.thumbSpinnerUIVisibile = true;
			if (this.thumbSpinnerTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.thumbSpinnerTimerId);
			}
			this.thumbSpinnerTimerId = Service.Get<ViewTimerManager>().CreateViewTimer(0.1f, true, new TimerDelegate(this.OnShowThumbSpinner), null);
		}

		private void HideSearchSpinnerUI()
		{
			this.thumbSpinnerUIVisibile = false;
			if (this.thumbSpinnerUI != null)
			{
				this.thumbSpinnerUI.Visible = this.thumbSpinnerUIVisibile;
			}
			Service.Get<ViewTimerManager>().KillViewTimer(this.thumbSpinnerTimerId);
			this.thumbSpinnerTimerId = 0u;
		}

		private void SetVideoLength(string elementName, int lengthInSeconds)
		{
			UXLabel element = this.parentScreen.GetElement<UXLabel>(elementName);
			StringBuilder stringBuilder = new StringBuilder();
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)lengthInSeconds);
			if (timeSpan.get_Hours() >= 1)
			{
				stringBuilder.Append(timeSpan.get_Hours().ToString("00"));
				stringBuilder.Append(":");
				stringBuilder.Append(timeSpan.get_Minutes().ToString("00"));
				stringBuilder.Append(":");
				stringBuilder.Append(timeSpan.get_Seconds().ToString("00"));
			}
			else
			{
				stringBuilder.Append(timeSpan.get_Minutes().ToString("00"));
				stringBuilder.Append(":");
				stringBuilder.Append(timeSpan.get_Seconds().ToString("00"));
			}
			element.Text = stringBuilder.ToString();
		}

		private void SetupPlayButton(string buttonName, string logName)
		{
			UXButton element = this.parentScreen.GetElement<UXButton>(buttonName);
			if (this.inResetMode)
			{
				element.OnClicked = null;
				element.Visible = false;
				return;
			}
			element.OnClicked = new UXButtonClickedDelegate(this.OnPlayClicked);
			element.Visible = true;
			this.playAction = logName;
		}

		private void SetupVideoLength(string labelName)
		{
			VideoData videoData = this.summaryData.GetVideoData();
			this.SetVideoLength(labelName, this.inResetMode ? 0 : videoData.Length);
		}

		private void SetupVideoTitle(string labelName)
		{
			VideoData videoData = this.summaryData.GetVideoData();
			UXLabel element = this.parentScreen.GetElement<UXLabel>(labelName);
			element.Text = (this.inResetMode ? "" : this.CleanseString(videoData.Title));
		}

		private void SetupViewCount(string labelName)
		{
			UXLabel element = this.parentScreen.GetElement<UXLabel>(labelName);
			if (this.inResetMode || !this.showViewCounts)
			{
				element.Text = "";
				return;
			}
			VideoData videoData = this.summaryData.GetVideoData();
			element.Text = Service.Get<Lang>().Get("hn_ui_views", new object[]
			{
				videoData.Viewcount.ToString()
			});
		}

		private void SetupVideoDate(string labelName)
		{
			UXLabel element = this.parentScreen.GetElement<UXLabel>(labelName);
			VideoData videoData = this.summaryData.GetVideoData();
			element.Text = (this.inResetMode ? "" : Service.Get<Lang>().Get("hn_ui_uploaded", new object[]
			{
				videoData.Timestamp.ToString("d")
			}));
		}

		private void SetupViewCountAndDate(string labelName)
		{
			UXLabel element = this.parentScreen.GetElement<UXLabel>(labelName);
			if (this.inResetMode)
			{
				element.Text = "";
				return;
			}
			if (!this.showViewCounts)
			{
				this.SetupVideoDate(labelName);
				return;
			}
			VideoData videoData = this.summaryData.GetVideoData();
			element.Text = this.GetViewsAndUploadedString(videoData.Viewcount.ToString(), videoData.Timestamp.ToString("d"));
		}

		private void SetupVideoCreator(string labelName, string langTag = null)
		{
			if (string.IsNullOrEmpty(labelName))
			{
				return;
			}
			UXLabel element = this.parentScreen.GetElement<UXLabel>(labelName);
			VideoData videoData = this.summaryData.GetVideoData();
			if (this.inResetMode || string.IsNullOrEmpty(videoData.Author))
			{
				element.Text = "";
				return;
			}
			if (langTag == null)
			{
				element.Text = videoData.Author;
				return;
			}
			element.Text = Service.Get<Lang>().Get(langTag, new object[]
			{
				videoData.Author
			});
		}

		private void SetupSourceType(string bgName, string labelName)
		{
			if (string.IsNullOrEmpty(this.sourceType) || this.inResetMode)
			{
				UXElement element = this.parentScreen.GetElement<UXElement>(bgName);
				element.Visible = false;
				this.parentScreen.GetElement<UXLabel>(labelName).Visible = false;
				return;
			}
			this.UpdateSourceType(this.sourceType);
		}

		private string GetVideoSharingSource()
		{
			if (this.summaryData.Style == VideoSummaryStyle.Search)
			{
				return "search";
			}
			if (this.summaryData.Style == VideoSummaryStyle.FeaturedEmpty)
			{
				return "featured";
			}
			if (this.summaryData.Style == VideoSummaryStyle.PostView)
			{
				return "postwatch";
			}
			return string.Empty;
		}

		private void TrackVideoSharing(VideoSharingNetwork network)
		{
			Service.Get<BILoggingController>().TrackVideoSharing(network, this.GetVideoSharingSource(), this.summaryData.Guid);
		}

		protected void ShowShareToSquad(string guid)
		{
			Service.Get<ScreenController>().AddScreen(new VideosShareSquadScreen(this.summaryData.Guid, this.GetVideoSharingSource()));
		}

		private void OnShareSquadButtonClicked(UXButton button)
		{
			if (Service.Get<SquadController>().StateManager.GetCurrentSquad() == null)
			{
				Service.Get<ScreenController>().AddScreen(new SquadJoinScreen());
				return;
			}
			this.ShowShareToSquad(this.summaryData.Guid);
		}

		private void OnShareFacebookButtonClicked(UXButton button)
		{
			VideoSharing.ShareVideo(VideoSharingNetwork.FACEBOOK, this.summaryData.Guid, null);
			this.TrackVideoSharing(VideoSharingNetwork.FACEBOOK);
		}

		private void OnShareGoogleButtonClicked(UXButton button)
		{
			VideoSharing.ShareVideo(VideoSharingNetwork.GOOGLEPLUS, this.summaryData.Guid, null);
			this.TrackVideoSharing(VideoSharingNetwork.GOOGLEPLUS);
		}

		private void SetupSharing(string shareButtonName, string fbButtonName, string googleButtonName)
		{
			this.SetupSharing(shareButtonName, "", fbButtonName, googleButtonName);
		}

		private void SetupSharing(string shareButtonName, string shareIconName, string fbButtonName, string googleButtonName)
		{
			UXButton element = this.parentScreen.GetElement<UXButton>(shareButtonName);
			element.OnClicked = new UXButtonClickedDelegate(this.OnShareSquadButtonClicked);
			if (!string.IsNullOrEmpty(shareIconName))
			{
				UXSprite element2 = this.parentScreen.GetElement<UXSprite>(shareIconName);
				element2.SpriteName = "icoShareIOS";
			}
			element = this.parentScreen.GetElement<UXButton>(fbButtonName);
			element.OnClicked = new UXButtonClickedDelegate(this.OnShareFacebookButtonClicked);
			element.Visible = false;
			element = this.parentScreen.GetElement<UXButton>(googleButtonName);
			element.OnClicked = new UXButtonClickedDelegate(this.OnShareGoogleButtonClicked);
			element.Visible = false;
			if (this.summaryData.Style == VideoSummaryStyle.Search)
			{
				string uniqueNamePostfix = this.GetUniqueNamePostfix();
				element = this.parentScreen.GetElement<UXButton>("MakerButtonSlideOutMoreVideos" + uniqueNamePostfix);
				UXElement element3 = this.parentScreen.GetElement<UXElement>("MakerContainerSlideOutShareMoreVideos" + uniqueNamePostfix);
				element.OnClicked = new UXButtonClickedDelegate(this.OnShareSquadButtonClicked);
				element3.Visible = false;
			}
			if (GameConstants.HOLONET_FEATURE_SHARE_ENABLED)
			{
				Service.Get<VideoDataManager>().GetVideoDetails(this.summaryData.Guid, new VideoDataManager.DataQueryCompleteDelegate(this.OnVideoDetails));
			}
		}

		private void OnVideoDetails(string guid)
		{
			if (guid == null || this.cleanedUp)
			{
				return;
			}
			VideoData videoData;
			if (!Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData) || videoData == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(videoData.YoutubeId))
			{
				string uniqueNamePostfix = this.GetUniqueNamePostfix();
				string text = "MakerButtonFB";
				if (this.summaryData.Style == VideoSummaryStyle.Search)
				{
					text = "MakerButtonFBMoreVideos";
					UXButton element = this.parentScreen.GetElement<UXButton>("MakerButtonSlideOutMoreVideos" + uniqueNamePostfix);
					UXElement element2 = this.parentScreen.GetElement<UXElement>("MakerContainerSlideOutShareMoreVideos" + uniqueNamePostfix);
					element.OnClicked = null;
					element2.Visible = true;
				}
				else if (this.summaryData.Style == VideoSummaryStyle.PostView)
				{
					text = "CMakerButtonFBPostView";
				}
				UXButton element3 = this.parentScreen.GetElement<UXButton>(text + uniqueNamePostfix);
				element3.Visible = true;
			}
			UXTable element4 = this.parentScreen.GetElement<UXTable>("MakerTableShareButtonsPostView");
			element4.RepositionItems();
		}

		private void GenerateSummaryFeatured()
		{
			string uniqueNamePostfix = this.GetUniqueNamePostfix();
			if (this.summaryData.Location > 0)
			{
				this.parentScreen.GetElement<UXLabel>("MakerLabelFeaturedVideo" + uniqueNamePostfix).Visible = false;
			}
			this.SetupPlayButton("MakerButtonPlayFeatured" + uniqueNamePostfix, "featured");
			this.SetupVideoLength("MakerLabelTime" + uniqueNamePostfix);
			this.SetupVideoTitle("MakerLabelVideoTitle" + uniqueNamePostfix);
			this.SetupViewCount("MakerLabelViewCount" + uniqueNamePostfix);
			this.SetupVideoDate("MakerLabelUploadDate" + uniqueNamePostfix);
			this.SetupVideoCreator(this.GetAuthorLabelName(), null);
			this.SetupSourceType("MakerSpriteBGUserType" + uniqueNamePostfix, "MakerLabelUserType" + uniqueNamePostfix);
			this.UpdateThumbnail();
			this.SetupSharing("MakerButtonSquad" + uniqueNamePostfix, "MakerSpriteIconBGSquadShareIcon" + uniqueNamePostfix, "MakerButtonFB" + uniqueNamePostfix, "MakerButtonGoogle" + uniqueNamePostfix);
		}

		private void DownloadThumbnail()
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.UIVideosThumbnailResponse);
			this.ShowSearchSpinnerUI();
			this.summaryData.DownloadThumbnail();
		}

		public void Cleanup()
		{
			if (this.thumbSpinnerTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.thumbSpinnerTimerId);
				this.thumbSpinnerTimerId = 0u;
			}
			Service.Get<EventManager>().UnregisterObserver(this, EventId.UIVideosThumbnailResponse);
			this.cleanedUp = true;
		}

		public void Recycle()
		{
			if (this.summaryData.Style == VideoSummaryStyle.Search)
			{
				UXButton element = this.parentScreen.GetElement<UXButton>("MakerButtonSlideBackMoreVideos" + this.GetUniqueNamePostfix());
				UIPlayTween component = element.NGUIButton.GetComponent<UIPlayTween>();
				component.Play(true);
			}
		}

		private string GetViewsAndUploadedString(string count, string date)
		{
			Lang lang = Service.Get<Lang>();
			return lang.Get("hn_ui_views", new object[]
			{
				count
			}) + " | " + lang.Get("hn_ui_uploaded", new object[]
			{
				date
			});
		}

		private void GenerateSummarySearch()
		{
			string uniqueNamePostfix = this.GetUniqueNamePostfix();
			this.SetupPlayButton("MakerButtonPlayMoreVideos" + uniqueNamePostfix, "search");
			this.SetupVideoLength("MakerLabelTimeMoreVideos" + uniqueNamePostfix);
			this.SetupVideoTitle("MakerLabelVideoTitleMoreVideos" + uniqueNamePostfix);
			this.SetupViewCountAndDate("MakerLabelViewsDateMoreVideos" + uniqueNamePostfix);
			this.SetupVideoCreator(this.GetAuthorLabelName(), null);
			this.SetupSourceType("MakerSpriteBGUserTypeMoreVideos" + uniqueNamePostfix, "MakerLabelUserTypeMoreVideos" + uniqueNamePostfix);
			UXTexture element = this.parentScreen.GetElement<UXTexture>("MakerImageThumbMoreVideos" + uniqueNamePostfix);
			element.MainTexture = null;
			this.UpdateThumbnail();
			this.SetupSharing("MakerButtonSquadMoreVideos" + uniqueNamePostfix, "MakerButtonFBMoreVideos" + uniqueNamePostfix, "MakerButtonGoogleMoreVideos" + uniqueNamePostfix);
		}

		private void GenerateSummaryPostView()
		{
			this.SetupPlayButton("MakerButtonWatchAgain", "postwatch");
			this.SetupVideoTitle("MakerLabelTitlePostView");
			this.SetupVideoCreator(this.GetAuthorLabelName(), "hn_ui_author");
			this.SetupSourceType("MakerBGUserTypePostView", "MakerLabelUserTypePostView");
			this.SetupSharing("BMakerButtonSquadPostView", "MakerSpriteIconSquadShareIcon", "CMakerButtonFBPostView", "DMakerButtonGooglePostView");
		}

		private void GenerateSummaryChat()
		{
			if (!this.inResetMode)
			{
				this.inResetMode = true;
				this.GenerateMessageSquadChat();
				this.inResetMode = false;
			}
			string uniqueNamePostfix = this.GetUniqueNamePostfix();
			this.SetupPlayButton("MakerButtonPlayChatBottom" + uniqueNamePostfix, "squad");
			this.SetupVideoLength("MakerLabelTimeChatBottom" + uniqueNamePostfix);
			this.SetupVideoTitle("MakerLabelVideoTitleChatBottom" + uniqueNamePostfix);
			this.SetupVideoCreator(this.GetAuthorLabelName(), null);
			this.SetupSourceType("MakerSpriteBGUserTypeChatBottom" + uniqueNamePostfix, "MakerLabelUserTypeChatBottom" + uniqueNamePostfix);
			this.SetupViewCountAndDate("MakerLabelViewsDateChatBottom" + uniqueNamePostfix);
			this.UpdateThumbnail();
		}

		private void GenerateSummaryHolonet()
		{
			this.SetupPlayButton("BtnFeaturedVideoPlay", "command_center");
			this.SetupVideoLength("VideoPanelLabelTimeHolo");
			this.SetupVideoTitle("VideoPanelTitleLabel");
			this.SetupVideoCreator(this.GetAuthorLabelName(), "hn_ui_author_linebreak");
			this.SetupSourceType("VideoPanelBGUserTypeMoreVideos", "VideoPanelLabelUserTypeMoreVideos");
			this.UpdateThumbnail();
		}

		private void GenerateSummaryHolonetFeatured()
		{
			string uniqueNamePostfix = this.GetUniqueNamePostfix();
			this.SetupPlayButton("NewsItemHalfBtnAction" + uniqueNamePostfix, "holonet");
			this.SetupVideoTitle("NewsItemHalfTitleLabel" + uniqueNamePostfix);
			this.SetupVideoCreator(this.GetAuthorLabelName(), null);
			this.UpdateThumbnail();
		}

		private void UpdateSourceTypeDisplay(string type, string bgName, string labelName)
		{
			string id;
			if (!Service.Get<VideoDataManager>().SourceTypes.TryGetValue(type, out id))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Video Source Type '{0}' not found in types", new object[]
				{
					type
				});
				return;
			}
			string spriteName;
			if (!this.sourceBackgrounds.TryGetValue(type, out spriteName))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Video Source Type '{0}' not found in backgrounds", new object[]
				{
					type
				});
				return;
			}
			string uniqueNamePostfix = this.GetUniqueNamePostfix();
			UXSprite element = this.parentScreen.GetElement<UXSprite>(bgName + uniqueNamePostfix);
			element.Visible = true;
			element.SpriteName = spriteName;
			UXLabel element2 = this.parentScreen.GetElement<UXLabel>(labelName + uniqueNamePostfix);
			element2.Text = Service.Get<Lang>().Get(id, new object[0]);
			element2.Visible = true;
			this.UpdateAuthorDisplay(type);
		}

		private void UpdateAuthorDisplay(string authorType)
		{
			string authorLabelName = this.GetAuthorLabelName();
			if (string.IsNullOrEmpty(authorLabelName))
			{
				return;
			}
			if (authorType == "official")
			{
				UXLabel element = this.parentScreen.GetElement<UXLabel>(authorLabelName);
				element.Text = "";
			}
		}

		private string GetAuthorLabelName()
		{
			switch (this.summaryData.Style)
			{
			case VideoSummaryStyle.Featured:
				return "MakerLabelUserName" + this.GetUniqueNamePostfix();
			case VideoSummaryStyle.Search:
				return "MakerLabelUserNameMoreVideos" + this.GetUniqueNamePostfix();
			case VideoSummaryStyle.PostView:
				return "MakerLabelUsernamePostView";
			case VideoSummaryStyle.SquadChat:
			case VideoSummaryStyle.SquadChatError:
				return "MakerLabelUserNameChatBottom" + this.GetUniqueNamePostfix();
			case VideoSummaryStyle.Holonet:
			case VideoSummaryStyle.HolonetError:
				return "VideoPanelCreatorLabel";
			case VideoSummaryStyle.HolonetFeatured:
			case VideoSummaryStyle.HolonetFeaturedError:
				return "NewsItemHalfBodyLabel" + this.GetUniqueNamePostfix();
			case VideoSummaryStyle.HolonetEmpty:
			case VideoSummaryStyle.SquadChatEmpty:
				return string.Empty;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("Video summary style {0} not handled", new object[]
			{
				this.summaryData.Style
			});
			return "";
		}

		private void UpdateThumbnail()
		{
			if (this.inResetMode)
			{
				this.ShowOrHideThumbnail(false);
				return;
			}
			this.DownloadThumbnail();
		}

		private string CleanseString(string input)
		{
			return input.Replace('▐', '|');
		}

		private void GenerateMessageFeatured()
		{
			string id = (this.summaryData.Style == VideoSummaryStyle.FeaturedError) ? "hn_ui_display_error" : "hn_ui_no_results";
			this.parentScreen.GetElement<UXLabel>("MakerLabelErrorMsgFeatured").Text = Service.Get<Lang>().Get(id, new object[0]);
			UXButton element = this.parentScreen.GetElement<UXButton>("MakerButtonErrorRetryFeatured");
			this.SetupRetryButton(element);
		}

		private void GenerateMessageSearch()
		{
			string id = (this.summaryData.Style == VideoSummaryStyle.SearchError) ? "hn_ui_display_error" : "hn_ui_no_results";
			this.parentScreen.GetElement<UXLabel>("MakerLabelErrorMsgMoreVideos").Text = Service.Get<Lang>().Get(id, new object[0]);
			UXButton element = this.parentScreen.GetElement<UXButton>("MakerButtonErrorRetryMoreVideos");
			this.SetupRetryButton(element);
		}

		private void GenerateMessageHolonet()
		{
			if (!this.inResetMode)
			{
				this.inResetMode = true;
				this.GenerateSummaryHolonet();
				this.inResetMode = false;
			}
			this.parentScreen.GetElement<UXElement>("MakerContainerErrorMsgCC").Visible = true;
			string id = (this.summaryData.Style == VideoSummaryStyle.HolonetError) ? "hn_ui_display_error" : "hn_ui_no_results";
			this.parentScreen.GetElement<UXLabel>("MakerLabelErrorMsgCC").Text = (this.inResetMode ? "" : Service.Get<Lang>().Get(id, new object[0]));
			UXButton element = this.parentScreen.GetElement<UXButton>("MakerButtonErrorRetryCC");
			this.SetupRetryButton(element);
		}

		private void GenerateMessageSquadChat()
		{
			if (!this.inResetMode)
			{
				this.inResetMode = true;
				this.GenerateSummaryChat();
				this.inResetMode = false;
			}
			string uniqueNamePostfix = this.GetUniqueNamePostfix();
			string id = (this.summaryData.Style == VideoSummaryStyle.SquadChatError) ? "hn_ui_display_error" : "hn_ui_no_results";
			UXLabel element = this.parentScreen.GetElement<UXLabel>("MakerLabelErrorMsgChat" + uniqueNamePostfix);
			element.Text = (this.inResetMode ? "" : Service.Get<Lang>().Get(id, new object[0]));
			UXButton element2 = this.parentScreen.GetElement<UXButton>("MakerButtonErrorRetryChat" + uniqueNamePostfix);
			this.SetupRetryButton(element2);
		}

		private bool IsEmptyMessage()
		{
			return this.summaryData.Style == VideoSummaryStyle.HolonetEmpty || this.summaryData.Style == VideoSummaryStyle.FeaturedEmpty || this.summaryData.Style == VideoSummaryStyle.SearchEmpty || this.summaryData.Style == VideoSummaryStyle.HolonetFeaturedEmpty || this.summaryData.Style == VideoSummaryStyle.SquadChatEmpty;
		}

		private void SetupRetryButton(UXButton button)
		{
			if (this.inResetMode || this.IsEmptyMessage())
			{
				button.OnClicked = null;
				button.Visible = false;
				return;
			}
			button.OnClicked = new UXButtonClickedDelegate(this.OnRetryDownload);
			button.Visible = true;
		}

		private void OnRetryDownload(UXButton button)
		{
			Service.Get<EventManager>().SendEvent(EventId.UIVideosRefresh, this);
		}

		public void ShowLoading()
		{
			this.inResetMode = true;
			this.GenerateOrResetSummary();
			this.inResetMode = false;
		}

		public string GetGuid()
		{
			return this.summaryData.Guid;
		}

		public void GenerateSummary()
		{
			this.inResetMode = false;
			this.GenerateOrResetSummary();
		}

		public void UpdateSourceType(string type)
		{
			this.sourceType = type;
			if (!this.summaryData.IsVisible)
			{
				return;
			}
			string text = null;
			string labelName = null;
			switch (this.summaryData.Style)
			{
			case VideoSummaryStyle.Featured:
				text = "MakerSpriteBGUserType";
				labelName = "MakerLabelUserType";
				break;
			case VideoSummaryStyle.Search:
				text = "MakerSpriteBGUserTypeMoreVideos";
				labelName = "MakerLabelUserTypeMoreVideos";
				break;
			case VideoSummaryStyle.PostView:
				text = "MakerBGUserTypePostView";
				labelName = "MakerLabelUserTypePostView";
				break;
			case VideoSummaryStyle.SquadChat:
				text = "MakerSpriteBGUserTypeChatBottom";
				labelName = "MakerLabelUserTypeChatBottom";
				break;
			case VideoSummaryStyle.Holonet:
				text = "VideoPanelBGUserTypeMoreVideos";
				labelName = "VideoPanelLabelUserTypeMoreVideos";
				break;
			}
			if (text != null)
			{
				this.UpdateSourceTypeDisplay(type, text, labelName);
			}
		}

		private void GenerateOrResetSummary()
		{
			switch (this.summaryData.Style)
			{
			case VideoSummaryStyle.Featured:
				this.GenerateSummaryFeatured();
				return;
			case VideoSummaryStyle.Search:
				this.GenerateSummarySearch();
				return;
			case VideoSummaryStyle.PostView:
				this.GenerateSummaryPostView();
				return;
			case VideoSummaryStyle.SquadChat:
				this.GenerateSummaryChat();
				return;
			case VideoSummaryStyle.Holonet:
				this.GenerateSummaryHolonet();
				return;
			case VideoSummaryStyle.HolonetFeatured:
				this.GenerateSummaryHolonetFeatured();
				return;
			case VideoSummaryStyle.FeaturedError:
			case VideoSummaryStyle.FeaturedEmpty:
				this.GenerateMessageFeatured();
				return;
			case VideoSummaryStyle.SearchError:
			case VideoSummaryStyle.SearchEmpty:
				this.GenerateMessageSearch();
				return;
			case VideoSummaryStyle.HolonetError:
			case VideoSummaryStyle.HolonetEmpty:
				this.GenerateMessageHolonet();
				return;
			case VideoSummaryStyle.HolonetFeaturedError:
			case VideoSummaryStyle.HolonetFeaturedEmpty:
				break;
			case VideoSummaryStyle.SquadChatError:
			case VideoSummaryStyle.SquadChatEmpty:
				this.GenerateMessageSquadChat();
				break;
			default:
				return;
			}
		}

		private void OnPlayClicked(UXButton button)
		{
			if (!string.IsNullOrEmpty(this.playAction))
			{
				Service.Get<BILoggingController>().TrackGameAction("video_start", this.playAction, this.summaryData.Guid, null);
			}
			GameStateMachine gameStateMachine = Service.Get<GameStateMachine>();
			if (gameStateMachine.CurrentState is WarBoardState)
			{
				SquadWarScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<SquadWarScreen>();
				if (highestLevelScreen != null)
				{
					highestLevelScreen.CloseSquadWarScreen(new TransitionCompleteDelegate(this.OnHomeStateTransitionComplete));
					return;
				}
			}
			else
			{
				this.summaryData.PlayVideo(this.playAction);
			}
		}

		private void OnHomeStateTransitionComplete()
		{
			this.summaryData.PlayVideo(this.playAction);
		}

		private void OnQueryRetry(UXButton button)
		{
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.UIVideosThumbnailResponse)
			{
				VideoSummaryData videoSummaryData = (VideoSummaryData)cookie;
				if (videoSummaryData == this.summaryData)
				{
					this.ReceivedThumbnail();
				}
			}
			return EatResponse.NotEaten;
		}

		protected internal VideoSummaryDisplay(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).CleanseString(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).CreateLoadingUI((UXElement)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).DownloadThumbnail();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateMessageFeatured();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateMessageHolonet();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateMessageSearch();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateMessageSquadChat();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateOrResetSummary();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateSummary();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateSummaryChat();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateSummaryFeatured();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateSummaryHolonet();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateSummaryHolonetFeatured();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateSummaryPostView();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GenerateSummarySearch();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GetAuthorLabelName());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GetGuid());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GetUniqueNamePostfix());
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GetVideoSharingSource());
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).GetViewsAndUploadedString(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).HideSearchSpinnerUI();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).IsEmptyMessage());
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).OnHomeStateTransitionComplete();
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).OnPlayClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).OnQueryRetry((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).OnRetryDownload((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).OnShareFacebookButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).OnShareGoogleButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).OnShareSquadButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).OnVideoDetails(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).ReceivedThumbnail();
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).Recycle();
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupPlayButton(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupRetryButton((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupSharing(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupSharing(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupSourceType(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupVideoCreator(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupVideoDate(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupVideoLength(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupVideoTitle(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupViewCount(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetupViewCountAndDate(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).SetVideoLength(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).ShowLoading();
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).ShowOrHideThumbnail(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).ShowSearchSpinnerUI();
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).ShowShareToSquad(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).TrackVideoSharing((VideoSharingNetwork)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).UpdateAuthorDisplay(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).UpdateSourceType(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).UpdateSourceTypeDisplay(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(instance)).UpdateThumbnail();
			return -1L;
		}
	}
}
