using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Viewport Camera"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class UIViewport : MonoBehaviour, IUnitySerializable
{
	public Camera sourceCamera;

	public Transform topLeft;

	public Transform bottomRight;

	public float fullSize;

	private Camera mCam;

	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		if (this.sourceCamera == null)
		{
			this.sourceCamera = Camera.main;
		}
	}

	private void LateUpdate()
	{
		if (this.topLeft != null && this.bottomRight != null)
		{
			if (this.topLeft.gameObject.activeInHierarchy)
			{
				Vector3 vector = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
				Vector3 vector2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
				Rect rect = new Rect(vector.x / (float)Screen.width, vector2.y / (float)Screen.height, (vector2.x - vector.x) / (float)Screen.width, (vector.y - vector2.y) / (float)Screen.height);
				float num = this.fullSize * rect.height;
				if (rect != this.mCam.rect)
				{
					this.mCam.rect = rect;
				}
				if (this.mCam.orthographicSize != num)
				{
					this.mCam.orthographicSize = num;
				}
				this.mCam.enabled = true;
				return;
			}
			this.mCam.enabled = false;
		}
	}

	public UIViewport()
	{
		this.fullSize = 1f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.sourceCamera);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.topLeft);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.bottomRight);
		}
		SerializedStateWriter.Instance.WriteSingle(this.fullSize);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.sourceCamera = (SerializedStateReader.Instance.ReadUnityEngineObject() as Camera);
		}
		if (depth <= 7)
		{
			this.topLeft = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		if (depth <= 7)
		{
			this.bottomRight = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		this.fullSize = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.sourceCamera != null)
		{
			this.sourceCamera = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.sourceCamera) as Camera);
		}
		if (this.topLeft != null)
		{
			this.topLeft = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.topLeft) as Transform);
		}
		if (this.bottomRight != null)
		{
			this.bottomRight = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.bottomRight) as Transform);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.sourceCamera;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 4652);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.topLeft, &var_0_cp_0[var_0_cp_1] + 4665);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.bottomRight, &var_0_cp_0[var_0_cp_1] + 4673);
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.fullSize, &var_0_cp_0[var_0_cp_1] + 4685);
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
			this.sourceCamera = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4652) as Camera);
		}
		if (depth <= 7)
		{
			this.topLeft = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4665) as Transform);
		}
		if (depth <= 7)
		{
			this.bottomRight = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4673) as Transform);
		}
		this.fullSize = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 4685);
	}

	protected internal UIViewport(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIViewport)instance).sourceCamera);
	}

	public static void $Set0(object instance, long value)
	{
		((UIViewport)instance).sourceCamera = (Camera)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIViewport)instance).topLeft);
	}

	public static void $Set1(object instance, long value)
	{
		((UIViewport)instance).topLeft = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIViewport)instance).bottomRight);
	}

	public static void $Set2(object instance, long value)
	{
		((UIViewport)instance).bottomRight = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get3(object instance)
	{
		return ((UIViewport)instance).fullSize;
	}

	public static void $Set3(object instance, float value)
	{
		((UIViewport)instance).fullSize = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIViewport)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIViewport)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIViewport)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIViewport)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIViewport)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIViewport)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIViewport)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
