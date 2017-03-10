using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Tween/Tween Scale")]
public class TweenScale : UITweener, IUnitySerializable
{
	public Vector3 from;

	public Vector3 to;

	public bool updateTable;

	private Transform mTrans;

	private UITable mTable;

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public Vector3 value
	{
		get
		{
			return this.cachedTransform.localScale;
		}
		set
		{
			this.cachedTransform.localScale = value;
		}
	}

	[Obsolete("Use 'value' instead")]
	public Vector3 scale
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

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		if (this.updateTable)
		{
			if (this.mTable == null)
			{
				this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
				if (this.mTable == null)
				{
					this.updateTable = false;
					return;
				}
			}
			this.mTable.repositionNow = true;
		}
	}

	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.value;
		tweenScale.to = scale;
		if (duration <= 0f)
		{
			tweenScale.Sample(1f, true);
			tweenScale.enabled = false;
		}
		return tweenScale;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	public TweenScale()
	{
		this.from = Vector3.one;
		this.to = Vector3.one;
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
		if (depth <= 7)
		{
			this.from.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.to.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.updateTable);
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
			this.from.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.to.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.updateTable = SerializedStateReader.Instance.ReadBoolean();
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
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2734);
			this.from.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2739);
			this.to.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.updateTable, &var_0_cp_0[var_0_cp_1] + 2742);
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
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2734);
			this.from.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2739);
			this.to.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.updateTable = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2742);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal TweenScale(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance, int index)
	{
		TweenScale expr_06_cp_0 = (TweenScale)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.from.x;
		case 1:
			return expr_06_cp_0.from.y;
		case 2:
			return expr_06_cp_0.from.z;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set0(object instance, float value, int index)
	{
		TweenScale expr_06_cp_0 = (TweenScale)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.from.x = value;
			return;
		case 1:
			expr_06_cp_0.from.y = value;
			return;
		case 2:
			expr_06_cp_0.from.z = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get1(object instance, int index)
	{
		TweenScale expr_06_cp_0 = (TweenScale)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.to.x;
		case 1:
			return expr_06_cp_0.to.y;
		case 2:
			return expr_06_cp_0.to.z;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set1(object instance, float value, int index)
	{
		TweenScale expr_06_cp_0 = (TweenScale)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.to.x = value;
			return;
		case 1:
			expr_06_cp_0.to.y = value;
			return;
		case 2:
			expr_06_cp_0.to.z = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static bool $Get2(object instance)
	{
		return ((TweenScale)instance).updateTable;
	}

	public static void $Set2(object instance, bool value)
	{
		((TweenScale)instance).updateTable = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(TweenScale.Begin((GameObject)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(*(IntPtr*)(args + 2))));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((TweenScale)GCHandledObjects.GCHandleToObject(instance)).cachedTransform);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((TweenScale)GCHandledObjects.GCHandleToObject(instance)).scale);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((TweenScale)GCHandledObjects.GCHandleToObject(instance)).value);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args, *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).scale = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).value = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).SetCurrentValueToEnd();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).SetCurrentValueToStart();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).SetEndToCurrentValue();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).SetStartToCurrentValue();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((TweenScale)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
