using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using WinRTBridge;

namespace StaRTS.Utils.Json
{
	public class Serializer
	{
		private StringBuilder sb;

		private bool first;

		private const string KEY_VALUE_QUOTED = "\"{0}\":\"{1}\"";

		private const string KEY_VALUE_UNQUOTED = "\"{0}\":{1}";

		private const string KEY_ONLY = "\"{0}\":";

		private const string OBJECT_START = "{";

		private const string OBJECT_END = "}";

		private const string ARRAY_START = "[";

		private const string ARRAY_END = "]";

		private const string COMMA = ",";

		private static readonly Dictionary<string, string> ESCAPE_MAPPING = new Dictionary<string, string>
		{
			{
				"\"",
				"\\\""
			},
			{
				"\\",
				"\\\\"
			},
			{
				"\a",
				"\\a"
			},
			{
				"\b",
				"\\b"
			},
			{
				"\f",
				"\\f"
			},
			{
				"\n",
				"\\n"
			},
			{
				"\r",
				"\\r"
			},
			{
				"\t",
				"\\t"
			},
			{
				"\v",
				"\\v"
			}
		};

		private static readonly Regex ESCAPE_REGEX = new Regex(string.Join("|", Serializer.ESCAPE_MAPPING.Keys.ToArray<string>()));

		private static string Escape(string s)
		{
			if (s != null)
			{
				return Serializer.ESCAPE_REGEX.Replace(s, new MatchEvaluator(Serializer.EscapeMatchEval));
			}
			return s;
		}

		private static string EscapeMatchEval(Match m)
		{
			return Serializer.ESCAPE_MAPPING[m.Value];
		}

		public Serializer()
		{
			this.sb = new StringBuilder();
			this.sb.Append("{");
			this.first = true;
		}

		public static Serializer Start()
		{
			return new Serializer();
		}

		public Serializer End()
		{
			this.sb.Append("}");
			return this;
		}

		public override string ToString()
		{
			return this.sb.ToString();
		}

		public Serializer AddString(string key, string val)
		{
			return this.AddInternal<string>(key, Serializer.Escape(val), "\"{0}\":\"{1}\"");
		}

		public Serializer AddBool(string key, bool val)
		{
			return this.Add<string>(key, val.ToString().ToLower());
		}

		public Serializer Add<T>(string key, T val)
		{
			return this.AddInternal<T>(key, val, "\"{0}\":{1}");
		}

		private Serializer AddInternal<T>(string key, T val, string format)
		{
			this.AppendComma(this.first);
			this.sb.AppendFormat(CultureInfo.InvariantCulture, format, new object[]
			{
				Serializer.Escape(key),
				val
			});
			this.first = false;
			return this;
		}

		public Serializer AddObject<T>(string key, T val) where T : ISerializable
		{
			return this.Add<string>(key, val.ToJson());
		}

		public Serializer AddArray<T>(string key, List<T> values) where T : ISerializable
		{
			this.AppendComma(this.first);
			this.sb.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\":", new object[]
			{
				Serializer.Escape(key)
			});
			this.sb.Append("[");
			bool flag = true;
			foreach (T current in values)
			{
				this.AppendComma(flag);
				this.Add(current);
				flag = false;
			}
			this.sb.Append("]");
			this.first = false;
			return this;
		}

		public Serializer AddDictionary<T>(string key, Dictionary<string, T> values)
		{
			this.AppendComma(this.first);
			this.sb.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\":", new object[]
			{
				Serializer.Escape(key)
			});
			this.sb.Append("{");
			bool flag = typeof(T) == typeof(string);
			this.first = true;
			foreach (KeyValuePair<string, T> current in values)
			{
				if (flag)
				{
					if (current.get_Value() != null)
					{
						string arg_9F_1 = current.get_Key();
						T value = current.get_Value();
						this.AddString(arg_9F_1, value.ToString());
					}
					else
					{
						this.AddString(current.get_Key(), null);
					}
				}
				else
				{
					this.Add<T>(current.get_Key(), current.get_Value());
				}
			}
			this.first = false;
			this.sb.Append("}");
			return this;
		}

		public Serializer AddArrayOfPrimitives<T>(string key, List<T> values)
		{
			this.AppendComma(this.first);
			this.sb.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\":", new object[]
			{
				Serializer.Escape(key)
			});
			this.sb.Append("[");
			bool flag = true;
			bool flag2 = typeof(T) == typeof(string);
			foreach (T current in values)
			{
				this.AppendComma(flag);
				if (flag2)
				{
					this.AddQuoted(current.ToString());
				}
				else
				{
					this.Add(current.ToString());
				}
				flag = false;
			}
			this.sb.Append("]");
			this.first = false;
			return this;
		}

		private void Add(ISerializable val)
		{
			this.Add(val.ToJson());
		}

		private void Add(string val)
		{
			this.sb.Append(val);
		}

		private void AddQuoted(string val)
		{
			this.sb.Append('"');
			this.sb.Append(Serializer.Escape(val));
			this.sb.Append('"');
		}

		private void AppendComma(bool first)
		{
			if (!first)
			{
				this.sb.Append(",");
			}
		}

		protected internal Serializer(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Serializer)GCHandledObjects.GCHandleToObject(instance)).Add((ISerializable)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((Serializer)GCHandledObjects.GCHandleToObject(instance)).Add(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Serializer)GCHandledObjects.GCHandleToObject(instance)).AddBool(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Serializer)GCHandledObjects.GCHandleToObject(instance)).AddQuoted(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Serializer)GCHandledObjects.GCHandleToObject(instance)).AddString(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Serializer)GCHandledObjects.GCHandleToObject(instance)).AppendComma(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Serializer)GCHandledObjects.GCHandleToObject(instance)).End());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Serializer.Escape(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Serializer.EscapeMatchEval((Match)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Serializer.Start());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Serializer)GCHandledObjects.GCHandleToObject(instance)).ToString());
		}
	}
}
