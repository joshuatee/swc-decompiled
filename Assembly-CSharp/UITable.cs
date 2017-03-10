using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : UIWidgetContainer, IUnitySerializable
{
	public delegate void OnReposition();

	public enum Direction
	{
		Down,
		Up
	}

	public enum Sorting
	{
		None,
		Alphabetic,
		Horizontal,
		Vertical,
		Custom
	}

	public int columns;

	public UITable.Direction direction;

	public UITable.Sorting sorting;

	public UIWidget.Pivot pivot;

	public UIWidget.Pivot cellAlignment;

	public bool hideInactive;

	public bool keepWithinPanel;

	public Vector2 padding;

	public UITable.OnReposition onReposition;

	public Comparison<Transform> onCustomSort;

	protected UIPanel mPanel;

	protected bool mInitDone;

	protected bool mReposition;

	public bool repositionNow
	{
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.enabled = true;
			}
		}
	}

	public List<Transform> GetChildList()
	{
		Transform transform = base.transform;
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!this.hideInactive || (child && NGUITools.GetActive(child.gameObject)))
			{
				list.Add(child);
			}
		}
		if (this.sorting != UITable.Sorting.None)
		{
			if (this.sorting == UITable.Sorting.Alphabetic)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UITable.Sorting.Horizontal)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UITable.Sorting.Vertical)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortVertical));
			}
			else if (this.onCustomSort != null)
			{
				list.Sort(this.onCustomSort);
			}
			else
			{
				this.Sort(list);
			}
		}
		return list;
	}

	protected virtual void Sort(List<Transform> list)
	{
		list.Sort(new Comparison<Transform>(UIGrid.SortByName));
	}

	protected virtual void Start()
	{
		this.Init();
		this.Reposition();
		base.enabled = false;
	}

	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	protected virtual void LateUpdate()
	{
		if (this.mReposition)
		{
			this.Reposition();
		}
		base.enabled = false;
	}

	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	protected void RepositionVariableSize(List<Transform> children)
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = (this.columns > 0) ? (children.Count / this.columns + 1) : 1;
		int num4 = (this.columns > 0) ? this.columns : children.Count;
		Bounds[,] array = new Bounds[num3, num4];
		Bounds[] array2 = new Bounds[num4];
		Bounds[] array3 = new Bounds[num3];
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int count = children.Count;
		while (i < count)
		{
			Transform transform = children[i];
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform, !this.hideInactive);
			Vector3 localScale = transform.localScale;
			bounds.min = Vector3.Scale(bounds.min, localScale);
			bounds.max = Vector3.Scale(bounds.max, localScale);
			array[num6, num5] = bounds;
			array2[num5].Encapsulate(bounds);
			array3[num6].Encapsulate(bounds);
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
			}
			i++;
		}
		num5 = 0;
		num6 = 0;
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.cellAlignment);
		int j = 0;
		int count2 = children.Count;
		while (j < count2)
		{
			Transform transform2 = children[j];
			Bounds bounds2 = array[num6, num5];
			Bounds bounds3 = array2[num5];
			Bounds bounds4 = array3[num6];
			Vector3 localPosition = transform2.localPosition;
			localPosition.x = num + bounds2.extents.x - bounds2.center.x;
			localPosition.x -= Mathf.Lerp(0f, bounds2.max.x - bounds2.min.x - bounds3.max.x + bounds3.min.x, pivotOffset.x) - this.padding.x;
			if (this.direction == UITable.Direction.Down)
			{
				localPosition.y = -num2 - bounds2.extents.y - bounds2.center.y;
				localPosition.y += Mathf.Lerp(bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y, 0f, pivotOffset.y) - this.padding.y;
			}
			else
			{
				localPosition.y = num2 + bounds2.extents.y - bounds2.center.y;
				localPosition.y -= Mathf.Lerp(0f, bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y, pivotOffset.y) - this.padding.y;
			}
			num += bounds3.size.x + this.padding.x * 2f;
			transform2.localPosition = localPosition;
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
				num = 0f;
				num2 += bounds4.size.y + this.padding.y * 2f;
			}
			j++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			Bounds bounds5 = NGUIMath.CalculateRelativeWidgetBounds(base.transform);
			float num7 = Mathf.Lerp(0f, bounds5.size.x, pivotOffset.x);
			float num8 = Mathf.Lerp(-bounds5.size.y, 0f, pivotOffset.y);
			Transform transform3 = base.transform;
			for (int k = 0; k < transform3.childCount; k++)
			{
				Transform child = transform3.GetChild(k);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					SpringPosition expr_415_cp_0_cp_0 = component;
					expr_415_cp_0_cp_0.target.x = expr_415_cp_0_cp_0.target.x - num7;
					SpringPosition expr_427_cp_0_cp_0 = component;
					expr_427_cp_0_cp_0.target.y = expr_427_cp_0_cp_0.target.y - num8;
				}
				else
				{
					Vector3 localPosition2 = child.localPosition;
					localPosition2.x -= num7;
					localPosition2.y -= num8;
					child.localPosition = localPosition2;
				}
			}
		}
	}

	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this))
		{
			this.Init();
		}
		this.mReposition = false;
		Transform transform = base.transform;
		List<Transform> childList = this.GetChildList();
		if (childList.Count > 0)
		{
			this.RepositionVariableSize(childList);
		}
		if (this.keepWithinPanel && this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	public UITable()
	{
		this.hideInactive = true;
		this.padding = Vector2.zero;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32(this.columns);
		SerializedStateWriter.Instance.WriteInt32((int)this.direction);
		SerializedStateWriter.Instance.WriteInt32((int)this.sorting);
		SerializedStateWriter.Instance.WriteInt32((int)this.pivot);
		SerializedStateWriter.Instance.WriteInt32((int)this.cellAlignment);
		SerializedStateWriter.Instance.WriteBoolean(this.hideInactive);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.keepWithinPanel);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.padding.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		this.columns = SerializedStateReader.Instance.ReadInt32();
		this.direction = (UITable.Direction)SerializedStateReader.Instance.ReadInt32();
		this.sorting = (UITable.Sorting)SerializedStateReader.Instance.ReadInt32();
		this.pivot = (UIWidget.Pivot)SerializedStateReader.Instance.ReadInt32();
		this.cellAlignment = (UIWidget.Pivot)SerializedStateReader.Instance.ReadInt32();
		this.hideInactive = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.keepWithinPanel = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.padding.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = this.columns;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 1879);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.direction, &var_0_cp_0[var_0_cp_1] + 1639);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.sorting, &var_0_cp_0[var_0_cp_1] + 940);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.pivot, &var_0_cp_0[var_0_cp_1] + 697);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.cellAlignment, &var_0_cp_0[var_0_cp_1] + 1887);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.hideInactive, &var_0_cp_0[var_0_cp_1] + 996);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.keepWithinPanel, &var_0_cp_0[var_0_cp_1] + 1009);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1353);
			this.padding.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.columns = arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1879);
		this.direction = (UITable.Direction)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1639);
		this.sorting = (UITable.Sorting)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 940);
		this.pivot = (UIWidget.Pivot)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 697);
		this.cellAlignment = (UIWidget.Pivot)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1887);
		this.hideInactive = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 996);
		SerializedNamedStateReader.Instance.Align();
		this.keepWithinPanel = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1009);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1353);
			this.padding.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UITable(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UITable)instance).hideInactive;
	}

	public static void $Set0(object instance, bool value)
	{
		((UITable)instance).hideInactive = value;
	}

	public static bool $Get1(object instance)
	{
		return ((UITable)instance).keepWithinPanel;
	}

	public static void $Set1(object instance, bool value)
	{
		((UITable)instance).keepWithinPanel = value;
	}

	public static float $Get2(object instance, int index)
	{
		UITable expr_06_cp_0 = (UITable)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.padding.x;
		case 1:
			return expr_06_cp_0.padding.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set2(object instance, float value, int index)
	{
		UITable expr_06_cp_0 = (UITable)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.padding.x = value;
			return;
		case 1:
			expr_06_cp_0.padding.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITable)GCHandledObjects.GCHandleToObject(instance)).GetChildList());
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).Init();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).OnValidate();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).Reposition();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).RepositionVariableSize((List<Transform>)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).repositionNow = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).Sort((List<Transform>)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UITable)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
