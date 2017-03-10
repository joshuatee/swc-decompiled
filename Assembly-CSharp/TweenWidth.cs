using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Tween/Tween Width"), RequireComponent(typeof(UIWidget))]
public class TweenWidth : UITweener, IUnitySerializable
{
	public int from;

	public int to;

	public bool updateTable;

	private UIWidget mWidget;

	private UITable mTable;

	public UIWidget cachedWidget
	{
		get
		{
			if (this.mWidget == null)
			{
				this.mWidget = base.GetComponent<UIWidget>();
			}
			return this.mWidget;
		}
	}

	[Obsolete("Use 'value' instead")]
	public int width
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

	public int value
	{
		get
		{
			return this.cachedWidget.width;
		}
		set
		{
			this.cachedWidget.width = value;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.RoundToInt((float)this.from * (1f - factor) + (float)this.to * factor);
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

	public static TweenWidth Begin(UIWidget widget, float duration, int width)
	{
		TweenWidth tweenWidth = UITweener.Begin<TweenWidth>(widget.gameObject, duration);
		tweenWidth.from = widget.width;
		tweenWidth.to = width;
		if (duration <= 0f)
		{
			tweenWidth.Sample(1f, true);
			tweenWidth.enabled = false;
		}
		return tweenWidth;
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

	public TweenWidth()
	{
		this.from = 100;
		this.to = 100;
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
		SerializedStateWriter.Instance.WriteInt32(this.from);
		SerializedStateWriter.Instance.WriteInt32(this.to);
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
		this.from = SerializedStateReader.Instance.ReadInt32();
		this.to = SerializedStateReader.Instance.ReadInt32();
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
		SerializedNamedStateWriter.Instance.WriteInt32(this.from, &var_0_cp_0[var_0_cp_1] + 2734);
		SerializedNamedStateWriter.Instance.WriteInt32(this.to, &var_0_cp_0[var_0_cp_1] + 2739);
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
		this.from = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2734);
		this.to = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2739);
		this.updateTable = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2742);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal TweenWidth(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((TweenWidth)instance).updateTable;
	}

	public static void $Set0(object instance, bool value)
	{
		((TweenWidth)instance).updateTable = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(TweenWidth.Begin((UIWidget)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(int*)(args + 2)));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).cachedWidget);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).value);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).width);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args, *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).value = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).width = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).SetCurrentValueToEnd();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).SetCurrentValueToStart();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).SetEndToCurrentValue();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).SetStartToCurrentValue();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((TweenWidth)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
