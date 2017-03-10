using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Play Animation"), ExecuteInEditMode]
public class UIPlayAnimation : MonoBehaviour, IUnitySerializable
{
	public static UIPlayAnimation current;

	public Animation target;

	public Animator animator;

	public string clipName;

	public Trigger trigger;

	public Direction playDirection;

	public bool resetOnPlay;

	public bool clearSelection;

	public EnableCondition ifDisabledOnPlay;

	public DisableCondition disableWhenFinished;

	public List<EventDelegate> onFinished;

	[HideInInspector, SerializeField]
	protected internal GameObject eventReceiver;

	[HideInInspector, SerializeField]
	protected internal string callWhenFinished;

	private bool mStarted;

	private bool mActivated;

	private bool dragHighlight;

	private bool dualState
	{
		get
		{
			return this.trigger == Trigger.OnPress || this.trigger == Trigger.OnHover;
		}
	}

	private void Awake()
	{
		UIButton component = base.GetComponent<UIButton>();
		if (component != null)
		{
			this.dragHighlight = component.dragHighlight;
		}
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (this.target == null && this.animator == null)
		{
			this.animator = base.GetComponentInChildren<Animator>();
		}
		if (this.animator != null)
		{
			if (this.animator.enabled)
			{
				this.animator.enabled = false;
			}
			return;
		}
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<Animation>();
		}
		if (this.target != null && this.target.enabled)
		{
			this.target.enabled = false;
		}
	}

	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (this.trigger == Trigger.OnPress || this.trigger == Trigger.OnPressTrue)
			{
				this.mActivated = (UICamera.currentTouch.pressed == base.gameObject);
			}
			if (this.trigger == Trigger.OnHover || this.trigger == Trigger.OnHoverTrue)
			{
				this.mActivated = (UICamera.currentTouch.current == base.gameObject);
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	private void OnDisable()
	{
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	private void OnHover(bool isOver)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue & isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver))
		{
			this.Play(isOver, this.dualState);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (!base.enabled)
		{
			return;
		}
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue & isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed))
		{
			this.Play(isPressed, this.dualState);
		}
	}

	private void OnClick()
	{
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true, false);
		}
	}

	private void OnDoubleClick()
	{
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true, false);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue & isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected))
		{
			this.Play(isSelected, this.dualState);
		}
	}

	private void OnToggle()
	{
		if (!base.enabled || UIToggle.current == null)
		{
			return;
		}
		if (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && UIToggle.current.value) || (this.trigger == Trigger.OnActivateFalse && !UIToggle.current.value))
		{
			this.Play(UIToggle.current.value, this.dualState);
		}
	}

	private void OnDragOver()
	{
		if (base.enabled && this.dualState)
		{
			if (UICamera.currentTouch.dragged == base.gameObject)
			{
				this.Play(true, true);
				return;
			}
			if (this.dragHighlight && this.trigger == Trigger.OnPress)
			{
				this.Play(true, true);
			}
		}
	}

	private void OnDragOut()
	{
		if (base.enabled && this.dualState && UICamera.hoveredObject != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	private void OnDrop(GameObject go)
	{
		if (base.enabled && this.trigger == Trigger.OnPress && UICamera.currentTouch.dragged != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	public void Play(bool forward)
	{
		this.Play(forward, true);
	}

	public void Play(bool forward, bool onlyIfDifferent)
	{
		if (this.target || this.animator)
		{
			if (onlyIfDifferent)
			{
				if (this.mActivated == forward)
				{
					return;
				}
				this.mActivated = forward;
			}
			if (this.clearSelection && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			int num = (int)(-(int)this.playDirection);
			Direction direction = forward ? this.playDirection : ((Direction)num);
			ActiveAnimation activeAnimation = this.target ? ActiveAnimation.Play(this.target, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished) : ActiveAnimation.Play(this.animator, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished);
			if (activeAnimation != null)
			{
				if (this.resetOnPlay)
				{
					activeAnimation.Reset();
				}
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
				}
			}
		}
	}

	public void PlayForward()
	{
		this.Play(true);
	}

	public void PlayReverse()
	{
		this.Play(false);
	}

	private void OnFinished()
	{
		if (UIPlayAnimation.current == null)
		{
			UIPlayAnimation.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			this.eventReceiver = null;
			UIPlayAnimation.current = null;
		}
	}

	public UIPlayAnimation()
	{
		this.playDirection = Direction.Forward;
		this.onFinished = new List<EventDelegate>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.target);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.animator);
		}
		SerializedStateWriter.Instance.WriteString(this.clipName);
		SerializedStateWriter.Instance.WriteInt32((int)this.trigger);
		SerializedStateWriter.Instance.WriteInt32((int)this.playDirection);
		SerializedStateWriter.Instance.WriteBoolean(this.resetOnPlay);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.clearSelection);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.ifDisabledOnPlay);
		SerializedStateWriter.Instance.WriteInt32((int)this.disableWhenFinished);
		if (depth <= 7)
		{
			if (this.onFinished == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onFinished.Count);
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					((this.onFinished[i] != null) ? this.onFinished[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver);
		}
		SerializedStateWriter.Instance.WriteString(this.callWhenFinished);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target = (SerializedStateReader.Instance.ReadUnityEngineObject() as Animation);
		}
		if (depth <= 7)
		{
			this.animator = (SerializedStateReader.Instance.ReadUnityEngineObject() as Animator);
		}
		this.clipName = (SerializedStateReader.Instance.ReadString() as string);
		this.trigger = (Trigger)SerializedStateReader.Instance.ReadInt32();
		this.playDirection = (Direction)SerializedStateReader.Instance.ReadInt32();
		this.resetOnPlay = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.clearSelection = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.ifDisabledOnPlay = (EnableCondition)SerializedStateReader.Instance.ReadInt32();
		this.disableWhenFinished = (DisableCondition)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onFinished = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				eventDelegate.Unity_Deserialize(depth + 1);
				this.onFinished.Add(eventDelegate);
			}
		}
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.callWhenFinished = (SerializedStateReader.Instance.ReadString() as string);
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.target != null)
		{
			this.target = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.target) as Animation);
		}
		if (this.animator != null)
		{
			this.animator = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.animator) as Animator);
		}
		if (depth <= 7)
		{
			if (this.onFinished != null)
			{
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate eventDelegate = this.onFinished[i];
					if (eventDelegate != null)
					{
						eventDelegate.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (this.eventReceiver != null)
		{
			this.eventReceiver = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.eventReceiver) as GameObject);
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
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.animator, &var_0_cp_0[var_0_cp_1] + 1069);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.clipName, &var_0_cp_0[var_0_cp_1] + 1078);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.trigger, &var_0_cp_0[var_0_cp_1] + 415);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.playDirection, &var_0_cp_0[var_0_cp_1] + 1087);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.resetOnPlay, &var_0_cp_0[var_0_cp_1] + 1101);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.clearSelection, &var_0_cp_0[var_0_cp_1] + 1113);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.ifDisabledOnPlay, &var_0_cp_0[var_0_cp_1] + 1128);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.disableWhenFinished, &var_0_cp_0[var_0_cp_1] + 1145);
		if (depth <= 7)
		{
			if (this.onFinished == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85, this.onFinished.Count);
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate arg_182_0 = (this.onFinished[i] != null) ? this.onFinished[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_182_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver, &var_0_cp_0[var_0_cp_1] + 1165);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.callWhenFinished, &var_0_cp_0[var_0_cp_1] + 1179);
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
			this.target = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 265) as Animation);
		}
		if (depth <= 7)
		{
			this.animator = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1069) as Animator);
		}
		this.clipName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 1078) as string);
		this.trigger = (Trigger)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 415);
		this.playDirection = (Direction)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1087);
		this.resetOnPlay = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1101);
		SerializedNamedStateReader.Instance.Align();
		this.clearSelection = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1113);
		SerializedNamedStateReader.Instance.Align();
		this.ifDisabledOnPlay = (EnableCondition)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1128);
		this.disableWhenFinished = (DisableCondition)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1145);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85);
			this.onFinished = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_14C_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_14C_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onFinished.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1165) as GameObject);
		}
		this.callWhenFinished = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 1179) as string);
	}

	protected internal UIPlayAnimation(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPlayAnimation)instance).target);
	}

	public static void $Set0(object instance, long value)
	{
		((UIPlayAnimation)instance).target = (Animation)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPlayAnimation)instance).animator);
	}

	public static void $Set1(object instance, long value)
	{
		((UIPlayAnimation)instance).animator = (Animator)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get2(object instance)
	{
		return ((UIPlayAnimation)instance).resetOnPlay;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIPlayAnimation)instance).resetOnPlay = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UIPlayAnimation)instance).clearSelection;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIPlayAnimation)instance).clearSelection = value;
	}

	public static long $Get4(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPlayAnimation)instance).eventReceiver);
	}

	public static void $Set4(object instance, long value)
	{
		((UIPlayAnimation)instance).eventReceiver = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).dualState);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnDoubleClick();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnDragOut();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnDragOver();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnDrop((GameObject)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnFinished();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnHover(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnSelect(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).OnToggle();
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).Play(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).Play(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).PlayForward();
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).PlayReverse();
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((UIPlayAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
