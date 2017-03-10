using System;
using UnityEngine;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour, IUnitySerializable
{
	public static Transform root;

	private void OnEnable()
	{
		UIDragDropRoot.root = base.transform;
	}

	private void OnDisable()
	{
		if (UIDragDropRoot.root == base.transform)
		{
			UIDragDropRoot.root = null;
		}
	}

	public UIDragDropRoot()
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

	protected internal UIDragDropRoot(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIDragDropRoot)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIDragDropRoot)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIDragDropRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIDragDropRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIDragDropRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIDragDropRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIDragDropRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
