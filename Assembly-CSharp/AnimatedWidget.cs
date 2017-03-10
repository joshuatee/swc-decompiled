using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[ExecuteInEditMode]
public class AnimatedWidget : MonoBehaviour, IUnitySerializable
{
	public float width;

	public float height;

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
			this.mWidget.width = Mathf.RoundToInt(this.width);
			this.mWidget.height = Mathf.RoundToInt(this.height);
		}
	}

	public AnimatedWidget()
	{
		this.width = 1f;
		this.height = 1f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteSingle(this.width);
		SerializedStateWriter.Instance.WriteSingle(this.height);
	}

	public override void Unity_Deserialize(int depth)
	{
		this.width = SerializedStateReader.Instance.ReadSingle();
		this.height = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		float arg_1F_1 = this.width;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteSingle(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 2142);
		SerializedNamedStateWriter.Instance.WriteSingle(this.height, &var_0_cp_0[var_0_cp_1] + 2148);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.width = arg_1A_0.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2142);
		this.height = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2148);
	}

	protected internal AnimatedWidget(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance)
	{
		return ((AnimatedWidget)instance).width;
	}

	public static void $Set0(object instance, float value)
	{
		((AnimatedWidget)instance).width = value;
	}

	public static float $Get1(object instance)
	{
		return ((AnimatedWidget)instance).height;
	}

	public static void $Set1(object instance, float value)
	{
		((AnimatedWidget)instance).height = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((AnimatedWidget)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((AnimatedWidget)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((AnimatedWidget)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((AnimatedWidget)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((AnimatedWidget)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((AnimatedWidget)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((AnimatedWidget)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
