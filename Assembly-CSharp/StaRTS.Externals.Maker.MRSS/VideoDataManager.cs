using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace StaRTS.Externals.Maker.MRSS
{
	public class VideoDataManager
	{
		public delegate void DataQueryCompleteDelegate(string videoGuid);

		public delegate void DataListQueryCompleteDelegate(List<string> videoGuidList);

		private delegate void QueryCompleteDelegate(string json, object callback, object data);

		private const string SECTION_FEATURED = "featured_videos_section";

		private const string SECTION_RECENT = "most_recent_videos_section";

		public const string TAG_WEEK_IN_DAYS = "7";

		public const string TAG_MONTH_IN_DAYS = "30";

		public const string TAG_OFFICIAL = "official";

		public const string TAG_REBEL = "rebel";

		public const string TAG_EMPIRE = "empire";

		public const string TAG_PROD = "Production";

		private const string HN_UI_MAKER = "hn_ui_partner";

		private const string HN_UI_OFFICIAL = "hn_ui_official";

		private readonly string[] EmpireFeaturedTags = new string[]
		{
			"empire"
		};

		private readonly string[] RebelFeaturedTags = new string[]
		{
			"rebel"
		};

		public Dictionary<string, string> SourceTypes = new Dictionary<string, string>
		{
			{
				"official",
				"hn_ui_official"
			}
		};

		private QueryURLBuilder urlBuilder;

		private List<VideoDataManager.DataListQueryCompleteDelegate> onFeaturedQueryComplete;

		private QueryData activeQuery;

		public bool IsFeedLoaded
		{
			get;
			internal set;
		}

		public Dictionary<string, VideoData> VideoDatas
		{
			get;
			internal set;
		}

		public Dictionary<string, List<string>> Sections
		{
			get;
			internal set;
		}

		public Dictionary<string, List<string>> Tags
		{
			get;
			internal set;
		}

		public Dictionary<string, List<string>> Keywords
		{
			get;
			internal set;
		}

		public VideoDataManager()
		{
			this.VideoDatas = new Dictionary<string, VideoData>();
			this.Sections = new Dictionary<string, List<string>>();
			this.Tags = new Dictionary<string, List<string>>();
			this.Keywords = new Dictionary<string, List<string>>();
			this.onFeaturedQueryComplete = new List<VideoDataManager.DataListQueryCompleteDelegate>();
			this.activeQuery = null;
			this.urlBuilder = new QueryURLBuilder();
			Service.Set<VideoDataManager>(this);
		}

		public void Clear()
		{
			this.VideoDatas.Clear();
			this.Sections.Clear();
			this.Tags.Clear();
			this.Keywords.Clear();
			this.IsFeedLoaded = false;
		}

		public void GetFeed(VideoDataManager.DataListQueryCompleteDelegate onQueryComplete)
		{
			if (this.IsFeedLoaded)
			{
				List<string> list = new List<string>();
				foreach (VideoData current in this.VideoDatas.Values)
				{
					list.Add(current.Guid);
				}
				onQueryComplete((list.Count != 0) ? list : null);
			}
			else
			{
				Service.Get<Engine>().StartCoroutine(this.Query(this.urlBuilder.UserFeed(), new VideoDataManager.QueryCompleteDelegate(this.ParseFeed), onQueryComplete, null));
			}
		}

		private void ParseFeed(string json, object callback, object data)
		{
			Dictionary<string, List<VideoData>> dictionary = VideoDataParser.ParseUserFeed(json);
			if (dictionary != null)
			{
				foreach (string current in dictionary.Keys)
				{
					this.Sections[current] = this.LoadVideoDataList(dictionary[current]);
				}
			}
			this.IsFeedLoaded = true;
			this.GetFeed((VideoDataManager.DataListQueryCompleteDelegate)callback);
		}

		private void GetFeaturedAll(VideoDataManager.DataListQueryCompleteDelegate onQueryComplete)
		{
			if (this.IsFeedLoaded)
			{
				List<string> videoGuidList = (!this.Sections.ContainsKey("featured_videos_section")) ? null : this.Sections["featured_videos_section"];
				onQueryComplete(videoGuidList);
			}
			else
			{
				this.onFeaturedQueryComplete.Add(onQueryComplete);
				this.GetFeed(new VideoDataManager.DataListQueryCompleteDelegate(this.OnFeaturedFeedAllLoaded));
			}
		}

		private void OnFeaturedFeedAllLoaded(List<string> videoGuids)
		{
			for (int i = 0; i < this.onFeaturedQueryComplete.Count; i++)
			{
				this.GetFeaturedAll(this.onFeaturedQueryComplete[i]);
			}
			this.onFeaturedQueryComplete.Clear();
		}

		public void GetFeatured(VideoDataManager.DataListQueryCompleteDelegate onQueryComplete)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			FactionType faction = currentPlayer.Faction;
			string[] tags = (faction != FactionType.Empire) ? this.RebelFeaturedTags : this.EmpireFeaturedTags;
			string[] keywords = new string[0];
			this.SearchSubCategories(VideoSection.FEATURED, tags, keywords, onQueryComplete);
		}

		public void SearchSubCategory(string tag, VideoDataManager.DataListQueryCompleteDelegate onQueryComplete)
		{
			List<string> list;
			this.Tags.TryGetValue(tag, out list);
			if (list == null)
			{
				Service.Get<Engine>().StartCoroutine(this.Query(this.urlBuilder.Tag(tag), new VideoDataManager.QueryCompleteDelegate(this.ParseTag), onQueryComplete, tag));
			}
			else
			{
				onQueryComplete(list);
			}
		}

		private string GetEnvironmentTag()
		{
			return "Production";
		}

		public void GetAllEnvironmentVideos(VideoDataManager.DataListQueryCompleteDelegate onQueryComplete)
		{
			string[] tags = new string[0];
			string[] keywords = new string[0];
			Service.Get<VideoDataManager>().SearchSubCategories(VideoSection.ALL, tags, keywords, onQueryComplete);
		}

		public void SearchSubCategories(VideoSection section, string[] tags, string[] keywords, VideoDataManager.DataListQueryCompleteDelegate onQueryComplete)
		{
			string environmentTag = this.GetEnvironmentTag();
			if (!string.IsNullOrEmpty(environmentTag) && Array.IndexOf<string>(tags, environmentTag) < 0)
			{
				string[] array = new string[tags.Length + 1];
				Array.Copy(tags, array, tags.Length);
				array[tags.Length] = environmentTag;
				tags = array;
			}
			if (tags.Length == 0 && keywords.Length == 0)
			{
				this.GetFeed(onQueryComplete);
				return;
			}
			if (this.activeQuery != null && !this.activeQuery.QueryMatch(section, tags, keywords))
			{
				this.activeQuery.Active = false;
			}
			if (this.activeQuery == null || !this.activeQuery.Active)
			{
				this.activeQuery = new QueryData(section, tags, keywords, onQueryComplete);
				for (int i = 0; i < tags.Length; i++)
				{
					List<string> list;
					this.Tags.TryGetValue(tags[i], out list);
					if (list == null)
					{
						Service.Get<Engine>().StartCoroutine(this.Query(this.urlBuilder.Tag(tags[i]), new VideoDataManager.QueryCompleteDelegate(this.ParseTagWithQuery), this.activeQuery, tags[i]));
					}
					else
					{
						this.activeQuery.AddResult(new List<string>(list), false);
						this.activeQuery.FilterResults();
					}
				}
				if (keywords.Length > 0)
				{
					string text = string.Join(" ", keywords);
					List<string> list2;
					this.Keywords.TryGetValue(text, out list2);
					if (list2 == null)
					{
						Service.Get<Engine>().StartCoroutine(this.Query(this.urlBuilder.Search(text, true, -1, -1), new VideoDataManager.QueryCompleteDelegate(this.ParseKeywords), this.activeQuery, text));
					}
					else
					{
						this.activeQuery.AddResult(new List<string>(list2), false);
						this.activeQuery.FilterResults();
					}
				}
				if (section == VideoSection.FEATURED)
				{
					this.GetFeaturedAll(new VideoDataManager.DataListQueryCompleteDelegate(this.OnCategoriesQueried));
				}
				else if (keywords.Length == 0)
				{
					this.GetFeed(new VideoDataManager.DataListQueryCompleteDelegate(this.OnCategoriesQueried));
				}
				return;
			}
			this.activeQuery.AddCallback(onQueryComplete);
		}

		private void OnCategoriesQueried(List<string> videoGuidList)
		{
			if (this.activeQuery == null || !this.activeQuery.Active)
			{
				return;
			}
			this.activeQuery.AddResult(videoGuidList, this.activeQuery.Section == VideoSection.FEATURED);
			this.activeQuery.FilterResults();
		}

		private void ParseTagWithQuery(string json, object query, object data)
		{
			QueryData queryData = (QueryData)query;
			if (!queryData.Active)
			{
				return;
			}
			List<string> list = null;
			if (!string.IsNullOrEmpty(json))
			{
				list = VideoDataParser.ParseTag(json);
			}
			string key = (string)data;
			this.Tags[key] = list;
			queryData.AddResult(list, false);
			queryData.FilterResults();
		}

		private void ParseTag(string json, object callback, object data)
		{
			VideoDataManager.DataListQueryCompleteDelegate dataListQueryCompleteDelegate = (VideoDataManager.DataListQueryCompleteDelegate)callback;
			string key = (string)data;
			List<string> list = null;
			if (!string.IsNullOrEmpty(json))
			{
				list = VideoDataParser.ParseTag(json);
				this.Tags[key] = list;
			}
			dataListQueryCompleteDelegate(list);
		}

		private void ParseKeywords(string json, object query, object data)
		{
			QueryData queryData = (QueryData)query;
			if (!queryData.Active)
			{
				return;
			}
			List<VideoData> list = null;
			if (json != null)
			{
				list = VideoDataParser.ParseSearch(json);
			}
			string key = (string)data;
			List<string> list2 = this.LoadVideoDataList(list);
			this.Keywords[key] = list2;
			queryData.AddResult(list2, false);
			queryData.FilterResults();
		}

		public void GetVideoDetails(string guid, VideoDataManager.DataQueryCompleteDelegate onQueryComplete)
		{
			VideoData videoData = (!this.VideoDatas.ContainsKey(guid)) ? null : this.VideoDatas[guid];
			if (videoData != null && videoData.HasDetails)
			{
				onQueryComplete(guid);
			}
			else
			{
				Service.Get<Engine>().StartCoroutine(this.Query(this.urlBuilder.VideoDetails(guid), new VideoDataManager.QueryCompleteDelegate(this.ParseDetails), onQueryComplete, guid));
			}
		}

		private void ParseDetails(string json, object callback, object data)
		{
			VideoDataManager.DataQueryCompleteDelegate dataQueryCompleteDelegate = (VideoDataManager.DataQueryCompleteDelegate)callback;
			string videoGuid = (string)data;
			VideoData videoData = VideoDataParser.ParseDetails(videoGuid, json);
			if (videoData != null)
			{
				this.Merge(videoData);
				dataQueryCompleteDelegate(videoData.Guid);
			}
			else
			{
				dataQueryCompleteDelegate(null);
			}
		}

		private void Merge(VideoData videoData)
		{
			VideoData videoData2;
			if (this.VideoDatas.TryGetValue(videoData.Guid, out videoData2))
			{
				videoData2.Merge(videoData);
			}
			else
			{
				this.VideoDatas[videoData.Guid] = videoData;
			}
		}

		[DebuggerHidden]
		private IEnumerator Query(string url, VideoDataManager.QueryCompleteDelegate onQueryComplete, object callback, object data)
		{
			VideoDataManager.<Query>c__Iterator11 <Query>c__Iterator = new VideoDataManager.<Query>c__Iterator11();
			<Query>c__Iterator.url = url;
			<Query>c__Iterator.onQueryComplete = onQueryComplete;
			<Query>c__Iterator.callback = callback;
			<Query>c__Iterator.data = data;
			<Query>c__Iterator.<$>url = url;
			<Query>c__Iterator.<$>onQueryComplete = onQueryComplete;
			<Query>c__Iterator.<$>callback = callback;
			<Query>c__Iterator.<$>data = data;
			return <Query>c__Iterator;
		}

		private List<string> LoadVideoDataList(List<VideoData> list)
		{
			List<string> list2 = new List<string>(list.Count);
			foreach (VideoData current in list)
			{
				this.Merge(current);
				list2.Add(current.Guid);
			}
			return list2;
		}

		public void AddView(string guid)
		{
			VideoData videoData = this.VideoDatas[guid];
			if (videoData != null)
			{
				videoData.AddView();
			}
		}
	}
}
