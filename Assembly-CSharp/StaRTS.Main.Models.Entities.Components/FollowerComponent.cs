using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class FollowerComponent : ComponentBase
	{
		public Entity Child
		{
			get;
			set;
		}

		public FollowerComponent(Entity child)
		{
			this.Child = child;
		}

		protected internal FollowerComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FollowerComponent)GCHandledObjects.GCHandleToObject(instance)).Child);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((FollowerComponent)GCHandledObjects.GCHandleToObject(instance)).Child = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
