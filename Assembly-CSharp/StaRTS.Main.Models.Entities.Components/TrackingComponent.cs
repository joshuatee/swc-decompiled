using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TrackingComponent : ComponentBase
	{
		public const float DEFAULT_VELOCITY = 0.25f;

		public const float IDLE_VELOCITY = 0.75f;

		public const float MAX_PITCH_ANGLE = 0.2617994f;

		private Entity targetEntity;

		public float MaxVelocity
		{
			get;
			set;
		}

		public TransformComponent TargetTransform
		{
			get;
			set;
		}

		public int TargetX
		{
			get;
			set;
		}

		public int TargetZ
		{
			get;
			set;
		}

		public float TargetYaw
		{
			get;
			set;
		}

		public float TargetPitch
		{
			get;
			set;
		}

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

		public bool TrackPitch
		{
			get;
			private set;
		}

		public TrackingMode Mode
		{
			get;
			set;
		}

		public float IdleFastForwardTimeRemaining
		{
			get;
			set;
		}

		public Entity TargetEntity
		{
			get
			{
				return this.targetEntity;
			}
			set
			{
				this.targetEntity = value;
				if (this.targetEntity == null)
				{
					this.TargetTransform = null;
					return;
				}
				this.TargetTransform = this.targetEntity.Get<TransformComponent>();
			}
		}

		public TrackingComponent(TurretTypeVO turrentType, bool trackPitch)
		{
			this.MaxVelocity = 0.25f;
			float num = (float)Service.Get<Rand>().SimRange(0, 360) * 3.14159274f / 180f;
			this.TargetEntity = null;
			this.TargetTransform = null;
			this.TargetX = 0;
			this.TargetZ = 0;
			this.TargetYaw = num;
			this.Mode = TrackingMode.Disabled;
			this.Yaw = num;
			this.TrackPitch = trackPitch;
			if (trackPitch)
			{
				this.TargetPitch = 0f;
				this.Pitch = 0f;
			}
		}

		protected internal TrackingComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).IdleFastForwardTimeRemaining);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).MaxVelocity);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).Mode);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).Pitch);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetEntity);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetPitch);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetTransform);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetX);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetYaw);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetZ);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TrackPitch);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).Yaw);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).IdleFastForwardTimeRemaining = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).MaxVelocity = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).Mode = (TrackingMode)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).Pitch = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetEntity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetPitch = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetTransform = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetYaw = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TargetZ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).TrackPitch = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((TrackingComponent)GCHandledObjects.GCHandleToObject(instance)).Yaw = *(float*)args;
			return -1L;
		}
	}
}
