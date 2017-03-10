using System;

namespace StaRTS.Externals.FileManagement
{
	public interface IFileManifest
	{
		string TranslateFileUrl(string relativePath);

		int GetVersionFromFileUrl(string relativePath);

		void Prepare(FmsOptions options, string json);

		int GetManifestVersion();
	}
}
