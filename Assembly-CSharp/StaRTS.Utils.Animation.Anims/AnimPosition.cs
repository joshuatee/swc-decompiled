using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils.Animation.Anims
{
	public class AnimPosition : AbstractAnimVector
	{
		public Transform Target
		{
			get;
			set;
		}

		public AnimPosition(Transform target, float duration, Vector3 endPos)
		{
			this.Target = target;
			base.Duration = duration;
			base.End = endPos;
		}

		public AnimPosition(GameObject target, float duration, Vector3 endPos) : this(target.transform, duration, endPos)
		{
		}

		public override void OnBegin()
		{
			base.Start = this.Target.localPosition;
			base.Delta = base.End - base.Start;
		}

		public override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.Target == null)
			{
				Service.Get<AnimController>().CompleteAndRemoveAnim(this);
				return;
			}
			this.Target.localPosition = this.Vector;
		}

		protected internal AnimPosition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AnimPosition)GCHandledObjects.GCHandleToObject(instance)).Target);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AnimPosition)GCHandledObjects.GCHandleToObject(instance)).OnBegin();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AnimPosition)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AnimPosition)GCHandledObjects.GCHandleToObject(instance)).Target = (Transform)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
