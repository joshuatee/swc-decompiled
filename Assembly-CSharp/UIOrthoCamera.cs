using System;
using UnityEngine;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Orthographic Camera"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class UIOrthoCamera : MonoBehaviour, IUnitySerializable
{
	private Camera mCam;

	private Transform mTrans;

	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		this.mTrans = base.transform;
		this.mCam.orthographic = true;
	}

	private void Update()
	{
		float num = this.mCam.rect.yMin * (float)Screen.height;
		float num2 = this.mCam.rect.yMax * (float)Screen.height;
		float num3 = (num2 - num) * 0.5f * this.mTrans.lossyScale.y;
		if (!Mathf.Approximately(this.mCam.orthographicSize, num3))
		{
			this.mCam.orthographicSize = num3;
		}
	}

	public UIOrthoCamera()
	{
	}

	public override void Unity_Serialize(int depth)
	{
	}

	public override void Unity_Deserialize(int depth)
	{
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public override void Unity_NamedSerialize(int depth)
	{
	}

	public override void Unity_NamedDeserialize(int depth)
	{
	}

	protected internal UIOrthoCamera(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIOrthoCamera)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIOrthoCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIOrthoCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIOrthoCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIOrthoCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIOrthoCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIOrthoCamera)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
