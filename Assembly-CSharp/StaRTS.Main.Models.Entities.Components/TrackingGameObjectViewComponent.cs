using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TrackingGameObjectViewComponent : ComponentBase
	{
		private const float TRACKING_ROTATION = -3.14159274f;

		private const string NOT_APPLICABLE = "n/a";

		private GameObject trackingGameObject;

		public float YawVelocity;

		public float PitchVelocity;

		public float Yaw
		{
			get;
			set;
		}

		public float Pitch
		{
			get;
			set;
		}

		public float Speed
		{
			get;
			set;
		}

		public TrackingGameObjectViewComponent(GameObject gameObject, ITrackerVO trackerVO, TrackingComponent trackingComp)
		{
			if (string.IsNullOrEmpty(trackerVO.TrackerName))
			{
				Service.Get<StaRTSLogger>().Error("TrackerName not set for an entity which has a TrackingGameObjectViewComponent.");
				return;
			}
			Transform transform = gameObject.transform.FindChild(trackerVO.TrackerName);
			if (transform != null && transform.gameObject != null)
			{
				this.trackingGameObject = transform.gameObject;
			}
			else if (trackerVO.TrackerName != "n/a")
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Turret {0} cannot find a tracking object at {1}", new object[]
				{
					gameObject.name,
					trackerVO.TrackerName
				});
			}
			this.Yaw = trackingComp.TargetYaw;
			this.YawVelocity = 0f;
			if (trackingComp.TrackPitch)
			{
				this.Pitch = trackingComp.TargetPitch;
				this.PitchVelocity = 0f;
			}
			this.Speed = Service.Get<BattlePlaybackController>().CurrentPlaybackScale;
		}

		public void YawRotate(float rads)
		{
			if (this.trackingGameObject == null)
			{
				return;
			}
			rads += -3.14159274f;
			float angle = -rads * 57.2957764f;
			this.trackingGameObject.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
		}

		public void PitchRotate(float rads)
		{
			if (this.trackingGameObject == null)
			{
				return;
			}
			this.trackingGameObject.transform.Rotate(rads * 57.2957764f, 0f, 0f);
		}

		protected internal TrackingGameObjectViewComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingGameObjectViewComponent)GCHandledObjects.GCHandleToObject(instance)).Pitch);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingGameObjectViewComponent)GCHandledObjects.GCHandleToObject(instance)).Speed);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingGameObjectViewComponent)GCHandledObjects.GCHandleToObject(instance)).Yaw);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TrackingGameObjectViewComponent)GCHandledObjects.GCHandleToObject(instance)).PitchRotate(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TrackingGameObjectViewComponent)GCHandledObjects.GCHandleToObject(instance)).Pitch = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TrackingGameObjectViewComponent)GCHandledObjects.GCHandleToObject(instance)).Speed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TrackingGameObjectViewComponent)GCHandledObjects.GCHandleToObject(instance)).Yaw = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TrackingGameObjectViewComponent)GCHandledObjects.GCHandleToObject(instance)).YawRotate(*(float*)args);
			return -1L;
		}
	}
}
