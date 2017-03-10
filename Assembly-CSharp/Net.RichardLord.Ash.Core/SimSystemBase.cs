using System;
using WinRTBridge;

namespace Net.RichardLord.Ash.Core
{
	public abstract class SimSystemBase : SystemBase<SimSystemBase>
	{
		internal uint DT
		{
			get;
			private set;
		}

		protected abstract void Update(uint dt);

		public override void Update()
		{
			this.Update(this.DT);
			this.DT = 0u;
		}

		public void AccumulateDT(uint dt)
		{
			this.DT += dt;
		}

		protected SimSystemBase()
		{
		}

		protected internal SimSystemBase(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SimSystemBase)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}
	}
}
