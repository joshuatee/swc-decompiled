using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

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
			string manifestUrl = "";
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
				Service.Get<StaRTSLogger>().WarnFormat("A file URL was requested for {0} but the manifest has not been instantiated.", new object[]
				{
					relativePath
				});
				return "";
			}
			if (this.manifestLoader.IsLoaded())
			{
				return this.manifestLoader.GetManifest().TranslateFileUrl(relativePath);
			}
			Service.Get<StaRTSLogger>().WarnFormat("A file URL was requested for {0}, but the manifest is not ready.", new object[]
			{
				relativePath
			});
			return "";
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

		protected internal FMS(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FMS)GCHandledObjects.GCHandleToObject(instance)).GetFileUrl(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FMS)GCHandledObjects.GCHandleToObject(instance)).GetFileVersion(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FMS)GCHandledObjects.GCHandleToObject(instance)).GetManifestEnvironment());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FMS)GCHandledObjects.GCHandleToObject(instance)).GetManifestLoader());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FMS)GCHandledObjects.GCHandleToObject(instance)).GetManifestVersion());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((FMS)GCHandledObjects.GCHandleToObject(instance)).Init((FmsOptions)GCHandledObjects.GCHandleToObject(*args), (FmsCallback)GCHandledObjects.GCHandleToObject(args[1]), (FmsCallback)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}
	}
}
