using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Maker.MRSS
{
	public class VideoDataParser
	{
		private const string KEY_ERROR_CODE = "code";

		private const string KEY_ERROR_MESSAGE = "message";

		private const string KEY_GROUPS = "groups";

		private const string KEY_ITEM_GROUPS = "item_groups";

		private const string KEY_SECTIONS = "sections";

		private const string KEY_SECTION_NAME = "internal";

		private const string KEY_VIDEOS = "items";

		public static VideoData ParseDetails(string videoGuid, string jsonSource)
		{
			if (jsonSource == null)
			{
				return null;
			}
			object json;
			if (!VideoDataParser.ParseJSON(jsonSource, out json))
			{
				Service.Get<StaRTSLogger>().Error("Failed to parse VideoDetails JSON");
				return null;
			}
			return new VideoData(videoGuid, json);
		}

		public static Dictionary<string, List<VideoData>> ParseUserFeed(string jsonSource)
		{
			if (jsonSource == null)
			{
				return null;
			}
			object json;
			if (!VideoDataParser.ParseJSON(jsonSource, out json))
			{
				Service.Get<StaRTSLogger>().Error("Failed to parse UserFeed JSON");
				return null;
			}
			Dictionary<string, List<VideoData>> result = new Dictionary<string, List<VideoData>>();
			if (!VideoDataParser.ParseFeed(json, ref result))
			{
				Service.Get<StaRTSLogger>().Error("Failed to parse UserFeed");
				return null;
			}
			return result;
		}

		public static List<VideoData> ParseSearch(string jsonSource)
		{
			object json;
			if (!VideoDataParser.ParseJSON(jsonSource, out json))
			{
				Service.Get<StaRTSLogger>().Error("Failed to parse Search JSON");
				return null;
			}
			Dictionary<string, List<VideoData>> dictionary = new Dictionary<string, List<VideoData>>();
			if (!VideoDataParser.ParseFeed(json, ref dictionary))
			{
				Service.Get<StaRTSLogger>().Error("Failed to parse Search");
				return null;
			}
			using (Dictionary<string, List<VideoData>>.ValueCollection.Enumerator enumerator = dictionary.Values.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		private static bool ParseJSON(string jsonSource, out object jsonObj)
		{
			jsonObj = new JsonParser(jsonSource).Parse();
			Dictionary<string, object> dictionary = jsonObj as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("message") && dictionary.ContainsKey("code"))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Error in JSON - {0} {1}", new object[]
				{
					dictionary["code"],
					dictionary["message"]
				});
				return false;
			}
			return true;
		}

		public static List<string> ParseTag(string jsonSource)
		{
			object obj;
			if (!VideoDataParser.ParseJSON(jsonSource, out obj))
			{
				Service.Get<StaRTSLogger>().Error("Failed to parse Tag JSON");
				return null;
			}
			if (obj == null)
			{
				Service.Get<StaRTSLogger>().Error("Failed to parse jsonSource");
				return null;
			}
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Invalid structure for Tag - {0}", new object[]
				{
					obj.GetType().ToString()
				});
				return null;
			}
			if (!dictionary.ContainsKey("item_groups"))
			{
				Service.Get<StaRTSLogger>().Error("Failed to find Group in Categories");
				return null;
			}
			List<object> list = dictionary["item_groups"] as List<object>;
			if (list == null)
			{
				Service.Get<StaRTSLogger>().Error("Null Group in Categories");
				return null;
			}
			List<string> result = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				if (!VideoDataParser.ParseTagGroup(list[i], ref result))
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Failed to parse Category Group {0}", new object[]
					{
						i
					});
					return null;
				}
			}
			return result;
		}

		private static bool ParseFeed(object json, ref Dictionary<string, List<VideoData>> feed)
		{
			Dictionary<string, object> dictionary = json as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Invalid structure for Feed - {0}", new object[]
				{
					json.GetType().ToString()
				});
				return false;
			}
			if (!dictionary.ContainsKey("groups"))
			{
				Service.Get<StaRTSLogger>().Error("Failed to find Group in JSON");
				return false;
			}
			List<object> list = dictionary["groups"] as List<object>;
			if (list == null)
			{
				Service.Get<StaRTSLogger>().Error("Null Group in JSON");
				return false;
			}
			foreach (object current in list)
			{
				if (!VideoDataParser.ParseGroup(current, ref feed))
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Failed to parse Group {0}", new object[]
					{
						list.IndexOf(current)
					});
				}
			}
			return true;
		}

		private static bool ParseGroup(object json, ref Dictionary<string, List<VideoData>> feed)
		{
			Dictionary<string, object> dictionary = json as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Invalid structure for Group - {0}", new object[]
				{
					json.GetType().ToString()
				});
				return false;
			}
			if (!dictionary.ContainsKey("sections"))
			{
				Service.Get<StaRTSLogger>().Error("Failed to find Sections in JSON");
				return false;
			}
			List<object> list = dictionary["sections"] as List<object>;
			if (list == null)
			{
				Service.Get<StaRTSLogger>().Error("Null Sections in JSON");
				return false;
			}
			foreach (object current in list)
			{
				if (!VideoDataParser.ParseSection(current, ref feed))
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Failed to parse Section {0}", new object[]
					{
						list.IndexOf(current)
					});
				}
			}
			return true;
		}

		private static bool ParseTagGroup(object json, ref List<string> videosReceived)
		{
			Dictionary<string, object> dictionary = json as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Invalid structure for Group - {0}", new object[]
				{
					json.GetType().ToString()
				});
				return false;
			}
			if (!dictionary.ContainsKey("items"))
			{
				Service.Get<StaRTSLogger>().Error("Failed to find Items in JSON");
				return false;
			}
			List<object> list = dictionary["items"] as List<object>;
			if (list == null)
			{
				Service.Get<StaRTSLogger>().Error("Null Items in JSON");
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (!VideoDataParser.ParseTagItem(list[i], ref videosReceived))
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Failed to parse Item {0}", new object[]
					{
						i
					});
				}
			}
			return true;
		}

		private static bool ParseSection(object json, ref Dictionary<string, List<VideoData>> feed)
		{
			Dictionary<string, object> dictionary = json as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Invalid structure for Section - {0}", new object[]
				{
					json.GetType().ToString()
				});
				return false;
			}
			if (!dictionary.ContainsKey("internal"))
			{
				Service.Get<StaRTSLogger>().Error("Failed to find Section Name in JSON");
				return false;
			}
			if (!dictionary.ContainsKey("items"))
			{
				Service.Get<StaRTSLogger>().Error("Failed to find Videos in JSON");
				return false;
			}
			string key = (string)dictionary["internal"];
			feed[key] = VideoDataParser.ParseVideoList(dictionary["items"]);
			return true;
		}

		private static bool ParseTagItem(object json, ref List<string> videosReceived)
		{
			Dictionary<string, object> dictionary = json as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Invalid structure for Tag - {0}", new object[]
				{
					json.GetType().ToString()
				});
				return false;
			}
			if (!dictionary.ContainsKey("internal"))
			{
				Service.Get<StaRTSLogger>().Error("Failed to find Video Name in JSON");
				return false;
			}
			if (!videosReceived.Contains((string)dictionary["internal"]))
			{
				videosReceived.Add((string)dictionary["internal"]);
			}
			return true;
		}

		private static List<VideoData> ParseVideoList(object json)
		{
			List<object> list = json as List<object>;
			List<VideoData> list2 = new List<VideoData>();
			if (list == null)
			{
				return list2;
			}
			foreach (object current in list)
			{
				list2.Add(new VideoData(current));
			}
			return list2;
		}

		public VideoDataParser()
		{
		}

		protected internal VideoDataParser(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(VideoDataParser.ParseDetails(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(VideoDataParser.ParseSearch(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(VideoDataParser.ParseTag(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(VideoDataParser.ParseUserFeed(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(VideoDataParser.ParseVideoList(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
