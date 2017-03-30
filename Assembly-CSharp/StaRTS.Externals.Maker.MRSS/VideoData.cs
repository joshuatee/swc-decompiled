using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

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

		private string[] ThumbnailURLs = new string[6];

		private string[] VideoURLs = new string[2];

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
				return DateTime.UtcNow - this.Timestamp;
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

		public VideoData()
		{
		}

		public VideoData(object json)
		{
			this.Parse(json);
		}

		public VideoData(string guid, object json)
		{
			this.Parse(json);
			this.Guid = guid;
		}

		public string GetVideoURL(VideoQuality quality)
		{
			return this.VideoURLs[(int)quality];
		}

		public string GetThumbnailURL(ThumbnailSize size)
		{
			return this.ThumbnailURLs[(int)size];
		}

		public void Parse(object json)
		{
			Dictionary<string, object> dictionary = json as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<Logger>().Error("JSON Root is not a Dictionary");
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
			long millis;
			if (string.IsNullOrEmpty(jSONString))
			{
				Service.Get<Logger>().ErrorFormat("Expecting '{0}' or '{1}' in JSON", new object[]
				{
					"original_published_at",
					"uploaded_at"
				});
			}
			else if (!long.TryParse(jSONString, out millis))
			{
				Service.Get<Logger>().ErrorFormat("Failed to parse timestamp from JSON - '{0}'", new object[]
				{
					jSONString
				});
			}
			else
			{
				this.Timestamp = DateUtils.DateFromMillis(millis);
			}
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
				Service.Get<Logger>().ErrorFormat("Expecting '{0}' as string in JSON. Found '{1}'", new object[]
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
			if (!int.TryParse(jSONString, out result))
			{
				Service.Get<Logger>().ErrorFormat("Failed to parse int from JSON - '{0}'", new object[]
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
				Service.Get<Logger>().ErrorFormat("Expecting 'Dictionary' in JSON. Found '{0}'", new object[]
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
				Service.Get<Logger>().ErrorFormat("Expecting '{0}' as Dictionary in JSON. Found '{1}'", new object[]
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
				Service.Get<Logger>().ErrorFormat("Expecting '{0}' as List in JSON. Found '{1}'", new object[]
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
					Service.Get<Logger>().ErrorFormat("Expecting '{0}' in JSON", new object[]
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
			this.Viewcount++;
		}
	}
}
