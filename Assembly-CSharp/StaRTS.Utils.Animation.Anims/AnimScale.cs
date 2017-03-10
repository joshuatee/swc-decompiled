using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils.Animation.Anims
{
	public class AnimScale : AbstractAnimVector
	{
		public Transform Target
		{
			get;
			set;
		}

		public AnimScale(Transform target, float duration, Vector3 endScale)
		{
			this.Target = target;
			base.Duration = duration;
			base.End = endScale;
		}

		public AnimScale(GameObject target, float duration, Vector3 endScale) : this(target.transform, duration, endScale)
		{
		}

		public override void OnBegin()
		{
			base.Start = this.Target.localScale;
			base.Delta = base.End - base.Start;
		}

		public override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.Target.localScale = this.Vector;
		}

		public void SetEndPos(Vector3 endScale)
		{
			base.Delta = endScale - base.Start;
		}

		protected internal AnimScale(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AnimScale)GCHandledObjects.GCHandleToObject(instance)).Target);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AnimScale)GCHandledObjects.GCHandleToObject(instance)).OnBegin();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AnimScale)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AnimScale)GCHandledObjects.GCHandleToObject(instance)).Target = (Transform)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AnimScale)GCHandledObjects.GCHandleToObject(instance)).SetEndPos(*(*(IntPtr*)args));
			return -1L;
		}
	}
}
