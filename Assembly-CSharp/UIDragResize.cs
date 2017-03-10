using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour, IUnitySerializable
{
	public UIWidget target;

	public UIWidget.Pivot pivot;

	public int minWidth;

	public int minHeight;

	public int maxWidth;

	public int maxHeight;

	public bool updateAnchors;

	private Plane mPlane;

	private Vector3 mRayPos;

	private Vector3 mLocalPos;

	private int mWidth;

	private int mHeight;

	private bool mDragging;

	private void OnDragStart()
	{
		if (this.target != null)
		{
			Vector3[] worldCorners = this.target.worldCorners;
			this.mPlane = new Plane(worldCorners[0], worldCorners[1], worldCorners[3]);
			Ray currentRay = UICamera.currentRay;
			float distance;
			if (this.mPlane.Raycast(currentRay, out distance))
			{
				this.mRayPos = currentRay.GetPoint(distance);
				this.mLocalPos = this.target.cachedTransform.localPosition;
				this.mWidth = this.target.width;
				this.mHeight = this.target.height;
				this.mDragging = true;
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.mDragging && this.target != null)
		{
			Ray currentRay = UICamera.currentRay;
			float distance;
			if (this.mPlane.Raycast(currentRay, out distance))
			{
				Transform cachedTransform = this.target.cachedTransform;
				cachedTransform.localPosition = this.mLocalPos;
				this.target.width = this.mWidth;
				this.target.height = this.mHeight;
				Vector3 b = currentRay.GetPoint(distance) - this.mRayPos;
				cachedTransform.position += b;
				Vector3 vector = Quaternion.Inverse(cachedTransform.localRotation) * (cachedTransform.localPosition - this.mLocalPos);
				cachedTransform.localPosition = this.mLocalPos;
				NGUIMath.ResizeWidget(this.target, this.pivot, vector.x, vector.y, this.minWidth, this.minHeight, this.maxWidth, this.maxHeight);
				if (this.updateAnchors)
				{
					this.target.BroadcastMessage("UpdateAnchors");
				}
			}
		}
	}

	private void OnDragEnd()
	{
		this.mDragging = false;
	}

	public UIDragResize()
	{
		this.pivot = UIWidget.Pivot.BottomRight;
		this.minWidth = 100;
		this.minHeight = 100;
		this.maxWidth = 100000;
		this.maxHeight = 100000;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.target);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.pivot);
		SerializedStateWriter.Instance.WriteInt32(this.minWidth);
		SerializedStateWriter.Instance.WriteInt32(this.minHeight);
		SerializedStateWriter.Instance.WriteInt32(this.maxWidth);
		SerializedStateWriter.Instance.WriteInt32(this.maxHeight);
		SerializedStateWriter.Instance.WriteBoolean(this.updateAnchors);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIWidget);
		}
		this.pivot = (UIWidget.Pivot)SerializedStateReader.Instance.ReadInt32();
		this.minWidth = SerializedStateReader.Instance.ReadInt32();
		this.minHeight = SerializedStateReader.Instance.ReadInt32();
		this.maxWidth = SerializedStateReader.Instance.ReadInt32();
		this.maxHeight = SerializedStateReader.Instance.ReadInt32();
		this.updateAnchors = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.target != null)
		{
			this.target = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.target) as UIWidget);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.target;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 265);
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.pivot, &var_0_cp_0[var_0_cp_1] + 697);
		SerializedNamedStateWriter.Instance.WriteInt32(this.minWidth, &var_0_cp_0[var_0_cp_1] + 703);
		SerializedNamedStateWriter.Instance.WriteInt32(this.minHeight, &var_0_cp_0[var_0_cp_1] + 712);
		SerializedNamedStateWriter.Instance.WriteInt32(this.maxWidth, &var_0_cp_0[var_0_cp_1] + 722);
		SerializedNamedStateWriter.Instance.WriteInt32(this.maxHeight, &var_0_cp_0[var_0_cp_1] + 731);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.updateAnchors, &var_0_cp_0[var_0_cp_1] + 741);
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
			this.target = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 265) as UIWidget);
		}
		this.pivot = (UIWidget.Pivot)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 697);
		this.minWidth = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 703);
		this.minHeight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 712);
		this.maxWidth = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 722);
		this.maxHeight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 731);
		this.updateAnchors = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 741);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIDragResize(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragResize)instance).target);
	}

	public static void $Set0(object instance, long value)
	{
		((UIDragResize)instance).target = (UIWidget)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UIDragResize)instance).updateAnchors;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIDragResize)instance).updateAnchors = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIDragResize)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIDragResize)GCHandledObjects.GCHandleToObject(instance)).OnDragEnd();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIDragResize)GCHandledObjects.GCHandleToObject(instance)).OnDragStart();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIDragResize)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIDragResize)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIDragResize)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIDragResize)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIDragResize)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
