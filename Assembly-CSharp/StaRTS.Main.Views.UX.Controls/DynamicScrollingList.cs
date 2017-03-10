using StaRTS.Externals.Maker;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Controls
{
	public class DynamicScrollingList
	{
		private int maxItemsToLoad;

		private Dictionary<int, ListItem> listItems;

		private int firstLoadedItem;

		private int lastLoadedItem;

		private UXGrid myGrid;

		private int direction;

		private bool recycleEnabled;

		public int Count
		{
			get
			{
				return this.listItems.Count;
			}
		}

		public GameObject Root
		{
			get
			{
				return this.myGrid.Root;
			}
		}

		public UXGrid Grid
		{
			get
			{
				return this.myGrid;
			}
		}

		public DynamicScrollingList(UXGrid grid, int maxItemsToLoad)
		{
			this.listItems = new Dictionary<int, ListItem>();
			this.myGrid = grid;
			this.firstLoadedItem = -1;
			this.lastLoadedItem = -1;
			this.maxItemsToLoad = maxItemsToLoad;
			this.recycleEnabled = true;
		}

		public void AddItem(object cookie, int location)
		{
			ListItem value = new ListItem(cookie, null);
			this.listItems[location] = value;
			this.firstLoadedItem = Mathf.Max(this.firstLoadedItem, 0);
			this.lastLoadedItem = Mathf.Min(this.listItems.Count - 1, this.maxItemsToLoad - 1);
			if (this.IsItemLocationLoaded(location))
			{
				this.RequestUIForItem(cookie, location, location);
			}
		}

		public void SetItemUI(UXElement item, int location)
		{
			this.myGrid.AddItem(item, location);
			this.myGrid.RepositionItems();
			item.Visible = true;
			ListItem value = this.listItems[location];
			value.UIItem = item;
			this.listItems[location] = value;
		}

		public UXElement GetItemUI(int location)
		{
			return this.listItems[location].UIItem;
		}

		public object GetItemCookie(int location)
		{
			return this.listItems[location].Cookie;
		}

		public void RemoveItems()
		{
			this.myGrid.Clear();
			this.listItems.Clear();
			this.firstLoadedItem = -1;
			this.lastLoadedItem = -1;
		}

		private bool IsItemLocationLoaded(int location)
		{
			return location >= this.firstLoadedItem && location <= this.lastLoadedItem;
		}

		private bool IsLastItemVisible()
		{
			return this.firstLoadedItem != -1 && this.lastLoadedItem != -1 && this.listItems[this.firstLoadedItem].UIItem != null && this.listItems[this.lastLoadedItem].UIItem != null && !this.listItems[this.firstLoadedItem].UIItem.Root.GetComponent<UIWidget>().isVisible && this.listItems[this.lastLoadedItem].UIItem.Root.GetComponent<UIWidget>().isVisible;
		}

		private bool IsFirstItemVisible()
		{
			return this.firstLoadedItem != -1 && this.lastLoadedItem != -1 && this.listItems[this.firstLoadedItem].UIItem != null && this.listItems[this.lastLoadedItem].UIItem != null && !this.listItems[this.lastLoadedItem].UIItem.Root.GetComponent<UIWidget>().isVisible && this.listItems[this.firstLoadedItem].UIItem.Root.GetComponent<UIWidget>().isVisible;
		}

		public void UpdateItems()
		{
			if (!this.recycleEnabled)
			{
				return;
			}
			if (this.IsLastItemVisible() && this.listItems[this.lastLoadedItem].UIItem != null)
			{
				this.RecycleFirst();
				return;
			}
			if (this.IsFirstItemVisible() && this.listItems[this.firstLoadedItem].UIItem != null)
			{
				this.RecycleLast();
			}
		}

		private void Recycle(int direction)
		{
			this.direction = direction;
			UXGridComponent component = this.Root.GetComponent<UXGridComponent>();
			if (component.NGUIScrollView.panel.isAnchoredVertically)
			{
				Transform anchor = null;
				component.NGUIScrollView.panel.SetAnchor(anchor);
			}
			int num;
			if (direction > 0)
			{
				num = this.firstLoadedItem;
			}
			else
			{
				num = this.lastLoadedItem;
			}
			UIScrollView uIScrollView = NGUITools.FindInParents<UIScrollView>(this.myGrid.Root);
			float currentScrollPosition = this.myGrid.GetCurrentScrollPosition(true);
			float y = uIScrollView.bounds.size.y;
			this.CleanupUIForItem(this.listItems[num].Cookie);
			this.listItems[num].UIItem.Visible = false;
			this.myGrid.RemoveItem(this.listItems[num].UIItem);
			ListItem listItem = this.listItems[num];
			UXElement uIItem = listItem.UIItem;
			listItem.UIItem = null;
			this.listItems[num] = listItem;
			this.firstLoadedItem += direction;
			this.lastLoadedItem += direction;
			int num2;
			if (direction > 0)
			{
				num2 = this.lastLoadedItem;
			}
			else
			{
				num2 = this.firstLoadedItem;
			}
			listItem = this.listItems[num2];
			listItem.UIItem = uIItem;
			this.listItems[num2] = listItem;
			this.RequestUIForItem(this.listItems[num2].Cookie, num2, num);
			float num3 = y - component.CellHeight;
			float num4 = num3 - component.NGUIScrollView.panel.finalClipRegion.w;
			float num5 = y - component.NGUIScrollView.panel.finalClipRegion.w;
			float num6;
			if (direction > 0)
			{
				num6 = (component.CellHeight - (float)direction) / num4;
			}
			else
			{
				num6 = (float)(-(float)direction) / num4;
			}
			this.myGrid.Scroll(currentScrollPosition / (num4 / num5) - num6);
			component.NGUIGrid.onReposition = new UIGrid.OnReposition(this.RepositionDone);
			this.recycleEnabled = false;
		}

		private void RepositionDone()
		{
			UXGridComponent component = this.Root.GetComponent<UXGridComponent>();
			component.NGUIGrid.onReposition = null;
			this.recycleEnabled = true;
			if (this.direction < 0)
			{
				component.NGUIScrollView.InvalidateBounds();
				Bounds bounds = component.NGUIScrollView.bounds;
				float num = component.NGUIScrollView.panel.finalClipRegion.w * 0.5f;
				float num2 = bounds.min.y + num;
				float num3 = bounds.max.y - num;
				if (component.NGUIScrollView.panel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num2 -= component.NGUIScrollView.panel.clipSoftness.y;
					num3 += component.NGUIScrollView.panel.clipSoftness.y;
				}
				float num4 = Mathf.Abs(num2 - num3);
				float num5 = component.CellHeight / num4;
				this.myGrid.Scroll(this.myGrid.GetCurrentScrollPosition(true) + num5);
			}
		}

		private void RequestUIForItem(object cookie, int location, int oldLocation)
		{
			ListItemCreateData listItemCreateData = new ListItemCreateData(this, cookie, location, oldLocation);
			Service.Get<EventManager>().SendEvent(EventId.UIDynamicScrollingListCreateItem, listItemCreateData);
		}

		private void CleanupUIForItem(object cookie)
		{
			Service.Get<EventManager>().SendEvent(EventId.UIDynamicScrollingListCleanupItem, cookie);
		}

		private void RecycleFirst()
		{
			if (this.lastLoadedItem + 1 < this.listItems.Count)
			{
				this.Recycle(1);
			}
		}

		private void RecycleLast()
		{
			if (this.firstLoadedItem > 0)
			{
				this.Recycle(-1);
			}
		}

		protected internal DynamicScrollingList(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).AddItem(GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).CleanupUIForItem(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).Count);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).Grid);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).Root);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).GetItemCookie(*(int*)args));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).GetItemUI(*(int*)args));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).IsFirstItemVisible());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).IsItemLocationLoaded(*(int*)args));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).IsLastItemVisible());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).Recycle(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).RecycleFirst();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).RecycleLast();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).RemoveItems();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).RepositionDone();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).RequestUIForItem(GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).SetItemUI((UXElement)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((DynamicScrollingList)GCHandledObjects.GCHandleToObject(instance)).UpdateItems();
			return -1L;
		}
	}
}
