using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class TrackingRenderSystem : ViewSystemBase
	{
		private NodeList<TrackingRenderNode> nodeList;

		public override void AddToGame(IGame game)
		{
			this.nodeList = Service.Get<EntityController>().GetNodeList<TrackingRenderNode>();
		}

		public override void RemoveFromGame(IGame game)
		{
			this.nodeList = null;
		}

		protected override void Update(float dt)
		{
			for (TrackingRenderNode trackingRenderNode = this.nodeList.Head; trackingRenderNode != null; trackingRenderNode = trackingRenderNode.Next)
			{
				TrackingComponent trackingComp = trackingRenderNode.TrackingComp;
				TrackingGameObjectViewComponent trackingView = trackingRenderNode.TrackingView;
				if (trackingView.Speed != 0f)
				{
					float target = MathUtils.MinAngle(trackingView.Yaw, trackingComp.Yaw);
					trackingView.Yaw = Mathf.SmoothDampAngle(trackingView.Yaw, target, ref trackingView.YawVelocity, trackingComp.MaxVelocity / trackingView.Speed);
					if (trackingView.YawVelocity != 0f)
					{
						trackingView.Yaw = MathUtils.WrapAngle(trackingView.Yaw);
					}
					trackingView.YawRotate(trackingView.Yaw);
					if (trackingComp.TrackPitch)
					{
						float target2 = MathUtils.MinAngle(trackingView.Pitch, trackingComp.Pitch);
						trackingView.Pitch = Mathf.SmoothDampAngle(trackingView.Pitch, target2, ref trackingView.PitchVelocity, trackingComp.MaxVelocity / trackingView.Speed);
						if (trackingView.PitchVelocity != 0f)
						{
							trackingView.Pitch = MathUtils.WrapAngle(trackingView.Pitch);
						}
						trackingView.PitchRotate(trackingView.Pitch);
					}
				}
			}
		}

		public TrackingRenderSystem()
		{
		}

		protected internal TrackingRenderSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TrackingRenderSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TrackingRenderSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TrackingRenderSystem)GCHandledObjects.GCHandleToObject(instance)).Update(*(float*)args);
			return -1L;
		}
	}
}
