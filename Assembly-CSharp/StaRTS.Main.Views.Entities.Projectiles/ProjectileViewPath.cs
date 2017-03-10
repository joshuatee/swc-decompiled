using StaRTS.Main.Views.World;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities.Projectiles
{
	public class ProjectileViewPath
	{
		public float AgeSeconds;

		protected ProjectileView view;

		public Vector3 CurrentLocation
		{
			get;
			protected set;
		}

		public ProjectileViewPath(ProjectileView view)
		{
			this.view = view;
			this.CurrentLocation = view.StartLocation;
		}

		public virtual void OnUpdate(float dt)
		{
		}

		protected internal ProjectileViewPath(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileViewPath)GCHandledObjects.GCHandleToObject(instance)).CurrentLocation);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ProjectileViewPath)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ProjectileViewPath)GCHandledObjects.GCHandleToObject(instance)).CurrentLocation = *(*(IntPtr*)args);
			return -1L;
		}
	}
}
