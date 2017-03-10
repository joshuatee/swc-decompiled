using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Tween/Tween Transform")]
public class TweenTransform : UITweener, IUnitySerializable
{
	public Transform from;

	public Transform to;

	public bool parentWhenFinished;

	private Transform mTrans;

	private Vector3 mPos;

	private Quaternion mRot;

	private Vector3 mScale;

	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.to != null)
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
				this.mPos = this.mTrans.position;
				this.mRot = this.mTrans.rotation;
				this.mScale = this.mTrans.localScale;
			}
			if (this.from != null)
			{
				this.mTrans.position = this.from.position * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.from.localScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.from.rotation, this.to.rotation, factor);
			}
			else
			{
				this.mTrans.position = this.mPos * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.mScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.mRot, this.to.rotation, factor);
			}
			if (this.parentWhenFinished & isFinished)
			{
				this.mTrans.parent = this.to;
			}
		}
	}

	public static TweenTransform Begin(GameObject go, float duration, Transform to)
	{
		return TweenTransform.Begin(go, duration, null, to);
	}

	public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
	{
		TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration);
		tweenTransform.from = from;
		tweenTransform.to = to;
		if (duration <= 0f)
		{
			tweenTransform.Sample(1f, true);
			tweenTransform.enabled = false;
		}
		return tweenTransform;
	}

	public TweenTransform()
	{
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
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.from);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.to);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.parentWhenFinished);
		SerializedStateWriter.Instance.Align();
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
		if (depth <= 7)
		{
			this.from = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		if (depth <= 7)
		{
			this.to = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		this.parentWhenFinished = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
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
		if (this.from != null)
		{
			this.from = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.from) as Transform);
		}
		if (this.to != null)
		{
			this.to = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.to) as Transform);
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
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.from, &var_0_cp_0[var_0_cp_1] + 2734);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.to, &var_0_cp_0[var_0_cp_1] + 2739);
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.parentWhenFinished, &var_0_cp_0[var_0_cp_1] + 2769);
		SerializedNamedStateWriter.Instance.Align();
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
		if (depth <= 7)
		{
			this.from = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2734) as Transform);
		}
		if (depth <= 7)
		{
			this.to = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2739) as Transform);
		}
		this.parentWhenFinished = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2769);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal TweenTransform(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((TweenTransform)instance).from);
	}

	public static void $Set0(object instance, long value)
	{
		((TweenTransform)instance).from = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((TweenTransform)instance).to);
	}

	public static void $Set1(object instance, long value)
	{
		((TweenTransform)instance).to = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get2(object instance)
	{
		return ((TweenTransform)instance).parentWhenFinished;
	}

	public static void $Set2(object instance, bool value)
	{
		((TweenTransform)instance).parentWhenFinished = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(TweenTransform.Begin((GameObject)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), (Transform)GCHandledObjects.GCHandleToObject(args[2])));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(TweenTransform.Begin((GameObject)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), (Transform)GCHandledObjects.GCHandleToObject(args[2]), (Transform)GCHandledObjects.GCHandleToObject(args[3])));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((TweenTransform)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args, *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((TweenTransform)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((TweenTransform)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((TweenTransform)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((TweenTransform)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((TweenTransform)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
