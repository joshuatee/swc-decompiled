using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

public class UIShowControlScheme : MonoBehaviour, IUnitySerializable
{
	public GameObject target;

	public bool mouse;

	public bool touch;

	public bool controller;

	private void OnEnable()
	{
		UICamera.onSchemeChange = (UICamera.OnSchemeChange)Delegate.Combine(UICamera.onSchemeChange, new UICamera.OnSchemeChange(this.OnScheme));
		this.OnScheme();
	}

	private void OnDisable()
	{
		UICamera.onSchemeChange = (UICamera.OnSchemeChange)Delegate.Remove(UICamera.onSchemeChange, new UICamera.OnSchemeChange(this.OnScheme));
	}

	private void OnScheme()
	{
		if (this.target != null)
		{
			UICamera.ControlScheme currentScheme = UICamera.currentScheme;
			if (currentScheme == UICamera.ControlScheme.Mouse)
			{
				this.target.SetActive(this.mouse);
				return;
			}
			if (currentScheme == UICamera.ControlScheme.Touch)
			{
				this.target.SetActive(this.touch);
				return;
			}
			if (currentScheme == UICamera.ControlScheme.Controller)
			{
				this.target.SetActive(this.controller);
			}
		}
	}

	public UIShowControlScheme()
	{
		this.controller = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.target);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.mouse);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.touch);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.controller);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.mouse = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.touch = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.controller = SerializedStateReader.Instance.ReadBoolean();
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
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mouse, &var_0_cp_0[var_0_cp_1] + 1856);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.touch, &var_0_cp_0[var_0_cp_1] + 1862);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.controller, &var_0_cp_0[var_0_cp_1] + 1868);
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
		this.mouse = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1856);
		SerializedNamedStateReader.Instance.Align();
		this.touch = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1862);
		SerializedNamedStateReader.Instance.Align();
		this.controller = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1868);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIShowControlScheme(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIShowControlScheme)instance).target);
	}

	public static void $Set0(object instance, long value)
	{
		((UIShowControlScheme)instance).target = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UIShowControlScheme)instance).mouse;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIShowControlScheme)instance).mouse = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UIShowControlScheme)instance).touch;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIShowControlScheme)instance).touch = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UIShowControlScheme)instance).controller;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIShowControlScheme)instance).controller = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIShowControlScheme)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIShowControlScheme)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIShowControlScheme)GCHandledObjects.GCHandleToObject(instance)).OnScheme();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIShowControlScheme)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIShowControlScheme)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIShowControlScheme)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIShowControlScheme)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIShowControlScheme)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
