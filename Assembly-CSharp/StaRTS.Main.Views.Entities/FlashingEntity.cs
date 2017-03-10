using StaRTS.Assets;
using StaRTS.Main.Controllers.CombineMesh;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class FlashingEntity
	{
		private SmartEntity entity;

		private float flashDelay;

		private float flashDuration;

		private float flashDelayCounter;

		private float flashActiveCounter;

		private List<Material> flashingMaterials;

		private Dictionary<int, Material> oldMaterials;

		private CombineMeshManager combineMeshManager;

		private GameObject gameObject;

		private Color startColor;

		private Color finalColor;

		public SmartEntity Entity
		{
			get
			{
				return this.entity;
			}
		}

		public FlashingEntity(SmartEntity entity, GameObject gameObject, float flashDuration, float flashDelay)
		{
			this.startColor = new Color(1f, 1f, 1f, 1f);
			this.finalColor = new Color(0.5f, 0.5f, 0.5f, 1f);
			base..ctor();
			this.entity = entity;
			this.gameObject = gameObject;
			this.flashDuration = ((flashDuration < 0f) ? 0f : flashDuration);
			this.flashDelay = ((flashDelay < 0f) ? 0f : flashDelay);
			this.flashActiveCounter = 0f;
			this.flashDelayCounter = 0f;
			this.flashingMaterials = null;
			this.oldMaterials = null;
			this.combineMeshManager = Service.Get<CombineMeshManager>();
		}

		public bool Flash(float dt)
		{
			if (this.flashDuration == 0f)
			{
				return true;
			}
			this.flashActiveCounter += dt;
			if (this.flashActiveCounter <= this.flashDuration)
			{
				if (!this.EnsureMaterials())
				{
					return true;
				}
				float percentage = this.flashActiveCounter / this.flashDuration;
				Color color = this.LinearTweenColor(percentage);
				int i = 0;
				int count = this.flashingMaterials.Count;
				while (i < count)
				{
					this.SetColor(i, color);
					i++;
				}
			}
			else
			{
				if (this.flashDelay == 0f)
				{
					return true;
				}
				this.flashDelayCounter += dt;
				if (this.flashDelayCounter > this.flashDelay)
				{
					this.flashDelayCounter -= this.flashDelay;
					this.flashActiveCounter = 0f;
				}
			}
			return false;
		}

		private Color LinearTweenColor(float percentage)
		{
			float r = this.startColor.r + (this.finalColor.r - this.startColor.r) * percentage;
			float g = this.startColor.g + (this.finalColor.g - this.startColor.g) * percentage;
			float b = this.startColor.b + (this.finalColor.b - this.startColor.b) * percentage;
			float a = this.startColor.a + (this.finalColor.a - this.startColor.a) * percentage;
			return new Color(r, g, b, a);
		}

		private void SetColor(int i, Color color)
		{
			this.flashingMaterials[i].color = color;
		}

		public void Complete()
		{
			this.RestoreMaterials();
			if (this.flashingMaterials != null)
			{
				int i = 0;
				int count = this.flashingMaterials.Count;
				while (i < count)
				{
					UnityUtils.DestroyMaterial(this.flashingMaterials[i]);
					i++;
				}
				this.flashingMaterials = null;
			}
			this.oldMaterials = null;
		}

		private bool EnsureMaterials()
		{
			if (this.flashingMaterials != null)
			{
				return true;
			}
			string shaderName = "UnlitTexture_Color_Boosted";
			Shader shader = Service.Get<AssetManager>().Shaders.GetShader(shaderName);
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
			return this.flashingMaterials != null;
		}

		private void RestoreMaterials()
		{
			if (this.oldMaterials == null || this.oldMaterials.Count == 0)
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

		private void EnsureMaterialForRenderer(Renderer renderer, Shader shader)
		{
			if (this.combineMeshManager.IsRendererCombined(this.entity, renderer))
			{
				renderer.enabled = true;
			}
			Material sharedMaterial = renderer.sharedMaterial;
			if (this.oldMaterials == null)
			{
				this.oldMaterials = new Dictionary<int, Material>();
			}
			Material material = UnityUtils.EnsureMaterialCopy(renderer);
			if (material != null)
			{
				if (this.flashingMaterials == null)
				{
					this.flashingMaterials = new List<Material>();
				}
				if (!this.oldMaterials.ContainsValue(sharedMaterial))
				{
					material.shader = shader;
					this.flashingMaterials.Add(material);
				}
				else
				{
					string name = material.name;
					int i = 0;
					int count = this.flashingMaterials.Count;
					while (i < count)
					{
						if (name == this.flashingMaterials[i].name)
						{
							renderer.sharedMaterial = this.flashingMaterials[i];
							break;
						}
						i++;
					}
				}
				this.oldMaterials.Add(renderer.gameObject.GetInstanceID(), sharedMaterial);
			}
		}

		private void RestoreMaterialForRenderer(Renderer renderer)
		{
			if (this.combineMeshManager.IsRendererCombined(this.entity, renderer))
			{
				renderer.enabled = false;
			}
			int instanceID = renderer.gameObject.GetInstanceID();
			if (this.oldMaterials.ContainsKey(instanceID))
			{
				renderer.sharedMaterial = this.oldMaterials[instanceID];
			}
			Service.Get<EventManager>().SendEvent(EventId.ShaderResetOnEntity, this.entity);
		}

		protected internal FlashingEntity(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((FlashingEntity)GCHandledObjects.GCHandleToObject(instance)).Complete();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((FlashingEntity)GCHandledObjects.GCHandleToObject(instance)).EnsureMaterialForRenderer((Renderer)GCHandledObjects.GCHandleToObject(*args), (Shader)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FlashingEntity)GCHandledObjects.GCHandleToObject(instance)).EnsureMaterials());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FlashingEntity)GCHandledObjects.GCHandleToObject(instance)).Flash(*(float*)args));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FlashingEntity)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FlashingEntity)GCHandledObjects.GCHandleToObject(instance)).LinearTweenColor(*(float*)args));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((FlashingEntity)GCHandledObjects.GCHandleToObject(instance)).RestoreMaterialForRenderer((Renderer)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((FlashingEntity)GCHandledObjects.GCHandleToObject(instance)).RestoreMaterials();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((FlashingEntity)GCHandledObjects.GCHandleToObject(instance)).SetColor(*(int*)args, *(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}
