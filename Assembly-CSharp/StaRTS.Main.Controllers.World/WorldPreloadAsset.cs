using StaRTS.Assets;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class WorldPreloadAsset
	{
		public AssetHandle Handle;

		public string AssetName
		{
			get;
			private set;
		}

		public GameObject GameObj
		{
			get;
			set;
		}

		public WorldPreloadAsset(string assetName)
		{
			this.AssetName = assetName;
			this.Handle = AssetHandle.Invalid;
			this.GameObj = null;
		}

		protected internal WorldPreloadAsset(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldPreloadAsset)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldPreloadAsset)GCHandledObjects.GCHandleToObject(instance)).GameObj);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WorldPreloadAsset)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WorldPreloadAsset)GCHandledObjects.GCHandleToObject(instance)).GameObj = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
