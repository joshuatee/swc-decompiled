using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.FX
{
	public class MobilizationEffectsManager : IEventObserver
	{
		private const string NAV_CENTER_FX_UID = "GalacticNavHologram";

		private Dictionary<uint, BuildingHoloEffect> effectsByEntityId;

		public MobilizationEffectsManager()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.ContractStarted, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ContractContinued, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ContractCanceled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.HeroMobilized, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.StarshipMobilized, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.HeroMobilizedFromPrize, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.StarshipMobilizedFromPrize, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingCancelled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingConstructed, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingReplaced, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.WorldReset, EventPriority.Default);
			Service.Set<MobilizationEffectsManager>(this);
		}

		private void CreateEffect(Entity building, string unitUid, bool isStarship, bool isNavCenter)
		{
			if (this.effectsByEntityId == null)
			{
				this.effectsByEntityId = new Dictionary<uint, BuildingHoloEffect>();
			}
			BuildingHoloEffect buildingHoloEffect;
			if (!this.effectsByEntityId.ContainsKey(building.ID))
			{
				buildingHoloEffect = new BuildingHoloEffect(building, unitUid, isStarship, isNavCenter);
				this.effectsByEntityId.Add(building.ID, buildingHoloEffect);
				return;
			}
			buildingHoloEffect = this.effectsByEntityId[building.ID];
			buildingHoloEffect.CreateMobilizationHolo(unitUid, isStarship, isNavCenter);
		}

		private void RemoveEffectByEntityId(uint entityId)
		{
			if (this.effectsByEntityId != null && this.effectsByEntityId.ContainsKey(entityId))
			{
				BuildingHoloEffect buildingHoloEffect = this.effectsByEntityId[entityId];
				buildingHoloEffect.Cleanup();
				this.effectsByEntityId.Remove(entityId);
			}
		}

		private void AddAllEffects(bool checkContracts)
		{
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			NodeList<TacticalCommandNode> tacticalCommandNodeList = buildingLookupController.TacticalCommandNodeList;
			for (TacticalCommandNode tacticalCommandNode = tacticalCommandNodeList.Head; tacticalCommandNode != null; tacticalCommandNode = tacticalCommandNode.Next)
			{
				this.UpdateEffectsForBuilding(tacticalCommandNode.BuildingComp, false, checkContracts);
			}
			NodeList<FleetCommandNode> fleetCommandNodeList = buildingLookupController.FleetCommandNodeList;
			for (FleetCommandNode fleetCommandNode = fleetCommandNodeList.Head; fleetCommandNode != null; fleetCommandNode = fleetCommandNode.Next)
			{
				this.UpdateEffectsForBuilding(fleetCommandNode.BuildingComp, true, checkContracts);
			}
			this.AddNavigationCenterHolo();
		}

		private void AddNavigationCenterHolo(Entity building)
		{
			if (building != null && building.Has<GameObjectViewComponent>())
			{
				this.CreateEffect(building, "GalacticNavHologram", false, true);
				return;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingViewReady, EventPriority.Default);
		}

		private void AddNavigationCenterHolo()
		{
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			NodeList<NavigationCenterNode> navigationCenterNodeList = buildingLookupController.NavigationCenterNodeList;
			for (NavigationCenterNode navigationCenterNode = navigationCenterNodeList.Head; navigationCenterNode != null; navigationCenterNode = navigationCenterNode.Next)
			{
				bool flag = ContractUtils.IsBuildingUpgrading(navigationCenterNode.Entity);
				bool flag2 = ContractUtils.IsBuildingConstructing(navigationCenterNode.Entity);
				if (!(flag2 | flag))
				{
					this.CreateEffect(navigationCenterNode.BuildingComp.Entity, "GalacticNavHologram", false, true);
				}
			}
		}

		private void UpdateEffectsForBuilding(BuildingComponent buildingComp, bool isStarship, bool checkContracts)
		{
			string mobilizedUnit = this.GetMobilizedUnit(isStarship);
			if (mobilizedUnit != null)
			{
				this.CreateEffect(buildingComp.Entity, mobilizedUnit, isStarship, false);
				return;
			}
			if (checkContracts && Service.Get<ISupportController>().HasTroopContractForBuilding(buildingComp.BuildingTO.Key))
			{
				this.CreateEffect(buildingComp.Entity, null, isStarship, false);
			}
		}

		public void UpdataAllEffects()
		{
			this.RemoveAllEffects();
			this.AddAllEffects(true);
		}

		private void RemoveAllEffects()
		{
			if (this.effectsByEntityId != null)
			{
				foreach (BuildingHoloEffect current in this.effectsByEntityId.Values)
				{
					current.Cleanup();
				}
				this.effectsByEntityId.Clear();
			}
		}

		public void TransferEffects(Entity oldBuilding, Entity newBuilding)
		{
			if (this.effectsByEntityId != null && this.effectsByEntityId.ContainsKey(oldBuilding.ID) && !this.effectsByEntityId.ContainsKey(newBuilding.ID))
			{
				BuildingHoloEffect buildingHoloEffect = this.effectsByEntityId[oldBuilding.ID];
				buildingHoloEffect.TransferEffect(newBuilding);
				this.effectsByEntityId.Remove(oldBuilding.ID);
				this.effectsByEntityId.Add(newBuilding.ID, buildingHoloEffect);
				if (buildingHoloEffect.WaitingForBuildingView)
				{
					Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingViewReady, EventPriority.Default);
				}
			}
		}

		private void OnContractStarted(ContractEventData data)
		{
			DeliveryType deliveryType = data.Contract.DeliveryType;
			if (deliveryType == DeliveryType.Hero || deliveryType == DeliveryType.Starship)
			{
				this.CreateEffect(data.Entity, null, false, false);
				return;
			}
			if (deliveryType == DeliveryType.UpgradeBuilding && data.BuildingVO.Type == BuildingType.NavigationCenter)
			{
				this.RemoveEffectByEntityId(data.Entity.ID);
			}
		}

		private void OnContractCanceled(ContractEventData data)
		{
			if (this.effectsByEntityId == null || !this.effectsByEntityId.ContainsKey(data.Entity.ID))
			{
				return;
			}
			DeliveryType deliveryType = data.Contract.DeliveryType;
			if (deliveryType == DeliveryType.Hero || deliveryType == DeliveryType.Starship)
			{
				if (this.GetMobilizedUnit(deliveryType == DeliveryType.Starship) != null)
				{
					return;
				}
				List<Contract> list = Service.Get<ISupportController>().FindAllTroopContractsForBuilding(data.BuildingKey);
				if (list.Count <= 1)
				{
					this.RemoveEffectByEntityId(data.Entity.ID);
				}
			}
		}

		private void OnUnitMobilized(ContractEventData data)
		{
			DeliveryType deliveryType = data.Contract.DeliveryType;
			if (deliveryType == DeliveryType.Hero || deliveryType == DeliveryType.Starship)
			{
				this.CreateEffect(data.Entity, data.Contract.ProductUid, deliveryType == DeliveryType.Starship, false);
			}
		}

		private string GetMobilizedUnit(bool isStarship)
		{
			IDataController dataController = Service.Get<IDataController>();
			if (!isStarship)
			{
				List<TroopTypeVO> list = null;
				IEnumerable<KeyValuePair<string, InventoryEntry>> allHeroes = Service.Get<CurrentPlayer>().GetAllHeroes();
				using (IEnumerator<KeyValuePair<string, InventoryEntry>> enumerator = allHeroes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, InventoryEntry> current = enumerator.get_Current();
						if (current.get_Value().Amount > 0)
						{
							if (list == null)
							{
								list = new List<TroopTypeVO>();
							}
							list.Add(dataController.Get<TroopTypeVO>(current.get_Key()));
						}
					}
				}
				if (list != null)
				{
					list.Sort(new Comparison<TroopTypeVO>(this.CompareTroops));
					return list[0].Uid;
				}
			}
			else
			{
				List<SpecialAttackTypeVO> list2 = null;
				IEnumerable<KeyValuePair<string, InventoryEntry>> allSpecialAttacks = Service.Get<CurrentPlayer>().GetAllSpecialAttacks();
				using (IEnumerator<KeyValuePair<string, InventoryEntry>> enumerator2 = allSpecialAttacks.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<string, InventoryEntry> current2 = enumerator2.get_Current();
						if (current2.get_Value().Amount > 0)
						{
							if (list2 == null)
							{
								list2 = new List<SpecialAttackTypeVO>();
							}
							list2.Add(dataController.Get<SpecialAttackTypeVO>(current2.get_Key()));
						}
					}
				}
				if (list2 != null)
				{
					list2.Sort(new Comparison<SpecialAttackTypeVO>(this.CompareStarships));
					return list2[0].Uid;
				}
			}
			return null;
		}

		private int CompareTroops(TroopTypeVO a, TroopTypeVO b)
		{
			return b.Order.CompareTo(a.Order);
		}

		private int CompareStarships(SpecialAttackTypeVO a, SpecialAttackTypeVO b)
		{
			return b.Order.CompareTo(a.Order);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.BuildingReplaced)
			{
				if (id <= EventId.BuildingCancelled)
				{
					if (id != EventId.BuildingViewReady)
					{
						if (id != EventId.BuildingCancelled)
						{
							return EatResponse.NotEaten;
						}
					}
					else
					{
						EntityViewParams entityViewParams = (EntityViewParams)cookie;
						if (this.effectsByEntityId != null && this.effectsByEntityId.ContainsKey(entityViewParams.Entity.ID))
						{
							BuildingHoloEffect buildingHoloEffect = this.effectsByEntityId[entityViewParams.Entity.ID];
							if (buildingHoloEffect.WaitingForBuildingView)
							{
								buildingHoloEffect.UpdateEffect();
							}
						}
						if (this.effectsByEntityId != null && entityViewParams.Entity.Has<NavigationCenterComponent>())
						{
							this.AddNavigationCenterHolo(entityViewParams.Entity);
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
				}
				else
				{
					switch (id)
					{
					case EventId.StarshipMobilized:
					case EventId.HeroMobilized:
						this.OnUnitMobilized((ContractEventData)cookie);
						return EatResponse.NotEaten;
					case EventId.StarshipMobilizedFromPrize:
					{
						Entity entity = Service.Get<BuildingLookupController>().FleetCommandNodeList.Head.Entity;
						this.CreateEffect(entity, (string)cookie, true, false);
						return EatResponse.NotEaten;
					}
					case EventId.HeroMobilizedFromPrize:
					{
						Entity entity2 = Service.Get<BuildingLookupController>().TacticalCommandNodeList.Head.Entity;
						this.CreateEffect(entity2, (string)cookie, false, false);
						return EatResponse.NotEaten;
					}
					default:
						if (id != EventId.BuildingConstructed)
						{
							if (id != EventId.BuildingReplaced)
							{
								return EatResponse.NotEaten;
							}
							Entity entity3 = cookie as Entity;
							if (entity3.Has<NavigationCenterComponent>())
							{
								this.AddNavigationCenterHolo(entity3);
								return EatResponse.NotEaten;
							}
							return EatResponse.NotEaten;
						}
						break;
					}
				}
				ContractEventData contractEventData = (ContractEventData)cookie;
				if (contractEventData.BuildingVO.Type == BuildingType.NavigationCenter)
				{
					this.AddNavigationCenterHolo();
				}
			}
			else if (id <= EventId.WorldReset)
			{
				if (id != EventId.WorldLoadComplete)
				{
					if (id == EventId.WorldReset)
					{
						this.RemoveAllEffects();
					}
				}
				else
				{
					IState currentState = Service.Get<GameStateMachine>().CurrentState;
					if (currentState is ApplicationLoadState || currentState is HomeState || currentState is WarBoardState)
					{
						this.AddAllEffects(false);
					}
				}
			}
			else if (id != EventId.ContractStarted && id != EventId.ContractContinued)
			{
				if (id == EventId.ContractCanceled)
				{
					this.OnContractCanceled((ContractEventData)cookie);
				}
			}
			else
			{
				this.OnContractStarted((ContractEventData)cookie);
			}
			return EatResponse.NotEaten;
		}

		protected internal MobilizationEffectsManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).AddAllEffects(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).AddNavigationCenterHolo();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).AddNavigationCenterHolo((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).CompareStarships((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args), (SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).CompareTroops((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (TroopTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).CreateEffect((Entity)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).GetMobilizedUnit(*(sbyte*)args != 0));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).OnContractCanceled((ContractEventData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).OnContractStarted((ContractEventData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).OnUnitMobilized((ContractEventData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).RemoveAllEffects();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).TransferEffects((Entity)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).UpdataAllEffects();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((MobilizationEffectsManager)GCHandledObjects.GCHandleToObject(instance)).UpdateEffectsForBuilding((BuildingComponent)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0);
			return -1L;
		}
	}
}
