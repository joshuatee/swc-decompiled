using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Animation.Anims;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Animations
{
	public class AnimUXPosition : AbstractAnimVector
	{
		public UXElement Target
		{
			get;
			set;
		}

		public AnimUXPosition(UXElement target, float duration, Vector3 endPos)
		{
			this.Target = target;
			base.Duration = duration;
			base.End = endPos;
		}

		public override void OnBegin()
		{
			base.Start = this.Target.LocalPosition;
			base.Delta = base.End - base.Start;
		}

		public override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.Target.LocalPosition = this.Vector;
		}

		public void SetEndPos(Vector3 endPos)
		{
			base.Delta = endPos - base.Start;
		}

		protected internal AnimUXPosition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AnimUXPosition)GCHandledObjects.GCHandleToObject(instance)).Target);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AnimUXPosition)GCHandledObjects.GCHandleToObject(instance)).OnBegin();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AnimUXPosition)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AnimUXPosition)GCHandledObjects.GCHandleToObject(instance)).Target = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AnimUXPosition)GCHandledObjects.GCHandleToObject(instance)).SetEndPos(*(*(IntPtr*)args));
			return -1L;
		}
	}
}
