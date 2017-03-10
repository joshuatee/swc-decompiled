using System;
using WinRTBridge;

namespace Net.RichardLord.Ash.Core
{
	public abstract class ViewSystemBase : SystemBase<ViewSystemBase>
	{
		internal float DT
		{
			get;
			private set;
		}

		protected abstract void Update(float dt);

		public override void Update()
		{
			this.Update(this.DT);
			this.DT = 0f;
		}

		public void AccumulateDT(float dt)
		{
			this.DT += dt;
		}

		protected ViewSystemBase()
		{
		}

		protected internal ViewSystemBase(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ViewSystemBase)GCHandledObjects.GCHandleToObject(instance)).AccumulateDT(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ViewSystemBase)GCHandledObjects.GCHandleToObject(instance)).DT);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ViewSystemBase)GCHandledObjects.GCHandleToObject(instance)).DT = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ViewSystemBase)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ViewSystemBase)GCHandledObjects.GCHandleToObject(instance)).Update(*(float*)args);
			return -1L;
		}
	}
}
