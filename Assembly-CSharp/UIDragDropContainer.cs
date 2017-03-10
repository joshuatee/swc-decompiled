using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Container")]
public class UIDragDropContainer : MonoBehaviour, IUnitySerializable
{
	public Transform reparentTarget;

	protected virtual void Start()
	{
		if (this.reparentTarget == null)
		{
			this.reparentTarget = base.transform;
		}
	}

	public UIDragDropContainer()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.reparentTarget);
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.reparentTarget = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.reparentTarget != null)
		{
			this.reparentTarget = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.reparentTarget) as Transform);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.reparentTarget, &$FieldNamesStorage.$RuntimeNames[0] + 488);
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		if (depth <= 7)
		{
			this.reparentTarget = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&$FieldNamesStorage.$RuntimeNames[0] + 488) as Transform);
		}
	}

	protected internal UIDragDropContainer(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragDropContainer)instance).reparentTarget);
	}

	public static void $Set0(object instance, long value)
	{
		((UIDragDropContainer)instance).reparentTarget = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIDragDropContainer)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIDragDropContainer)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIDragDropContainer)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIDragDropContainer)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIDragDropContainer)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIDragDropContainer)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
