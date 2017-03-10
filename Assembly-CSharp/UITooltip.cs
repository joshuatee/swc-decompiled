using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour, IUnitySerializable
{
	protected static UITooltip mInstance;

	public Camera uiCamera;

	public UILabel text;

	public GameObject tooltipRoot;

	public UISprite background;

	public float appearSpeed;

	public bool scalingTransitions;

	protected GameObject mTooltip;

	protected Transform mTrans;

	protected float mTarget;

	protected float mCurrent;

	protected Vector3 mPos;

	protected Vector3 mSize;

	protected UIWidget[] mWidgets;

	public static bool isVisible
	{
		get
		{
			return UITooltip.mInstance != null && UITooltip.mInstance.mTarget == 1f;
		}
	}

	private void Awake()
	{
		UITooltip.mInstance = this;
	}

	private void OnDestroy()
	{
		UITooltip.mInstance = null;
	}

	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.mPos = this.mTrans.localPosition;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.SetAlpha(0f);
	}

	protected virtual void Update()
	{
		if (this.mTooltip != UICamera.tooltipObject)
		{
			this.mTooltip = null;
			this.mTarget = 0f;
		}
		if (this.mCurrent != this.mTarget)
		{
			this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, RealTime.deltaTime * this.appearSpeed);
			if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
			{
				this.mCurrent = this.mTarget;
			}
			this.SetAlpha(this.mCurrent * this.mCurrent);
			if (this.scalingTransitions)
			{
				Vector3 vector = this.mSize * 0.25f;
				vector.y = -vector.y;
				Vector3 localScale = Vector3.one * (1.5f - this.mCurrent * 0.5f);
				Vector3 localPosition = Vector3.Lerp(this.mPos - vector, this.mPos, this.mCurrent);
				this.mTrans.localPosition = localPosition;
				this.mTrans.localScale = localScale;
			}
		}
	}

	protected virtual void SetAlpha(float val)
	{
		int i = 0;
		int num = this.mWidgets.Length;
		while (i < num)
		{
			UIWidget uIWidget = this.mWidgets[i];
			Color color = uIWidget.color;
			color.a = val;
			uIWidget.color = color;
			i++;
		}
	}

	protected virtual void SetText(string tooltipText)
	{
		if (!(this.text != null) || string.IsNullOrEmpty(tooltipText))
		{
			this.mTooltip = null;
			this.mTarget = 0f;
			return;
		}
		this.mTarget = 1f;
		this.mTooltip = UICamera.tooltipObject;
		this.text.text = tooltipText;
		this.mPos = UICamera.lastEventPosition;
		Transform transform = this.text.transform;
		Vector3 localPosition = transform.localPosition;
		Vector3 localScale = transform.localScale;
		this.mSize = this.text.printedSize;
		this.mSize.x = this.mSize.x * localScale.x;
		this.mSize.y = this.mSize.y * localScale.y;
		if (this.background != null)
		{
			Vector4 border = this.background.border;
			this.mSize.x = this.mSize.x + (border.x + border.z + (localPosition.x - border.x) * 2f);
			this.mSize.y = this.mSize.y + (border.y + border.w + (-localPosition.y - border.y) * 2f);
			this.background.width = Mathf.RoundToInt(this.mSize.x);
			this.background.height = Mathf.RoundToInt(this.mSize.y);
		}
		if (this.uiCamera != null)
		{
			this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.width);
			this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.height);
			float num = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
			float num2 = (float)Screen.height * 0.5f / num;
			Vector2 vector = new Vector2(num2 * this.mSize.x / (float)Screen.width, num2 * this.mSize.y / (float)Screen.height);
			this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector.x);
			this.mPos.y = Mathf.Max(this.mPos.y, vector.y);
			this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
			this.mPos = this.mTrans.localPosition;
			this.mPos.x = Mathf.Round(this.mPos.x);
			this.mPos.y = Mathf.Round(this.mPos.y);
			this.mTrans.localPosition = this.mPos;
		}
		else
		{
			if (this.mPos.x + this.mSize.x > (float)Screen.width)
			{
				this.mPos.x = (float)Screen.width - this.mSize.x;
			}
			if (this.mPos.y - this.mSize.y < 0f)
			{
				this.mPos.y = this.mSize.y;
			}
			this.mPos.x = this.mPos.x - (float)Screen.width * 0.5f;
			this.mPos.y = this.mPos.y - (float)Screen.height * 0.5f;
		}
		if (this.tooltipRoot != null)
		{
			this.tooltipRoot.BroadcastMessage("UpdateAnchors");
			return;
		}
		this.text.BroadcastMessage("UpdateAnchors");
	}

	[Obsolete("Use UITooltip.Show instead")]
	public static void ShowText(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	public static void Show(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	public static void Hide()
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.mTooltip = null;
			UITooltip.mInstance.mTarget = 0f;
		}
	}

	public UITooltip()
	{
		this.appearSpeed = 10f;
		this.scalingTransitions = true;
		this.mSize = Vector3.zero;
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
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.text);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.tooltipRoot);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.background);
		}
		SerializedStateWriter.Instance.WriteSingle(this.appearSpeed);
		SerializedStateWriter.Instance.WriteBoolean(this.scalingTransitions);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.uiCamera = (SerializedStateReader.Instance.ReadUnityEngineObject() as Camera);
		}
		if (depth <= 7)
		{
			this.text = (SerializedStateReader.Instance.ReadUnityEngineObject() as UILabel);
		}
		if (depth <= 7)
		{
			this.tooltipRoot = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.background = (SerializedStateReader.Instance.ReadUnityEngineObject() as UISprite);
		}
		this.appearSpeed = SerializedStateReader.Instance.ReadSingle();
		this.scalingTransitions = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.uiCamera != null)
		{
			this.uiCamera = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.uiCamera) as Camera);
		}
		if (this.text != null)
		{
			this.text = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.text) as UILabel);
		}
		if (this.tooltipRoot != null)
		{
			this.tooltipRoot = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.tooltipRoot) as GameObject);
		}
		if (this.background != null)
		{
			this.background = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.background) as UISprite);
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
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.text, &var_0_cp_0[var_0_cp_1] + 4593);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.tooltipRoot, &var_0_cp_0[var_0_cp_1] + 4598);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.background, &var_0_cp_0[var_0_cp_1] + 4610);
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.appearSpeed, &var_0_cp_0[var_0_cp_1] + 4621);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.scalingTransitions, &var_0_cp_0[var_0_cp_1] + 4633);
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
			this.uiCamera = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2874) as Camera);
		}
		if (depth <= 7)
		{
			this.text = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4593) as UILabel);
		}
		if (depth <= 7)
		{
			this.tooltipRoot = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4598) as GameObject);
		}
		if (depth <= 7)
		{
			this.background = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4610) as UISprite);
		}
		this.appearSpeed = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 4621);
		this.scalingTransitions = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4633);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UITooltip(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITooltip)instance).uiCamera);
	}

	public static void $Set0(object instance, long value)
	{
		((UITooltip)instance).uiCamera = (Camera)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITooltip)instance).text);
	}

	public static void $Set1(object instance, long value)
	{
		((UITooltip)instance).text = (UILabel)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITooltip)instance).tooltipRoot);
	}

	public static void $Set2(object instance, long value)
	{
		((UITooltip)instance).tooltipRoot = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get3(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITooltip)instance).background);
	}

	public static void $Set3(object instance, long value)
	{
		((UITooltip)instance).background = (UISprite)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get4(object instance)
	{
		return ((UITooltip)instance).appearSpeed;
	}

	public static void $Set4(object instance, float value)
	{
		((UITooltip)instance).appearSpeed = value;
	}

	public static bool $Get5(object instance)
	{
		return ((UITooltip)instance).scalingTransitions;
	}

	public static void $Set5(object instance, bool value)
	{
		((UITooltip)instance).scalingTransitions = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UITooltip.isVisible);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		UITooltip.Hide();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).OnDestroy();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).SetAlpha(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).SetText(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		UITooltip.Show(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		UITooltip.ShowText(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UITooltip)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
