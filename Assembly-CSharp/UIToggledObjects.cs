using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour, IUnitySerializable
{
	public List<GameObject> activate;

	public List<GameObject> deactivate;

	[HideInInspector, SerializeField]
	protected internal GameObject target;

	[HideInInspector, SerializeField]
	protected internal bool inverse;

	private void Awake()
	{
		if (this.target != null)
		{
			if (this.activate.Count == 0 && this.deactivate.Count == 0)
			{
				if (this.inverse)
				{
					this.deactivate.Add(this.target);
				}
				else
				{
					this.activate.Add(this.target);
				}
			}
			else
			{
				this.target = null;
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.Toggle));
	}

	public void Toggle()
	{
		bool value = UIToggle.current.value;
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				this.Set(this.activate[i], value);
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				this.Set(this.deactivate[j], !value);
			}
		}
	}

	private void Set(GameObject go, bool state)
	{
		if (go != null)
		{
			NGUITools.SetActive(go, state);
		}
	}

	public UIToggledObjects()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			if (this.activate == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.activate.Count);
				for (int i = 0; i < this.activate.Count; i++)
				{
					SerializedStateWriter.Instance.WriteUnityEngineObject(this.activate[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.deactivate == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.deactivate.Count);
				for (int i = 0; i < this.deactivate.Count; i++)
				{
					SerializedStateWriter.Instance.WriteUnityEngineObject(this.deactivate[i]);
				}
			}
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.target);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.inverse);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.activate = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.activate.Add(SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.deactivate = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.deactivate.Add(SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
			}
		}
		if (depth <= 7)
		{
			this.target = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.inverse = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (depth <= 7)
		{
			if (this.activate != null)
			{
				for (int i = 0; i < this.activate.Count; i++)
				{
					this.activate[i] = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.activate[i]) as GameObject);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.deactivate != null)
			{
				for (int i = 0; i < this.deactivate.Count; i++)
				{
					this.deactivate[i] = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.deactivate[i]) as GameObject);
				}
			}
		}
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
			if (this.activate == null)
			{
				ISerializedNamedStateWriter arg_29_0 = SerializedNamedStateWriter.Instance;
				var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
				var_0_cp_1 = 0;
				arg_29_0.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2025, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2025, this.activate.Count);
				for (int i = 0; i < this.activate.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.activate[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.deactivate == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2034, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2034, this.deactivate.Count);
				for (int i = 0; i < this.deactivate.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.deactivate[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.target, &var_0_cp_0[var_0_cp_1] + 265);
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.inverse, &var_0_cp_0[var_0_cp_1] + 2045);
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
			int num = arg_1E_0.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2025);
			this.activate = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.activate.Add(SerializedNamedStateReader.Instance.ReadUnityEngineObject((IntPtr)0) as GameObject);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2034);
			this.deactivate = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.deactivate.Add(SerializedNamedStateReader.Instance.ReadUnityEngineObject((IntPtr)0) as GameObject);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			this.target = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 265) as GameObject);
		}
		this.inverse = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2045);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIToggledObjects(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIToggledObjects)instance).target);
	}

	public static void $Set0(object instance, long value)
	{
		((UIToggledObjects)instance).target = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UIToggledObjects)instance).inverse;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIToggledObjects)instance).inverse = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIToggledObjects)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIToggledObjects)GCHandledObjects.GCHandleToObject(instance)).Set((GameObject)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIToggledObjects)GCHandledObjects.GCHandleToObject(instance)).Toggle();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIToggledObjects)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIToggledObjects)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIToggledObjects)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIToggledObjects)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIToggledObjects)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
