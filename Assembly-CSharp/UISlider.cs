using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/NGUI Slider"), ExecuteInEditMode]
public class UISlider : UIProgressBar, IUnitySerializable
{
	private enum Direction
	{
		Horizontal,
		Vertical,
		Upgraded
	}

	[HideInInspector, SerializeField]
	protected internal Transform foreground;

	[HideInInspector, SerializeField]
	protected internal float rawValue;

	[HideInInspector, SerializeField]
	protected internal UISlider.Direction direction;

	[HideInInspector, SerializeField]
	protected bool mInverted;

	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	[Obsolete("Use 'value' instead")]
	public float sliderValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	[Obsolete("Use 'fillDirection' instead")]
	public bool inverted
	{
		get
		{
			return base.isInverted;
		}
		set
		{
		}
	}

	protected override void Upgrade()
	{
		if (this.direction != UISlider.Direction.Upgraded)
		{
			this.mValue = this.rawValue;
			if (this.foreground != null)
			{
				this.mFG = this.foreground.GetComponent<UIWidget>();
			}
			if (this.direction == UISlider.Direction.Horizontal)
			{
				this.mFill = (this.mInverted ? UIProgressBar.FillDirection.RightToLeft : UIProgressBar.FillDirection.LeftToRight);
			}
			else
			{
				this.mFill = (this.mInverted ? UIProgressBar.FillDirection.TopToBottom : UIProgressBar.FillDirection.BottomToTop);
			}
			this.direction = UISlider.Direction.Upgraded;
		}
	}

	protected override void OnStart()
	{
		GameObject go = (this.mBG != null && (this.mBG.GetComponent<Collider>() != null || this.mBG.GetComponent<Collider2D>() != null)) ? this.mBG.gameObject : base.gameObject;
		UIEventListener uIEventListener = UIEventListener.Get(go);
		UIEventListener expr_50 = uIEventListener;
		expr_50.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_50.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
		UIEventListener expr_72 = uIEventListener;
		expr_72.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_72.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
		if (this.thumb != null && (this.thumb.GetComponent<Collider>() != null || this.thumb.GetComponent<Collider2D>() != null) && (this.mFG == null || this.thumb != this.mFG.cachedTransform))
		{
			UIEventListener uIEventListener2 = UIEventListener.Get(this.thumb.gameObject);
			UIEventListener expr_102 = uIEventListener2;
			expr_102.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_102.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
			UIEventListener expr_124 = uIEventListener2;
			expr_124.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_124.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
		}
	}

	protected void OnPressBackground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastEventPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	protected void OnDragBackground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastEventPosition);
	}

	protected void OnPressForeground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		if (isPressed)
		{
			this.mOffset = ((this.mFG == null) ? 0f : (base.value - base.ScreenToValue(UICamera.lastEventPosition)));
			return;
		}
		if (this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	protected void OnDragForeground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = this.mOffset + base.ScreenToValue(UICamera.lastEventPosition);
	}

	public override void OnPan(Vector2 delta)
	{
		if (base.enabled && this.isColliderEnabled)
		{
			base.OnPan(delta);
		}
	}

	public UISlider()
	{
		this.rawValue = 1f;
		this.direction = UISlider.Direction.Upgraded;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.thumb);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mBG);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mFG);
		}
		SerializedStateWriter.Instance.WriteSingle(this.mValue);
		SerializedStateWriter.Instance.WriteInt32((int)this.mFill);
		SerializedStateWriter.Instance.WriteInt32(this.numberOfSteps);
		if (depth <= 7)
		{
			if (this.onChange == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onChange.Count);
				for (int i = 0; i < this.onChange.Count; i++)
				{
					((this.onChange[i] != null) ? this.onChange[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.foreground);
		}
		SerializedStateWriter.Instance.WriteSingle(this.rawValue);
		SerializedStateWriter.Instance.WriteInt32((int)this.direction);
		SerializedStateWriter.Instance.WriteBoolean(this.mInverted);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.thumb = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		if (depth <= 7)
		{
			this.mBG = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIWidget);
		}
		if (depth <= 7)
		{
			this.mFG = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIWidget);
		}
		this.mValue = SerializedStateReader.Instance.ReadSingle();
		this.mFill = (UIProgressBar.FillDirection)SerializedStateReader.Instance.ReadInt32();
		this.numberOfSteps = SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				eventDelegate.Unity_Deserialize(depth + 1);
				this.onChange.Add(eventDelegate);
			}
		}
		if (depth <= 7)
		{
			this.foreground = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		this.rawValue = SerializedStateReader.Instance.ReadSingle();
		this.direction = (UISlider.Direction)SerializedStateReader.Instance.ReadInt32();
		this.mInverted = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.thumb != null)
		{
			this.thumb = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.thumb) as Transform);
		}
		if (this.mBG != null)
		{
			this.mBG = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mBG) as UIWidget);
		}
		if (this.mFG != null)
		{
			this.mFG = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mFG) as UIWidget);
		}
		if (depth <= 7)
		{
			if (this.onChange != null)
			{
				for (int i = 0; i < this.onChange.Count; i++)
				{
					EventDelegate eventDelegate = this.onChange[i];
					if (eventDelegate != null)
					{
						eventDelegate.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (this.foreground != null)
		{
			this.foreground = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.foreground) as Transform);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.thumb;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 1570);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mBG, &var_0_cp_0[var_0_cp_1] + 1576);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mFG, &var_0_cp_0[var_0_cp_1] + 1580);
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.mValue, &var_0_cp_0[var_0_cp_1] + 1584);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mFill, &var_0_cp_0[var_0_cp_1] + 1591);
		SerializedNamedStateWriter.Instance.WriteInt32(this.numberOfSteps, &var_0_cp_0[var_0_cp_1] + 1597);
		if (depth <= 7)
		{
			if (this.onChange == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446, this.onChange.Count);
				for (int i = 0; i < this.onChange.Count; i++)
				{
					EventDelegate arg_131_0 = (this.onChange[i] != null) ? this.onChange[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_131_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.foreground, &var_0_cp_0[var_0_cp_1] + 1619);
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.rawValue, &var_0_cp_0[var_0_cp_1] + 1630);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.direction, &var_0_cp_0[var_0_cp_1] + 1639);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mInverted, &var_0_cp_0[var_0_cp_1] + 1649);
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
			this.thumb = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1570) as Transform);
		}
		if (depth <= 7)
		{
			this.mBG = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1576) as UIWidget);
		}
		if (depth <= 7)
		{
			this.mFG = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1580) as UIWidget);
		}
		this.mValue = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1584);
		this.mFill = (UIProgressBar.FillDirection)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1591);
		this.numberOfSteps = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1597);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446);
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_F8_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_F8_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onChange.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			this.foreground = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1619) as Transform);
		}
		this.rawValue = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1630);
		this.direction = (UISlider.Direction)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1639);
		this.mInverted = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1649);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UISlider(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISlider)instance).foreground);
	}

	public static void $Set0(object instance, long value)
	{
		((UISlider)instance).foreground = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get1(object instance)
	{
		return ((UISlider)instance).rawValue;
	}

	public static void $Set1(object instance, float value)
	{
		((UISlider)instance).rawValue = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UISlider)instance).mInverted;
	}

	public static void $Set2(object instance, bool value)
	{
		((UISlider)instance).mInverted = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISlider)GCHandledObjects.GCHandleToObject(instance)).inverted);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISlider)GCHandledObjects.GCHandleToObject(instance)).isColliderEnabled);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISlider)GCHandledObjects.GCHandleToObject(instance)).sliderValue);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).OnDragBackground((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).OnDragForeground((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).OnPan(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).OnPressBackground((GameObject)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).OnPressForeground((GameObject)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).OnStart();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).inverted = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).sliderValue = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UISlider)GCHandledObjects.GCHandleToObject(instance)).Upgrade();
		return -1L;
	}
}
