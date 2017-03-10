using StaRTS.Utils;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Externals.Maker.MRSS
{
	internal class QueryURLBuilder
	{
		private const string HOST = "http://mobileapi.makerstudios.com";

		private const string HOST_VERSION = "v1";

		private const string HOST_SITECODE = "swcommander";

		private const string HMAC_KEY = "fd84f83863a613bf37fa";

		private const string AUTHORIZATION_KEY = "authorization";

		private const string USERFEED_PATH = "userFeed";

		private const string VIDEO_PATH = "video";

		private const string SEARCH_PATH = "search";

		private const string TAG_TAG = "tag";

		private const string SEARCH_QUERY_KEY = "q";

		private const string SEARCH_ORDER_KEY = "order";

		private const string SEARCH_ORDER_ASC = "relevance-asc";

		private const string SEARCH_ORDER_DESC = "relevance-desc";

		private const string TAGSEARCH_PATH = "tag";

		private const string STARTAT_KEY = "startat";

		private const string LIMITTO_KEY = "limitto";

		private static readonly char[] BYTE2HEXCHAR = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f'
		};

		private StringBuilder urlString;

		public string UserFeed()
		{
			this.BuildBaseURL("userFeed");
			return this.GenerateHashedURL();
		}

		public string VideoDetails(string guid)
		{
			this.BuildBaseURL("video");
			this.urlString.Append("/").Append(guid);
			return this.GenerateHashedURL();
		}

		public string Search(string query, bool ascending = true, int startAt = -1, int limitTo = -1)
		{
			this.BuildBaseURL("search");
			query = query.Trim();
			if (query.get_Length() == 0)
			{
				query = " ";
			}
			this.AppendParameter("q", Uri.EscapeDataString(query));
			this.AppendParameter("order", ascending ? "relevance-asc" : "relevance-desc");
			if (startAt != -1)
			{
				this.AppendParameter("startat", startAt.ToString());
			}
			if (limitTo != -1)
			{
				this.AppendParameter("limitto", limitTo.ToString());
			}
			return this.GenerateHashedURL();
		}

		public string Tag(string tag)
		{
			this.BuildBaseURL("tag");
			this.urlString.Append("/").Append(Uri.EscapeDataString(tag));
			return this.GenerateHashedURL();
		}

		private void BuildBaseURL(string path)
		{
			this.urlString.set_Length(0);
			this.urlString.AppendFormat(CultureInfo.InvariantCulture, "{0}/{1}/{2}/{3}", new object[]
			{
				"http://mobileapi.makerstudios.com",
				"v1",
				"swcommander",
				path
			});
		}

		private void AppendParameter(string key, string value)
		{
			this.urlString.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}={2}", new object[]
			{
				this.HasParameter() ? "&" : "?",
				key,
				value
			});
		}

		private string GenerateHashedURL()
		{
			byte[] array = CryptographyUtils.ComputeHmacHash("HmacSHA256", "fd84f83863a613bf37fa", this.urlString.ToString());
			this.urlString.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}=", new object[]
			{
				this.HasParameter() ? "&" : "?",
				"authorization"
			});
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				this.urlString.Append(QueryURLBuilder.BYTE2HEXCHAR[(b & 240) >> 4]);
				this.urlString.Append(QueryURLBuilder.BYTE2HEXCHAR[(int)(b & 15)]);
			}
			return this.urlString.ToString();
		}

		private bool HasParameter()
		{
			for (int i = this.urlString.get_Length() - 1; i >= 0; i--)
			{
				char c = this.urlString.get_Chars(i);
				if (c == '?')
				{
					return true;
				}
				if (c == '/')
				{
					return false;
				}
			}
			return false;
		}

		public QueryURLBuilder()
		{
			this.urlString = new StringBuilder();
			base..ctor();
		}

		protected internal QueryURLBuilder(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((QueryURLBuilder)GCHandledObjects.GCHandleToObject(instance)).AppendParameter(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((QueryURLBuilder)GCHandledObjects.GCHandleToObject(instance)).BuildBaseURL(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryURLBuilder)GCHandledObjects.GCHandleToObject(instance)).GenerateHashedURL());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryURLBuilder)GCHandledObjects.GCHandleToObject(instance)).HasParameter());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryURLBuilder)GCHandledObjects.GCHandleToObject(instance)).Search(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0, *(int*)(args + 2), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryURLBuilder)GCHandledObjects.GCHandleToObject(instance)).Tag(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryURLBuilder)GCHandledObjects.GCHandleToObject(instance)).UserFeed());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryURLBuilder)GCHandledObjects.GCHandleToObject(instance)).VideoDetails(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}
