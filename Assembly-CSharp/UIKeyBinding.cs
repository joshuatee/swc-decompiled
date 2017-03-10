using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour, IUnitySerializable
{
	public enum Action
	{
		PressAndClick,
		Select,
		All
	}

	public enum Modifier
	{
		Any,
		Shift,
		Control,
		Alt,
		None
	}

	private static List<UIKeyBinding> mList = new List<UIKeyBinding>();

	public KeyCode keyCode;

	public UIKeyBinding.Modifier modifier;

	public UIKeyBinding.Action action;

	[System.NonSerialized]
	private bool mIgnoreUp;

	[System.NonSerialized]
	private bool mIsInput;

	[System.NonSerialized]
	private bool mPress;

	public static bool IsBound(KeyCode key)
	{
		int i = 0;
		int count = UIKeyBinding.mList.Count;
		while (i < count)
		{
			UIKeyBinding uIKeyBinding = UIKeyBinding.mList[i];
			if (uIKeyBinding != null && uIKeyBinding.keyCode == key)
			{
				return true;
			}
			i++;
		}
		return false;
	}

	protected virtual void OnEnable()
	{
		UIKeyBinding.mList.Add(this);
	}

	protected virtual void OnDisable()
	{
		UIKeyBinding.mList.Remove(this);
	}

	protected virtual void Start()
	{
		UIInput component = base.GetComponent<UIInput>();
		this.mIsInput = (component != null);
		if (component != null)
		{
			EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
		}
	}

	protected virtual void OnSubmit()
	{
		if (UICamera.currentKey == this.keyCode && this.IsModifierActive())
		{
			this.mIgnoreUp = true;
		}
	}

	protected virtual bool IsModifierActive()
	{
		if (this.modifier == UIKeyBinding.Modifier.Any)
		{
			return true;
		}
		if (this.modifier == UIKeyBinding.Modifier.Alt)
		{
			if (UICamera.GetKey(KeyCode.LeftAlt) || UICamera.GetKey(KeyCode.RightAlt))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Control)
		{
			if (UICamera.GetKey(KeyCode.LeftControl) || UICamera.GetKey(KeyCode.RightControl))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Shift)
		{
			if (UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.None)
		{
			return !UICamera.GetKey(KeyCode.LeftAlt) && !UICamera.GetKey(KeyCode.RightAlt) && !UICamera.GetKey(KeyCode.LeftControl) && !UICamera.GetKey(KeyCode.RightControl) && !UICamera.GetKey(KeyCode.LeftShift) && !UICamera.GetKey(KeyCode.RightShift);
		}
		return false;
	}

	protected virtual void Update()
	{
		if (UICamera.inputHasFocus)
		{
			return;
		}
		if (this.keyCode == KeyCode.None || !this.IsModifierActive())
		{
			return;
		}
		bool flag = UICamera.GetKeyDown(this.keyCode);
		bool flag2 = UICamera.GetKeyUp(this.keyCode);
		if (flag)
		{
			this.mPress = true;
		}
		if (this.action == UIKeyBinding.Action.PressAndClick || this.action == UIKeyBinding.Action.All)
		{
			if (flag)
			{
				UICamera.currentKey = this.keyCode;
				this.OnBindingPress(true);
			}
			if (this.mPress & flag2)
			{
				UICamera.currentKey = this.keyCode;
				this.OnBindingPress(false);
				this.OnBindingClick();
			}
		}
		if ((this.action == UIKeyBinding.Action.Select || this.action == UIKeyBinding.Action.All) && flag2)
		{
			if (this.mIsInput)
			{
				if (!this.mIgnoreUp && !UICamera.inputHasFocus && this.mPress)
				{
					UICamera.selectedObject = base.gameObject;
				}
				this.mIgnoreUp = false;
			}
			else if (this.mPress)
			{
				UICamera.hoveredObject = base.gameObject;
			}
		}
		if (flag2)
		{
			this.mPress = false;
		}
	}

	protected virtual void OnBindingPress(bool pressed)
	{
		UICamera.Notify(base.gameObject, "OnPress", pressed);
	}

	protected virtual void OnBindingClick()
	{
		UICamera.Notify(base.gameObject, "OnClick", null);
	}

	public UIKeyBinding()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32((int)this.keyCode);
		SerializedStateWriter.Instance.WriteInt32((int)this.modifier);
		SerializedStateWriter.Instance.WriteInt32((int)this.action);
	}

	public override void Unity_Deserialize(int depth)
	{
		this.keyCode = (KeyCode)SerializedStateReader.Instance.ReadInt32();
		this.modifier = (UIKeyBinding.Modifier)SerializedStateReader.Instance.ReadInt32();
		this.action = (UIKeyBinding.Action)SerializedStateReader.Instance.ReadInt32();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = (int)this.keyCode;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 1045);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.modifier, &var_0_cp_0[var_0_cp_1] + 1053);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.action, &var_0_cp_0[var_0_cp_1] + 1062);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.keyCode = (KeyCode)arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1045);
		this.modifier = (UIKeyBinding.Modifier)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1053);
		this.action = (UIKeyBinding.Action)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1062);
	}

	protected internal UIKeyBinding(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIKeyBinding.IsBound((KeyCode)(*(int*)args)));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).IsModifierActive());
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).OnBindingClick();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).OnBindingPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).OnSubmit();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIKeyBinding)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
