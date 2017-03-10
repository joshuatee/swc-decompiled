using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers.CombineMesh;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class ShaderSwappedEntity : ShaderSwappedAsset
	{
		private Entity entity;

		private CombineMeshManager combineMeshManager;

		public Entity Entity
		{
			get
			{
				return this.entity;
			}
		}

		public ShaderSwappedEntity(Entity entity, string shaderName)
		{
			this.entity = entity;
			this.combineMeshManager = Service.Get<CombineMeshManager>();
			GameObjectViewComponent gameObjectViewComponent = entity.Get<GameObjectViewComponent>();
			if (gameObjectViewComponent != null)
			{
				base.Init(gameObjectViewComponent.MainGameObject, shaderName);
			}
		}

		protected override void RestoreMaterialForRenderer(Renderer renderer)
		{
			if (this.combineMeshManager.IsRendererCombined((SmartEntity)this.entity, renderer))
			{
				renderer.enabled = false;
			}
			base.RestoreMaterialForRenderer(renderer);
		}

		protected override void EnsureMaterialForRenderer(Renderer renderer, Shader shader)
		{
			if (this.combineMeshManager.IsRendererCombined((SmartEntity)this.entity, renderer))
			{
				renderer.enabled = true;
			}
			base.EnsureMaterialForRenderer(renderer, shader);
		}

		protected internal ShaderSwappedEntity(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShaderSwappedEntity)GCHandledObjects.GCHandleToObject(instance)).EnsureMaterialForRenderer((Renderer)GCHandledObjects.GCHandleToObject(*args), (Shader)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShaderSwappedEntity)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ShaderSwappedEntity)GCHandledObjects.GCHandleToObject(instance)).RestoreMaterialForRenderer((Renderer)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
