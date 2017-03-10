using StaRTS.Assets;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class WorldPreloader
	{
		public delegate void PreloadSuccessDelegate();

		private WorldPreloader.PreloadSuccessDelegate onPreloadSuccess;

		private int numAssetsRemainingToLoad;

		private Dictionary<string, Queue<WorldPreloadAsset>> preloadedAssets;

		public WorldPreloader()
		{
			Service.Set<WorldPreloader>(this);
			this.preloadedAssets = new Dictionary<string, Queue<WorldPreloadAsset>>();
		}

		public void Load(List<IAssetVO> assetsToLoad, WorldPreloader.PreloadSuccessDelegate onPreloadSuccess)
		{
			if (this.numAssetsRemainingToLoad != 0)
			{
				Service.Get<StaRTSLogger>().Error("WorldPreloader.Load() is called when it's still loading!");
				return;
			}
			int num = (assetsToLoad == null) ? 0 : assetsToLoad.Count;
			if (num == 0)
			{
				onPreloadSuccess();
				return;
			}
			this.onPreloadSuccess = onPreloadSuccess;
			this.numAssetsRemainingToLoad = num;
			AssetManager assetManager = Service.Get<AssetManager>();
			for (int i = 0; i < num; i++)
			{
				string assetName = assetsToLoad[i].AssetName;
				WorldPreloadAsset worldPreloadAsset = new WorldPreloadAsset(assetName);
				assetManager.Load(ref worldPreloadAsset.Handle, assetName, new AssetSuccessDelegate(this.OnAssetLoadSuccess), new AssetFailureDelegate(this.OnAssetLoadFailure), worldPreloadAsset);
				Queue<WorldPreloadAsset> queue;
				if (this.preloadedAssets.ContainsKey(assetName))
				{
					queue = this.preloadedAssets[assetName];
				}
				else
				{
					queue = new Queue<WorldPreloadAsset>();
					this.preloadedAssets.Add(assetName, queue);
				}
				queue.Enqueue(worldPreloadAsset);
			}
		}

		public void Unload()
		{
			AssetManager assetManager = Service.Get<AssetManager>();
			foreach (Queue<WorldPreloadAsset> current in this.preloadedAssets.Values)
			{
				int i = 0;
				int count = current.Count;
				while (i < count)
				{
					WorldPreloadAsset worldPreloadAsset = current.Dequeue();
					if (worldPreloadAsset.GameObj != null && assetManager.IsAssetCloned(worldPreloadAsset.AssetName))
					{
						UnityEngine.Object.Destroy(worldPreloadAsset.GameObj);
						worldPreloadAsset.GameObj = null;
					}
					if (worldPreloadAsset.Handle != AssetHandle.Invalid)
					{
						assetManager.Unload(worldPreloadAsset.Handle);
						worldPreloadAsset.Handle = AssetHandle.Invalid;
					}
					i++;
				}
			}
			this.preloadedAssets.Clear();
		}

		public WorldPreloadAsset GetPreloadedAsset(string assetName)
		{
			WorldPreloadAsset worldPreloadAsset = null;
			if (this.preloadedAssets.ContainsKey(assetName))
			{
				Queue<WorldPreloadAsset> queue = this.preloadedAssets[assetName];
				worldPreloadAsset = queue.Dequeue();
				if (queue.Count == 0)
				{
					this.preloadedAssets.Remove(assetName);
				}
				if (worldPreloadAsset.GameObj != null)
				{
					worldPreloadAsset.GameObj.SetActive(true);
				}
			}
			return worldPreloadAsset;
		}

		private void OnAssetLoadFailure(object cookie)
		{
			this.OnAssetLoadSuccess(null, cookie);
		}

		private void OnAssetLoadSuccess(object asset, object cookie)
		{
			if (asset != null)
			{
				GameObject gameObject = asset as GameObject;
				gameObject.SetActive(false);
				WorldPreloadAsset worldPreloadAsset = (WorldPreloadAsset)cookie;
				worldPreloadAsset.GameObj = gameObject;
			}
			int num = this.numAssetsRemainingToLoad - 1;
			this.numAssetsRemainingToLoad = num;
			if (num == 0)
			{
				this.onPreloadSuccess();
			}
		}

		protected internal WorldPreloader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldPreloader)GCHandledObjects.GCHandleToObject(instance)).GetPreloadedAsset(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WorldPreloader)GCHandledObjects.GCHandleToObject(instance)).Load((List<IAssetVO>)GCHandledObjects.GCHandleToObject(*args), (WorldPreloader.PreloadSuccessDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WorldPreloader)GCHandledObjects.GCHandleToObject(instance)).OnAssetLoadFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WorldPreloader)GCHandledObjects.GCHandleToObject(instance)).OnAssetLoadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((WorldPreloader)GCHandledObjects.GCHandleToObject(instance)).Unload();
			return -1L;
		}
	}
}
