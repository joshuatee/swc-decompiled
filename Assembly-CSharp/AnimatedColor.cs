using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[ExecuteInEditMode, RequireComponent(typeof(UIWidget))]
public class AnimatedColor : MonoBehaviour, IUnitySerializable
{
	public Color color;

	private UIWidget mWidget;

	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.LateUpdate();
	}

	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.color = this.color;
		}
	}

	public AnimatedColor()
	{
		this.color = Color.white;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			this.color.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.color.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&$FieldNamesStorage.$RuntimeNames[0] + 2636);
			this.color.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&$FieldNamesStorage.$RuntimeNames[0] + 2636);
			this.color.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal AnimatedColor(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance, int index)
	{
		AnimatedColor expr_06_cp_0 = (AnimatedColor)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.color.r;
		case 1:
			return expr_06_cp_0.color.g;
		case 2:
			return expr_06_cp_0.color.b;
		case 3:
			return expr_06_cp_0.color.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set0(object instance, float value, int index)
	{
		AnimatedColor expr_06_cp_0 = (AnimatedColor)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.color.r = value;
			return;
		case 1:
			expr_06_cp_0.color.g = value;
			return;
		case 2:
			expr_06_cp_0.color.b = value;
			return;
		case 3:
			expr_06_cp_0.color.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((AnimatedColor)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((AnimatedColor)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((AnimatedColor)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((AnimatedColor)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((AnimatedColor)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((AnimatedColor)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((AnimatedColor)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
