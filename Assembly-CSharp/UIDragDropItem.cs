using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour, IUnitySerializable
{
	public enum Restriction
	{
		None,
		Horizontal,
		Vertical,
		PressAndHold
	}

	public UIDragDropItem.Restriction restriction;

	public bool cloneOnDrag;

	[HideInInspector]
	public float pressAndHoldDelay;

	public bool interactable;

	[System.NonSerialized]
	protected Transform mTrans;

	[System.NonSerialized]
	protected Transform mParent;

	[System.NonSerialized]
	protected Collider mCollider;

	[System.NonSerialized]
	protected Collider2D mCollider2D;

	[System.NonSerialized]
	protected UIButton mButton;

	[System.NonSerialized]
	protected UIRoot mRoot;

	[System.NonSerialized]
	protected UIGrid mGrid;

	[System.NonSerialized]
	protected UITable mTable;

	[System.NonSerialized]
	protected float mDragStartTime;

	[System.NonSerialized]
	protected UIDragScrollView mDragScrollView;

	[System.NonSerialized]
	protected bool mPressed;

	[System.NonSerialized]
	protected bool mDragging;

	[System.NonSerialized]
	protected UICamera.MouseOrTouch mTouch;

	public static List<UIDragDropItem> draggedItems = new List<UIDragDropItem>();

	protected virtual void Awake()
	{
		this.mTrans = base.transform;
		this.mCollider = base.gameObject.GetComponent<Collider>();
		this.mCollider2D = base.gameObject.GetComponent<Collider2D>();
	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void OnDisable()
	{
		if (this.mDragging)
		{
			this.StopDragging(UICamera.hoveredObject);
		}
	}

	protected virtual void Start()
	{
		this.mButton = base.GetComponent<UIButton>();
		this.mDragScrollView = base.GetComponent<UIDragScrollView>();
	}

	protected virtual void OnPress(bool isPressed)
	{
		if (!this.interactable || UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (isPressed)
		{
			if (!this.mPressed)
			{
				this.mTouch = UICamera.currentTouch;
				this.mDragStartTime = RealTime.time + this.pressAndHoldDelay;
				this.mPressed = true;
				return;
			}
		}
		else if (this.mPressed && this.mTouch == UICamera.currentTouch)
		{
			this.mPressed = false;
			this.mTouch = null;
		}
	}

	protected virtual void Update()
	{
		if (this.restriction == UIDragDropItem.Restriction.PressAndHold && this.mPressed && !this.mDragging && this.mDragStartTime < RealTime.time)
		{
			this.StartDragging();
		}
	}

	protected virtual void OnDragStart()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!base.enabled || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		if (this.restriction != UIDragDropItem.Restriction.None)
		{
			if (this.restriction == UIDragDropItem.Restriction.Horizontal)
			{
				Vector2 totalDelta = this.mTouch.totalDelta;
				if (Mathf.Abs(totalDelta.x) < Mathf.Abs(totalDelta.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.Vertical)
			{
				Vector2 totalDelta2 = this.mTouch.totalDelta;
				if (Mathf.Abs(totalDelta2.x) > Mathf.Abs(totalDelta2.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.PressAndHold)
			{
				return;
			}
		}
		this.StartDragging();
	}

	public virtual void StartDragging()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!this.mDragging)
		{
			if (this.cloneOnDrag)
			{
				this.mPressed = false;
				GameObject gameObject = NGUITools.AddChild(base.transform.parent.gameObject, base.gameObject);
				gameObject.transform.localPosition = base.transform.localPosition;
				gameObject.transform.localRotation = base.transform.localRotation;
				gameObject.transform.localScale = base.transform.localScale;
				UIButtonColor component = gameObject.GetComponent<UIButtonColor>();
				if (component != null)
				{
					component.defaultColor = base.GetComponent<UIButtonColor>().defaultColor;
				}
				if (this.mTouch != null && this.mTouch.pressed == base.gameObject)
				{
					this.mTouch.current = gameObject;
					this.mTouch.pressed = gameObject;
					this.mTouch.dragged = gameObject;
					this.mTouch.last = gameObject;
				}
				UIDragDropItem component2 = gameObject.GetComponent<UIDragDropItem>();
				component2.mTouch = this.mTouch;
				component2.mPressed = true;
				component2.mDragging = true;
				component2.Start();
				component2.OnClone(base.gameObject);
				component2.OnDragDropStart();
				if (UICamera.currentTouch == null)
				{
					UICamera.currentTouch = this.mTouch;
				}
				this.mTouch = null;
				UICamera.Notify(base.gameObject, "OnPress", false);
				UICamera.Notify(base.gameObject, "OnHover", false);
				return;
			}
			this.mDragging = true;
			this.OnDragDropStart();
		}
	}

	protected virtual void OnClone(GameObject original)
	{
	}

	protected virtual void OnDrag(Vector2 delta)
	{
		if (!this.interactable)
		{
			return;
		}
		if (!this.mDragging || !base.enabled || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		this.OnDragDropMove(delta * this.mRoot.pixelSizeAdjustment);
	}

	protected virtual void OnDragEnd()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!base.enabled || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		this.StopDragging(UICamera.hoveredObject);
	}

	public void StopDragging(GameObject go)
	{
		if (this.mDragging)
		{
			this.mDragging = false;
			this.OnDragDropRelease(go);
		}
	}

	protected virtual void OnDragDropStart()
	{
		if (!UIDragDropItem.draggedItems.Contains(this))
		{
			UIDragDropItem.draggedItems.Add(this);
		}
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = false;
		}
		if (this.mButton != null)
		{
			this.mButton.isEnabled = false;
		}
		else if (this.mCollider != null)
		{
			this.mCollider.enabled = false;
		}
		else if (this.mCollider2D != null)
		{
			this.mCollider2D.enabled = false;
		}
		this.mParent = this.mTrans.parent;
		this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
		this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
		this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
		if (UIDragDropRoot.root != null)
		{
			this.mTrans.parent = UIDragDropRoot.root;
		}
		Vector3 localPosition = this.mTrans.localPosition;
		localPosition.z = 0f;
		this.mTrans.localPosition = localPosition;
		TweenPosition component = base.GetComponent<TweenPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
		SpringPosition component2 = base.GetComponent<SpringPosition>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
		NGUITools.MarkParentAsChanged(base.gameObject);
		if (this.mTable != null)
		{
			this.mTable.repositionNow = true;
		}
		if (this.mGrid != null)
		{
			this.mGrid.repositionNow = true;
		}
	}

	protected virtual void OnDragDropMove(Vector2 delta)
	{
		this.mTrans.localPosition += delta;
	}

	protected virtual void OnDragDropRelease(GameObject surface)
	{
		if (!this.cloneOnDrag)
		{
			if (this.mButton != null)
			{
				this.mButton.isEnabled = true;
			}
			else if (this.mCollider != null)
			{
				this.mCollider.enabled = true;
			}
			else if (this.mCollider2D != null)
			{
				this.mCollider2D.enabled = true;
			}
			UIDragDropContainer uIDragDropContainer = surface ? NGUITools.FindInParents<UIDragDropContainer>(surface) : null;
			if (uIDragDropContainer != null)
			{
				this.mTrans.parent = ((uIDragDropContainer.reparentTarget != null) ? uIDragDropContainer.reparentTarget : uIDragDropContainer.transform);
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.z = 0f;
				this.mTrans.localPosition = localPosition;
			}
			else
			{
				this.mTrans.parent = this.mParent;
			}
			this.mParent = this.mTrans.parent;
			this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
			this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
			if (this.mDragScrollView != null)
			{
				base.StartCoroutine(this.EnableDragScrollView());
			}
			NGUITools.MarkParentAsChanged(base.gameObject);
			if (this.mTable != null)
			{
				this.mTable.repositionNow = true;
			}
			if (this.mGrid != null)
			{
				this.mGrid.repositionNow = true;
			}
		}
		else
		{
			NGUITools.Destroy(base.gameObject);
		}
		this.OnDragDropEnd();
	}

	protected virtual void OnDragDropEnd()
	{
		UIDragDropItem.draggedItems.Remove(this);
	}

	[IteratorStateMachine(typeof(UIDragDropItem.<EnableDragScrollView>d__35))]
	protected IEnumerator EnableDragScrollView()
	{
		yield return new WaitForEndOfFrame();
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = true;
		}
		yield break;
	}

	public UIDragDropItem()
	{
		this.pressAndHoldDelay = 1f;
		this.interactable = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32((int)this.restriction);
		SerializedStateWriter.Instance.WriteBoolean(this.cloneOnDrag);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.pressAndHoldDelay);
		SerializedStateWriter.Instance.WriteBoolean(this.interactable);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		this.restriction = (UIDragDropItem.Restriction)SerializedStateReader.Instance.ReadInt32();
		this.cloneOnDrag = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.pressAndHoldDelay = SerializedStateReader.Instance.ReadSingle();
		this.interactable = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = (int)this.restriction;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 503);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.cloneOnDrag, &var_0_cp_0[var_0_cp_1] + 515);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.pressAndHoldDelay, &var_0_cp_0[var_0_cp_1] + 527);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.interactable, &var_0_cp_0[var_0_cp_1] + 545);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.restriction = (UIDragDropItem.Restriction)arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 503);
		this.cloneOnDrag = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 515);
		SerializedNamedStateReader.Instance.Align();
		this.pressAndHoldDelay = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 527);
		this.interactable = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 545);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIDragDropItem(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UIDragDropItem)instance).cloneOnDrag;
	}

	public static void $Set0(object instance, bool value)
	{
		((UIDragDropItem)instance).cloneOnDrag = value;
	}

	public static float $Get1(object instance)
	{
		return ((UIDragDropItem)instance).pressAndHoldDelay;
	}

	public static void $Set1(object instance, float value)
	{
		((UIDragDropItem)instance).pressAndHoldDelay = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UIDragDropItem)instance).interactable;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIDragDropItem)instance).interactable = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).EnableDragScrollView());
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnClone((GameObject)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnDragDropEnd();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnDragDropMove(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnDragDropRelease((GameObject)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnDragDropStart();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnDragEnd();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnDragStart();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).StartDragging();
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).StopDragging((GameObject)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIDragDropItem)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
