using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class TrackingSystem : SimSystemBase
	{
		private NodeList<TrackingNode> nodeList;

		public override void AddToGame(IGame game)
		{
			this.nodeList = Service.Get<EntityController>().GetNodeList<TrackingNode>();
		}

		public override void RemoveFromGame(IGame game)
		{
			this.nodeList = null;
		}

		protected override void Update(uint dt)
		{
			ShooterController shooterController = Service.Get<ShooterController>();
			for (TrackingNode trackingNode = this.nodeList.Head; trackingNode != null; trackingNode = trackingNode.Next)
			{
				TrackingComponent trackingComp = trackingNode.TrackingComp;
				Entity turretTarget = shooterController.GetTurretTarget(trackingNode.ShooterComp);
				if (turretTarget != null && turretTarget != trackingComp.TargetEntity)
				{
					trackingComp.TargetEntity = turretTarget;
					trackingComp.Mode = TrackingMode.Entity;
				}
				if (trackingComp.Mode != TrackingMode.Disabled)
				{
					if (trackingComp.Mode == TrackingMode.Entity)
					{
						float num = (float)trackingNode.TrackingComp.TargetTransform.X - trackingNode.Transform.CenterX();
						float num2 = (float)trackingNode.TrackingComp.TargetTransform.Z - trackingNode.Transform.CenterZ();
						trackingComp.Yaw = Mathf.Atan2(num2, num);
						if (trackingNode.TrackingComp.TrackPitch)
						{
							float num3 = Mathf.Sqrt(num * num + num2 * num2);
							float num4 = Mathf.Sqrt(trackingNode.ShooterComp.MinAttackRangeSquared);
							float num5 = Mathf.Sqrt(trackingNode.ShooterComp.MaxAttackRangeSquared);
							num3 = Mathf.Clamp(num3, num4, num5);
							trackingComp.Pitch = 0.2617994f * (num5 - num3) / (num5 - num4);
						}
					}
					else if (trackingComp.Mode == TrackingMode.Location)
					{
						float x = (float)trackingComp.TargetX - trackingNode.Transform.CenterX();
						float y = (float)trackingComp.TargetZ - trackingNode.Transform.CenterZ();
						trackingComp.Yaw = Mathf.Atan2(y, x);
					}
					else if (trackingComp.Mode == TrackingMode.Angle)
					{
						trackingComp.Yaw = trackingComp.TargetYaw;
					}
				}
			}
		}

		public TrackingSystem()
		{
		}

		protected internal TrackingSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TrackingSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TrackingSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
