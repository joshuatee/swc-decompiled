using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.FileManagement
{
	public class ServerVersionedFileManifestLoader : IManifestLoader
	{
		private const int MAX_LOAD_ATTEMPTS = 3;

		private const float LOAD_ATTEMPT_INTERVAL = 0.5f;

		private FmsOptions options;

		private FmsCallback onComplete;

		private IFileManifest manifest;

		private string manifestUrl;

		private int loadAttempts;

		public void Load(FmsOptions options, string manifestUrl, FmsCallback onComplete, FmsCallback onError)
		{
			this.options = options;
			this.onComplete = onComplete;
			this.manifestUrl = manifestUrl;
			Service.Get<StaRTSLogger>().DebugFormat("Setting manifestUrl to {0}", new object[]
			{
				manifestUrl
			});
			this.AttemptManifestRequest(0u, null);
		}

		public bool IsLoaded()
		{
			return this.manifest != null;
		}

		public IFileManifest GetManifest()
		{
			if (!this.IsLoaded())
			{
				throw new Exception("The versioned manifest has not been instantiated yet.");
			}
			return this.manifest;
		}

		private void AttemptManifestRequest(uint id, object cookie)
		{
		}

		private void PrepareManifest(string json)
		{
			this.manifest = new VersionedFileManifest();
			this.manifest.Prepare(this.options, json);
			if (this.onComplete != null)
			{
				this.onComplete();
			}
		}

		public ServerVersionedFileManifestLoader()
		{
		}

		protected internal ServerVersionedFileManifestLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerVersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).GetManifest());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerVersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).IsLoaded());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ServerVersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).Load((FmsOptions)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (FmsCallback)GCHandledObjects.GCHandleToObject(args[2]), (FmsCallback)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ServerVersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).PrepareManifest(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
