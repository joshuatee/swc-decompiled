using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace StaRTS.Externals.Maker
{
	public class VideosSearch
	{
		private struct FilterData
		{
			public Dictionary<string, string[]> Choices;

			public ChoiceType UIType;

			public FilterType ValueType;

			public FilterData(Dictionary<string, string[]> choices, ChoiceType uiType, FilterType filterType)
			{
				this.UIType = uiType;
				this.ValueType = filterType;
				this.Choices = choices;
			}
		}

		private const int MESSAGE_GRID_LOCATION = 1;

		private const string HN_UI_DATE = "hn_ui_date";

		private const string HN_UI_ANYTIME = "hn_ui_anytime";

		private const string HN_UI_MOST_RECENT = "hn_ui_most_recent";

		private const string HN_UI_PAST_WEEK = "hn_ui_past_week";

		private const string HN_UI_PAST_MONTH = "hn_ui_past_month";

		private const string HN_UI_SOURCE = "hn_ui_source";

		private const string HN_UI_ALL = "hn_ui_all";

		private const string HN_UI_HQ_LEVEL = "hn_ui_hq_level";

		private const string HN_UI_ANY = "hn_ui_any";

		private const string HN_UI_FACTION = "hn_ui_faction";

		private const string HN_UI_BOTH = "hn_ui_both";

		private const string HN_UI_REBEL = "hn_ui_rebel";

		private const string HN_UI_EMPIRE = "hn_ui_empire";

		private DataState dataState;

		private List<VideoSummaryData> foundVids;

		private string vidSummaryLabel;

		private List<VideosFilterChoice> filters;

		private Dictionary<string, string> searchTags;

		private string searchKeywords;

		private int ageLimitInDays;

		private QuerySourceTypes sourceTypeHelper;

		public VideosSearch(int numShowing, string vidSummaryLabel)
		{
			this.foundVids = new List<VideoSummaryData>();
			this.filters = new List<VideosFilterChoice>();
			this.dataState = DataState.NotLoaded;
			this.vidSummaryLabel = vidSummaryLabel;
			this.searchTags = new Dictionary<string, string>();
			this.searchKeywords = string.Empty;
			this.ageLimitInDays = -1;
			this.sourceTypeHelper = new QuerySourceTypes();
		}

		private bool VideoValid(string guid)
		{
			VideoData videoData;
			return Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData) && (this.ageLimitInDays == -1 || videoData.Age.Days <= this.ageLimitInDays);
		}

		private void CreateMessageSummary(VideoSummaryStyle messageType)
		{
			VideoSummaryData item = new VideoSummaryData(1, this.vidSummaryLabel + "_" + 1, string.Empty, ThumbnailSize.SMALL, messageType);
			KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>> keyValuePair = new KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>>(messageType, new List<VideoSummaryData>
			{
				item
			});
			Service.Get<EventManager>().SendEvent(EventId.UIVideosQueryResponse, keyValuePair);
		}

		private void CreateVideoSummaries(List<string> videoGuidList)
		{
			List<string> list = new List<string>();
			List<VideoSummaryData> list2 = new List<VideoSummaryData>();
			int count = videoGuidList.Count;
			for (int i = 0; i < count; i++)
			{
				string text = videoGuidList[i];
				if (!list.Contains(text) && this.VideoValid(text))
				{
					list.Add(text);
					VideoSummaryData item = new VideoSummaryData(i, this.vidSummaryLabel + "_" + (i + 1), text, ThumbnailSize.SMALL, VideoSummaryStyle.Search);
					this.foundVids.Add(item);
					list2.Add(item);
				}
			}
			if (list.Count == 0)
			{
				this.CreateMessageSummary(VideoSummaryStyle.SearchEmpty);
				return;
			}
			this.dataState = DataState.Loaded;
			Service.Get<Logger>().DebugFormat("Got {0} results", new object[]
			{
				list.Count
			});
			KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>> keyValuePair = new KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>>(VideoSummaryStyle.Search, list2);
			Service.Get<EventManager>().SendEvent(EventId.UIVideosQueryResponse, keyValuePair);
		}

		private void OnSearchQueried(List<string> videoGuidList)
		{
			if (videoGuidList == null)
			{
				this.CreateMessageSummary(VideoSummaryStyle.SearchError);
			}
			else if (videoGuidList.Count == 0)
			{
				this.CreateMessageSummary(VideoSummaryStyle.SearchEmpty);
			}
			else
			{
				this.CreateVideoSummaries(videoGuidList);
				this.sourceTypeHelper.QueryStart();
			}
		}

		private Dictionary<string, VideosSearch.FilterData> GetFilterData()
		{
			Lang lang = Service.Get<Lang>();
			string key = lang.Get("hn_ui_hq_level", new object[0]);
			string key2 = lang.Get("hn_ui_source", new object[0]);
			Dictionary<string, VideosSearch.FilterData> dictionary = new Dictionary<string, VideosSearch.FilterData>
			{
				{
					lang.Get("hn_ui_date", new object[0]),
					new VideosSearch.FilterData(new Dictionary<string, string[]>
					{
						{
							lang.Get("hn_ui_anytime", new object[0]),
							new string[]
							{
								string.Empty
							}
						},
						{
							lang.Get("hn_ui_past_week", new object[0]),
							new string[]
							{
								"7"
							}
						},
						{
							lang.Get("hn_ui_past_month", new object[0]),
							new string[]
							{
								"30"
							}
						}
					}, ChoiceType.SimpleList, FilterType.Date)
				},
				{
					key2,
					new VideosSearch.FilterData(new Dictionary<string, string[]>
					{
						{
							lang.Get("hn_ui_all", new object[0]),
							new string[]
							{
								string.Empty
							}
						}
					}, ChoiceType.SimpleList, FilterType.Tag)
				},
				{
					key,
					new VideosSearch.FilterData(new Dictionary<string, string[]>
					{
						{
							lang.Get("hn_ui_any", new object[0]),
							new string[]
							{
								string.Empty
							}
						}
					}, ChoiceType.Picker, FilterType.Tag)
				},
				{
					lang.Get("hn_ui_faction", new object[0]),
					new VideosSearch.FilterData(new Dictionary<string, string[]>
					{
						{
							lang.Get("hn_ui_both", new object[0]),
							new string[]
							{
								string.Empty
							}
						},
						{
							lang.Get("hn_ui_rebel", new object[0]),
							new string[]
							{
								"rebel"
							}
						},
						{
							lang.Get("hn_ui_empire", new object[0]),
							new string[]
							{
								"empire"
							}
						}
					}, ChoiceType.SimpleList, FilterType.Tag)
				}
			};
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			Entity currentHQ = buildingLookupController.GetCurrentHQ();
			BuildingTypeVO buildingType = currentHQ.Get<BuildingComponent>().BuildingType;
			int lvl = Service.Get<BuildingUpgradeCatalog>().GetMaxLevel(buildingType.UpgradeGroup).Lvl;
			for (int i = 1; i <= lvl; i++)
			{
				string text = i.ToString();
				dictionary[key].Choices[text] = new string[]
				{
					text
				};
			}
			Dictionary<string, string> sourceTypes = Service.Get<VideoDataManager>().SourceTypes;
			foreach (string current in sourceTypes.Keys)
			{
				string key3 = lang.Get(sourceTypes[current], new object[0]);
				dictionary[key2].Choices[key3] = new string[]
				{
					current
				};
			}
			return dictionary;
		}

		public void ShowPage(string searchText)
		{
			if (this.dataState == DataState.NotLoaded)
			{
				Dictionary<string, VideosSearch.FilterData> filterData = this.GetFilterData();
				int firstFilterLocation = this.GetFirstFilterLocation();
				foreach (KeyValuePair<string, VideosSearch.FilterData> current in filterData)
				{
					List<VideoFilterOption> list = new List<VideoFilterOption>();
					int firstFilterLocation2 = this.GetFirstFilterLocation();
					foreach (KeyValuePair<string, string[]> current2 in current.Value.Choices)
					{
						VideoFilterOption item = new VideoFilterOption(current2.Key, current2.Value[0], firstFilterLocation2++);
						list.Add(item);
					}
					this.filters.Add(new VideosFilterChoice(current.Key, firstFilterLocation++, list, current.Value.UIType, current.Value.ValueType));
				}
				this.dataState = DataState.Loading;
			}
			if (searchText != null)
			{
				this.SetKeywords(searchText);
			}
			this.PerformSearch();
		}

		public int GetFirstFilterLocation()
		{
			return 1;
		}

		[DebuggerHidden]
		private IEnumerator HideResultsAndSearch()
		{
			VideosSearch.<HideResultsAndSearch>c__Iterator12 <HideResultsAndSearch>c__Iterator = new VideosSearch.<HideResultsAndSearch>c__Iterator12();
			<HideResultsAndSearch>c__Iterator.<>f__this = this;
			return <HideResultsAndSearch>c__Iterator;
		}

		public void SetTag(string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				this.searchTags[key] = value;
			}
			else
			{
				this.searchTags.Remove(key);
			}
		}

		public void SetKeywords(string keywords)
		{
			this.searchKeywords = keywords;
		}

		private VideosFilterChoice GetFilterChoice(int filterId)
		{
			foreach (VideosFilterChoice current in this.filters)
			{
				if (filterId == current.Id)
				{
					return current;
				}
			}
			return null;
		}

		private VideoFilterOption GetFilterOption(int choiceId, VideosFilterChoice currFilter)
		{
			if (currFilter == null)
			{
				return null;
			}
			foreach (VideoFilterOption current in currFilter.Options)
			{
				if (choiceId == current.Id)
				{
					return current;
				}
			}
			return null;
		}

		public void OptionSelected(int filterId, int choiceId)
		{
			VideosFilterChoice filterChoice = this.GetFilterChoice(filterId);
			VideoFilterOption filterOption = this.GetFilterOption(choiceId, filterChoice);
			if (filterOption == null)
			{
				return;
			}
			if (filterChoice.ValueType == FilterType.Tag)
			{
				this.SetTag(filterId.ToString(), filterOption.Value);
			}
			else if (filterChoice.ValueType == FilterType.Date)
			{
				this.ageLimitInDays = -1;
				if (!string.IsNullOrEmpty(filterOption.Value))
				{
					this.ageLimitInDays = (int)Convert.ToUInt16(filterOption.Value);
				}
			}
			this.PerformSearch();
		}

		public int GetChoiceIdFromLabel(int filterId, string label)
		{
			foreach (VideosFilterChoice current in this.filters)
			{
				if (filterId == current.Id)
				{
					return current.GetChoiceIdFromLabel(label);
				}
			}
			return -1;
		}

		public string GetNextChoice(int filterId, string label, bool reverse = false, bool loop = false)
		{
			foreach (VideosFilterChoice current in this.filters)
			{
				if (filterId == current.Id)
				{
					return current.GetNextChoice(this.GetChoiceIdFromLabel(filterId, label), reverse, loop);
				}
			}
			return string.Empty;
		}

		public void PerformSearch()
		{
			this.UpdateSearch();
		}

		private void UpdateSearch()
		{
			Service.Get<Engine>().StartCoroutine(this.HideResultsAndSearch());
		}

		private void ContinueSearch()
		{
			char[] separator = new char[]
			{
				' ',
				',',
				'.',
				':',
				'\t'
			};
			string[] array = this.searchKeywords.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = new string[this.searchTags.Count];
			this.searchTags.Values.CopyTo(array2, 0);
			Service.Get<Logger>().DebugFormat("search requested with keywords '{0}' and tags '{1}'", new object[]
			{
				string.Join(", ", array),
				string.Join(", ", array2)
			});
			Service.Get<VideoDataManager>().SearchSubCategories(VideoSection.ALL, array2, array, new VideoDataManager.DataListQueryCompleteDelegate(this.OnSearchQueried));
		}

		public void Cleanup()
		{
			this.sourceTypeHelper.Active = false;
		}
	}
}
