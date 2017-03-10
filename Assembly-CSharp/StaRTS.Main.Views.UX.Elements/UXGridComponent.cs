using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXGridComponent : AbstractUXListComponent, IUnitySerializable
	{
		private bool manualCullGUIObjects;

		private float cullingFactor;

		public UIGrid NGUIGrid
		{
			get;
			set;
		}

		public UXGrid Grid
		{
			get;
			set;
		}

		public float CellWidth
		{
			get
			{
				if (!(this.NGUIGrid == null))
				{
					return this.NGUIGrid.cellWidth;
				}
				return 0f;
			}
			set
			{
				if (this.NGUIGrid != null)
				{
					this.NGUIGrid.cellWidth = value;
				}
			}
		}

		public float CellHeight
		{
			get
			{
				if (!(this.NGUIGrid == null))
				{
					return this.NGUIGrid.cellHeight;
				}
				return 0f;
			}
			set
			{
				if (this.NGUIGrid != null)
				{
					this.NGUIGrid.cellHeight = value;
				}
			}
		}

		public int MaxItemsPerLine
		{
			get
			{
				if (!(this.NGUIGrid == null))
				{
					return this.NGUIGrid.maxPerLine;
				}
				return 0;
			}
			set
			{
				if (this.NGUIGrid != null)
				{
					this.NGUIGrid.maxPerLine = value;
				}
			}
		}

		public override void Init()
		{
			base.Init();
			if (this.NGUIGrid != null)
			{
				this.NGUIGrid.sorting = UIGrid.Sorting.Alphabetic;
			}
		}

		public override float GetCurrentScrollPosition(bool softClip)
		{
			if (base.NGUIScrollView == null || this.NGUIGrid == null || base.NGUIScrollView.panel == null)
			{
				return 0f;
			}
			bool flag = this.NGUIGrid.arrangement == UIGrid.Arrangement.Vertical;
			Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(base.transform);
			Vector3[] worldCorners = base.NGUIScrollView.panel.worldCorners;
			float num6;
			if (flag)
			{
				Vector4 finalClipRegion = base.NGUIScrollView.panel.finalClipRegion;
				float num = 0f;
				if (softClip && base.NGUIScrollView.panel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					float num2 = base.NGUIScrollView.panel.clipSoftness.y / finalClipRegion.w;
					num = num2 * (worldCorners[2].y - worldCorners[0].y);
				}
				float num3 = num + bounds.extents.y;
				float num4 = worldCorners[0].y + num3;
				float num5 = worldCorners[2].y - num3;
				float y = bounds.center.y;
				num6 = (y - num4) / (num5 - num4);
				num6 -= 1f;
				num6 *= -1f;
			}
			else
			{
				float num7 = worldCorners[0].x + bounds.extents.x;
				float num8 = worldCorners[2].x - bounds.extents.x;
				float x = bounds.center.x;
				num6 = (x - num7) / (num8 - num7);
			}
			return Mathf.Clamp01(num6);
		}

		public override void RepositionItems(bool delayedReposition)
		{
			bool flag = true;
			if (this.NGUIGrid != null)
			{
				this.NGUIGrid.Reposition();
				if (base.gameObject.activeInHierarchy & delayedReposition)
				{
					flag = false;
					base.StartCoroutine(this.DelayedReposition());
				}
				if (base.NGUIPanel != null && base.NGUIScrollView != null)
				{
					Vector3 position = base.NGUIPanel.gameObject.transform.position;
					Vector4 baseClipRegion = base.NGUIPanel.baseClipRegion;
					base.NGUIScrollView.ResetPosition();
					base.NGUIScrollView.RestrictWithinBounds(true);
					Vector3 position2 = base.NGUIPanel.gameObject.transform.position;
					Vector4 baseClipRegion2 = base.NGUIPanel.baseClipRegion;
					if (this.NGUIGrid.arrangement == UIGrid.Arrangement.Vertical)
					{
						position2.x = position.x;
						baseClipRegion2.x = baseClipRegion.x;
					}
					else
					{
						position2.y = position.y;
						baseClipRegion2.y = baseClipRegion.y;
					}
					base.NGUIPanel.gameObject.transform.position = position2;
					base.NGUIPanel.baseClipRegion = baseClipRegion2;
				}
			}
			this.UpdateScrollArrows();
			if (flag)
			{
				base.OnReposition();
			}
		}

		public override void UpdateScrollArrows()
		{
			if (this.Grid != null && this.Grid.Visible)
			{
				base.UpdateScrollArrows();
			}
		}

		[IteratorStateMachine(typeof(UXGridComponent.<DelayedReposition>d__23))]
		private IEnumerator DelayedReposition()
		{
			yield return null;
			this.NGUIGrid.Reposition();
			this.OnReposition();
			yield break;
		}

		public override void Scroll(float location)
		{
			if (this.NGUIGrid != null && base.NGUIScrollView != null)
			{
				float x = 0f;
				float y = 0f;
				if (this.NGUIGrid.arrangement == UIGrid.Arrangement.Vertical)
				{
					y = location;
				}
				else
				{
					x = location;
				}
				base.NGUIScrollView.SetDragAmount(x, y, false);
				base.NGUIScrollView.UpdateScrollbars(true);
				this.UpdateScrollArrows();
			}
		}

		public override Vector3 GetItemDimension()
		{
			if (this.NGUIGrid == null)
			{
				return Vector3.zero;
			}
			if (this.NGUIGrid.arrangement != UIGrid.Arrangement.Vertical)
			{
				return Vector3.right * this.NGUIGrid.cellWidth;
			}
			return Vector3.up * this.NGUIGrid.cellHeight;
		}

		protected override void OnDrag()
		{
			if (this.Grid != null)
			{
				this.Grid.InternalOnDrag();
			}
			this.UpdateScrollArrows();
		}

		public void CullScrollObjects(bool enable, float cullFactor)
		{
			this.manualCullGUIObjects = enable;
			if (enable)
			{
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
				base.GetComponentInParent<UIScrollView>().disableDragIfFits = false;
				if (cullFactor < 2f)
				{
					cullFactor = 2f;
				}
				this.cullingFactor = cullFactor;
				return;
			}
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public override void OnViewFrameTime(float dt)
		{
			base.OnViewFrameTime(dt);
			if (!this.manualCullGUIObjects)
			{
				return;
			}
			if (this.Grid == null)
			{
				return;
			}
			foreach (UXElement current in this.Grid.GetElementList())
			{
				if (current != null)
				{
					Bounds bounds = UXUtils.CalculateAbsoluteWidgetBound(current.GetUIWidget.transform);
					Vector3[] worldCorners = base.NGUIPanel.worldCorners;
					Vector3 vector = bounds.center - bounds.size * this.cullingFactor;
					Vector3 vector2 = bounds.center + bounds.size * this.cullingFactor;
					if (this.NGUIGrid.arrangement == UIGrid.Arrangement.Horizontal)
					{
						if (vector.x > worldCorners[2].x || vector2.x < worldCorners[0].x)
						{
							current.GetUIWidget.gameObject.SetActive(false);
						}
						else
						{
							current.GetUIWidget.gameObject.SetActive(true);
						}
					}
					else if (vector.y > worldCorners[2].y || vector2.y < worldCorners[0].y)
					{
						current.GetUIWidget.gameObject.SetActive(false);
					}
					else
					{
						current.GetUIWidget.gameObject.SetActive(true);
					}
				}
			}
		}

		public void CenterElementsInPanel()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.DelayedCenterElementsInPanel());
				return;
			}
			this.DoCenterElementsInPanel();
		}

		[IteratorStateMachine(typeof(UXGridComponent.<DelayedCenterElementsInPanel>d__30))]
		private IEnumerator DelayedCenterElementsInPanel()
		{
			yield return null;
			this.DoCenterElementsInPanel();
			yield break;
		}

		private void DoCenterElementsInPanel()
		{
			UIScrollView component = this.NGUIGrid.transform.parent.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.contentPivot = UIWidget.Pivot.Center;
				component.ResetPosition();
				return;
			}
			Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.NGUIGrid.transform);
			UIPanel componentInParent = this.NGUIGrid.GetComponentInParent<UIPanel>();
			Vector3[] worldCorners = componentInParent.worldCorners;
			Vector3 zero = Vector3.zero;
			zero.x = bounds.center.x - (worldCorners[0].x + worldCorners[2].x) / 2f;
			zero.y = bounds.center.y - (worldCorners[0].y + worldCorners[2].y) / 2f;
			this.NGUIGrid.transform.position -= zero;
		}

		public UXGridComponent()
		{
			this.cullingFactor = 2f;
			base..ctor();
		}

		public override void Unity_Serialize(int depth)
		{
		}

		public override void Unity_Deserialize(int depth)
		{
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public override void Unity_NamedSerialize(int depth)
		{
		}

		public override void Unity_NamedDeserialize(int depth)
		{
		}

		protected internal UXGridComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).CenterElementsInPanel();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).CullScrollObjects(*(sbyte*)args != 0, *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).DelayedCenterElementsInPanel());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).DelayedReposition());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).DoCenterElementsInPanel();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).CellHeight);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).CellWidth);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).Grid);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).MaxItemsPerLine);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIGrid);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).GetCurrentScrollPosition(*(sbyte*)args != 0));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).GetItemDimension());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).OnDrag();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).RepositionItems(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).Scroll(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).CellHeight = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).CellWidth = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).Grid = (UXGrid)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).MaxItemsPerLine = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIGrid = (UIGrid)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((UXGridComponent)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollArrows();
			return -1L;
		}
	}
}
