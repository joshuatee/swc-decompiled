using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

namespace StaRTS.Externals.FileManagement
{
	public class FMS
	{
		private IManifestLoader manifestLoader;

		private string environment;

		public FMS()
		{
			Service.Set<FMS>(this);
		}

		public void Init(FmsOptions options, FmsCallback onReady, FmsCallback onFailed)
		{
			string manifestUrl = string.Empty;
			this.environment = options.Env.ToString().ToLower();
			FmsMode mode = options.Mode;
			if (mode != FmsMode.Passthrough)
			{
				if (mode == FmsMode.Versioned)
				{
					manifestUrl = string.Format("{0}manifest/{1}/{2}/{3}.json", new object[]
					{
						options.RemoteRootUrl,
						options.CodeName,
						options.Env.ToString().ToLower(),
						options.ManifestVersion
					});
					this.manifestLoader = new VersionedFileManifestLoader(options.Engine);
				}
			}
			else
			{
				this.manifestLoader = new PassthroughFileManifestLoader();
			}
			this.manifestLoader.Load(options, manifestUrl, onReady, onFailed);
		}

		public string GetFileUrl(string relativePath)
		{
			if (this.manifestLoader == null)
			{
				Service.Get<Logger>().WarnFormat("A file URL was requested for {0} but the manifest has not been instantiated.", new object[]
				{
					relativePath
				});
				return string.Empty;
			}
			if (this.manifestLoader.IsLoaded())
			{
				return this.manifestLoader.GetManifest().TranslateFileUrl(relativePath);
			}
			Service.Get<Logger>().WarnFormat("A file URL was requested for {0}, but the manifest is not ready.", new object[]
			{
				relativePath
			});
			return string.Empty;
		}

		public int GetFileVersion(string relativePath)
		{
			return this.manifestLoader.GetManifest().GetVersionFromFileUrl(relativePath);
		}

		public int GetManifestVersion()
		{
			return this.manifestLoader.GetManifest().GetManifestVersion();
		}

		public IManifestLoader GetManifestLoader()
		{
			return this.manifestLoader;
		}

		public string GetManifestEnvironment()
		{
			return this.environment;
		}
	}
}
