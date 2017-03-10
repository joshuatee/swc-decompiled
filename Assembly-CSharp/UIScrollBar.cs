using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/NGUI Scroll Bar"), ExecuteInEditMode]
public class UIScrollBar : UISlider, IUnitySerializable
{
	private enum Direction
	{
		Horizontal,
		Vertical,
		Upgraded
	}

	[HideInInspector, SerializeField]
	protected float mSize;

	[HideInInspector, SerializeField]
	protected internal float mScroll;

	[HideInInspector, SerializeField]
	protected internal UIScrollBar.Direction mDir;

	[Obsolete("Use 'value' instead")]
	public float scrollValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	public float barSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mSize != num)
			{
				this.mSize = num;
				this.mIsDirty = true;
				if (NGUITools.GetActive(this))
				{
					if (UIProgressBar.current == null && this.onChange != null)
					{
						UIProgressBar.current = this;
						EventDelegate.Execute(this.onChange);
						UIProgressBar.current = null;
					}
					this.ForceUpdate();
				}
			}
		}
	}

	protected override void Upgrade()
	{
		if (this.mDir != UIScrollBar.Direction.Upgraded)
		{
			this.mValue = this.mScroll;
			if (this.mDir == UIScrollBar.Direction.Horizontal)
			{
				this.mFill = (this.mInverted ? UIProgressBar.FillDirection.RightToLeft : UIProgressBar.FillDirection.LeftToRight);
			}
			else
			{
				this.mFill = (this.mInverted ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom);
			}
			this.mDir = UIScrollBar.Direction.Upgraded;
		}
	}

	protected override void OnStart()
	{
		base.OnStart();
		if (this.mFG != null && this.mFG.gameObject != base.gameObject)
		{
			if (!(this.mFG.GetComponent<Collider>() != null) && !(this.mFG.GetComponent<Collider2D>() != null))
			{
				return;
			}
			UIEventListener uIEventListener = UIEventListener.Get(this.mFG.gameObject);
			UIEventListener expr_70 = uIEventListener;
			expr_70.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_70.onPress, new UIEventListener.BoolDelegate(base.OnPressForeground));
			UIEventListener expr_92 = uIEventListener;
			expr_92.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_92.onDrag, new UIEventListener.VectorDelegate(base.OnDragForeground));
			this.mFG.autoResizeBoxCollider = true;
		}
	}

	protected override float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return base.LocalToValue(localPos);
		}
		float num = Mathf.Clamp01(this.mSize) * 0.5f;
		float num2 = num;
		float num3 = 1f - num;
		Vector3[] localCorners = this.mFG.localCorners;
		if (base.isHorizontal)
		{
			num2 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, num2);
			num3 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, num3);
			float num4 = num3 - num2;
			if (num4 == 0f)
			{
				return base.value;
			}
			if (!base.isInverted)
			{
				return (localPos.x - num2) / num4;
			}
			return (num3 - localPos.x) / num4;
		}
		else
		{
			num2 = Mathf.Lerp(localCorners[0].y, localCorners[1].y, num2);
			num3 = Mathf.Lerp(localCorners[3].y, localCorners[2].y, num3);
			float num5 = num3 - num2;
			if (num5 == 0f)
			{
				return base.value;
			}
			if (!base.isInverted)
			{
				return (localPos.y - num2) / num5;
			}
			return (num3 - localPos.y) / num5;
		}
	}

	public override void ForceUpdate()
	{
		if (this.mFG != null)
		{
			this.mIsDirty = false;
			float num = Mathf.Clamp01(this.mSize) * 0.5f;
			float num2 = Mathf.Lerp(num, 1f - num, base.value);
			float num3 = num2 - num;
			float num4 = num2 + num;
			if (base.isHorizontal)
			{
				this.mFG.drawRegion = (base.isInverted ? new Vector4(1f - num4, 0f, 1f - num3, 1f) : new Vector4(num3, 0f, num4, 1f));
			}
			else
			{
				this.mFG.drawRegion = (base.isInverted ? new Vector4(0f, 1f - num4, 1f, 1f - num3) : new Vector4(0f, num3, 1f, num4));
			}
			if (this.thumb != null)
			{
				Vector4 drawingDimensions = this.mFG.drawingDimensions;
				Vector3 position = new Vector3(Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, 0.5f), Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, 0.5f));
				base.SetThumbPosition(this.mFG.cachedTransform.TransformPoint(position));
				return;
			}
		}
		else
		{
			base.ForceUpdate();
		}
	}

	public UIScrollBar()
	{
		this.mSize = 1f;
		this.mDir = UIScrollBar.Direction.Upgraded;
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
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.foreground);
		}
		SerializedStateWriter.Instance.WriteSingle(this.rawValue);
		SerializedStateWriter.Instance.WriteInt32((int)this.direction);
		SerializedStateWriter.Instance.WriteBoolean(this.mInverted);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.mSize);
		SerializedStateWriter.Instance.WriteSingle(this.mScroll);
		SerializedStateWriter.Instance.WriteInt32((int)this.mDir);
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
		if (depth <= 7)
		{
			this.foreground = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
		}
		this.rawValue = SerializedStateReader.Instance.ReadSingle();
		this.direction = (UISlider.Direction)SerializedStateReader.Instance.ReadInt32();
		this.mInverted = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.mSize = SerializedStateReader.Instance.ReadSingle();
		this.mScroll = SerializedStateReader.Instance.ReadSingle();
		this.mDir = (UIScrollBar.Direction)SerializedStateReader.Instance.ReadInt32();
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
		if (this.foreground != null)
		{
			this.foreground = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.foreground) as Transform);
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
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.foreground, &var_0_cp_0[var_0_cp_1] + 1619);
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.rawValue, &var_0_cp_0[var_0_cp_1] + 1630);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.direction, &var_0_cp_0[var_0_cp_1] + 1639);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mInverted, &var_0_cp_0[var_0_cp_1] + 1649);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.mSize, &var_0_cp_0[var_0_cp_1] + 1659);
		SerializedNamedStateWriter.Instance.WriteSingle(this.mScroll, &var_0_cp_0[var_0_cp_1] + 1665);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mDir, &var_0_cp_0[var_0_cp_1] + 1673);
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
		if (depth <= 7)
		{
			this.foreground = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1619) as Transform);
		}
		this.rawValue = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1630);
		this.direction = (UISlider.Direction)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1639);
		this.mInverted = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1649);
		SerializedNamedStateReader.Instance.Align();
		this.mSize = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1659);
		this.mScroll = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1665);
		this.mDir = (UIScrollBar.Direction)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1673);
	}

	protected internal UIScrollBar(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance)
	{
		return ((UIScrollBar)instance).mSize;
	}

	public static void $Set0(object instance, float value)
	{
		((UIScrollBar)instance).mSize = value;
	}

	public static float $Get1(object instance)
	{
		return ((UIScrollBar)instance).mScroll;
	}

	public static void $Set1(object instance, float value)
	{
		((UIScrollBar)instance).mScroll = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).ForceUpdate();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).barSize);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).scrollValue);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).LocalToValue(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).OnStart();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).barSize = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).scrollValue = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIScrollBar)GCHandledObjects.GCHandleToObject(instance)).Upgrade();
		return -1L;
	}
}
