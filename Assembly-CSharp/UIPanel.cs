using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/NGUI Panel"), ExecuteInEditMode]
public class UIPanel : UIRect, IUnitySerializable
{
	public enum RenderQueue
	{
		Automatic,
		StartAt,
		Explicit
	}

	public delegate void OnGeometryUpdated();

	public delegate void OnClippingMoved(UIPanel panel);

	public static List<UIPanel> list = new List<UIPanel>();

	public UIPanel.OnGeometryUpdated onGeometryUpdated;

	public bool showInPanelTool;

	public bool generateNormals;

	public bool widgetsAreStatic;

	public bool cullWhileDragging;

	public bool alwaysOnScreen;

	public bool anchorOffset;

	public bool softBorderPadding;

	public UIPanel.RenderQueue renderQueue;

	public int startingRenderQueue;

	[System.NonSerialized]
	public List<UIWidget> widgets;

	[System.NonSerialized]
	public List<UIDrawCall> drawCalls;

	[System.NonSerialized]
	public Matrix4x4 worldToLocal;

	[System.NonSerialized]
	public Vector4 drawCallClipRange;

	public UIPanel.OnClippingMoved onClipMove;

	[HideInInspector, SerializeField]
	protected internal Texture2D mClipTexture;

	[HideInInspector, SerializeField]
	protected internal float mAlpha;

	[HideInInspector, SerializeField]
	protected internal UIDrawCall.Clipping mClipping;

	[HideInInspector, SerializeField]
	protected internal Vector4 mClipRange;

	[HideInInspector, SerializeField]
	protected internal Vector2 mClipSoftness;

	[HideInInspector, SerializeField]
	protected internal int mDepth;

	[HideInInspector, SerializeField]
	protected internal int mSortingOrder;

	private bool mRebuild;

	private bool mResized;

	[SerializeField]
	protected internal Vector2 mClipOffset;

	private int mMatrixFrame;

	private int mAlphaFrameID;

	private int mLayer;

	private static float[] mTemp = new float[4];

	private Vector2 mMin;

	private Vector2 mMax;

	private bool mHalfPixelOffset;

	private bool mSortWidgets;

	private bool mUpdateScroll;

	private UIPanel mParentPanel;

	private static Vector3[] mCorners = new Vector3[4];

	private static int mUpdateFrame = -1;

	private UIDrawCall.OnRenderCallback mOnRender;

	private bool mForced;

	public static int nextUnusedDepth
	{
		get
		{
			int num = -2147483648;
			int i = 0;
			int count = UIPanel.list.Count;
			while (i < count)
			{
				num = Mathf.Max(num, UIPanel.list[i].depth);
				i++;
			}
			if (num != -2147483648)
			{
				return num + 1;
			}
			return 0;
		}
	}

	public override bool canBeAnchored
	{
		get
		{
			return this.mClipping > UIDrawCall.Clipping.None;
		}
	}

	public override float alpha
	{
		get
		{
			return this.mAlpha;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mAlpha != num)
			{
				this.mAlphaFrameID = -1;
				this.mResized = true;
				this.mAlpha = num;
				this.SetDirty();
			}
		}
	}

	public int depth
	{
		get
		{
			return this.mDepth;
		}
		set
		{
			if (this.mDepth != value)
			{
				this.mDepth = value;
				UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
			}
		}
	}

	public int sortingOrder
	{
		get
		{
			return this.mSortingOrder;
		}
		set
		{
			if (this.mSortingOrder != value)
			{
				this.mSortingOrder = value;
				this.UpdateDrawCalls();
			}
		}
	}

	public float width
	{
		get
		{
			return this.GetViewSize().x;
		}
	}

	public float height
	{
		get
		{
			return this.GetViewSize().y;
		}
	}

	public bool halfPixelOffset
	{
		get
		{
			return this.mHalfPixelOffset;
		}
	}

	public bool usedForUI
	{
		get
		{
			return base.anchorCamera != null && this.mCam.orthographic;
		}
	}

	public Vector3 drawCallOffset
	{
		get
		{
			if (base.anchorCamera != null && this.mCam.orthographic)
			{
				Vector2 windowSize = this.GetWindowSize();
				float num = (base.root != null) ? base.root.pixelSizeAdjustment : 1f;
				float num2 = num / windowSize.y / this.mCam.orthographicSize;
				bool flag = this.mHalfPixelOffset;
				bool flag2 = this.mHalfPixelOffset;
				if ((Mathf.RoundToInt(windowSize.x) & 1) == 1)
				{
					flag = !flag;
				}
				if ((Mathf.RoundToInt(windowSize.y) & 1) == 1)
				{
					flag2 = !flag2;
				}
				return new Vector3(flag ? (-num2) : 0f, flag2 ? num2 : 0f);
			}
			return Vector3.zero;
		}
	}

	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mResized = true;
				this.mClipping = value;
				this.mMatrixFrame = -1;
			}
		}
	}

	public UIPanel parentPanel
	{
		get
		{
			return this.mParentPanel;
		}
	}

	public int clipCount
	{
		get
		{
			int num = 0;
			UIPanel uIPanel = this;
			while (uIPanel != null)
			{
				if (uIPanel.mClipping == UIDrawCall.Clipping.SoftClip || uIPanel.mClipping == UIDrawCall.Clipping.TextureMask)
				{
					num++;
				}
				uIPanel = uIPanel.mParentPanel;
			}
			return num;
		}
	}

	public bool hasClipping
	{
		get
		{
			return this.mClipping == UIDrawCall.Clipping.SoftClip || this.mClipping == UIDrawCall.Clipping.TextureMask;
		}
	}

	public bool hasCumulativeClipping
	{
		get
		{
			return this.clipCount != 0;
		}
	}

	[Obsolete("Use 'hasClipping' or 'hasCumulativeClipping' instead")]
	public bool clipsChildren
	{
		get
		{
			return this.hasCumulativeClipping;
		}
	}

	public Vector2 clipOffset
	{
		get
		{
			return this.mClipOffset;
		}
		set
		{
			if (Mathf.Abs(this.mClipOffset.x - value.x) > 0.001f || Mathf.Abs(this.mClipOffset.y - value.y) > 0.001f)
			{
				this.mClipOffset = value;
				this.InvalidateClipping();
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	public Texture2D clipTexture
	{
		get
		{
			return this.mClipTexture;
		}
		set
		{
			if (this.mClipTexture != value)
			{
				this.mClipTexture = value;
			}
		}
	}

	[Obsolete("Use 'finalClipRegion' or 'baseClipRegion' instead")]
	public Vector4 clipRange
	{
		get
		{
			return this.baseClipRegion;
		}
		set
		{
			this.baseClipRegion = value;
		}
	}

	public Vector4 baseClipRegion
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			if (Mathf.Abs(this.mClipRange.x - value.x) > 0.001f || Mathf.Abs(this.mClipRange.y - value.y) > 0.001f || Mathf.Abs(this.mClipRange.z - value.z) > 0.001f || Mathf.Abs(this.mClipRange.w - value.w) > 0.001f)
			{
				this.mResized = true;
				this.mClipRange = value;
				this.mMatrixFrame = -1;
				UIScrollView component = base.GetComponent<UIScrollView>();
				if (component != null)
				{
					component.UpdatePosition();
				}
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	public Vector4 finalClipRegion
	{
		get
		{
			Vector2 viewSize = this.GetViewSize();
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				return new Vector4(this.mClipRange.x + this.mClipOffset.x, this.mClipRange.y + this.mClipOffset.y, viewSize.x, viewSize.y);
			}
			return new Vector4(0f, 0f, viewSize.x, viewSize.y);
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoftness;
		}
		set
		{
			if (this.mClipSoftness != value)
			{
				this.mClipSoftness = value;
			}
		}
	}

	public override Vector3[] localCorners
	{
		get
		{
			if (this.mClipping == UIDrawCall.Clipping.None)
			{
				Vector3[] worldCorners = this.worldCorners;
				Transform cachedTransform = base.cachedTransform;
				for (int i = 0; i < 4; i++)
				{
					worldCorners[i] = cachedTransform.InverseTransformPoint(worldCorners[i]);
				}
				return worldCorners;
			}
			float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
			float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
			float x = num + this.mClipRange.z;
			float y = num2 + this.mClipRange.w;
			UIPanel.mCorners[0] = new Vector3(num, num2);
			UIPanel.mCorners[1] = new Vector3(num, y);
			UIPanel.mCorners[2] = new Vector3(x, y);
			UIPanel.mCorners[3] = new Vector3(x, num2);
			return UIPanel.mCorners;
		}
	}

	public override Vector3[] worldCorners
	{
		get
		{
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
				float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
				float x = num + this.mClipRange.z;
				float y = num2 + this.mClipRange.w;
				Transform cachedTransform = base.cachedTransform;
				UIPanel.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
				UIPanel.mCorners[1] = cachedTransform.TransformPoint(num, y, 0f);
				UIPanel.mCorners[2] = cachedTransform.TransformPoint(x, y, 0f);
				UIPanel.mCorners[3] = cachedTransform.TransformPoint(x, num2, 0f);
			}
			else
			{
				if (base.anchorCamera != null)
				{
					return this.mCam.GetWorldCorners(base.cameraRayDistance);
				}
				Vector2 viewSize = this.GetViewSize();
				float num3 = -0.5f * viewSize.x;
				float num4 = -0.5f * viewSize.y;
				float x2 = num3 + viewSize.x;
				float y2 = num4 + viewSize.y;
				UIPanel.mCorners[0] = new Vector3(num3, num4);
				UIPanel.mCorners[1] = new Vector3(num3, y2);
				UIPanel.mCorners[2] = new Vector3(x2, y2);
				UIPanel.mCorners[3] = new Vector3(x2, num4);
				if (this.anchorOffset && (this.mCam == null || this.mCam.transform.parent != base.cachedTransform))
				{
					Vector3 position = base.cachedTransform.position;
					for (int i = 0; i < 4; i++)
					{
						UIPanel.mCorners[i] += position;
					}
				}
			}
			return UIPanel.mCorners;
		}
	}

	public static int CompareFunc(UIPanel a, UIPanel b)
	{
		if (!(a != b) || !(a != null) || !(b != null))
		{
			return 0;
		}
		if (a.mDepth < b.mDepth)
		{
			return -1;
		}
		if (a.mDepth > b.mDepth)
		{
			return 1;
		}
		if (a.GetInstanceID() >= b.GetInstanceID())
		{
			return 1;
		}
		return -1;
	}

	private void InvalidateClipping()
	{
		this.mResized = true;
		this.mMatrixFrame = -1;
		int i = 0;
		int count = UIPanel.list.Count;
		while (i < count)
		{
			UIPanel uIPanel = UIPanel.list[i];
			if (uIPanel != this && uIPanel.parentPanel == this)
			{
				uIPanel.InvalidateClipping();
			}
			i++;
		}
	}

	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
			float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
			float num3 = num + this.mClipRange.z;
			float num4 = num2 + this.mClipRange.w;
			float x = (num + num3) * 0.5f;
			float y = (num2 + num4) * 0.5f;
			Transform cachedTransform = base.cachedTransform;
			UIRect.mSides[0] = cachedTransform.TransformPoint(num, y, 0f);
			UIRect.mSides[1] = cachedTransform.TransformPoint(x, num4, 0f);
			UIRect.mSides[2] = cachedTransform.TransformPoint(num3, y, 0f);
			UIRect.mSides[3] = cachedTransform.TransformPoint(x, num2, 0f);
			if (relativeTo != null)
			{
				for (int i = 0; i < 4; i++)
				{
					UIRect.mSides[i] = relativeTo.InverseTransformPoint(UIRect.mSides[i]);
				}
			}
			return UIRect.mSides;
		}
		if (base.anchorCamera != null && this.anchorOffset)
		{
			Vector3[] sides = this.mCam.GetSides(base.cameraRayDistance);
			Vector3 position = base.cachedTransform.position;
			for (int j = 0; j < 4; j++)
			{
				sides[j] += position;
			}
			if (relativeTo != null)
			{
				for (int k = 0; k < 4; k++)
				{
					sides[k] = relativeTo.InverseTransformPoint(sides[k]);
				}
			}
			return sides;
		}
		return base.GetSides(relativeTo);
	}

	public override void Invalidate(bool includeChildren)
	{
		this.mAlphaFrameID = -1;
		base.Invalidate(includeChildren);
	}

	public override float CalculateFinalAlpha(int frameID)
	{
		if (this.mAlphaFrameID != frameID)
		{
			this.mAlphaFrameID = frameID;
			UIRect parent = base.parent;
			this.finalAlpha = ((base.parent != null) ? (parent.CalculateFinalAlpha(frameID) * this.mAlpha) : this.mAlpha);
		}
		return this.finalAlpha;
	}

	public override void SetRect(float x, float y, float width, float height)
	{
		int num = Mathf.FloorToInt(width + 0.5f);
		int num2 = Mathf.FloorToInt(height + 0.5f);
		num = num >> 1 << 1;
		num2 = num2 >> 1 << 1;
		Transform transform = base.cachedTransform;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = Mathf.Floor(x + 0.5f);
		localPosition.y = Mathf.Floor(y + 0.5f);
		if (num < 2)
		{
			num = 2;
		}
		if (num2 < 2)
		{
			num2 = 2;
		}
		this.baseClipRegion = new Vector4(localPosition.x, localPosition.y, (float)num, (float)num2);
		if (base.isAnchored)
		{
			transform = transform.parent;
			if (this.leftAnchor.target)
			{
				this.leftAnchor.SetHorizontal(transform, x);
			}
			if (this.rightAnchor.target)
			{
				this.rightAnchor.SetHorizontal(transform, x + width);
			}
			if (this.bottomAnchor.target)
			{
				this.bottomAnchor.SetVertical(transform, y);
			}
			if (this.topAnchor.target)
			{
				this.topAnchor.SetVertical(transform, y + height);
			}
		}
	}

	public bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this.UpdateTransformMatrix();
		a = this.worldToLocal.MultiplyPoint3x4(a);
		b = this.worldToLocal.MultiplyPoint3x4(b);
		c = this.worldToLocal.MultiplyPoint3x4(c);
		d = this.worldToLocal.MultiplyPoint3x4(d);
		UIPanel.mTemp[0] = a.x;
		UIPanel.mTemp[1] = b.x;
		UIPanel.mTemp[2] = c.x;
		UIPanel.mTemp[3] = d.x;
		float num = Mathf.Min(UIPanel.mTemp);
		float num2 = Mathf.Max(UIPanel.mTemp);
		UIPanel.mTemp[0] = a.y;
		UIPanel.mTemp[1] = b.y;
		UIPanel.mTemp[2] = c.y;
		UIPanel.mTemp[3] = d.y;
		float num3 = Mathf.Min(UIPanel.mTemp);
		float num4 = Mathf.Max(UIPanel.mTemp);
		return num2 >= this.mMin.x && num4 >= this.mMin.y && num <= this.mMax.x && num3 <= this.mMax.y;
	}

	public bool IsVisible(Vector3 worldPos)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None || this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip)
		{
			return true;
		}
		this.UpdateTransformMatrix();
		Vector3 vector = this.worldToLocal.MultiplyPoint3x4(worldPos);
		return vector.x >= this.mMin.x && vector.y >= this.mMin.y && vector.x <= this.mMax.x && vector.y <= this.mMax.y;
	}

	public bool IsVisible(UIWidget w)
	{
		UIPanel uIPanel = this;
		Vector3[] array = null;
		while (uIPanel != null)
		{
			if ((uIPanel.mClipping == UIDrawCall.Clipping.None || uIPanel.mClipping == UIDrawCall.Clipping.ConstrainButDontClip) && !w.hideIfOffScreen)
			{
				uIPanel = uIPanel.mParentPanel;
			}
			else
			{
				if (array == null)
				{
					array = w.worldCorners;
				}
				if (!uIPanel.IsVisible(array[0], array[1], array[2], array[3]))
				{
					return false;
				}
				uIPanel = uIPanel.mParentPanel;
			}
		}
		return true;
	}

	public bool Affects(UIWidget w)
	{
		if (w == null)
		{
			return false;
		}
		UIPanel panel = w.panel;
		if (panel == null)
		{
			return false;
		}
		UIPanel uIPanel = this;
		while (uIPanel != null)
		{
			if (uIPanel == panel)
			{
				return true;
			}
			if (!uIPanel.hasCumulativeClipping)
			{
				return false;
			}
			uIPanel = uIPanel.mParentPanel;
		}
		return false;
	}

	[ContextMenu("Force Refresh")]
	public void RebuildAllDrawCalls()
	{
		this.mRebuild = true;
	}

	public void SetDirty()
	{
		int i = 0;
		int count = this.drawCalls.Count;
		while (i < count)
		{
			this.drawCalls[i].isDirty = true;
			i++;
		}
		this.Invalidate(true);
	}

	protected override void Awake()
	{
		base.Awake();
		this.mHalfPixelOffset = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.XBOX360 || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.WindowsEditor);
		if (this.mHalfPixelOffset && SystemInfo.graphicsDeviceVersion.Contains("Direct3D"))
		{
			this.mHalfPixelOffset = (SystemInfo.graphicsShaderLevel < 40);
		}
	}

	private void FindParent()
	{
		Transform parent = base.cachedTransform.parent;
		this.mParentPanel = ((parent != null) ? NGUITools.FindInParents<UIPanel>(parent.gameObject) : null);
	}

	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		this.FindParent();
	}

	protected override void OnStart()
	{
		this.mLayer = base.cachedGameObject.layer;
	}

	protected override void OnEnable()
	{
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		this.OnStart();
		base.OnEnable();
		this.mMatrixFrame = -1;
	}

	protected override void OnInit()
	{
		if (UIPanel.list.Contains(this))
		{
			return;
		}
		base.OnInit();
		this.FindParent();
		if (base.GetComponent<Rigidbody>() == null && this.mParentPanel == null)
		{
			UICamera uICamera = (base.anchorCamera != null) ? this.mCam.GetComponent<UICamera>() : null;
			if (uICamera != null && (uICamera.eventType == UICamera.EventType.UI_3D || uICamera.eventType == UICamera.EventType.World_3D))
			{
				Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
				rigidbody.isKinematic = true;
				rigidbody.useGravity = false;
			}
		}
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		UIPanel.list.Add(this);
		UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
	}

	protected override void OnDisable()
	{
		int i = 0;
		int count = this.drawCalls.Count;
		while (i < count)
		{
			UIDrawCall uIDrawCall = this.drawCalls[i];
			if (uIDrawCall != null)
			{
				UIDrawCall.Destroy(uIDrawCall);
			}
			i++;
		}
		this.drawCalls.Clear();
		UIPanel.list.Remove(this);
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		if (UIPanel.list.Count == 0)
		{
			UIDrawCall.ReleaseAll();
			UIPanel.mUpdateFrame = -1;
		}
		base.OnDisable();
	}

	private void UpdateTransformMatrix()
	{
		int frameCount = Time.frameCount;
		if (base.cachedTransform.hasChanged)
		{
			this.mTrans.hasChanged = false;
			this.mMatrixFrame = -1;
		}
		if (this.mMatrixFrame != frameCount)
		{
			this.mMatrixFrame = frameCount;
			this.worldToLocal = this.mTrans.worldToLocalMatrix;
			Vector2 vector = this.GetViewSize() * 0.5f;
			float num = this.mClipOffset.x + this.mClipRange.x;
			float num2 = this.mClipOffset.y + this.mClipRange.y;
			this.mMin.x = num - vector.x;
			this.mMin.y = num2 - vector.y;
			this.mMax.x = num + vector.x;
			this.mMax.y = num2 + vector.y;
		}
	}

	protected override void OnAnchor()
	{
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return;
		}
		Transform cachedTransform = base.cachedTransform;
		Transform parent = cachedTransform.parent;
		Vector2 viewSize = this.GetViewSize();
		Vector2 vector = cachedTransform.localPosition;
		float num;
		float num2;
		float num3;
		float num4;
		if (this.leftAnchor.target == this.bottomAnchor.target && this.leftAnchor.target == this.rightAnchor.target && this.leftAnchor.target == this.topAnchor.target)
		{
			Vector3[] sides = this.leftAnchor.GetSides(parent);
			if (sides != null)
			{
				num = NGUIMath.Lerp(sides[0].x, sides[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				num2 = NGUIMath.Lerp(sides[0].x, sides[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				num3 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				num4 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
			}
			else
			{
				Vector2 vector2 = base.GetLocalPos(this.leftAnchor, parent);
				num = vector2.x + (float)this.leftAnchor.absolute;
				num3 = vector2.y + (float)this.bottomAnchor.absolute;
				num2 = vector2.x + (float)this.rightAnchor.absolute;
				num4 = vector2.y + (float)this.topAnchor.absolute;
			}
		}
		else
		{
			if (this.leftAnchor.target)
			{
				Vector3[] sides2 = this.leftAnchor.GetSides(parent);
				if (sides2 != null)
				{
					num = NGUIMath.Lerp(sides2[0].x, sides2[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				}
				else
				{
					num = base.GetLocalPos(this.leftAnchor, parent).x + (float)this.leftAnchor.absolute;
				}
			}
			else
			{
				num = this.mClipRange.x - 0.5f * viewSize.x;
			}
			if (this.rightAnchor.target)
			{
				Vector3[] sides3 = this.rightAnchor.GetSides(parent);
				if (sides3 != null)
				{
					num2 = NGUIMath.Lerp(sides3[0].x, sides3[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				}
				else
				{
					num2 = base.GetLocalPos(this.rightAnchor, parent).x + (float)this.rightAnchor.absolute;
				}
			}
			else
			{
				num2 = this.mClipRange.x + 0.5f * viewSize.x;
			}
			if (this.bottomAnchor.target)
			{
				Vector3[] sides4 = this.bottomAnchor.GetSides(parent);
				if (sides4 != null)
				{
					num3 = NGUIMath.Lerp(sides4[3].y, sides4[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				}
				else
				{
					num3 = base.GetLocalPos(this.bottomAnchor, parent).y + (float)this.bottomAnchor.absolute;
				}
			}
			else
			{
				num3 = this.mClipRange.y - 0.5f * viewSize.y;
			}
			if (this.topAnchor.target)
			{
				Vector3[] sides5 = this.topAnchor.GetSides(parent);
				if (sides5 != null)
				{
					num4 = NGUIMath.Lerp(sides5[3].y, sides5[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
				}
				else
				{
					num4 = base.GetLocalPos(this.topAnchor, parent).y + (float)this.topAnchor.absolute;
				}
			}
			else
			{
				num4 = this.mClipRange.y + 0.5f * viewSize.y;
			}
		}
		num -= vector.x + this.mClipOffset.x;
		num2 -= vector.x + this.mClipOffset.x;
		num3 -= vector.y + this.mClipOffset.y;
		num4 -= vector.y + this.mClipOffset.y;
		float x = Mathf.Lerp(num, num2, 0.5f);
		float y = Mathf.Lerp(num3, num4, 0.5f);
		float num5 = num2 - num;
		float num6 = num4 - num3;
		float num7 = Mathf.Max(2f, this.mClipSoftness.x);
		float num8 = Mathf.Max(2f, this.mClipSoftness.y);
		if (num5 < num7)
		{
			num5 = num7;
		}
		if (num6 < num8)
		{
			num6 = num8;
		}
		this.baseClipRegion = new Vector4(x, y, num5, num6);
	}

	private void LateUpdate()
	{
		if (UIPanel.mUpdateFrame != Time.frameCount)
		{
			UIPanel.mUpdateFrame = Time.frameCount;
			int i = 0;
			int count = UIPanel.list.Count;
			while (i < count)
			{
				UIPanel.list[i].UpdateSelf();
				i++;
			}
			int num = 3000;
			int j = 0;
			int count2 = UIPanel.list.Count;
			while (j < count2)
			{
				UIPanel uIPanel = UIPanel.list[j];
				if (uIPanel.renderQueue == UIPanel.RenderQueue.Automatic)
				{
					uIPanel.startingRenderQueue = num;
					uIPanel.UpdateDrawCalls();
					num += uIPanel.drawCalls.Count;
				}
				else if (uIPanel.renderQueue == UIPanel.RenderQueue.StartAt)
				{
					uIPanel.UpdateDrawCalls();
					if (uIPanel.drawCalls.Count != 0)
					{
						num = Mathf.Max(num, uIPanel.startingRenderQueue + uIPanel.drawCalls.Count);
					}
				}
				else
				{
					uIPanel.UpdateDrawCalls();
					if (uIPanel.drawCalls.Count != 0)
					{
						num = Mathf.Max(num, uIPanel.startingRenderQueue + 1);
					}
				}
				j++;
			}
		}
	}

	private void UpdateSelf()
	{
		this.UpdateTransformMatrix();
		this.UpdateLayers();
		this.UpdateWidgets();
		if (this.mRebuild)
		{
			this.mRebuild = false;
			this.FillAllDrawCalls();
		}
		else
		{
			int i = 0;
			while (i < this.drawCalls.Count)
			{
				UIDrawCall uIDrawCall = this.drawCalls[i];
				if (uIDrawCall.isDirty && !this.FillDrawCall(uIDrawCall))
				{
					UIDrawCall.Destroy(uIDrawCall);
					this.drawCalls.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
		if (this.mUpdateScroll)
		{
			this.mUpdateScroll = false;
			UIScrollView component = base.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars();
			}
		}
	}

	public void SortWidgets()
	{
		this.mSortWidgets = false;
		this.widgets.Sort(new Comparison<UIWidget>(UIWidget.PanelCompareFunc));
	}

	private void FillAllDrawCalls()
	{
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall.Destroy(this.drawCalls[i]);
		}
		this.drawCalls.Clear();
		Material material = null;
		Texture texture = null;
		Shader shader = null;
		UIDrawCall uIDrawCall = null;
		int num = 0;
		if (this.mSortWidgets)
		{
			this.SortWidgets();
		}
		for (int j = 0; j < this.widgets.Count; j++)
		{
			UIWidget uIWidget = this.widgets[j];
			if (uIWidget.isVisible && uIWidget.hasVertices)
			{
				Material material2 = uIWidget.material;
				Texture mainTexture = uIWidget.mainTexture;
				Shader shader2 = uIWidget.shader;
				if (material != material2 || texture != mainTexture || shader != shader2)
				{
					if (uIDrawCall != null && uIDrawCall.verts.size != 0)
					{
						this.drawCalls.Add(uIDrawCall);
						uIDrawCall.UpdateGeometry(num);
						uIDrawCall.onRender = this.mOnRender;
						this.mOnRender = null;
						num = 0;
						uIDrawCall = null;
					}
					material = material2;
					texture = mainTexture;
					shader = shader2;
				}
				if (material != null || shader != null || texture != null)
				{
					if (uIDrawCall == null)
					{
						uIDrawCall = UIDrawCall.Create(this, material, texture, shader);
						uIDrawCall.depthStart = uIWidget.depth;
						uIDrawCall.depthEnd = uIDrawCall.depthStart;
						uIDrawCall.panel = this;
					}
					else
					{
						int depth = uIWidget.depth;
						if (depth < uIDrawCall.depthStart)
						{
							uIDrawCall.depthStart = depth;
						}
						if (depth > uIDrawCall.depthEnd)
						{
							uIDrawCall.depthEnd = depth;
						}
					}
					uIWidget.drawCall = uIDrawCall;
					num++;
					if (this.generateNormals)
					{
						uIWidget.WriteToBuffers(uIDrawCall.verts, uIDrawCall.uvs, uIDrawCall.cols, uIDrawCall.norms, uIDrawCall.tans);
					}
					else
					{
						uIWidget.WriteToBuffers(uIDrawCall.verts, uIDrawCall.uvs, uIDrawCall.cols, null, null);
					}
					if (uIWidget.mOnRender != null)
					{
						if (this.mOnRender == null)
						{
							this.mOnRender = uIWidget.mOnRender;
						}
						else
						{
							this.mOnRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(this.mOnRender, uIWidget.mOnRender);
						}
					}
				}
			}
			else
			{
				uIWidget.drawCall = null;
			}
		}
		if (uIDrawCall != null && uIDrawCall.verts.size != 0)
		{
			this.drawCalls.Add(uIDrawCall);
			uIDrawCall.UpdateGeometry(num);
			uIDrawCall.onRender = this.mOnRender;
			this.mOnRender = null;
		}
	}

	private bool FillDrawCall(UIDrawCall dc)
	{
		if (dc != null)
		{
			dc.isDirty = false;
			int num = 0;
			int i = 0;
			while (i < this.widgets.Count)
			{
				UIWidget uIWidget = this.widgets[i];
				if (uIWidget == null)
				{
					this.widgets.RemoveAt(i);
				}
				else
				{
					if (uIWidget.drawCall == dc)
					{
						if (uIWidget.isVisible && uIWidget.hasVertices)
						{
							num++;
							if (this.generateNormals)
							{
								uIWidget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, dc.norms, dc.tans);
							}
							else
							{
								uIWidget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, null, null);
							}
							if (uIWidget.mOnRender != null)
							{
								if (this.mOnRender == null)
								{
									this.mOnRender = uIWidget.mOnRender;
								}
								else
								{
									this.mOnRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(this.mOnRender, uIWidget.mOnRender);
								}
							}
						}
						else
						{
							uIWidget.drawCall = null;
						}
					}
					i++;
				}
			}
			if (dc.verts.size != 0)
			{
				dc.UpdateGeometry(num);
				dc.onRender = this.mOnRender;
				this.mOnRender = null;
				return true;
			}
		}
		return false;
	}

	private void UpdateDrawCalls()
	{
		Transform cachedTransform = base.cachedTransform;
		bool usedForUI = this.usedForUI;
		if (this.clipping != UIDrawCall.Clipping.None)
		{
			this.drawCallClipRange = this.finalClipRegion;
			this.drawCallClipRange.z = this.drawCallClipRange.z * 0.5f;
			this.drawCallClipRange.w = this.drawCallClipRange.w * 0.5f;
		}
		else
		{
			this.drawCallClipRange = Vector4.zero;
		}
		int width = Screen.width;
		int height = Screen.height;
		if (this.drawCallClipRange.z == 0f)
		{
			this.drawCallClipRange.z = (float)width * 0.5f;
		}
		if (this.drawCallClipRange.w == 0f)
		{
			this.drawCallClipRange.w = (float)height * 0.5f;
		}
		if (this.halfPixelOffset)
		{
			this.drawCallClipRange.x = this.drawCallClipRange.x - 0.5f;
			this.drawCallClipRange.y = this.drawCallClipRange.y + 0.5f;
		}
		Vector3 vector;
		if (usedForUI)
		{
			Transform parent = base.cachedTransform.parent;
			vector = base.cachedTransform.localPosition;
			if (this.clipping != UIDrawCall.Clipping.None)
			{
				vector.x = (float)Mathf.RoundToInt(vector.x);
				vector.y = (float)Mathf.RoundToInt(vector.y);
			}
			if (parent != null)
			{
				vector = parent.TransformPoint(vector);
			}
			vector += this.drawCallOffset;
		}
		else
		{
			vector = cachedTransform.position;
		}
		Quaternion rotation = cachedTransform.rotation;
		Vector3 lossyScale = cachedTransform.lossyScale;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall uIDrawCall = this.drawCalls[i];
			Transform cachedTransform2 = uIDrawCall.cachedTransform;
			cachedTransform2.position = vector;
			cachedTransform2.rotation = rotation;
			cachedTransform2.localScale = lossyScale;
			uIDrawCall.renderQueue = ((this.renderQueue == UIPanel.RenderQueue.Explicit) ? this.startingRenderQueue : (this.startingRenderQueue + i));
			uIDrawCall.alwaysOnScreen = (this.alwaysOnScreen && (this.mClipping == UIDrawCall.Clipping.None || this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip));
			uIDrawCall.sortingOrder = this.mSortingOrder;
			uIDrawCall.clipTexture = this.mClipTexture;
		}
	}

	private void UpdateLayers()
	{
		if (this.mLayer != base.cachedGameObject.layer)
		{
			this.mLayer = this.mGo.layer;
			int i = 0;
			int count = this.widgets.Count;
			while (i < count)
			{
				UIWidget uIWidget = this.widgets[i];
				if (uIWidget && uIWidget.parent == this)
				{
					uIWidget.gameObject.layer = this.mLayer;
				}
				i++;
			}
			base.ResetAnchors();
			for (int j = 0; j < this.drawCalls.Count; j++)
			{
				this.drawCalls[j].gameObject.layer = this.mLayer;
			}
		}
	}

	private void UpdateWidgets()
	{
		bool flag = false;
		bool flag2 = false;
		bool hasCumulativeClipping = this.hasCumulativeClipping;
		if (!this.cullWhileDragging)
		{
			for (int i = 0; i < UIScrollView.list.size; i++)
			{
				UIScrollView uIScrollView = UIScrollView.list[i];
				if (uIScrollView.panel == this && uIScrollView.isDragging)
				{
					flag2 = true;
				}
			}
		}
		if (this.mForced != flag2)
		{
			this.mForced = flag2;
			this.mResized = true;
		}
		int frameCount = Time.frameCount;
		int j = 0;
		int count = this.widgets.Count;
		while (j < count)
		{
			UIWidget uIWidget = this.widgets[j];
			if (uIWidget != null && uIWidget.panel == this && uIWidget.enabled)
			{
				if (uIWidget.UpdateTransform(frameCount) || this.mResized)
				{
					bool visibleByAlpha = flag2 || uIWidget.CalculateCumulativeAlpha(frameCount) > 0.001f;
					uIWidget.UpdateVisibility(visibleByAlpha, flag2 || (!hasCumulativeClipping && !uIWidget.hideIfOffScreen) || this.IsVisible(uIWidget));
				}
				if (uIWidget.UpdateGeometry(frameCount))
				{
					flag = true;
					if (!this.mRebuild)
					{
						if (uIWidget.drawCall != null)
						{
							uIWidget.drawCall.isDirty = true;
						}
						else
						{
							this.FindDrawCall(uIWidget);
						}
					}
				}
			}
			j++;
		}
		if (flag && this.onGeometryUpdated != null)
		{
			this.onGeometryUpdated();
		}
		this.mResized = false;
	}

	public UIDrawCall FindDrawCall(UIWidget w)
	{
		Material material = w.material;
		Texture mainTexture = w.mainTexture;
		int depth = w.depth;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall uIDrawCall = this.drawCalls[i];
			int num = (i == 0) ? -2147483648 : (this.drawCalls[i - 1].depthEnd + 1);
			int num2 = (i + 1 == this.drawCalls.Count) ? 2147483647 : (this.drawCalls[i + 1].depthStart - 1);
			if (num <= depth && num2 >= depth)
			{
				if (uIDrawCall.baseMaterial == material && uIDrawCall.mainTexture == mainTexture)
				{
					if (w.isVisible)
					{
						w.drawCall = uIDrawCall;
						if (w.hasVertices)
						{
							uIDrawCall.isDirty = true;
						}
						return uIDrawCall;
					}
				}
				else
				{
					this.mRebuild = true;
				}
				return null;
			}
		}
		this.mRebuild = true;
		return null;
	}

	public void AddWidget(UIWidget w)
	{
		this.mUpdateScroll = true;
		if (this.widgets.Count == 0)
		{
			this.widgets.Add(w);
		}
		else if (this.mSortWidgets)
		{
			this.widgets.Add(w);
			this.SortWidgets();
		}
		else if (UIWidget.PanelCompareFunc(w, this.widgets[0]) == -1)
		{
			this.widgets.Insert(0, w);
		}
		else
		{
			int i = this.widgets.Count;
			while (i > 0)
			{
				if (UIWidget.PanelCompareFunc(w, this.widgets[--i]) != -1)
				{
					this.widgets.Insert(i + 1, w);
					break;
				}
			}
		}
		this.FindDrawCall(w);
	}

	public void RemoveWidget(UIWidget w)
	{
		if (this.widgets.Remove(w) && w.drawCall != null)
		{
			int depth = w.depth;
			if (depth == w.drawCall.depthStart || depth == w.drawCall.depthEnd)
			{
				this.mRebuild = true;
			}
			w.drawCall.isDirty = true;
			w.drawCall = null;
		}
	}

	public void Refresh()
	{
		this.mRebuild = true;
		UIPanel.mUpdateFrame = -1;
		if (UIPanel.list.Count > 0)
		{
			UIPanel.list[0].LateUpdate();
		}
	}

	public virtual Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		Vector4 finalClipRegion = this.finalClipRegion;
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		Vector2 minRect = new Vector2(min.x, min.y);
		Vector2 maxRect = new Vector2(max.x, max.y);
		Vector2 minArea = new Vector2(finalClipRegion.x - num, finalClipRegion.y - num2);
		Vector2 maxArea = new Vector2(finalClipRegion.x + num, finalClipRegion.y + num2);
		if (this.softBorderPadding && this.clipping == UIDrawCall.Clipping.SoftClip)
		{
			minArea.x += this.mClipSoftness.x;
			minArea.y += this.mClipSoftness.y;
			maxArea.x -= this.mClipSoftness.x;
			maxArea.y -= this.mClipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}

	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 vector = targetBounds.min;
		Vector3 vector2 = targetBounds.max;
		float num = 1f;
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				num = root.pixelSizeAdjustment;
			}
		}
		if (num != 1f)
		{
			vector /= num;
			vector2 /= num;
		}
		Vector3 b = this.CalculateConstrainOffset(vector, vector2) * num;
		if (b.sqrMagnitude > 0f)
		{
			if (immediate)
			{
				target.localPosition += b;
				targetBounds.center += b;
				SpringPosition component = target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + b, 13f);
				springPosition.ignoreTimeScale = true;
				springPosition.worldSpace = false;
			}
			return true;
		}
		return false;
	}

	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.cachedTransform, target);
		return this.ConstrainTargetToBounds(target, ref bounds, immediate);
	}

	public static UIPanel Find(Transform trans)
	{
		return UIPanel.Find(trans, false, -1);
	}

	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		return UIPanel.Find(trans, createIfMissing, -1);
	}

	public static UIPanel Find(Transform trans, bool createIfMissing, int layer)
	{
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(trans);
		if (uIPanel != null)
		{
			return uIPanel;
		}
		while (trans.parent != null)
		{
			trans = trans.parent;
		}
		if (!createIfMissing)
		{
			return null;
		}
		return NGUITools.CreateUI(trans, false, layer);
	}

	public Vector2 GetWindowSize()
	{
		UIRoot root = base.root;
		Vector2 vector = NGUITools.screenSize;
		if (root != null)
		{
			vector *= root.GetPixelSizeAdjustment(Mathf.RoundToInt(vector.y));
		}
		return vector;
	}

	public Vector2 GetViewSize()
	{
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			return new Vector2(this.mClipRange.z, this.mClipRange.w);
		}
		return NGUITools.screenSize;
	}

	public UIPanel()
	{
		this.showInPanelTool = true;
		this.cullWhileDragging = true;
		this.softBorderPadding = true;
		this.startingRenderQueue = 3000;
		this.widgets = new List<UIWidget>();
		this.drawCalls = new List<UIDrawCall>();
		this.worldToLocal = Matrix4x4.identity;
		this.drawCallClipRange = new Vector4(0f, 0f, 1f, 1f);
		this.mAlpha = 1f;
		this.mClipRange = new Vector4(0f, 0f, 300f, 200f);
		this.mClipSoftness = new Vector2(4f, 4f);
		this.mClipOffset = Vector2.zero;
		this.mMatrixFrame = -1;
		this.mLayer = -1;
		this.mMin = Vector2.zero;
		this.mMax = Vector2.zero;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			if (this.leftAnchor == null)
			{
				this.leftAnchor = new UIRect.AnchorPoint();
			}
			this.leftAnchor.Unity_Serialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.rightAnchor == null)
			{
				this.rightAnchor = new UIRect.AnchorPoint();
			}
			this.rightAnchor.Unity_Serialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor == null)
			{
				this.bottomAnchor = new UIRect.AnchorPoint();
			}
			this.bottomAnchor.Unity_Serialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.topAnchor == null)
			{
				this.topAnchor = new UIRect.AnchorPoint();
			}
			this.topAnchor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.updateAnchors);
		SerializedStateWriter.Instance.WriteBoolean(this.showInPanelTool);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.generateNormals);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.widgetsAreStatic);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.cullWhileDragging);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.alwaysOnScreen);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.anchorOffset);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.softBorderPadding);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.renderQueue);
		SerializedStateWriter.Instance.WriteInt32(this.startingRenderQueue);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mClipTexture);
		}
		SerializedStateWriter.Instance.WriteSingle(this.mAlpha);
		SerializedStateWriter.Instance.WriteInt32((int)this.mClipping);
		if (depth <= 7)
		{
			this.mClipRange.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.mClipSoftness.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32(this.mDepth);
		SerializedStateWriter.Instance.WriteInt32(this.mSortingOrder);
		if (depth <= 7)
		{
			this.mClipOffset.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			if (this.leftAnchor == null)
			{
				this.leftAnchor = new UIRect.AnchorPoint();
			}
			this.leftAnchor.Unity_Deserialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.rightAnchor == null)
			{
				this.rightAnchor = new UIRect.AnchorPoint();
			}
			this.rightAnchor.Unity_Deserialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor == null)
			{
				this.bottomAnchor = new UIRect.AnchorPoint();
			}
			this.bottomAnchor.Unity_Deserialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.topAnchor == null)
			{
				this.topAnchor = new UIRect.AnchorPoint();
			}
			this.topAnchor.Unity_Deserialize(depth + 1);
		}
		this.updateAnchors = (UIRect.AnchorUpdate)SerializedStateReader.Instance.ReadInt32();
		this.showInPanelTool = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.generateNormals = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.widgetsAreStatic = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.cullWhileDragging = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.alwaysOnScreen = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.anchorOffset = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.softBorderPadding = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.renderQueue = (UIPanel.RenderQueue)SerializedStateReader.Instance.ReadInt32();
		this.startingRenderQueue = SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.mClipTexture = (SerializedStateReader.Instance.ReadUnityEngineObject() as Texture2D);
		}
		this.mAlpha = SerializedStateReader.Instance.ReadSingle();
		this.mClipping = (UIDrawCall.Clipping)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.mClipRange.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.mClipSoftness.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.mDepth = SerializedStateReader.Instance.ReadInt32();
		this.mSortingOrder = SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.mClipOffset.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (depth <= 7)
		{
			if (this.leftAnchor != null)
			{
				this.leftAnchor.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (depth <= 7)
		{
			if (this.rightAnchor != null)
			{
				this.rightAnchor.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor != null)
			{
				this.bottomAnchor.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (depth <= 7)
		{
			if (this.topAnchor != null)
			{
				this.topAnchor.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (this.mClipTexture != null)
		{
			this.mClipTexture = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mClipTexture) as Texture2D);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			if (this.leftAnchor == null)
			{
				this.leftAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_3F_0 = this.leftAnchor;
			ISerializedNamedStateWriter arg_37_0 = SerializedNamedStateWriter.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_37_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2296);
			arg_3F_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.rightAnchor == null)
			{
				this.rightAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_82_0 = this.rightAnchor;
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2307);
			arg_82_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor == null)
			{
				this.bottomAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_C5_0 = this.bottomAnchor;
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2319);
			arg_C5_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.topAnchor == null)
			{
				this.topAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_108_0 = this.topAnchor;
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2332);
			arg_108_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.updateAnchors, &var_0_cp_0[var_0_cp_1] + 741);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.showInPanelTool, &var_0_cp_0[var_0_cp_1] + 4089);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.generateNormals, &var_0_cp_0[var_0_cp_1] + 4105);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.widgetsAreStatic, &var_0_cp_0[var_0_cp_1] + 4121);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.cullWhileDragging, &var_0_cp_0[var_0_cp_1] + 4138);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.alwaysOnScreen, &var_0_cp_0[var_0_cp_1] + 4156);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.anchorOffset, &var_0_cp_0[var_0_cp_1] + 4171);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.softBorderPadding, &var_0_cp_0[var_0_cp_1] + 4184);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.renderQueue, &var_0_cp_0[var_0_cp_1] + 4202);
		SerializedNamedStateWriter.Instance.WriteInt32(this.startingRenderQueue, &var_0_cp_0[var_0_cp_1] + 4214);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mClipTexture, &var_0_cp_0[var_0_cp_1] + 4234);
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.mAlpha, &var_0_cp_0[var_0_cp_1] + 4247);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mClipping, &var_0_cp_0[var_0_cp_1] + 4254);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4264);
			this.mClipRange.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4275);
			this.mClipSoftness.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32(this.mDepth, &var_0_cp_0[var_0_cp_1] + 2356);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mSortingOrder, &var_0_cp_0[var_0_cp_1] + 4289);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4303);
			this.mClipOffset.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			if (this.leftAnchor == null)
			{
				this.leftAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_3F_0 = this.leftAnchor;
			ISerializedNamedStateReader arg_37_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_37_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2296);
			arg_3F_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.rightAnchor == null)
			{
				this.rightAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_82_0 = this.rightAnchor;
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2307);
			arg_82_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor == null)
			{
				this.bottomAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_C5_0 = this.bottomAnchor;
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2319);
			arg_C5_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.topAnchor == null)
			{
				this.topAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_108_0 = this.topAnchor;
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2332);
			arg_108_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		this.updateAnchors = (UIRect.AnchorUpdate)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 741);
		this.showInPanelTool = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4089);
		SerializedNamedStateReader.Instance.Align();
		this.generateNormals = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4105);
		SerializedNamedStateReader.Instance.Align();
		this.widgetsAreStatic = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4121);
		SerializedNamedStateReader.Instance.Align();
		this.cullWhileDragging = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4138);
		SerializedNamedStateReader.Instance.Align();
		this.alwaysOnScreen = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4156);
		SerializedNamedStateReader.Instance.Align();
		this.anchorOffset = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4171);
		SerializedNamedStateReader.Instance.Align();
		this.softBorderPadding = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4184);
		SerializedNamedStateReader.Instance.Align();
		this.renderQueue = (UIPanel.RenderQueue)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4202);
		this.startingRenderQueue = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4214);
		if (depth <= 7)
		{
			this.mClipTexture = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4234) as Texture2D);
		}
		this.mAlpha = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 4247);
		this.mClipping = (UIDrawCall.Clipping)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4254);
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4264);
			this.mClipRange.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4275);
			this.mClipSoftness.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.mDepth = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2356);
		this.mSortingOrder = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4289);
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4303);
			this.mClipOffset.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIPanel(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UIPanel)instance).showInPanelTool;
	}

	public static void $Set0(object instance, bool value)
	{
		((UIPanel)instance).showInPanelTool = value;
	}

	public static bool $Get1(object instance)
	{
		return ((UIPanel)instance).generateNormals;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIPanel)instance).generateNormals = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UIPanel)instance).widgetsAreStatic;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIPanel)instance).widgetsAreStatic = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UIPanel)instance).cullWhileDragging;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIPanel)instance).cullWhileDragging = value;
	}

	public static bool $Get4(object instance)
	{
		return ((UIPanel)instance).alwaysOnScreen;
	}

	public static void $Set4(object instance, bool value)
	{
		((UIPanel)instance).alwaysOnScreen = value;
	}

	public static bool $Get5(object instance)
	{
		return ((UIPanel)instance).anchorOffset;
	}

	public static void $Set5(object instance, bool value)
	{
		((UIPanel)instance).anchorOffset = value;
	}

	public static bool $Get6(object instance)
	{
		return ((UIPanel)instance).softBorderPadding;
	}

	public static void $Set6(object instance, bool value)
	{
		((UIPanel)instance).softBorderPadding = value;
	}

	public static long $Get7(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)instance).mClipTexture);
	}

	public static void $Set7(object instance, long value)
	{
		((UIPanel)instance).mClipTexture = (Texture2D)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get8(object instance)
	{
		return ((UIPanel)instance).mAlpha;
	}

	public static void $Set8(object instance, float value)
	{
		((UIPanel)instance).mAlpha = value;
	}

	public static float $Get9(object instance, int index)
	{
		UIPanel expr_06_cp_0 = (UIPanel)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.mClipRange.x;
		case 1:
			return expr_06_cp_0.mClipRange.y;
		case 2:
			return expr_06_cp_0.mClipRange.z;
		case 3:
			return expr_06_cp_0.mClipRange.w;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set9(object instance, float value, int index)
	{
		UIPanel expr_06_cp_0 = (UIPanel)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.mClipRange.x = value;
			return;
		case 1:
			expr_06_cp_0.mClipRange.y = value;
			return;
		case 2:
			expr_06_cp_0.mClipRange.z = value;
			return;
		case 3:
			expr_06_cp_0.mClipRange.w = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get10(object instance, int index)
	{
		UIPanel expr_06_cp_0 = (UIPanel)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.mClipSoftness.x;
		case 1:
			return expr_06_cp_0.mClipSoftness.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set10(object instance, float value, int index)
	{
		UIPanel expr_06_cp_0 = (UIPanel)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.mClipSoftness.x = value;
			return;
		case 1:
			expr_06_cp_0.mClipSoftness.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get11(object instance, int index)
	{
		UIPanel expr_06_cp_0 = (UIPanel)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.mClipOffset.x;
		case 1:
			return expr_06_cp_0.mClipOffset.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set11(object instance, float value, int index)
	{
		UIPanel expr_06_cp_0 = (UIPanel)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.mClipOffset.x = value;
			return;
		case 1:
			expr_06_cp_0.mClipOffset.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).AddWidget((UIWidget)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).Affects((UIWidget)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).CalculateConstrainOffset(*(*(IntPtr*)args), *(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).CalculateFinalAlpha(*(int*)args));
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIPanel.CompareFunc((UIPanel)GCHandledObjects.GCHandleToObject(*args), (UIPanel)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).ConstrainTargetToBounds((Transform)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).FillAllDrawCalls();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).FillDrawCall((UIDrawCall)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIPanel.Find((Transform)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIPanel.Find((Transform)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIPanel.Find((Transform)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(int*)(args + 2)));
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).FindDrawCall((UIWidget)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).FindParent();
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).alpha);
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).baseClipRegion);
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).canBeAnchored);
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipCount);
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipOffset);
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipping);
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipRange);
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipsChildren);
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipSoftness);
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipTexture);
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).depth);
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).drawCallOffset);
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).finalClipRegion);
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).halfPixelOffset);
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).hasClipping);
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).hasCumulativeClipping);
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).height);
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).localCorners);
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIPanel.nextUnusedDepth);
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).parentPanel);
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).sortingOrder);
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).usedForUI);
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).width);
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).worldCorners);
	}

	public unsafe static long $Invoke38(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).GetSides((Transform)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke39(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).GetViewSize());
	}

	public unsafe static long $Invoke40(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).GetWindowSize());
	}

	public unsafe static long $Invoke41(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).Invalidate(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke42(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).InvalidateClipping();
		return -1L;
	}

	public unsafe static long $Invoke43(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).IsVisible((UIWidget)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke44(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).IsVisible(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke45(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPanel)GCHandledObjects.GCHandleToObject(instance)).IsVisible(*(*(IntPtr*)args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
	}

	public unsafe static long $Invoke46(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke47(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).OnAnchor();
		return -1L;
	}

	public unsafe static long $Invoke48(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke49(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke50(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).OnInit();
		return -1L;
	}

	public unsafe static long $Invoke51(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).OnStart();
		return -1L;
	}

	public unsafe static long $Invoke52(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).ParentHasChanged();
		return -1L;
	}

	public unsafe static long $Invoke53(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).RebuildAllDrawCalls();
		return -1L;
	}

	public unsafe static long $Invoke54(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).Refresh();
		return -1L;
	}

	public unsafe static long $Invoke55(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).RemoveWidget((UIWidget)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke56(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).alpha = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke57(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).baseClipRegion = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke58(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipOffset = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke59(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipping = (UIDrawCall.Clipping)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke60(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipRange = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke61(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipSoftness = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke62(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).clipTexture = (Texture2D)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke63(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).depth = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke64(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).sortingOrder = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke65(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).SetDirty();
		return -1L;
	}

	public unsafe static long $Invoke66(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).SetRect(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3));
		return -1L;
	}

	public unsafe static long $Invoke67(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).SortWidgets();
		return -1L;
	}

	public unsafe static long $Invoke68(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke69(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke70(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke71(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke72(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke73(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).UpdateDrawCalls();
		return -1L;
	}

	public unsafe static long $Invoke74(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).UpdateLayers();
		return -1L;
	}

	public unsafe static long $Invoke75(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).UpdateSelf();
		return -1L;
	}

	public unsafe static long $Invoke76(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).UpdateTransformMatrix();
		return -1L;
	}

	public unsafe static long $Invoke77(long instance, long* args)
	{
		((UIPanel)GCHandledObjects.GCHandleToObject(instance)).UpdateWidgets();
		return -1L;
	}
}
