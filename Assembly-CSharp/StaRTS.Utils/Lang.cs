using StaRTS.Assets;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils
{
	public class Lang
	{
		public const string DEFAULT_LANGUAGE = "en";

		public const string DEFAULT_LOCALE = "en_US";

		public const string AUTO_LOCALIZE_ID_PREFIX = "s_";

		private const string KEY_CONTENT = "content";

		private const string KEY_OBJECTS = "objects";

		private const string KEY_LOCALIZEDSTRINGS = "LocalizedStrings";

		private const string KEY_ID = "uid";

		private const string KEY_TEXT = "text";

		private const string LANG_NOT_FOUND = "LANG: {0} missing";

		private const string LANG_BAD_FORMAT = "LANG: {0} bad format";

		public const string KOREAN_FONT = "malgunbd";

		public const string KOREAN_LOCALE = "ko_KR";

		private const string ID_NULL_OR_EMPTY = "ID_NULL_OR_EMPTY";

		private string locale;

		private Dictionary<string, string> strings;

		private Dictionary<string, string> localeToDisplayLanguage;

		private List<AssetHandle> assetHandles;

		private Font koreanFontInternal;

		private static Lang staticLang;

		private static StringComparer staticComp;

		public bool Initialized
		{
			get;
			set;
		}

		public Font CustomKoreanFont
		{
			get
			{
				if (this.koreanFontInternal == null)
				{
					this.koreanFontInternal = (Font)Resources.Load("malgunbd");
				}
				return this.koreanFontInternal;
			}
			set
			{
				if (this.koreanFontInternal != null)
				{
					Resources.UnloadAsset(this.koreanFontInternal);
				}
				this.koreanFontInternal = value;
			}
		}

		public string Locale
		{
			get
			{
				return this.locale;
			}
			set
			{
				this.locale = value;
				Service.Get<StaRTSLogger>().Debug("Locale: " + this.locale);
				this.ClearStringData();
			}
		}

		public string DotNetLocale
		{
			get
			{
				return Lang.ToDotNetLocale(this.locale);
			}
		}

		public Lang(string locale)
		{
			Service.Set<Lang>(this);
			this.assetHandles = new List<AssetHandle>();
			this.Initialized = false;
			this.strings = new Dictionary<string, string>();
			this.localeToDisplayLanguage = new Dictionary<string, string>();
			this.Locale = locale;
		}

		public string ExtractLanguageFromLocale()
		{
			int num = this.locale.IndexOf('_');
			if (num < 0)
			{
				return this.locale;
			}
			return this.locale.Substring(0, num);
		}

		public static string ToDotNetLocale(string locale)
		{
			return locale.Replace("_", "-");
		}

		public bool IsKorean()
		{
			return this.locale == "ko_KR";
		}

		public void LoadAssets(List<string> assetNames, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, object cookie)
		{
			List<object> list = new List<object>();
			int i = 0;
			int count = assetNames.Count;
			while (i < count)
			{
				this.assetHandles.Add(AssetHandle.Invalid);
				list.Add(null);
				i++;
			}
			Service.Get<AssetManager>().MultiLoad(this.assetHandles, assetNames, onSuccess, onFailure, list, (AssetsCompleteDelegate)cookie, null);
		}

		public void UnloadAssets()
		{
			AssetManager assetManager = Service.Get<AssetManager>();
			int i = 0;
			int count = this.assetHandles.Count;
			while (i < count)
			{
				AssetHandle assetHandle = this.assetHandles[i];
				if (assetHandle != AssetHandle.Invalid)
				{
					assetManager.Unload(assetHandle);
				}
				i++;
			}
			this.assetHandles.Clear();
		}

		public void AddStringData(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				return;
			}
			JsonParser jsonParser = new JsonParser(json);
			object obj = jsonParser.Parse();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null || !dictionary.ContainsKey("content"))
			{
				Service.Get<StaRTSLogger>().Warn("Invalid lang json: no content");
				return;
			}
			dictionary = (dictionary["content"] as Dictionary<string, object>);
			if (dictionary == null || !dictionary.ContainsKey("objects"))
			{
				Service.Get<StaRTSLogger>().Warn("Invalid lang json: no objects");
				return;
			}
			dictionary = (dictionary["objects"] as Dictionary<string, object>);
			if (dictionary == null || !dictionary.ContainsKey("LocalizedStrings"))
			{
				Service.Get<StaRTSLogger>().Warn("Invalid lang json: no strings");
				return;
			}
			List<object> list = dictionary["LocalizedStrings"] as List<object>;
			if (list == null)
			{
				Service.Get<StaRTSLogger>().Warn("Invalid lang json: no entries");
				return;
			}
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
				if (dictionary2 != null && dictionary2.ContainsKey("uid") && dictionary2.ContainsKey("text"))
				{
					string id = dictionary2["uid"] as string;
					string text = dictionary2["text"] as string;
					this.AddIdText(id, text);
				}
				i++;
			}
		}

		public void AddStringData(JoeFile joe)
		{
			if (joe == null)
			{
				return;
			}
			Sheet sheet = null;
			Sheet[] allSheets = joe.GetAllSheets();
			int i = 0;
			int num = allSheets.Length;
			while (i < num)
			{
				sheet = joe.GetSheet(i);
				if (sheet.SheetName == "LocalizedStrings")
				{
					break;
				}
				i++;
			}
			if (sheet == null)
			{
				Service.Get<StaRTSLogger>().Warn("Invalid lang joe file: no strings");
				return;
			}
			int num2 = -1;
			Column[] array = sheet.InternalGetAllColumns();
			int j = 0;
			int num3 = array.Length;
			while (j < num3)
			{
				if (array[j].ColName == "text")
				{
					num2 = j;
					break;
				}
				j++;
			}
			if (num2 < 0)
			{
				Service.Get<StaRTSLogger>().Warn("Invalid lang joe file: no text column");
				return;
			}
			Dictionary<string, Row> allRows = sheet.GetAllRows();
			if (allRows != null)
			{
				foreach (KeyValuePair<string, Row> current in allRows)
				{
					Row value = current.get_Value();
					string text = value.TryGetString(num2);
					if (text != null)
					{
						string key = current.get_Key();
						this.AddIdText(key, text);
					}
				}
			}
		}

		private void AddIdText(string id, string text)
		{
			if (this.strings.ContainsKey(id))
			{
				if (this.strings[id] != text)
				{
					Service.Get<StaRTSLogger>().WarnFormat("String id {0} must have the same translated values for locale {1}", new object[]
					{
						id,
						this.locale
					});
					return;
				}
			}
			else
			{
				this.strings.Add(id, text);
			}
		}

		private void ClearStringData()
		{
			this.strings.Clear();
			this.localeToDisplayLanguage.Clear();
			this.Initialized = false;
		}

		public void SetupAvailableLocales(string locales, string displayLanguages)
		{
			this.localeToDisplayLanguage.Clear();
			if (locales == null || displayLanguages == null)
			{
				Service.Get<StaRTSLogger>().Warn("Invalid locale-language mapping");
				if (locales == null)
				{
					return;
				}
				if (displayLanguages == null)
				{
					displayLanguages = locales;
				}
			}
			string[] array = locales.Split(new char[]
			{
				'|'
			});
			string[] array2 = displayLanguages.Split(new char[]
			{
				'|'
			});
			int num = array.Length;
			if (num != array2.Length)
			{
				Service.Get<StaRTSLogger>().Warn("Mismatched locale-language mapping");
				array2 = array;
			}
			for (int i = 0; i < num; i++)
			{
				string key = array[i];
				if (this.localeToDisplayLanguage.ContainsKey(key))
				{
					Service.Get<StaRTSLogger>().Warn("Duplicate locale in locale-language mapping");
				}
				else
				{
					this.localeToDisplayLanguage.Add(key, array2[i]);
				}
			}
		}

		public string GetDisplayLanguage(string locale)
		{
			if (this.localeToDisplayLanguage == null || !this.localeToDisplayLanguage.ContainsKey(locale))
			{
				return null;
			}
			return this.localeToDisplayLanguage[locale];
		}

		public List<string> GetAvailableLocales()
		{
			List<string> list = new List<string>();
			foreach (string current in this.localeToDisplayLanguage.Keys)
			{
				list.Add(current);
			}
			if (this.GetCultureInfo() == null)
			{
				list.Sort();
			}
			else
			{
				Lang.staticLang = this;
				Lang.staticComp = StringComparer.CurrentCulture;
				try
				{
					list.Sort(new Comparison<string>(Lang.CompareLocaleDisplayLanguage));
				}
				finally
				{
					Lang.staticLang = null;
					Lang.staticComp = null;
				}
			}
			return list;
		}

		private static int CompareLocaleDisplayLanguage(string a, string b)
		{
			return Lang.staticComp.Compare(Lang.staticLang.GetDisplayLanguage(a), Lang.staticLang.GetDisplayLanguage(b));
		}

		private CultureInfo GetCultureInfo()
		{
			CultureInfo cultureInfo = null;
			try
			{
				cultureInfo = new CultureInfo(this.DotNetLocale);
			}
			catch
			{
				cultureInfo = null;
			}
			if (cultureInfo == null)
			{
				try
				{
					cultureInfo = new CultureInfo(Lang.ToDotNetLocale("en_US"));
				}
				catch
				{
					cultureInfo = null;
				}
			}
			return cultureInfo;
		}

		public string ThousandsSeparated(int value)
		{
			CultureInfo cultureInfo = this.GetCultureInfo();
			if (cultureInfo != null)
			{
				return string.Format(cultureInfo, "{0:n0}", new object[]
				{
					value
				});
			}
			return string.Format("{0:n0}", new object[]
			{
				value
			});
		}

		public bool GetOptional(string id, out string text)
		{
			if (this.strings.ContainsKey(id))
			{
				text = this.Get(id, new object[0]);
				return true;
			}
			text = id;
			return false;
		}

		public string Get(string id, params object[] args)
		{
			if (string.IsNullOrEmpty(id))
			{
				Service.Get<StaRTSLogger>().Error("Lang.Get(id) passed null or empty parameter.");
				return "ID_NULL_OR_EMPTY";
			}
			string text;
			if (!this.strings.ContainsKey(id))
			{
				text = (id.StartsWith("s_") ? id : string.Format("LANG: {0} missing", new object[]
				{
					id
				}));
			}
			else
			{
				text = this.strings[id];
				if (args.Length != 0)
				{
					try
					{
						text = string.Format(text, args);
					}
					catch
					{
						text = string.Format("LANG: {0} bad format", new object[]
						{
							id
						});
					}
				}
			}
			if (text.Contains("@"))
			{
				return this.GetMadlibs(text, args);
			}
			return text;
		}

		private string GetMadlibs(string text, params object[] args)
		{
			string[] array = text.Split(new char[]
			{
				'|'
			});
			string text2 = array[0];
			int i = 1;
			int num = array.Length;
			while (i < num)
			{
				string[] array2 = array[i].Split(new char[]
				{
					':'
				});
				string[] array3 = array2[1].Split(new char[]
				{
					'='
				});
				string text3 = "@" + array2[0];
				string type = array3[0];
				string arg = array3[1];
				text2 = text2.Replace(text3, this.DetermineMadlibValue(type, arg));
				i++;
			}
			return text2;
		}

		private string DetermineMadlibValue(string type, string arg)
		{
			if (type == "faction")
			{
				FactionType faction = Service.Get<CurrentPlayer>().Faction;
				if (arg.Equals("player"))
				{
					return LangUtils.GetFactionName(faction);
				}
				if (arg.Equals("enemy"))
				{
					return LangUtils.GetEnemyFactionName(faction);
				}
			}
			else if (type == "locplanet")
			{
				if (arg.Equals("current"))
				{
					return LangUtils.GetPlanetDisplayName(Service.Get<CurrentPlayer>().PlanetId);
				}
			}
			else if (type == "locpref")
			{
				string pref = Service.Get<SharedPlayerPrefs>().GetPref<string>(arg);
				if (pref == null)
				{
					return string.Format("(Pref {0} has not been set)", new object[]
					{
						arg
					});
				}
				return this.Get(pref, new object[0]);
			}
			else if (type == "pref")
			{
				return Service.Get<SharedPlayerPrefs>().GetPref<string>(arg);
			}
			return type + "=" + arg + ":Undefined";
		}

		protected internal Lang(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).AddIdText(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).AddStringData((JoeFile)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).AddStringData(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).ClearStringData();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Lang.CompareLocaleDisplayLanguage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).DetermineMadlibValue(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).ExtractLanguageFromLocale());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).Get(Marshal.PtrToStringUni(*(IntPtr*)args), (object[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).CustomKoreanFont);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).DotNetLocale);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).Initialized);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).Locale);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).GetAvailableLocales());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).GetCultureInfo());
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).GetDisplayLanguage(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).GetMadlibs(Marshal.PtrToStringUni(*(IntPtr*)args), (object[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1])));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).IsKorean());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).LoadAssets((List<string>)GCHandledObjects.GCHandleToObject(*args), (AssetSuccessDelegate)GCHandledObjects.GCHandleToObject(args[1]), (AssetFailureDelegate)GCHandledObjects.GCHandleToObject(args[2]), GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).CustomKoreanFont = (Font)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).Initialized = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).Locale = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).SetupAvailableLocales(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Lang)GCHandledObjects.GCHandleToObject(instance)).ThousandsSeparated(*(int*)args));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Lang.ToDotNetLocale(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((Lang)GCHandledObjects.GCHandleToObject(instance)).UnloadAssets();
			return -1L;
		}
	}
}
