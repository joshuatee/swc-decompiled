using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Cameras
{
	public abstract class WorldCamera : CameraBase
	{
		public float GroundOffset
		{
			get;
			set;
		}

		public abstract bool GetGroundPosition(Vector3 screenPosition, ref Vector3 groundPosition);

		public Vector3 WorldPositionToScreenPoint(Vector3 worldPoint)
		{
			return this.unityCamera.WorldToScreenPoint(worldPoint);
		}

		protected WorldCamera()
		{
		}

		protected internal WorldCamera(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldCamera)GCHandledObjects.GCHandleToObject(instance)).GroundOffset);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WorldCamera)GCHandledObjects.GCHandleToObject(instance)).GroundOffset = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldCamera)GCHandledObjects.GCHandleToObject(instance)).WorldPositionToScreenPoint(*(*(IntPtr*)args)));
		}
	}
}
