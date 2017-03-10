using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Draggable Camera"), RequireComponent(typeof(Camera))]
public class UIDraggableCamera : MonoBehaviour, IUnitySerializable
{
	public Transform rootForBounds;

	public Vector2 scale;

	public float scrollWheelFactor;

	public UIDragObject.DragEffect dragEffect;

	public bool smoothDragStart;

	public float momentumAmount;

	private Camera mCam;

	private Transform mTrans;

	private bool mPressed;

	private Vector2 mMomentum;

	private Bounds mBounds;

	private float mScroll;

	private UIRoot mRoot;

	private bool mDragStarted;

	public Vector2 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
		}
	}

	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		this.mTrans = base.transform;
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		if (this.rootForBounds == null)
		{
			Debug.LogError(NGUITools.GetHierarchy(base.gameObject) + " needs the 'Root For Bounds' parameter to be set", this);
			base.enabled = false;
		}
	}

	private Vector3 CalculateConstrainOffset()
	{
		if (this.rootForBounds == null || this.rootForBounds.childCount == 0)
		{
			return Vector3.zero;
		}
		Vector3 vector = new Vector3(this.mCam.rect.xMin * (float)Screen.width, this.mCam.rect.yMin * (float)Screen.height, 0f);
		Vector3 vector2 = new Vector3(this.mCam.rect.xMax * (float)Screen.width, this.mCam.rect.yMax * (float)Screen.height, 0f);
		vector = this.mCam.ScreenToWorldPoint(vector);
		vector2 = this.mCam.ScreenToWorldPoint(vector2);
		Vector2 minRect = new Vector2(this.mBounds.min.x, this.mBounds.min.y);
		Vector2 maxRect = new Vector2(this.mBounds.max.x, this.mBounds.max.y);
		return NGUIMath.ConstrainRect(minRect, maxRect, vector, vector2);
	}

	public bool ConstrainToBounds(bool immediate)
	{
		if (this.mTrans != null && this.rootForBounds != null)
		{
			Vector3 b = this.CalculateConstrainOffset();
			if (b.sqrMagnitude > 0f)
			{
				if (immediate)
				{
					this.mTrans.position -= b;
				}
				else
				{
					SpringPosition springPosition = SpringPosition.Begin(base.gameObject, this.mTrans.position - b, 13f);
					springPosition.ignoreTimeScale = true;
					springPosition.worldSpace = true;
				}
				return true;
			}
		}
		return false;
	}

	public void Press(bool isPressed)
	{
		if (isPressed)
		{
			this.mDragStarted = false;
		}
		if (this.rootForBounds != null)
		{
			this.mPressed = isPressed;
			if (isPressed)
			{
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				this.mMomentum = Vector2.zero;
				this.mScroll = 0f;
				SpringPosition component = base.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
					return;
				}
			}
			else if (this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
			{
				this.ConstrainToBounds(false);
			}
		}
	}

	public void Drag(Vector2 delta)
	{
		if (this.smoothDragStart && !this.mDragStarted)
		{
			this.mDragStarted = true;
			return;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		if (this.mRoot != null)
		{
			delta *= this.mRoot.pixelSizeAdjustment;
		}
		Vector2 vector = Vector2.Scale(delta, -this.scale);
		this.mTrans.localPosition += vector;
		this.mMomentum = Vector2.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
		if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.ConstrainToBounds(true))
		{
			this.mMomentum = Vector2.zero;
			this.mScroll = 0f;
		}
	}

	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}

	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		if (this.mPressed)
		{
			SpringPosition component = base.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
			this.mScroll = 0f;
		}
		else
		{
			this.mMomentum += this.scale * (this.mScroll * 20f);
			this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
			if (this.mMomentum.magnitude > 0.01f)
			{
				this.mTrans.localPosition += NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				if (!this.ConstrainToBounds(this.dragEffect == UIDragObject.DragEffect.None))
				{
					SpringPosition component2 = base.GetComponent<SpringPosition>();
					if (component2 != null)
					{
						component2.enabled = false;
					}
				}
				return;
			}
			this.mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
	}

	public UIDraggableCamera()
	{
		this.scale = Vector2.one;
		this.dragEffect = UIDragObject.DragEffect.MomentumAndSpring;
		this.smoothDragStart = true;
		this.momentumAmount = 35f;
		this.mMomentum = Vector2.zero;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.rootForBounds);
		}
		if (depth <= 7)
		{
			this.scale.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.scrollWheelFactor);
		SerializedStateWriter.Instance.WriteInt32((int)this.dragEffect);
		SerializedStateWriter.Instance.WriteBoolean(this.smoothDragStart);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.momentumAmount);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.rootForBounds = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		if (depth <= 7)
		{
			this.scale.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.scrollWheelFactor = SerializedStateReader.Instance.ReadSingle();
		this.dragEffect = (UIDragObject.DragEffect)SerializedStateReader.Instance.ReadInt32();
		this.smoothDragStart = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.momentumAmount = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.rootForBounds != null)
		{
			this.rootForBounds = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.rootForBounds) as Transform);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.rootForBounds;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 558);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 572);
			this.scale.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.scrollWheelFactor, &var_0_cp_0[var_0_cp_1] + 578);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.dragEffect, &var_0_cp_0[var_0_cp_1] + 596);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.smoothDragStart, &var_0_cp_0[var_0_cp_1] + 607);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.momentumAmount, &var_0_cp_0[var_0_cp_1] + 623);
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
			this.rootForBounds = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 558) as Transform);
		}
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 572);
			this.scale.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.scrollWheelFactor = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 578);
		this.dragEffect = (UIDragObject.DragEffect)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 596);
		this.smoothDragStart = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 607);
		SerializedNamedStateReader.Instance.Align();
		this.momentumAmount = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 623);
	}

	protected internal UIDraggableCamera(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDraggableCamera)instance).rootForBounds);
	}

	public static void $Set0(object instance, long value)
	{
		((UIDraggableCamera)instance).rootForBounds = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get1(object instance, int index)
	{
		UIDraggableCamera expr_06_cp_0 = (UIDraggableCamera)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.scale.x;
		case 1:
			return expr_06_cp_0.scale.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set1(object instance, float value, int index)
	{
		UIDraggableCamera expr_06_cp_0 = (UIDraggableCamera)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.scale.x = value;
			return;
		case 1:
			expr_06_cp_0.scale.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get2(object instance)
	{
		return ((UIDraggableCamera)instance).scrollWheelFactor;
	}

	public static void $Set2(object instance, float value)
	{
		((UIDraggableCamera)instance).scrollWheelFactor = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UIDraggableCamera)instance).smoothDragStart;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIDraggableCamera)instance).smoothDragStart = value;
	}

	public static float $Get4(object instance)
	{
		return ((UIDraggableCamera)instance).momentumAmount;
	}

	public static void $Set4(object instance, float value)
	{
		((UIDraggableCamera)instance).momentumAmount = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).CalculateConstrainOffset());
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).ConstrainToBounds(*(sbyte*)args != 0));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Drag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).currentMomentum);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Press(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Scroll(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).currentMomentum = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIDraggableCamera)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
