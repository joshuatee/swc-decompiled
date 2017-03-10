using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXInputComponent : MonoBehaviour, IUnitySerializable
	{
		public UIInput NGUIInput
		{
			get;
			set;
		}

		public UXInput Input
		{
			get;
			set;
		}

		public string InitText
		{
			set
			{
				if (this.NGUIInput != null)
				{
					this.NGUIInput.defaultText = value;
				}
			}
		}

		public string Text
		{
			get
			{
				if (!(this.NGUIInput == null))
				{
					return this.NGUIInput.value;
				}
				return null;
			}
			set
			{
				if (this.NGUIInput != null)
				{
					this.NGUIInput.value = value;
				}
			}
		}

		public UXInputComponent()
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

		protected internal UXInputComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).Input);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIInput);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).Text);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).InitText = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).Input = (UXInput)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIInput = (UIInput)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).Text = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXInputComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
