using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

public static class Localization
{
	public delegate byte[] LoadFunction(string path);

	public delegate void OnLocalizeNotification();

	public static Localization.LoadFunction loadFunction;

	public static Localization.OnLocalizeNotification onLocalize;

	public static bool localizationHasBeenSet = false;

	private static string[] mLanguages = null;

	private static Dictionary<string, string> mOldDictionary = new Dictionary<string, string>();

	private static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();

	private static Dictionary<string, string> mReplacement = new Dictionary<string, string>();

	private static int mLanguageIndex = -1;

	private static string mLanguage;

	private static bool mMerging = false;

	public static Dictionary<string, string[]> dictionary
	{
		get
		{
			if (!Localization.localizationHasBeenSet)
			{
				Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
			}
			return Localization.mDictionary;
		}
		set
		{
			Localization.localizationHasBeenSet = (value != null);
			Localization.mDictionary = value;
		}
	}

	public static string[] knownLanguages
	{
		get
		{
			if (!Localization.localizationHasBeenSet)
			{
				Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
			}
			return Localization.mLanguages;
		}
	}

	public static string language
	{
		get
		{
			if (string.IsNullOrEmpty(Localization.mLanguage))
			{
				Localization.mLanguage = PlayerPrefs.GetString("Language", "English");
				Localization.LoadAndSelect(Localization.mLanguage);
			}
			return Localization.mLanguage;
		}
		set
		{
			if (Localization.mLanguage != value)
			{
				Localization.mLanguage = value;
				Localization.LoadAndSelect(value);
			}
		}
	}

	[Obsolete("Localization is now always active. You no longer need to check this property.")]
	public static bool isActive
	{
		get
		{
			return true;
		}
	}

	private static bool LoadDictionary(string value)
	{
		byte[] array = null;
		if (!Localization.localizationHasBeenSet)
		{
			if (Localization.loadFunction == null)
			{
				TextAsset textAsset = Resources.Load<TextAsset>("Localization");
				if (textAsset != null)
				{
					array = textAsset.bytes;
				}
			}
			else
			{
				array = Localization.loadFunction("Localization");
			}
			Localization.localizationHasBeenSet = true;
		}
		if (Localization.LoadCSV(array, false))
		{
			return true;
		}
		if (string.IsNullOrEmpty(value))
		{
			value = Localization.mLanguage;
		}
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		if (Localization.loadFunction == null)
		{
			TextAsset textAsset2 = Resources.Load<TextAsset>(value);
			if (textAsset2 != null)
			{
				array = textAsset2.bytes;
			}
		}
		else
		{
			array = Localization.loadFunction(value);
		}
		if (array != null)
		{
			Localization.Set(value, array);
			return true;
		}
		return false;
	}

	private static bool LoadAndSelect(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (Localization.mDictionary.Count == 0 && !Localization.LoadDictionary(value))
			{
				return false;
			}
			if (Localization.SelectLanguage(value))
			{
				return true;
			}
		}
		if (Localization.mOldDictionary.Count > 0)
		{
			return true;
		}
		Localization.mOldDictionary.Clear();
		Localization.mDictionary.Clear();
		if (string.IsNullOrEmpty(value))
		{
			PlayerPrefs.DeleteKey("Language");
		}
		return false;
	}

	public static void Load(TextAsset asset)
	{
		ByteReader byteReader = new ByteReader(asset);
		Localization.Set(asset.name, byteReader.ReadDictionary());
	}

	public static void Set(string languageName, byte[] bytes)
	{
		ByteReader byteReader = new ByteReader(bytes);
		Localization.Set(languageName, byteReader.ReadDictionary());
	}

	public static void ReplaceKey(string key, string val)
	{
		if (!string.IsNullOrEmpty(val))
		{
			Localization.mReplacement[key] = val;
			return;
		}
		Localization.mReplacement.Remove(key);
	}

	public static void ClearReplacements()
	{
		Localization.mReplacement.Clear();
	}

	public static bool LoadCSV(TextAsset asset, bool merge = false)
	{
		return Localization.LoadCSV(asset.bytes, asset, merge);
	}

	public static bool LoadCSV(byte[] bytes, bool merge = false)
	{
		return Localization.LoadCSV(bytes, null, merge);
	}

	private static bool HasLanguage(string languageName)
	{
		int i = 0;
		int num = Localization.mLanguages.Length;
		while (i < num)
		{
			if (Localization.mLanguages[i] == languageName)
			{
				return true;
			}
			i++;
		}
		return false;
	}

	private static bool LoadCSV(byte[] bytes, TextAsset asset, bool merge = false)
	{
		if (bytes == null)
		{
			return false;
		}
		ByteReader byteReader = new ByteReader(bytes);
		BetterList<string> betterList = byteReader.ReadCSV();
		if (betterList.size < 2)
		{
			return false;
		}
		betterList.RemoveAt(0);
		string[] array = null;
		if (string.IsNullOrEmpty(Localization.mLanguage))
		{
			Localization.localizationHasBeenSet = false;
		}
		if (!Localization.localizationHasBeenSet || (!merge && !Localization.mMerging) || Localization.mLanguages == null || Localization.mLanguages.Length == 0)
		{
			Localization.mDictionary.Clear();
			Localization.mLanguages = new string[betterList.size];
			if (!Localization.localizationHasBeenSet)
			{
				Localization.mLanguage = PlayerPrefs.GetString("Language", betterList[0]);
				Localization.localizationHasBeenSet = true;
			}
			for (int i = 0; i < betterList.size; i++)
			{
				Localization.mLanguages[i] = betterList[i];
				if (Localization.mLanguages[i] == Localization.mLanguage)
				{
					Localization.mLanguageIndex = i;
				}
			}
		}
		else
		{
			array = new string[betterList.size];
			for (int j = 0; j < betterList.size; j++)
			{
				array[j] = betterList[j];
			}
			for (int k = 0; k < betterList.size; k++)
			{
				if (!Localization.HasLanguage(betterList[k]))
				{
					int num = Localization.mLanguages.Length + 1;
					Array.Resize<string>(ref Localization.mLanguages, num);
					Localization.mLanguages[num - 1] = betterList[k];
					Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
					foreach (KeyValuePair<string, string[]> current in Localization.mDictionary)
					{
						string[] value = current.get_Value();
						Array.Resize<string>(ref value, num);
						value[num - 1] = value[0];
						dictionary.Add(current.get_Key(), value);
					}
					Localization.mDictionary = dictionary;
				}
			}
		}
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		for (int l = 0; l < Localization.mLanguages.Length; l++)
		{
			dictionary2.Add(Localization.mLanguages[l], l);
		}
		while (true)
		{
			BetterList<string> betterList2 = byteReader.ReadCSV();
			if (betterList2 == null || betterList2.size == 0)
			{
				break;
			}
			if (!string.IsNullOrEmpty(betterList2[0]))
			{
				Localization.AddCSV(betterList2, array, dictionary2);
			}
		}
		if (!Localization.mMerging && Localization.onLocalize != null)
		{
			Localization.mMerging = true;
			Localization.OnLocalizeNotification onLocalizeNotification = Localization.onLocalize;
			Localization.onLocalize = null;
			onLocalizeNotification();
			Localization.onLocalize = onLocalizeNotification;
			Localization.mMerging = false;
		}
		return true;
	}

	private static void AddCSV(BetterList<string> newValues, string[] newLanguages, Dictionary<string, int> languageIndices)
	{
		if (newValues.size < 2)
		{
			return;
		}
		string text = newValues[0];
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		string[] value = Localization.ExtractStrings(newValues, newLanguages, languageIndices);
		if (Localization.mDictionary.ContainsKey(text))
		{
			Localization.mDictionary[text] = value;
			if (newLanguages == null)
			{
				Debug.LogWarning("Localization key '" + text + "' is already present");
				return;
			}
		}
		else
		{
			try
			{
				Localization.mDictionary.Add(text, value);
			}
			catch (Exception ex)
			{
				Debug.LogError("Unable to add '" + text + "' to the Localization dictionary.\n" + ex.get_Message());
			}
		}
	}

	private static string[] ExtractStrings(BetterList<string> added, string[] newLanguages, Dictionary<string, int> languageIndices)
	{
		if (newLanguages == null)
		{
			string[] array = new string[Localization.mLanguages.Length];
			int i = 1;
			int num = Mathf.Min(added.size, array.Length + 1);
			while (i < num)
			{
				array[i - 1] = added[i];
				i++;
			}
			return array;
		}
		string key = added[0];
		string[] array2;
		if (!Localization.mDictionary.TryGetValue(key, out array2))
		{
			array2 = new string[Localization.mLanguages.Length];
		}
		int j = 0;
		int num2 = newLanguages.Length;
		while (j < num2)
		{
			string key2 = newLanguages[j];
			int num3 = languageIndices[key2];
			array2[num3] = added[j + 1];
			j++;
		}
		return array2;
	}

	private static bool SelectLanguage(string language)
	{
		Localization.mLanguageIndex = -1;
		if (Localization.mDictionary.Count == 0)
		{
			return false;
		}
		int i = 0;
		int num = Localization.mLanguages.Length;
		while (i < num)
		{
			if (Localization.mLanguages[i] == language)
			{
				Localization.mOldDictionary.Clear();
				Localization.mLanguageIndex = i;
				Localization.mLanguage = language;
				PlayerPrefs.SetString("Language", Localization.mLanguage);
				if (Localization.onLocalize != null)
				{
					Localization.onLocalize();
				}
				UIRoot.Broadcast("OnLocalize");
				return true;
			}
			i++;
		}
		return false;
	}

	public static void Set(string languageName, Dictionary<string, string> dictionary)
	{
		Localization.mLanguage = languageName;
		PlayerPrefs.SetString("Language", Localization.mLanguage);
		Localization.mOldDictionary = dictionary;
		Localization.localizationHasBeenSet = true;
		Localization.mLanguageIndex = -1;
		Localization.mLanguages = new string[]
		{
			languageName
		};
		if (Localization.onLocalize != null)
		{
			Localization.onLocalize();
		}
		UIRoot.Broadcast("OnLocalize");
	}

	public static void Set(string key, string value)
	{
		if (Localization.mOldDictionary.ContainsKey(key))
		{
			Localization.mOldDictionary[key] = value;
			return;
		}
		Localization.mOldDictionary.Add(key, value);
	}

	public static string Get(string key)
	{
		if (!Localization.localizationHasBeenSet)
		{
			Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
		}
		if (Localization.mLanguages == null)
		{
			Debug.LogError("No localization data present");
			return null;
		}
		string language = Localization.language;
		if (Localization.mLanguageIndex == -1)
		{
			for (int i = 0; i < Localization.mLanguages.Length; i++)
			{
				if (Localization.mLanguages[i] == language)
				{
					Localization.mLanguageIndex = i;
					break;
				}
			}
		}
		if (Localization.mLanguageIndex == -1)
		{
			Localization.mLanguageIndex = 0;
			Localization.mLanguage = Localization.mLanguages[0];
			Debug.LogWarning("Language not found: " + language);
		}
		UICamera.ControlScheme currentScheme = UICamera.currentScheme;
		string result;
		string[] array;
		if (currentScheme == UICamera.ControlScheme.Touch)
		{
			string key2 = key + " Mobile";
			if (Localization.mReplacement.TryGetValue(key2, out result))
			{
				return result;
			}
			if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(key2, out array) && Localization.mLanguageIndex < array.Length)
			{
				return array[Localization.mLanguageIndex];
			}
			if (Localization.mOldDictionary.TryGetValue(key2, out result))
			{
				return result;
			}
		}
		else if (currentScheme == UICamera.ControlScheme.Controller)
		{
			string key3 = key + " Controller";
			if (Localization.mReplacement.TryGetValue(key3, out result))
			{
				return result;
			}
			if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(key3, out array) && Localization.mLanguageIndex < array.Length)
			{
				return array[Localization.mLanguageIndex];
			}
			if (Localization.mOldDictionary.TryGetValue(key3, out result))
			{
				return result;
			}
		}
		if (Localization.mReplacement.TryGetValue(key, out result))
		{
			return result;
		}
		if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(key, out array))
		{
			if (Localization.mLanguageIndex < array.Length)
			{
				string text = array[Localization.mLanguageIndex];
				if (string.IsNullOrEmpty(text))
				{
					text = array[0];
				}
				return text;
			}
			return array[0];
		}
		else
		{
			if (Localization.mOldDictionary.TryGetValue(key, out result))
			{
				return result;
			}
			return key;
		}
	}

	public static string Format(string key, params object[] parameters)
	{
		return string.Format(Localization.Get(key), parameters);
	}

	[Obsolete("Use Localization.Get instead")]
	public static string Localize(string key)
	{
		return Localization.Get(key);
	}

	public static bool Exists(string key)
	{
		if (!Localization.localizationHasBeenSet)
		{
			Localization.language = PlayerPrefs.GetString("Language", "English");
		}
		return Localization.mDictionary.ContainsKey(key) || Localization.mOldDictionary.ContainsKey(key);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		Localization.AddCSV((BetterList<string>)GCHandledObjects.GCHandleToObject(*args), (string[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		Localization.ClearReplacements();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.Exists(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.ExtractStrings((BetterList<string>)GCHandledObjects.GCHandleToObject(*args), (string[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[2])));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.Format(Marshal.PtrToStringUni(*(IntPtr*)args), (object[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1])));
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.Get(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.dictionary);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.isActive);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.knownLanguages);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.language);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.HasLanguage(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		Localization.Load((TextAsset)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.LoadAndSelect(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.LoadCSV((byte[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.LoadCSV((TextAsset)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.LoadCSV((byte[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), (TextAsset)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0));
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.LoadDictionary(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.Localize(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		Localization.ReplaceKey(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Localization.SelectLanguage(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		Localization.Set(Marshal.PtrToStringUni(*(IntPtr*)args), (byte[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		Localization.Set(Marshal.PtrToStringUni(*(IntPtr*)args), (Dictionary<string, string>)GCHandledObjects.GCHandleToObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		Localization.Set(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		Localization.dictionary = (Dictionary<string, string[]>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		Localization.language = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}
}
