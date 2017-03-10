using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class TransportSystem : ViewSystemBase
	{
		private const float FADE_FACTOR = 15f;

		private const float SCALE_FACTOR = 30f;

		private EntityController entityController;

		private NodeList<TransportNode> nodeList;

		public override void AddToGame(IGame game)
		{
			this.entityController = Service.Get<EntityController>();
			this.nodeList = this.entityController.GetNodeList<TransportNode>();
		}

		public override void RemoveFromGame(IGame game)
		{
			this.entityController = null;
			this.nodeList = null;
		}

		protected override void Update(float dt)
		{
			for (TransportNode transportNode = this.nodeList.Head; transportNode != null; transportNode = transportNode.Next)
			{
				GameObject gameObj = transportNode.Transport.GameObj;
				if (transportNode.State.CurState == EntityState.Moving && gameObj != null)
				{
					Vector3 vector;
					Quaternion rotation;
					if (transportNode.Transport.Spline.Update(dt, out vector, out rotation))
					{
						transportNode.State.CurState = EntityState.Idle;
						gameObj.SetActive(false);
						Service.Get<EntityFactory>().DestroyTransportEntity(transportNode.Entity);
					}
					else if (transportNode.Transport.ShadowMaterial != null)
					{
						Transform transform = gameObj.transform;
						transform.position = vector;
						transform.rotation = rotation;
						GameObject shadowGameObject = transportNode.Transport.ShadowGameObject;
						Transform transform2 = shadowGameObject.transform;
						transform2.position = new Vector3(vector.x, Mathf.Clamp(vector.y, 0f, 1f), vector.z);
						transform2.localScale = new Vector3(1f + vector.y / 30f, 1f, 1f + vector.y / 30f);
						transform2.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
						float a = Mathf.Clamp(1f - vector.y / 15f, 0f, 1f);
						transportNode.Transport.ShadowMaterial.color = new Color(0f, 0f, 0f, a);
					}
				}
			}
		}

		public TransportSystem()
		{
		}

		protected internal TransportSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TransportSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TransportSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TransportSystem)GCHandledObjects.GCHandleToObject(instance)).Update(*(float*)args);
			return -1L;
		}
	}
}
