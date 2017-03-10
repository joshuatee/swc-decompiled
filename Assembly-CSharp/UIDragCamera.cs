using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Drag Camera"), ExecuteInEditMode]
public class UIDragCamera : MonoBehaviour, IUnitySerializable
{
	public UIDraggableCamera draggableCamera;

	private void Awake()
	{
		if (this.draggableCamera == null)
		{
			this.draggableCamera = NGUITools.FindInParents<UIDraggableCamera>(base.gameObject);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Press(isPressed);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Drag(delta);
		}
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Scroll(delta);
		}
	}

	public UIDragCamera()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.draggableCamera);
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.draggableCamera = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIDraggableCamera);
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.draggableCamera != null)
		{
			this.draggableCamera = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.draggableCamera) as UIDraggableCamera);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.draggableCamera, &$FieldNamesStorage.$RuntimeNames[0] + 472);
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		if (depth <= 7)
		{
			this.draggableCamera = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&$FieldNamesStorage.$RuntimeNames[0] + 472) as UIDraggableCamera);
		}
	}

	protected internal UIDragCamera(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragCamera)instance).draggableCamera);
	}

	public static void $Set0(object instance, long value)
	{
		((UIDragCamera)instance).draggableCamera = (UIDraggableCamera)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIDragCamera)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIDragCamera)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIDragCamera)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIDragCamera)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIDragCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIDragCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIDragCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIDragCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIDragCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
