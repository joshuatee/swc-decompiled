using StaRTS.Externals.Maker;
using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet
{
	public class VideosHolonetTab : AbstractHolonetTab, IEventObserver, IViewFrameTimeObserver
	{
		private const string POST_VID_GROUP = "MakerPostViewGroup";

		private const string POST_VID_CLOSE = "MakerButtonBackPostView";

		private const string FEATURED_GROUP = "MakerFeaturedGroup";

		private const string FEATURED_MESSAGE_CONTAINER = "MakerContainerErrorMsgFeatured";

		private const string FEATURED_SUMMARY = "MakerVideosCard";

		private const string FEATURED_SUMMARY_PARENT = "MakerVideosContainer";

		private const string FEATURED_SUMMARY_GRID = "MakerVideosGrid";

		private const string FEATURED_SUMMARY_LABEL = "MakerLabelFeaturedVideo";

		private const string FEATURED_SUMMARY_SHARE_LABEL = "MakerLabelShare";

		private const string SEARCH_CONTAINER = "MakerSearchContainer";

		private const string SEARCH_INPUT = "MakerLabelSearchInput";

		private const string SEARCH_TAB = "MakerButtonMoreVideos";

		private const string MORE_VIDEOS_LABLE = "MakerLabelMoreVideos";

		private const string MAKER_CLOSE_BUTTON = "MakerButtonClose";

		private const string SPINNER_PREFIX = "VIDEOS_LOADING_";

		private const string SPINNER_POSTFIX_SEARCH = "search";

		private const string SPINNER_POSTFIX_FEATURED = "featured";

		private const string SEARCH_SUMMARY = "MakerCardMoreVideos";

		private const string SEARCH_SUMMARY_PARENT = "MakerContainerMoreVideos";

		private const string SEARCH_SUMMARY_GRID = "MakerGridMoreVideos";

		private const string SEARCH_GROUP = "MakerMoreVideosGroup";

		private const string SEARCH_BACK = "MakerButtonBackMoreVideos";

		private const string SEARCH_MESSAGE_CONTAINER = "MakerContainerErrorMsgMoreVideos";

		private const string SEARCH_MESSAGE_LABEL = "MakerLabelErrorMsgMoreVideos";

		private const string SEARCH_FILTERS_CONTAINER = "MakerContainerFilters";

		private const string SEARCH_SUBFILTERS_CONTAINER = "MakerContainerSubFilters";

		private const string SEARCH_FILTERS_GRID = "MakerGridFiltersTop";

		private const string SEARCH_FILTER_CARD = "MakerCardFiltersTop";

		private const string SEARCH_FILTER_BUTTON = "MakerButtonFiltersTop";

		private const string SEARCH_FILTER_LABEL = "MakerLabelFilter";

		private const string SEARCH_FILTER_CHOICE_BUTTON = "MakerButtonSubFilter";

		private const string SEARCH_FILTER_CHOICE_LABEL = "MakerLabelSubFilter";

		private const string SEARCH_FILTER_CHOICE_CARD = "MakerCardSubFilter";

		private const string SEARCH_FILTER_CHOICE_GRID = "MakerGridSubFilters";

		private const string SEARCH_FITLER_CHOICE_BG = "MakerContainerBGSubFilters";

		private const string SEARCH_FILTER_PICKER = "MakerContainerHQLevelSubFilter";

		private const string SEARCH_FILTER_PICKER_LABEL = "MakerLabelTitleHQLevel";

		private const string SEARCH_FILTER_PICKER_DECREMENT_BUTTON = "MakerCounterButtonDecrement";

		private const string SEARCH_FILTER_PICKER_INCREMENT_BUTTON = "MakerCounterButtonIncrement";

		private const string SEARCH_FILTER_PICKER_VALUE = "MakerLabelCounterValueHQLevel";

		private const string SEARCH_FILTER_PICKER_SET_BUTTON = "MakerButtonSetHQLevel";

		private const string SEARCH_FILTER_PICKER_SET_LABEL = "MakerLabelSetHQLevel";

		private const string SEARCH_FILTER_SCRIM_PREFIX = "MakerSpriteBGDimSubFilters";

		private const string POST_VID_MORE_VIDEOS_BTN = "BtnMoreVideosPostView";

		private const string POST_VID_MORE_VIDEOS_LABEL = "BtnLabelMoreVideosPostView";

		private const string MAKER_LABEL_WATCH_AGAIN = "MakerLabelWatchAgain";

		private const string AMAKER_LABEL_SHARE_POST_VIEW = "AMakerLabelSharePostView";

		private const string LOADING_SPINNER = "WidgetLoadingSpinner";

		private const string HN_UI_FEATURED_VIDEO = "hn_ui_featured_video";

		private const string HN_UI_SHARE = "hn_ui_share";

		private const string MORE_VIDEOS = "MORE_VIDEOS";

		private const string HN_UI_DISPLAY_ERROR = "hn_ui_display_error";

		private const string HN_UI_WATCH_AGAIN = "hn_ui_watch_again";

		private const string HN_UI_SEARCH = "s_Search";

		private const string HN_UI_SET = "s_Set";

		private const int NUM_VIDS_FEATURED = 4;

		private const int NUM_VIDS_DISPLAYED = 8;

		private const int NUM_VIDS_LOADED_MAX = 1000;

		private const int DROPDOWN_CHOICE_HEIGHT = 80;

		private const byte SCROLL_ARROWS_UPDATE_DELAY = 1;

		private UXButton backToFeatured;

		private UXButton toMoreVideos;

		private UXLabel moreVideosLabel;

		private VideosFeatured featuredPage;

		private VideosSearch searchPage;

		private VideosPostView postViewPage;

		private UXButton shareCloseButton;

		private UXElement searchSpinnerUI;

		private bool searchSpinnerUIVisibility;

		private UXElement featuredSpinnerUI;

		private bool featuredSpinnerUIVisibility;

		private List<UXGrid> grids;

		private PageType lastPage;

		private Dictionary<UXButton, ChoiceListUI> filters;

		private UXInput searchInput;

		private DynamicScrollingList searchGridDynamic;

		private Dictionary<int, VideoSummaryDisplay> videoSummaries;

		private ScreenBase parentScreen;

		private bool isOpen;

		private UXElement searchContainer;

		private uint searchSpinnerTimerId;

		private uint featuredSpinnerTimerId;

		private HolonetAnimationController anims;

		private HolonetVideoTabState currentState;

		private uint showScrollArrowsTimerId;

		public VideosHolonetTab(HolonetScreen screen, HolonetControllerType holonetControllerType, HolonetAnimationController anims) : base(screen, holonetControllerType)
		{
			base.InitializeTab("MakerTab", "hn_ui_videos");
			this.anims = anims;
			this.parentScreen = screen;
			this.isOpen = false;
			this.currentState = HolonetVideoTabState.Invalid;
			this.videoSummaries = new Dictionary<int, VideoSummaryDisplay>();
			this.lastPage = PageType.NONE;
			this.featuredSpinnerUIVisibility = false;
			this.searchSpinnerUIVisibility = false;
			this.searchSpinnerTimerId = 0u;
			this.featuredSpinnerTimerId = 0u;
			this.grids = new List<UXGrid>();
			this.showScrollArrowsTimerId = 0u;
			this.parentScreen.GetElement<UXLabel>("MakerLabelErrorMsgMoreVideos").Text = this.lang.Get("hn_ui_display_error", new object[0]);
		}

		private void SetupDropdownChoice(ChoiceData queryChoiceData)
		{
			StringBuilder stringBuilder = new StringBuilder("MakerCardFiltersTop");
			stringBuilder.Append("_");
			stringBuilder.Append(queryChoiceData.Id);
			string text = stringBuilder.ToString();
			UXGrid element = this.parentScreen.GetElement<UXGrid>("MakerGridFiltersTop");
			this.grids.Add(element);
			UXElement element2 = this.parentScreen.GetElement<UXElement>("MakerCardFiltersTop");
			UXElement item = this.parentScreen.CloneElement<UXElement>(element2, text, element.Root);
			element.AddItem(item, queryChoiceData.Id);
			element.RepositionItems();
			UXButton subElement = element.GetSubElement<UXButton>(text, "MakerSpriteBGDimSubFilters");
			subElement.OnClicked = new UXButtonClickedDelegate(this.CloseAllDropdowns);
			stringBuilder = new StringBuilder(" (");
			stringBuilder.Append(text);
			stringBuilder.Append(")");
			string str = stringBuilder.ToString();
			if (queryChoiceData.UIType == ChoiceType.SimpleList)
			{
				this.parentScreen.DestroyElement(this.parentScreen.GetElement<UXElement>("MakerContainerHQLevelSubFilter" + str));
				UXElement element3 = this.parentScreen.GetElement<UXElement>("MakerContainerBGSubFilters" + str);
				UIWidget component = element3.Root.GetComponent<UIWidget>();
				component.SetDimensions(component.width, 80 * queryChoiceData.Choices.Count);
			}
			else
			{
				this.parentScreen.DestroyElement(this.parentScreen.GetElement<UXElement>("MakerContainerBGSubFilters" + str));
				UXLabel element4 = this.parentScreen.GetElement<UXLabel>("MakerLabelTitleHQLevel" + str);
				element4.Text = queryChoiceData.UILabel;
				UXButton element5 = this.parentScreen.GetElement<UXButton>("MakerCounterButtonDecrement" + str);
				element5.OnClicked = new UXButtonClickedDelegate(this.OnFilterPickerClicked);
				element5.Tag = PickerButtonType.Decrement;
				UXButton element6 = this.parentScreen.GetElement<UXButton>("MakerCounterButtonIncrement" + str);
				element6.OnClicked = new UXButtonClickedDelegate(this.OnFilterPickerClicked);
				element6.Tag = PickerButtonType.Increment;
				UXButton element7 = this.parentScreen.GetElement<UXButton>("MakerButtonSetHQLevel" + str);
				element7.OnClicked = new UXButtonClickedDelegate(this.OnFilterPickerClicked);
				element7.Tag = PickerButtonType.SetType;
				UXLabel element8 = this.parentScreen.GetElement<UXLabel>("MakerLabelSetHQLevel" + str);
				element8.Text = this.lang.Get("s_Set", new object[0]);
			}
			UXElement element9 = this.parentScreen.GetElement<UXElement>("MakerContainerSubFilters" + str);
			element9.Visible = false;
			UXButton element10 = this.parentScreen.GetElement<UXButton>("MakerButtonFiltersTop" + str);
			element10.OnClicked = new UXButtonClickedDelegate(this.OnFilterButtonClicked);
			element10.Tag = queryChoiceData.UIType;
			UXLabel element11 = this.parentScreen.GetElement<UXLabel>("MakerLabelFilter" + str);
			element11.Text = queryChoiceData.UILabel;
			ChoiceListUI value = default(ChoiceListUI);
			value.buttonLabel = element11;
			value.choices = new List<ChoiceUI>();
			value.filterOptions = element9;
			value.id = queryChoiceData.Id;
			UXGrid element12 = this.parentScreen.GetElement<UXGrid>("MakerGridSubFilters" + str);
			value.filterGrid = element12;
			this.filters[element10] = value;
			for (int i = 0; i < queryChoiceData.Choices.Count; i++)
			{
				if (queryChoiceData.UIType == ChoiceType.SimpleList)
				{
					stringBuilder = new StringBuilder("MakerCardSubFilter");
					stringBuilder.Append("_");
					stringBuilder.Append(queryChoiceData.Id);
					stringBuilder.Append("_");
					stringBuilder.Append(i);
					string text2 = stringBuilder.ToString();
					this.grids.Add(element12);
					UXElement element13 = this.parentScreen.GetElement<UXElement>("MakerCardSubFilter");
					UXElement item2 = this.parentScreen.CloneElement<UXElement>(element13, text2, element12.Root);
					element12.AddItem(item2, element12.Count);
					stringBuilder = new StringBuilder("MakerButtonSubFilter");
					stringBuilder.Append(" (");
					stringBuilder.Append(text2);
					stringBuilder.Append(")");
					VideoFilterOption videoFilterOption = queryChoiceData.Choices[i];
					UXButton element14 = this.parentScreen.GetElement<UXButton>(stringBuilder.ToString());
					element14.OnClicked = new UXButtonClickedDelegate(this.OnFilterChoiceClicked);
					stringBuilder = new StringBuilder("MakerLabelSubFilter");
					stringBuilder.Append(" (");
					stringBuilder.Append(text2);
					stringBuilder.Append(")");
					UXLabel element15 = this.parentScreen.GetElement<UXLabel>(stringBuilder.ToString());
					if (videoFilterOption.UILabel != string.Empty)
					{
						element15.Text = videoFilterOption.UILabel;
					}
					ChoiceUI item3 = new ChoiceUI(element14, videoFilterOption.UILabel, videoFilterOption.Id);
					value.choices.Add(item3);
				}
				else
				{
					VideoFilterOption videoFilterOption2 = queryChoiceData.Choices[i];
					if (videoFilterOption2.Id == this.searchPage.GetFirstFilterLocation())
					{
						UXLabel element16 = this.parentScreen.GetElement<UXLabel>("MakerLabelCounterValueHQLevel" + str);
						element16.Text = videoFilterOption2.UILabel;
					}
					UXButton element17 = this.parentScreen.GetElement<UXButton>("MakerCounterButtonDecrement" + str);
					ChoiceUI item4 = new ChoiceUI(element17, videoFilterOption2.UILabel, videoFilterOption2.Id);
					value.choices.Add(item4);
					element17 = this.parentScreen.GetElement<UXButton>("MakerCounterButtonIncrement" + str);
					item4 = new ChoiceUI(element17, videoFilterOption2.UILabel, videoFilterOption2.Id);
					value.choices.Add(item4);
					element17 = this.parentScreen.GetElement<UXButton>("MakerButtonSetHQLevel" + str);
					item4 = new ChoiceUI(element17, videoFilterOption2.UILabel, videoFilterOption2.Id);
					value.choices.Add(item4);
				}
			}
		}

		private void CloseAllDropdowns(UXButton button)
		{
			foreach (ChoiceListUI current in this.filters.Values)
			{
				current.filterOptions.Visible = false;
			}
		}

		private bool CanUpdateVideoSummaryStyle(VideoSummaryStyle style)
		{
			switch (style)
			{
			case VideoSummaryStyle.Featured:
			case VideoSummaryStyle.Search:
			case VideoSummaryStyle.PostView:
			case VideoSummaryStyle.FeaturedError:
			case VideoSummaryStyle.SearchError:
			case VideoSummaryStyle.FeaturedEmpty:
			case VideoSummaryStyle.SearchEmpty:
				return true;
			}
			return false;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			switch (id)
			{
			case EventId.UIVideosQueryResponse:
			{
				KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>> keyValuePair = (KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>>)cookie;
				if (this.CanUpdateVideoSummaryStyle(keyValuePair.Key))
				{
					foreach (VideoSummaryData current in keyValuePair.Value)
					{
						this.videoSummaries[current.Location] = new VideoSummaryDisplay(this.parentScreen, current);
						switch (current.Style)
						{
						case VideoSummaryStyle.Featured:
							this.SetupVideoSummaryFeatured(current);
							continue;
						case VideoSummaryStyle.Search:
							this.SetupVideoSummarySearch(current);
							continue;
						case VideoSummaryStyle.PostView:
							this.SetupVideoSummaryPostView(current);
							continue;
						case VideoSummaryStyle.FeaturedError:
						case VideoSummaryStyle.FeaturedEmpty:
							this.SetupMessageFeatured(current);
							continue;
						case VideoSummaryStyle.SearchError:
						case VideoSummaryStyle.SearchEmpty:
							this.SetupMessageSearch(current);
							continue;
						}
						Service.Get<Logger>().ErrorFormat("VideosHolonetTab UIVideosQueryResponse unhandled type {0}", new object[]
						{
							current.Style
						});
					}
				}
				break;
			}
			case EventId.UIVideosFeaturedStart:
				this.parentScreen.GetElement<UXLabel>("MakerLabelFeaturedVideo").Text = this.lang.Get("hn_ui_featured_video", new object[0]);
				this.parentScreen.GetElement<UXLabel>("MakerLabelShare").Text = this.lang.Get("hn_ui_share", new object[0]);
				for (int i = 1; i <= 4; i++)
				{
					string name = "MakerVideosCard_" + i;
					UXElement optionalElement = this.parentScreen.GetOptionalElement<UXElement>(name);
					if (optionalElement != null)
					{
						this.parentScreen.DestroyElement(optionalElement);
					}
				}
				this.parentScreen.GetElement<UXElement>("MakerContainerErrorMsgFeatured").Visible = false;
				this.ShowFeaturedSpinnerUI();
				break;
			case EventId.UIVideosQueryStart:
				foreach (VideoSummaryDisplay current2 in this.videoSummaries.Values)
				{
					current2.Cleanup();
				}
				this.videoSummaries.Clear();
				this.searchGridDynamic.RemoveItems();
				this.parentScreen.GetElement<UXElement>("MakerContainerErrorMsgMoreVideos").Visible = false;
				this.ShowSearchSpinnerUI();
				break;
			case EventId.UIVideosQueryFilter:
				this.SetupDropdownChoice((ChoiceData)cookie);
				break;
			case EventId.UIVideosViewBegin:
				this.ShowPreViewPage(false);
				break;
			case EventId.UIVideosViewComplete:
			{
				KeyValuePair<bool, string> keyValuePair2 = (KeyValuePair<bool, string>)cookie;
				if (keyValuePair2.Key)
				{
					this.ShowPostViewPage(keyValuePair2.Value);
				}
				else
				{
					this.HidePreViewPage();
				}
				break;
			}
			case EventId.UIVideosRefresh:
				if (this.currentState == HolonetVideoTabState.Search && this.searchPage != null)
				{
					this.searchPage.PerformSearch();
				}
				else if (this.currentState == HolonetVideoTabState.Featured && this.featuredPage != null)
				{
					this.featuredPage.DoFeaturedQuery();
				}
				break;
			case EventId.UIVideosSourceTypeResponse:
			{
				KeyValuePair<List<string>, string> keyValuePair3 = (KeyValuePair<List<string>, string>)cookie;
				foreach (VideoSummaryDisplay current3 in this.videoSummaries.Values)
				{
					if (keyValuePair3.Key.Contains(current3.GetGuid()))
					{
						current3.UpdateSourceType(keyValuePair3.Value);
					}
				}
				break;
			}
			case EventId.UIDynamicScrollingListCreateItem:
			{
				ListItemCreateData listItemCreateData = (ListItemCreateData)cookie;
				UXElement itemUI = listItemCreateData.ParentList.GetItemUI(listItemCreateData.Location);
				VideoSummaryData oldVideoThumb = (VideoSummaryData)listItemCreateData.ParentList.GetItemCookie(listItemCreateData.OldLocation);
				UXElement item = this.CreateDynamicScrollingListItem((VideoSummaryData)listItemCreateData.Cookie, listItemCreateData.ParentList, itemUI, oldVideoThumb);
				listItemCreateData.ParentList.SetItemUI(item, listItemCreateData.Location);
				break;
			}
			case EventId.UIDynamicScrollingListCleanupItem:
			{
				VideoSummaryData videoSummaryData = (VideoSummaryData)cookie;
				VideoSummaryDisplay videoSummaryDisplay;
				this.videoSummaries.TryGetValue(videoSummaryData.Location, out videoSummaryDisplay);
				if (videoSummaryDisplay != null)
				{
					videoSummaryDisplay.Recycle();
				}
				break;
			}
			}
			return EatResponse.NotEaten;
		}

		private void SetupVideoSummaryFeatured(VideoSummaryData videoThumb)
		{
			UXElement uXElement = this.parentScreen.GetElement<UXElement>("MakerVideosCard");
			UXGrid element = this.parentScreen.GetElement<UXGrid>("MakerVideosGrid");
			uXElement = this.parentScreen.CloneElement<UXElement>(uXElement, videoThumb.UILabel, element.Root);
			this.videoSummaries[videoThumb.Location].GenerateSummary();
			uXElement.Visible = true;
			this.HideFeaturedSpinnerUI();
			this.grids.Add(element);
			element.AddItem(uXElement, videoThumb.Location);
			element.RepositionItems();
		}

		private void SetupMessageFeatured(VideoSummaryData videoThumb)
		{
			this.parentScreen.GetElement<UXElement>("MakerContainerErrorMsgFeatured").Visible = true;
			this.videoSummaries[videoThumb.Location].GenerateSummary();
			this.HideFeaturedSpinnerUI();
		}

		private void SetupVideoSummarySearch(VideoSummaryData videoThumb)
		{
			videoThumb.IsVisible = false;
			this.searchGridDynamic.AddItem(videoThumb, this.searchGridDynamic.Count);
			this.HideSearchSpinnerUI();
		}

		private void SetupMessageSearch(VideoSummaryData videoThumb)
		{
			this.parentScreen.GetElement<UXElement>("MakerContainerErrorMsgMoreVideos").Visible = true;
			this.videoSummaries[videoThumb.Location].GenerateSummary();
			this.HideSearchSpinnerUI();
		}

		private UXElement CreateDynamicScrollingListItem(VideoSummaryData videoThumb, DynamicScrollingList list, UXElement existingItem, VideoSummaryData oldVideoThumb)
		{
			string text = string.Empty;
			UXElement uXElement;
			if (existingItem != null)
			{
				videoThumb.UILabel = oldVideoThumb.UILabel;
				text = videoThumb.UILabel;
				existingItem.SetRootName(text);
				uXElement = existingItem;
				oldVideoThumb.IsVisible = false;
			}
			else
			{
				text = videoThumb.UILabel;
				uXElement = this.parentScreen.GetElement<UXElement>("MakerCardMoreVideos");
				uXElement = this.parentScreen.CloneElement<UXElement>(uXElement, text, list.Root);
			}
			videoThumb.IsVisible = true;
			this.videoSummaries[videoThumb.Location].GenerateSummary();
			return uXElement;
		}

		private void SetupVideoSummaryPostView(VideoSummaryData videoThumb)
		{
			VideoSummaryDisplay videoSummaryDisplay = this.videoSummaries[videoThumb.Location];
			videoSummaryDisplay.ShowLoading();
			videoSummaryDisplay.GenerateSummary();
		}

		private void InitChoices()
		{
			this.filters = new Dictionary<UXButton, ChoiceListUI>();
		}

		private void OnMoreVideosClicked(UXButton button)
		{
			this.lastPage = PageType.SEARCH;
			this.OnConfirmButtonClicked(this.shareCloseButton);
		}

		protected void InitDisplay()
		{
			this.shareCloseButton = this.parentScreen.GetElement<UXButton>("MakerButtonBackPostView");
			UXButton element = this.parentScreen.GetElement<UXButton>("BtnMoreVideosPostView");
			element.OnClicked = new UXButtonClickedDelegate(this.OnMoreVideosClicked);
			UXLabel element2 = this.parentScreen.GetElement<UXLabel>("BtnLabelMoreVideosPostView");
			element2.Text = this.lang.Get("MORE_VIDEOS", new object[0]);
			UXElement element3 = this.parentScreen.GetElement<UXElement>("MakerVideosCard");
			element3.Visible = false;
			this.toMoreVideos = this.parentScreen.GetElement<UXButton>("MakerButtonMoreVideos");
			this.toMoreVideos.OnClicked = new UXButtonClickedDelegate(this.OnConfirmButtonClicked);
			this.moreVideosLabel = this.parentScreen.GetElement<UXLabel>("MakerLabelMoreVideos");
			this.moreVideosLabel.Text = this.lang.Get("MORE_VIDEOS", new object[0]);
			UXElement element4 = this.parentScreen.GetElement<UXElement>("MakerCardMoreVideos");
			element4.Visible = false;
			this.backToFeatured = this.parentScreen.GetElement<UXButton>("MakerButtonBackMoreVideos");
			this.backToFeatured.OnClicked = new UXButtonClickedDelegate(this.OnConfirmButtonClicked);
			this.shareCloseButton.OnClicked = new UXButtonClickedDelegate(this.OnConfirmButtonClicked);
			this.searchInput = this.parentScreen.GetElement<UXInput>("MakerLabelSearchInput");
			this.searchInput.InitText(this.lang.Get("s_Search", new object[0]));
			UIInput uIInputComponent = this.searchInput.GetUIInputComponent();
			uIInputComponent.label.maxLineCount = 1;
			EventDelegate item = new EventDelegate(new EventDelegate.Callback(this.OnSubmit));
			uIInputComponent.onSubmit.Add(item);
			this.searchGridDynamic = new DynamicScrollingList(this.parentScreen.GetElement<UXGrid>("MakerGridMoreVideos"), 8);
			this.SetupSearchFilters();
			this.searchContainer = this.parentScreen.GetOptionalElement<UXElement>("MakerSearchContainer");
			this.parentScreen.GetElement<UXLabel>("MakerLabelWatchAgain").Text = this.lang.Get("hn_ui_watch_again", new object[0]);
			this.parentScreen.GetElement<UXLabel>("AMakerLabelSharePostView").Text = this.lang.Get("hn_ui_share", new object[0]);
			this.ShowFeaturedPage();
		}

		private void SetupSearchFilters()
		{
			UXElement element = this.parentScreen.GetElement<UXElement>("MakerCardFiltersTop");
			element.Visible = false;
			UXElement element2 = this.parentScreen.GetElement<UXElement>("MakerCardSubFilter");
			element2.Visible = false;
			this.InitChoices();
		}

		private void OnUpdateScrollArrowsDelayed(uint id, object cookie)
		{
			if (this.showScrollArrowsTimerId != 0u)
			{
				UXGrid uXGrid = (UXGrid)cookie;
				uXGrid.UpdateScrollArrows();
				this.showScrollArrowsTimerId = 0u;
			}
		}

		private void UpdateScrollArrowsDelayed(UXGrid grid)
		{
			ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
			if (this.showScrollArrowsTimerId != 0u)
			{
				viewTimerManager.KillViewTimer(this.showScrollArrowsTimerId);
			}
			this.showScrollArrowsTimerId = viewTimerManager.CreateViewTimer(1f, false, new TimerDelegate(this.OnUpdateScrollArrowsDelayed), grid);
		}

		protected void HideAllPages()
		{
			this.HideFeaturedPage();
			this.HideSearchPage();
			this.HidePostViewPage();
			this.HideShareToSquad();
			UXGrid element = this.parentScreen.GetElement<UXGrid>("MakerVideosGrid");
			element.HideScrollArrows();
			this.searchGridDynamic.Grid.HideScrollArrows();
		}

		protected void ShowFeaturedPage()
		{
			this.HideAllPages();
			this.currentState = HolonetVideoTabState.Featured;
			if (this.featuredPage == null)
			{
				this.featuredPage = new VideosFeatured(4, "MakerVideosCard");
			}
			this.featuredPage.ShowPage();
			this.ShowSharedUI();
			if (this.lastPage == PageType.FEATURED)
			{
				this.anims.ShowVideosFromPostView();
			}
			else if (this.lastPage == PageType.SEARCH)
			{
				this.anims.ShowVideosFromMoreVideos();
			}
			this.lastPage = PageType.FEATURED;
			this.UpdateScrollArrowsDelayed(this.parentScreen.GetElement<UXGrid>("MakerVideosGrid"));
		}

		protected void HideFeaturedPage()
		{
			if (this.currentState == HolonetVideoTabState.Featured)
			{
				this.currentState = HolonetVideoTabState.Invalid;
			}
			this.HideFeaturedSpinnerUI();
			if (this.featuredPage != null)
			{
				this.featuredPage.HidePage();
			}
		}

		public void ShowSearchPage(string searchText)
		{
			this.HideAllPages();
			this.currentState = HolonetVideoTabState.Search;
			this.toMoreVideos.Visible = false;
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			if (this.searchPage == null)
			{
				this.searchPage = new VideosSearch(1000, "MakerCardMoreVideos");
			}
			this.searchPage.ShowPage(searchText);
			this.ShowSharedUI();
			if (this.lastPage == PageType.SEARCH)
			{
				this.anims.ShowMoreVideosFromPostView();
			}
			else if (this.lastPage == PageType.FEATURED)
			{
				this.anims.ShowMoreVideos();
			}
			this.lastPage = PageType.SEARCH;
			this.UpdateScrollArrowsDelayed(this.searchGridDynamic.Grid);
		}

		protected void HideSearchPage()
		{
			if (this.toMoreVideos != null)
			{
				this.toMoreVideos.Visible = true;
			}
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			if (this.currentState == HolonetVideoTabState.Search)
			{
				this.currentState = HolonetVideoTabState.Invalid;
			}
		}

		public void ShowPreViewPage(bool fromExternalTab)
		{
			if (this.currentState == HolonetVideoTabState.PostView)
			{
				return;
			}
			this.currentState = HolonetVideoTabState.PreView;
			this.anims.ShowBlank();
			if (this.lastPage == PageType.NONE || fromExternalTab)
			{
				this.lastPage = PageType.POSTVIEW;
			}
		}

		public void HidePreViewPage()
		{
			this.OnConfirmButtonClicked(this.shareCloseButton);
		}

		public void ShowPostViewPage(string guid)
		{
			this.ShowPostViewPage(guid, false);
		}

		public void ShowPostViewPage(string guid, bool fromExternalTab)
		{
			if (this.currentState == HolonetVideoTabState.PostView)
			{
				return;
			}
			this.HideAllPages();
			this.screen.SetBackButtonForVideoPostView(this.shareCloseButton, new UXButtonClickedDelegate(this.OnConfirmButtonClicked));
			this.currentState = HolonetVideoTabState.PostView;
			if (this.postViewPage == null)
			{
				this.postViewPage = new VideosPostView();
			}
			this.HideSharedUI();
			this.postViewPage.ShowPage(guid);
			this.anims.ShowVideoPostView();
			if (this.lastPage == PageType.NONE || fromExternalTab)
			{
				this.lastPage = PageType.POSTVIEW;
			}
		}

		protected void HidePostViewPage()
		{
			this.screen.SetBackButtonToDefault();
			if (this.currentState == HolonetVideoTabState.PostView)
			{
				this.currentState = HolonetVideoTabState.Invalid;
			}
			if (this.postViewPage != null)
			{
				this.postViewPage.Cleanup();
			}
		}

		protected void HideShareToSquad()
		{
			VideosShareSquadScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<VideosShareSquadScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.DestroyScreen();
			}
		}

		private void ShowSharedUI()
		{
			this.searchContainer.Visible = true;
			this.toMoreVideos.Visible = true;
		}

		private void HideSharedUI()
		{
			this.searchContainer.Visible = false;
			this.toMoreVideos.Visible = false;
		}

		protected override void SetVisibleByTabButton(UXCheckbox button, bool selected)
		{
			base.SetVisibleByTabButton(button, selected);
			if (selected)
			{
				EventManager eventManager = Service.Get<EventManager>();
				eventManager.SendEvent(EventId.HolonetVideoTab, "tab");
			}
		}

		public override void OnTabOpen()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.UIVideosQueryStart);
			eventManager.RegisterObserver(this, EventId.UIVideosFeaturedStart);
			eventManager.RegisterObserver(this, EventId.UIVideosQueryResponse);
			eventManager.RegisterObserver(this, EventId.UIVideosQueryFilter);
			eventManager.RegisterObserver(this, EventId.UIVideosSourceTypeResponse);
			eventManager.RegisterObserver(this, EventId.UIVideosViewComplete);
			eventManager.RegisterObserver(this, EventId.UIVideosViewBegin);
			eventManager.RegisterObserver(this, EventId.UIDynamicScrollingListCreateItem);
			eventManager.RegisterObserver(this, EventId.UIDynamicScrollingListCleanupItem);
			eventManager.RegisterObserver(this, EventId.UIVideosRefresh);
			this.InitDisplay();
			this.isOpen = true;
			base.OnTabOpen();
		}

		public override void OnTabClose()
		{
			if (this.isOpen)
			{
				this.Close(null);
				this.isOpen = false;
			}
			this.lastPage = PageType.NONE;
			base.OnTabClose();
		}

		public void Close(object modalResult)
		{
			this.HideAllPages();
			foreach (VideoSummaryDisplay current in this.videoSummaries.Values)
			{
				current.Cleanup();
			}
			this.videoSummaries.Clear();
			if (this.searchInput != null)
			{
				UIInput uIInputComponent = this.searchInput.GetUIInputComponent();
				uIInputComponent.onSubmit.Clear();
				this.searchInput = null;
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.UIVideosQueryStart);
			eventManager.UnregisterObserver(this, EventId.UIVideosFeaturedStart);
			eventManager.UnregisterObserver(this, EventId.UIVideosQueryResponse);
			eventManager.UnregisterObserver(this, EventId.UIVideosQueryFilter);
			eventManager.UnregisterObserver(this, EventId.UIVideosSourceTypeResponse);
			eventManager.UnregisterObserver(this, EventId.UIVideosViewComplete);
			eventManager.UnregisterObserver(this, EventId.UIVideosViewBegin);
			eventManager.UnregisterObserver(this, EventId.UIDynamicScrollingListCreateItem);
			eventManager.UnregisterObserver(this, EventId.UIDynamicScrollingListCleanupItem);
			eventManager.UnregisterObserver(this, EventId.UIVideosRefresh);
			if (this.searchSpinnerTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.searchSpinnerTimerId);
				this.searchSpinnerTimerId = 0u;
			}
			if (this.featuredSpinnerTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.featuredSpinnerTimerId);
				this.featuredSpinnerTimerId = 0u;
			}
			Service.Get<ThumbnailManager>().Clear();
			if (this.featuredPage != null)
			{
				this.featuredPage.Cleanup();
				this.featuredPage = null;
			}
			if (this.searchPage != null)
			{
				this.searchPage.Cleanup();
				this.searchPage = null;
			}
			if (this.postViewPage != null)
			{
				this.postViewPage.Cleanup();
				this.postViewPage = null;
			}
			int i = 0;
			int count = this.grids.Count;
			while (i < count)
			{
				if (this.grids[i] != null)
				{
					this.grids[i].Clear();
				}
				i++;
			}
			this.grids.Clear();
			ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
			viewTimerManager.EnsureTimerKilled(ref this.showScrollArrowsTimerId);
		}

		public override void OnDestroyTab()
		{
			if (this.isOpen)
			{
				this.Close(null);
				this.isOpen = false;
			}
			if (this.searchGridDynamic != null)
			{
				this.searchGridDynamic.RemoveItems();
				this.searchGridDynamic = null;
			}
			Service.Get<VideoDataManager>().Clear();
		}

		private UXElement CreateLoadingUI(UXElement parent, string name)
		{
			UXSprite holonetLoader = Service.Get<UXController>().MiscElementsManager.GetHolonetLoader(parent);
			holonetLoader.LocalPosition = Vector3.zero;
			return holonetLoader;
		}

		private void OnShowSearchSpinner(uint id, object cookie)
		{
			if (this.searchSpinnerUI == null)
			{
				UXElement element = this.parentScreen.GetElement<UXElement>("MakerContainerMoreVideos");
				this.searchSpinnerUI = this.CreateLoadingUI(element, "search");
			}
			this.searchSpinnerUI.Visible = this.searchSpinnerUIVisibility;
			Service.Get<ViewTimerManager>().KillViewTimer(this.searchSpinnerTimerId);
			this.searchSpinnerTimerId = 0u;
		}

		private void ShowSearchSpinnerUI()
		{
			this.searchSpinnerUIVisibility = true;
			if (this.searchSpinnerTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.searchSpinnerTimerId);
			}
			this.searchSpinnerTimerId = Service.Get<ViewTimerManager>().CreateViewTimer(0.1f, true, new TimerDelegate(this.OnShowSearchSpinner), null);
		}

		private void HideSearchSpinnerUI()
		{
			this.searchSpinnerUIVisibility = false;
			if (this.searchSpinnerUI != null)
			{
				this.searchSpinnerUI.Visible = this.searchSpinnerUIVisibility;
			}
			Service.Get<ViewTimerManager>().KillViewTimer(this.searchSpinnerTimerId);
			this.searchSpinnerTimerId = 0u;
		}

		private void OnShowFeaturedSpinner(uint id, object cookie)
		{
			if (this.featuredSpinnerUI == null)
			{
				UXElement element = this.parentScreen.GetElement<UXElement>("MakerVideosContainer");
				this.featuredSpinnerUI = this.CreateLoadingUI(element, "featured");
			}
			this.featuredSpinnerUI.Visible = this.featuredSpinnerUIVisibility;
			Service.Get<ViewTimerManager>().KillViewTimer(this.featuredSpinnerTimerId);
			this.featuredSpinnerTimerId = 0u;
		}

		private void ShowFeaturedSpinnerUI()
		{
			this.featuredSpinnerUIVisibility = true;
			if (this.featuredSpinnerTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.featuredSpinnerTimerId);
			}
			this.featuredSpinnerTimerId = Service.Get<ViewTimerManager>().CreateViewTimer(0.1f, true, new TimerDelegate(this.OnShowFeaturedSpinner), null);
		}

		private void HideFeaturedSpinnerUI()
		{
			this.featuredSpinnerUIVisibility = false;
			if (this.featuredSpinnerUI != null)
			{
				this.featuredSpinnerUI.Visible = this.featuredSpinnerUIVisibility;
			}
			Service.Get<ViewTimerManager>().KillViewTimer(this.featuredSpinnerTimerId);
			this.featuredSpinnerTimerId = 0u;
		}

		private void OnConfirmButtonClicked(UXButton button)
		{
			if (button == this.backToFeatured)
			{
				this.ShowFeaturedPage();
			}
			else if (button == this.toMoreVideos)
			{
				this.ShowSearchPage(null);
			}
			else if (button == this.shareCloseButton)
			{
				this.HidePostViewPage();
				EventManager eventManager = Service.Get<EventManager>();
				eventManager.SendEvent(EventId.HolonetVideoTab, "post_watch");
				if (this.lastPage == PageType.SEARCH)
				{
					this.ShowSearchPage(null);
				}
				else if (this.lastPage == PageType.FEATURED)
				{
					this.ShowFeaturedPage();
				}
				else if (this.lastPage == PageType.POSTVIEW)
				{
					this.ShowSharedUI();
					if (this.screen.PreviousTab != null)
					{
						this.screen.OpenTab(this.screen.PreviousTab.HolonetControllerType);
					}
				}
			}
		}

		private string GetFilterCardName(string cardName, int id)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" (");
			stringBuilder.Append(cardName);
			stringBuilder.Append("_");
			stringBuilder.Append(id);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		private void OnFilterButtonClicked(UXButton button)
		{
			foreach (UXButton current in this.filters.Keys)
			{
				if (button == current)
				{
					if (this.filters[current].filterOptions.Visible)
					{
						this.filters[current].filterOptions.Visible = false;
					}
					else
					{
						this.filters[current].filterOptions.Visible = true;
						if ((int)button.Tag == 2)
						{
							string filterCardName = this.GetFilterCardName("MakerCardFiltersTop", this.filters[current].id);
							UXLabel element = this.parentScreen.GetElement<UXLabel>("MakerLabelFilter" + filterCardName);
							int choiceIdFromLabel = this.searchPage.GetChoiceIdFromLabel(this.filters[current].id, element.Text);
							if (choiceIdFromLabel >= 0)
							{
								UXLabel element2 = this.parentScreen.GetElement<UXLabel>("MakerLabelCounterValueHQLevel" + filterCardName);
								element2.Text = element.Text;
							}
						}
						else
						{
							UXGrid filterGrid = this.filters[current].filterGrid;
							if (filterGrid != null)
							{
								filterGrid.RepositionItems();
							}
						}
					}
				}
				else
				{
					this.filters[current].filterOptions.Visible = false;
				}
			}
		}

		private void OnFilterChoiceClicked(UXButton button)
		{
			bool flag = true;
			if (flag)
			{
				foreach (UXButton current in this.filters.Keys)
				{
					this.filters[current].filterOptions.Visible = false;
					foreach (ChoiceUI current2 in this.filters[current].choices)
					{
						if (button == current2.Button)
						{
							UXLabel buttonLabel = this.filters[current].buttonLabel;
							buttonLabel.Text = current2.DisplayValue;
							this.searchPage.OptionSelected(this.filters[current].id, current2.Id);
							break;
						}
					}
				}
			}
		}

		private void OnFilterPickerClicked(UXButton button)
		{
			foreach (UXButton current in this.filters.Keys)
			{
				foreach (ChoiceUI current2 in this.filters[current].choices)
				{
					if (button == current2.Button)
					{
						string filterCardName = this.GetFilterCardName("MakerCardFiltersTop", this.filters[current].id);
						UXLabel element = this.parentScreen.GetElement<UXLabel>("MakerLabelCounterValueHQLevel" + filterCardName);
						if ((int)button.Tag == 1)
						{
							this.filters[current].buttonLabel.Text = element.Text;
							int choiceIdFromLabel = this.searchPage.GetChoiceIdFromLabel(this.filters[current].id, this.filters[current].buttonLabel.Text);
							this.searchPage.OptionSelected(this.filters[current].id, choiceIdFromLabel);
							this.filters[current].filterOptions.Visible = false;
						}
						else if ((int)button.Tag == 2)
						{
							string nextChoice = this.searchPage.GetNextChoice(this.filters[current].id, element.Text, true, true);
							if (nextChoice != string.Empty)
							{
								element.Text = nextChoice;
							}
						}
						else if ((int)button.Tag == 3)
						{
							string nextChoice2 = this.searchPage.GetNextChoice(this.filters[current].id, element.Text, false, true);
							if (nextChoice2 != string.Empty)
							{
								element.Text = nextChoice2;
							}
						}
						break;
					}
				}
			}
		}

		private void OnSubmit()
		{
			if (this.currentState != HolonetVideoTabState.Search)
			{
				this.ShowSearchPage(this.searchInput.Text);
			}
			else if (this.searchPage != null)
			{
				this.searchPage.SetKeywords(this.searchInput.Text);
				this.searchPage.PerformSearch();
			}
		}

		private void OnRetryFeatured(UXButton button)
		{
			if (this.featuredPage == null)
			{
				Service.Get<Logger>().Error("No featured page to retry query");
				return;
			}
			this.featuredPage.DoFeaturedQuery();
		}

		private void OnRetrySearch(UXButton button)
		{
			if (this.searchPage == null)
			{
				Service.Get<Logger>().Error("No search page to retry query");
				return;
			}
			this.searchPage.PerformSearch();
		}

		public void OnViewFrameTime(float dt)
		{
			if (!this.parentScreen.IsLoaded())
			{
				return;
			}
			if (this.currentState == HolonetVideoTabState.Search)
			{
				this.searchGridDynamic.UpdateItems();
			}
		}

		public override string GetBITabName()
		{
			string str = string.Empty;
			if (this.currentState != HolonetVideoTabState.Invalid)
			{
				str = "_" + this.currentState.ToString().ToLower();
			}
			return "video" + str;
		}
	}
}
