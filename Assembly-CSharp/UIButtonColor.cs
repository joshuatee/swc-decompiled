using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Button Color"), ExecuteInEditMode]
public class UIButtonColor : UIWidgetContainer, IUnitySerializable
{
	public enum State
	{
		Normal,
		Hover,
		Pressed,
		Disabled
	}

	public GameObject tweenTarget;

	public Color hover;

	public Color pressed;

	public Color disabledColor;

	public float duration;

	[System.NonSerialized]
	protected Color mStartingColor;

	[System.NonSerialized]
	protected Color mDefaultColor;

	[System.NonSerialized]
	protected bool mInitDone;

	[System.NonSerialized]
	protected UIWidget mWidget;

	[System.NonSerialized]
	protected UIButtonColor.State mState;

	public UIButtonColor.State state
	{
		get
		{
			return this.mState;
		}
		set
		{
			this.SetState(value, false);
		}
	}

	public Color defaultColor
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mDefaultColor;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			this.mDefaultColor = value;
			UIButtonColor.State state = this.mState;
			this.mState = UIButtonColor.State.Disabled;
			this.SetState(state, false);
		}
	}

	public virtual bool isEnabled
	{
		get
		{
			return base.enabled;
		}
		set
		{
			base.enabled = value;
		}
	}

	public void ResetDefaultColor()
	{
		this.defaultColor = this.mStartingColor;
	}

	public void CacheDefaultColor()
	{
		if (!this.mInitDone)
		{
			this.OnInit();
		}
	}

	private void Start()
	{
		if (!this.mInitDone)
		{
			this.OnInit();
		}
		if (!this.isEnabled)
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	protected virtual void OnInit()
	{
		this.mInitDone = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
		if (this.tweenTarget != null)
		{
			this.mWidget = this.tweenTarget.GetComponent<UIWidget>();
		}
		if (this.mWidget != null)
		{
			this.mDefaultColor = this.mWidget.color;
			this.mStartingColor = this.mDefaultColor;
			return;
		}
		if (this.tweenTarget != null)
		{
			Renderer component = this.tweenTarget.GetComponent<Renderer>();
			if (component != null)
			{
				this.mDefaultColor = (Application.isPlaying ? component.material.color : component.sharedMaterial.color);
				this.mStartingColor = this.mDefaultColor;
				return;
			}
			Light component2 = this.tweenTarget.GetComponent<Light>();
			if (component2 != null)
			{
				this.mDefaultColor = component2.color;
				this.mStartingColor = this.mDefaultColor;
				return;
			}
			this.tweenTarget = null;
			this.mInitDone = false;
		}
	}

	protected virtual void OnEnable()
	{
		if (this.mInitDone)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (UICamera.currentTouch.pressed == base.gameObject)
			{
				this.OnPress(true);
				return;
			}
			if (UICamera.currentTouch.current == base.gameObject)
			{
				this.OnHover(true);
			}
		}
	}

	protected virtual void OnDisable()
	{
		if (this.mInitDone && this.tweenTarget != null)
		{
			this.SetState(UIButtonColor.State.Normal, true);
			TweenColor component = this.tweenTarget.GetComponent<TweenColor>();
			if (component != null)
			{
				component.value = this.mDefaultColor;
				component.enabled = false;
			}
		}
	}

	protected virtual void OnHover(bool isOver)
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(isOver ? UIButtonColor.State.Hover : UIButtonColor.State.Normal, false);
			}
		}
	}

	protected virtual void OnPress(bool isPressed)
	{
		if (this.isEnabled && UICamera.currentTouch != null)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				if (isPressed)
				{
					this.SetState(UIButtonColor.State.Pressed, false);
					return;
				}
				if (UICamera.currentTouch.current == base.gameObject)
				{
					if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
					{
						this.SetState(UIButtonColor.State.Hover, false);
						return;
					}
					if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.hoveredObject == base.gameObject)
					{
						this.SetState(UIButtonColor.State.Hover, false);
						return;
					}
					this.SetState(UIButtonColor.State.Normal, false);
					return;
				}
				else
				{
					this.SetState(UIButtonColor.State.Normal, false);
				}
			}
		}
	}

	protected virtual void OnDragOver()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Pressed, false);
			}
		}
	}

	protected virtual void OnDragOut()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Normal, false);
			}
		}
	}

	public virtual void SetState(UIButtonColor.State state, bool instant)
	{
		if (!this.mInitDone)
		{
			this.mInitDone = true;
			this.OnInit();
		}
		if (this.mState != state)
		{
			this.mState = state;
			this.UpdateColor(instant);
		}
	}

	public void UpdateColor(bool instant)
	{
		if (this.tweenTarget != null)
		{
			TweenColor tweenColor;
			switch (this.mState)
			{
			case UIButtonColor.State.Hover:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.hover);
				break;
			case UIButtonColor.State.Pressed:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.pressed);
				break;
			case UIButtonColor.State.Disabled:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.disabledColor);
				break;
			default:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.mDefaultColor);
				break;
			}
			if (instant && tweenColor != null)
			{
				tweenColor.value = tweenColor.to;
				tweenColor.enabled = false;
			}
		}
	}

	public UIButtonColor()
	{
		this.hover = new Color(0.882352948f, 0.784313738f, 0.5882353f, 1f);
		this.pressed = new Color(0.7176471f, 0.6392157f, 0.482352942f, 1f);
		this.disabledColor = Color.grey;
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
		if (depth <= 7)
		{
			this.disabledColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.duration);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.tweenTarget = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
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
		if (depth <= 7)
		{
			this.disabledColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.duration = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.tweenTarget != null)
		{
			this.tweenTarget = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.tweenTarget) as GameObject);
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
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 122);
			this.disabledColor.Unity_NamedSerialize(depth + 1);
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
			this.tweenTarget = (arg_1B_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 96) as GameObject);
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
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 122);
			this.disabledColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.duration = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 136);
	}

	protected internal UIButtonColor(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonColor)instance).tweenTarget);
	}

	public static void $Set0(object instance, long value)
	{
		((UIButtonColor)instance).tweenTarget = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get1(object instance, int index)
	{
		UIButtonColor expr_06_cp_0 = (UIButtonColor)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.hover.r;
		case 1:
			return expr_06_cp_0.hover.g;
		case 2:
			return expr_06_cp_0.hover.b;
		case 3:
			return expr_06_cp_0.hover.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set1(object instance, float value, int index)
	{
		UIButtonColor expr_06_cp_0 = (UIButtonColor)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.hover.r = value;
			return;
		case 1:
			expr_06_cp_0.hover.g = value;
			return;
		case 2:
			expr_06_cp_0.hover.b = value;
			return;
		case 3:
			expr_06_cp_0.hover.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get2(object instance, int index)
	{
		UIButtonColor expr_06_cp_0 = (UIButtonColor)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.pressed.r;
		case 1:
			return expr_06_cp_0.pressed.g;
		case 2:
			return expr_06_cp_0.pressed.b;
		case 3:
			return expr_06_cp_0.pressed.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set2(object instance, float value, int index)
	{
		UIButtonColor expr_06_cp_0 = (UIButtonColor)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.pressed.r = value;
			return;
		case 1:
			expr_06_cp_0.pressed.g = value;
			return;
		case 2:
			expr_06_cp_0.pressed.b = value;
			return;
		case 3:
			expr_06_cp_0.pressed.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get3(object instance, int index)
	{
		UIButtonColor expr_06_cp_0 = (UIButtonColor)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.disabledColor.r;
		case 1:
			return expr_06_cp_0.disabledColor.g;
		case 2:
			return expr_06_cp_0.disabledColor.b;
		case 3:
			return expr_06_cp_0.disabledColor.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set3(object instance, float value, int index)
	{
		UIButtonColor expr_06_cp_0 = (UIButtonColor)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.disabledColor.r = value;
			return;
		case 1:
			expr_06_cp_0.disabledColor.g = value;
			return;
		case 2:
			expr_06_cp_0.disabledColor.b = value;
			return;
		case 3:
			expr_06_cp_0.disabledColor.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get4(object instance)
	{
		return ((UIButtonColor)instance).duration;
	}

	public static void $Set4(object instance, float value)
	{
		((UIButtonColor)instance).duration = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).CacheDefaultColor();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).defaultColor);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).isEnabled);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).state);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).OnDragOut();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).OnDragOver();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).OnHover(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).OnInit();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).ResetDefaultColor();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).defaultColor = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).isEnabled = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).state = (UIButtonColor.State)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).SetState((UIButtonColor.State)(*(int*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UIButtonColor)GCHandledObjects.GCHandleToObject(instance)).UpdateColor(*(sbyte*)args != 0);
		return -1L;
	}
}
