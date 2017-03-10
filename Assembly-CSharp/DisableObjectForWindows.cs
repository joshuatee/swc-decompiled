using System;
using UnityEngine;
using WinRTBridge;

public class DisableObjectForWindows : MonoBehaviour, IUnitySerializable
{
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	public DisableObjectForWindows()
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

	protected internal DisableObjectForWindows(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((DisableObjectForWindows)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((DisableObjectForWindows)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((DisableObjectForWindows)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((DisableObjectForWindows)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((DisableObjectForWindows)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((DisableObjectForWindows)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
