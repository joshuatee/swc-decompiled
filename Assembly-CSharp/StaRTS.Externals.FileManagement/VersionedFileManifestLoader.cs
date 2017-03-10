using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.FileManagement
{
	public class VersionedFileManifestLoader : IManifestLoader
	{
		private const int MAX_LOAD_ATTEMPTS = 3;

		private const float LOAD_ATTEMPT_INTERVAL = 0.5f;

		private FmsOptions options;

		private FmsCallback onComplete;

		private FmsCallback onError;

		private MonoBehaviour engine;

		private IFileManifest manifest;

		private string manifestUrl;

		private StaRTSLogger logger;

		private int loadAttempts;

		public VersionedFileManifestLoader(MonoBehaviour engine)
		{
			this.engine = engine;
			this.logger = Service.Get<StaRTSLogger>();
		}

		public void Load(FmsOptions options, string manifestUrl, FmsCallback onComplete, FmsCallback onError)
		{
			this.options = options;
			this.onComplete = onComplete;
			this.onError = onError;
			this.manifestUrl = manifestUrl;
			this.logger.DebugFormat("Setting manifestUrl to {0}", new object[]
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
				throw new Exception("The versioned manifest has not been instantiated yet. Has VersionedFileManifestLoader.Load been called?");
			}
			return this.manifest;
		}

		private void AttemptManifestRequest(uint id, object cookie)
		{
			int num = this.loadAttempts + 1;
			this.loadAttempts = num;
			if (num > 3)
			{
				this.onError();
				return;
			}
			this.engine.StartCoroutine(this.RequestManifestFile());
		}

		private void RetryRequest()
		{
			Service.Get<ViewTimerManager>().CreateViewTimer(0.5f, false, new TimerDelegate(this.AttemptManifestRequest), null);
		}

		[IteratorStateMachine(typeof(VersionedFileManifestLoader.<RequestManifestFile>d__16))]
		private IEnumerator RequestManifestFile()
		{
			WWW wWW = new WWW(this.manifestUrl);
			WWWManager.Add(wWW);
			yield return wWW;
			if (!WWWManager.Remove(wWW))
			{
				yield break;
			}
			if (wWW.error != null)
			{
				this.logger.ErrorFormat("Unable to request manifest file [{0}] on attempt #{1} with the following error: {2}", new object[]
				{
					this.manifestUrl,
					this.loadAttempts,
					wWW.error
				});
				this.RetryRequest();
			}
			else if (wWW.isDone)
			{
				if (wWW.text != "")
				{
					this.PrepareManifest(wWW.text);
				}
				else
				{
					this.logger.ErrorFormat("Manifest request attempt #{0} yielded an empty manifest.", new object[]
					{
						this.loadAttempts
					});
					this.RetryRequest();
				}
			}
			wWW.Dispose();
			yield break;
		}

		private void PrepareManifest(string json)
		{
			this.manifest = new VersionedFileManifest();
			this.manifest.Prepare(this.options, json);
			this.onComplete();
		}

		protected internal VersionedFileManifestLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).GetManifest());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).IsLoaded());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((VersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).Load((FmsOptions)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (FmsCallback)GCHandledObjects.GCHandleToObject(args[2]), (FmsCallback)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).PrepareManifest(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).RequestManifestFile());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((VersionedFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).RetryRequest();
			return -1L;
		}
	}
}
