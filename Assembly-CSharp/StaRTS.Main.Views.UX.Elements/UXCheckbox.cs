using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXCheckbox : UXElement
	{
		private const float AUTO_DESELECT_TIME = 0.1f;

		private UXCheckboxComponent component;

		private UserInputInhibitor inhibitor;

		private int radioGroup;

		public UXCheckboxSelectedDelegate OnSelected
		{
			get;
			set;
		}

		public bool Selected
		{
			get
			{
				return this.component.Selected;
			}
			set
			{
				this.component.Selected = ((this.inhibitor == null) ? value : (value && this.inhibitor.IsAllowable(this)));
			}
		}

		public int RadioGroup
		{
			get
			{
				return this.component.RadioGroup;
			}
			set
			{
				this.radioGroup = value;
				this.component.RadioGroup = this.radioGroup;
			}
		}

		public UXCheckbox(UXCamera uxCamera, UXCheckboxComponent component) : base(uxCamera, component.gameObject, component.NGUIButton)
		{
			this.component = component;
			this.inhibitor = Service.Get<UserInputInhibitor>();
			this.radioGroup = this.RadioGroup;
			this.OnSelected = null;
		}

		public override void InternalDestroyComponent()
		{
			this.component.RemoveDelegate();
			this.component.Checkbox = null;
			UnityEngine.Object.Destroy(this.component);
		}

		public void SetSelected(bool selected)
		{
			this.component.Selected = selected;
		}

		public void InternalOnSelect(bool selected)
		{
			if (this.inhibitor == null || this.inhibitor.IsAllowable(this))
			{
				if (this.OnSelected != null)
				{
					this.OnSelected(this, selected);
				}
			}
			else if (this.radioGroup != 0 & selected)
			{
				Service.Get<ViewTimerManager>().CreateViewTimer(0.1f, false, new TimerDelegate(this.OnDeselectTimer), this);
			}
			if (this.radioGroup == 0 | selected)
			{
				base.SendClickEvent();
			}
		}

		private void OnDeselectTimer(uint id, object cookie)
		{
			UXCheckbox uXCheckbox = cookie as UXCheckbox;
			if (uXCheckbox.component != null)
			{
				uXCheckbox.component.Selected = false;
			}
		}

		public void SetTweenTarget(UXElement element)
		{
			UIButton nGUIButton = this.component.NGUIButton;
			nGUIButton.tweenTarget = element.Root;
		}

		public void SetSelectable(bool selectable)
		{
			UIToggle nGUICheckbox = this.component.NGUICheckbox;
			if (nGUICheckbox != null && nGUICheckbox.enabled != selectable)
			{
				nGUICheckbox.enabled = selectable;
			}
			UIPlayTween nGUITween = this.component.NGUITween;
			if (nGUITween != null && nGUITween.enabled != selectable)
			{
				nGUITween.enabled = selectable;
			}
		}

		public void SetAnimationAndSprite(UXSprite uxSprite)
		{
			this.component.NGUICheckbox.activeAnimation = uxSprite.Root.GetComponent<Animation>();
			this.component.NGUICheckbox.activeSprite = uxSprite.GetUIWidget;
		}

		public void DelayedSelect(bool value)
		{
			this.component.DelayedSelect(value);
		}

		protected internal UXCheckbox(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).DelayedSelect(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).OnSelected);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).RadioGroup);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).Selected);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).InternalOnSelect(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).OnSelected = (UXCheckboxSelectedDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).RadioGroup = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).Selected = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).SetAnimationAndSprite((UXSprite)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).SetSelectable(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).SetSelected(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXCheckbox)GCHandledObjects.GCHandleToObject(instance)).SetTweenTarget((UXElement)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
