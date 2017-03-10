using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[RequireComponent(typeof(UITexture))]
public class UIColorPicker : MonoBehaviour, IUnitySerializable
{
	public static UIColorPicker current;

	public Color value;

	public UIWidget selectionWidget;

	public List<EventDelegate> onChange;

	[System.NonSerialized]
	private Transform mTrans;

	[System.NonSerialized]
	private UITexture mUITex;

	[System.NonSerialized]
	private Texture2D mTex;

	[System.NonSerialized]
	private UICamera mCam;

	[System.NonSerialized]
	private Vector2 mPos;

	[System.NonSerialized]
	private int mWidth;

	[System.NonSerialized]
	private int mHeight;

	private static AnimationCurve mRed;

	private static AnimationCurve mGreen;

	private static AnimationCurve mBlue;

	private void Start()
	{
		this.mTrans = base.transform;
		this.mUITex = base.GetComponent<UITexture>();
		this.mCam = UICamera.FindCameraForLayer(base.gameObject.layer);
		this.mWidth = this.mUITex.width;
		this.mHeight = this.mUITex.height;
		Color[] array = new Color[this.mWidth * this.mHeight];
		for (int i = 0; i < this.mHeight; i++)
		{
			float y = ((float)i - 1f) / (float)this.mHeight;
			for (int j = 0; j < this.mWidth; j++)
			{
				float x = ((float)j - 1f) / (float)this.mWidth;
				int num = j + i * this.mWidth;
				array[num] = UIColorPicker.Sample(x, y);
			}
		}
		this.mTex = new Texture2D(this.mWidth, this.mHeight, TextureFormat.RGB24, false);
		this.mTex.SetPixels(array);
		this.mTex.filterMode = FilterMode.Trilinear;
		this.mTex.wrapMode = TextureWrapMode.Clamp;
		this.mTex.Apply();
		this.mUITex.mainTexture = this.mTex;
		this.Select(this.value);
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.mTex);
		this.mTex = null;
	}

	private void OnPress(bool pressed)
	{
		if ((base.enabled & pressed) && UICamera.currentScheme != UICamera.ControlScheme.Controller)
		{
			this.Sample();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (base.enabled)
		{
			this.Sample();
		}
	}

	private void OnPan(Vector2 delta)
	{
		if (base.enabled)
		{
			this.mPos.x = Mathf.Clamp01(this.mPos.x + delta.x);
			this.mPos.y = Mathf.Clamp01(this.mPos.y + delta.y);
			this.Select(this.mPos);
		}
	}

	private void Sample()
	{
		Vector3 vector = UICamera.lastEventPosition;
		vector = this.mCam.cachedCamera.ScreenToWorldPoint(vector);
		vector = this.mTrans.InverseTransformPoint(vector);
		Vector3[] localCorners = this.mUITex.localCorners;
		this.mPos.x = Mathf.Clamp01((vector.x - localCorners[0].x) / (localCorners[2].x - localCorners[0].x));
		this.mPos.y = Mathf.Clamp01((vector.y - localCorners[0].y) / (localCorners[2].y - localCorners[0].y));
		if (this.selectionWidget != null)
		{
			vector.x = Mathf.Lerp(localCorners[0].x, localCorners[2].x, this.mPos.x);
			vector.y = Mathf.Lerp(localCorners[0].y, localCorners[2].y, this.mPos.y);
			vector = this.mTrans.TransformPoint(vector);
			this.selectionWidget.transform.OverlayPosition(vector, this.mCam.cachedCamera);
		}
		this.value = UIColorPicker.Sample(this.mPos.x, this.mPos.y);
		UIColorPicker.current = this;
		EventDelegate.Execute(this.onChange);
		UIColorPicker.current = null;
	}

	public void Select(Vector2 v)
	{
		v.x = Mathf.Clamp01(v.x);
		v.y = Mathf.Clamp01(v.y);
		this.mPos = v;
		if (this.selectionWidget != null)
		{
			Vector3[] localCorners = this.mUITex.localCorners;
			v.x = Mathf.Lerp(localCorners[0].x, localCorners[2].x, this.mPos.x);
			v.y = Mathf.Lerp(localCorners[0].y, localCorners[2].y, this.mPos.y);
			v = this.mTrans.TransformPoint(v);
			this.selectionWidget.transform.OverlayPosition(v, this.mCam.cachedCamera);
		}
		this.value = UIColorPicker.Sample(this.mPos.x, this.mPos.y);
		UIColorPicker.current = this;
		EventDelegate.Execute(this.onChange);
		UIColorPicker.current = null;
	}

	public Vector2 Select(Color c)
	{
		if (this.mUITex == null)
		{
			this.value = c;
			return this.mPos;
		}
		float num = 3.40282347E+38f;
		for (int i = 0; i < this.mHeight; i++)
		{
			float y = ((float)i - 1f) / (float)this.mHeight;
			for (int j = 0; j < this.mWidth; j++)
			{
				float x = ((float)j - 1f) / (float)this.mWidth;
				Color color = UIColorPicker.Sample(x, y);
				Color color2 = color;
				color2.r -= c.r;
				color2.g -= c.g;
				color2.b -= c.b;
				float num2 = color2.r * color2.r + color2.g * color2.g + color2.b * color2.b;
				if (num2 < num)
				{
					num = num2;
					this.mPos.x = x;
					this.mPos.y = y;
				}
			}
		}
		if (this.selectionWidget != null)
		{
			Vector3[] localCorners = this.mUITex.localCorners;
			Vector3 vector;
			vector.x = Mathf.Lerp(localCorners[0].x, localCorners[2].x, this.mPos.x);
			vector.y = Mathf.Lerp(localCorners[0].y, localCorners[2].y, this.mPos.y);
			vector.z = 0f;
			vector = this.mTrans.TransformPoint(vector);
			this.selectionWidget.transform.OverlayPosition(vector, this.mCam.cachedCamera);
		}
		this.value = c;
		UIColorPicker.current = this;
		EventDelegate.Execute(this.onChange);
		UIColorPicker.current = null;
		return this.mPos;
	}

	public static Color Sample(float x, float y)
	{
		if (UIColorPicker.mRed == null)
		{
			UIColorPicker.mRed = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 1f),
				new Keyframe(0.142857149f, 1f),
				new Keyframe(0.2857143f, 0f),
				new Keyframe(0.428571433f, 0f),
				new Keyframe(0.5714286f, 0f),
				new Keyframe(0.714285731f, 1f),
				new Keyframe(0.857142866f, 1f),
				new Keyframe(1f, 0.5f)
			});
			UIColorPicker.mGreen = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(0.142857149f, 1f),
				new Keyframe(0.2857143f, 1f),
				new Keyframe(0.428571433f, 1f),
				new Keyframe(0.5714286f, 0f),
				new Keyframe(0.714285731f, 0f),
				new Keyframe(0.857142866f, 0f),
				new Keyframe(1f, 0.5f)
			});
			UIColorPicker.mBlue = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(0.142857149f, 0f),
				new Keyframe(0.2857143f, 0f),
				new Keyframe(0.428571433f, 1f),
				new Keyframe(0.5714286f, 1f),
				new Keyframe(0.714285731f, 1f),
				new Keyframe(0.857142866f, 0f),
				new Keyframe(1f, 0.5f)
			});
		}
		Vector3 vector = new Vector3(UIColorPicker.mRed.Evaluate(x), UIColorPicker.mGreen.Evaluate(x), UIColorPicker.mBlue.Evaluate(x));
		if (y < 0.5f)
		{
			y *= 2f;
			vector.x *= y;
			vector.y *= y;
			vector.z *= y;
		}
		else
		{
			vector = Vector3.Lerp(vector, Vector3.one, y * 2f - 1f);
		}
		return new Color(vector.x, vector.y, vector.z, 1f);
	}

	public UIColorPicker()
	{
		this.value = Color.white;
		this.onChange = new List<EventDelegate>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			this.value.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.selectionWidget);
		}
		if (depth <= 7)
		{
			if (this.onChange == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onChange.Count);
				for (int i = 0; i < this.onChange.Count; i++)
				{
					((this.onChange[i] != null) ? this.onChange[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.value.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.selectionWidget = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIWidget);
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				eventDelegate.Unity_Deserialize(depth + 1);
				this.onChange.Add(eventDelegate);
			}
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.selectionWidget != null)
		{
			this.selectionWidget = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.selectionWidget) as UIWidget);
		}
		if (depth <= 7)
		{
			if (this.onChange != null)
			{
				for (int i = 0; i < this.onChange.Count; i++)
				{
					EventDelegate eventDelegate = this.onChange[i];
					if (eventDelegate != null)
					{
						eventDelegate.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3495);
			this.value.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.selectionWidget, &var_0_cp_0[var_0_cp_1] + 3501);
		}
		if (depth <= 7)
		{
			if (this.onChange == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446, this.onChange.Count);
				for (int i = 0; i < this.onChange.Count; i++)
				{
					EventDelegate arg_E8_0 = (this.onChange[i] != null) ? this.onChange[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_E8_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateReader arg_23_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3495);
			this.value.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.selectionWidget = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3501) as UIWidget);
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446);
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_A5_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_A5_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onChange.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal UIColorPicker(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance, int index)
	{
		UIColorPicker expr_06_cp_0 = (UIColorPicker)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.value.r;
		case 1:
			return expr_06_cp_0.value.g;
		case 2:
			return expr_06_cp_0.value.b;
		case 3:
			return expr_06_cp_0.value.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set0(object instance, float value, int index)
	{
		UIColorPicker expr_06_cp_0 = (UIColorPicker)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.value.r = value;
			return;
		case 1:
			expr_06_cp_0.value.g = value;
			return;
		case 2:
			expr_06_cp_0.value.b = value;
			return;
		case 3:
			expr_06_cp_0.value.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIColorPicker)instance).selectionWidget);
	}

	public static void $Set1(object instance, long value)
	{
		((UIColorPicker)instance).selectionWidget = (UIWidget)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).OnDestroy();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).OnPan(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIColorPicker.Sample(*(float*)args, *(float*)(args + 1)));
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).Sample();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).Select(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).Select(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIColorPicker)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
