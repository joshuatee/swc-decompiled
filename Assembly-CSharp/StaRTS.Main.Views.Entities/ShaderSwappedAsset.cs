using StaRTS.Assets;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class ShaderSwappedAsset
	{
		protected GameObject gameObject;

		protected List<Material> shaderSwappedMaterials;

		protected List<Material> cleanUpList;

		protected Dictionary<int, Material> oldMaterials;

		private string shaderName;

		public ShaderSwappedAsset()
		{
			this.shaderSwappedMaterials = null;
			this.cleanUpList = new List<Material>();
			this.oldMaterials = new Dictionary<int, Material>();
		}

		public void Init(GameObject gameObj, string shaderName)
		{
			this.gameObject = gameObj;
			this.shaderName = shaderName;
			this.EnsureMaterials();
		}

		public void RemoveAppliedShader()
		{
			this.RestoreMaterials();
			this.DestroyShaderSwappedMaterials();
		}

		protected virtual void EnsureMaterialForRenderer(Renderer renderer, Shader shader)
		{
			Material sharedMaterial = renderer.sharedMaterial;
			Material material = UnityUtils.EnsureMaterialCopy(renderer);
			if (this.cleanUpList.IndexOf(material) < 0)
			{
				this.cleanUpList.Add(material);
			}
			if (material != null)
			{
				if (this.shaderSwappedMaterials == null)
				{
					this.shaderSwappedMaterials = new List<Material>();
				}
				if (!this.oldMaterials.ContainsValue(sharedMaterial))
				{
					material.shader = shader;
					this.SetMaterialCustomProperties(material);
					this.shaderSwappedMaterials.Add(material);
				}
				else
				{
					string name = material.name;
					int i = 0;
					int count = this.shaderSwappedMaterials.Count;
					while (i < count)
					{
						if (name == this.shaderSwappedMaterials[i].name)
						{
							renderer.sharedMaterial = this.shaderSwappedMaterials[i];
							break;
						}
						i++;
					}
				}
				this.oldMaterials.Add(renderer.gameObject.GetInstanceID(), sharedMaterial);
			}
		}

		protected virtual void SetMaterialCustomProperties(Material material)
		{
		}

		protected bool EnsureMaterials()
		{
			if (this.shaderSwappedMaterials != null)
			{
				return true;
			}
			Shader shader = Service.Get<AssetManager>().Shaders.GetShader(this.shaderName);
			if (shader == null)
			{
				return false;
			}
			if (this.gameObject == null)
			{
				return false;
			}
			AssetMeshDataMonoBehaviour component = this.gameObject.GetComponent<AssetMeshDataMonoBehaviour>();
			if (component == null)
			{
				return false;
			}
			int i = 0;
			int count = component.SelectableGameObjects.Count;
			while (i < count)
			{
				Renderer component2 = component.SelectableGameObjects[i].GetComponent<Renderer>();
				if (component2 != null)
				{
					this.EnsureMaterialForRenderer(component2, shader);
				}
				i++;
			}
			return this.shaderSwappedMaterials != null;
		}

		protected virtual void RestoreMaterialForRenderer(Renderer renderer)
		{
			int instanceID = renderer.gameObject.GetInstanceID();
			if (this.oldMaterials.ContainsKey(instanceID))
			{
				renderer.sharedMaterial = this.oldMaterials[instanceID];
			}
		}

		private void RestoreMaterials()
		{
			if (this.oldMaterials.Count == 0)
			{
				return;
			}
			if (this.gameObject == null)
			{
				return;
			}
			AssetMeshDataMonoBehaviour component = this.gameObject.GetComponent<AssetMeshDataMonoBehaviour>();
			if (component != null)
			{
				int i = 0;
				int count = component.SelectableGameObjects.Count;
				while (i < count)
				{
					Renderer component2 = component.SelectableGameObjects[i].GetComponent<Renderer>();
					if (component2 != null)
					{
						this.RestoreMaterialForRenderer(component2);
					}
					i++;
				}
			}
			this.oldMaterials.Clear();
		}

		private void DestroyShaderSwappedMaterials()
		{
			int count = this.cleanUpList.Count;
			for (int i = 0; i < count; i++)
			{
				UnityUtils.DestroyMaterial(this.cleanUpList[i]);
			}
			this.cleanUpList.Clear();
			if (this.shaderSwappedMaterials != null)
			{
				this.shaderSwappedMaterials.Clear();
				this.shaderSwappedMaterials = null;
			}
		}

		public void Cleanup()
		{
			this.DestroyShaderSwappedMaterials();
			this.oldMaterials.Clear();
			this.gameObject = null;
		}

		protected internal ShaderSwappedAsset(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShaderSwappedAsset)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ShaderSwappedAsset)GCHandledObjects.GCHandleToObject(instance)).DestroyShaderSwappedMaterials();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ShaderSwappedAsset)GCHandledObjects.GCHandleToObject(instance)).EnsureMaterialForRenderer((Renderer)GCHandledObjects.GCHandleToObject(*args), (Shader)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShaderSwappedAsset)GCHandledObjects.GCHandleToObject(instance)).EnsureMaterials());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ShaderSwappedAsset)GCHandledObjects.GCHandleToObject(instance)).Init((GameObject)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ShaderSwappedAsset)GCHandledObjects.GCHandleToObject(instance)).RemoveAppliedShader();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ShaderSwappedAsset)GCHandledObjects.GCHandleToObject(instance)).RestoreMaterialForRenderer((Renderer)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ShaderSwappedAsset)GCHandledObjects.GCHandleToObject(instance)).RestoreMaterials();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ShaderSwappedAsset)GCHandledObjects.GCHandleToObject(instance)).SetMaterialCustomProperties((Material)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
