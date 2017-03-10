using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Button Message (Legacy)")]
public class UIButtonMessage : MonoBehaviour, IUnitySerializable
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		OnDoubleClick
	}

	public GameObject target;

	public string functionName;

	public UIButtonMessage.Trigger trigger;

	public bool includeChildren;

	private bool mStarted;

	private void Start()
	{
		this.mStarted = true;
	}

	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOver) || (!isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOut)))
		{
			this.Send();
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && this.trigger == UIButtonMessage.Trigger.OnPress) || (!isPressed && this.trigger == UIButtonMessage.Trigger.OnRelease)))
		{
			this.Send();
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	private void OnClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnClick)
		{
			this.Send();
		}
	}

	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnDoubleClick)
		{
			this.Send();
		}
	}

	private void Send()
	{
		if (string.IsNullOrEmpty(this.functionName))
		{
			return;
		}
		if (this.target == null)
		{
			this.target = base.gameObject;
		}
		if (this.includeChildren)
		{
			Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				Transform transform = componentsInChildren[i];
				transform.gameObject.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
				i++;
			}
			return;
		}
		this.target.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
	}

	public UIButtonMessage()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.target);
		}
		SerializedStateWriter.Instance.WriteString(this.functionName);
		SerializedStateWriter.Instance.WriteInt32((int)this.trigger);
		SerializedStateWriter.Instance.WriteBoolean(this.includeChildren);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.functionName = (SerializedStateReader.Instance.ReadString() as string);
		this.trigger = (UIButtonMessage.Trigger)SerializedStateReader.Instance.ReadInt32();
		this.includeChildren = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.target != null)
		{
			this.target = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.target) as GameObject);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.target;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 265);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.functionName, &var_0_cp_0[var_0_cp_1] + 402);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.trigger, &var_0_cp_0[var_0_cp_1] + 415);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.includeChildren, &var_0_cp_0[var_0_cp_1] + 423);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateReader arg_1E_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			this.target = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 265) as GameObject);
		}
		this.functionName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 402) as string);
		this.trigger = (UIButtonMessage.Trigger)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 415);
		this.includeChildren = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 423);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIButtonMessage(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonMessage)instance).target);
	}

	public static void $Set0(object instance, long value)
	{
		((UIButtonMessage)instance).target = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UIButtonMessage)instance).includeChildren;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIButtonMessage)instance).includeChildren = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).OnDoubleClick();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).OnHover(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).OnSelect(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).Send();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIButtonMessage)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
