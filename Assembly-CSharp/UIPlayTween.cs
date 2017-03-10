using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Play Tween"), ExecuteInEditMode]
public class UIPlayTween : MonoBehaviour, IUnitySerializable
{
	public static UIPlayTween current;

	public GameObject tweenTarget;

	public int tweenGroup;

	public Trigger trigger;

	public Direction playDirection;

	public bool resetOnPlay;

	public bool resetIfDisabled;

	public EnableCondition ifDisabledOnPlay;

	public DisableCondition disableWhenFinished;

	public bool includeChildren;

	public List<EventDelegate> onFinished;

	[HideInInspector, SerializeField]
	protected internal GameObject eventReceiver;

	[HideInInspector, SerializeField]
	protected internal string callWhenFinished;

	private UITweener[] mTweens;

	private bool mStarted;

	private int mActive;

	private bool mActivated;

	private void Awake()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
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

	private void OnDragOver()
	{
		if (this.trigger == Trigger.OnHover)
		{
			this.OnHover(true);
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled && (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue & isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver)))
		{
			this.mActivated = (isOver && this.trigger == Trigger.OnHover);
			this.Play(isOver);
		}
	}

	private void OnDragOut()
	{
		if (base.enabled && this.mActivated)
		{
			this.mActivated = false;
			this.Play(false);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue & isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed)))
		{
			this.mActivated = (isPressed && this.trigger == Trigger.OnPress);
			this.Play(isPressed);
		}
	}

	private void OnClick()
	{
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true);
		}
	}

	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue & isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected)))
		{
			this.mActivated = (isSelected && this.trigger == Trigger.OnSelect);
			this.Play(isSelected);
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
			this.Play(UIToggle.current.value);
		}
	}

	private void Update()
	{
		if (this.disableWhenFinished != DisableCondition.DoNotDisable && this.mTweens != null)
		{
			bool flag = true;
			bool flag2 = true;
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uITweener = this.mTweens[i];
				if (uITweener.tweenGroup == this.tweenGroup)
				{
					if (uITweener.enabled)
					{
						flag = false;
						break;
					}
					if (uITweener.direction != (Direction)this.disableWhenFinished)
					{
						flag2 = false;
					}
				}
				i++;
			}
			if (flag)
			{
				if (flag2)
				{
					NGUITools.SetActive(this.tweenTarget, false);
				}
				this.mTweens = null;
			}
		}
	}

	public void Play(bool forward)
	{
		this.mActive = 0;
		GameObject gameObject = (this.tweenTarget == null) ? base.gameObject : this.tweenTarget;
		if (!NGUITools.GetActive(gameObject))
		{
			if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			NGUITools.SetActive(gameObject, true);
		}
		this.mTweens = (this.includeChildren ? gameObject.GetComponentsInChildren<UITweener>() : gameObject.GetComponents<UITweener>());
		if (this.mTweens.Length == 0)
		{
			if (this.disableWhenFinished != DisableCondition.DoNotDisable)
			{
				NGUITools.SetActive(this.tweenTarget, false);
				return;
			}
		}
		else
		{
			bool flag = false;
			if (this.playDirection == Direction.Reverse)
			{
				forward = !forward;
			}
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uITweener = this.mTweens[i];
				if (uITweener.tweenGroup == this.tweenGroup)
				{
					if (!flag && !NGUITools.GetActive(gameObject))
					{
						flag = true;
						NGUITools.SetActive(gameObject, true);
					}
					this.mActive++;
					if (this.playDirection == Direction.Toggle)
					{
						EventDelegate.Add(uITweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
						uITweener.Toggle();
					}
					else
					{
						if (this.resetOnPlay || (this.resetIfDisabled && !uITweener.enabled))
						{
							uITweener.Play(forward);
							uITweener.ResetToBeginning();
						}
						EventDelegate.Add(uITweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
						uITweener.Play(forward);
					}
				}
				i++;
			}
		}
	}

	private void OnFinished()
	{
		int num = this.mActive - 1;
		this.mActive = num;
		if (num == 0 && UIPlayTween.current == null)
		{
			UIPlayTween.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			this.eventReceiver = null;
			UIPlayTween.current = null;
		}
	}

	public UIPlayTween()
	{
		this.playDirection = Direction.Forward;
		this.onFinished = new List<EventDelegate>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.tweenTarget);
		}
		SerializedStateWriter.Instance.WriteInt32(this.tweenGroup);
		SerializedStateWriter.Instance.WriteInt32((int)this.trigger);
		SerializedStateWriter.Instance.WriteInt32((int)this.playDirection);
		SerializedStateWriter.Instance.WriteBoolean(this.resetOnPlay);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.resetIfDisabled);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.ifDisabledOnPlay);
		SerializedStateWriter.Instance.WriteInt32((int)this.disableWhenFinished);
		SerializedStateWriter.Instance.WriteBoolean(this.includeChildren);
		SerializedStateWriter.Instance.Align();
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
			this.tweenTarget = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.tweenGroup = SerializedStateReader.Instance.ReadInt32();
		this.trigger = (Trigger)SerializedStateReader.Instance.ReadInt32();
		this.playDirection = (Direction)SerializedStateReader.Instance.ReadInt32();
		this.resetOnPlay = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.resetIfDisabled = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.ifDisabledOnPlay = (EnableCondition)SerializedStateReader.Instance.ReadInt32();
		this.disableWhenFinished = (DisableCondition)SerializedStateReader.Instance.ReadInt32();
		this.includeChildren = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
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
		if (this.tweenTarget != null)
		{
			this.tweenTarget = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.tweenTarget) as GameObject);
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
			ISerializedNamedStateWriter arg_20_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_20_1 = this.tweenTarget;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_20_0.WriteUnityEngineObject(arg_20_1, &var_0_cp_0[var_0_cp_1] + 96);
		}
		SerializedNamedStateWriter.Instance.WriteInt32(this.tweenGroup, &var_0_cp_0[var_0_cp_1] + 1219);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.trigger, &var_0_cp_0[var_0_cp_1] + 415);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.playDirection, &var_0_cp_0[var_0_cp_1] + 1087);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.resetOnPlay, &var_0_cp_0[var_0_cp_1] + 1101);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.resetIfDisabled, &var_0_cp_0[var_0_cp_1] + 1230);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.ifDisabledOnPlay, &var_0_cp_0[var_0_cp_1] + 1128);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.disableWhenFinished, &var_0_cp_0[var_0_cp_1] + 1145);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.includeChildren, &var_0_cp_0[var_0_cp_1] + 423);
		SerializedNamedStateWriter.Instance.Align();
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
					EventDelegate arg_184_0 = (this.onFinished[i] != null) ? this.onFinished[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_184_0.Unity_NamedSerialize(depth + 1);
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
			ISerializedNamedStateReader arg_1B_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			this.tweenTarget = (arg_1B_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 96) as GameObject);
		}
		this.tweenGroup = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1219);
		this.trigger = (Trigger)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 415);
		this.playDirection = (Direction)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1087);
		this.resetOnPlay = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1101);
		SerializedNamedStateReader.Instance.Align();
		this.resetIfDisabled = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1230);
		SerializedNamedStateReader.Instance.Align();
		this.ifDisabledOnPlay = (EnableCondition)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1128);
		this.disableWhenFinished = (DisableCondition)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1145);
		this.includeChildren = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 423);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85);
			this.onFinished = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_144_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_144_0.Unity_NamedDeserialize(depth + 1);
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

	protected internal UIPlayTween(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPlayTween)instance).tweenTarget);
	}

	public static void $Set0(object instance, long value)
	{
		((UIPlayTween)instance).tweenTarget = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UIPlayTween)instance).resetOnPlay;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIPlayTween)instance).resetOnPlay = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UIPlayTween)instance).resetIfDisabled;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIPlayTween)instance).resetIfDisabled = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UIPlayTween)instance).includeChildren;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIPlayTween)instance).includeChildren = value;
	}

	public static long $Get4(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPlayTween)instance).eventReceiver);
	}

	public static void $Set4(object instance, long value)
	{
		((UIPlayTween)instance).eventReceiver = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnDoubleClick();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnDragOut();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnDragOver();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnFinished();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnHover(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnSelect(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).OnToggle();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).Play(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIPlayTween)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
