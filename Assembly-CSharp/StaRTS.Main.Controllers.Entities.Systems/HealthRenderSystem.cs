using Net.RichardLord.Ash.Core;
using StaRTS.FX;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class HealthRenderSystem : ViewSystemBase
	{
		private EntityController entityController;

		private NodeList<HealthViewNode> nodeList;

		private const float HP_REGENERATION_INTERVAL_TIME = 0.1f;

		private const int HP_REGENERATION_PER_INTERVAL = 80;

		public const float HP_REGENERATION_PERCENT_THRESHOLD_RUBBLE = 0.2f;

		private const float REGENERATION_FINISHED_EVENT_DELAY = 1f;

		private float timeSinceRegenerationUpdate;

		public override void AddToGame(IGame game)
		{
			this.entityController = Service.Get<EntityController>();
			this.nodeList = this.entityController.GetNodeList<HealthViewNode>();
		}

		public override void RemoveFromGame(IGame game)
		{
		}

		public void ForceUpdate()
		{
			this.Update(0.1f);
		}

		protected override void Update(float dt)
		{
			if (Service.IsSet<BuildingTooltipController>())
			{
				Service.Get<BuildingTooltipController>().ResetTooltipSlots();
			}
			for (HealthViewNode healthViewNode = this.nodeList.Head; healthViewNode != null; healthViewNode = healthViewNode.Next)
			{
				HealthViewComponent healthView = healthViewNode.HealthView;
				if (healthView != null)
				{
					healthView.UpdateLocation();
					if (healthView.AutoRegenerating)
					{
						this.UpdateAutoRegeneration(healthView, dt);
					}
				}
			}
		}

		private void UpdateAutoRegeneration(HealthViewComponent healthView, float dt)
		{
			this.timeSinceRegenerationUpdate += dt;
			if (this.timeSinceRegenerationUpdate >= 0.1f)
			{
				this.timeSinceRegenerationUpdate = 0f;
				int num = healthView.HealthAmount + 80;
				int maxHealthAmount = healthView.MaxHealthAmount;
				if (num > maxHealthAmount)
				{
					num = maxHealthAmount;
				}
				healthView.UpdateHealth(num, maxHealthAmount, false);
				if (num == maxHealthAmount)
				{
					healthView.AutoRegenerating = false;
					healthView.TeardownElements();
					Service.Get<ViewTimerManager>().CreateViewTimer(1f, false, new TimerDelegate(this.NotifyAutoRegenerationFinished), healthView);
				}
				this.UpdateRubbleStateFromHealthView(healthView);
			}
		}

		public void UpdateRubbleStateFromHealthView(HealthViewComponent healthView)
		{
			float num = (healthView.MaxHealthAmount == 0) ? 0f : ((float)healthView.HealthAmount / (float)healthView.MaxHealthAmount);
			bool flag = num < 0.2f;
			if (healthView.HasRubble)
			{
				if (!flag)
				{
					Service.Get<FXManager>().RemoveAttachedRubbleFromEntity(healthView.Entity);
					healthView.HasRubble = false;
					return;
				}
			}
			else if (flag)
			{
				Service.Get<FXManager>().CreateAndAttachRubbleToEntity(healthView.Entity);
				healthView.HasRubble = true;
			}
		}

		private void NotifyAutoRegenerationFinished(uint timerId, object cookie)
		{
			Service.Get<EventManager>().SendEvent(EventId.EntityHealthViewRegenerated, cookie);
		}

		public HealthRenderSystem()
		{
		}

		protected internal HealthRenderSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HealthRenderSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HealthRenderSystem)GCHandledObjects.GCHandleToObject(instance)).ForceUpdate();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HealthRenderSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HealthRenderSystem)GCHandledObjects.GCHandleToObject(instance)).Update(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((HealthRenderSystem)GCHandledObjects.GCHandleToObject(instance)).UpdateAutoRegeneration((HealthViewComponent)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((HealthRenderSystem)GCHandledObjects.GCHandleToObject(instance)).UpdateRubbleStateFromHealthView((HealthViewComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
