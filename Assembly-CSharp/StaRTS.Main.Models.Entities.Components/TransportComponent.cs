using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TransportComponent : AssetComponent
	{
		public TransportTypeVO TransportType
		{
			get;
			private set;
		}

		public LinearSpline Spline
		{
			get;
			set;
		}

		public GameObject GameObj
		{
			get;
			set;
		}

		public GameObject ShadowGameObject
		{
			get;
			set;
		}

		public Material ShadowMaterial
		{
			get;
			set;
		}

		public TransportComponent(TransportTypeVO transportType) : base(transportType.AssetName)
		{
			this.TransportType = transportType;
			this.Spline = new LinearSpline((float)transportType.MaxSpeed);
			this.GameObj = null;
			this.ShadowGameObject = null;
			this.ShadowMaterial = null;
		}

		public override void OnRemove()
		{
			if (this.ShadowMaterial != null)
			{
				UnityUtils.DestroyMaterial(this.ShadowMaterial);
				this.ShadowMaterial = null;
			}
			if (this.GameObj != null)
			{
				UnityEngine.Object.Destroy(this.GameObj);
				this.GameObj = null;
				this.ShadowGameObject = null;
			}
		}

		protected internal TransportComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).GameObj);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).ShadowGameObject);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).ShadowMaterial);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).Spline);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).TransportType);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).OnRemove();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).GameObj = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).ShadowGameObject = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).ShadowMaterial = (Material)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).Spline = (LinearSpline)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TransportComponent)GCHandledObjects.GCHandleToObject(instance)).TransportType = (TransportTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
