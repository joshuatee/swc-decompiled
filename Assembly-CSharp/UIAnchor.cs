using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Anchor"), ExecuteInEditMode]
public class UIAnchor : MonoBehaviour, IUnitySerializable
{
	public enum Side
	{
		BottomLeft,
		Left,
		TopLeft,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		Center
	}

	public Camera uiCamera;

	public GameObject container;

	public UIAnchor.Side side;

	public bool runOnlyOnce;

	public Vector2 relativeOffset;

	public Vector2 pixelOffset;

	[HideInInspector, SerializeField]
	protected internal UIWidget widgetContainer;

	private Transform mTrans;

	private Animation mAnim;

	private Rect mRect;

	private UIRoot mRoot;

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
		this.mTrans = base.transform;
		this.mAnim = base.GetComponent<Animation>();
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
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.Update();
		this.mStarted = true;
	}

	private void Update()
	{
		if (this.mAnim != null && this.mAnim.enabled && this.mAnim.isPlaying)
		{
			return;
		}
		bool flag = false;
		UIWidget uIWidget = (this.container == null) ? null : this.container.GetComponent<UIWidget>();
		UIPanel uIPanel = (this.container == null && uIWidget == null) ? null : this.container.GetComponent<UIPanel>();
		if (uIWidget != null)
		{
			Bounds bounds = uIWidget.CalculateBounds(this.container.transform.parent);
			this.mRect.x = bounds.min.x;
			this.mRect.y = bounds.min.y;
			this.mRect.width = bounds.size.x;
			this.mRect.height = bounds.size.y;
		}
		else if (uIPanel != null)
		{
			if (uIPanel.clipping == UIDrawCall.Clipping.None)
			{
				float num = (this.mRoot != null) ? ((float)this.mRoot.activeHeight / (float)Screen.height * 0.5f) : 0.5f;
				this.mRect.xMin = (float)(-(float)Screen.width) * num;
				this.mRect.yMin = (float)(-(float)Screen.height) * num;
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
			Transform parent = this.container.transform.parent;
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
			flag = true;
			this.mRect = this.uiCamera.pixelRect;
		}
		float x = (this.mRect.xMin + this.mRect.xMax) * 0.5f;
		float y = (this.mRect.yMin + this.mRect.yMax) * 0.5f;
		Vector3 vector = new Vector3(x, y, 0f);
		if (this.side != UIAnchor.Side.Center)
		{
			if (this.side == UIAnchor.Side.Right || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.BottomRight)
			{
				vector.x = this.mRect.xMax;
			}
			else if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Bottom)
			{
				vector.x = x;
			}
			else
			{
				vector.x = this.mRect.xMin;
			}
			if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.TopLeft)
			{
				vector.y = this.mRect.yMax;
			}
			else if (this.side == UIAnchor.Side.Left || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Right)
			{
				vector.y = y;
			}
			else
			{
				vector.y = this.mRect.yMin;
			}
		}
		float width = this.mRect.width;
		float height = this.mRect.height;
		vector.x += this.pixelOffset.x + this.relativeOffset.x * width;
		vector.y += this.pixelOffset.y + this.relativeOffset.y * height;
		if (flag)
		{
			if (this.uiCamera.orthographic)
			{
				vector.x = Mathf.Round(vector.x);
				vector.y = Mathf.Round(vector.y);
			}
			vector.z = this.uiCamera.WorldToScreenPoint(this.mTrans.position).z;
			vector = this.uiCamera.ScreenToWorldPoint(vector);
		}
		else
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			if (uIPanel != null)
			{
				vector = uIPanel.cachedTransform.TransformPoint(vector);
			}
			else if (this.container != null)
			{
				Transform parent2 = this.container.transform.parent;
				if (parent2 != null)
				{
					vector = parent2.TransformPoint(vector);
				}
			}
			vector.z = this.mTrans.position.z;
		}
		if (flag && this.uiCamera.orthographic && this.mTrans.parent != null)
		{
			vector = this.mTrans.parent.InverseTransformPoint(vector);
			vector.x = (float)Mathf.RoundToInt(vector.x);
			vector.y = (float)Mathf.RoundToInt(vector.y);
			if (this.mTrans.localPosition != vector)
			{
				this.mTrans.localPosition = vector;
			}
		}
		else if (this.mTrans.position != vector)
		{
			this.mTrans.position = vector;
		}
		if (this.RunOnlyOnce && Application.isPlaying)
		{
			base.enabled = false;
		}
	}

	public UIAnchor()
	{
		this.side = UIAnchor.Side.Center;
		this.runOnlyOnce = true;
		this.relativeOffset = Vector2.zero;
		this.pixelOffset = Vector2.zero;
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
		SerializedStateWriter.Instance.WriteInt32((int)this.side);
		SerializedStateWriter.Instance.WriteBoolean(this.runOnlyOnce);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.relativeOffset.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.pixelOffset.Unity_Serialize(depth + 1);
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
		this.side = (UIAnchor.Side)SerializedStateReader.Instance.ReadInt32();
		this.runOnlyOnce = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.relativeOffset.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.pixelOffset.Unity_Deserialize(depth + 1);
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
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.side, &var_0_cp_0[var_0_cp_1] + 2893);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.runOnlyOnce, &var_0_cp_0[var_0_cp_1] + 2898);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2910);
			this.relativeOffset.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2925);
			this.pixelOffset.Unity_NamedSerialize(depth + 1);
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
		this.side = (UIAnchor.Side)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2893);
		this.runOnlyOnce = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2898);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2910);
			this.relativeOffset.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2925);
			this.pixelOffset.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.widgetContainer = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2937) as UIWidget);
		}
	}

	protected internal UIAnchor(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAnchor)instance).uiCamera);
	}

	public static void $Set0(object instance, long value)
	{
		((UIAnchor)instance).uiCamera = (Camera)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAnchor)instance).container);
	}

	public static void $Set1(object instance, long value)
	{
		((UIAnchor)instance).container = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get2(object instance)
	{
		return ((UIAnchor)instance).runOnlyOnce;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIAnchor)instance).runOnlyOnce = value;
	}

	public static float $Get3(object instance, int index)
	{
		UIAnchor expr_06_cp_0 = (UIAnchor)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.relativeOffset.x;
		case 1:
			return expr_06_cp_0.relativeOffset.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set3(object instance, float value, int index)
	{
		UIAnchor expr_06_cp_0 = (UIAnchor)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.relativeOffset.x = value;
			return;
		case 1:
			expr_06_cp_0.relativeOffset.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get4(object instance, int index)
	{
		UIAnchor expr_06_cp_0 = (UIAnchor)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.pixelOffset.x;
		case 1:
			return expr_06_cp_0.pixelOffset.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set4(object instance, float value, int index)
	{
		UIAnchor expr_06_cp_0 = (UIAnchor)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.pixelOffset.x = value;
			return;
		case 1:
			expr_06_cp_0.pixelOffset.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static long $Get5(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAnchor)instance).widgetContainer);
	}

	public static void $Set5(object instance, long value)
	{
		((UIAnchor)instance).widgetContainer = (UIWidget)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).RunOnlyOnce);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).OnDestroy();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).ScreenSizeChanged();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIAnchor)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
