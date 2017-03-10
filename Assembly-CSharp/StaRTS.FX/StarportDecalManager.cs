using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class StarportDecalManager : IEventObserver
	{
		public static readonly float[] FX_STARPORT_DECAL_OFFSET = new float[]
		{
			default(float),
			0.666f,
			0.333f
		};

		public const string FX_STARPORT_DECAL_MESH_NAME = "numberMesh";

		private List<Material> decalMaterials;

		public StarportDecalManager()
		{
			this.decalMaterials = new List<Material>();
			base..ctor();
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingViewReady, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldReset, EventPriority.Default);
		}

		private void SetStarportDecal(Entity entity)
		{
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			if (buildingComponent.BuildingType.Type == BuildingType.Starport)
			{
				int lvl = buildingComponent.BuildingType.Lvl;
				int num = (lvl - 1) % 3;
				float x = StarportDecalManager.FX_STARPORT_DECAL_OFFSET[num];
				Vector2 mainTextureOffset = new Vector2(x, 0f);
				GameObjectViewComponent gameObjectViewComponent = entity.Get<GameObjectViewComponent>();
				if (gameObjectViewComponent != null)
				{
					Transform[] componentsInChildren = gameObjectViewComponent.MainGameObject.GetComponentsInChildren<Transform>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						if (componentsInChildren[i].gameObject.name.Contains("numberMesh"))
						{
							GameObject gameObject = componentsInChildren[i].gameObject;
							Renderer component = gameObject.GetComponent<Renderer>();
							Material material = UnityUtils.EnsureMaterialCopy(component);
							material.mainTextureOffset = mainTextureOffset;
							this.decalMaterials.Add(material);
						}
					}
				}
			}
		}

		private void DestroyMaterials()
		{
			for (int i = 0; i < this.decalMaterials.Count; i++)
			{
				UnityUtils.DestroyMaterial(this.decalMaterials[i]);
			}
			this.decalMaterials.Clear();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.BuildingViewReady)
			{
				if (id == EventId.WorldReset)
				{
					this.DestroyMaterials();
				}
			}
			else
			{
				Entity entity = ((EntityViewParams)cookie).Entity;
				this.SetStarportDecal(entity);
			}
			return EatResponse.NotEaten;
		}

		protected internal StarportDecalManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StarportDecalManager)GCHandledObjects.GCHandleToObject(instance)).DestroyMaterials();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StarportDecalManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StarportDecalManager)GCHandledObjects.GCHandleToObject(instance)).SetStarportDecal((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
