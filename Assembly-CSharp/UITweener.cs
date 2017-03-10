using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

public abstract class UITweener : MonoBehaviour, IUnitySerializable
{
	public enum Method
	{
		Linear,
		EaseIn,
		EaseOut,
		EaseInOut,
		BounceIn,
		BounceOut
	}

	public enum Style
	{
		Once,
		Loop,
		PingPong
	}

	public static UITweener current;

	[HideInInspector]
	public UITweener.Method method;

	[HideInInspector]
	public UITweener.Style style;

	[HideInInspector]
	public AnimationCurve animationCurve;

	[HideInInspector]
	public bool ignoreTimeScale;

	[HideInInspector]
	public float delay;

	[HideInInspector]
	public float duration;

	[HideInInspector]
	public bool steeperCurves;

	[HideInInspector]
	public int tweenGroup;

	[HideInInspector]
	public List<EventDelegate> onFinished;

	[HideInInspector]
	public GameObject eventReceiver;

	[HideInInspector]
	public string callWhenFinished;

	private bool mStarted;

	private float mStartTime;

	private float mDuration;

	private float mAmountPerDelta;

	private float mFactor;

	private List<EventDelegate> mTemp;

	public float amountPerDelta
	{
		get
		{
			if (this.mDuration != this.duration)
			{
				this.mDuration = this.duration;
				this.mAmountPerDelta = Mathf.Abs((this.duration > 0f) ? (1f / this.duration) : 1000f) * Mathf.Sign(this.mAmountPerDelta);
			}
			return this.mAmountPerDelta;
		}
	}

	public float tweenFactor
	{
		get
		{
			return this.mFactor;
		}
		set
		{
			this.mFactor = Mathf.Clamp01(value);
		}
	}

	public Direction direction
	{
		get
		{
			if (this.amountPerDelta >= 0f)
			{
				return Direction.Forward;
			}
			return Direction.Reverse;
		}
	}

	private void Reset()
	{
		if (!this.mStarted)
		{
			this.SetStartToCurrentValue();
			this.SetEndToCurrentValue();
		}
	}

	protected virtual void Start()
	{
		this.Update();
	}

	private void Update()
	{
		float num = this.ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
		float num2 = this.ignoreTimeScale ? RealTime.time : Time.time;
		if (!this.mStarted)
		{
			this.mStarted = true;
			this.mStartTime = num2 + this.delay;
		}
		if (num2 < this.mStartTime)
		{
			return;
		}
		this.mFactor += this.amountPerDelta * num;
		if (this.style == UITweener.Style.Loop)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor -= Mathf.Floor(this.mFactor);
			}
		}
		else if (this.style == UITweener.Style.PingPong)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			else if (this.mFactor < 0f)
			{
				this.mFactor = -this.mFactor;
				this.mFactor -= Mathf.Floor(this.mFactor);
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
		}
		if (this.style == UITweener.Style.Once && (this.duration == 0f || this.mFactor > 1f || this.mFactor < 0f))
		{
			this.mFactor = Mathf.Clamp01(this.mFactor);
			this.Sample(this.mFactor, true);
			base.enabled = false;
			if (UITweener.current == null)
			{
				UITweener uITweener = UITweener.current;
				UITweener.current = this;
				if (this.onFinished != null)
				{
					this.mTemp = this.onFinished;
					this.onFinished = new List<EventDelegate>();
					EventDelegate.Execute(this.mTemp);
					for (int i = 0; i < this.mTemp.Count; i++)
					{
						EventDelegate eventDelegate = this.mTemp[i];
						if (eventDelegate != null && !eventDelegate.oneShot)
						{
							EventDelegate.Add(this.onFinished, eventDelegate, eventDelegate.oneShot);
						}
					}
					this.mTemp = null;
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				UITweener.current = uITweener;
				return;
			}
		}
		else
		{
			this.Sample(this.mFactor, false);
		}
	}

	public void SetOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	public void SetOnFinished(EventDelegate del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	public void AddOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	public void AddOnFinished(EventDelegate del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	public void RemoveOnFinished(EventDelegate del)
	{
		if (this.onFinished != null)
		{
			this.onFinished.Remove(del);
		}
		if (this.mTemp != null)
		{
			this.mTemp.Remove(del);
		}
	}

	private void OnDisable()
	{
		this.mStarted = false;
	}

	public void Sample(float factor, bool isFinished)
	{
		float num = Mathf.Clamp01(factor);
		if (this.method == UITweener.Method.EaseIn)
		{
			num = 1f - Mathf.Sin(1.57079637f * (1f - num));
			if (this.steeperCurves)
			{
				num *= num;
			}
		}
		else if (this.method == UITweener.Method.EaseOut)
		{
			num = Mathf.Sin(1.57079637f * num);
			if (this.steeperCurves)
			{
				num = 1f - num;
				num = 1f - num * num;
			}
		}
		else if (this.method == UITweener.Method.EaseInOut)
		{
			num -= Mathf.Sin(num * 6.28318548f) / 6.28318548f;
			if (this.steeperCurves)
			{
				num = num * 2f - 1f;
				float num2 = Mathf.Sign(num);
				num = 1f - Mathf.Abs(num);
				num = 1f - num * num;
				num = num2 * num * 0.5f + 0.5f;
			}
		}
		else if (this.method == UITweener.Method.BounceIn)
		{
			num = this.BounceLogic(num);
		}
		else if (this.method == UITweener.Method.BounceOut)
		{
			num = 1f - this.BounceLogic(1f - num);
		}
		this.OnUpdate((this.animationCurve != null) ? this.animationCurve.Evaluate(num) : num, isFinished);
	}

	private float BounceLogic(float val)
	{
		if (val < 0.363636f)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f)
		{
			val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
		}
		else if (val < 0.90909f)
		{
			val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
		}
		else
		{
			val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;
		}
		return val;
	}

	[Obsolete("Use PlayForward() instead")]
	public void Play()
	{
		this.Play(true);
	}

	public void PlayForward()
	{
		this.Play(true);
	}

	public void PlayReverse()
	{
		this.Play(false);
	}

	public void Play(bool forward)
	{
		this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		if (!forward)
		{
			this.mAmountPerDelta = -this.mAmountPerDelta;
		}
		base.enabled = true;
		this.Update();
	}

	public void ResetToBeginning()
	{
		this.mStarted = false;
		this.mFactor = ((this.amountPerDelta < 0f) ? 1f : 0f);
		this.Sample(this.mFactor, false);
	}

	public void Toggle()
	{
		if (this.mFactor > 0f)
		{
			this.mAmountPerDelta = -this.amountPerDelta;
		}
		else
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		}
		base.enabled = true;
	}

	protected abstract void OnUpdate(float factor, bool isFinished);

	public static T Begin<T>(GameObject go, float duration) where T : UITweener
	{
		T t = go.GetComponent<T>();
		if (t != null && t.tweenGroup != 0)
		{
			t = default(T);
			T[] components = go.GetComponents<T>();
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				t = components[i];
				if (t != null && t.tweenGroup == 0)
				{
					break;
				}
				t = default(T);
				i++;
			}
		}
		if (t == null)
		{
			t = go.AddComponent<T>();
			if (t == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Unable to add ",
					typeof(T),
					" to ",
					NGUITools.GetHierarchy(go)
				}), go);
				return default(T);
			}
		}
		t.mStarted = false;
		t.duration = duration;
		t.mFactor = 0f;
		t.mAmountPerDelta = Mathf.Abs(t.amountPerDelta);
		t.style = UITweener.Style.Once;
		t.animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});
		t.eventReceiver = null;
		t.callWhenFinished = null;
		t.enabled = true;
		return t;
	}

	public virtual void SetStartToCurrentValue()
	{
	}

	public virtual void SetEndToCurrentValue()
	{
	}

	public UITweener()
	{
		this.animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});
		this.ignoreTimeScale = true;
		this.duration = 1f;
		this.onFinished = new List<EventDelegate>();
		this.mAmountPerDelta = 1000f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32((int)this.method);
		SerializedStateWriter.Instance.WriteInt32((int)this.style);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteAnimationCurve(this.animationCurve);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.ignoreTimeScale);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.delay);
		SerializedStateWriter.Instance.WriteSingle(this.duration);
		SerializedStateWriter.Instance.WriteBoolean(this.steeperCurves);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32(this.tweenGroup);
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
		this.method = (UITweener.Method)SerializedStateReader.Instance.ReadInt32();
		this.style = (UITweener.Style)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.animationCurve = (SerializedStateReader.Instance.ReadAnimationCurve() as AnimationCurve);
		}
		this.ignoreTimeScale = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.delay = SerializedStateReader.Instance.ReadSingle();
		this.duration = SerializedStateReader.Instance.ReadSingle();
		this.steeperCurves = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.tweenGroup = SerializedStateReader.Instance.ReadInt32();
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
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = (int)this.method;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 2686);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.style, &var_0_cp_0[var_0_cp_1] + 2693);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteAnimationCurve(this.animationCurve, &var_0_cp_0[var_0_cp_1] + 2699);
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.ignoreTimeScale, &var_0_cp_0[var_0_cp_1] + 2653);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.delay, &var_0_cp_0[var_0_cp_1] + 2714);
		SerializedNamedStateWriter.Instance.WriteSingle(this.duration, &var_0_cp_0[var_0_cp_1] + 136);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.steeperCurves, &var_0_cp_0[var_0_cp_1] + 2720);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32(this.tweenGroup, &var_0_cp_0[var_0_cp_1] + 1219);
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
					EventDelegate arg_165_0 = (this.onFinished[i] != null) ? this.onFinished[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_165_0.Unity_NamedSerialize(depth + 1);
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
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.method = (UITweener.Method)arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2686);
		this.style = (UITweener.Style)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2693);
		if (depth <= 7)
		{
			this.animationCurve = (SerializedNamedStateReader.Instance.ReadAnimationCurve(&var_0_cp_0[var_0_cp_1] + 2699) as AnimationCurve);
		}
		this.ignoreTimeScale = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2653);
		SerializedNamedStateReader.Instance.Align();
		this.delay = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2714);
		this.duration = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 136);
		this.steeperCurves = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2720);
		SerializedNamedStateReader.Instance.Align();
		this.tweenGroup = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1219);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85);
			this.onFinished = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_125_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_125_0.Unity_NamedDeserialize(depth + 1);
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

	protected internal UITweener(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UITweener)instance).ignoreTimeScale;
	}

	public static void $Set0(object instance, bool value)
	{
		((UITweener)instance).ignoreTimeScale = value;
	}

	public static float $Get1(object instance)
	{
		return ((UITweener)instance).delay;
	}

	public static void $Set1(object instance, float value)
	{
		((UITweener)instance).delay = value;
	}

	public static float $Get2(object instance)
	{
		return ((UITweener)instance).duration;
	}

	public static void $Set2(object instance, float value)
	{
		((UITweener)instance).duration = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UITweener)instance).steeperCurves;
	}

	public static void $Set3(object instance, bool value)
	{
		((UITweener)instance).steeperCurves = value;
	}

	public static long $Get4(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITweener)instance).eventReceiver);
	}

	public static void $Set4(object instance, long value)
	{
		((UITweener)instance).eventReceiver = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).AddOnFinished((EventDelegate)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).AddOnFinished((EventDelegate.Callback)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITweener)GCHandledObjects.GCHandleToObject(instance)).BounceLogic(*(float*)args));
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITweener)GCHandledObjects.GCHandleToObject(instance)).amountPerDelta);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITweener)GCHandledObjects.GCHandleToObject(instance)).direction);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITweener)GCHandledObjects.GCHandleToObject(instance)).tweenFactor);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args, *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Play();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Play(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).PlayForward();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).PlayReverse();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).RemoveOnFinished((EventDelegate)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Reset();
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).ResetToBeginning();
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Sample(*(float*)args, *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).tweenFactor = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).SetEndToCurrentValue();
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).SetOnFinished((EventDelegate)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).SetOnFinished((EventDelegate.Callback)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).SetStartToCurrentValue();
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Toggle();
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((UITweener)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
