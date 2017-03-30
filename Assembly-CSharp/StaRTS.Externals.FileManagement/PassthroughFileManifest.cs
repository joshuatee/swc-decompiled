using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

namespace StaRTS.Externals.FileManagement
{
	public class PassthroughFileManifest : IFileManifest
	{
		private FmsOptions options;

		public void Prepare(FmsOptions options, string json)
		{
			this.options = options;
			Service.Get<Logger>().Debug("Passthrough manifest is ready.");
		}

		public string TranslateFileUrl(string relativePath)
		{
			return this.options.LocalRootUrl + relativePath;
		}

		public int GetVersionFromFileUrl(string relativePath)
		{
			return 0;
		}

		public int GetManifestVersion()
		{
			return 0;
		}
	}
}
