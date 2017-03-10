using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Internal/Property Binding"), ExecuteInEditMode]
public class PropertyBinding : MonoBehaviour, IUnitySerializable
{
	public enum UpdateCondition
	{
		OnStart,
		OnUpdate,
		OnLateUpdate,
		OnFixedUpdate
	}

	public enum Direction
	{
		SourceUpdatesTarget,
		TargetUpdatesSource,
		BiDirectional
	}

	public PropertyReference source;

	public PropertyReference target;

	public PropertyBinding.Direction direction;

	public PropertyBinding.UpdateCondition update;

	public bool editMode;

	private object mLastValue;

	private void Start()
	{
		this.UpdateTarget();
		if (this.update == PropertyBinding.UpdateCondition.OnStart)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnUpdate)
		{
			this.UpdateTarget();
		}
	}

	private void LateUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnLateUpdate)
		{
			this.UpdateTarget();
		}
	}

	private void FixedUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnFixedUpdate)
		{
			this.UpdateTarget();
		}
	}

	private void OnValidate()
	{
		if (this.source != null)
		{
			this.source.Reset();
		}
		if (this.target != null)
		{
			this.target.Reset();
		}
	}

	[ContextMenu("Update Now")]
	public void UpdateTarget()
	{
		if (this.source != null && this.target != null && this.source.isValid && this.target.isValid)
		{
			if (this.direction == PropertyBinding.Direction.SourceUpdatesTarget)
			{
				this.target.Set(this.source.Get());
				return;
			}
			if (this.direction == PropertyBinding.Direction.TargetUpdatesSource)
			{
				this.source.Set(this.target.Get());
				return;
			}
			if (this.source.GetPropertyType() == this.target.GetPropertyType())
			{
				object obj = this.source.Get();
				if (this.mLastValue == null || !this.mLastValue.Equals(obj))
				{
					this.mLastValue = obj;
					this.target.Set(obj);
					return;
				}
				obj = this.target.Get();
				if (!this.mLastValue.Equals(obj))
				{
					this.mLastValue = obj;
					this.source.Set(obj);
				}
			}
		}
	}

	public PropertyBinding()
	{
		this.update = PropertyBinding.UpdateCondition.OnUpdate;
		this.editMode = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			if (this.source == null)
			{
				this.source = new PropertyReference();
			}
			this.source.Unity_Serialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.target == null)
			{
				this.target = new PropertyReference();
			}
			this.target.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.direction);
		SerializedStateWriter.Instance.WriteInt32((int)this.update);
		SerializedStateWriter.Instance.WriteBoolean(this.editMode);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			if (this.source == null)
			{
				this.source = new PropertyReference();
			}
			this.source.Unity_Deserialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.target == null)
			{
				this.target = new PropertyReference();
			}
			this.target.Unity_Deserialize(depth + 1);
		}
		this.direction = (PropertyBinding.Direction)SerializedStateReader.Instance.ReadInt32();
		this.update = (PropertyBinding.UpdateCondition)SerializedStateReader.Instance.ReadInt32();
		this.editMode = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (depth <= 7)
		{
			if (this.source != null)
			{
				this.source.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (depth <= 7)
		{
			if (this.target != null)
			{
				this.target.Unity_RemapPPtrs(depth + 1);
			}
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			if (this.source == null)
			{
				this.source = new PropertyReference();
			}
			PropertyReference arg_3F_0 = this.source;
			ISerializedNamedStateWriter arg_37_0 = SerializedNamedStateWriter.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_37_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1563);
			arg_3F_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.target == null)
			{
				this.target = new PropertyReference();
			}
			PropertyReference arg_82_0 = this.target;
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 265);
			arg_82_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.direction, &var_0_cp_0[var_0_cp_1] + 1639);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.update, &var_0_cp_0[var_0_cp_1] + 2265);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.editMode, &var_0_cp_0[var_0_cp_1] + 2272);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			if (this.source == null)
			{
				this.source = new PropertyReference();
			}
			PropertyReference arg_3F_0 = this.source;
			ISerializedNamedStateReader arg_37_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_37_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1563);
			arg_3F_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.target == null)
			{
				this.target = new PropertyReference();
			}
			PropertyReference arg_82_0 = this.target;
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 265);
			arg_82_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		this.direction = (PropertyBinding.Direction)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1639);
		this.update = (PropertyBinding.UpdateCondition)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2265);
		this.editMode = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2272);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal PropertyBinding(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((PropertyBinding)instance).editMode;
	}

	public static void $Set0(object instance, bool value)
	{
		((PropertyBinding)instance).editMode = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).FixedUpdate();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).OnValidate();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((PropertyBinding)GCHandledObjects.GCHandleToObject(instance)).UpdateTarget();
		return -1L;
	}
}
