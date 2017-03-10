using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils.Animation.Anims
{
	public abstract class AbstractAnimVector : Anim
	{
		protected Vector3 Vector;

		public Vector3 Start
		{
			get;
			set;
		}

		public Vector3 End
		{
			get;
			set;
		}

		public Vector3 Delta
		{
			get;
			set;
		}

		public Easing.EasingDelegate EaseFunctionX
		{
			get;
			set;
		}

		public Easing.EasingDelegate EaseFunctionY
		{
			get;
			set;
		}

		public Easing.EasingDelegate EaseFunctionZ
		{
			get;
			set;
		}

		public override Easing.EasingDelegate EaseFunction
		{
			get
			{
				return base.EaseFunction;
			}
			set
			{
				base.EaseFunction = value;
				this.EaseFunctionX = value;
				this.EaseFunctionY = value;
				this.EaseFunctionZ = value;
			}
		}

		public AbstractAnimVector()
		{
			this.Vector = Vector3.zero;
		}

		public override void OnUpdate(float dt)
		{
			this.Vector.x = this.EaseFunctionX(base.Age, this.Start.x, this.Delta.x, base.Duration);
			this.Vector.y = this.EaseFunctionY(base.Age, this.Start.y, this.Delta.y, base.Duration);
			this.Vector.z = this.EaseFunctionZ(base.Age, this.Start.z, this.Delta.z, base.Duration);
		}

		protected internal AbstractAnimVector(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).Delta);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).EaseFunction);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).EaseFunctionX);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).EaseFunctionY);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).EaseFunctionZ);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).End);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).Start);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).Delta = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).EaseFunction = (Easing.EasingDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).EaseFunctionX = (Easing.EasingDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).EaseFunctionY = (Easing.EasingDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).EaseFunctionZ = (Easing.EasingDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).End = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AbstractAnimVector)GCHandledObjects.GCHandleToObject(instance)).Start = *(*(IntPtr*)args);
			return -1L;
		}
	}
}
