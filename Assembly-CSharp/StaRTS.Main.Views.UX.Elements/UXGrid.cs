using StaRTS.Main.Views.Cameras;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXGrid : AbstractUXList
	{
		public delegate void OnCentered(UXElement element, int index);

		private UXGrid.OnCentered onCentered;

		private UXGrid.OnCentered onCenteredFinished;

		private UXGridComponent gridComponent;

		private Comparison<UXElement> comparisonCallback;

		public Action RepositionCallback
		{
			get
			{
				return this.gridComponent.RepositionCallback;
			}
			set
			{
				this.gridComponent.RepositionCallback = value;
			}
		}

		public float CellWidth
		{
			get
			{
				return this.gridComponent.CellWidth * this.uxCamera.Scale;
			}
			set
			{
				this.gridComponent.CellWidth = value / this.uxCamera.Scale;
			}
		}

		public float CellHeight
		{
			get
			{
				return this.gridComponent.CellHeight * this.uxCamera.Scale;
			}
			set
			{
				this.gridComponent.CellHeight = value / this.uxCamera.Scale;
			}
		}

		public int MaxItemsPerLine
		{
			get
			{
				return this.gridComponent.MaxItemsPerLine;
			}
			set
			{
				this.gridComponent.MaxItemsPerLine = value;
			}
		}

		public bool IsScrollable
		{
			get
			{
				if (this.gridComponent.transform.parent != null)
				{
					UIScrollView component = this.gridComponent.transform.parent.GetComponent<UIScrollView>();
					return component != null && component.enabled;
				}
				return false;
			}
			set
			{
				if (this.gridComponent.transform.parent != null)
				{
					UIScrollView component = this.gridComponent.transform.parent.GetComponent<UIScrollView>();
					if (component != null)
					{
						component.enabled = value;
					}
				}
			}
		}

		public UXGrid(UXFactory uxFactory, UXCamera uxCamera, UXGridComponent component) : base(uxFactory, uxCamera, component)
		{
			this.gridComponent = component;
		}

		public override void InternalDestroyComponent()
		{
			this.gridComponent.Grid = null;
			this.gridComponent = null;
			base.InternalDestroyComponent();
		}

		public float GetCurrentScrollPosition(bool softClip)
		{
			return this.gridComponent.GetCurrentScrollPosition(softClip);
		}

		public void HideScrollArrows()
		{
			this.gridComponent.HideScrollArrows();
		}

		public void UpdateScrollArrows()
		{
			this.gridComponent.UpdateScrollArrows();
		}

		public bool IsGridComponentScrollable()
		{
			return this.gridComponent.IsScrollable();
		}

		public void SetAnimateSmoothly(bool value)
		{
			this.gridComponent.NGUIGrid.animateSmoothly = value;
		}

		public Transform GetParent()
		{
			return this.gridComponent.transform.parent;
		}

		public void ScrollToItem(int i)
		{
			int num = this.addedItems.Count;
			int maxItemsPerLine = this.MaxItemsPerLine;
			if (maxItemsPerLine > 0)
			{
				i %= maxItemsPerLine;
				if (num > maxItemsPerLine)
				{
					num = maxItemsPerLine;
				}
			}
			float location;
			if (i < 0 || num <= 1)
			{
				location = 0f;
			}
			else if (i >= num)
			{
				location = 1f;
			}
			else
			{
				location = (float)i / (float)(num - 1);
			}
			base.Scroll(location);
		}

		public void SmoothScrollToItem(int i)
		{
			if (!this.EnsureCenterOnChild())
			{
				return;
			}
			if (0 <= i && i < this.addedItems.Count)
			{
				this.gridComponent.NGUICenterOnChild.CenterOn(this.addedItems[i].GetUIWidget.transform);
				return;
			}
			Service.Get<StaRTSLogger>().Warn("SmoothScrollToItem invalid Index:" + i);
		}

		public void CullScrollObjects(bool enable, float cullFactor)
		{
			this.gridComponent.CullScrollObjects(enable, cullFactor);
		}

		public void CenterElementsInPanel()
		{
			this.gridComponent.CenterElementsInPanel();
		}

		public UXElement ScrollToNextElement()
		{
			if (!this.EnsureCenterOnChild())
			{
				return null;
			}
			GameObject centeredObject = this.gridComponent.NGUICenterOnChild.centeredObject;
			int num = 0;
			for (int i = 0; i < this.addedItems.Count; i++)
			{
				if (this.addedItems[i].Root == centeredObject)
				{
					num = i;
				}
			}
			num++;
			if (num >= this.addedItems.Count)
			{
				num = 0;
			}
			this.SmoothScrollToItem(num);
			return this.addedItems[num];
		}

		public void SetCenteredCallback(UXGrid.OnCentered onCentered)
		{
			if (!this.EnsureCenterOnChild())
			{
				return;
			}
			this.onCentered = onCentered;
			this.gridComponent.NGUICenterOnChild.onCenter = new UICenterOnChild.OnCenterCallback(this.OnCenteredCallback);
		}

		private void OnCenteredCallback(GameObject centeredObject)
		{
			for (int i = 0; i < this.addedItems.Count; i++)
			{
				if (this.addedItems[i].Root == centeredObject)
				{
					this.onCentered(this.addedItems[i], i);
					return;
				}
			}
		}

		public void SetCenteredFinishedCallback(UXGrid.OnCentered onCenteredFinished)
		{
			if (!this.EnsureCenterOnChild())
			{
				return;
			}
			this.onCenteredFinished = onCenteredFinished;
			this.gridComponent.NGUICenterOnChild.onFinished = new SpringPanel.OnFinished(this.OnCenteredFinishedCallback);
		}

		private void OnCenteredFinishedCallback()
		{
			GameObject centeredObject = this.gridComponent.NGUICenterOnChild.centeredObject;
			for (int i = 0; i < this.addedItems.Count; i++)
			{
				if (this.addedItems[i].Root == centeredObject)
				{
					this.onCenteredFinished(this.addedItems[i], i);
					return;
				}
			}
		}

		public UXElement GetCenteredElement()
		{
			if (!this.EnsureCenterOnChild())
			{
				return null;
			}
			GameObject centeredObject = this.gridComponent.NGUICenterOnChild.centeredObject;
			int i = 0;
			int count = this.addedItems.Count;
			while (i < count)
			{
				if (this.addedItems[i].Root == centeredObject)
				{
					return this.addedItems[i];
				}
				i++;
			}
			if (this.addedItems.Count > 0)
			{
				return this.addedItems[0];
			}
			return null;
		}

		public int GetCenteredElementIndex()
		{
			if (!this.EnsureCenterOnChild())
			{
				return 0;
			}
			GameObject centeredObject = this.gridComponent.NGUICenterOnChild.centeredObject;
			for (int i = 0; i < this.addedItems.Count; i++)
			{
				if (this.addedItems[i].Root == centeredObject)
				{
					return i;
				}
			}
			return 0;
		}

		public override void OnDestroyElement()
		{
			if (this.gridComponent != null)
			{
				if (this.gridComponent.NGUICenterOnChild != null)
				{
					this.gridComponent.NGUICenterOnChild.onFinished = null;
				}
				if (this.gridComponent.NGUIGrid != null)
				{
					this.gridComponent.NGUIGrid.onCustomSort = null;
				}
			}
			this.onCentered = null;
			this.comparisonCallback = null;
			base.OnDestroyElement();
		}

		public bool EnsureCenterOnChild()
		{
			if (this.gridComponent == null)
			{
				Service.Get<StaRTSLogger>().Warn("Missing gridComponent: " + this.gridComponent.gameObject.name);
				return false;
			}
			if (this.gridComponent.NGUICenterOnChild == null)
			{
				Service.Get<StaRTSLogger>().Warn("Missing GUICenterOnChild: " + this.gridComponent.gameObject.name);
				return false;
			}
			return true;
		}

		private bool IsArrangementCellSnap()
		{
			return this.gridComponent.NGUIGrid.arrangement == UIGrid.Arrangement.CellSnap;
		}

		private bool IsArrangementHorizontal()
		{
			return this.gridComponent.NGUIGrid.arrangement == UIGrid.Arrangement.Horizontal;
		}

		private bool IsArrangementVertical()
		{
			return this.gridComponent.NGUIGrid.arrangement == UIGrid.Arrangement.Vertical;
		}

		public void SetSortComparisonCallback(Comparison<UXElement> callback)
		{
			this.comparisonCallback = callback;
			this.gridComponent.NGUIGrid.onCustomSort = new Comparison<Transform>(this.NGUIOnCustomSortCallbacK);
		}

		public int NGUIOnCustomSortCallbacK(Transform transformA, Transform transformB)
		{
			UXElement uXElement = this.FindUXElementForTransform(transformA);
			UXElement uXElement2 = this.FindUXElementForTransform(transformB);
			if (uXElement == null || uXElement2 == null)
			{
				Service.Get<StaRTSLogger>().Warn("Missing NGUIOnCustomSortCallbacK UXElement Reference");
				return 0;
			}
			return this.comparisonCallback.Invoke(uXElement, uXElement2);
		}

		private UXElement FindUXElementForTransform(Transform transform)
		{
			int i = 0;
			int count = this.addedItems.Count;
			while (i < count)
			{
				if (this.addedItems[i].Root.transform == transform)
				{
					return this.addedItems[i];
				}
				i++;
			}
			return null;
		}

		public void SetSortModeAlphebetical()
		{
			this.gridComponent.NGUIGrid.sorting = UIGrid.Sorting.Alphabetic;
		}

		public void SetSortModeCustom()
		{
			this.gridComponent.NGUIGrid.sorting = UIGrid.Sorting.Custom;
		}

		public void SetSortModeHorizontal()
		{
			this.gridComponent.NGUIGrid.sorting = UIGrid.Sorting.Horizontal;
		}

		public void SetSortModeNone()
		{
			this.gridComponent.NGUIGrid.sorting = UIGrid.Sorting.None;
		}

		public void SetSortModeVertical()
		{
			this.gridComponent.NGUIGrid.sorting = UIGrid.Sorting.Vertical;
		}

		public int GetSortedIndex(UXElement element)
		{
			return this.gridComponent.NGUIGrid.GetIndex(element.Root.transform);
		}

		public void RepositionElement(UXElement element)
		{
			Vector3 localPosition = element.LocalPosition;
			float z = localPosition.z;
			int num = this.addedItems.IndexOf(element);
			if (num < 0)
			{
				Service.Get<StaRTSLogger>().Warn("Add element to grid before positioning: " + element.Root.name);
				return;
			}
			if (this.IsArrangementCellSnap())
			{
				if (this.CellWidth > 0f)
				{
					localPosition.x = Mathf.Round(localPosition.x / this.CellWidth) * this.CellWidth;
				}
				if (this.CellHeight > 0f)
				{
					localPosition.y = Mathf.Round(localPosition.y / this.CellHeight) * this.CellHeight;
				}
			}
			else if (this.IsArrangementHorizontal())
			{
				localPosition = new Vector3(this.CellWidth * (float)num, 0f, z);
			}
			else
			{
				localPosition = new Vector3(0f, -this.CellHeight * (float)num, z);
			}
			element.LocalPosition = localPosition;
		}

		public void ResetScrollViewPosition()
		{
			this.component.ResetScrollViewPosition();
		}

		protected internal UXGrid(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).CenterElementsInPanel();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).CullScrollObjects(*(sbyte*)args != 0, *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).EnsureCenterOnChild());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).FindUXElementForTransform((Transform)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).CellHeight);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).CellWidth);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).IsScrollable);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).MaxItemsPerLine);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).RepositionCallback);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).GetCenteredElement());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).GetCenteredElementIndex());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).GetCurrentScrollPosition(*(sbyte*)args != 0));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).GetParent());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).GetSortedIndex((UXElement)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).HideScrollArrows();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).IsArrangementCellSnap());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).IsArrangementHorizontal());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).IsArrangementVertical());
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).IsGridComponentScrollable());
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).NGUIOnCustomSortCallbacK((Transform)GCHandledObjects.GCHandleToObject(*args), (Transform)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).OnCenteredCallback((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).OnCenteredFinishedCallback();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).RepositionElement((UXElement)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).ResetScrollViewPosition();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).ScrollToItem(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGrid)GCHandledObjects.GCHandleToObject(instance)).ScrollToNextElement());
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).CellHeight = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).CellWidth = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).IsScrollable = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).MaxItemsPerLine = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).RepositionCallback = (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SetAnimateSmoothly(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SetCenteredCallback((UXGrid.OnCentered)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SetCenteredFinishedCallback((UXGrid.OnCentered)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SetSortComparisonCallback((Comparison<UXElement>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SetSortModeAlphebetical();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SetSortModeCustom();
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SetSortModeHorizontal();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SetSortModeNone();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SetSortModeVertical();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).SmoothScrollToItem(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((UXGrid)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollArrows();
			return -1L;
		}
	}
}
