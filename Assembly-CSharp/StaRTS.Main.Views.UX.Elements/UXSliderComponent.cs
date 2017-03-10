using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXSliderComponent : MonoBehaviour, IUnitySerializable
	{
		public UISlider NGUISlider
		{
			get;
			set;
		}

		public UXSlider Slider
		{
			get;
			set;
		}

		public float Value
		{
			get
			{
				if (!(this.NGUISlider == null))
				{
					return this.NGUISlider.value;
				}
				return 0f;
			}
			set
			{
				if (this.NGUISlider != null)
				{
					this.NGUISlider.value = value;
				}
			}
		}

		public UXSliderComponent()
		{
		}

		public override void Unity_Serialize(int depth)
		{
		}

		public override void Unity_Deserialize(int depth)
		{
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public override void Unity_NamedSerialize(int depth)
		{
		}

		public override void Unity_NamedDeserialize(int depth)
		{
		}

		protected internal UXSliderComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).NGUISlider);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).Slider);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).Value);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).NGUISlider = (UISlider)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).Slider = (UXSlider)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).Value = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXSliderComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
