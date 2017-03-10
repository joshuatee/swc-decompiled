using StaRTS.Main.Views.Cameras;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXTable : AbstractUXList
	{
		private UXTableComponent tableComponent;

		private bool autoCenterPositionSet;

		private Vector3 autoCenterPosition;

		public Action RepositionCallback
		{
			get
			{
				return this.tableComponent.RepositionCallback;
			}
			set
			{
				this.tableComponent.RepositionCallback = value;
			}
		}

		public Vector2 Padding
		{
			get
			{
				return this.tableComponent.Padding;
			}
		}

		public bool HideInactive
		{
			get
			{
				return this.tableComponent != null && this.tableComponent.NGUITable != null && this.tableComponent.NGUITable.hideInactive;
			}
			set
			{
				if (this.tableComponent != null && this.tableComponent.NGUITable != null)
				{
					this.tableComponent.NGUITable.hideInactive = value;
				}
			}
		}

		public UXTable(UXFactory uxFactory, UXCamera uxCamera, UXTableComponent component) : base(uxFactory, uxCamera, component)
		{
			this.tableComponent = component;
		}

		public override void InternalDestroyComponent()
		{
			this.tableComponent.Table = null;
			this.tableComponent = null;
			base.InternalDestroyComponent();
		}

		public void HideScrollArrows()
		{
			this.tableComponent.HideScrollArrows();
		}

		public override void ClearWithoutDestroy()
		{
			base.ClearWithoutDestroy();
			if (this.autoCenterPositionSet)
			{
				base.LocalPosition = this.autoCenterPosition;
			}
		}

		public override void Clear()
		{
			base.Clear();
			if (this.autoCenterPositionSet)
			{
				base.LocalPosition = this.autoCenterPosition;
			}
		}

		public void AutoCenter()
		{
			if (base.Count == 0)
			{
				return;
			}
			if (!this.autoCenterPositionSet)
			{
				this.autoCenterPositionSet = true;
				this.autoCenterPosition = base.LocalPosition;
			}
			base.Width = 0f;
			Vector3 localPosition = this.autoCenterPosition;
			localPosition.x -= 0.5f * (UXUtils.CalculateElementSize(this).x + this.Padding.x);
			base.LocalPosition = localPosition;
		}

		protected internal UXTable(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UXTable)GCHandledObjects.GCHandleToObject(instance)).AutoCenter();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UXTable)GCHandledObjects.GCHandleToObject(instance)).Clear();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UXTable)GCHandledObjects.GCHandleToObject(instance)).ClearWithoutDestroy();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTable)GCHandledObjects.GCHandleToObject(instance)).HideInactive);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTable)GCHandledObjects.GCHandleToObject(instance)).Padding);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTable)GCHandledObjects.GCHandleToObject(instance)).RepositionCallback);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXTable)GCHandledObjects.GCHandleToObject(instance)).HideScrollArrows();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXTable)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXTable)GCHandledObjects.GCHandleToObject(instance)).HideInactive = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXTable)GCHandledObjects.GCHandleToObject(instance)).RepositionCallback = (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
