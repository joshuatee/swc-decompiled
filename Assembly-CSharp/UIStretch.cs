using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Stretch"), ExecuteInEditMode]
public class UIStretch : MonoBehaviour, IUnitySerializable
{
	public enum Style
	{
		None,
		Horizontal,
		Vertical,
		Both,
		BasedOnHeight,
		FillKeepingRatio,
		FitInternalKeepingRatio
	}

	public Camera uiCamera;

	public GameObject container;

	public UIStretch.Style style;

	public bool runOnlyOnce;

	public Vector2 relativeSize;

	public Vector2 initialSize;

	public Vector2 borderPadding;

	[HideInInspector, SerializeField]
	protected internal UIWidget widgetContainer;

	private Transform mTrans;

	private UIWidget mWidget;

	private UISprite mSprite;

	private UIPanel mPanel;

	private UIRoot mRoot;

	private Animation mAnim;

	private Rect mRect;

	private bool mStarted;

	public bool RunOnlyOnce
	{
		get
		{
			return false;
		}
	}

	private void Awake()
	{
		this.mAnim = base.GetComponent<Animation>();
		this.mRect = default(Rect);
		this.mTrans = base.transform;
		this.mWidget = base.GetComponent<UIWidget>();
		this.mSprite = base.GetComponent<UISprite>();
		this.mPanel = base.GetComponent<UIPanel>();
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	private void ScreenSizeChanged()
	{
		if (this.mStarted && this.RunOnlyOnce)
		{
			this.Update();
		}
	}

	private void Start()
	{
		if (this.container == null && this.widgetContainer != null)
		{
			this.container = this.widgetContainer.gameObject;
			this.widgetContainer = null;
		}
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		this.Update();
		this.mStarted = true;
	}

	private void Update()
	{
		if (this.mAnim != null && this.mAnim.isPlaying)
		{
			return;
		}
		if (this.style != UIStretch.Style.None)
		{
			UIWidget uIWidget = (this.container == null) ? null : this.container.GetComponent<UIWidget>();
			UIPanel uIPanel = (this.container == null && uIWidget == null) ? null : this.container.GetComponent<UIPanel>();
			float num = 1f;
			if (uIWidget != null)
			{
				Bounds bounds = uIWidget.CalculateBounds(base.transform.parent);
				this.mRect.x = bounds.min.x;
				this.mRect.y = bounds.min.y;
				this.mRect.width = bounds.size.x;
				this.mRect.height = bounds.size.y;
			}
			else if (uIPanel != null)
			{
				if (uIPanel.clipping == UIDrawCall.Clipping.None)
				{
					float num2 = (this.mRoot != null) ? ((float)this.mRoot.activeHeight / (float)Screen.height * 0.5f) : 0.5f;
					this.mRect.xMin = (float)(-(float)Screen.width) * num2;
					this.mRect.yMin = (float)(-(float)Screen.height) * num2;
					this.mRect.xMax = -this.mRect.xMin;
					this.mRect.yMax = -this.mRect.yMin;
				}
				else
				{
					Vector4 finalClipRegion = uIPanel.finalClipRegion;
					this.mRect.x = finalClipRegion.x - finalClipRegion.z * 0.5f;
					this.mRect.y = finalClipRegion.y - finalClipRegion.w * 0.5f;
					this.mRect.width = finalClipRegion.z;
					this.mRect.height = finalClipRegion.w;
				}
			}
			else if (this.container != null)
			{
				Transform parent = base.transform.parent;
				Bounds bounds2 = (parent != null) ? NGUIMath.CalculateRelativeWidgetBounds(parent, this.container.transform) : NGUIMath.CalculateRelativeWidgetBounds(this.container.transform);
				this.mRect.x = bounds2.min.x;
				this.mRect.y = bounds2.min.y;
				this.mRect.width = bounds2.size.x;
				this.mRect.height = bounds2.size.y;
			}
			else
			{
				if (!(this.uiCamera != null))
				{
					return;
				}
				this.mRect = this.uiCamera.pixelRect;
				if (this.mRoot != null)
				{
					num = this.mRoot.pixelSizeAdjustment;
				}
			}
			float num3 = this.mRect.width;
			float num4 = this.mRect.height;
			if (num != 1f && num4 > 1f)
			{
				float num5 = (float)this.mRoot.activeHeight / num4;
				num3 *= num5;
				num4 *= num5;
			}
			Vector3 vector = (this.mWidget != null) ? new Vector3((float)this.mWidget.width, (float)this.mWidget.height) : this.mTrans.localScale;
			if (this.style == UIStretch.Style.BasedOnHeight)
			{
				vector.x = this.relativeSize.x * num4;
				vector.y = this.relativeSize.y * num4;
			}
			else if (this.style == UIStretch.Style.FillKeepingRatio)
			{
				float num6 = num3 / num4;
				float num7 = this.initialSize.x / this.initialSize.y;
				if (num7 < num6)
				{
					float num8 = num3 / this.initialSize.x;
					vector.x = num3;
					vector.y = this.initialSize.y * num8;
				}
				else
				{
					float num9 = num4 / this.initialSize.y;
					vector.x = this.initialSize.x * num9;
					vector.y = num4;
				}
			}
			else if (this.style == UIStretch.Style.FitInternalKeepingRatio)
			{
				float num10 = num3 / num4;
				float num11 = this.initialSize.x / this.initialSize.y;
				if (num11 > num10)
				{
					float num12 = num3 / this.initialSize.x;
					vector.x = num3;
					vector.y = this.initialSize.y * num12;
				}
				else
				{
					float num13 = num4 / this.initialSize.y;
					vector.x = this.initialSize.x * num13;
					vector.y = num4;
				}
			}
			else
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector.x = this.relativeSize.x * num3;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector.y = this.relativeSize.y * num4;
				}
			}
			if (this.mSprite != null)
			{
				float num14 = (this.mSprite.atlas != null) ? this.mSprite.atlas.pixelSize : 1f;
				vector.x -= this.borderPadding.x * num14;
				vector.y -= this.borderPadding.y * num14;
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mSprite.width = Mathf.RoundToInt(vector.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mSprite.height = Mathf.RoundToInt(vector.y);
				}
				vector = Vector3.one;
			}
			else if (this.mWidget != null)
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mWidget.width = Mathf.RoundToInt(vector.x - this.borderPadding.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mWidget.height = Mathf.RoundToInt(vector.y - this.borderPadding.y);
				}
				vector = Vector3.one;
			}
			else if (this.mPanel != null)
			{
				Vector4 baseClipRegion = this.mPanel.baseClipRegion;
				if (this.style != UIStretch.Style.Vertical)
				{
					baseClipRegion.z = vector.x - this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					baseClipRegion.w = vector.y - this.borderPadding.y;
				}
				this.mPanel.baseClipRegion = baseClipRegion;
				vector = Vector3.one;
			}
			else
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector.x -= this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector.y -= this.borderPadding.y;
				}
			}
			if (this.mTrans.localScale != vector)
			{
				this.mTrans.localScale = vector;
			}
			if (this.RunOnlyOnce && Application.isPlaying)
			{
				base.enabled = false;
			}
		}
	}

	public UIStretch()
	{
		this.runOnlyOnce = true;
		this.relativeSize = Vector2.one;
		this.initialSize = Vector2.one;
		this.borderPadding = Vector2.zero;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.uiCamera);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.container);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.style);
		SerializedStateWriter.Instance.WriteBoolean(this.runOnlyOnce);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.relativeSize.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.initialSize.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.borderPadding.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.widgetContainer);
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.uiCamera = (SerializedStateReader.Instance.ReadUnityEngineObject() as Camera);
		}
		if (depth <= 7)
		{
			this.container = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.style = (UIStretch.Style)SerializedStateReader.Instance.ReadInt32();
		this.runOnlyOnce = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.relativeSize.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.initialSize.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.borderPadding.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.widgetContainer = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIWidget);
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.uiCamera != null)
		{
			this.uiCamera = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.uiCamera) as Camera);
		}
		if (this.container != null)
		{
			this.container = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.container) as GameObject);
		}
		if (this.widgetContainer != null)
		{
			this.widgetContainer = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.widgetContainer) as UIWidget);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.uiCamera;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 2874);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.container, &var_0_cp_0[var_0_cp_1] + 2883);
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.style, &var_0_cp_0[var_0_cp_1] + 2693);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.runOnlyOnce, &var_0_cp_0[var_0_cp_1] + 2898);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4512);
			this.relativeSize.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4525);
			this.initialSize.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4537);
			this.borderPadding.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.widgetContainer, &var_0_cp_0[var_0_cp_1] + 2937);
		}
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
			this.uiCamera = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2874) as Camera);
		}
		if (depth <= 7)
		{
			this.container = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2883) as GameObject);
		}
		this.style = (UIStretch.Style)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2693);
		this.runOnlyOnce = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2898);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4512);
			this.relativeSize.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4525);
			this.initialSize.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4537);
			this.borderPadding.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.widgetContainer = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2937) as UIWidget);
		}
	}

	protected internal UIStretch(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIStretch)instance).uiCamera);
	}

	public static void $Set0(object instance, long value)
	{
		((UIStretch)instance).uiCamera = (Camera)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIStretch)instance).container);
	}

	public static void $Set1(object instance, long value)
	{
		((UIStretch)instance).container = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get2(object instance)
	{
		return ((UIStretch)instance).runOnlyOnce;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIStretch)instance).runOnlyOnce = value;
	}

	public static float $Get3(object instance, int index)
	{
		UIStretch expr_06_cp_0 = (UIStretch)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.relativeSize.x;
		case 1:
			return expr_06_cp_0.relativeSize.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set3(object instance, float value, int index)
	{
		UIStretch expr_06_cp_0 = (UIStretch)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.relativeSize.x = value;
			return;
		case 1:
			expr_06_cp_0.relativeSize.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get4(object instance, int index)
	{
		UIStretch expr_06_cp_0 = (UIStretch)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.initialSize.x;
		case 1:
			return expr_06_cp_0.initialSize.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set4(object instance, float value, int index)
	{
		UIStretch expr_06_cp_0 = (UIStretch)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.initialSize.x = value;
			return;
		case 1:
			expr_06_cp_0.initialSize.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get5(object instance, int index)
	{
		UIStretch expr_06_cp_0 = (UIStretch)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.borderPadding.x;
		case 1:
			return expr_06_cp_0.borderPadding.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set5(object instance, float value, int index)
	{
		UIStretch expr_06_cp_0 = (UIStretch)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.borderPadding.x = value;
			return;
		case 1:
			expr_06_cp_0.borderPadding.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static long $Get6(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIStretch)instance).widgetContainer);
	}

	public static void $Set6(object instance, long value)
	{
		((UIStretch)instance).widgetContainer = (UIWidget)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIStretch)GCHandledObjects.GCHandleToObject(instance)).RunOnlyOnce);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).OnDestroy();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).ScreenSizeChanged();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIStretch)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
