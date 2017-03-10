using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.FileManagement
{
	public class PassthroughFileManifestLoader : IManifestLoader
	{
		private IFileManifest manifest;

		public void Load(FmsOptions options, string manifestUrl, FmsCallback onComplete, FmsCallback onError)
		{
			this.manifest = new PassthroughFileManifest();
			this.manifest.Prepare(options, "");
			Caching.CleanCache();
			onComplete();
		}

		public bool IsLoaded()
		{
			return this.manifest != null;
		}

		public IFileManifest GetManifest()
		{
			if (!this.IsLoaded())
			{
				throw new Exception("The passthrough manifest has not been instantiated. Has PassthroughFileManifestLoader.Load been called?");
			}
			return this.manifest;
		}

		public PassthroughFileManifestLoader()
		{
		}

		protected internal PassthroughFileManifestLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PassthroughFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).GetManifest());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PassthroughFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).IsLoaded());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PassthroughFileManifestLoader)GCHandledObjects.GCHandleToObject(instance)).Load((FmsOptions)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (FmsCallback)GCHandledObjects.GCHandleToObject(args[2]), (FmsCallback)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}
	}
}
