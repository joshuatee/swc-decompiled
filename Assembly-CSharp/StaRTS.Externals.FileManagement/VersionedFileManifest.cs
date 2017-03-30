using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

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

		private Logger logger;

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
			this.logger = Service.Get<Logger>();
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
				if (text != "android")
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
					return string.Empty;
				}
				string text = "{root}{codename}/{environment}/{relativePath}/{hash}.{filename}";
				text = text.Replace("{root}", this.options.RemoteRootUrl);
				text = text.Replace("{codename}", this.options.CodeName);
				text = text.Replace("{environment}", this.options.Env.ToString().ToLower());
				text = text.Replace("{relativePath}", relativePath);
				text = text.Replace("{hash}", this.hashes[relativePath]);
				string newValue = relativePath.Substring(relativePath.LastIndexOf("/") + 1);
				text = text.Replace("{filename}", newValue);
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
			return Convert.ToInt32(text.Substring(text.Length - 8), 16);
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
			num += "\"hashes\":{".Length;
			int num2 = 0;
			int num3 = -1;
			string text = null;
			int i = num;
			int length = json.Length;
			while (i < length)
			{
				char c = json[i];
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
			num += "\"version\":".Length;
			int length = json.Length;
			int i;
			for (i = num; i < length; i++)
			{
				if (!char.IsDigit(json[i]))
				{
					break;
				}
			}
			this.version = int.Parse(json.Substring(num, i - num));
		}
	}
}
