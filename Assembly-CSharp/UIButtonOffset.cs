using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour, IUnitySerializable
{
	public Transform tweenTarget;

	public Vector3 hover;

	public Vector3 pressed;

	public float duration;

	[System.NonSerialized]
	private Vector3 mPos;

	[System.NonSerialized]
	private bool mStarted;

	[System.NonSerialized]
	private bool mPressed;

	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mPos = this.tweenTarget.localPosition;
		}
	}

	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.value = this.mPos;
				component.enabled = false;
			}
		}
	}

	private void OnPress(bool isPressed)
	{
		this.mPressed = isPressed;
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, isPressed ? (this.mPos + this.pressed) : (UICamera.IsHighlighted(base.gameObject) ? (this.mPos + this.hover) : this.mPos)).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, isOver ? (this.mPos + this.hover) : this.mPos).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnDragOver()
	{
		if (this.mPressed)
		{
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, this.mPos + this.hover).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnDragOut()
	{
		if (this.mPressed)
		{
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, this.mPos).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	public UIButtonOffset()
	{
		this.hover = Vector3.zero;
		this.pressed = new Vector3(2f, -2f);
		this.duration = 0.2f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.tweenTarget);
		}
		if (depth <= 7)
		{
			this.hover.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.pressed.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.duration);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.tweenTarget = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		if (depth <= 7)
		{
			this.hover.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.pressed.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.duration = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.tweenTarget != null)
		{
			this.tweenTarget = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.tweenTarget) as Transform);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_20_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_20_1 = this.tweenTarget;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_20_0.WriteUnityEngineObject(arg_20_1, &var_0_cp_0[var_0_cp_1] + 96);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 108);
			this.hover.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 114);
			this.pressed.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.duration, &var_0_cp_0[var_0_cp_1] + 136);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateReader arg_1B_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			this.tweenTarget = (arg_1B_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 96) as Transform);
		}
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 108);
			this.hover.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 114);
			this.pressed.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.duration = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 136);
	}

	protected internal UIButtonOffset(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonOffset)instance).tweenTarget);
	}

	public static void $Set0(object instance, long value)
	{
		((UIButtonOffset)instance).tweenTarget = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get1(object instance, int index)
	{
		UIButtonOffset expr_06_cp_0 = (UIButtonOffset)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.hover.x;
		case 1:
			return expr_06_cp_0.hover.y;
		case 2:
			return expr_06_cp_0.hover.z;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set1(object instance, float value, int index)
	{
		UIButtonOffset expr_06_cp_0 = (UIButtonOffset)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.hover.x = value;
			return;
		case 1:
			expr_06_cp_0.hover.y = value;
			return;
		case 2:
			expr_06_cp_0.hover.z = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get2(object instance, int index)
	{
		UIButtonOffset expr_06_cp_0 = (UIButtonOffset)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.pressed.x;
		case 1:
			return expr_06_cp_0.pressed.y;
		case 2:
			return expr_06_cp_0.pressed.z;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set2(object instance, float value, int index)
	{
		UIButtonOffset expr_06_cp_0 = (UIButtonOffset)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.pressed.x = value;
			return;
		case 1:
			expr_06_cp_0.pressed.y = value;
			return;
		case 2:
			expr_06_cp_0.pressed.z = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get3(object instance)
	{
		return ((UIButtonOffset)instance).duration;
	}

	public static void $Set3(object instance, float value)
	{
		((UIButtonOffset)instance).duration = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).OnDragOut();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).OnDragOver();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).OnHover(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).OnSelect(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIButtonOffset)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
