using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Forward Events (Legacy)")]
public class UIForwardEvents : MonoBehaviour, IUnitySerializable
{
	public GameObject target;

	public bool onHover;

	public bool onPress;

	public bool onClick;

	public bool onDoubleClick;

	public bool onSelect;

	public bool onDrag;

	public bool onDrop;

	public bool onSubmit;

	public bool onScroll;

	private void OnHover(bool isOver)
	{
		if (this.onHover && this.target != null)
		{
			this.target.SendMessage("OnHover", isOver, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnPress(bool pressed)
	{
		if (this.onPress && this.target != null)
		{
			this.target.SendMessage("OnPress", pressed, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnClick()
	{
		if (this.onClick && this.target != null)
		{
			this.target.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnDoubleClick()
	{
		if (this.onDoubleClick && this.target != null)
		{
			this.target.SendMessage("OnDoubleClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnSelect(bool selected)
	{
		if (this.onSelect && this.target != null)
		{
			this.target.SendMessage("OnSelect", selected, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag && this.target != null)
		{
			this.target.SendMessage("OnDrag", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnDrop(GameObject go)
	{
		if (this.onDrop && this.target != null)
		{
			this.target.SendMessage("OnDrop", go, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnSubmit()
	{
		if (this.onSubmit && this.target != null)
		{
			this.target.SendMessage("OnSubmit", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnScroll(float delta)
	{
		if (this.onScroll && this.target != null)
		{
			this.target.SendMessage("OnScroll", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	public UIForwardEvents()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.target);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.onHover);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.onPress);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.onClick);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.onDoubleClick);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.onSelect);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.onDrag);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.onDrop);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.onSubmit);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.onScroll);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.onHover = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.onPress = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.onClick = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.onDoubleClick = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.onSelect = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.onDrag = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.onDrop = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.onSubmit = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.onScroll = SerializedStateReader.Instance.ReadBoolean();
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
		SerializedNamedStateWriter.Instance.WriteBoolean(this.onHover, &var_0_cp_0[var_0_cp_1] + 895);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.onPress, &var_0_cp_0[var_0_cp_1] + 793);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.onClick, &var_0_cp_0[var_0_cp_1] + 257);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.onDoubleClick, &var_0_cp_0[var_0_cp_1] + 831);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.onSelect, &var_0_cp_0[var_0_cp_1] + 811);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.onDrag, &var_0_cp_0[var_0_cp_1] + 888);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.onDrop, &var_0_cp_0[var_0_cp_1] + 903);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.onSubmit, &var_0_cp_0[var_0_cp_1] + 910);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.onScroll, &var_0_cp_0[var_0_cp_1] + 919);
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
		this.onHover = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 895);
		SerializedNamedStateReader.Instance.Align();
		this.onPress = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 793);
		SerializedNamedStateReader.Instance.Align();
		this.onClick = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 257);
		SerializedNamedStateReader.Instance.Align();
		this.onDoubleClick = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 831);
		SerializedNamedStateReader.Instance.Align();
		this.onSelect = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 811);
		SerializedNamedStateReader.Instance.Align();
		this.onDrag = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 888);
		SerializedNamedStateReader.Instance.Align();
		this.onDrop = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 903);
		SerializedNamedStateReader.Instance.Align();
		this.onSubmit = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 910);
		SerializedNamedStateReader.Instance.Align();
		this.onScroll = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 919);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIForwardEvents(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIForwardEvents)instance).target);
	}

	public static void $Set0(object instance, long value)
	{
		((UIForwardEvents)instance).target = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UIForwardEvents)instance).onHover;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIForwardEvents)instance).onHover = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UIForwardEvents)instance).onPress;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIForwardEvents)instance).onPress = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UIForwardEvents)instance).onClick;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIForwardEvents)instance).onClick = value;
	}

	public static bool $Get4(object instance)
	{
		return ((UIForwardEvents)instance).onDoubleClick;
	}

	public static void $Set4(object instance, bool value)
	{
		((UIForwardEvents)instance).onDoubleClick = value;
	}

	public static bool $Get5(object instance)
	{
		return ((UIForwardEvents)instance).onSelect;
	}

	public static void $Set5(object instance, bool value)
	{
		((UIForwardEvents)instance).onSelect = value;
	}

	public static bool $Get6(object instance)
	{
		return ((UIForwardEvents)instance).onDrag;
	}

	public static void $Set6(object instance, bool value)
	{
		((UIForwardEvents)instance).onDrag = value;
	}

	public static bool $Get7(object instance)
	{
		return ((UIForwardEvents)instance).onDrop;
	}

	public static void $Set7(object instance, bool value)
	{
		((UIForwardEvents)instance).onDrop = value;
	}

	public static bool $Get8(object instance)
	{
		return ((UIForwardEvents)instance).onSubmit;
	}

	public static void $Set8(object instance, bool value)
	{
		((UIForwardEvents)instance).onSubmit = value;
	}

	public static bool $Get9(object instance)
	{
		return ((UIForwardEvents)instance).onScroll;
	}

	public static void $Set9(object instance, bool value)
	{
		((UIForwardEvents)instance).onScroll = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).OnDoubleClick();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).OnDrop((GameObject)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).OnHover(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).OnSelect(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).OnSubmit();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIForwardEvents)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
