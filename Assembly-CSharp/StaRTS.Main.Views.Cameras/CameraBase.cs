using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Cameras
{
	public class CameraBase
	{
		protected Camera unityCamera;

		public Camera Camera
		{
			get
			{
				return this.unityCamera;
			}
		}

		public CameraBase()
		{
			this.unityCamera = null;
		}

		public Ray ScreenPointToRay(Vector3 screenPoint)
		{
			return this.unityCamera.ScreenPointToRay(screenPoint);
		}

		public Vector3 ViewportPositionToScreenPoint(Vector3 viewportPosition)
		{
			return this.unityCamera.ViewportToScreenPoint(viewportPosition);
		}

		public void Enable()
		{
			this.unityCamera.enabled = true;
		}

		public void Disable()
		{
			this.unityCamera.enabled = false;
		}

		protected internal CameraBase(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CameraBase)GCHandledObjects.GCHandleToObject(instance)).Disable();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CameraBase)GCHandledObjects.GCHandleToObject(instance)).Enable();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CameraBase)GCHandledObjects.GCHandleToObject(instance)).Camera);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CameraBase)GCHandledObjects.GCHandleToObject(instance)).ScreenPointToRay(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CameraBase)GCHandledObjects.GCHandleToObject(instance)).ViewportPositionToScreenPoint(*(*(IntPtr*)args)));
		}
	}
}
