using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Maker.MRSS
{
	public class VideoData
	{
		private const string KEY_GUID = "internal";

		private const string KEY_TITLE = "title";

		private const string KEY_DESCRIPTION = "description";

		private const string KEY_AUTHOR = "show_name_internal";

		private const string KEY_VIDEO = "play_urls";

		private const string KEY_VIDEO_LOW = "play_url_lo";

		private const string KEY_VIDEO_HIGH = "play_url_hi";

		private const string KEY_THUMBNAIL = "images";

		private const string KEY_THUMBNAIL_TINY = "t";

		private const string KEY_THUMBNAIL_SMALL = "s";

		private const string KEY_THUMBNAIL_MEDIUM = "m";

		private const string KEY_THUMBNAIL_LARGE = "l";

		private const string KEY_THUMBNAIL_XLARGE = "xl";

		private const string KEY_THUMBNAIL_XXLARGE = "xxl";

		private const string KEY_PUBDATE = "original_published_at";

		private const string KEY_UPLOADDATE = "uploaded_at";

		private const string KEY_DURATION = "duration";

		private const string KEY_VIEWCOUNT = "view_count";

		private const string KEY_INTERNAL = "internal";

		private const string KEY_YOUTUBE_ID = "youtube_id";

		private string[] ThumbnailURLs;

		private string[] VideoURLs;

		public string Guid
		{
			get;
			internal set;
		}

		public int Length
		{
			get;
			internal set;
		}

		public string VideoURL
		{
			get
			{
				return this.GetVideoURL(VideoQuality.HIGH);
			}
			internal set
			{
				for (int i = 0; i < 2; i++)
				{
					this.VideoURLs[i] = value;
				}
			}
		}

		public string ThumbnailURL
		{
			get
			{
				return this.GetThumbnailURL(ThumbnailSize.XXLARGE);
			}
			internal set
			{
				for (int i = 0; i < 6; i++)
				{
					this.ThumbnailURLs[i] = value;
				}
			}
		}

		public string Title
		{
			get;
			internal set;
		}

		public string Description
		{
			get;
			internal set;
		}

		public DateTime Timestamp
		{
			get;
			internal set;
		}

		public TimeSpan Age
		{
			get
			{
				return DateTime.get_UtcNow() - this.Timestamp;
			}
		}

		public string Author
		{
			get;
			internal set;
		}

		public int Viewcount
		{
			get;
			internal set;
		}

		public string YoutubeId
		{
			get;
			internal set;
		}

		public bool HasDetails
		{
			get
			{
				for (int i = 0; i < 2; i++)
				{
					if (!string.IsNullOrEmpty(this.VideoURLs[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		public string GetVideoURL(VideoQuality quality)
		{
			return this.VideoURLs[(int)quality];
		}

		public string GetThumbnailURL(ThumbnailSize size)
		{
			return this.ThumbnailURLs[(int)size];
		}

		public VideoData()
		{
			this.ThumbnailURLs = new string[6];
			this.VideoURLs = new string[2];
			base..ctor();
		}

		public VideoData(object json)
		{
			this.ThumbnailURLs = new string[6];
			this.VideoURLs = new string[2];
			base..ctor();
			this.Parse(json);
		}

		public VideoData(string guid, object json)
		{
			this.ThumbnailURLs = new string[6];
			this.VideoURLs = new string[2];
			base..ctor();
			this.Parse(json);
			this.Guid = guid;
		}

		public void Parse(object json)
		{
			Dictionary<string, object> dictionary = json as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().Error("JSON Root is not a Dictionary");
				return;
			}
			this.Guid = this.GetJSONString(dictionary, "internal", false);
			this.Title = this.GetJSONString(dictionary, "title", true);
			this.Author = this.GetJSONString(dictionary, "show_name_internal", false);
			this.Length = this.GetJSONInt(dictionary, "duration", true);
			this.ParseThumbnailURLs(dictionary, "images");
			this.ParseTimestamp(dictionary);
			this.Viewcount = this.GetJSONInt(dictionary, "view_count", true);
			this.YoutubeId = this.GetJSONString(dictionary, "youtube_id", false);
			this.Description = this.GetJSONString(dictionary, "description", false);
			this.ParseVideoURLs(dictionary, "play_urls");
		}

		private void ParseVideoURLs(Dictionary<string, object> json, string key)
		{
			Dictionary<string, object> jSONDictionary = this.GetJSONDictionary(json, key, false);
			if (jSONDictionary == null)
			{
				return;
			}
			this.VideoURLs[0] = this.GetJSONString(jSONDictionary, "play_url_lo", true);
			this.VideoURLs[1] = this.GetJSONString(jSONDictionary, "play_url_hi", true);
		}

		private void ParseThumbnailURLs(Dictionary<string, object> json, string key)
		{
			Dictionary<string, object> jSONDictionary = this.GetJSONDictionary(json, key, true);
			if (jSONDictionary == null)
			{
				return;
			}
			this.ThumbnailURLs[0] = this.GetJSONString(jSONDictionary, "t", true);
			this.ThumbnailURLs[1] = this.GetJSONString(jSONDictionary, "s", true);
			this.ThumbnailURLs[2] = this.GetJSONString(jSONDictionary, "m", true);
			this.ThumbnailURLs[3] = this.GetJSONString(jSONDictionary, "l", true);
			this.ThumbnailURLs[4] = this.GetJSONString(jSONDictionary, "xl", true);
			this.ThumbnailURLs[5] = this.GetJSONString(jSONDictionary, "xxl", true);
		}

		private void ParseTimestamp(Dictionary<string, object> json)
		{
			string jSONString = this.GetJSONString(json, "original_published_at", false);
			if (string.IsNullOrEmpty(jSONString))
			{
				jSONString = this.GetJSONString(json, "uploaded_at", false);
			}
			if (string.IsNullOrEmpty(jSONString))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Expecting '{0}' or '{1}' in JSON", new object[]
				{
					"original_published_at",
					"uploaded_at"
				});
				return;
			}
			long millis;
			if (!long.TryParse(jSONString, ref millis))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Failed to parse timestamp from JSON - '{0}'", new object[]
				{
					jSONString
				});
				return;
			}
			this.Timestamp = DateUtils.DateFromMillis(millis);
		}

		private string GetJSONString(Dictionary<string, object> data, string key, bool required = true)
		{
			if (!this.HasJSONKey(data, key, required))
			{
				return null;
			}
			object obj = data[key];
			if (!(obj is string))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Expecting '{0}' as string in JSON. Found '{1}'", new object[]
				{
					key,
					obj.GetType().ToString()
				});
				return string.Empty;
			}
			return (string)obj;
		}

		private int GetJSONInt(Dictionary<string, object> data, string key, bool required = true)
		{
			string jSONString = this.GetJSONString(data, key, required);
			if (string.IsNullOrEmpty(jSONString))
			{
				return 0;
			}
			int result;
			if (!int.TryParse(jSONString, ref result))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Failed to parse int from JSON - '{0}'", new object[]
				{
					jSONString
				});
				return 0;
			}
			return result;
		}

		private Dictionary<string, object> ToJSONDictionary(object data)
		{
			Dictionary<string, object> dictionary = data as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Expecting 'Dictionary' in JSON. Found '{0}'", new object[]
				{
					data.GetType().ToString()
				});
				return null;
			}
			return dictionary;
		}

		private Dictionary<string, object> GetJSONDictionary(Dictionary<string, object> data, string key, bool required = true)
		{
			if (!this.HasJSONKey(data, key, required))
			{
				return null;
			}
			Dictionary<string, object> dictionary = data[key] as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Expecting '{0}' as Dictionary in JSON. Found '{1}'", new object[]
				{
					key,
					data.GetType().ToString()
				});
				return null;
			}
			return dictionary;
		}

		private List<object> GetJSONList(Dictionary<string, object> data, string key, bool required = true)
		{
			if (!this.HasJSONKey(data, key, required))
			{
				return null;
			}
			List<object> list = data[key] as List<object>;
			if (list == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Expecting '{0}' as List in JSON. Found '{1}'", new object[]
				{
					key,
					data.GetType().ToString()
				});
				return null;
			}
			return list;
		}

		private bool HasJSONKey(Dictionary<string, object> data, string key, bool required)
		{
			if (!data.ContainsKey(key))
			{
				if (required)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Expecting '{0}' in JSON", new object[]
					{
						key
					});
				}
				return false;
			}
			return true;
		}

		public void Merge(VideoData videoData)
		{
			this.Length = videoData.Length;
			for (int i = 0; i < 2; i++)
			{
				if (!string.IsNullOrEmpty(videoData.VideoURLs[i]))
				{
					this.VideoURLs[i] = videoData.VideoURLs[i];
				}
			}
			for (int j = 0; j < 6; j++)
			{
				if (!string.IsNullOrEmpty(videoData.ThumbnailURLs[j]))
				{
					this.ThumbnailURLs[j] = videoData.ThumbnailURLs[j];
				}
			}
			if (!string.IsNullOrEmpty(videoData.Title))
			{
				this.Title = videoData.Title;
			}
			if (!string.IsNullOrEmpty(videoData.Description))
			{
				this.Description = videoData.Description;
			}
			if (this.Timestamp < videoData.Timestamp)
			{
				this.Timestamp = videoData.Timestamp;
			}
			if (!string.IsNullOrEmpty(videoData.Author))
			{
				this.Author = videoData.Author;
			}
			if (videoData.Viewcount > this.Viewcount)
			{
				this.Viewcount = videoData.Viewcount;
			}
			if (!string.IsNullOrEmpty(videoData.YoutubeId))
			{
				this.YoutubeId = videoData.YoutubeId;
			}
		}

		public void AddView()
		{
			int viewcount = this.Viewcount;
			this.Viewcount = viewcount + 1;
		}

		protected internal VideoData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).AddView();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).Age);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).Author);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).Guid);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).HasDetails);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).Length);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).ThumbnailURL);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).Timestamp);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).Title);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).VideoURL);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).Viewcount);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).YoutubeId);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).GetJSONDictionary((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).GetJSONInt((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).GetJSONList((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).GetJSONString((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).GetThumbnailURL((ThumbnailSize)(*(int*)args)));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).GetVideoURL((VideoQuality)(*(int*)args)));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).HasJSONKey((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).Merge((VideoData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).Parse(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).ParseThumbnailURLs((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).ParseTimestamp((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).ParseVideoURLs((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).Author = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).Description = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).Guid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).Length = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).ThumbnailURL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).Timestamp = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).Title = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).VideoURL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).Viewcount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((VideoData)GCHandledObjects.GCHandleToObject(instance)).YoutubeId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoData)GCHandledObjects.GCHandleToObject(instance)).ToJSONDictionary(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
