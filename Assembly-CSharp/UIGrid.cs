using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer, IUnitySerializable
{
	public delegate void OnReposition();

	public enum Arrangement
	{
		Horizontal,
		Vertical,
		CellSnap
	}

	public enum Sorting
	{
		None,
		Alphabetic,
		Horizontal,
		Vertical,
		Custom
	}

	public UIGrid.Arrangement arrangement;

	public UIGrid.Sorting sorting;

	public UIWidget.Pivot pivot;

	public int maxPerLine;

	public float cellWidth;

	public float cellHeight;

	public bool animateSmoothly;

	public bool hideInactive;

	public bool keepWithinPanel;

	public UIGrid.OnReposition onReposition;

	public Comparison<Transform> onCustomSort;

	[HideInInspector, SerializeField]
	protected internal bool sorted;

	protected bool mReposition;

	protected UIPanel mPanel;

	protected bool mInitDone;

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
		if (this.sorting != UIGrid.Sorting.None && this.arrangement != UIGrid.Arrangement.CellSnap)
		{
			if (this.sorting == UIGrid.Sorting.Alphabetic)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UIGrid.Sorting.Horizontal)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UIGrid.Sorting.Vertical)
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

	public Transform GetChild(int index)
	{
		List<Transform> childList = this.GetChildList();
		if (index >= childList.Count)
		{
			return null;
		}
		return childList[index];
	}

	public int GetIndex(Transform trans)
	{
		return this.GetChildList().IndexOf(trans);
	}

	public void AddChild(Transform trans)
	{
		this.AddChild(trans, true);
	}

	public void AddChild(Transform trans, bool sort)
	{
		if (trans != null)
		{
			trans.parent = base.transform;
			this.ResetPosition(this.GetChildList());
		}
	}

	public bool RemoveChild(Transform t)
	{
		List<Transform> childList = this.GetChildList();
		if (childList.Remove(t))
		{
			this.ResetPosition(childList);
			return true;
		}
		return false;
	}

	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	protected virtual void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		bool flag = this.animateSmoothly;
		this.animateSmoothly = false;
		this.Reposition();
		this.animateSmoothly = flag;
		base.enabled = false;
	}

	protected virtual void Update()
	{
		this.Reposition();
		base.enabled = false;
	}

	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	public static int SortHorizontal(Transform a, Transform b)
	{
		return a.localPosition.x.CompareTo(b.localPosition.x);
	}

	public static int SortVertical(Transform a, Transform b)
	{
		return b.localPosition.y.CompareTo(a.localPosition.y);
	}

	protected virtual void Sort(List<Transform> list)
	{
	}

	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(base.gameObject))
		{
			this.Init();
		}
		if (this.sorted)
		{
			this.sorted = false;
			if (this.sorting == UIGrid.Sorting.None)
			{
				this.sorting = UIGrid.Sorting.Alphabetic;
			}
			NGUITools.SetDirty(this);
		}
		List<Transform> childList = this.GetChildList();
		this.ResetPosition(childList);
		if (this.keepWithinPanel)
		{
			this.ConstrainWithinPanel();
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	public void ConstrainWithinPanel()
	{
		if (this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(base.transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
	}

	protected virtual void ResetPosition(List<Transform> list)
	{
		this.mReposition = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		Transform transform = base.transform;
		int i = 0;
		int count = list.Count;
		while (i < count)
		{
			Transform transform2 = list[i];
			Vector3 vector = transform2.localPosition;
			float z = vector.z;
			if (this.arrangement == UIGrid.Arrangement.CellSnap)
			{
				if (this.cellWidth > 0f)
				{
					vector.x = Mathf.Round(vector.x / this.cellWidth) * this.cellWidth;
				}
				if (this.cellHeight > 0f)
				{
					vector.y = Mathf.Round(vector.y / this.cellHeight) * this.cellHeight;
				}
			}
			else
			{
				vector = ((this.arrangement == UIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z) : new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z));
			}
			if (this.animateSmoothly && Application.isPlaying && Vector3.SqrMagnitude(transform2.localPosition - vector) >= 0.0001f)
			{
				SpringPosition springPosition = SpringPosition.Begin(transform2.gameObject, vector, 15f);
				springPosition.updateScrollView = true;
				springPosition.ignoreTimeScale = true;
			}
			else
			{
				transform2.localPosition = vector;
			}
			num3 = Mathf.Max(num3, num);
			num4 = Mathf.Max(num4, num2);
			if (++num >= this.maxPerLine && this.maxPerLine > 0)
			{
				num = 0;
				num2++;
			}
			i++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			float num5;
			float num6;
			if (this.arrangement == UIGrid.Arrangement.Horizontal)
			{
				num5 = Mathf.Lerp(0f, (float)num3 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num4) * this.cellHeight, 0f, pivotOffset.y);
			}
			else
			{
				num5 = Mathf.Lerp(0f, (float)num4 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num3) * this.cellHeight, 0f, pivotOffset.y);
			}
			for (int j = 0; j < transform.childCount; j++)
			{
				Transform child = transform.GetChild(j);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					SpringPosition expr_241_cp_0_cp_0 = component;
					expr_241_cp_0_cp_0.target.x = expr_241_cp_0_cp_0.target.x - num5;
					SpringPosition expr_253_cp_0_cp_0 = component;
					expr_253_cp_0_cp_0.target.y = expr_253_cp_0_cp_0.target.y - num6;
				}
				else
				{
					Vector3 localPosition = child.localPosition;
					localPosition.x -= num5;
					localPosition.y -= num6;
					child.localPosition = localPosition;
				}
			}
		}
	}

	public UIGrid()
	{
		this.cellWidth = 200f;
		this.cellHeight = 200f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32((int)this.arrangement);
		SerializedStateWriter.Instance.WriteInt32((int)this.sorting);
		SerializedStateWriter.Instance.WriteInt32((int)this.pivot);
		SerializedStateWriter.Instance.WriteInt32(this.maxPerLine);
		SerializedStateWriter.Instance.WriteSingle(this.cellWidth);
		SerializedStateWriter.Instance.WriteSingle(this.cellHeight);
		SerializedStateWriter.Instance.WriteBoolean(this.animateSmoothly);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.hideInactive);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.keepWithinPanel);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.sorted);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		this.arrangement = (UIGrid.Arrangement)SerializedStateReader.Instance.ReadInt32();
		this.sorting = (UIGrid.Sorting)SerializedStateReader.Instance.ReadInt32();
		this.pivot = (UIWidget.Pivot)SerializedStateReader.Instance.ReadInt32();
		this.maxPerLine = SerializedStateReader.Instance.ReadInt32();
		this.cellWidth = SerializedStateReader.Instance.ReadSingle();
		this.cellHeight = SerializedStateReader.Instance.ReadSingle();
		this.animateSmoothly = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.hideInactive = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.keepWithinPanel = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.sorted = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = (int)this.arrangement;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 928);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.sorting, &var_0_cp_0[var_0_cp_1] + 940);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.pivot, &var_0_cp_0[var_0_cp_1] + 697);
		SerializedNamedStateWriter.Instance.WriteInt32(this.maxPerLine, &var_0_cp_0[var_0_cp_1] + 948);
		SerializedNamedStateWriter.Instance.WriteSingle(this.cellWidth, &var_0_cp_0[var_0_cp_1] + 959);
		SerializedNamedStateWriter.Instance.WriteSingle(this.cellHeight, &var_0_cp_0[var_0_cp_1] + 969);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.animateSmoothly, &var_0_cp_0[var_0_cp_1] + 980);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.hideInactive, &var_0_cp_0[var_0_cp_1] + 996);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.keepWithinPanel, &var_0_cp_0[var_0_cp_1] + 1009);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.sorted, &var_0_cp_0[var_0_cp_1] + 1025);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.arrangement = (UIGrid.Arrangement)arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 928);
		this.sorting = (UIGrid.Sorting)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 940);
		this.pivot = (UIWidget.Pivot)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 697);
		this.maxPerLine = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 948);
		this.cellWidth = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 959);
		this.cellHeight = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 969);
		this.animateSmoothly = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 980);
		SerializedNamedStateReader.Instance.Align();
		this.hideInactive = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 996);
		SerializedNamedStateReader.Instance.Align();
		this.keepWithinPanel = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1009);
		SerializedNamedStateReader.Instance.Align();
		this.sorted = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1025);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIGrid(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance)
	{
		return ((UIGrid)instance).cellWidth;
	}

	public static void $Set0(object instance, float value)
	{
		((UIGrid)instance).cellWidth = value;
	}

	public static float $Get1(object instance)
	{
		return ((UIGrid)instance).cellHeight;
	}

	public static void $Set1(object instance, float value)
	{
		((UIGrid)instance).cellHeight = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UIGrid)instance).animateSmoothly;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIGrid)instance).animateSmoothly = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UIGrid)instance).hideInactive;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIGrid)instance).hideInactive = value;
	}

	public static bool $Get4(object instance)
	{
		return ((UIGrid)instance).keepWithinPanel;
	}

	public static void $Set4(object instance, bool value)
	{
		((UIGrid)instance).keepWithinPanel = value;
	}

	public static bool $Get5(object instance)
	{
		return ((UIGrid)instance).sorted;
	}

	public static void $Set5(object instance, bool value)
	{
		((UIGrid)instance).sorted = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).AddChild((Transform)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).AddChild((Transform)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).ConstrainWithinPanel();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIGrid)GCHandledObjects.GCHandleToObject(instance)).GetChild(*(int*)args));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIGrid)GCHandledObjects.GCHandleToObject(instance)).GetChildList());
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIGrid)GCHandledObjects.GCHandleToObject(instance)).GetIndex((Transform)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Init();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).OnValidate();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIGrid)GCHandledObjects.GCHandleToObject(instance)).RemoveChild((Transform)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Reposition();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).ResetPosition((List<Transform>)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).repositionNow = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Sort((List<Transform>)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIGrid.SortByName((Transform)GCHandledObjects.GCHandleToObject(*args), (Transform)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIGrid.SortHorizontal((Transform)GCHandledObjects.GCHandleToObject(*args), (Transform)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIGrid.SortVertical((Transform)GCHandledObjects.GCHandleToObject(*args), (Transform)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UIGrid)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
