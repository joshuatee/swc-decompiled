using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Planets
{
	public class GalaxyTransitionData
	{
		public float StartViewDistance
		{
			get;
			private set;
		}

		public float EndViewDistance
		{
			get;
			private set;
		}

		public float StartViewHeight
		{
			get;
			private set;
		}

		public float EndViewHeight
		{
			get;
			private set;
		}

		public float StartViewRotation
		{
			get;
			private set;
		}

		public float EndViewRotation
		{
			get;
			private set;
		}

		public Vector3 StartViewLookAt
		{
			get;
			private set;
		}

		public Vector3 EndViewLookAt
		{
			get;
			private set;
		}

		public bool Instant
		{
			get;
			set;
		}

		public float TransitionDuration
		{
			get;
			set;
		}

		public GalaxyTransitionData()
		{
		}

		public void SetTransitionDistance(float startDistance, float endDistance)
		{
			this.StartViewDistance = startDistance;
			this.EndViewDistance = endDistance;
		}

		public void SetTransitionHeight(float startHeight, float endHeight)
		{
			this.StartViewHeight = startHeight;
			this.EndViewHeight = endHeight;
		}

		public void SetTransitionRotation(float startRotation, float endRotation)
		{
			this.StartViewRotation = startRotation;
			this.EndViewRotation = endRotation;
		}

		public void SetTransitionLookAt(Vector3 startlook, Vector3 endLook)
		{
			this.StartViewLookAt = startlook;
			this.EndViewLookAt = endLook;
		}

		public void Reset()
		{
			this.StartViewDistance = 0f;
			this.EndViewDistance = 0f;
			this.StartViewHeight = 0f;
			this.EndViewHeight = 0f;
			this.StartViewRotation = 0f;
			this.EndViewRotation = 0f;
			this.StartViewLookAt = Vector3.zero;
			this.EndViewLookAt = Vector3.zero;
			this.TransitionDuration = 0f;
			this.Instant = false;
		}

		protected internal GalaxyTransitionData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).EndViewDistance);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).EndViewHeight);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).EndViewLookAt);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).EndViewRotation);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).Instant);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).StartViewDistance);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).StartViewHeight);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).StartViewLookAt);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).StartViewRotation);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).TransitionDuration);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).EndViewDistance = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).EndViewHeight = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).EndViewLookAt = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).EndViewRotation = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).Instant = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).StartViewDistance = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).StartViewHeight = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).StartViewLookAt = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).StartViewRotation = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).TransitionDuration = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).SetTransitionDistance(*(float*)args, *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).SetTransitionHeight(*(float*)args, *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).SetTransitionLookAt(*(*(IntPtr*)args), *(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((GalaxyTransitionData)GCHandledObjects.GCHandleToObject(instance)).SetTransitionRotation(*(float*)args, *(float*)(args + 1));
			return -1L;
		}
	}
}
