using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.FileManagement
{
	public class PassthroughFileManifest : IFileManifest
	{
		private FmsOptions options;

		public void Prepare(FmsOptions options, string json)
		{
			this.options = options;
			Service.Get<StaRTSLogger>().Debug("Passthrough manifest is ready.");
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

		public PassthroughFileManifest()
		{
		}

		protected internal PassthroughFileManifest(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PassthroughFileManifest)GCHandledObjects.GCHandleToObject(instance)).GetManifestVersion());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PassthroughFileManifest)GCHandledObjects.GCHandleToObject(instance)).GetVersionFromFileUrl(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PassthroughFileManifest)GCHandledObjects.GCHandleToObject(instance)).Prepare((FmsOptions)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PassthroughFileManifest)GCHandledObjects.GCHandleToObject(instance)).TranslateFileUrl(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}
