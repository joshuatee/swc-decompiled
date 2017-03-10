using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/NGUI Progress Bar"), ExecuteInEditMode]
public class UIProgressBar : UIWidgetContainer, IUnitySerializable
{
	public enum FillDirection
	{
		LeftToRight,
		RightToLeft,
		BottomToTop,
		TopToBottom
	}

	public delegate void OnDragFinished();

	public static UIProgressBar current;

	public UIProgressBar.OnDragFinished onDragFinished;

	public Transform thumb;

	[HideInInspector, SerializeField]
	protected UIWidget mBG;

	[HideInInspector, SerializeField]
	protected UIWidget mFG;

	[HideInInspector, SerializeField]
	protected float mValue;

	[HideInInspector, SerializeField]
	protected UIProgressBar.FillDirection mFill;

	protected Transform mTrans;

	protected bool mIsDirty;

	protected Camera mCam;

	protected float mOffset;

	public int numberOfSteps;

	public List<EventDelegate> onChange;

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			return this.mCam;
		}
	}

	public UIWidget foregroundWidget
	{
		get
		{
			return this.mFG;
		}
		set
		{
			if (this.mFG != value)
			{
				this.mFG = value;
				this.mIsDirty = true;
			}
		}
	}

	public UIWidget backgroundWidget
	{
		get
		{
			return this.mBG;
		}
		set
		{
			if (this.mBG != value)
			{
				this.mBG = value;
				this.mIsDirty = true;
			}
		}
	}

	public UIProgressBar.FillDirection fillDirection
	{
		get
		{
			return this.mFill;
		}
		set
		{
			if (this.mFill != value)
			{
				this.mFill = value;
				this.ForceUpdate();
			}
		}
	}

	public float value
	{
		get
		{
			if (this.numberOfSteps > 1)
			{
				return Mathf.Round(this.mValue * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
			}
			return this.mValue;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mValue != num)
			{
				float value2 = this.value;
				this.mValue = num;
				if (value2 != this.value)
				{
					this.ForceUpdate();
					if (NGUITools.GetActive(this) && EventDelegate.IsValid(this.onChange))
					{
						UIProgressBar.current = this;
						EventDelegate.Execute(this.onChange);
						UIProgressBar.current = null;
					}
				}
			}
		}
	}

	public float alpha
	{
		get
		{
			if (this.mFG != null)
			{
				return this.mFG.alpha;
			}
			if (this.mBG != null)
			{
				return this.mBG.alpha;
			}
			return 1f;
		}
		set
		{
			if (this.mFG != null)
			{
				this.mFG.alpha = value;
				if (this.mFG.GetComponent<Collider>() != null)
				{
					this.mFG.GetComponent<Collider>().enabled = (this.mFG.alpha > 0.001f);
				}
				else if (this.mFG.GetComponent<Collider2D>() != null)
				{
					this.mFG.GetComponent<Collider2D>().enabled = (this.mFG.alpha > 0.001f);
				}
			}
			if (this.mBG != null)
			{
				this.mBG.alpha = value;
				if (this.mBG.GetComponent<Collider>() != null)
				{
					this.mBG.GetComponent<Collider>().enabled = (this.mBG.alpha > 0.001f);
				}
				else if (this.mBG.GetComponent<Collider2D>() != null)
				{
					this.mBG.GetComponent<Collider2D>().enabled = (this.mBG.alpha > 0.001f);
				}
			}
			if (this.thumb != null)
			{
				UIWidget component = this.thumb.GetComponent<UIWidget>();
				if (component != null)
				{
					component.alpha = value;
					if (component.GetComponent<Collider>() != null)
					{
						component.GetComponent<Collider>().enabled = (component.alpha > 0.001f);
						return;
					}
					if (component.GetComponent<Collider2D>() != null)
					{
						component.GetComponent<Collider2D>().enabled = (component.alpha > 0.001f);
					}
				}
			}
		}
	}

	protected bool isHorizontal
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.LeftToRight || this.mFill == UIProgressBar.FillDirection.RightToLeft;
		}
	}

	protected bool isInverted
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.RightToLeft || this.mFill == UIProgressBar.FillDirection.TopToBottom;
		}
	}

	protected void Start()
	{
		this.Upgrade();
		if (Application.isPlaying)
		{
			if (this.mBG != null)
			{
				this.mBG.autoResizeBoxCollider = true;
			}
			this.OnStart();
			if (UIProgressBar.current == null && this.onChange != null)
			{
				UIProgressBar.current = this;
				EventDelegate.Execute(this.onChange);
				UIProgressBar.current = null;
			}
		}
		this.ForceUpdate();
	}

	protected virtual void Upgrade()
	{
	}

	protected virtual void OnStart()
	{
	}

	protected void Update()
	{
		if (this.mIsDirty)
		{
			this.ForceUpdate();
		}
	}

	protected void OnValidate()
	{
		if (NGUITools.GetActive(this))
		{
			this.Upgrade();
			this.mIsDirty = true;
			float num = Mathf.Clamp01(this.mValue);
			if (this.mValue != num)
			{
				this.mValue = num;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 20)
			{
				this.numberOfSteps = 20;
			}
			this.ForceUpdate();
			return;
		}
		float num2 = Mathf.Clamp01(this.mValue);
		if (this.mValue != num2)
		{
			this.mValue = num2;
		}
		if (this.numberOfSteps < 0)
		{
			this.numberOfSteps = 0;
			return;
		}
		if (this.numberOfSteps > 20)
		{
			this.numberOfSteps = 20;
		}
	}

	protected float ScreenToValue(Vector2 screenPos)
	{
		Transform cachedTransform = this.cachedTransform;
		Plane plane = new Plane(cachedTransform.rotation * Vector3.back, cachedTransform.position);
		Ray ray = this.cachedCamera.ScreenPointToRay(screenPos);
		float distance;
		if (!plane.Raycast(ray, out distance))
		{
			return this.value;
		}
		return this.LocalToValue(cachedTransform.InverseTransformPoint(ray.GetPoint(distance)));
	}

	protected virtual float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return this.value;
		}
		Vector3[] localCorners = this.mFG.localCorners;
		Vector3 vector = localCorners[2] - localCorners[0];
		if (this.isHorizontal)
		{
			float num = (localPos.x - localCorners[0].x) / vector.x;
			if (!this.isInverted)
			{
				return num;
			}
			return 1f - num;
		}
		else
		{
			float num2 = (localPos.y - localCorners[0].y) / vector.y;
			if (!this.isInverted)
			{
				return num2;
			}
			return 1f - num2;
		}
	}

	public virtual void ForceUpdate()
	{
		this.mIsDirty = false;
		bool flag = false;
		if (this.mFG != null)
		{
			UIBasicSprite uIBasicSprite = this.mFG as UIBasicSprite;
			if (this.isHorizontal)
			{
				if (uIBasicSprite != null && uIBasicSprite.type == UIBasicSprite.Type.Filled)
				{
					if (uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
					{
						uIBasicSprite.fillDirection = UIBasicSprite.FillDirection.Horizontal;
						uIBasicSprite.invert = this.isInverted;
					}
					uIBasicSprite.fillAmount = this.value;
				}
				else
				{
					this.mFG.drawRegion = (this.isInverted ? new Vector4(1f - this.value, 0f, 1f, 1f) : new Vector4(0f, 0f, this.value, 1f));
					this.mFG.enabled = true;
					flag = (this.value < 0.001f);
				}
			}
			else if (uIBasicSprite != null && uIBasicSprite.type == UIBasicSprite.Type.Filled)
			{
				if (uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
				{
					uIBasicSprite.fillDirection = UIBasicSprite.FillDirection.Vertical;
					uIBasicSprite.invert = this.isInverted;
				}
				uIBasicSprite.fillAmount = this.value;
			}
			else
			{
				this.mFG.drawRegion = (this.isInverted ? new Vector4(0f, 1f - this.value, 1f, 1f) : new Vector4(0f, 0f, 1f, this.value));
				this.mFG.enabled = true;
				flag = (this.value < 0.001f);
			}
		}
		if (this.thumb != null && (this.mFG != null || this.mBG != null))
		{
			Vector3[] array = (this.mFG != null) ? this.mFG.localCorners : this.mBG.localCorners;
			Vector4 vector = (this.mFG != null) ? this.mFG.border : this.mBG.border;
			Vector3[] expr_21D_cp_0_cp_0 = array;
			int expr_21D_cp_0_cp_1 = 0;
			expr_21D_cp_0_cp_0[expr_21D_cp_0_cp_1].x = expr_21D_cp_0_cp_0[expr_21D_cp_0_cp_1].x + vector.x;
			Vector3[] expr_233_cp_0_cp_0 = array;
			int expr_233_cp_0_cp_1 = 1;
			expr_233_cp_0_cp_0[expr_233_cp_0_cp_1].x = expr_233_cp_0_cp_0[expr_233_cp_0_cp_1].x + vector.x;
			Vector3[] expr_249_cp_0_cp_0 = array;
			int expr_249_cp_0_cp_1 = 2;
			expr_249_cp_0_cp_0[expr_249_cp_0_cp_1].x = expr_249_cp_0_cp_0[expr_249_cp_0_cp_1].x - vector.z;
			Vector3[] expr_25F_cp_0_cp_0 = array;
			int expr_25F_cp_0_cp_1 = 3;
			expr_25F_cp_0_cp_0[expr_25F_cp_0_cp_1].x = expr_25F_cp_0_cp_0[expr_25F_cp_0_cp_1].x - vector.z;
			Vector3[] expr_275_cp_0_cp_0 = array;
			int expr_275_cp_0_cp_1 = 0;
			expr_275_cp_0_cp_0[expr_275_cp_0_cp_1].y = expr_275_cp_0_cp_0[expr_275_cp_0_cp_1].y + vector.y;
			Vector3[] expr_28B_cp_0_cp_0 = array;
			int expr_28B_cp_0_cp_1 = 1;
			expr_28B_cp_0_cp_0[expr_28B_cp_0_cp_1].y = expr_28B_cp_0_cp_0[expr_28B_cp_0_cp_1].y - vector.w;
			Vector3[] expr_2A1_cp_0_cp_0 = array;
			int expr_2A1_cp_0_cp_1 = 2;
			expr_2A1_cp_0_cp_0[expr_2A1_cp_0_cp_1].y = expr_2A1_cp_0_cp_0[expr_2A1_cp_0_cp_1].y - vector.w;
			Vector3[] expr_2B7_cp_0_cp_0 = array;
			int expr_2B7_cp_0_cp_1 = 3;
			expr_2B7_cp_0_cp_0[expr_2B7_cp_0_cp_1].y = expr_2B7_cp_0_cp_0[expr_2B7_cp_0_cp_1].y + vector.y;
			Transform transform = (this.mFG != null) ? this.mFG.cachedTransform : this.mBG.cachedTransform;
			for (int i = 0; i < 4; i++)
			{
				array[i] = transform.TransformPoint(array[i]);
			}
			if (this.isHorizontal)
			{
				Vector3 a = Vector3.Lerp(array[0], array[1], 0.5f);
				Vector3 b = Vector3.Lerp(array[2], array[3], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(a, b, this.isInverted ? (1f - this.value) : this.value));
			}
			else
			{
				Vector3 a2 = Vector3.Lerp(array[0], array[3], 0.5f);
				Vector3 b2 = Vector3.Lerp(array[1], array[2], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(a2, b2, this.isInverted ? (1f - this.value) : this.value));
			}
		}
		if (flag)
		{
			this.mFG.enabled = false;
		}
	}

	protected void SetThumbPosition(Vector3 worldPos)
	{
		Transform parent = this.thumb.parent;
		if (parent != null)
		{
			worldPos = parent.InverseTransformPoint(worldPos);
			worldPos.x = Mathf.Round(worldPos.x);
			worldPos.y = Mathf.Round(worldPos.y);
			worldPos.z = 0f;
			if (Vector3.Distance(this.thumb.localPosition, worldPos) > 0.001f)
			{
				this.thumb.localPosition = worldPos;
				return;
			}
		}
		else if (Vector3.Distance(this.thumb.position, worldPos) > 1E-05f)
		{
			this.thumb.position = worldPos;
		}
	}

	public virtual void OnPan(Vector2 delta)
	{
		if (base.enabled)
		{
			switch (this.mFill)
			{
			case UIProgressBar.FillDirection.LeftToRight:
			{
				float value = Mathf.Clamp01(this.mValue + delta.x);
				this.value = value;
				this.mValue = value;
				return;
			}
			case UIProgressBar.FillDirection.RightToLeft:
			{
				float value2 = Mathf.Clamp01(this.mValue - delta.x);
				this.value = value2;
				this.mValue = value2;
				return;
			}
			case UIProgressBar.FillDirection.BottomToTop:
			{
				float value3 = Mathf.Clamp01(this.mValue + delta.y);
				this.value = value3;
				this.mValue = value3;
				return;
			}
			case UIProgressBar.FillDirection.TopToBottom:
			{
				float value4 = Mathf.Clamp01(this.mValue - delta.y);
				this.value = value4;
				this.mValue = value4;
				break;
			}
			default:
				return;
			}
		}
	}

	public UIProgressBar()
	{
		this.mValue = 1f;
		this.onChange = new List<EventDelegate>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.thumb);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mBG);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mFG);
		}
		SerializedStateWriter.Instance.WriteSingle(this.mValue);
		SerializedStateWriter.Instance.WriteInt32((int)this.mFill);
		SerializedStateWriter.Instance.WriteInt32(this.numberOfSteps);
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
			this.thumb = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		if (depth <= 7)
		{
			this.mBG = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIWidget);
		}
		if (depth <= 7)
		{
			this.mFG = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIWidget);
		}
		this.mValue = SerializedStateReader.Instance.ReadSingle();
		this.mFill = (UIProgressBar.FillDirection)SerializedStateReader.Instance.ReadInt32();
		this.numberOfSteps = SerializedStateReader.Instance.ReadInt32();
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
		if (this.thumb != null)
		{
			this.thumb = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.thumb) as Transform);
		}
		if (this.mBG != null)
		{
			this.mBG = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mBG) as UIWidget);
		}
		if (this.mFG != null)
		{
			this.mFG = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mFG) as UIWidget);
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
			UnityEngine.Object arg_23_1 = this.thumb;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 1570);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mBG, &var_0_cp_0[var_0_cp_1] + 1576);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mFG, &var_0_cp_0[var_0_cp_1] + 1580);
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.mValue, &var_0_cp_0[var_0_cp_1] + 1584);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mFill, &var_0_cp_0[var_0_cp_1] + 1591);
		SerializedNamedStateWriter.Instance.WriteInt32(this.numberOfSteps, &var_0_cp_0[var_0_cp_1] + 1597);
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
					EventDelegate arg_131_0 = (this.onChange[i] != null) ? this.onChange[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_131_0.Unity_NamedSerialize(depth + 1);
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
			ISerializedNamedStateReader arg_1E_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			this.thumb = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1570) as Transform);
		}
		if (depth <= 7)
		{
			this.mBG = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1576) as UIWidget);
		}
		if (depth <= 7)
		{
			this.mFG = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1580) as UIWidget);
		}
		this.mValue = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1584);
		this.mFill = (UIProgressBar.FillDirection)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1591);
		this.numberOfSteps = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1597);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446);
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_F8_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_F8_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onChange.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal UIProgressBar(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)instance).thumb);
	}

	public static void $Set0(object instance, long value)
	{
		((UIProgressBar)instance).thumb = (Transform)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)instance).mBG);
	}

	public static void $Set1(object instance, long value)
	{
		((UIProgressBar)instance).mBG = (UIWidget)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)instance).mFG);
	}

	public static void $Set2(object instance, long value)
	{
		((UIProgressBar)instance).mFG = (UIWidget)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get3(object instance)
	{
		return ((UIProgressBar)instance).mValue;
	}

	public static void $Set3(object instance, float value)
	{
		((UIProgressBar)instance).mValue = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).ForceUpdate();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).alpha);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).backgroundWidget);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).cachedCamera);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).cachedTransform);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).fillDirection);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).foregroundWidget);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).isHorizontal);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).isInverted);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).value);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).LocalToValue(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).OnPan(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).OnStart();
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).OnValidate();
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).ScreenToValue(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).alpha = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).backgroundWidget = (UIWidget)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).fillDirection = (UIProgressBar.FillDirection)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).foregroundWidget = (UIWidget)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).value = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).SetThumbPosition(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((UIProgressBar)GCHandledObjects.GCHandleToObject(instance)).Upgrade();
		return -1L;
	}
}
