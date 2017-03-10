using StaRTS.Assets;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class ProjectorAssetProcessor
	{
		private GeometryProjector parent;

		private Action<GeometryProjector> onLoadedCompleteCallback;

		private Action<GeometryProjector> onLoadedFailureCallback;

		private List<AssetHandle> handles;

		private List<GameObject> orphans;

		private int outstandingAssets;

		private const string OUTLINE_OUTER_PARAM = "_Outline";

		private const string OUTLINE_INNER_PARAM = "_OutlineInnerWidth";

		private const float NO_OUTLINE = 0f;

		public ProjectorAssetProcessor(GeometryProjector parent)
		{
			this.parent = parent;
			this.handles = new List<AssetHandle>();
			this.orphans = new List<GameObject>();
		}

		public void LoadAllAssets(Action<GeometryProjector> successCallback, Action<GeometryProjector> failureCallback)
		{
			this.onLoadedCompleteCallback = successCallback;
			this.onLoadedFailureCallback = failureCallback;
			AssetManager assetManager = Service.Get<AssetManager>();
			assetManager.Add3DModelToManifest(this.parent.Config.AssetName);
			AssetHandle item = AssetHandle.Invalid;
			this.outstandingAssets++;
			assetManager.Load(ref item, this.parent.Config.AssetName, new AssetSuccessDelegate(this.OnAssetSuccess), new AssetFailureDelegate(this.OnAssetFailure), this.parent.Config.AssetName);
			this.handles.Add(item);
			if (this.parent.Config.AttachmentAssets != null)
			{
				int i = 0;
				int num = this.parent.Config.AttachmentAssets.Length;
				while (i < num)
				{
					string text = this.parent.Config.AttachmentAssets[i];
					assetManager.Add3DModelToManifest(text);
					AssetHandle item2 = AssetHandle.Invalid;
					this.outstandingAssets++;
					assetManager.Load(ref item2, text, new AssetSuccessDelegate(this.OnAssetSuccess), new AssetFailureDelegate(this.OnAssetFailure), text);
					this.handles.Add(item2);
					i++;
				}
			}
		}

		public void Reset()
		{
			AssetManager assetManager = Service.Get<AssetManager>();
			assetManager.Add3DModelToManifest(this.parent.Config.AssetName);
			if (this.parent.Config.AttachmentAssets != null)
			{
				int i = 0;
				int num = this.parent.Config.AttachmentAssets.Length;
				while (i < num)
				{
					string assetName = this.parent.Config.AttachmentAssets[i];
					assetManager.Add3DModelToManifest(assetName);
					i++;
				}
			}
		}

		private void ExecuteCallback(uint id, object cookie)
		{
			Action<GeometryProjector> action = cookie as Action<GeometryProjector>;
			if (action != null)
			{
				action.Invoke(this.parent);
			}
		}

		private void OnAssetSuccess(object asset, object cookie)
		{
			string text = cookie as string;
			bool flag = !Service.Get<AssetManager>().IsAssetCloned(text);
			GameObject gameObject;
			if (flag)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(asset as GameObject);
			}
			else
			{
				gameObject = (asset as GameObject);
			}
			if (text == this.parent.Config.AssetName)
			{
				this.parent.Config.MainAsset = gameObject;
				int i = 0;
				int count = this.orphans.Count;
				while (i < count)
				{
					this.orphans[i].transform.parent = this.parent.Config.MainAsset.transform;
					i++;
				}
				this.orphans.Clear();
				AssetMeshDataMonoBehaviour component = gameObject.GetComponent<AssetMeshDataMonoBehaviour>();
				if (component != null && component.ShadowGameObject != null)
				{
					component.ShadowGameObject.SetActive(false);
				}
			}
			else if (this.parent.Config.MainAsset == null)
			{
				this.orphans.Add(gameObject);
			}
			else
			{
				gameObject.transform.parent = this.parent.Config.MainAsset.transform;
			}
			this.outstandingAssets--;
			if (this.outstandingAssets > 0)
			{
				return;
			}
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
			int j = 0;
			int num = componentsInChildren.Length;
			while (j < num)
			{
				Renderer renderer = componentsInChildren[j];
				if (renderer.sharedMaterial != null && renderer.sharedMaterial.HasProperty("_OutlineInnerWidth") && renderer.sharedMaterial.HasProperty("_Outline"))
				{
					renderer.sharedMaterial.SetFloat("_Outline", 0f);
					renderer.sharedMaterial.SetFloat("_OutlineInnerWidth", 0f);
				}
				j++;
			}
			this.parent.Config.AssetReady = true;
			this.ExecuteCallback(0u, this.onLoadedCompleteCallback);
		}

		private void OnAssetFailure(object cookie)
		{
			Service.Get<StaRTSLogger>().ErrorFormat("Unable to load asset {0} for projector", new object[]
			{
				cookie as string
			});
			this.ExecuteCallback(0u, this.onLoadedFailureCallback);
		}

		public void UnloadAllAssets(Action<GeometryProjector> callback)
		{
			AssetManager assetManager = Service.Get<AssetManager>();
			this.onLoadedCompleteCallback = null;
			this.onLoadedFailureCallback = null;
			this.DestroyAssets();
			int i = 0;
			int count = this.handles.Count;
			while (i < count)
			{
				assetManager.Unload(this.handles[i]);
				i++;
			}
			this.handles.Clear();
			this.parent.Config.AssetReady = false;
			this.ExecuteCallback(0u, callback);
		}

		private void DestroyAssets()
		{
			GameObject mainAsset = this.parent.Config.MainAsset;
			this.parent.Config.MainAsset = null;
			UnityEngine.Object.Destroy(mainAsset);
		}

		protected internal ProjectorAssetProcessor(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ProjectorAssetProcessor)GCHandledObjects.GCHandleToObject(instance)).DestroyAssets();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ProjectorAssetProcessor)GCHandledObjects.GCHandleToObject(instance)).LoadAllAssets((Action<GeometryProjector>)GCHandledObjects.GCHandleToObject(*args), (Action<GeometryProjector>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ProjectorAssetProcessor)GCHandledObjects.GCHandleToObject(instance)).OnAssetFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ProjectorAssetProcessor)GCHandledObjects.GCHandleToObject(instance)).OnAssetSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ProjectorAssetProcessor)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ProjectorAssetProcessor)GCHandledObjects.GCHandleToObject(instance)).UnloadAllAssets((Action<GeometryProjector>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
