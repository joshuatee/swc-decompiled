using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXCheckboxComponent : MonoBehaviour, IUnitySerializable
	{
		private bool started;

		private UIToggle toggle;

		public UIToggle NGUICheckbox
		{
			get
			{
				return this.toggle;
			}
			set
			{
				if (this.toggle != null)
				{
					EventDelegate.Remove(this.toggle.onChange, new EventDelegate.Callback(this.OnSelectStateChanged));
				}
				this.toggle = value;
				if (this.toggle != null)
				{
					EventDelegate.Add(this.toggle.onChange, new EventDelegate.Callback(this.OnSelectStateChanged), false);
				}
			}
		}

		public UIButton NGUIButton
		{
			get;
			set;
		}

		public UIPlayTween NGUITween
		{
			get;
			set;
		}

		public UXCheckbox Checkbox
		{
			get;
			set;
		}

		public bool Selected
		{
			get
			{
				return this.NGUICheckbox != null && this.NGUICheckbox.value;
			}
			set
			{
				if (this.NGUICheckbox != null)
				{
					int radioGroup = 0;
					if (!value)
					{
						radioGroup = this.RadioGroup;
						this.RadioGroup = 0;
					}
					this.NGUICheckbox.value = value;
					if (!value)
					{
						this.RadioGroup = radioGroup;
					}
				}
			}
		}

		public int RadioGroup
		{
			get
			{
				if (!(this.NGUICheckbox == null))
				{
					return this.NGUICheckbox.group;
				}
				return 0;
			}
			set
			{
				if (this.NGUICheckbox != null)
				{
					this.NGUICheckbox.group = value;
				}
			}
		}

		public void RemoveDelegate()
		{
			if (this.NGUICheckbox != null)
			{
				EventDelegate.Remove(this.NGUICheckbox.onChange, new EventDelegate.Callback(this.OnSelectStateChanged));
			}
		}

		public void DelayedSelect(bool value)
		{
			if (base.gameObject.activeSelf)
			{
				base.StartCoroutine(this.DelayedSelectCoroutine(value));
			}
		}

		[IteratorStateMachine(typeof(UXCheckboxComponent.<DelayedSelectCoroutine>d__22))]
		private IEnumerator DelayedSelectCoroutine(bool value)
		{
			yield return null;
			this.NGUICheckbox.value = value;
			yield break;
		}

		private void Start()
		{
			this.started = true;
		}

		private void OnSelectStateChanged()
		{
			if (this.started && this.Checkbox != null)
			{
				this.Checkbox.InternalOnSelect(this.Selected);
			}
		}

		public UXCheckboxComponent()
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

		protected internal UXCheckboxComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).DelayedSelect(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).DelayedSelectCoroutine(*(sbyte*)args != 0));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Checkbox);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIButton);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).NGUICheckbox);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).NGUITween);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).RadioGroup);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Selected);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).OnSelectStateChanged();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).RemoveDelegate();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Checkbox = (UXCheckbox)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIButton = (UIButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).NGUICheckbox = (UIToggle)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).NGUITween = (UIPlayTween)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).RadioGroup = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Selected = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((UXCheckboxComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
