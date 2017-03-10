using StaRTS.Assets;
using StaRTS.Main.Controllers;
using StaRTS.Main.Views.Cameras;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXTexture : UXElement
	{
		private UXTextureComponent component;

		private AssetManager assetManager;

		private string assetName;

		private AssetHandle assetHandle;

		private bool textureHasBeenSet;

		private UXSprite spinner;

		private Action onLoadCompleteCallback;

		private Action onLoadFailedCallback;

		public Texture MainTexture
		{
			get
			{
				return this.component.MainTexture;
			}
			set
			{
				this.DestroyCurrentTexture();
				this.textureHasBeenSet = true;
				this.component.MainTexture = value;
			}
		}

		public UXTexture(UXCamera uxCamera, UXTextureComponent component) : base(uxCamera, component.gameObject, null)
		{
			this.textureHasBeenSet = false;
			this.assetManager = Service.Get<AssetManager>();
			this.component = component;
			this.onLoadCompleteCallback = null;
		}

		public override void InternalDestroyComponent()
		{
			this.component.Texture = null;
			UnityEngine.Object.Destroy(this.component);
		}

		public void DeferTextureForLoad(string assetName)
		{
			this.assetName = assetName;
			this.UnloadCurrentTexture();
			this.spinner = Service.Get<UXController>().MiscElementsManager.GetHolonetLoader(this);
			this.spinner.LocalPosition = Vector3.zero;
		}

		public void LoadDeferred()
		{
			this.assetManager.Load(ref this.assetHandle, this.assetName, new AssetSuccessDelegate(this.OnLoadSuccess), null, null);
		}

		public void LoadTexture(string assetName)
		{
			this.LoadTexture(assetName, null);
		}

		public void LoadTexture(string assetName, Action onLoadComplete)
		{
			this.LoadTexture(assetName, onLoadComplete, null);
		}

		public void LoadTexture(string assetName, Action onLoadComplete, Action onLoadFail)
		{
			this.onLoadCompleteCallback = onLoadComplete;
			this.onLoadFailedCallback = onLoadFail;
			if (assetName == this.assetName || this.component == null || this.component.gameObject == null)
			{
				return;
			}
			this.component.gameObject.SetActive(false);
			this.UnloadCurrentTexture();
			this.assetName = assetName;
			this.assetManager.Load(ref this.assetHandle, assetName, new AssetSuccessDelegate(this.OnLoadSuccess), new AssetFailureDelegate(this.OnLoadFailure), null);
		}

		private void OnLoadFailure(object cookie)
		{
			if (this.onLoadFailedCallback != null)
			{
				this.onLoadFailedCallback.Invoke();
			}
		}

		private void OnLoadSuccess(object asset, object cookie)
		{
			if (this.onLoadCompleteCallback != null)
			{
				this.onLoadCompleteCallback.Invoke();
			}
			if (this.component != null)
			{
				this.MainTexture = (Texture2D)asset;
				this.component.gameObject.SetActive(true);
				if (this.spinner != null)
				{
					this.spinner.OnDestroyElement();
					UnityEngine.Object.Destroy(this.spinner.Root);
					this.spinner = null;
					return;
				}
			}
			else
			{
				this.UnloadCurrentTexture();
				this.DestroyCurrentTexture();
			}
		}

		private void DestroyCurrentTexture()
		{
			if (this.textureHasBeenSet && this.component.MainTexture != null)
			{
				this.component.MainTexture = null;
			}
		}

		private void UnloadCurrentTexture()
		{
			if (this.assetHandle != AssetHandle.Invalid)
			{
				this.assetManager.Unload(this.assetHandle);
				this.assetHandle = AssetHandle.Invalid;
				this.assetName = null;
			}
		}

		public override void OnDestroyElement()
		{
			this.onLoadCompleteCallback = null;
			this.UnloadCurrentTexture();
			this.DestroyCurrentTexture();
			base.OnDestroyElement();
		}

		protected internal UXTexture(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).DeferTextureForLoad(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).DestroyCurrentTexture();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTexture)GCHandledObjects.GCHandleToObject(instance)).MainTexture);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).LoadDeferred();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).LoadTexture(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).LoadTexture(Marshal.PtrToStringUni(*(IntPtr*)args), (Action)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).LoadTexture(Marshal.PtrToStringUni(*(IntPtr*)args), (Action)GCHandledObjects.GCHandleToObject(args[1]), (Action)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).OnLoadFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).OnLoadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).MainTexture = (Texture)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXTexture)GCHandledObjects.GCHandleToObject(instance)).UnloadCurrentTexture();
			return -1L;
		}
	}
}
