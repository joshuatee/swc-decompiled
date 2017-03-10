using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXTableComponent : AbstractUXListComponent, IUnitySerializable
	{
		public UITable NGUITable
		{
			get;
			set;
		}

		public UXTable Table
		{
			get;
			set;
		}

		public Vector2 Padding
		{
			get
			{
				if (!(this.NGUITable == null))
				{
					return this.NGUITable.padding;
				}
				return Vector2.zero;
			}
		}

		public override void Init()
		{
			base.Init();
			if (this.NGUITable != null)
			{
				this.NGUITable.sorting = UITable.Sorting.Alphabetic;
			}
		}

		public override void RepositionItems(bool delayedReposition)
		{
			bool flag = true;
			if (this.NGUITable != null)
			{
				this.NGUITable.Reposition();
				if (base.gameObject.activeInHierarchy & delayedReposition)
				{
					flag = false;
					base.StartCoroutine(this.DelayedReposition());
				}
				if (base.NGUIPanel != null && base.NGUIScrollView != null)
				{
					base.NGUIScrollView.ResetPosition();
					base.NGUIScrollView.RestrictWithinBounds(true);
					Vector3 position = base.NGUIPanel.gameObject.transform.position;
					Vector4 baseClipRegion = base.NGUIPanel.baseClipRegion;
					base.NGUIPanel.gameObject.transform.position = position;
					base.NGUIPanel.baseClipRegion = baseClipRegion;
				}
			}
			this.UpdateScrollArrows();
			if (flag)
			{
				base.OnReposition();
			}
		}

		[IteratorStateMachine(typeof(UXTableComponent.<DelayedReposition>d__12))]
		private IEnumerator DelayedReposition()
		{
			yield return null;
			this.NGUITable.Reposition();
			this.OnReposition();
			yield break;
		}

		public override float GetCurrentScrollPosition(bool softClip)
		{
			if (base.NGUIScrollView == null || this.NGUITable == null)
			{
				return 0f;
			}
			Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(base.transform);
			Vector3[] worldCorners = base.NGUIScrollView.panel.worldCorners;
			float num = worldCorners[0].y + bounds.extents.y;
			float num2 = worldCorners[2].y - bounds.extents.y;
			float y = bounds.center.y;
			float num3 = (y - num) / (num2 - num);
			num3 -= 1f;
			num3 *= -1f;
			return Mathf.Clamp01(num3);
		}

		public override void Scroll(float location)
		{
			if (this.NGUITable != null && base.NGUIScrollView != null)
			{
				float x = 0f;
				base.NGUIScrollView.SetDragAmount(x, location, false);
			}
		}

		protected override void OnDrag()
		{
			if (this.Table != null)
			{
				this.Table.InternalOnDrag();
			}
			this.UpdateScrollArrows();
		}

		public UXTableComponent()
		{
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

		protected internal UXTableComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).DelayedReposition());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).NGUITable);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Padding);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Table);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).GetCurrentScrollPosition(*(sbyte*)args != 0));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).OnDrag();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).RepositionItems(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Scroll(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).NGUITable = (UITable)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Table = (UXTable)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((UXTableComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
