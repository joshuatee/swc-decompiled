using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Assets
{
	public class ManifestEntry
	{
		public AssetType AssetType
		{
			get;
			set;
		}

		public string AssetPath
		{
			get;
			set;
		}

		public ManifestEntry(AssetType assetType, string assetPath)
		{
			this.AssetType = assetType;
			this.AssetPath = assetPath;
		}

		protected internal ManifestEntry(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ManifestEntry)GCHandledObjects.GCHandleToObject(instance)).AssetPath);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ManifestEntry)GCHandledObjects.GCHandleToObject(instance)).AssetType);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ManifestEntry)GCHandledObjects.GCHandleToObject(instance)).AssetPath = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ManifestEntry)GCHandledObjects.GCHandleToObject(instance)).AssetType = (AssetType)(*(int*)args);
			return -1L;
		}
	}
}
