using StaRTS.Main.Models.Entities;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class EntityViewParams
	{
		public SmartEntity Entity
		{
			get;
			set;
		}

		public bool CreateCollider
		{
			get;
			set;
		}

		public EntityViewParams(SmartEntity entity, bool createCollider)
		{
			this.Entity = entity;
			this.CreateCollider = createCollider;
		}

		protected internal EntityViewParams(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityViewParams)GCHandledObjects.GCHandleToObject(instance)).CreateCollider);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityViewParams)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EntityViewParams)GCHandledObjects.GCHandleToObject(instance)).CreateCollider = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EntityViewParams)GCHandledObjects.GCHandleToObject(instance)).Entity = (SmartEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
