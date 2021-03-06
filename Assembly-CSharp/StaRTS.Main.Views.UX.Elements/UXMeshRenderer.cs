using StaRTS.Assets;
using StaRTS.Main.Views.Cameras;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXMeshRenderer : UXElement
	{
		private MeshRenderer component;

		private AssetManager assetManager;

		private string assetName;

		private AssetHandle assetHandle;

		private Material material;

		public Texture MainTexture
		{
			get
			{
				if (this.material == null && this.component.sharedMaterial)
				{
					return this.component.sharedMaterial.mainTexture;
				}
				if (this.material != null)
				{
					return this.material.mainTexture;
				}
				return null;
			}
			set
			{
				if (this.material == null)
				{
					this.material = UnityUtils.EnsureMaterialCopy(this.component);
				}
				this.material.mainTexture = value;
			}
		}

		public UXMeshRenderer(UXCamera uxCamera, MeshRenderer component) : base(uxCamera, component.gameObject, null)
		{
			this.assetManager = Service.Get<AssetManager>();
			this.component = component;
		}

		public override void InternalDestroyComponent()
		{
			this.UnloadCurrentTexture();
			if (this.material != null)
			{
				this.material.mainTexture = null;
				UnityUtils.DestroyMaterial(this.material);
				this.material = null;
			}
			this.component = null;
		}

		public void LoadTexture(string assetName)
		{
			if (assetName == this.assetName)
			{
				return;
			}
			this.component.gameObject.SetActive(false);
			this.UnloadCurrentTexture();
			this.assetName = assetName;
			this.assetManager.Load(ref this.assetHandle, assetName, new AssetSuccessDelegate(this.OnLoadSuccess), null, null);
		}

		private void OnLoadSuccess(object asset, object cookie)
		{
			this.MainTexture = (Texture2D)asset;
			this.component.gameObject.SetActive(true);
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

		public void SetShader(string shaderName)
		{
			if (this.material == null)
			{
				this.material = UnityUtils.EnsureMaterialCopy(this.component);
			}
			Shader shader = Service.Get<AssetManager>().Shaders.GetShader(shaderName);
			if (shader == null)
			{
				shader = Shader.Find(shaderName);
			}
			if (shader == null)
			{
				Service.Get<StaRTSLogger>().Error("Shader missing: '" + shaderName + "'");
				return;
			}
			this.material.shader = shader;
		}

		public void SetShaderFloat(string nameID, float value)
		{
			if (this.material == null)
			{
				this.material = UnityUtils.EnsureMaterialCopy(this.component);
			}
			this.material.SetFloat(nameID, value);
		}

		protected internal UXMeshRenderer(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXMeshRenderer)GCHandledObjects.GCHandleToObject(instance)).MainTexture);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UXMeshRenderer)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UXMeshRenderer)GCHandledObjects.GCHandleToObject(instance)).LoadTexture(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UXMeshRenderer)GCHandledObjects.GCHandleToObject(instance)).OnLoadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXMeshRenderer)GCHandledObjects.GCHandleToObject(instance)).MainTexture = (Texture)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXMeshRenderer)GCHandledObjects.GCHandleToObject(instance)).SetShader(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXMeshRenderer)GCHandledObjects.GCHandleToObject(instance)).SetShaderFloat(Marshal.PtrToStringUni(*(IntPtr*)args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXMeshRenderer)GCHandledObjects.GCHandleToObject(instance)).UnloadCurrentTexture();
			return -1L;
		}
	}
}
