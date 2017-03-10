using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.Main.Controllers.CombatTriggers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class CombatTriggerManager : IEventObserver
	{
		private const uint EFFECTIVELY_NEVER_SECONDS = 1800u;

		private List<ICombatTrigger> deathTriggers;

		private List<ICombatTrigger> areaTriggers;

		private List<ICombatTrigger> loadTriggers;

		private List<ICombatTrigger> deployTriggers;

		private uint lowestDelay;

		public CombatTriggerManager()
		{
			Service.Set<CombatTriggerManager>(this);
			this.ResetLowestDelay();
			this.deathTriggers = new List<ICombatTrigger>();
			this.areaTriggers = new List<ICombatTrigger>();
			this.loadTriggers = new List<ICombatTrigger>();
			this.deployTriggers = new List<ICombatTrigger>();
		}

		public void Enable(bool enable)
		{
			EventManager eventManager = Service.Get<EventManager>();
			if (enable)
			{
				eventManager.RegisterObserver(this, EventId.EntityKilled, EventPriority.Default);
				eventManager.RegisterObserver(this, EventId.TroopDeployed, EventPriority.AfterDefault);
				eventManager.RegisterObserver(this, EventId.HeroDeployed, EventPriority.AfterDefault);
				eventManager.RegisterObserver(this, EventId.ChampionDeployed, EventPriority.AfterDefault);
				return;
			}
			eventManager.UnregisterObserver(this, EventId.EntityKilled);
			eventManager.UnregisterObserver(this, EventId.TroopDeployed);
			eventManager.UnregisterObserver(this, EventId.HeroDeployed);
			eventManager.UnregisterObserver(this, EventId.ChampionDeployed);
		}

		private void ResetLowestDelay()
		{
			this.lowestDelay = 1800000u;
		}

		public void RegisterTrigger(ICombatTrigger combatTrigger)
		{
			List<ICombatTrigger> list;
			switch (combatTrigger.Type)
			{
			case CombatTriggerType.Area:
			{
				list = this.areaTriggers;
				Entity entity = (Entity)combatTrigger.Owner;
				if (!entity.Has<AreaTriggerComponent>())
				{
					if (combatTrigger is TrapCombatTrigger)
					{
						TrapCombatTrigger trapCombatTrigger = (TrapCombatTrigger)combatTrigger;
						entity.Add<AreaTriggerComponent>(new AreaTriggerComponent(trapCombatTrigger.AreaRadius));
					}
					else
					{
						BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
						entity.Add<AreaTriggerComponent>(new AreaTriggerComponent(buildingComponent.BuildingType.ActivationRadius));
					}
				}
				break;
			}
			case CombatTriggerType.Death:
				list = this.deathTriggers;
				break;
			case CombatTriggerType.Load:
				list = this.loadTriggers;
				break;
			case CombatTriggerType.TroopDeploy:
				list = this.deployTriggers;
				break;
			default:
				list = this.deathTriggers;
				break;
			}
			if (!list.Contains(combatTrigger))
			{
				list.Add(combatTrigger);
				if (combatTrigger.LastDitchDelayMillis < this.lowestDelay)
				{
					this.lowestDelay = combatTrigger.LastDitchDelayMillis;
				}
			}
		}

		public void UnregisterAllTriggers()
		{
			this.deathTriggers.Clear();
			this.areaTriggers.Clear();
			this.loadTriggers.Clear();
			this.deployTriggers.Clear();
			this.ResetLowestDelay();
		}

		public void UpdateCurrentTime(uint battleMillis)
		{
			if (battleMillis < this.lowestDelay)
			{
				return;
			}
			this.ResetLowestDelay();
			this.ExecuteLastDitchTriggers(this.areaTriggers, battleMillis);
			this.ExecuteLastDitchTriggers(this.deathTriggers, battleMillis);
		}

		private void ExecuteLastDitchTriggers(List<ICombatTrigger> combatTriggerList, uint battleMillis)
		{
			int i = 0;
			int count = combatTriggerList.Count;
			while (i < count)
			{
				ICombatTrigger combatTrigger = combatTriggerList[i];
				if (!combatTrigger.IsAlreadyTriggered() && combatTrigger.LastDitchDelayMillis < this.lowestDelay)
				{
					this.lowestDelay = combatTrigger.LastDitchDelayMillis;
				}
				if (battleMillis >= combatTrigger.LastDitchDelayMillis)
				{
					this.TryAndTrigger(combatTrigger, null);
				}
				i++;
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.TroopDeployed)
			{
				if (id == EventId.EntityKilled)
				{
					List<ICombatTrigger> list = new List<ICombatTrigger>();
					int i = 0;
					int count = this.deathTriggers.Count;
					while (i < count)
					{
						ICombatTrigger combatTrigger = this.deathTriggers[i];
						if (combatTrigger.Owner == cookie)
						{
							this.TryAndTrigger(combatTrigger, null);
							list.Add(combatTrigger);
						}
						i++;
					}
					int j = 0;
					int count2 = this.areaTriggers.Count;
					while (j < count2)
					{
						if (this.areaTriggers[j].Owner == cookie)
						{
							list.Add(this.areaTriggers[j]);
							if (this.areaTriggers[j] is DefendedBuildingCombatTrigger)
							{
								DefendedBuildingCombatTrigger defendedBuildingCombatTrigger = (DefendedBuildingCombatTrigger)this.areaTriggers[j];
								defendedBuildingCombatTrigger.Type = CombatTriggerType.Death;
								if (defendedBuildingCombatTrigger.TroopsHurtable)
								{
									defendedBuildingCombatTrigger.InitialDelay = GameConstants.SPAWN_DELAY;
									defendedBuildingCombatTrigger.TroopsHurt = true;
									defendedBuildingCombatTrigger.Leashed = false;
								}
								this.TryAndTrigger(defendedBuildingCombatTrigger, null);
							}
						}
						j++;
					}
					for (int k = 0; k < list.Count; k++)
					{
						this.RemoveTrigger(list[k]);
					}
					return EatResponse.NotEaten;
				}
				if (id != EventId.TroopDeployed)
				{
					return EatResponse.NotEaten;
				}
			}
			else if (id != EventId.HeroDeployed && id != EventId.ChampionDeployed)
			{
				return EatResponse.NotEaten;
			}
			int l = 0;
			int count3 = this.deployTriggers.Count;
			while (l < count3)
			{
				ICombatTrigger combatTrigger2 = this.deployTriggers[l];
				string text = ((SmartEntity)cookie).TroopComp.TroopType.TroopID.ToLower();
				string text2 = (string)combatTrigger2.Owner;
				if (text == text2)
				{
					this.TryAndTrigger(combatTrigger2, null);
				}
				l++;
			}
			return EatResponse.NotEaten;
		}

		private void TryAndTrigger(ICombatTrigger trigger, Entity intruder)
		{
			if (trigger.IsAlreadyTriggered())
			{
				return;
			}
			trigger.Trigger(intruder);
		}

		private void RemoveTrigger(ICombatTrigger trigger)
		{
			this.deathTriggers.Remove(trigger);
			this.areaTriggers.Remove(trigger);
			this.loadTriggers.Remove(trigger);
			this.deployTriggers.Remove(trigger);
		}

		public void InformAreaTriggerBuildings(List<ElementPriorityPair<Entity>> areaTriggerBuildingsInRangeOf, SmartEntity troop)
		{
			int i = 0;
			int count = areaTriggerBuildingsInRangeOf.Count;
			while (i < count)
			{
				Entity element = areaTriggerBuildingsInRangeOf[i].Element;
				int j = 0;
				int count2 = this.areaTriggers.Count;
				while (j < count2)
				{
					ICombatTrigger combatTrigger = this.areaTriggers[j];
					if (combatTrigger.Owner == element)
					{
						this.TryAndTrigger(combatTrigger, troop);
					}
					j++;
				}
				i++;
			}
		}

		public void ExecuteLoadTriggers()
		{
			int i = 0;
			int count = this.loadTriggers.Count;
			while (i < count)
			{
				ICombatTrigger trigger = this.loadTriggers[i];
				this.TryAndTrigger(trigger, null);
				i++;
			}
		}

		protected internal CombatTriggerManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CombatTriggerManager)GCHandledObjects.GCHandleToObject(instance)).Enable(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CombatTriggerManager)GCHandledObjects.GCHandleToObject(instance)).ExecuteLoadTriggers();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CombatTriggerManager)GCHandledObjects.GCHandleToObject(instance)).InformAreaTriggerBuildings((List<ElementPriorityPair<Entity>>)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombatTriggerManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((CombatTriggerManager)GCHandledObjects.GCHandleToObject(instance)).RegisterTrigger((ICombatTrigger)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((CombatTriggerManager)GCHandledObjects.GCHandleToObject(instance)).RemoveTrigger((ICombatTrigger)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((CombatTriggerManager)GCHandledObjects.GCHandleToObject(instance)).ResetLowestDelay();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((CombatTriggerManager)GCHandledObjects.GCHandleToObject(instance)).TryAndTrigger((ICombatTrigger)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((CombatTriggerManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterAllTriggers();
			return -1L;
		}
	}
}
