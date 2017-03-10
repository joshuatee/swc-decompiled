using System;
using WinRTBridge;

namespace Net.RichardLord.Ash.Core
{
	public abstract class ComponentBase
	{
		public Entity Entity;

		public virtual void OnRemove()
		{
		}

		protected ComponentBase()
		{
		}

		protected internal ComponentBase(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ComponentBase)GCHandledObjects.GCHandleToObject(instance)).OnRemove();
			return -1L;
		}
	}
}
