using Net.RichardLord.Ash.Core;
using StaRTS.Utils;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class MeterShaderComponent : ComponentBase
	{
		private const string METER_SHADER_PROGRESS = "_Progress";

		private GameObject meterObject;

		private Material meterMaterial;

		public float Percentage
		{
			get;
			private set;
		}

		public int FillSize
		{
			get;
			set;
		}

		public MeterShaderComponent(GameObject gameObject)
		{
			this.meterObject = gameObject;
			this.meterMaterial = UnityUtils.EnsureMaterialCopy(this.meterObject.GetComponent<Renderer>());
			this.UpdatePercentage(0f);
			this.FillSize = 0;
		}

		public bool GameObjectEquals(GameObject gameObject)
		{
			return this.meterObject == gameObject;
		}

		public override void OnRemove()
		{
			if (this.meterMaterial != null)
			{
				UnityUtils.DestroyMaterial(this.meterMaterial);
				this.meterMaterial = null;
			}
		}

		public void UpdatePercentage(float percentage)
		{
			percentage = Mathf.Max(0f, percentage);
			percentage = Mathf.Min(1f, percentage);
			this.Percentage = percentage;
			this.meterMaterial.SetFloat("_Progress", this.Percentage);
		}

		protected internal MeterShaderComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MeterShaderComponent)GCHandledObjects.GCHandleToObject(instance)).GameObjectEquals((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MeterShaderComponent)GCHandledObjects.GCHandleToObject(instance)).FillSize);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MeterShaderComponent)GCHandledObjects.GCHandleToObject(instance)).Percentage);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((MeterShaderComponent)GCHandledObjects.GCHandleToObject(instance)).OnRemove();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((MeterShaderComponent)GCHandledObjects.GCHandleToObject(instance)).FillSize = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((MeterShaderComponent)GCHandledObjects.GCHandleToObject(instance)).Percentage = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((MeterShaderComponent)GCHandledObjects.GCHandleToObject(instance)).UpdatePercentage(*(float*)args);
			return -1L;
		}
	}
}
