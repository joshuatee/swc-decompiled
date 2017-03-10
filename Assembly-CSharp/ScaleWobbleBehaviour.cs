using System;
using UnityEngine;
using WinRTBridge;

public class ScaleWobbleBehaviour : MonoBehaviour, IUnitySerializable
{
	private const float SCALE_VALUE = 3f;

	private const float WOBBLE_SPEED_VALUE = 12f;

	private const float WOBBLE_AMPLITUDE_VALUE = 3f;

	private Vector3 scaleVector;

	private Vector3 wobbleSpeed;

	private Vector3 wobbleAmplitude;

	private float phaseOffset;

	private void Start()
	{
		this.wobbleSpeed = new Vector3(0f, 12f, 12f);
		this.wobbleAmplitude = new Vector3(0f, 3f, 3f);
		this.phaseOffset = UnityEngine.Random.Range(0f, 6.28318548f);
		this.scaleVector = new Vector3(3f, 3f, 3f);
		base.transform.localScale = this.scaleVector;
	}

	private void LateUpdate()
	{
		Vector3 eulerAngles;
		eulerAngles.x = this.wobbleAmplitude.x * Mathf.Sin(Time.realtimeSinceStartup * this.wobbleSpeed.x + this.phaseOffset);
		eulerAngles.y = this.wobbleAmplitude.y * Mathf.Sin(Time.realtimeSinceStartup * this.wobbleSpeed.y + this.phaseOffset);
		eulerAngles.z = this.wobbleAmplitude.z * Mathf.Sin(Time.realtimeSinceStartup * this.wobbleSpeed.z + this.phaseOffset);
		base.transform.Rotate(eulerAngles);
		if (base.transform.localScale == Vector3.one)
		{
			base.transform.localScale = this.scaleVector;
		}
	}

	private void OnDestroy()
	{
		base.transform.localScale = Vector3.one;
		base.transform.localRotation = Quaternion.identity;
	}

	public ScaleWobbleBehaviour()
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

	protected internal ScaleWobbleBehaviour(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((ScaleWobbleBehaviour)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((ScaleWobbleBehaviour)GCHandledObjects.GCHandleToObject(instance)).OnDestroy();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((ScaleWobbleBehaviour)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((ScaleWobbleBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((ScaleWobbleBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((ScaleWobbleBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((ScaleWobbleBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((ScaleWobbleBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
