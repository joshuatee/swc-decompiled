using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.FileManagement
{
	public class VersionedFileManifest : IFileManifest
	{
		private const string VERSION_KEY = "\"version\":";

		private const string HASHES_KEY = "\"hashes\":{";

		private Dictionary<string, string> hashes;

		private Dictionary<string, string> translatedUrls;

		private List<string> cdnRoots;

		private FmsOptions options;

		private bool isReady;

		private int version;

		private StaRTSLogger logger;

		private static readonly string[] allTargets = new string[]
		{
			"android",
			"android_atc",
			"android_pvrtc",
			"ios",
			"webplayer"
		};

		public VersionedFileManifest()
		{
			this.translatedUrls = new Dictionary<string, string>();
			this.logger = Service.Get<StaRTSLogger>();
		}

		public void Prepare(FmsOptions options, string json)
		{
			this.options = options;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			int i = 0;
			int num = VersionedFileManifest.allTargets.Length;
			while (i < num)
			{
				string text = VersionedFileManifest.allTargets[i];
				if (text != "wsaplayer")
				{
					list.Add("assetbundles/" + text + "/");
					list2.Add("." + text + ".assetbundle");
				}
				i++;
			}
			list2.Add(".json.joe");
			this.ParseHashes(json, list, list2);
			this.ParseVersion(json);
			this.isReady = true;
			this.logger.DebugFormat("Versioned manifest version #{0} is ready with {1} files.", new object[]
			{
				this.version,
				this.hashes.Count
			});
		}

		private bool HashesContainsKey(ref string key)
		{
			if (this.hashes.ContainsKey(key))
			{
				return true;
			}
			key = key.ToLower();
			return this.hashes.ContainsKey(key);
		}

		public string TranslateFileUrl(string relativePath)
		{
			this.AssertReady();
			if (!this.translatedUrls.ContainsKey(relativePath))
			{
				if (!this.HashesContainsKey(ref relativePath))
				{
					this.logger.WarnFormat("Unable to find '{0}' version in the manifest.", new object[]
					{
						relativePath
					});
					return "";
				}
				string text = "{root}{codename}/{environment}/{relativePath}/{hash}.{filename}";
				text = text.Replace("{root}", this.options.RemoteRootUrl);
				text = text.Replace("{codename}", this.options.CodeName);
				text = text.Replace("{environment}", this.options.Env.ToString().ToLower());
				text = text.Replace("{relativePath}", relativePath);
				text = text.Replace("{hash}", this.hashes[relativePath]);
				string text2 = relativePath.Substring(relativePath.LastIndexOf("/") + 1);
				text = text.Replace("{filename}", text2);
				this.translatedUrls[relativePath] = text;
			}
			return this.translatedUrls[relativePath];
		}

		public int GetVersionFromFileUrl(string relativePath)
		{
			this.AssertReady();
			if (!this.HashesContainsKey(ref relativePath))
			{
				this.logger.WarnFormat("Unable to find '{0}' url in the manifest.", new object[]
				{
					relativePath
				});
				return 0;
			}
			string text = this.hashes[relativePath];
			return Convert.ToInt32(text.Substring(text.get_Length() - 8), 16);
		}

		public int GetManifestVersion()
		{
			this.AssertReady();
			return this.version;
		}

		private void AssertReady()
		{
			if (!this.isReady)
			{
				throw new Exception("Versioned manifest is not ready. Call VersionedFileManifest.Prepare first.");
			}
		}

		private void ParseHashes(string json, List<string> ignorePrefix, List<string> ignorePostfix)
		{
			this.hashes = new Dictionary<string, string>();
			if (string.IsNullOrEmpty(json))
			{
				this.logger.Error("Cannot find FMS hashes in empty json");
				return;
			}
			int num = json.IndexOf("\"hashes\":{");
			if (num < 0)
			{
				this.logger.Error("Cannot find FMS hashes in json");
				return;
			}
			num += "\"hashes\":{".get_Length();
			int num2 = 0;
			int num3 = -1;
			string text = null;
			int i = num;
			int length = json.get_Length();
			while (i < length)
			{
				char c = json.get_Chars(i);
				if (c == '"')
				{
					switch (num2)
					{
					case 0:
					case 2:
						num3 = i + 1;
						num2++;
						break;
					case 1:
						text = json.Substring(num3, i - num3);
						num2++;
						break;
					case 3:
						if (!string.IsNullOrEmpty(text))
						{
							bool flag = false;
							int j = 0;
							int count = ignorePrefix.Count;
							while (j < count)
							{
								if (text.StartsWith(ignorePrefix[j]))
								{
									flag = true;
									break;
								}
								j++;
							}
							if (!flag)
							{
								int k = 0;
								int count2 = ignorePostfix.Count;
								while (k < count2)
								{
									if (text.EndsWith(ignorePostfix[k]))
									{
										flag = true;
										break;
									}
									k++;
								}
								if (!flag)
								{
									string value = json.Substring(num3, i - num3);
									this.hashes.Add(text, value);
								}
							}
						}
						num2 = 0;
						break;
					}
				}
				else if (c == '}' && num2 == 0)
				{
					return;
				}
				i++;
			}
		}

		private void ParseVersion(string json)
		{
			this.version = 0;
			if (string.IsNullOrEmpty(json))
			{
				this.logger.Error("Cannot find FMS version in empty json");
				return;
			}
			int num = json.LastIndexOf("\"version\":");
			if (num < 0)
			{
				this.logger.Error("Cannot find FMS version in json");
				return;
			}
			num += "\"version\":".get_Length();
			int length = json.get_Length();
			int num2 = num;
			while (num2 < length && char.IsDigit(json.get_Chars(num2)))
			{
				num2++;
			}
			this.version = int.Parse(json.Substring(num, num2 - num));
		}

		protected internal VersionedFileManifest(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VersionedFileManifest)GCHandledObjects.GCHandleToObject(instance)).AssertReady();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VersionedFileManifest)GCHandledObjects.GCHandleToObject(instance)).GetManifestVersion());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VersionedFileManifest)GCHandledObjects.GCHandleToObject(instance)).GetVersionFromFileUrl(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VersionedFileManifest)GCHandledObjects.GCHandleToObject(instance)).ParseHashes(Marshal.PtrToStringUni(*(IntPtr*)args), (List<string>)GCHandledObjects.GCHandleToObject(args[1]), (List<string>)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((VersionedFileManifest)GCHandledObjects.GCHandleToObject(instance)).ParseVersion(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((VersionedFileManifest)GCHandledObjects.GCHandleToObject(instance)).Prepare((FmsOptions)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VersionedFileManifest)GCHandledObjects.GCHandleToObject(instance)).TranslateFileUrl(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}
