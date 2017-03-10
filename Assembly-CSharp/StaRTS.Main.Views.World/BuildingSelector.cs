using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World
{
	public class BuildingSelector : IUserInputObserver, IEventObserver
	{
		private const int FINGER_ID = 0;

		public const float LIFT_TIMEOUT = 1f;

		public const float EDIT_MODE_TIMEOUT = 1f;

		private const float PULSATE_FREQUENCY = 1f;

		private Entity selectedBuilding;

		private List<Entity> additionalSelectedBuildings;

		private Vector3 grabPoint;

		private Vector2 pressScreenPosition;

		private bool dragged;

		private uint liftTimerId;

		private Entity liftingBuilding;

		private uint editModeTimerId;

		private DynamicRadiusView radiusView;

		private BuildingController buildingController;

		private LongPressView longPress;

		public bool Enabled
		{
			set
			{
				if (value)
				{
					this.EnsureRadiusView();
					Service.Get<UserInputManager>().RegisterObserver(this, UserInputLayer.World);
					return;
				}
				Service.Get<UserInputManager>().UnregisterObserver(this, UserInputLayer.World);
			}
		}

		public Entity SelectedBuilding
		{
			get
			{
				return this.selectedBuilding;
			}
		}

		public List<Entity> AdditionalSelectedBuildings
		{
			get
			{
				return this.additionalSelectedBuildings;
			}
		}

		public Vector3 GrabPoint
		{
			get
			{
				return this.grabPoint;
			}
		}

		public BuildingSelector()
		{
			this.selectedBuilding = null;
			this.grabPoint = Vector3.zero;
			this.pressScreenPosition = Vector2.zero;
			this.dragged = false;
			this.liftTimerId = 0u;
			this.liftingBuilding = null;
			this.editModeTimerId = 0u;
			this.radiusView = null;
			this.buildingController = Service.Get<BuildingController>();
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingViewReady, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
			this.longPress = new LongPressView();
			this.additionalSelectedBuildings = new List<Entity>();
		}

		public bool IsPartOfSelection(Entity building)
		{
			return building == this.selectedBuilding || this.additionalSelectedBuildings.Contains(building);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.BuildingViewReady)
			{
				if (id == EventId.GameStateChanged)
				{
					Type type = (Type)cookie;
					IState currentState = Service.Get<GameStateMachine>().CurrentState;
					if ((type != typeof(HomeState) || !(currentState is EditBaseState)) && (type != typeof(EditBaseState) || !(currentState is HomeState)))
					{
						this.EnsureDeselectSelectedBuilding();
					}
					Entity entity = (currentState is HomeState || currentState is EditBaseState) ? this.selectedBuilding : null;
					Service.Get<UXController>().HUD.ShowContextButtons(entity);
				}
			}
			else
			{
				EntityViewParams entityViewParams = cookie as EntityViewParams;
				if (this.IsPartOfSelection(entityViewParams.Entity))
				{
					this.ApplySelectedEffect(entityViewParams.Entity);
				}
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnPress(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (id != 0)
			{
				return EatResponse.NotEaten;
			}
			this.pressScreenPosition = screenPosition;
			this.dragged = false;
			Entity buildingEntity = this.GetBuildingEntity(target);
			if (buildingEntity == null)
			{
				this.StartEditModeTimer(groundPosition);
				return EatResponse.NotEaten;
			}
			if (!Service.Get<UserInputInhibitor>().IsAllowable(buildingEntity))
			{
				return EatResponse.NotEaten;
			}
			Vector3 offset = groundPosition - target.transform.position;
			this.GrabBuilding(offset);
			this.StartLiftingBuilding(buildingEntity);
			return EatResponse.NotEaten;
		}

		public EatResponse OnDrag(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (id != 0)
			{
				this.CancelEditModeTimer();
				return EatResponse.NotEaten;
			}
			if (!this.dragged && CameraUtils.HasDragged(screenPosition, this.pressScreenPosition))
			{
				this.dragged = true;
				this.CancelEditModeTimer();
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnRelease(int id)
		{
			if (id != 0)
			{
				return EatResponse.NotEaten;
			}
			if (!this.dragged && !this.buildingController.IsPurchasing)
			{
				if (this.liftingBuilding == null)
				{
					this.CancelEditModeTimer();
					if (this.selectedBuilding != null && !Service.Get<UserInputInhibitor>().IsDenying())
					{
						this.DeselectSelectedBuilding();
					}
				}
				else
				{
					Entity entity = this.liftingBuilding;
					this.CancelLiftingBuilding();
					if (this.selectedBuilding != null && Service.Get<UserInputInhibitor>().IsAllowable(entity))
					{
						if (entity != this.selectedBuilding && !this.additionalSelectedBuildings.Contains(entity))
						{
							this.DeselectSelectedBuilding();
						}
						else if (!Service.Get<ICurrencyController>().TryCollectCurrencyOnSelection(entity))
						{
							Service.Get<EventManager>().SendEvent(EventId.BuildingSelected, this.selectedBuilding);
							Service.Get<EventManager>().SendEvent(EventId.BuildingSelectedSound, this.selectedBuilding);
						}
					}
					if (this.selectedBuilding == null)
					{
						if (!Service.Get<ICurrencyController>().TryCollectCurrencyOnSelection(entity) && this.CanSelectBuilding((SmartEntity)entity))
						{
							this.SelectBuilding(entity, this.grabPoint);
						}
					}
					else if (!Service.Get<UserInputInhibitor>().IsDenying())
					{
						this.DeselectSelectedBuilding();
					}
				}
			}
			return EatResponse.NotEaten;
		}

		private bool CanSelectBuilding(SmartEntity building)
		{
			bool result = false;
			if (building != null)
			{
				result = true;
				if (GameUtils.IsVisitingBase())
				{
					bool flag = building.TrapComp != null;
					if (flag)
					{
						result = false;
					}
				}
			}
			return result;
		}

		public EatResponse OnScroll(float delta, Vector2 screenPosition)
		{
			return EatResponse.NotEaten;
		}

		public Entity GetBuildingEntity(GameObject target)
		{
			if (target == null)
			{
				return null;
			}
			EntityRef component = target.GetComponent<EntityRef>();
			if (component == null)
			{
				return null;
			}
			if (component.Entity.Get<BuildingComponent>() == null)
			{
				return null;
			}
			return component.Entity;
		}

		public void SelectAdjacentWalls(Entity building)
		{
			WallConnector wallConnector = Service.Get<EntityViewManager>().WallConnector;
			List<Entity> wallChains = wallConnector.GetWallChains(building, -1, 0);
			List<Entity> wallChains2 = wallConnector.GetWallChains(building, 1, 0);
			List<Entity> wallChains3 = wallConnector.GetWallChains(building, 0, -1);
			List<Entity> wallChains4 = wallConnector.GetWallChains(building, 0, 1);
			List<Entity> list = new List<Entity>();
			if (wallChains.Count + wallChains2.Count > wallChains3.Count + wallChains4.Count)
			{
				list.AddRange(wallChains);
				list.AddRange(wallChains2);
			}
			else
			{
				list.AddRange(wallChains4);
				list.AddRange(wallChains3);
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.AddBuildingToSelection(list[i]);
			}
		}

		public void AddBuildingToSelection(Entity building)
		{
			if (this.selectedBuilding == null)
			{
				throw new Exception("Can't select additional buildings until a root building is selected!");
			}
			if (!this.additionalSelectedBuildings.Contains(building))
			{
				this.additionalSelectedBuildings.Add(building);
				this.ApplySelectedEffect(building);
				if (building.Has<SupportViewComponent>())
				{
					building.Get<SupportViewComponent>().UpdateSelected(true);
				}
			}
		}

		public void SelectBuilding(Entity building, Vector3 offset)
		{
			this.SelectBuilding(building, offset, false);
		}

		public void SelectBuilding(Entity building, Vector3 offset, bool silent)
		{
			if (this.selectedBuilding != null)
			{
				throw new Exception("Must deselect old building before selecting a new one");
			}
			this.selectedBuilding = building;
			this.GrabBuilding(offset);
			this.ApplySelectedEffect(this.selectedBuilding);
			Service.Get<UXController>().HUD.ShowContextButtons(this.selectedBuilding);
			if (this.selectedBuilding.Has<SupportViewComponent>())
			{
				this.selectedBuilding.Get<SupportViewComponent>().UpdateSelected(true);
			}
			Service.Get<EventManager>().SendEvent(EventId.BuildingSelected, this.selectedBuilding);
			if (!silent)
			{
				Service.Get<EventManager>().SendEvent(EventId.BuildingSelectedSound, this.selectedBuilding);
			}
		}

		public void GrabBuilding(Vector3 offset)
		{
			this.grabPoint = offset;
			this.grabPoint.y = 0f;
		}

		public void RemoveBuildingFromCurrentSelection(Entity building)
		{
			if (this.additionalSelectedBuildings.Contains(building))
			{
				this.additionalSelectedBuildings.Remove(building);
				this.ApplyDeselectedEffect(building);
				Service.Get<EventManager>().SendEvent(EventId.BuildingDeselected, building);
				return;
			}
			if (this.selectedBuilding == building)
			{
				List<Entity> list = new List<Entity>();
				Entity entity = null;
				if (this.additionalSelectedBuildings.Count > 0)
				{
					entity = this.additionalSelectedBuildings[0];
					list.AddRange(this.additionalSelectedBuildings.GetRange(1, this.additionalSelectedBuildings.Count - 1));
				}
				this.DeselectSelectedBuilding();
				if (entity != null)
				{
					this.SelectBuilding(entity, Vector3.zero);
					for (int i = 0; i < list.Count; i++)
					{
						this.AddBuildingToSelection(list[i]);
					}
				}
			}
		}

		public void DeselectSelectedBuilding()
		{
			if (this.selectedBuilding == null)
			{
				throw new Exception("Must have a selected building in order to deselect it");
			}
			Service.Get<UXController>().HUD.ShowContextButtons(null);
			Service.Get<EventManager>().SendEvent(EventId.BuildingDeselected, this.selectedBuilding);
			Entity entity = this.selectedBuilding;
			this.selectedBuilding = null;
			this.DeselectGroup();
			this.ApplyDeselectedEffect(entity);
			if (entity.Has<SupportViewComponent>())
			{
				entity.Get<SupportViewComponent>().UpdateSelected(false);
			}
		}

		private void DeselectGroup()
		{
			for (int i = 0; i < this.additionalSelectedBuildings.Count; i++)
			{
				Entity entity = this.additionalSelectedBuildings[i];
				this.ApplyDeselectedEffect(entity);
				Service.Get<EventManager>().SendEvent(EventId.BuildingDeselected, entity);
				if (entity.Has<SupportViewComponent>())
				{
					entity.Get<SupportViewComponent>().UpdateSelected(false);
				}
			}
			this.additionalSelectedBuildings.Clear();
		}

		public void EnsureDeselectSelectedBuilding()
		{
			if (this.liftingBuilding != null)
			{
				this.CancelLiftingBuilding();
			}
			if (this.selectedBuilding != null)
			{
				this.DeselectSelectedBuilding();
			}
		}

		private void StartLiftingBuilding(Entity building)
		{
			if (!Service.Get<UserInputInhibitor>().IsDenying())
			{
				this.ApplyLiftingEffect(building);
				this.liftTimerId = Service.Get<ViewTimerManager>().CreateViewTimer(1f, false, new TimerDelegate(this.OnLiftTimer), null);
			}
			this.liftingBuilding = building;
		}

		private void StartEditModeTimer(Vector3 groundPosition)
		{
			if (!Service.Get<UserInputInhibitor>().IsDenying() && !GameUtils.IsVisitingBase())
			{
				this.longPress.StartTimer(groundPosition);
				this.editModeTimerId = Service.Get<ViewTimerManager>().CreateViewTimer(1f, false, new TimerDelegate(this.OnEditModeTimer), null);
			}
		}

		public void CancelEditModeTimer()
		{
			if (this.editModeTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.editModeTimerId);
				this.editModeTimerId = 0u;
			}
			this.longPress.KillTimer();
			if (this.liftingBuilding != null)
			{
				this.CancelLiftingBuilding();
			}
		}

		private void CancelLiftingBuilding()
		{
			if (this.liftingBuilding == this.selectedBuilding)
			{
				this.ApplySelectedEffect(this.liftingBuilding);
			}
			else
			{
				this.ApplyDeselectedEffect(this.liftingBuilding);
			}
			Service.Get<ViewTimerManager>().KillViewTimer(this.liftTimerId);
			this.longPress.KillTimer();
			this.liftTimerId = 0u;
			this.liftingBuilding = null;
		}

		private void OnEditModeTimer(uint id, object cookie)
		{
			this.longPress.KillTimer();
			Service.Get<EventManager>().SendEvent(EventId.UserWantedEditBaseState, false);
		}

		private void OnLiftTimer(uint id, object cookie)
		{
			if (id == this.liftTimerId && this.liftingBuilding != null)
			{
				if (this.liftingBuilding != this.selectedBuilding)
				{
					if (this.selectedBuilding != null)
					{
						this.DeselectSelectedBuilding();
					}
					this.SelectBuilding(this.liftingBuilding, this.grabPoint);
				}
				else
				{
					this.ApplySelectedEffect(this.liftingBuilding);
				}
				Service.Get<EventManager>().SendEvent(EventId.UserWantedEditBaseState, true);
				this.liftTimerId = 0u;
				this.liftingBuilding = null;
			}
		}

		private void EnsureRadiusView()
		{
			if (this.radiusView == null)
			{
				this.radiusView = new DynamicRadiusView();
			}
		}

		public void RedrawRadiusView(Entity building)
		{
			this.EnsureRadiusView();
			this.radiusView.HideHighlight();
			this.radiusView.ShowHighlight(building);
		}

		public void ApplySelectedEffect(Entity building)
		{
			this.buildingController.HighlightBuilding(building);
			this.RedrawRadiusView(building);
			this.longPress.KillTimer();
		}

		private void ApplyDeselectedEffect(Entity building)
		{
			this.buildingController.ClearBuildingHighlight(building);
			this.buildingController.UpdateBuildingHighlightForPerks(building);
			this.EnsureRadiusView();
			this.radiusView.HideHighlight(building);
		}

		private void ApplyLiftingEffect(Entity building)
		{
			if (!GameUtils.IsVisitingBase())
			{
				this.longPress.StartTimer(building);
			}
		}

		protected internal BuildingSelector(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).AddBuildingToSelection((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).ApplyDeselectedEffect((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).ApplyLiftingEffect((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).ApplySelectedEffect((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).CancelEditModeTimer();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).CancelLiftingBuilding();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).CanSelectBuilding((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).DeselectGroup();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).DeselectSelectedBuilding();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).EnsureDeselectSelectedBuilding();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).EnsureRadiusView();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).AdditionalSelectedBuildings);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).GrabPoint);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).SelectedBuilding);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).GetBuildingEntity((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).GrabBuilding(*(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).IsPartOfSelection((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).OnRelease(*(int*)args));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args, *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).RedrawRadiusView((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).RemoveBuildingFromCurrentSelection((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).SelectAdjacentWalls((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).SelectBuilding((Entity)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).SelectBuilding((Entity)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).Enabled = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).StartEditModeTimer(*(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((BuildingSelector)GCHandledObjects.GCHandleToObject(instance)).StartLiftingBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
