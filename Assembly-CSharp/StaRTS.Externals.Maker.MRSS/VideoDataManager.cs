using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.Maker.MRSS
{
	public class VideoDataManager
	{
		public delegate void DataQueryCompleteDelegate(string videoGuid);

		public delegate void DataListQueryCompleteDelegate(List<string> videoGuidList);

		private delegate void QueryCompleteDelegate(string json, object callback, object data);

		private readonly string[] EmpireFeaturedTags;

		private readonly string[] RebelFeaturedTags;

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

		public Dictionary<string, string> SourceTypes;

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
			this.EmpireFeaturedTags = new string[]
			{
				"empire"
			};
			this.RebelFeaturedTags = new string[]
			{
				"rebel"
			};
			this.SourceTypes = new Dictionary<string, string>
			{
				{
					"official",
					"hn_ui_official"
				}
			};
			base..ctor();
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
				onQueryComplete((list.Count == 0) ? null : list);
				return;
			}
			Service.Get<Engine>().StartCoroutine(this.Query(this.urlBuilder.UserFeed(), new VideoDataManager.QueryCompleteDelegate(this.ParseFeed), onQueryComplete, null));
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
				List<string> videoGuidList = this.Sections.ContainsKey("featured_videos_section") ? this.Sections["featured_videos_section"] : null;
				onQueryComplete(videoGuidList);
				return;
			}
			this.onFeaturedQueryComplete.Add(onQueryComplete);
			this.GetFeed(new VideoDataManager.DataListQueryCompleteDelegate(this.OnFeaturedFeedAllLoaded));
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
			string[] tags = (faction == FactionType.Empire) ? this.EmpireFeaturedTags : this.RebelFeaturedTags;
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
				return;
			}
			onQueryComplete(list);
		}

		private string GetEnvironmentTag()
		{
			return string.Empty;
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
			if (this.activeQuery != null && this.activeQuery.Active)
			{
				this.activeQuery.AddCallback(onQueryComplete);
				return;
			}
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
			if (keywords.Length != 0)
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
				return;
			}
			if (keywords.Length == 0)
			{
				this.GetFeed(new VideoDataManager.DataListQueryCompleteDelegate(this.OnCategoriesQueried));
			}
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
			VideoData videoData = this.VideoDatas.ContainsKey(guid) ? this.VideoDatas[guid] : null;
			if (videoData != null && videoData.HasDetails)
			{
				onQueryComplete(guid);
				return;
			}
			Service.Get<Engine>().StartCoroutine(this.Query(this.urlBuilder.VideoDetails(guid), new VideoDataManager.QueryCompleteDelegate(this.ParseDetails), onQueryComplete, guid));
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
				return;
			}
			dataQueryCompleteDelegate(null);
		}

		private void Merge(VideoData videoData)
		{
			VideoData videoData2;
			if (this.VideoDatas.TryGetValue(videoData.Guid, out videoData2))
			{
				videoData2.Merge(videoData);
				return;
			}
			this.VideoDatas[videoData.Guid] = videoData;
		}

		[IteratorStateMachine(typeof(VideoDataManager.<Query>d__57))]
		private IEnumerator Query(string url, VideoDataManager.QueryCompleteDelegate onQueryComplete, object callback, object data)
		{
			WWW wWW = new WWW(url);
			WWWManager.Add(wWW);
			yield return wWW;
			if (!WWWManager.Remove(wWW))
			{
				yield break;
			}
			string error = wWW.error;
			if (!string.IsNullOrEmpty(error))
			{
				onQueryComplete(null, callback, data);
			}
			else
			{
				onQueryComplete(wWW.text, callback, data);
			}
			wWW.Dispose();
			yield break;
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

		protected internal VideoDataManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).AddView(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).Clear();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).IsFeedLoaded);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).Keywords);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).Sections);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).Tags);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).VideoDatas);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).GetAllEnvironmentVideos((VideoDataManager.DataListQueryCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).GetEnvironmentTag());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).GetFeatured((VideoDataManager.DataListQueryCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).GetFeaturedAll((VideoDataManager.DataListQueryCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).GetFeed((VideoDataManager.DataListQueryCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).GetVideoDetails(Marshal.PtrToStringUni(*(IntPtr*)args), (VideoDataManager.DataQueryCompleteDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).LoadVideoDataList((List<VideoData>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).Merge((VideoData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).OnCategoriesQueried((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).OnFeaturedFeedAllLoaded((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).ParseDetails(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).ParseFeed(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).ParseKeywords(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).ParseTag(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).ParseTagWithQuery(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).Query(Marshal.PtrToStringUni(*(IntPtr*)args), (VideoDataManager.QueryCompleteDelegate)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]), GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).SearchSubCategories((VideoSection)(*(int*)args), (string[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]), (string[])GCHandledObjects.GCHandleToPinnedArrayObject(args[2]), (VideoDataManager.DataListQueryCompleteDelegate)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).SearchSubCategory(Marshal.PtrToStringUni(*(IntPtr*)args), (VideoDataManager.DataListQueryCompleteDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).IsFeedLoaded = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).Keywords = (Dictionary<string, List<string>>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).Sections = (Dictionary<string, List<string>>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).Tags = (Dictionary<string, List<string>>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((VideoDataManager)GCHandledObjects.GCHandleToObject(instance)).VideoDatas = (Dictionary<string, VideoData>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
