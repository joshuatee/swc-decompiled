using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Toggle"), ExecuteInEditMode]
public class UIToggle : UIWidgetContainer, IUnitySerializable
{
	public delegate bool Validate(bool choice);

	public static BetterList<UIToggle> list = new BetterList<UIToggle>();

	public static UIToggle current;

	public int group;

	public UIWidget activeSprite;

	public Animation activeAnimation;

	public Animator animator;

	public UITweener tween;

	public bool startsActive;

	public bool instantTween;

	public bool optionCanBeNone;

	public List<EventDelegate> onChange;

	public UIToggle.Validate validator;

	[HideInInspector, SerializeField]
	protected internal UISprite checkSprite;

	[HideInInspector, SerializeField]
	protected internal Animation checkAnimation;

	[HideInInspector, SerializeField]
	protected internal GameObject eventReceiver;

	[HideInInspector, SerializeField]
	protected internal string functionName;

	[HideInInspector, SerializeField]
	protected internal bool startsChecked;

	private bool mIsActive;

	private bool mStarted;

	public bool value
	{
		get
		{
			if (!this.mStarted)
			{
				return this.startsActive;
			}
			return this.mIsActive;
		}
		set
		{
			if (!this.mStarted)
			{
				this.startsActive = value;
				return;
			}
			if ((this.group == 0 | value) || this.optionCanBeNone || !this.mStarted)
			{
				this.Set(value);
			}
		}
	}

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
	public bool isChecked
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	public static UIToggle GetActiveToggle(int group)
	{
		for (int i = 0; i < UIToggle.list.size; i++)
		{
			UIToggle uIToggle = UIToggle.list[i];
			if (uIToggle != null && uIToggle.group == group && uIToggle.mIsActive)
			{
				return uIToggle;
			}
		}
		return null;
	}

	private void OnEnable()
	{
		UIToggle.list.Add(this);
	}

	private void OnDisable()
	{
		UIToggle.list.Remove(this);
	}

	private void Start()
	{
		if (this.startsChecked)
		{
			this.startsChecked = false;
			this.startsActive = true;
		}
		if (!Application.isPlaying)
		{
			if (this.checkSprite != null && this.activeSprite == null)
			{
				this.activeSprite = this.checkSprite;
				this.checkSprite = null;
			}
			if (this.checkAnimation != null && this.activeAnimation == null)
			{
				this.activeAnimation = this.checkAnimation;
				this.checkAnimation = null;
			}
			if (Application.isPlaying && this.activeSprite != null)
			{
				this.activeSprite.alpha = (this.startsActive ? 1f : 0f);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				this.eventReceiver = null;
				this.functionName = null;
				return;
			}
		}
		else
		{
			this.mIsActive = !this.startsActive;
			this.mStarted = true;
			bool flag = this.instantTween;
			this.instantTween = true;
			this.Set(this.startsActive);
			this.instantTween = flag;
		}
	}

	private void OnClick()
	{
		if (base.enabled && this.isColliderEnabled && UICamera.currentTouchID != -2)
		{
			this.value = !this.value;
		}
	}

	public void Set(bool state)
	{
		if (this.validator != null && !this.validator(state))
		{
			return;
		}
		if (!this.mStarted)
		{
			this.mIsActive = state;
			this.startsActive = state;
			if (this.activeSprite != null)
			{
				this.activeSprite.alpha = (state ? 1f : 0f);
				return;
			}
		}
		else if (this.mIsActive != state)
		{
			if (this.group != 0 & state)
			{
				int i = 0;
				int size = UIToggle.list.size;
				while (i < size)
				{
					UIToggle uIToggle = UIToggle.list[i];
					if (uIToggle != this && uIToggle.group == this.group)
					{
						uIToggle.Set(false);
					}
					if (UIToggle.list.size != size)
					{
						size = UIToggle.list.size;
						i = 0;
					}
					else
					{
						i++;
					}
				}
			}
			this.mIsActive = state;
			if (this.activeSprite != null)
			{
				if (this.instantTween || !NGUITools.GetActive(this))
				{
					this.activeSprite.alpha = (this.mIsActive ? 1f : 0f);
				}
				else
				{
					TweenAlpha.Begin(this.activeSprite.gameObject, 0.15f, this.mIsActive ? 1f : 0f);
				}
			}
			if (UIToggle.current == null)
			{
				UIToggle uIToggle2 = UIToggle.current;
				UIToggle.current = this;
				if (EventDelegate.IsValid(this.onChange))
				{
					EventDelegate.Execute(this.onChange);
				}
				else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
				{
					this.eventReceiver.SendMessage(this.functionName, this.mIsActive, SendMessageOptions.DontRequireReceiver);
				}
				UIToggle.current = uIToggle2;
			}
			if (this.animator != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.animator, null, state ? Direction.Forward : Direction.Reverse, EnableCondition.IgnoreDisabledState, DisableCondition.DoNotDisable);
				if (activeAnimation != null && (this.instantTween || !NGUITools.GetActive(this)))
				{
					activeAnimation.Finish();
					return;
				}
			}
			else if (this.activeAnimation != null)
			{
				ActiveAnimation activeAnimation2 = ActiveAnimation.Play(this.activeAnimation, null, state ? Direction.Forward : Direction.Reverse, EnableCondition.IgnoreDisabledState, DisableCondition.DoNotDisable);
				if (activeAnimation2 != null && (this.instantTween || !NGUITools.GetActive(this)))
				{
					activeAnimation2.Finish();
					return;
				}
			}
			else if (this.tween != null)
			{
				bool active = NGUITools.GetActive(this);
				if (this.tween.tweenGroup != 0)
				{
					UITweener[] componentsInChildren = this.tween.GetComponentsInChildren<UITweener>();
					int j = 0;
					int num = componentsInChildren.Length;
					while (j < num)
					{
						UITweener uITweener = componentsInChildren[j];
						if (uITweener.tweenGroup == this.tween.tweenGroup)
						{
							uITweener.Play(state);
							if (this.instantTween || !active)
							{
								uITweener.tweenFactor = (state ? 1f : 0f);
							}
						}
						j++;
					}
					return;
				}
				this.tween.Play(state);
				if (this.instantTween || !active)
				{
					this.tween.tweenFactor = (state ? 1f : 0f);
				}
			}
		}
	}

	public UIToggle()
	{
		this.onChange = new List<EventDelegate>();
		this.functionName = "OnActivate";
		this.mIsActive = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32(this.group);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.activeSprite);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.activeAnimation);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.animator);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.tween);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.startsActive);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.instantTween);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.optionCanBeNone);
		SerializedStateWriter.Instance.Align();
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
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.checkSprite);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.checkAnimation);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver);
		}
		SerializedStateWriter.Instance.WriteString(this.functionName);
		SerializedStateWriter.Instance.WriteBoolean(this.startsChecked);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		this.group = SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.activeSprite = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIWidget);
		}
		if (depth <= 7)
		{
			this.activeAnimation = (SerializedStateReader.Instance.ReadUnityEngineObject() as Animation);
		}
		if (depth <= 7)
		{
			this.animator = (SerializedStateReader.Instance.ReadUnityEngineObject() as Animator);
		}
		if (depth <= 7)
		{
			this.tween = (SerializedStateReader.Instance.ReadUnityEngineObject() as UITweener);
		}
		this.startsActive = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.instantTween = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.optionCanBeNone = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
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
			this.checkSprite = (SerializedStateReader.Instance.ReadUnityEngineObject() as UISprite);
		}
		if (depth <= 7)
		{
			this.checkAnimation = (SerializedStateReader.Instance.ReadUnityEngineObject() as Animation);
		}
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.functionName = (SerializedStateReader.Instance.ReadString() as string);
		this.startsChecked = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.activeSprite != null)
		{
			this.activeSprite = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.activeSprite) as UIWidget);
		}
		if (this.activeAnimation != null)
		{
			this.activeAnimation = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.activeAnimation) as Animation);
		}
		if (this.animator != null)
		{
			this.animator = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.animator) as Animator);
		}
		if (this.tween != null)
		{
			this.tween = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.tween) as UITweener);
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
		if (this.checkSprite != null)
		{
			this.checkSprite = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.checkSprite) as UISprite);
		}
		if (this.checkAnimation != null)
		{
			this.checkAnimation = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.checkAnimation) as Animation);
		}
		if (this.eventReceiver != null)
		{
			this.eventReceiver = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.eventReceiver) as GameObject);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = this.group;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 1901);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.activeSprite, &var_0_cp_0[var_0_cp_1] + 1907);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.activeAnimation, &var_0_cp_0[var_0_cp_1] + 1920);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.animator, &var_0_cp_0[var_0_cp_1] + 1069);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.tween, &var_0_cp_0[var_0_cp_1] + 1936);
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.startsActive, &var_0_cp_0[var_0_cp_1] + 1942);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.instantTween, &var_0_cp_0[var_0_cp_1] + 1955);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.optionCanBeNone, &var_0_cp_0[var_0_cp_1] + 1968);
		SerializedNamedStateWriter.Instance.Align();
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
					EventDelegate arg_184_0 = (this.onChange[i] != null) ? this.onChange[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_184_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.checkSprite, &var_0_cp_0[var_0_cp_1] + 1984);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.checkAnimation, &var_0_cp_0[var_0_cp_1] + 1996);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver, &var_0_cp_0[var_0_cp_1] + 1165);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.functionName, &var_0_cp_0[var_0_cp_1] + 402);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.startsChecked, &var_0_cp_0[var_0_cp_1] + 2011);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.group = arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1901);
		if (depth <= 7)
		{
			this.activeSprite = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1907) as UIWidget);
		}
		if (depth <= 7)
		{
			this.activeAnimation = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1920) as Animation);
		}
		if (depth <= 7)
		{
			this.animator = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1069) as Animator);
		}
		if (depth <= 7)
		{
			this.tween = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1936) as UITweener);
		}
		this.startsActive = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1942);
		SerializedNamedStateReader.Instance.Align();
		this.instantTween = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1955);
		SerializedNamedStateReader.Instance.Align();
		this.optionCanBeNone = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1968);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446);
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_150_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_150_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onChange.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			this.checkSprite = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1984) as UISprite);
		}
		if (depth <= 7)
		{
			this.checkAnimation = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1996) as Animation);
		}
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1165) as GameObject);
		}
		this.functionName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 402) as string);
		this.startsChecked = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2011);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIToggle(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)instance).activeSprite);
	}

	public static void $Set0(object instance, long value)
	{
		((UIToggle)instance).activeSprite = (UIWidget)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)instance).activeAnimation);
	}

	public static void $Set1(object instance, long value)
	{
		((UIToggle)instance).activeAnimation = (Animation)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)instance).animator);
	}

	public static void $Set2(object instance, long value)
	{
		((UIToggle)instance).animator = (Animator)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get3(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)instance).tween);
	}

	public static void $Set3(object instance, long value)
	{
		((UIToggle)instance).tween = (UITweener)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get4(object instance)
	{
		return ((UIToggle)instance).startsActive;
	}

	public static void $Set4(object instance, bool value)
	{
		((UIToggle)instance).startsActive = value;
	}

	public static bool $Get5(object instance)
	{
		return ((UIToggle)instance).instantTween;
	}

	public static void $Set5(object instance, bool value)
	{
		((UIToggle)instance).instantTween = value;
	}

	public static bool $Get6(object instance)
	{
		return ((UIToggle)instance).optionCanBeNone;
	}

	public static void $Set6(object instance, bool value)
	{
		((UIToggle)instance).optionCanBeNone = value;
	}

	public static long $Get7(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)instance).checkSprite);
	}

	public static void $Set7(object instance, long value)
	{
		((UIToggle)instance).checkSprite = (UISprite)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get8(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)instance).checkAnimation);
	}

	public static void $Set8(object instance, long value)
	{
		((UIToggle)instance).checkAnimation = (Animation)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get9(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)instance).eventReceiver);
	}

	public static void $Set9(object instance, long value)
	{
		((UIToggle)instance).eventReceiver = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get10(object instance)
	{
		return ((UIToggle)instance).startsChecked;
	}

	public static void $Set10(object instance, bool value)
	{
		((UIToggle)instance).startsChecked = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)GCHandledObjects.GCHandleToObject(instance)).isChecked);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)GCHandledObjects.GCHandleToObject(instance)).isColliderEnabled);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggle)GCHandledObjects.GCHandleToObject(instance)).value);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIToggle.GetActiveToggle(*(int*)args));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).Set(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).isChecked = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).value = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIToggle)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
