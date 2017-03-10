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
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

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

		private DataState dataState;

		private List<VideoSummaryData> foundVids;

		private string vidSummaryLabel;

		private List<VideosFilterChoice> filters;

		private Dictionary<string, string> searchTags;

		private string searchKeywords;

		private int ageLimitInDays;

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

		private QuerySourceTypes sourceTypeHelper;

		public VideosSearch(int numShowing, string vidSummaryLabel)
		{
			this.foundVids = new List<VideoSummaryData>();
			this.filters = new List<VideosFilterChoice>();
			this.dataState = DataState.NotLoaded;
			this.vidSummaryLabel = vidSummaryLabel;
			this.searchTags = new Dictionary<string, string>();
			this.searchKeywords = "";
			this.ageLimitInDays = -1;
			this.sourceTypeHelper = new QuerySourceTypes();
		}

		private bool VideoValid(string guid)
		{
			VideoData videoData;
			return Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData) && (this.ageLimitInDays == -1 || videoData.Age.get_Days() <= this.ageLimitInDays);
		}

		private void CreateMessageSummary(VideoSummaryStyle messageType)
		{
			VideoSummaryData item = new VideoSummaryData(1, this.vidSummaryLabel + "_" + 1, "", ThumbnailSize.SMALL, messageType);
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
			Service.Get<StaRTSLogger>().DebugFormat("Got {0} results", new object[]
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
				return;
			}
			if (videoGuidList.Count == 0)
			{
				this.CreateMessageSummary(VideoSummaryStyle.SearchEmpty);
				return;
			}
			this.CreateVideoSummaries(videoGuidList);
			this.sourceTypeHelper.QueryStart();
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
								""
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
								""
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
								""
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
								""
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
					foreach (KeyValuePair<string, string[]> current2 in current.get_Value().Choices)
					{
						VideoFilterOption item = new VideoFilterOption(current2.get_Key(), current2.get_Value()[0], firstFilterLocation2++);
						list.Add(item);
					}
					this.filters.Add(new VideosFilterChoice(current.get_Key(), firstFilterLocation++, list, current.get_Value().UIType, current.get_Value().ValueType));
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

		[IteratorStateMachine(typeof(VideosSearch.<HideResultsAndSearch>d__31))]
		private IEnumerator HideResultsAndSearch()
		{
			Service.Get<EventManager>().SendEvent(EventId.UIVideosQueryStart, null);
			foreach (VideoSummaryData current in this.foundVids)
			{
				current.Cleanup();
			}
			this.foundVids.Clear();
			yield return Resources.UnloadUnusedAssets();
			this.ContinueSearch();
			yield break;
		}

		public void SetTag(string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				this.searchTags[key] = value;
				return;
			}
			this.searchTags.Remove(key);
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
					this.ageLimitInDays = (int)Convert.ToUInt16(filterOption.Value, CultureInfo.InvariantCulture);
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
			return "";
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
			char[] array = new char[]
			{
				' ',
				',',
				'.',
				':',
				'\t'
			};
			string[] array2 = this.searchKeywords.Split(array, 1);
			string[] array3 = new string[this.searchTags.Count];
			this.searchTags.Values.CopyTo(array3, 0);
			Service.Get<StaRTSLogger>().DebugFormat("search requested with keywords '{0}' and tags '{1}'", new object[]
			{
				string.Join(", ", array2),
				string.Join(", ", array3)
			});
			Service.Get<VideoDataManager>().SearchSubCategories(VideoSection.ALL, array3, array2, new VideoDataManager.DataListQueryCompleteDelegate(this.OnSearchQueried));
		}

		public void Cleanup()
		{
			this.sourceTypeHelper.Active = false;
		}

		protected internal VideosSearch(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).ContinueSearch();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).CreateMessageSummary((VideoSummaryStyle)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).CreateVideoSummaries((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).GetChoiceIdFromLabel(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).GetFilterChoice(*(int*)args));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).GetFilterData());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).GetFilterOption(*(int*)args, (VideosFilterChoice)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).GetFirstFilterLocation());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).GetNextChoice(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).HideResultsAndSearch());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).OnSearchQueried((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).OptionSelected(*(int*)args, *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).PerformSearch();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).SetKeywords(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).SetTag(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).ShowPage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).UpdateSearch();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosSearch)GCHandledObjects.GCHandleToObject(instance)).VideoValid(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}
