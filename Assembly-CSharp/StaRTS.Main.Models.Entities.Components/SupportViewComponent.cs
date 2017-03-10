using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Views;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class SupportViewComponent : ComponentBase
	{
		private BuildingTooltip buildingTooltip;

		private TooltipHelper tooltipHelper;

		public SupportViewComponentState State
		{
			get;
			private set;
		}

		public BuildingTooltip BuildingTooltip
		{
			get
			{
				return this.buildingTooltip;
			}
		}

		public bool Enabled
		{
			get
			{
				return this.tooltipHelper.Enabled;
			}
		}

		public SupportViewComponent()
		{
			this.buildingTooltip = null;
			this.State = SupportViewComponentState.Dormant;
			this.tooltipHelper = new TooltipHelper();
		}

		public void SetupElements(BuildingTooltip buildingTooltip, SupportViewComponentState s)
		{
			if (this.State != SupportViewComponentState.Dormant)
			{
				return;
			}
			this.buildingTooltip = buildingTooltip;
			this.State = s;
			this.tooltipHelper.SetupElements(this.Entity.Get<GameObjectViewComponent>(), buildingTooltip.TooltipElement, 0f, true, false);
		}

		public void SetEnabled(bool enable)
		{
			this.tooltipHelper.Enabled = enable;
		}

		public void UpdateLocation()
		{
			if (this.State == SupportViewComponentState.Dormant)
			{
				return;
			}
			this.tooltipHelper.UpdateLocation(false);
		}

		public void UpdateSelected(bool selected)
		{
			this.Refresh();
			if (this.buildingTooltip != null)
			{
				this.buildingTooltip.SetSelected(selected);
			}
		}

		public void Refresh()
		{
			this.TeardownElements();
			Service.Get<BuildingTooltipController>().EnsureBuildingTooltip((SmartEntity)this.Entity);
		}

		public void UpdateTime(int timeLeft, int timeTotal, bool updateProgress)
		{
			if (this.buildingTooltip != null)
			{
				this.buildingTooltip.SetTime(timeLeft);
				if (updateProgress)
				{
					this.buildingTooltip.SetProgress(timeLeft, timeTotal);
				}
			}
		}

		public void TeardownElements()
		{
			if (this.buildingTooltip != null)
			{
				this.buildingTooltip.DestroyTooltip();
				this.buildingTooltip = null;
			}
			this.State = SupportViewComponentState.Dormant;
			this.tooltipHelper.TeardownElements(false);
		}

		public override void OnRemove()
		{
			this.TeardownElements();
		}

		protected internal SupportViewComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).BuildingTooltip);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).State);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).OnRemove();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).Refresh();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).State = (SupportViewComponentState)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).SetEnabled(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).SetupElements((BuildingTooltip)GCHandledObjects.GCHandleToObject(*args), (SupportViewComponentState)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).TeardownElements();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).UpdateLocation();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).UpdateSelected(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SupportViewComponent)GCHandledObjects.GCHandleToObject(instance)).UpdateTime(*(int*)args, *(int*)(args + 1), *(sbyte*)(args + 2) != 0);
			return -1L;
		}
	}
}
