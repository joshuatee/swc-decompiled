using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Internal/Snapshot Point"), ExecuteInEditMode]
public class UISnapshotPoint : MonoBehaviour, IUnitySerializable
{
	public bool isOrthographic;

	public float nearClip;

	public float farClip;

	[Range(10f, 80f)]
	public int fieldOfView;

	public float orthoSize;

	public Texture2D thumbnail;

	private void Start()
	{
		if (base.tag != "EditorOnly")
		{
			base.tag = "EditorOnly";
		}
	}

	public UISnapshotPoint()
	{
		this.isOrthographic = true;
		this.nearClip = -100f;
		this.farClip = 100f;
		this.fieldOfView = 35;
		this.orthoSize = 30f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteBoolean(this.isOrthographic);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.nearClip);
		SerializedStateWriter.Instance.WriteSingle(this.farClip);
		SerializedStateWriter.Instance.WriteInt32(this.fieldOfView);
		SerializedStateWriter.Instance.WriteSingle(this.orthoSize);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.thumbnail);
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		this.isOrthographic = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.nearClip = SerializedStateReader.Instance.ReadSingle();
		this.farClip = SerializedStateReader.Instance.ReadSingle();
		this.fieldOfView = SerializedStateReader.Instance.ReadInt32();
		this.orthoSize = SerializedStateReader.Instance.ReadSingle();
		if (depth <= 7)
		{
			this.thumbnail = (SerializedStateReader.Instance.ReadUnityEngineObject() as Texture2D);
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.thumbnail != null)
		{
			this.thumbnail = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.thumbnail) as Texture2D);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		bool arg_1F_1 = this.isOrthographic;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteBoolean(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 2566);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.nearClip, &var_0_cp_0[var_0_cp_1] + 2581);
		SerializedNamedStateWriter.Instance.WriteSingle(this.farClip, &var_0_cp_0[var_0_cp_1] + 2590);
		SerializedNamedStateWriter.Instance.WriteInt32(this.fieldOfView, &var_0_cp_0[var_0_cp_1] + 2598);
		SerializedNamedStateWriter.Instance.WriteSingle(this.orthoSize, &var_0_cp_0[var_0_cp_1] + 2610);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.thumbnail, &var_0_cp_0[var_0_cp_1] + 2620);
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.isOrthographic = arg_1A_0.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2566);
		SerializedNamedStateReader.Instance.Align();
		this.nearClip = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2581);
		this.farClip = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2590);
		this.fieldOfView = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2598);
		this.orthoSize = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2610);
		if (depth <= 7)
		{
			this.thumbnail = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2620) as Texture2D);
		}
	}

	protected internal UISnapshotPoint(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UISnapshotPoint)instance).isOrthographic;
	}

	public static void $Set0(object instance, bool value)
	{
		((UISnapshotPoint)instance).isOrthographic = value;
	}

	public static float $Get1(object instance)
	{
		return ((UISnapshotPoint)instance).nearClip;
	}

	public static void $Set1(object instance, float value)
	{
		((UISnapshotPoint)instance).nearClip = value;
	}

	public static float $Get2(object instance)
	{
		return ((UISnapshotPoint)instance).farClip;
	}

	public static void $Set2(object instance, float value)
	{
		((UISnapshotPoint)instance).farClip = value;
	}

	public static float $Get3(object instance)
	{
		return ((UISnapshotPoint)instance).orthoSize;
	}

	public static void $Set3(object instance, float value)
	{
		((UISnapshotPoint)instance).orthoSize = value;
	}

	public static long $Get4(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISnapshotPoint)instance).thumbnail);
	}

	public static void $Set4(object instance, long value)
	{
		((UISnapshotPoint)instance).thumbnail = (Texture2D)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UISnapshotPoint)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UISnapshotPoint)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UISnapshotPoint)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UISnapshotPoint)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UISnapshotPoint)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UISnapshotPoint)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
