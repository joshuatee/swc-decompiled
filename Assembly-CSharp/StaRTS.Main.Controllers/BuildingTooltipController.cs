using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class BuildingTooltipController : IEventObserver
	{
		private const string BUILDINGTOOLTIP_NAME = "BuildingTooltip";

		private const string ARMORY_TOOLTIP_FULL = "ARMORY_TOOLTIP_FULL";

		private const string ARMORY_TOOLTIP_EMPTY = "ARMORY_TOOLTIP_EMPTY";

		private const int TOOLTIP_SLOT_DIMENSION = 50;

		private const int TOOLTIP_SLOT_FACTOR = 3;

		private const int TOOLTIP_SLOT_EDGE = 150;

		private bool[] tooltipSlots;

		public BuildingTooltipController()
		{
			Service.Set<BuildingTooltipController>(this);
			this.tooltipSlots = new bool[22500];
			this.ResetTooltipSlots();
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.EquipmentNowUpgradable, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.InventoryCapacityChanged, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.InventoryTroopUpdated, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.NumInventoryItemsNotViewedUpdated, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ShardUnitNowUpgradable, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.TroopUpgradeScreenOpened, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ScreenSizeChanged, EventPriority.Default);
		}

		public void ResetTooltipSlots()
		{
			int i = 0;
			int num = this.tooltipSlots.Length;
			while (i < num)
			{
				this.tooltipSlots[i] = false;
				i++;
			}
		}

		public bool UpdateTooltipScreenSlot(TooltipHelper tooltip, float x, float y)
		{
			this.ClearTooltipScreenSlot(tooltip);
			float num = (float)Screen.width;
			float num2 = (float)Screen.height;
			x -= num * 0.5f;
			y -= num2 * 0.5f;
			num *= 3f;
			num2 *= 3f;
			x += num * 0.5f;
			y += num2 * 0.5f;
			int num3 = (int)(150f * x / num);
			int num4 = (int)(150f * y / num2);
			if (num3 >= 0 && num3 < 150 && num4 >= 0 && num4 < 150)
			{
				int num5 = num4 * 150 + num3;
				bool flag = this.tooltipSlots[num5];
				if (!flag && tooltip != null)
				{
					this.tooltipSlots[num5] = true;
					tooltip.Slot = num5;
				}
				return flag;
			}
			return true;
		}

		public void ClearTooltipScreenSlot(TooltipHelper tooltip)
		{
			if (tooltip != null && tooltip.Slot >= 0)
			{
				this.tooltipSlots[tooltip.Slot] = false;
				tooltip.Slot = -1;
			}
		}

		public void EnsureBuildingTooltip(SmartEntity building)
		{
			if (building == null || building.SupportViewComp == null || Service.Get<PostBattleRepairController>().IsEntityInRepair(building))
			{
				return;
			}
			SupportViewComponent supportViewComp = building.SupportViewComp;
			BuildingComponent buildingComp = building.BuildingComp;
			BuildingTypeVO buildingType = buildingComp.BuildingType;
			if (!supportViewComp.Enabled)
			{
				return;
			}
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is WarBoardState)
			{
				return;
			}
			bool flag = currentState is HomeState;
			bool editState = currentState is EditBaseState;
			bool baseLayoutState = currentState is BaseLayoutToolState;
			int num = -1;
			int timeTotal = -1;
			string text = null;
			IUpgradeableVO iconAsset = null;
			bool flag2 = Service.Get<BuildingController>().SelectedBuilding == building;
			SupportViewComponentState supportViewComponentState = SupportViewComponentState.Dormant;
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComp.BuildingTO.Key);
			if (contract != null)
			{
				if (this.ShouldShowProgress(contract, flag, editState, baseLayoutState, building))
				{
					int remainingTimeForView = contract.GetRemainingTimeForView();
					if (remainingTimeForView <= 0 || !Service.Get<ISupportController>().IsContractValidForStorage(contract))
					{
						text = Service.Get<Lang>().Get("STOPPED", new object[0]);
						supportViewComponentState = SupportViewComponentState.Bubble;
					}
					else
					{
						num = remainingTimeForView;
						timeTotal = contract.TotalTime;
						if (this.ShouldShowIcon(contract.DeliveryType, flag))
						{
							IDataController dataController = Service.Get<IDataController>();
							DeliveryType deliveryType = contract.DeliveryType;
							if (deliveryType != DeliveryType.Starship && deliveryType != DeliveryType.UpgradeStarship)
							{
								if (deliveryType == DeliveryType.UpgradeEquipment)
								{
									iconAsset = dataController.Get<EquipmentVO>(contract.ProductUid);
								}
								else
								{
									iconAsset = dataController.Get<TroopTypeVO>(contract.ProductUid);
								}
							}
							else
							{
								iconAsset = dataController.Get<SpecialAttackTypeVO>(contract.ProductUid);
							}
							supportViewComponentState = SupportViewComponentState.IconProgress;
						}
						else
						{
							supportViewComponentState = SupportViewComponentState.Progress;
						}
					}
				}
			}
			else
			{
				text = this.GetBubbleText(building, flag, editState, flag2);
				if (text != null)
				{
					supportViewComponentState = this.GetBubbleViewComponentStateBasedOnBuilding(building);
				}
			}
			if ((flag2 & flag) && supportViewComponentState == SupportViewComponentState.Dormant && buildingType.Type == BuildingType.Starport)
			{
				supportViewComponentState = SupportViewComponentState.General;
			}
			if (supportViewComponentState != SupportViewComponentState.Dormant)
			{
				bool flag3 = supportViewComponentState == supportViewComp.State;
				BuildingTooltip buildingTooltip;
				if (flag3)
				{
					buildingTooltip = supportViewComp.BuildingTooltip;
				}
				else
				{
					supportViewComp.TeardownElements();
					UXController uXController = Service.Get<UXController>();
					MiscElementsManager miscElementsManager = uXController.MiscElementsManager;
					GameObjectViewComponent gameObjectViewComp = building.GameObjectViewComp;
					string text2 = gameObjectViewComp.MainGameObject.GetInstanceID().ToString();
					string name = "BuildingTooltip" + text2;
					GameObject worldUIParent = Service.Get<UXController>().WorldUIParent;
					switch (supportViewComponentState)
					{
					case SupportViewComponentState.Bubble:
						if (buildingType.Type == BuildingType.ChampionPlatform)
						{
							buildingTooltip = miscElementsManager.CreateBubbleRepairTooltip(name, worldUIParent);
						}
						else
						{
							buildingTooltip = miscElementsManager.CreateBubbleTooltip(name, worldUIParent);
						}
						break;
					case SupportViewComponentState.BubbleHQ:
					case SupportViewComponentState.BubbleArmoryUpgrade:
					case SupportViewComponentState.BubbleShardUpgrade:
						buildingTooltip = miscElementsManager.CreateHQBubbleTooltip(name, worldUIParent);
						break;
					case SupportViewComponentState.Progress:
						buildingTooltip = miscElementsManager.CreateProgressTooltip(name, worldUIParent);
						break;
					case SupportViewComponentState.IconProgress:
						buildingTooltip = miscElementsManager.CreateIconProgressTooltip(name, worldUIParent);
						break;
					default:
						buildingTooltip = miscElementsManager.CreateGeneralTooltip(name, worldUIParent);
						break;
					}
				}
				buildingTooltip.SetSelected(flag2);
				buildingTooltip.SetTitle(this.GetBuildingTitle(buildingType));
				buildingTooltip.SetLevel(buildingType);
				buildingTooltip.SetBubbleText(text);
				buildingTooltip.SetIconAsset(iconAsset);
				buildingTooltip.SetTime(num);
				buildingTooltip.SetProgress(num, timeTotal);
				if (!flag3)
				{
					supportViewComp.SetupElements(buildingTooltip, supportViewComponentState);
				}
			}
		}

		private SupportViewComponentState GetBubbleViewComponentStateBasedOnBuilding(SmartEntity building)
		{
			SupportViewComponentState result = SupportViewComponentState.Bubble;
			if (building == null)
			{
				return result;
			}
			BuildingComponent buildingComp = building.BuildingComp;
			if (buildingComp == null)
			{
				return result;
			}
			BuildingTypeVO buildingType = buildingComp.BuildingType;
			if (buildingType.Type == BuildingType.HQ)
			{
				return SupportViewComponentState.BubbleHQ;
			}
			if (buildingType.Type == BuildingType.Armory && this.ShouldBadgeArmoryBuilding())
			{
				result = SupportViewComponentState.BubbleArmoryUpgrade;
			}
			if (buildingType.Type == BuildingType.TroopResearch && this.ShouldBadgeResearchBuilding())
			{
				result = SupportViewComponentState.BubbleShardUpgrade;
			}
			return result;
		}

		private bool ShouldBadgeArmoryBuilding()
		{
			string pref = Service.Get<SharedPlayerPrefs>().GetPref<string>("NewEqp");
			return pref != null && pref != "false";
		}

		private bool ShouldBadgeResearchBuilding()
		{
			DeployableShardUnlockController deployableShardUnlockController = Service.Get<DeployableShardUnlockController>();
			ArmoryController armoryController = Service.Get<ArmoryController>();
			bool flag = deployableShardUnlockController.AllowResearchBuildingBadging && deployableShardUnlockController.DoesUserHaveAnyUpgradeableShardUnits();
			bool flag2 = armoryController.AllowShowEquipmentTabBadge && armoryController.DoesUserHaveAnyUpgradableEquipment();
			return flag | flag2;
		}

		public void HideBuildingTooltip(SmartEntity building)
		{
			SupportViewComponent supportViewComp = building.SupportViewComp;
			if (supportViewComp != null)
			{
				supportViewComp.TeardownElements();
			}
		}

		private string GetBubbleText(SmartEntity building, bool homeState, bool editState, bool isSelected)
		{
			string result = null;
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			if (homeState)
			{
				BuildingTypeVO buildingType = building.BuildingComp.BuildingType;
				BuildingType type = buildingType.Type;
				switch (type)
				{
				case BuildingType.HQ:
				{
					int numInventoryItemsNotViewed = GameUtils.GetNumInventoryItemsNotViewed();
					if (!Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress && numInventoryItemsNotViewed > 0)
					{
						result = Service.Get<Lang>().Get("HQ_BADGE", new object[0]);
						return result;
					}
					return result;
				}
				case BuildingType.Barracks:
				case BuildingType.Factory:
				case BuildingType.DefenseResearch:
					break;
				case BuildingType.FleetCommand:
				{
					int totalStorageCapacity = worldOwner.Inventory.SpecialAttack.GetTotalStorageCapacity();
					int totalStorageAmount = worldOwner.Inventory.SpecialAttack.GetTotalStorageAmount();
					if (totalStorageAmount >= totalStorageCapacity)
					{
						result = Service.Get<Lang>().Get("FULL", new object[0]);
						return result;
					}
					result = LangUtils.GetBuildingVerb(buildingType.Type);
					return result;
				}
				case BuildingType.HeroMobilizer:
				{
					int totalStorageCapacity2 = worldOwner.Inventory.Hero.GetTotalStorageCapacity();
					int totalStorageAmount2 = worldOwner.Inventory.Hero.GetTotalStorageAmount();
					if (totalStorageAmount2 >= totalStorageCapacity2)
					{
						result = Service.Get<Lang>().Get("FULL", new object[0]);
						return result;
					}
					result = LangUtils.GetBuildingVerb(buildingType.Type);
					return result;
				}
				case BuildingType.ChampionPlatform:
				case BuildingType.Housing:
				case BuildingType.DroidHut:
				case BuildingType.Wall:
				case BuildingType.Turret:
					return result;
				case BuildingType.Squad:
				{
					if (worldOwner.Squad == null)
					{
						result = Service.Get<Lang>().Get("context_Join", new object[0]);
						return result;
					}
					int donatedTroopStorageUsedByWorldOwner = SquadUtils.GetDonatedTroopStorageUsedByWorldOwner();
					int storage = buildingType.Storage;
					if (donatedTroopStorageUsedByWorldOwner >= storage)
					{
						result = Service.Get<Lang>().Get("FULL", new object[0]);
						return result;
					}
					result = Service.Get<Lang>().Get("context_RequestTroops", new object[0]);
					return result;
				}
				case BuildingType.Starport:
				{
					if (isSelected)
					{
						return result;
					}
					int donatedTroopStorageUsedByWorldOwner;
					int storage;
					GameUtils.GetStarportTroopCounts(out donatedTroopStorageUsedByWorldOwner, out storage);
					if (donatedTroopStorageUsedByWorldOwner >= storage)
					{
						result = Service.Get<Lang>().Get("FULL", new object[0]);
						return result;
					}
					return result;
				}
				case BuildingType.TroopResearch:
					if (this.ShouldBadgeResearchBuilding())
					{
						result = Service.Get<Lang>().Get("HQ_BADGE", new object[0]);
						return result;
					}
					result = LangUtils.GetBuildingVerb(buildingType.Type);
					return result;
				default:
					switch (type)
					{
					case BuildingType.Cantina:
					case BuildingType.NavigationCenter:
						break;
					case BuildingType.ScoutTower:
						return result;
					case BuildingType.Armory:
						if (this.ShouldBadgeArmoryBuilding())
						{
							result = Service.Get<Lang>().Get("HQ_BADGE", new object[0]);
							return result;
						}
						if (ArmoryUtils.IsArmoryFull(Service.Get<CurrentPlayer>()))
						{
							result = Service.Get<Lang>().Get("ARMORY_TOOLTIP_FULL", new object[0]);
							return result;
						}
						if (ArmoryUtils.IsArmoryEmpty(Service.Get<CurrentPlayer>()))
						{
							result = Service.Get<Lang>().Get("ARMORY_TOOLTIP_EMPTY", new object[0]);
							return result;
						}
						result = LangUtils.GetBuildingVerb(buildingType.Type);
						return result;
					default:
						return result;
					}
					break;
				}
				result = LangUtils.GetBuildingVerb(buildingType.Type);
			}
			return result;
		}

		private bool ShouldShowProgress(Contract contract, bool homeState, bool editState, bool baseLayoutState, Entity building)
		{
			if (contract.TotalTime == 0)
			{
				return false;
			}
			switch (contract.DeliveryType)
			{
			case DeliveryType.Infantry:
			case DeliveryType.Vehicle:
			case DeliveryType.Starship:
			case DeliveryType.Hero:
			case DeliveryType.Champion:
			case DeliveryType.UpgradeTroop:
			case DeliveryType.UpgradeStarship:
			case DeliveryType.UpgradeEquipment:
			case DeliveryType.Mercenary:
				return homeState;
			case DeliveryType.Building:
				return !Service.Get<BuildingController>().IsLifted(building) && !baseLayoutState;
			case DeliveryType.UpgradeBuilding:
			case DeliveryType.SwapBuilding:
				return homeState | editState;
			case DeliveryType.ClearClearable:
				return homeState | editState | baseLayoutState;
			default:
				return false;
			}
		}

		private bool ShouldShowIcon(DeliveryType contractType, bool homeState)
		{
			if (!homeState)
			{
				return false;
			}
			switch (contractType)
			{
			case DeliveryType.Infantry:
			case DeliveryType.Vehicle:
			case DeliveryType.Starship:
			case DeliveryType.Hero:
			case DeliveryType.UpgradeTroop:
			case DeliveryType.UpgradeStarship:
			case DeliveryType.UpgradeEquipment:
			case DeliveryType.Mercenary:
				return true;
			}
			return false;
		}

		private string GetBuildingTitle(BuildingTypeVO buildingInfo)
		{
			string text = LangUtils.GetBuildingDisplayName(buildingInfo);
			if (buildingInfo.Type == BuildingType.Starport)
			{
				int num;
				int num2;
				GameUtils.GetStarportTroopCounts(out num, out num2);
				text = string.Format("{0} {1}", new object[]
				{
					text,
					Service.Get<Lang>().Get("FRACTION", new object[]
					{
						num,
						num2
					})
				});
			}
			return text;
		}

		private void UpdateAllSupportBuildingTooltips()
		{
			NodeList<SupportViewNode> nodeList = Service.Get<EntityController>().GetNodeList<SupportViewNode>();
			for (SupportViewNode supportViewNode = nodeList.Head; supportViewNode != null; supportViewNode = supportViewNode.Next)
			{
				if (supportViewNode.SupportView != null)
				{
					supportViewNode.SupportView.TeardownElements();
					this.EnsureBuildingTooltip((SmartEntity)supportViewNode.SupportView.Entity);
				}
			}
		}

		private void UpdateAllStarportTooltips()
		{
			NodeList<StarportNode> starportNodeList = Service.Get<BuildingLookupController>().StarportNodeList;
			for (StarportNode starportNode = starportNodeList.Head; starportNode != null; starportNode = starportNode.Next)
			{
				SupportViewComponent supportViewComponent = starportNode.Entity.Get<SupportViewComponent>();
				if (supportViewComponent != null)
				{
					supportViewComponent.TeardownElements();
					this.EnsureBuildingTooltip((SmartEntity)supportViewComponent.Entity);
				}
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.NumInventoryItemsNotViewedUpdated)
			{
				if (id <= EventId.GameStateChanged)
				{
					if (id != EventId.InventoryCapacityChanged && id != EventId.GameStateChanged)
					{
						return EatResponse.NotEaten;
					}
				}
				else
				{
					if (id == EventId.InventoryTroopUpdated)
					{
						this.UpdateAllStarportTooltips();
						return EatResponse.NotEaten;
					}
					if (id != EventId.NumInventoryItemsNotViewedUpdated)
					{
						return EatResponse.NotEaten;
					}
				}
			}
			else if (id <= EventId.EquipmentNowUpgradable)
			{
				if (id != EventId.ShardUnitNowUpgradable && id != EventId.EquipmentNowUpgradable)
				{
					return EatResponse.NotEaten;
				}
			}
			else if (id != EventId.TroopUpgradeScreenOpened)
			{
				if (id != EventId.ScreenSizeChanged)
				{
					return EatResponse.NotEaten;
				}
				this.UpdateAllSupportBuildingTooltips();
				return EatResponse.NotEaten;
			}
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is HomeState || currentState is EditBaseState || currentState is BaseLayoutToolState || currentState is WarBoardState)
			{
				this.UpdateAllSupportBuildingTooltips();
			}
			return EatResponse.NotEaten;
		}

		protected internal BuildingTooltipController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).ClearTooltipScreenSlot((TooltipHelper)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).EnsureBuildingTooltip((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).GetBubbleText((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).GetBubbleViewComponentStateBasedOnBuilding((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).GetBuildingTitle((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).HideBuildingTooltip((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).ResetTooltipSlots();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).ShouldBadgeArmoryBuilding());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).ShouldBadgeResearchBuilding());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).ShouldShowIcon((DeliveryType)(*(int*)args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).ShouldShowProgress((Contract)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0, (Entity)GCHandledObjects.GCHandleToObject(args[4])));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).UpdateAllStarportTooltips();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).UpdateAllSupportBuildingTooltips();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltipController)GCHandledObjects.GCHandleToObject(instance)).UpdateTooltipScreenSlot((TooltipHelper)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(float*)(args + 2)));
		}
	}
}
