using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Scroll View"), ExecuteInEditMode, RequireComponent(typeof(UIPanel))]
public class UIScrollView : MonoBehaviour, IUnitySerializable
{
	public enum Movement
	{
		Horizontal,
		Vertical,
		Unrestricted,
		Custom
	}

	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}

	public enum ShowCondition
	{
		Always,
		OnlyIfNeeded,
		WhenDragging
	}

	public delegate void OnDragNotification();

	public static BetterList<UIScrollView> list = new BetterList<UIScrollView>();

	public UIScrollView.Movement movement;

	public UIScrollView.DragEffect dragEffect;

	public bool restrictWithinPanel;

	public bool disableDragIfFits;

	public bool smoothDragStart;

	public bool iOSDragEmulation;

	public float scrollWheelFactor;

	public float momentumAmount;

	public float dampenStrength;

	public UIProgressBar horizontalScrollBar;

	public UIProgressBar verticalScrollBar;

	public UIScrollView.ShowCondition showScrollBars;

	public Vector2 customMovement;

	public UIWidget.Pivot contentPivot;

	public UIScrollView.OnDragNotification onDragStarted;

	public UIScrollView.OnDragNotification onDragFinished;

	public UIScrollView.OnDragNotification onMomentumMove;

	public UIScrollView.OnDragNotification onStoppedMoving;

	[HideInInspector, SerializeField]
	protected internal Vector3 scale;

	[HideInInspector, SerializeField]
	protected internal Vector2 relativePositionOnReset;

	protected Transform mTrans;

	protected UIPanel mPanel;

	protected Plane mPlane;

	protected Vector3 mLastPos;

	protected bool mPressed;

	protected Vector3 mMomentum;

	protected float mScroll;

	protected Bounds mBounds;

	protected bool mCalculatedBounds;

	protected bool mShouldMove;

	protected bool mIgnoreCallbacks;

	protected int mDragID;

	protected Vector2 mDragStartOffset;

	protected bool mDragStarted;

	[System.NonSerialized]
	private bool mStarted;

	[HideInInspector]
	public UICenterOnChild centerOnChild;

	public UIPanel panel
	{
		get
		{
			return this.mPanel;
		}
	}

	public bool isDragging
	{
		get
		{
			return this.mPressed && this.mDragStarted;
		}
	}

	public virtual Bounds bounds
	{
		get
		{
			if (!this.mCalculatedBounds)
			{
				this.mCalculatedBounds = true;
				this.mTrans = base.transform;
				this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
			}
			return this.mBounds;
		}
	}

	public bool canMoveHorizontally
	{
		get
		{
			return this.movement == UIScrollView.Movement.Horizontal || this.movement == UIScrollView.Movement.Unrestricted || (this.movement == UIScrollView.Movement.Custom && this.customMovement.x != 0f);
		}
	}

	public bool canMoveVertically
	{
		get
		{
			return this.movement == UIScrollView.Movement.Vertical || this.movement == UIScrollView.Movement.Unrestricted || (this.movement == UIScrollView.Movement.Custom && this.customMovement.y != 0f);
		}
	}

	public virtual bool shouldMoveHorizontally
	{
		get
		{
			float num = this.bounds.size.x;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.x * 2f;
			}
			return Mathf.RoundToInt(num - this.mPanel.width) > 0;
		}
	}

	public virtual bool shouldMoveVertically
	{
		get
		{
			float num = this.bounds.size.y;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.y * 2f;
			}
			return Mathf.RoundToInt(num - this.mPanel.height) > 0;
		}
	}

	protected virtual bool shouldMove
	{
		get
		{
			if (!this.disableDragIfFits)
			{
				return true;
			}
			if (this.mPanel == null)
			{
				this.mPanel = base.GetComponent<UIPanel>();
			}
			Vector4 finalClipRegion = this.mPanel.finalClipRegion;
			Bounds bounds = this.bounds;
			float num = (finalClipRegion.z == 0f) ? ((float)Screen.width) : (finalClipRegion.z * 0.5f);
			float num2 = (finalClipRegion.w == 0f) ? ((float)Screen.height) : (finalClipRegion.w * 0.5f);
			if (this.canMoveHorizontally)
			{
				if (bounds.min.x < finalClipRegion.x - num)
				{
					return true;
				}
				if (bounds.max.x > finalClipRegion.x + num)
				{
					return true;
				}
			}
			if (this.canMoveVertically)
			{
				if (bounds.min.y < finalClipRegion.y - num2)
				{
					return true;
				}
				if (bounds.max.y > finalClipRegion.y + num2)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Vector3 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
			this.mShouldMove = true;
		}
	}

	private void Awake()
	{
		this.mTrans = base.transform;
		this.mPanel = base.GetComponent<UIPanel>();
		if (this.mPanel.clipping == UIDrawCall.Clipping.None)
		{
			this.mPanel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
		}
		if (this.movement != UIScrollView.Movement.Custom && this.scale.sqrMagnitude > 0.001f)
		{
			if (this.scale.x == 1f && this.scale.y == 0f)
			{
				this.movement = UIScrollView.Movement.Horizontal;
			}
			else if (this.scale.x == 0f && this.scale.y == 1f)
			{
				this.movement = UIScrollView.Movement.Vertical;
			}
			else if (this.scale.x == 1f && this.scale.y == 1f)
			{
				this.movement = UIScrollView.Movement.Unrestricted;
			}
			else
			{
				this.movement = UIScrollView.Movement.Custom;
				this.customMovement.x = this.scale.x;
				this.customMovement.y = this.scale.y;
			}
			this.scale = Vector3.zero;
		}
		if (this.contentPivot == UIWidget.Pivot.TopLeft && this.relativePositionOnReset != Vector2.zero)
		{
			this.contentPivot = NGUIMath.GetPivot(new Vector2(this.relativePositionOnReset.x, 1f - this.relativePositionOnReset.y));
			this.relativePositionOnReset = Vector2.zero;
		}
	}

	private void OnEnable()
	{
		UIScrollView.list.Add(this);
		if (this.mStarted && Application.isPlaying)
		{
			this.CheckScrollbars();
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (Application.isPlaying)
		{
			this.CheckScrollbars();
		}
	}

	private void CheckScrollbars()
	{
		if (this.horizontalScrollBar != null)
		{
			EventDelegate.Add(this.horizontalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
			this.horizontalScrollBar.BroadcastMessage("CacheDefaultColor", SendMessageOptions.DontRequireReceiver);
			this.horizontalScrollBar.alpha = ((this.showScrollBars == UIScrollView.ShowCondition.Always || this.shouldMoveHorizontally) ? 1f : 0f);
		}
		if (this.verticalScrollBar != null)
		{
			EventDelegate.Add(this.verticalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
			this.verticalScrollBar.BroadcastMessage("CacheDefaultColor", SendMessageOptions.DontRequireReceiver);
			this.verticalScrollBar.alpha = ((this.showScrollBars == UIScrollView.ShowCondition.Always || this.shouldMoveVertically) ? 1f : 0f);
		}
	}

	private void OnDisable()
	{
		UIScrollView.list.Remove(this);
	}

	public bool RestrictWithinBounds(bool instant)
	{
		return this.RestrictWithinBounds(instant, true, true);
	}

	public bool RestrictWithinBounds(bool instant, bool horizontal, bool vertical)
	{
		if (this.mPanel == null)
		{
			return false;
		}
		Bounds bounds = this.bounds;
		Vector3 vector = this.mPanel.CalculateConstrainOffset(bounds.min, bounds.max);
		if (!horizontal)
		{
			vector.x = 0f;
		}
		if (!vertical)
		{
			vector.y = 0f;
		}
		if (vector.sqrMagnitude > 0.1f)
		{
			if (!instant && this.dragEffect == UIScrollView.DragEffect.MomentumAndSpring)
			{
				Vector3 vector2 = this.mTrans.localPosition + vector;
				vector2.x = Mathf.Round(vector2.x);
				vector2.y = Mathf.Round(vector2.y);
				SpringPanel.Begin(this.mPanel.gameObject, vector2, 13f).strength = 8f;
			}
			else
			{
				this.MoveRelative(vector);
				if (Mathf.Abs(vector.x) > 0.01f)
				{
					this.mMomentum.x = 0f;
				}
				if (Mathf.Abs(vector.y) > 0.01f)
				{
					this.mMomentum.y = 0f;
				}
				if (Mathf.Abs(vector.z) > 0.01f)
				{
					this.mMomentum.z = 0f;
				}
				this.mScroll = 0f;
			}
			return true;
		}
		return false;
	}

	public void DisableSpring()
	{
		SpringPanel component = base.GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public void UpdateScrollbars()
	{
		this.UpdateScrollbars(true);
	}

	public virtual void UpdateScrollbars(bool recalculateBounds)
	{
		if (this.mPanel == null)
		{
			return;
		}
		if (this.horizontalScrollBar != null || this.verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				this.mCalculatedBounds = false;
				this.mShouldMove = this.shouldMove;
			}
			Bounds bounds = this.bounds;
			Vector2 vector = bounds.min;
			Vector2 vector2 = bounds.max;
			if (this.horizontalScrollBar != null && vector2.x > vector.x)
			{
				Vector4 finalClipRegion = this.mPanel.finalClipRegion;
				int num = Mathf.RoundToInt(finalClipRegion.z);
				if ((num & 1) != 0)
				{
					num--;
				}
				float num2 = (float)num * 0.5f;
				num2 = Mathf.Round(num2);
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num2 -= this.mPanel.clipSoftness.x;
				}
				float contentSize = vector2.x - vector.x;
				float viewSize = num2 * 2f;
				float num3 = vector.x;
				float num4 = vector2.x;
				float num5 = finalClipRegion.x - num2;
				float num6 = finalClipRegion.x + num2;
				num3 = num5 - num3;
				num4 -= num6;
				this.UpdateScrollbars(this.horizontalScrollBar, num3, num4, contentSize, viewSize, false);
			}
			if (this.verticalScrollBar != null && vector2.y > vector.y)
			{
				Vector4 finalClipRegion2 = this.mPanel.finalClipRegion;
				int num7 = Mathf.RoundToInt(finalClipRegion2.w);
				if ((num7 & 1) != 0)
				{
					num7--;
				}
				float num8 = (float)num7 * 0.5f;
				num8 = Mathf.Round(num8);
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num8 -= this.mPanel.clipSoftness.y;
				}
				float contentSize2 = vector2.y - vector.y;
				float viewSize2 = num8 * 2f;
				float num9 = vector.y;
				float num10 = vector2.y;
				float num11 = finalClipRegion2.y - num8;
				float num12 = finalClipRegion2.y + num8;
				num9 = num11 - num9;
				num10 -= num12;
				this.UpdateScrollbars(this.verticalScrollBar, num9, num10, contentSize2, viewSize2, true);
				return;
			}
		}
		else if (recalculateBounds)
		{
			this.mCalculatedBounds = false;
		}
	}

	protected void UpdateScrollbars(UIProgressBar slider, float contentMin, float contentMax, float contentSize, float viewSize, bool inverted)
	{
		if (slider == null)
		{
			return;
		}
		this.mIgnoreCallbacks = true;
		float num;
		if (viewSize < contentSize)
		{
			contentMin = Mathf.Clamp01(contentMin / contentSize);
			contentMax = Mathf.Clamp01(contentMax / contentSize);
			num = contentMin + contentMax;
			slider.value = (inverted ? ((num > 0.001f) ? (1f - contentMin / num) : 0f) : ((num > 0.001f) ? (contentMin / num) : 1f));
		}
		else
		{
			contentMin = Mathf.Clamp01(-contentMin / contentSize);
			contentMax = Mathf.Clamp01(-contentMax / contentSize);
			num = contentMin + contentMax;
			slider.value = (inverted ? ((num > 0.001f) ? (1f - contentMin / num) : 0f) : ((num > 0.001f) ? (contentMin / num) : 1f));
			if (contentSize > 0f)
			{
				contentMin = Mathf.Clamp01(contentMin / contentSize);
				contentMax = Mathf.Clamp01(contentMax / contentSize);
				num = contentMin + contentMax;
			}
		}
		UIScrollBar uIScrollBar = slider as UIScrollBar;
		if (uIScrollBar != null)
		{
			uIScrollBar.barSize = 1f - num;
		}
		this.mIgnoreCallbacks = false;
	}

	public virtual void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		if (this.mPanel == null)
		{
			this.mPanel = base.GetComponent<UIPanel>();
		}
		this.DisableSpring();
		Bounds bounds = this.bounds;
		if (bounds.min.x == bounds.max.x || bounds.min.y == bounds.max.y)
		{
			return;
		}
		Vector4 finalClipRegion = this.mPanel.finalClipRegion;
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		float num3 = bounds.min.x + num;
		float num4 = bounds.max.x - num;
		float num5 = bounds.min.y + num2;
		float num6 = bounds.max.y - num2;
		if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			num3 -= this.mPanel.clipSoftness.x;
			num4 += this.mPanel.clipSoftness.x;
			num5 -= this.mPanel.clipSoftness.y;
			num6 += this.mPanel.clipSoftness.y;
		}
		float num7 = Mathf.Lerp(num3, num4, x);
		float num8 = Mathf.Lerp(num6, num5, y);
		if (!updateScrollbars)
		{
			Vector3 localPosition = this.mTrans.localPosition;
			if (this.canMoveHorizontally)
			{
				localPosition.x += finalClipRegion.x - num7;
			}
			if (this.canMoveVertically)
			{
				localPosition.y += finalClipRegion.y - num8;
			}
			this.mTrans.localPosition = localPosition;
		}
		if (this.canMoveHorizontally)
		{
			finalClipRegion.x = num7;
		}
		if (this.canMoveVertically)
		{
			finalClipRegion.y = num8;
		}
		Vector4 baseClipRegion = this.mPanel.baseClipRegion;
		this.mPanel.clipOffset = new Vector2(finalClipRegion.x - baseClipRegion.x, finalClipRegion.y - baseClipRegion.y);
		if (updateScrollbars)
		{
			this.UpdateScrollbars(this.mDragID == -10);
		}
	}

	public void InvalidateBounds()
	{
		this.mCalculatedBounds = false;
	}

	[ContextMenu("Reset Clipping Position")]
	public void ResetPosition()
	{
		if (NGUITools.GetActive(this))
		{
			this.mCalculatedBounds = false;
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
			this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, false);
			this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, true);
		}
	}

	public void UpdatePosition()
	{
		if (!this.mIgnoreCallbacks && (this.horizontalScrollBar != null || this.verticalScrollBar != null))
		{
			this.mIgnoreCallbacks = true;
			this.mCalculatedBounds = false;
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
			float x = (this.horizontalScrollBar != null) ? this.horizontalScrollBar.value : pivotOffset.x;
			float y = (this.verticalScrollBar != null) ? this.verticalScrollBar.value : (1f - pivotOffset.y);
			this.SetDragAmount(x, y, false);
			this.UpdateScrollbars(true);
			this.mIgnoreCallbacks = false;
		}
	}

	public void OnScrollBar()
	{
		if (!this.mIgnoreCallbacks)
		{
			this.mIgnoreCallbacks = true;
			float x = (this.horizontalScrollBar != null) ? this.horizontalScrollBar.value : 0f;
			float y = (this.verticalScrollBar != null) ? this.verticalScrollBar.value : 0f;
			this.SetDragAmount(x, y, false);
			this.mIgnoreCallbacks = false;
		}
	}

	public virtual void MoveRelative(Vector3 relative)
	{
		this.mTrans.localPosition += relative;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= relative.x;
		clipOffset.y -= relative.y;
		this.mPanel.clipOffset = clipOffset;
		this.UpdateScrollbars(false);
	}

	public void MoveAbsolute(Vector3 absolute)
	{
		Vector3 a = this.mTrans.InverseTransformPoint(absolute);
		Vector3 b = this.mTrans.InverseTransformPoint(Vector3.zero);
		this.MoveRelative(a - b);
	}

	public void Press(bool pressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		if (this.smoothDragStart & pressed)
		{
			this.mDragStarted = false;
			this.mDragStartOffset = Vector2.zero;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (!pressed && this.mDragID == UICamera.currentTouchID)
			{
				this.mDragID = -10;
			}
			this.mCalculatedBounds = false;
			this.mShouldMove = this.shouldMove;
			if (!this.mShouldMove)
			{
				return;
			}
			this.mPressed = pressed;
			if (pressed)
			{
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
				this.DisableSpring();
				this.mLastPos = UICamera.lastWorldPosition;
				this.mPlane = new Plane(this.mTrans.rotation * Vector3.back, this.mLastPos);
				Vector2 clipOffset = this.mPanel.clipOffset;
				clipOffset.x = Mathf.Round(clipOffset.x);
				clipOffset.y = Mathf.Round(clipOffset.y);
				this.mPanel.clipOffset = clipOffset;
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.x = Mathf.Round(localPosition.x);
				localPosition.y = Mathf.Round(localPosition.y);
				this.mTrans.localPosition = localPosition;
				if (!this.smoothDragStart)
				{
					this.mDragStarted = true;
					this.mDragStartOffset = Vector2.zero;
					if (this.onDragStarted != null)
					{
						this.onDragStarted();
						return;
					}
				}
			}
			else
			{
				if (this.centerOnChild)
				{
					this.centerOnChild.Recenter();
					return;
				}
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					this.RestrictWithinBounds(this.dragEffect == UIScrollView.DragEffect.None, this.canMoveHorizontally, this.canMoveVertically);
				}
				if (this.mDragStarted && this.onDragFinished != null)
				{
					this.onDragFinished();
				}
				if (!this.mShouldMove && this.onStoppedMoving != null)
				{
					this.onStoppedMoving();
				}
			}
		}
	}

	public void Drag()
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mShouldMove)
		{
			if (this.mDragID == -10)
			{
				this.mDragID = UICamera.currentTouchID;
			}
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			if (this.smoothDragStart && !this.mDragStarted)
			{
				this.mDragStarted = true;
				this.mDragStartOffset = UICamera.currentTouch.totalDelta;
				if (this.onDragStarted != null)
				{
					this.onDragStarted();
				}
			}
			Ray ray = this.smoothDragStart ? UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos - this.mDragStartOffset) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float distance = 0f;
			if (this.mPlane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (vector.x != 0f || vector.y != 0f || vector.z != 0f)
				{
					vector = this.mTrans.InverseTransformDirection(vector);
					if (this.movement == UIScrollView.Movement.Horizontal)
					{
						vector.y = 0f;
						vector.z = 0f;
					}
					else if (this.movement == UIScrollView.Movement.Vertical)
					{
						vector.x = 0f;
						vector.z = 0f;
					}
					else if (this.movement == UIScrollView.Movement.Unrestricted)
					{
						vector.z = 0f;
					}
					else
					{
						vector.Scale(this.customMovement);
					}
					vector = this.mTrans.TransformDirection(vector);
				}
				if (this.dragEffect == UIScrollView.DragEffect.None)
				{
					this.mMomentum = Vector3.zero;
				}
				else
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				if (!this.iOSDragEmulation || this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.MoveAbsolute(vector);
				}
				else if (this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max).magnitude > 1f)
				{
					this.MoveAbsolute(vector * 0.5f);
					this.mMomentum *= 0.5f;
				}
				else
				{
					this.MoveAbsolute(vector);
				}
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.RestrictWithinBounds(true, this.canMoveHorizontally, this.canMoveVertically);
				}
			}
		}
	}

	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.scrollWheelFactor != 0f)
		{
			this.DisableSpring();
			this.mShouldMove |= this.shouldMove;
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}

	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		if (this.showScrollBars != UIScrollView.ShowCondition.Always && (this.verticalScrollBar || this.horizontalScrollBar))
		{
			bool flag = false;
			bool flag2 = false;
			if (this.showScrollBars != UIScrollView.ShowCondition.WhenDragging || this.mDragID != -10 || this.mMomentum.magnitude > 0.01f)
			{
				flag = this.shouldMoveVertically;
				flag2 = this.shouldMoveHorizontally;
			}
			if (this.verticalScrollBar)
			{
				float num = this.verticalScrollBar.alpha;
				num += (flag ? (deltaTime * 6f) : (-deltaTime * 3f));
				num = Mathf.Clamp01(num);
				if (this.verticalScrollBar.alpha != num)
				{
					this.verticalScrollBar.alpha = num;
				}
			}
			if (this.horizontalScrollBar)
			{
				float num2 = this.horizontalScrollBar.alpha;
				num2 += (flag2 ? (deltaTime * 6f) : (-deltaTime * 3f));
				num2 = Mathf.Clamp01(num2);
				if (this.horizontalScrollBar.alpha != num2)
				{
					this.horizontalScrollBar.alpha = num2;
				}
			}
		}
		if (!this.mShouldMove)
		{
			return;
		}
		if (!this.mPressed)
		{
			if (this.mMomentum.magnitude > 0.0001f || this.mScroll != 0f)
			{
				if (this.movement == UIScrollView.Movement.Horizontal)
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, 0f, 0f));
				}
				else if (this.movement == UIScrollView.Movement.Vertical)
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(0f, this.mScroll * 0.05f, 0f));
				}
				else if (this.movement == UIScrollView.Movement.Unrestricted)
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, this.mScroll * 0.05f, 0f));
				}
				else
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * this.customMovement.x * 0.05f, this.mScroll * this.customMovement.y * 0.05f, 0f));
				}
				this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
				Vector3 absolute = NGUIMath.SpringDampen(ref this.mMomentum, this.dampenStrength, deltaTime);
				this.MoveAbsolute(absolute);
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					if (NGUITools.GetActive(this.centerOnChild))
					{
						if (this.centerOnChild.nextPageThreshold != 0f)
						{
							this.mMomentum = Vector3.zero;
							this.mScroll = 0f;
						}
						else
						{
							this.centerOnChild.Recenter();
						}
					}
					else
					{
						this.RestrictWithinBounds(false, this.canMoveHorizontally, this.canMoveVertically);
					}
				}
				if (this.onMomentumMove != null)
				{
					this.onMomentumMove();
					return;
				}
			}
			else
			{
				this.mScroll = 0f;
				this.mMomentum = Vector3.zero;
				SpringPanel component = base.GetComponent<SpringPanel>();
				if (component != null && component.enabled)
				{
					return;
				}
				this.mShouldMove = false;
				if (this.onStoppedMoving != null)
				{
					this.onStoppedMoving();
					return;
				}
			}
		}
		else
		{
			this.mScroll = 0f;
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
		}
	}

	public void OnPan(Vector2 delta)
	{
		if (this.horizontalScrollBar != null)
		{
			this.horizontalScrollBar.OnPan(delta);
		}
		if (this.verticalScrollBar != null)
		{
			this.verticalScrollBar.OnPan(delta);
		}
		if (this.horizontalScrollBar == null && this.verticalScrollBar == null)
		{
			if (this.scale.x != 0f)
			{
				this.Scroll(delta.x);
				return;
			}
			if (this.scale.y != 0f)
			{
				this.Scroll(delta.y);
			}
		}
	}

	public UIScrollView()
	{
		this.dragEffect = UIScrollView.DragEffect.MomentumAndSpring;
		this.restrictWithinPanel = true;
		this.smoothDragStart = true;
		this.iOSDragEmulation = true;
		this.scrollWheelFactor = 0.25f;
		this.momentumAmount = 35f;
		this.dampenStrength = 9f;
		this.showScrollBars = UIScrollView.ShowCondition.OnlyIfNeeded;
		this.customMovement = new Vector2(1f, 0f);
		this.scale = new Vector3(1f, 0f, 0f);
		this.relativePositionOnReset = Vector2.zero;
		this.mMomentum = Vector3.zero;
		this.mDragID = -10;
		this.mDragStartOffset = Vector2.zero;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32((int)this.movement);
		SerializedStateWriter.Instance.WriteInt32((int)this.dragEffect);
		SerializedStateWriter.Instance.WriteBoolean(this.restrictWithinPanel);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.disableDragIfFits);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.smoothDragStart);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.iOSDragEmulation);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.scrollWheelFactor);
		SerializedStateWriter.Instance.WriteSingle(this.momentumAmount);
		SerializedStateWriter.Instance.WriteSingle(this.dampenStrength);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.horizontalScrollBar);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.verticalScrollBar);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.showScrollBars);
		if (depth <= 7)
		{
			this.customMovement.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.contentPivot);
		if (depth <= 7)
		{
			this.scale.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.relativePositionOnReset.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.centerOnChild);
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		this.movement = (UIScrollView.Movement)SerializedStateReader.Instance.ReadInt32();
		this.dragEffect = (UIScrollView.DragEffect)SerializedStateReader.Instance.ReadInt32();
		this.restrictWithinPanel = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.disableDragIfFits = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.smoothDragStart = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.iOSDragEmulation = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.scrollWheelFactor = SerializedStateReader.Instance.ReadSingle();
		this.momentumAmount = SerializedStateReader.Instance.ReadSingle();
		this.dampenStrength = SerializedStateReader.Instance.ReadSingle();
		if (depth <= 7)
		{
			this.horizontalScrollBar = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIProgressBar);
		}
		if (depth <= 7)
		{
			this.verticalScrollBar = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIProgressBar);
		}
		this.showScrollBars = (UIScrollView.ShowCondition)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.customMovement.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.contentPivot = (UIWidget.Pivot)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.scale.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.relativePositionOnReset.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.centerOnChild = (SerializedStateReader.Instance.ReadUnityEngineObject() as UICenterOnChild);
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.horizontalScrollBar != null)
		{
			this.horizontalScrollBar = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.horizontalScrollBar) as UIProgressBar);
		}
		if (this.verticalScrollBar != null)
		{
			this.verticalScrollBar = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.verticalScrollBar) as UIProgressBar);
		}
		if (this.centerOnChild != null)
		{
			this.centerOnChild = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.centerOnChild) as UICenterOnChild);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = (int)this.movement;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 1678);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.dragEffect, &var_0_cp_0[var_0_cp_1] + 596);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.restrictWithinPanel, &var_0_cp_0[var_0_cp_1] + 665);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.disableDragIfFits, &var_0_cp_0[var_0_cp_1] + 1687);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.smoothDragStart, &var_0_cp_0[var_0_cp_1] + 607);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.iOSDragEmulation, &var_0_cp_0[var_0_cp_1] + 1705);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.scrollWheelFactor, &var_0_cp_0[var_0_cp_1] + 578);
		SerializedNamedStateWriter.Instance.WriteSingle(this.momentumAmount, &var_0_cp_0[var_0_cp_1] + 623);
		SerializedNamedStateWriter.Instance.WriteSingle(this.dampenStrength, &var_0_cp_0[var_0_cp_1] + 1722);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.horizontalScrollBar, &var_0_cp_0[var_0_cp_1] + 1737);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.verticalScrollBar, &var_0_cp_0[var_0_cp_1] + 1757);
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.showScrollBars, &var_0_cp_0[var_0_cp_1] + 1775);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1790);
			this.customMovement.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.contentPivot, &var_0_cp_0[var_0_cp_1] + 1805);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 572);
			this.scale.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1818);
			this.relativePositionOnReset.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.centerOnChild, &var_0_cp_0[var_0_cp_1] + 1842);
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.movement = (UIScrollView.Movement)arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1678);
		this.dragEffect = (UIScrollView.DragEffect)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 596);
		this.restrictWithinPanel = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 665);
		SerializedNamedStateReader.Instance.Align();
		this.disableDragIfFits = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1687);
		SerializedNamedStateReader.Instance.Align();
		this.smoothDragStart = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 607);
		SerializedNamedStateReader.Instance.Align();
		this.iOSDragEmulation = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1705);
		SerializedNamedStateReader.Instance.Align();
		this.scrollWheelFactor = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 578);
		this.momentumAmount = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 623);
		this.dampenStrength = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1722);
		if (depth <= 7)
		{
			this.horizontalScrollBar = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1737) as UIProgressBar);
		}
		if (depth <= 7)
		{
			this.verticalScrollBar = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1757) as UIProgressBar);
		}
		this.showScrollBars = (UIScrollView.ShowCondition)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1775);
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1790);
			this.customMovement.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.contentPivot = (UIWidget.Pivot)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1805);
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 572);
			this.scale.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1818);
			this.relativePositionOnReset.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.centerOnChild = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1842) as UICenterOnChild);
		}
	}

	protected internal UIScrollView(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UIScrollView)instance).restrictWithinPanel;
	}

	public static void $Set0(object instance, bool value)
	{
		((UIScrollView)instance).restrictWithinPanel = value;
	}

	public static bool $Get1(object instance)
	{
		return ((UIScrollView)instance).disableDragIfFits;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIScrollView)instance).disableDragIfFits = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UIScrollView)instance).smoothDragStart;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIScrollView)instance).smoothDragStart = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UIScrollView)instance).iOSDragEmulation;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIScrollView)instance).iOSDragEmulation = value;
	}

	public static float $Get4(object instance)
	{
		return ((UIScrollView)instance).scrollWheelFactor;
	}

	public static void $Set4(object instance, float value)
	{
		((UIScrollView)instance).scrollWheelFactor = value;
	}

	public static float $Get5(object instance)
	{
		return ((UIScrollView)instance).momentumAmount;
	}

	public static void $Set5(object instance, float value)
	{
		((UIScrollView)instance).momentumAmount = value;
	}

	public static float $Get6(object instance)
	{
		return ((UIScrollView)instance).dampenStrength;
	}

	public static void $Set6(object instance, float value)
	{
		((UIScrollView)instance).dampenStrength = value;
	}

	public static long $Get7(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)instance).horizontalScrollBar);
	}

	public static void $Set7(object instance, long value)
	{
		((UIScrollView)instance).horizontalScrollBar = (UIProgressBar)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get8(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)instance).verticalScrollBar);
	}

	public static void $Set8(object instance, long value)
	{
		((UIScrollView)instance).verticalScrollBar = (UIProgressBar)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get9(object instance, int index)
	{
		UIScrollView expr_06_cp_0 = (UIScrollView)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.customMovement.x;
		case 1:
			return expr_06_cp_0.customMovement.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set9(object instance, float value, int index)
	{
		UIScrollView expr_06_cp_0 = (UIScrollView)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.customMovement.x = value;
			return;
		case 1:
			expr_06_cp_0.customMovement.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get10(object instance, int index)
	{
		UIScrollView expr_06_cp_0 = (UIScrollView)instance;
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

	public static void $Set10(object instance, float value, int index)
	{
		UIScrollView expr_06_cp_0 = (UIScrollView)instance;
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

	public static float $Get11(object instance, int index)
	{
		UIScrollView expr_06_cp_0 = (UIScrollView)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.relativePositionOnReset.x;
		case 1:
			return expr_06_cp_0.relativePositionOnReset.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set11(object instance, float value, int index)
	{
		UIScrollView expr_06_cp_0 = (UIScrollView)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.relativePositionOnReset.x = value;
			return;
		case 1:
			expr_06_cp_0.relativePositionOnReset.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static long $Get12(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)instance).centerOnChild);
	}

	public static void $Set12(object instance, long value)
	{
		((UIScrollView)instance).centerOnChild = (UICenterOnChild)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).CheckScrollbars();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).DisableSpring();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Drag();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).bounds);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).canMoveHorizontally);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).canMoveVertically);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).currentMomentum);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).isDragging);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).panel);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).shouldMove);
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).shouldMoveHorizontally);
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).shouldMoveVertically);
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).InvalidateBounds();
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).MoveAbsolute(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).MoveRelative(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).OnPan(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).OnScrollBar();
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Press(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).ResetPosition();
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).RestrictWithinBounds(*(sbyte*)args != 0));
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).RestrictWithinBounds(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0));
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Scroll(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).currentMomentum = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).SetDragAmount(*(float*)args, *(float*)(args + 1), *(sbyte*)(args + 2) != 0);
		return -1L;
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).UpdatePosition();
		return -1L;
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollbars();
		return -1L;
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollbars(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		((UIScrollView)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollbars((UIProgressBar)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3), *(float*)(args + 4), *(sbyte*)(args + 5) != 0);
		return -1L;
	}
}
