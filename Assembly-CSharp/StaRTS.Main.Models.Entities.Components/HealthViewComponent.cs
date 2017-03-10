using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Views;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class HealthViewComponent : ComponentBase
	{
		private const float SECONDARY_HEIGHT_OFFSET = 1f;

		private UXSlider slider;

		private UXSlider secondarySlider;

		private TooltipHelper tooltipHelper;

		private TooltipHelper secondaryTooltipHelper;

		private bool secondaryOnly;

		public int HealthAmount
		{
			get;
			private set;
		}

		public int MaxHealthAmount
		{
			get;
			private set;
		}

		public int SecondaryHealthAmount
		{
			get;
			private set;
		}

		public int SecondaryMaxHealthAmount
		{
			get;
			private set;
		}

		public bool IsInitialized
		{
			get;
			private set;
		}

		public bool AutoRegenerating
		{
			get;
			set;
		}

		public bool HasRubble
		{
			get;
			set;
		}

		public HealthViewComponent()
		{
			this.tooltipHelper = new TooltipHelper();
			this.secondaryTooltipHelper = new TooltipHelper();
			this.slider = null;
			this.secondarySlider = null;
			this.IsInitialized = false;
		}

		public void SetupElements()
		{
			this.SetupElements(this.Entity.Get<GameObjectViewComponent>(), true, false);
		}

		public void SetupElements(GameObjectViewComponent view, bool primary, bool secondary)
		{
			if (this.IsInitialized)
			{
				return;
			}
			if (this.HealthAmount >= this.MaxHealthAmount && this.SecondaryHealthAmount >= this.SecondaryMaxHealthAmount)
			{
				this.TeardownElements();
				return;
			}
			this.IsInitialized = true;
			if (primary && TooltipHelper.WouldOverlapAnotherTooltip(view))
			{
				return;
			}
			this.secondaryOnly = (secondary && !primary);
			UXController uXController = Service.Get<UXController>();
			string text = "HealthSlider" + view.MainGameObject.GetInstanceID().ToString();
			if (primary)
			{
				this.slider = uXController.MiscElementsManager.CreateHealthSlider(text, uXController.WorldUIParent, false);
				if (this.slider != null)
				{
					this.slider.Value = 1f;
					this.tooltipHelper.SetupElements(view, this.slider, 0f, true, true);
				}
			}
			if (secondary)
			{
				this.secondarySlider = uXController.MiscElementsManager.CreateHealthSlider("s_" + text, uXController.WorldUIParent, true);
				if (this.secondarySlider != null)
				{
					this.secondarySlider.Value = 1f;
					this.secondaryTooltipHelper.SetupElements(view, this.secondarySlider, 1f, true, false);
				}
			}
		}

		public void SetEnabled(bool enable)
		{
			if (!this.IsInitialized)
			{
				return;
			}
			this.tooltipHelper.Enabled = enable;
			this.secondaryTooltipHelper.Enabled = enable;
		}

		public void UpdateLocation()
		{
			if (!this.IsInitialized)
			{
				return;
			}
			if (this.slider != null)
			{
				this.tooltipHelper.Slot = -1;
				if (this.tooltipHelper.UpdateLocation(true))
				{
					this.TeardownElements();
					return;
				}
			}
			if (this.secondarySlider != null)
			{
				this.secondaryTooltipHelper.UpdateLocation(false);
			}
		}

		public void UpdateHealth(int health, int maxHealth, bool secondary)
		{
			if (secondary || this.secondaryOnly)
			{
				this.SecondaryHealthAmount = health;
				this.SecondaryMaxHealthAmount = maxHealth;
				if (this.secondarySlider != null)
				{
					this.secondarySlider.Value = ((maxHealth <= 0) ? 0f : ((float)health / (float)maxHealth));
					return;
				}
			}
			else
			{
				this.HealthAmount = health;
				this.MaxHealthAmount = maxHealth;
				if (this.slider != null)
				{
					this.slider.Value = ((maxHealth <= 0) ? 0f : ((float)health / (float)maxHealth));
				}
			}
		}

		public void TeardownElements()
		{
			if (!this.IsInitialized)
			{
				return;
			}
			this.IsInitialized = false;
			MiscElementsManager miscElementsManager = Service.Get<UXController>().MiscElementsManager;
			if (this.slider != null)
			{
				miscElementsManager.DestroyHealthSlider(this.slider, false);
				this.slider = null;
			}
			this.tooltipHelper.TeardownElements(true);
			if (this.secondarySlider != null)
			{
				miscElementsManager.DestroyHealthSlider(this.secondarySlider, true);
				this.secondarySlider = null;
			}
			this.secondaryTooltipHelper.TeardownElements(false);
		}

		public override void OnRemove()
		{
			this.RemoveSelf();
		}

		public bool WillFadeOnTimer()
		{
			return this.tooltipHelper.HasFadeTimer() || this.secondaryTooltipHelper.HasFadeTimer();
		}

		public void GoAwayIn(float seconds)
		{
			this.tooltipHelper.GoAwayIn(seconds, new Action(this.RemoveSelf));
			this.secondaryTooltipHelper.GoAwayIn(seconds, new Action(this.RemoveSelf));
		}

		private void RemoveSelf()
		{
			this.TeardownElements();
		}

		protected internal HealthViewComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).AutoRegenerating);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).HasRubble);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).HealthAmount);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).IsInitialized);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).MaxHealthAmount);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).SecondaryHealthAmount);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).SecondaryMaxHealthAmount);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).GoAwayIn(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).OnRemove();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).RemoveSelf();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).AutoRegenerating = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).HasRubble = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).HealthAmount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).IsInitialized = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).MaxHealthAmount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).SecondaryHealthAmount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).SecondaryMaxHealthAmount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).SetEnabled(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).SetupElements();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).SetupElements((GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).TeardownElements();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).UpdateHealth(*(int*)args, *(int*)(args + 1), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).UpdateLocation();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthViewComponent)GCHandledObjects.GCHandleToObject(instance)).WillFadeOnTimer());
		}
	}
}
