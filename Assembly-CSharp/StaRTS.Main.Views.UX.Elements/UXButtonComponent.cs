using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXButtonComponent : MonoBehaviour, IUnitySerializable
	{
		public UIButton NGUIButton
		{
			get;
			set;
		}

		public UXButton Button
		{
			get;
			set;
		}

		private void OnClick()
		{
			if (this.Button != null)
			{
				this.Button.InternalOnClick();
			}
		}

		private void OnPress(bool isPressed)
		{
			if (this.Button != null)
			{
				if (isPressed)
				{
					this.Button.InternalOnPress();
					return;
				}
				this.Button.InternalOnRelease();
			}
		}

		private void OnRelease()
		{
			if (this.Button != null)
			{
				this.Button.InternalOnRelease();
			}
		}

		public UXButtonComponent()
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

		protected internal UXButtonComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).Button);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIButton);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).OnClick();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).OnRelease();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).Button = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIButton = (UIButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXButtonComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
