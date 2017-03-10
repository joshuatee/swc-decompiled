using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour, IUnitySerializable
{
	public GameObject target;

	public bool state;

	private void OnClick()
	{
		if (this.target != null)
		{
			NGUITools.SetActive(this.target, this.state);
		}
	}

	public UIButtonActivate()
	{
		this.state = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.target);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.state);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.state = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.target != null)
		{
			this.target = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.target) as GameObject);
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
		SerializedNamedStateWriter.Instance.WriteBoolean(this.state, &var_0_cp_0[var_0_cp_1] + 272);
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
			this.target = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 265) as GameObject);
		}
		this.state = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 272);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIButtonActivate(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonActivate)instance).target);
	}

	public static void $Set0(object instance, long value)
	{
		((UIButtonActivate)instance).target = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UIButtonActivate)instance).state;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIButtonActivate)instance).state = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIButtonActivate)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIButtonActivate)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIButtonActivate)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIButtonActivate)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIButtonActivate)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIButtonActivate)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
