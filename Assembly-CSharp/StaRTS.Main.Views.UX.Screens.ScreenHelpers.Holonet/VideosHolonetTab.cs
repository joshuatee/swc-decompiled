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
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

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
			string text2 = stringBuilder.ToString();
			if (queryChoiceData.UIType == ChoiceType.SimpleList)
			{
				this.parentScreen.DestroyElement(this.parentScreen.GetElement<UXElement>("MakerContainerHQLevelSubFilter" + text2));
				UXElement element3 = this.parentScreen.GetElement<UXElement>("MakerContainerBGSubFilters" + text2);
				UIWidget component = element3.Root.GetComponent<UIWidget>();
				component.SetDimensions(component.width, 80 * queryChoiceData.Choices.Count);
			}
			else
			{
				this.parentScreen.DestroyElement(this.parentScreen.GetElement<UXElement>("MakerContainerBGSubFilters" + text2));
				UXLabel element4 = this.parentScreen.GetElement<UXLabel>("MakerLabelTitleHQLevel" + text2);
				element4.Text = queryChoiceData.UILabel;
				UXButton element5 = this.parentScreen.GetElement<UXButton>("MakerCounterButtonDecrement" + text2);
				element5.OnClicked = new UXButtonClickedDelegate(this.OnFilterPickerClicked);
				element5.Tag = PickerButtonType.Decrement;
				UXButton element6 = this.parentScreen.GetElement<UXButton>("MakerCounterButtonIncrement" + text2);
				element6.OnClicked = new UXButtonClickedDelegate(this.OnFilterPickerClicked);
				element6.Tag = PickerButtonType.Increment;
				UXButton element7 = this.parentScreen.GetElement<UXButton>("MakerButtonSetHQLevel" + text2);
				element7.OnClicked = new UXButtonClickedDelegate(this.OnFilterPickerClicked);
				element7.Tag = PickerButtonType.SetType;
				UXLabel element8 = this.parentScreen.GetElement<UXLabel>("MakerLabelSetHQLevel" + text2);
				element8.Text = this.lang.Get("s_Set", new object[0]);
			}
			UXElement element9 = this.parentScreen.GetElement<UXElement>("MakerContainerSubFilters" + text2);
			element9.Visible = false;
			UXButton element10 = this.parentScreen.GetElement<UXButton>("MakerButtonFiltersTop" + text2);
			element10.OnClicked = new UXButtonClickedDelegate(this.OnFilterButtonClicked);
			element10.Tag = queryChoiceData.UIType;
			UXLabel element11 = this.parentScreen.GetElement<UXLabel>("MakerLabelFilter" + text2);
			element11.Text = queryChoiceData.UILabel;
			ChoiceListUI value = default(ChoiceListUI);
			value.buttonLabel = element11;
			value.choices = new List<ChoiceUI>();
			value.filterOptions = element9;
			value.id = queryChoiceData.Id;
			UXGrid element12 = this.parentScreen.GetElement<UXGrid>("MakerGridSubFilters" + text2);
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
					string text3 = stringBuilder.ToString();
					this.grids.Add(element12);
					UXElement element13 = this.parentScreen.GetElement<UXElement>("MakerCardSubFilter");
					UXElement item2 = this.parentScreen.CloneElement<UXElement>(element13, text3, element12.Root);
					element12.AddItem(item2, element12.Count);
					stringBuilder = new StringBuilder("MakerButtonSubFilter");
					stringBuilder.Append(" (");
					stringBuilder.Append(text3);
					stringBuilder.Append(")");
					VideoFilterOption videoFilterOption = queryChoiceData.Choices[i];
					UXButton element14 = this.parentScreen.GetElement<UXButton>(stringBuilder.ToString());
					element14.OnClicked = new UXButtonClickedDelegate(this.OnFilterChoiceClicked);
					stringBuilder = new StringBuilder("MakerLabelSubFilter");
					stringBuilder.Append(" (");
					stringBuilder.Append(text3);
					stringBuilder.Append(")");
					UXLabel element15 = this.parentScreen.GetElement<UXLabel>(stringBuilder.ToString());
					if (videoFilterOption.UILabel != "")
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
						UXLabel element16 = this.parentScreen.GetElement<UXLabel>("MakerLabelCounterValueHQLevel" + text2);
						element16.Text = videoFilterOption2.UILabel;
					}
					UXButton element17 = this.parentScreen.GetElement<UXButton>("MakerCounterButtonDecrement" + text2);
					ChoiceUI item4 = new ChoiceUI(element17, videoFilterOption2.UILabel, videoFilterOption2.Id);
					value.choices.Add(item4);
					element17 = this.parentScreen.GetElement<UXButton>("MakerCounterButtonIncrement" + text2);
					item4 = new ChoiceUI(element17, videoFilterOption2.UILabel, videoFilterOption2.Id);
					value.choices.Add(item4);
					element17 = this.parentScreen.GetElement<UXButton>("MakerButtonSetHQLevel" + text2);
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
				if (!this.CanUpdateVideoSummaryStyle(keyValuePair.get_Key()))
				{
					return EatResponse.NotEaten;
				}
				using (List<VideoSummaryData>.Enumerator enumerator = keyValuePair.get_Value().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						VideoSummaryData current = enumerator.Current;
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
						Service.Get<StaRTSLogger>().ErrorFormat("VideosHolonetTab UIVideosQueryResponse unhandled type {0}", new object[]
						{
							current.Style
						});
					}
					return EatResponse.NotEaten;
				}
				break;
			}
			case EventId.UIVideosThumbnailResponse:
			case EventId.VideoStart:
			case EventId.VideoEnd:
				return EatResponse.NotEaten;
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
				return EatResponse.NotEaten;
			case EventId.UIVideosQueryStart:
				foreach (VideoSummaryDisplay current2 in this.videoSummaries.Values)
				{
					current2.Cleanup();
				}
				this.videoSummaries.Clear();
				this.searchGridDynamic.RemoveItems();
				this.parentScreen.GetElement<UXElement>("MakerContainerErrorMsgMoreVideos").Visible = false;
				this.ShowSearchSpinnerUI();
				return EatResponse.NotEaten;
			case EventId.UIVideosQueryFilter:
				break;
			case EventId.UIVideosViewBegin:
				this.ShowPreViewPage(false);
				return EatResponse.NotEaten;
			case EventId.UIVideosViewComplete:
			{
				KeyValuePair<bool, string> keyValuePair2 = (KeyValuePair<bool, string>)cookie;
				if (keyValuePair2.get_Key())
				{
					this.ShowPostViewPage(keyValuePair2.get_Value());
					return EatResponse.NotEaten;
				}
				this.HidePreViewPage();
				return EatResponse.NotEaten;
			}
			case EventId.UIVideosRefresh:
				goto IL_3C5;
			case EventId.UIVideosSourceTypeResponse:
			{
				KeyValuePair<List<string>, string> keyValuePair3 = (KeyValuePair<List<string>, string>)cookie;
				using (Dictionary<int, VideoSummaryDisplay>.ValueCollection.Enumerator enumerator3 = this.videoSummaries.Values.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						VideoSummaryDisplay current3 = enumerator3.Current;
						if (keyValuePair3.get_Key().Contains(current3.GetGuid()))
						{
							current3.UpdateSourceType(keyValuePair3.get_Value());
						}
					}
					return EatResponse.NotEaten;
				}
				goto IL_3C5;
			}
			case EventId.UIDynamicScrollingListCreateItem:
			{
				ListItemCreateData listItemCreateData = (ListItemCreateData)cookie;
				UXElement itemUI = listItemCreateData.ParentList.GetItemUI(listItemCreateData.Location);
				VideoSummaryData oldVideoThumb = (VideoSummaryData)listItemCreateData.ParentList.GetItemCookie(listItemCreateData.OldLocation);
				UXElement item = this.CreateDynamicScrollingListItem((VideoSummaryData)listItemCreateData.Cookie, listItemCreateData.ParentList, itemUI, oldVideoThumb);
				listItemCreateData.ParentList.SetItemUI(item, listItemCreateData.Location);
				return EatResponse.NotEaten;
			}
			case EventId.UIDynamicScrollingListCleanupItem:
			{
				VideoSummaryData videoSummaryData = (VideoSummaryData)cookie;
				VideoSummaryDisplay videoSummaryDisplay;
				this.videoSummaries.TryGetValue(videoSummaryData.Location, out videoSummaryDisplay);
				if (videoSummaryDisplay != null)
				{
					videoSummaryDisplay.Recycle();
					return EatResponse.NotEaten;
				}
				return EatResponse.NotEaten;
			}
			default:
				return EatResponse.NotEaten;
			}
			this.SetupDropdownChoice((ChoiceData)cookie);
			return EatResponse.NotEaten;
			IL_3C5:
			if (this.currentState == HolonetVideoTabState.Search && this.searchPage != null)
			{
				this.searchPage.PerformSearch();
			}
			else if (this.currentState == HolonetVideoTabState.Featured && this.featuredPage != null)
			{
				this.featuredPage.DoFeaturedQuery();
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
			UXElement uXElement;
			if (existingItem != null)
			{
				videoThumb.UILabel = oldVideoThumb.UILabel;
				string uILabel = videoThumb.UILabel;
				existingItem.SetRootName(uILabel);
				uXElement = existingItem;
				oldVideoThumb.IsVisible = false;
			}
			else
			{
				string uILabel = videoThumb.UILabel;
				uXElement = this.parentScreen.GetElement<UXElement>("MakerCardMoreVideos");
				uXElement = this.parentScreen.CloneElement<UXElement>(uXElement, uILabel, list.Root);
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
			if (this.lastPage == PageType.NONE | fromExternalTab)
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
			if (this.lastPage == PageType.NONE | fromExternalTab)
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
				return;
			}
			if (button == this.toMoreVideos)
			{
				this.ShowSearchPage(null);
				return;
			}
			if (button == this.shareCloseButton)
			{
				this.HidePostViewPage();
				EventManager eventManager = Service.Get<EventManager>();
				eventManager.SendEvent(EventId.HolonetVideoTab, "post_watch");
				if (this.lastPage == PageType.SEARCH)
				{
					this.ShowSearchPage(null);
					return;
				}
				if (this.lastPage == PageType.FEATURED)
				{
					this.ShowFeaturedPage();
					return;
				}
				if (this.lastPage == PageType.POSTVIEW)
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
						if ((ChoiceType)button.Tag == ChoiceType.Picker)
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
						if ((PickerButtonType)button.Tag == PickerButtonType.SetType)
						{
							this.filters[current].buttonLabel.Text = element.Text;
							int choiceIdFromLabel = this.searchPage.GetChoiceIdFromLabel(this.filters[current].id, this.filters[current].buttonLabel.Text);
							this.searchPage.OptionSelected(this.filters[current].id, choiceIdFromLabel);
							this.filters[current].filterOptions.Visible = false;
							break;
						}
						if ((PickerButtonType)button.Tag == PickerButtonType.Decrement)
						{
							string nextChoice = this.searchPage.GetNextChoice(this.filters[current].id, element.Text, true, true);
							if (nextChoice != "")
							{
								element.Text = nextChoice;
								break;
							}
							break;
						}
						else
						{
							if ((PickerButtonType)button.Tag != PickerButtonType.Increment)
							{
								break;
							}
							string nextChoice2 = this.searchPage.GetNextChoice(this.filters[current].id, element.Text, false, true);
							if (nextChoice2 != "")
							{
								element.Text = nextChoice2;
								break;
							}
							break;
						}
					}
				}
			}
		}

		private void OnSubmit()
		{
			if (this.currentState != HolonetVideoTabState.Search)
			{
				this.ShowSearchPage(this.searchInput.Text);
				return;
			}
			if (this.searchPage != null)
			{
				this.searchPage.SetKeywords(this.searchInput.Text);
				this.searchPage.PerformSearch();
			}
		}

		private void OnRetryFeatured(UXButton button)
		{
			if (this.featuredPage == null)
			{
				Service.Get<StaRTSLogger>().Error("No featured page to retry query");
				return;
			}
			this.featuredPage.DoFeaturedQuery();
		}

		private void OnRetrySearch(UXButton button)
		{
			if (this.searchPage == null)
			{
				Service.Get<StaRTSLogger>().Error("No search page to retry query");
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
			string text = string.Empty;
			if (this.currentState != HolonetVideoTabState.Invalid)
			{
				text = "_" + this.currentState.ToString().ToLower();
			}
			return "video" + text;
		}

		protected internal VideosHolonetTab(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).CanUpdateVideoSummaryStyle((VideoSummaryStyle)(*(int*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).CloseAllDropdowns((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).CreateDynamicScrollingListItem((VideoSummaryData)GCHandledObjects.GCHandleToObject(*args), (DynamicScrollingList)GCHandledObjects.GCHandleToObject(args[1]), (UXElement)GCHandledObjects.GCHandleToObject(args[2]), (VideoSummaryData)GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).CreateLoadingUI((UXElement)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).GetBITabName());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).GetFilterCardName(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HideAllPages();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HideFeaturedPage();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HideFeaturedSpinnerUI();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HidePostViewPage();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HidePreViewPage();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HideSearchPage();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HideSearchSpinnerUI();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HideSharedUI();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HideShareToSquad();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).InitChoices();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).InitDisplay();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnConfirmButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnDestroyTab();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnFilterButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnFilterChoiceClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnFilterPickerClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnMoreVideosClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnRetryFeatured((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnRetrySearch((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnSubmit();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnTabClose();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnTabOpen();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetupDropdownChoice((ChoiceData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetupMessageFeatured((VideoSummaryData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetupMessageSearch((VideoSummaryData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetupSearchFilters();
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetupVideoSummaryFeatured((VideoSummaryData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetupVideoSummaryPostView((VideoSummaryData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetupVideoSummarySearch((VideoSummaryData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetVisibleByTabButton((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ShowFeaturedPage();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ShowFeaturedSpinnerUI();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ShowPostViewPage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ShowPostViewPage(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ShowPreViewPage(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ShowSearchPage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ShowSearchSpinnerUI();
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).ShowSharedUI();
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((VideosHolonetTab)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollArrowsDelayed((UXGrid)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
