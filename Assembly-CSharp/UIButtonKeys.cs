using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Button Keys (Legacy)"), ExecuteInEditMode]
public class UIButtonKeys : UIKeyNavigation, IUnitySerializable
{
	public UIButtonKeys selectOnClick;

	public UIButtonKeys selectOnUp;

	public UIButtonKeys selectOnDown;

	public UIButtonKeys selectOnLeft;

	public UIButtonKeys selectOnRight;

	protected override void OnEnable()
	{
		this.Upgrade();
		base.OnEnable();
	}

	public void Upgrade()
	{
		if (this.onClick == null && this.selectOnClick != null)
		{
			this.onClick = this.selectOnClick.gameObject;
			this.selectOnClick = null;
			NGUITools.SetDirty(this);
		}
		if (this.onLeft == null && this.selectOnLeft != null)
		{
			this.onLeft = this.selectOnLeft.gameObject;
			this.selectOnLeft = null;
			NGUITools.SetDirty(this);
		}
		if (this.onRight == null && this.selectOnRight != null)
		{
			this.onRight = this.selectOnRight.gameObject;
			this.selectOnRight = null;
			NGUITools.SetDirty(this);
		}
		if (this.onUp == null && this.selectOnUp != null)
		{
			this.onUp = this.selectOnUp.gameObject;
			this.selectOnUp = null;
			NGUITools.SetDirty(this);
		}
		if (this.onDown == null && this.selectOnDown != null)
		{
			this.onDown = this.selectOnDown.gameObject;
			this.selectOnDown = null;
			NGUITools.SetDirty(this);
		}
	}

	public UIButtonKeys()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32((int)this.constraint);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onUp);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onDown);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onLeft);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onRight);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onClick);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onTab);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.startsSelected);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.selectOnClick);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.selectOnUp);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.selectOnDown);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.selectOnLeft);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.selectOnRight);
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		this.constraint = (UIKeyNavigation.Constraint)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.onUp = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onDown = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onLeft = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onRight = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onClick = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onTab = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.startsSelected = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.selectOnClick = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIButtonKeys);
		}
		if (depth <= 7)
		{
			this.selectOnUp = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIButtonKeys);
		}
		if (depth <= 7)
		{
			this.selectOnDown = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIButtonKeys);
		}
		if (depth <= 7)
		{
			this.selectOnLeft = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIButtonKeys);
		}
		if (depth <= 7)
		{
			this.selectOnRight = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIButtonKeys);
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.onUp != null)
		{
			this.onUp = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onUp) as GameObject);
		}
		if (this.onDown != null)
		{
			this.onDown = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onDown) as GameObject);
		}
		if (this.onLeft != null)
		{
			this.onLeft = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onLeft) as GameObject);
		}
		if (this.onRight != null)
		{
			this.onRight = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onRight) as GameObject);
		}
		if (this.onClick != null)
		{
			this.onClick = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onClick) as GameObject);
		}
		if (this.onTab != null)
		{
			this.onTab = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onTab) as GameObject);
		}
		if (this.selectOnClick != null)
		{
			this.selectOnClick = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.selectOnClick) as UIButtonKeys);
		}
		if (this.selectOnUp != null)
		{
			this.selectOnUp = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.selectOnUp) as UIButtonKeys);
		}
		if (this.selectOnDown != null)
		{
			this.selectOnDown = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.selectOnDown) as UIButtonKeys);
		}
		if (this.selectOnLeft != null)
		{
			this.selectOnLeft = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.selectOnLeft) as UIButtonKeys);
		}
		if (this.selectOnRight != null)
		{
			this.selectOnRight = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.selectOnRight) as UIButtonKeys);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = (int)this.constraint;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 278);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onUp, &var_0_cp_0[var_0_cp_1] + 289);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onDown, &var_0_cp_0[var_0_cp_1] + 294);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onLeft, &var_0_cp_0[var_0_cp_1] + 301);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onRight, &var_0_cp_0[var_0_cp_1] + 308);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onClick, &var_0_cp_0[var_0_cp_1] + 257);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onTab, &var_0_cp_0[var_0_cp_1] + 316);
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.startsSelected, &var_0_cp_0[var_0_cp_1] + 322);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.selectOnClick, &var_0_cp_0[var_0_cp_1] + 337);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.selectOnUp, &var_0_cp_0[var_0_cp_1] + 351);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.selectOnDown, &var_0_cp_0[var_0_cp_1] + 362);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.selectOnLeft, &var_0_cp_0[var_0_cp_1] + 375);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.selectOnRight, &var_0_cp_0[var_0_cp_1] + 388);
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.constraint = (UIKeyNavigation.Constraint)arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 278);
		if (depth <= 7)
		{
			this.onUp = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 289) as GameObject);
		}
		if (depth <= 7)
		{
			this.onDown = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 294) as GameObject);
		}
		if (depth <= 7)
		{
			this.onLeft = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 301) as GameObject);
		}
		if (depth <= 7)
		{
			this.onRight = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 308) as GameObject);
		}
		if (depth <= 7)
		{
			this.onClick = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 257) as GameObject);
		}
		if (depth <= 7)
		{
			this.onTab = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 316) as GameObject);
		}
		this.startsSelected = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 322);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.selectOnClick = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 337) as UIButtonKeys);
		}
		if (depth <= 7)
		{
			this.selectOnUp = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 351) as UIButtonKeys);
		}
		if (depth <= 7)
		{
			this.selectOnDown = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 362) as UIButtonKeys);
		}
		if (depth <= 7)
		{
			this.selectOnLeft = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 375) as UIButtonKeys);
		}
		if (depth <= 7)
		{
			this.selectOnRight = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 388) as UIButtonKeys);
		}
	}

	protected internal UIButtonKeys(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonKeys)instance).selectOnClick);
	}

	public static void $Set0(object instance, long value)
	{
		((UIButtonKeys)instance).selectOnClick = (UIButtonKeys)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonKeys)instance).selectOnUp);
	}

	public static void $Set1(object instance, long value)
	{
		((UIButtonKeys)instance).selectOnUp = (UIButtonKeys)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonKeys)instance).selectOnDown);
	}

	public static void $Set2(object instance, long value)
	{
		((UIButtonKeys)instance).selectOnDown = (UIButtonKeys)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get3(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonKeys)instance).selectOnLeft);
	}

	public static void $Set3(object instance, long value)
	{
		((UIButtonKeys)instance).selectOnLeft = (UIButtonKeys)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get4(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButtonKeys)instance).selectOnRight);
	}

	public static void $Set4(object instance, long value)
	{
		((UIButtonKeys)instance).selectOnRight = (UIButtonKeys)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIButtonKeys)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIButtonKeys)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIButtonKeys)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIButtonKeys)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIButtonKeys)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIButtonKeys)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIButtonKeys)GCHandledObjects.GCHandleToObject(instance)).Upgrade();
		return -1L;
	}
}
