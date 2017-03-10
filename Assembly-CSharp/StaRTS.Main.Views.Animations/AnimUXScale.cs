using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Animation.Anims;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Animations
{
	public class AnimUXScale : AbstractAnimVector
	{
		public UXElement Target
		{
			get;
			set;
		}

		public AnimUXScale(UXElement target, float duration, Vector3 endScale)
		{
			this.Target = target;
			base.Duration = duration;
			base.End = endScale;
		}

		public override void OnBegin()
		{
			base.Start = this.Target.LocalScale;
			base.Delta = base.End - base.Start;
		}

		public override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.Target.LocalScale = this.Vector;
		}

		public void SetEndScale(Vector3 endScale)
		{
			base.Delta = endScale - base.Start;
		}

		protected internal AnimUXScale(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AnimUXScale)GCHandledObjects.GCHandleToObject(instance)).Target);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AnimUXScale)GCHandledObjects.GCHandleToObject(instance)).OnBegin();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AnimUXScale)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AnimUXScale)GCHandledObjects.GCHandleToObject(instance)).Target = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AnimUXScale)GCHandledObjects.GCHandleToObject(instance)).SetEndScale(*(*(IntPtr*)args));
			return -1L;
		}
	}
}
