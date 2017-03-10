using StaRTS.Main.Controllers.Entities.Systems;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class HomeModeController : IEventObserver
	{
		private EntityController entityController;

		private EventManager eventManager;

		public bool Enabled
		{
			get;
			private set;
		}

		public HomeModeController()
		{
			Service.Set<HomeModeController>(this);
			this.entityController = Service.Get<EntityController>();
			this.eventManager = Service.Get<EventManager>();
			this.eventManager.RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.GameStateChanged)
			{
				if (id != EventId.UserWantedEditBaseState)
				{
					if (id == EventId.InventoryResourceUpdated)
					{
						if (cookie as string == "droids")
						{
							Service.Get<UXController>().HUD.UpdateDroidCount();
						}
					}
				}
				else
				{
					Service.Get<GameStateMachine>().SetState(new EditBaseState((bool)cookie));
				}
			}
			else
			{
				this.HandleGameStateChanged();
			}
			return EatResponse.NotEaten;
		}

		private void HandleGameStateChanged()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			bool flag = currentState is HomeState;
			if (flag)
			{
				this.eventManager.RegisterObserver(this, EventId.UserWantedEditBaseState, EventPriority.Default);
			}
			else
			{
				this.eventManager.UnregisterObserver(this, EventId.UserWantedEditBaseState);
			}
			if (flag || currentState is EditBaseState || currentState is BaseLayoutToolState)
			{
				this.eventManager.RegisterObserver(this, EventId.InventoryTroopUpdated, EventPriority.Default);
				this.eventManager.RegisterObserver(this, EventId.InventoryResourceUpdated, EventPriority.Default);
				this.Enabled = true;
				if (!this.entityController.IsViewSystemSet<SupportViewSystem>())
				{
					this.entityController.AddViewSystem(new SupportViewSystem(), 2080, 65535);
				}
				Service.Get<UXController>().HUD.UpdateDroidCount();
				return;
			}
			this.eventManager.UnregisterObserver(this, EventId.InventoryTroopUpdated);
			this.eventManager.UnregisterObserver(this, EventId.InventoryResourceUpdated);
			this.Enabled = false;
			if (this.entityController.IsViewSystemSet<SupportViewSystem>())
			{
				this.entityController.RemoveViewSystem<SupportViewSystem>();
			}
		}

		protected internal HomeModeController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeModeController)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HomeModeController)GCHandledObjects.GCHandleToObject(instance)).HandleGameStateChanged();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeModeController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HomeModeController)GCHandledObjects.GCHandleToObject(instance)).Enabled = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
