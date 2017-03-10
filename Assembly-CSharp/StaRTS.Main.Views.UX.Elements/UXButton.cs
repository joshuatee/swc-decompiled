using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXButton : UXElement
	{
		private Color defaultButtonColor;

		private UXButtonComponent component;

		public UIButton NGUIButton;

		private UserInputInhibitor inhibitor;

		public UXButtonClickedDelegate OnClicked
		{
			get;
			set;
		}

		public UXButtonPressedDelegate OnPressed
		{
			get;
			set;
		}

		public UXButtonReleasedDelegate OnReleased
		{
			get;
			set;
		}

		public bool VisuallyDisabled
		{
			get;
			private set;
		}

		public UXButton(UXCamera uxCamera, UXButtonComponent component)
		{
			this.defaultButtonColor = new Color(255f, 255f, 255f, 1f);
			base..ctor(uxCamera, component.gameObject, component.NGUIButton);
			this.component = component;
			this.OnClicked = null;
			this.OnPressed = null;
			this.OnReleased = null;
			this.NGUIButton = component.NGUIButton;
			this.VisuallyDisabled = false;
			if (this.NGUIButton != null)
			{
				this.defaultButtonColor = this.NGUIButton.defaultColor;
			}
			this.inhibitor = (Service.IsSet<UserInputInhibitor>() ? Service.Get<UserInputInhibitor>() : null);
		}

		public override void InternalDestroyComponent()
		{
			this.component.Button = null;
			UnityEngine.Object.Destroy(this.component);
		}

		public void InternalOnClick()
		{
			if (this.OnClicked != null && (this.inhibitor == null || this.inhibitor.IsAllowable(this)))
			{
				this.OnClicked(this);
				base.SendClickEvent();
			}
		}

		public void InternalOnPress()
		{
			if (this.OnPressed != null && (this.inhibitor == null || this.inhibitor.IsAllowable(this)))
			{
				this.OnPressed(this);
			}
		}

		public void InternalOnRelease()
		{
			if (this.OnReleased != null && (this.inhibitor == null || this.inhibitor.IsAllowable(this)))
			{
				this.OnReleased(this);
			}
		}

		public void VisuallyDisableButton()
		{
			this.VisuallyDisabled = true;
			this.NGUIButton.defaultColor = this.NGUIButton.disabledColor;
			this.NGUIButton.UpdateColor(true);
		}

		public void VisuallyEnableButton()
		{
			this.VisuallyDisabled = false;
			this.NGUIButton.defaultColor = this.defaultButtonColor;
			this.NGUIButton.UpdateColor(true);
		}

		public void SetDefaultColor(float r, float g, float b, float a)
		{
			this.NGUIButton.defaultColor = new Color(r, g, b, a);
			this.NGUIButton.UpdateColor(true);
		}

		protected internal UXButton(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXButton)GCHandledObjects.GCHandleToObject(instance)).OnClicked);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXButton)GCHandledObjects.GCHandleToObject(instance)).OnPressed);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXButton)GCHandledObjects.GCHandleToObject(instance)).OnReleased);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXButton)GCHandledObjects.GCHandleToObject(instance)).VisuallyDisabled);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).InternalOnClick();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).InternalOnPress();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).InternalOnRelease();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).OnClicked = (UXButtonClickedDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).OnPressed = (UXButtonPressedDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).OnReleased = (UXButtonReleasedDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).VisuallyDisabled = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).SetDefaultColor(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).VisuallyDisableButton();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((UXButton)GCHandledObjects.GCHandleToObject(instance)).VisuallyEnableButton();
			return -1L;
		}
	}
}
