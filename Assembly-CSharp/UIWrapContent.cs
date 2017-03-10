using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class UIWrapContent : MonoBehaviour, IUnitySerializable
{
	public delegate void OnInitializeItem(GameObject go, int wrapIndex, int realIndex);

	public int itemSize;

	public bool cullContent;

	public int minIndex;

	public int maxIndex;

	public UIWrapContent.OnInitializeItem onInitializeItem;

	protected Transform mTrans;

	protected UIPanel mPanel;

	protected UIScrollView mScroll;

	protected bool mHorizontal;

	protected bool mFirstTime;

	protected List<Transform> mChildren;

	protected virtual void Start()
	{
		this.SortBasedOnScrollMovement();
		this.WrapContent();
		if (this.mScroll != null)
		{
			this.mScroll.GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved(this.OnMove);
		}
		this.mFirstTime = false;
	}

	protected virtual void OnMove(UIPanel panel)
	{
		this.WrapContent();
	}

	[ContextMenu("Sort Based on Scroll Movement")]
	public virtual void SortBasedOnScrollMovement()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		if (this.mHorizontal)
		{
			this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
		}
		else
		{
			this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortVertical));
		}
		this.ResetChildPositions();
	}

	[ContextMenu("Sort Alphabetically")]
	public virtual void SortAlphabetically()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortByName));
		this.ResetChildPositions();
	}

	protected bool CacheScrollView()
	{
		this.mTrans = base.transform;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		this.mScroll = this.mPanel.GetComponent<UIScrollView>();
		if (this.mScroll == null)
		{
			return false;
		}
		if (this.mScroll.movement == UIScrollView.Movement.Horizontal)
		{
			this.mHorizontal = true;
		}
		else
		{
			if (this.mScroll.movement != UIScrollView.Movement.Vertical)
			{
				return false;
			}
			this.mHorizontal = false;
		}
		return true;
	}

	protected virtual void ResetChildPositions()
	{
		int i = 0;
		int count = this.mChildren.Count;
		while (i < count)
		{
			Transform transform = this.mChildren[i];
			transform.localPosition = (this.mHorizontal ? new Vector3((float)(i * this.itemSize), 0f, 0f) : new Vector3(0f, (float)(-(float)i * this.itemSize), 0f));
			this.UpdateItem(transform, i);
			i++;
		}
	}

	public virtual void WrapContent()
	{
		float num = (float)(this.itemSize * this.mChildren.Count) * 0.5f;
		Vector3[] worldCorners = this.mPanel.worldCorners;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = worldCorners[i];
			vector = this.mTrans.InverseTransformPoint(vector);
			worldCorners[i] = vector;
		}
		Vector3 vector2 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
		bool flag = true;
		float num2 = num * 2f;
		if (this.mHorizontal)
		{
			float num3 = worldCorners[0].x - (float)this.itemSize;
			float num4 = worldCorners[2].x + (float)this.itemSize;
			int j = 0;
			int count = this.mChildren.Count;
			while (j < count)
			{
				Transform transform = this.mChildren[j];
				float num5 = transform.localPosition.x - vector2.x;
				if (num5 < -num)
				{
					Vector3 localPosition = transform.localPosition;
					localPosition.x += num2;
					num5 = localPosition.x - vector2.x;
					int num6 = Mathf.RoundToInt(localPosition.x / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num6 && num6 <= this.maxIndex))
					{
						transform.localPosition = localPosition;
						this.UpdateItem(transform, j);
					}
					else
					{
						flag = false;
					}
				}
				else if (num5 > num)
				{
					Vector3 localPosition2 = transform.localPosition;
					localPosition2.x -= num2;
					num5 = localPosition2.x - vector2.x;
					int num7 = Mathf.RoundToInt(localPosition2.x / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num7 && num7 <= this.maxIndex))
					{
						transform.localPosition = localPosition2;
						this.UpdateItem(transform, j);
					}
					else
					{
						flag = false;
					}
				}
				else if (this.mFirstTime)
				{
					this.UpdateItem(transform, j);
				}
				if (this.cullContent)
				{
					num5 += this.mPanel.clipOffset.x - this.mTrans.localPosition.x;
					if (!UICamera.IsPressed(transform.gameObject))
					{
						NGUITools.SetActive(transform.gameObject, num5 > num3 && num5 < num4, false);
					}
				}
				j++;
			}
		}
		else
		{
			float num8 = worldCorners[0].y - (float)this.itemSize;
			float num9 = worldCorners[2].y + (float)this.itemSize;
			int k = 0;
			int count2 = this.mChildren.Count;
			while (k < count2)
			{
				Transform transform2 = this.mChildren[k];
				float num10 = transform2.localPosition.y - vector2.y;
				if (num10 < -num)
				{
					Vector3 localPosition3 = transform2.localPosition;
					localPosition3.y += num2;
					num10 = localPosition3.y - vector2.y;
					int num11 = Mathf.RoundToInt(localPosition3.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num11 && num11 <= this.maxIndex))
					{
						transform2.localPosition = localPosition3;
						this.UpdateItem(transform2, k);
					}
					else
					{
						flag = false;
					}
				}
				else if (num10 > num)
				{
					Vector3 localPosition4 = transform2.localPosition;
					localPosition4.y -= num2;
					num10 = localPosition4.y - vector2.y;
					int num12 = Mathf.RoundToInt(localPosition4.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num12 && num12 <= this.maxIndex))
					{
						transform2.localPosition = localPosition4;
						this.UpdateItem(transform2, k);
					}
					else
					{
						flag = false;
					}
				}
				else if (this.mFirstTime)
				{
					this.UpdateItem(transform2, k);
				}
				if (this.cullContent)
				{
					num10 += this.mPanel.clipOffset.y - this.mTrans.localPosition.y;
					if (!UICamera.IsPressed(transform2.gameObject))
					{
						NGUITools.SetActive(transform2.gameObject, num10 > num8 && num10 < num9, false);
					}
				}
				k++;
			}
		}
		this.mScroll.restrictWithinPanel = !flag;
	}

	private void OnValidate()
	{
		if (this.maxIndex < this.minIndex)
		{
			this.maxIndex = this.minIndex;
		}
		if (this.minIndex > this.maxIndex)
		{
			this.maxIndex = this.minIndex;
		}
	}

	protected virtual void UpdateItem(Transform item, int index)
	{
		if (this.onInitializeItem != null)
		{
			int realIndex = (this.mScroll.movement == UIScrollView.Movement.Vertical) ? Mathf.RoundToInt(item.localPosition.y / (float)this.itemSize) : Mathf.RoundToInt(item.localPosition.x / (float)this.itemSize);
			this.onInitializeItem(item.gameObject, index, realIndex);
		}
	}

	public UIWrapContent()
	{
		this.itemSize = 100;
		this.cullContent = true;
		this.mFirstTime = true;
		this.mChildren = new List<Transform>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32(this.itemSize);
		SerializedStateWriter.Instance.WriteBoolean(this.cullContent);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32(this.minIndex);
		SerializedStateWriter.Instance.WriteInt32(this.maxIndex);
	}

	public override void Unity_Deserialize(int depth)
	{
		this.itemSize = SerializedStateReader.Instance.ReadInt32();
		this.cullContent = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.minIndex = SerializedStateReader.Instance.ReadInt32();
		this.maxIndex = SerializedStateReader.Instance.ReadInt32();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = this.itemSize;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 2053);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.cullContent, &var_0_cp_0[var_0_cp_1] + 2062);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32(this.minIndex, &var_0_cp_0[var_0_cp_1] + 2074);
		SerializedNamedStateWriter.Instance.WriteInt32(this.maxIndex, &var_0_cp_0[var_0_cp_1] + 2083);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.itemSize = arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2053);
		this.cullContent = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2062);
		SerializedNamedStateReader.Instance.Align();
		this.minIndex = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2074);
		this.maxIndex = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2083);
	}

	protected internal UIWrapContent(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UIWrapContent)instance).cullContent;
	}

	public static void $Set0(object instance, bool value)
	{
		((UIWrapContent)instance).cullContent = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).CacheScrollView());
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).OnMove((UIPanel)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).OnValidate();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).ResetChildPositions();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).SortAlphabetically();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).SortBasedOnScrollMovement();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).UpdateItem((Transform)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIWrapContent)GCHandledObjects.GCHandleToObject(instance)).WrapContent();
		return -1L;
	}
}
