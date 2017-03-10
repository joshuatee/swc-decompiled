using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Drag Object"), ExecuteInEditMode]
public class UIDragObject : MonoBehaviour, IUnitySerializable
{
	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}

	public Transform target;

	public UIPanel panelRegion;

	public Vector3 scrollMomentum;

	public bool restrictWithinPanel;

	public UIRect contentRect;

	public UIDragObject.DragEffect dragEffect;

	public float momentumAmount;

	[SerializeField]
	protected Vector3 scale;

	[HideInInspector, SerializeField]
	protected internal float scrollWheelFactor;

	private Plane mPlane;

	private Vector3 mTargetPos;

	private Vector3 mLastPos;

	private Vector3 mMomentum;

	private Vector3 mScroll;

	private Bounds mBounds;

	private int mTouchID;

	private bool mStarted;

	private bool mPressed;

	public Vector3 dragMovement
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
		}
	}

	private void OnEnable()
	{
		if (this.scrollWheelFactor != 0f)
		{
			this.scrollMomentum = this.scale * this.scrollWheelFactor;
			this.scrollWheelFactor = 0f;
		}
		if (this.contentRect == null && this.target != null && Application.isPlaying)
		{
			UIWidget component = this.target.GetComponent<UIWidget>();
			if (component != null)
			{
				this.contentRect = component;
			}
		}
		this.mTargetPos = ((this.target != null) ? this.target.position : Vector3.zero);
	}

	private void OnDisable()
	{
		this.mStarted = false;
	}

	private void FindPanel()
	{
		this.panelRegion = ((this.target != null) ? UIPanel.Find(this.target.transform.parent) : null);
		if (this.panelRegion == null)
		{
			this.restrictWithinPanel = false;
		}
	}

	private void UpdateBounds()
	{
		if (this.contentRect)
		{
			Transform cachedTransform = this.panelRegion.cachedTransform;
			Matrix4x4 worldToLocalMatrix = cachedTransform.worldToLocalMatrix;
			Vector3[] worldCorners = this.contentRect.worldCorners;
			for (int i = 0; i < 4; i++)
			{
				worldCorners[i] = worldToLocalMatrix.MultiplyPoint3x4(worldCorners[i]);
			}
			this.mBounds = new Bounds(worldCorners[0], Vector3.zero);
			for (int j = 1; j < 4; j++)
			{
				this.mBounds.Encapsulate(worldCorners[j]);
			}
			return;
		}
		this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.panelRegion.cachedTransform, this.target);
	}

	private void OnPress(bool pressed)
	{
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		float timeScale = Time.timeScale;
		if (timeScale < 0.01f && timeScale != 0f)
		{
			return;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			if (pressed)
			{
				if (!this.mPressed)
				{
					this.mTouchID = UICamera.currentTouchID;
					this.mPressed = true;
					this.mStarted = false;
					this.CancelMovement();
					if (this.restrictWithinPanel && this.panelRegion == null)
					{
						this.FindPanel();
					}
					if (this.restrictWithinPanel)
					{
						this.UpdateBounds();
					}
					this.CancelSpring();
					Transform transform = UICamera.currentCamera.transform;
					this.mPlane = new Plane(((this.panelRegion != null) ? this.panelRegion.cachedTransform.rotation : transform.rotation) * Vector3.back, UICamera.lastWorldPosition);
					return;
				}
			}
			else if (this.mPressed && this.mTouchID == UICamera.currentTouchID)
			{
				this.mPressed = false;
				if (this.restrictWithinPanel && this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, false))
				{
					this.CancelMovement();
				}
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.mPressed && this.mTouchID == UICamera.currentTouchID && base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float distance = 0f;
			if (this.mPlane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (!this.mStarted)
				{
					this.mStarted = true;
					vector = Vector3.zero;
				}
				if (vector.x != 0f || vector.y != 0f)
				{
					vector = this.target.InverseTransformDirection(vector);
					vector.Scale(this.scale);
					vector = this.target.TransformDirection(vector);
				}
				if (this.dragEffect != UIDragObject.DragEffect.None)
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				Vector3 localPosition = this.target.localPosition;
				this.Move(vector);
				if (this.restrictWithinPanel)
				{
					this.mBounds.center = this.mBounds.center + (this.target.localPosition - localPosition);
					if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
					{
						this.CancelMovement();
					}
				}
			}
		}
	}

	private void Move(Vector3 worldDelta)
	{
		if (this.panelRegion != null)
		{
			this.mTargetPos += worldDelta;
			Transform parent = this.target.parent;
			Rigidbody component = this.target.GetComponent<Rigidbody>();
			if (parent != null)
			{
				Vector3 vector = parent.worldToLocalMatrix.MultiplyPoint3x4(this.mTargetPos);
				vector.x = Mathf.Round(vector.x);
				vector.y = Mathf.Round(vector.y);
				if (component != null)
				{
					vector = parent.localToWorldMatrix.MultiplyPoint3x4(vector);
					component.position = vector;
				}
				else
				{
					this.target.localPosition = vector;
				}
			}
			else if (component != null)
			{
				component.position = this.mTargetPos;
			}
			else
			{
				this.target.position = this.mTargetPos;
			}
			UIScrollView component2 = this.panelRegion.GetComponent<UIScrollView>();
			if (component2 != null)
			{
				component2.UpdateScrollbars(true);
				return;
			}
		}
		else
		{
			this.target.position += worldDelta;
		}
	}

	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		this.mMomentum -= this.mScroll;
		this.mScroll = NGUIMath.SpringLerp(this.mScroll, Vector3.zero, 20f, deltaTime);
		if (this.mMomentum.magnitude < 0.0001f)
		{
			return;
		}
		if (!this.mPressed)
		{
			if (this.panelRegion == null)
			{
				this.FindPanel();
			}
			this.Move(NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime));
			if (this.restrictWithinPanel && this.panelRegion != null)
			{
				this.UpdateBounds();
				if (this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
				{
					this.CancelMovement();
				}
				else
				{
					this.CancelSpring();
				}
			}
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
			if (this.mMomentum.magnitude < 0.0001f)
			{
				this.CancelMovement();
				return;
			}
		}
		else
		{
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
		}
	}

	public void CancelMovement()
	{
		if (this.target != null)
		{
			Vector3 localPosition = this.target.localPosition;
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			this.target.localPosition = localPosition;
		}
		this.mTargetPos = ((this.target != null) ? this.target.position : Vector3.zero);
		this.mMomentum = Vector3.zero;
		this.mScroll = Vector3.zero;
	}

	public void CancelSpring()
	{
		SpringPosition component = this.target.GetComponent<SpringPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.mScroll -= this.scrollMomentum * (delta * 0.05f);
		}
	}

	public UIDragObject()
	{
		this.scrollMomentum = Vector3.zero;
		this.dragEffect = UIDragObject.DragEffect.MomentumAndSpring;
		this.momentumAmount = 35f;
		this.scale = new Vector3(1f, 1f, 0f);
		this.mMomentum = Vector3.zero;
		this.mScroll = Vector3.zero;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.target);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.panelRegion);
		}
		if (depth <= 7)
		{
			this.scrollMomentum.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.restrictWithinPanel);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.contentRect);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.dragEffect);
		SerializedStateWriter.Instance.WriteSingle(this.momentumAmount);
		if (depth <= 7)
		{
			this.scale.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.scrollWheelFactor);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		if (depth <= 7)
		{
			this.panelRegion = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIPanel);
		}
		if (depth <= 7)
		{
			this.scrollMomentum.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.restrictWithinPanel = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.contentRect = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIRect);
		}
		this.dragEffect = (UIDragObject.DragEffect)SerializedStateReader.Instance.ReadInt32();
		this.momentumAmount = SerializedStateReader.Instance.ReadSingle();
		if (depth <= 7)
		{
			this.scale.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.scrollWheelFactor = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.target != null)
		{
			this.target = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.target) as Transform);
		}
		if (this.panelRegion != null)
		{
			this.panelRegion = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.panelRegion) as UIPanel);
		}
		if (this.contentRect != null)
		{
			this.contentRect = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.contentRect) as UIRect);
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
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.panelRegion, &var_0_cp_0[var_0_cp_1] + 638);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 650);
			this.scrollMomentum.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.restrictWithinPanel, &var_0_cp_0[var_0_cp_1] + 665);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.contentRect, &var_0_cp_0[var_0_cp_1] + 685);
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.dragEffect, &var_0_cp_0[var_0_cp_1] + 596);
		SerializedNamedStateWriter.Instance.WriteSingle(this.momentumAmount, &var_0_cp_0[var_0_cp_1] + 623);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 572);
			this.scale.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.scrollWheelFactor, &var_0_cp_0[var_0_cp_1] + 578);
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
			this.target = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 265) as Transform);
		}
		if (depth <= 7)
		{
			this.panelRegion = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 638) as UIPanel);
		}
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 650);
			this.scrollMomentum.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.restrictWithinPanel = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 665);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.contentRect = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 685) as UIRect);
		}
		this.dragEffect = (UIDragObject.DragEffect)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 596);
		this.momentumAmount = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 623);
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 572);
			this.scale.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.scrollWheelFactor = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 578);
	}

	protected internal UIDragObject(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragObject)instance).target);
	}

	public static void $Set0(object instance, long value)
	{
		((UIDragObject)instance).target = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragObject)instance).panelRegion);
	}

	public static void $Set1(object instance, long value)
	{
		((UIDragObject)instance).panelRegion = (UIPanel)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get2(object instance, int index)
	{
		UIDragObject expr_06_cp_0 = (UIDragObject)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.scrollMomentum.x;
		case 1:
			return expr_06_cp_0.scrollMomentum.y;
		case 2:
			return expr_06_cp_0.scrollMomentum.z;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set2(object instance, float value, int index)
	{
		UIDragObject expr_06_cp_0 = (UIDragObject)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.scrollMomentum.x = value;
			return;
		case 1:
			expr_06_cp_0.scrollMomentum.y = value;
			return;
		case 2:
			expr_06_cp_0.scrollMomentum.z = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static bool $Get3(object instance)
	{
		return ((UIDragObject)instance).restrictWithinPanel;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIDragObject)instance).restrictWithinPanel = value;
	}

	public static long $Get4(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragObject)instance).contentRect);
	}

	public static void $Set4(object instance, long value)
	{
		((UIDragObject)instance).contentRect = (UIRect)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get5(object instance)
	{
		return ((UIDragObject)instance).momentumAmount;
	}

	public static void $Set5(object instance, float value)
	{
		((UIDragObject)instance).momentumAmount = value;
	}

	public static float $Get6(object instance, int index)
	{
		UIDragObject expr_06_cp_0 = (UIDragObject)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.scale.x;
		case 1:
			return expr_06_cp_0.scale.y;
		case 2:
			return expr_06_cp_0.scale.z;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set6(object instance, float value, int index)
	{
		UIDragObject expr_06_cp_0 = (UIDragObject)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.scale.x = value;
			return;
		case 1:
			expr_06_cp_0.scale.y = value;
			return;
		case 2:
			expr_06_cp_0.scale.z = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get7(object instance)
	{
		return ((UIDragObject)instance).scrollWheelFactor;
	}

	public static void $Set7(object instance, float value)
	{
		((UIDragObject)instance).scrollWheelFactor = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).CancelMovement();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).CancelSpring();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).FindPanel();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).dragMovement);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).Move(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).dragMovement = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIDragObject)GCHandledObjects.GCHandleToObject(instance)).UpdateBounds();
		return -1L;
	}
}
