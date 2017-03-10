using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.GameBoard;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player.Building.Move;
using StaRTS.Main.Models.Commands.TransferObjects;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class BaseLayoutToolController : IEventObserver
	{
		private PositionMap lastSavedMap;

		public Dictionary<string, List<Entity>> stashedBuildingMap;

		private const string NO_VALID_POSITION_FOR_UNSTASH = "NO_VALID_POSITION_FOR_UNSTASH";

		public bool IsBaseLayoutModeActive
		{
			get;
			private set;
		}

		public bool IsSavingBaseLayout
		{
			get;
			private set;
		}

		public bool ShouldRevertMap
		{
			get;
			private set;
		}

		public bool IsQuickStashModeEnabled
		{
			get;
			set;
		}

		public BaseLayoutToolController()
		{
			Service.Set<BaseLayoutToolController>(this);
			this.ShouldRevertMap = false;
			this.IsQuickStashModeEnabled = false;
			this.IsSavingBaseLayout = false;
		}

		public void EnterBaseLayoutTool()
		{
			this.IsBaseLayoutModeActive = true;
			this.IsQuickStashModeEnabled = false;
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingQuickStashed, EventPriority.AfterDefault);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingViewReady);
			Service.Get<EventManager>().RegisterObserver(this, EventId.UserLoweredBuilding, EventPriority.AfterDefault);
		}

		public void ExitBaseLayoutTool()
		{
			this.IsBaseLayoutModeActive = false;
			Service.Get<EventManager>().UnregisterObserver(this, EventId.BuildingQuickStashed);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.BuildingViewReady);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.UserLoweredBuilding);
			Service.Get<BuildingController>().DisableUnstashStampingState();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.BuildingViewReady)
			{
				if (id != EventId.BuildingQuickStashed)
				{
					if (id == EventId.UserLoweredBuilding)
					{
						Entity entity = (Entity)cookie;
						if (entity != null)
						{
							Building buildingTO = entity.Get<BuildingComponent>().BuildingTO;
							Position position = this.lastSavedMap.GetPosition(buildingTO.Key);
							if ((position != null && this.HasBuildingMoved(buildingTO, position)) || (Service.Get<GameStateMachine>().CurrentState is WarBaseEditorState && position == null))
							{
								this.ShouldRevertMap = true;
							}
						}
					}
				}
				else if (this.IsQuickStashModeEnabled)
				{
					Entity entity2 = (Entity)cookie;
					this.StashBuilding(entity2);
					string uid = entity2.Get<BuildingComponent>().BuildingTO.Uid;
					Service.Get<BuildingController>().EnsureDeselectSelectedBuilding();
					Service.Get<UXController>().HUD.BaseLayoutToolView.RefreshStashedBuildingCount(uid);
				}
			}
			else
			{
				EntityViewParams entityViewParams = (EntityViewParams)cookie;
				if (this.IsBuildingStashed(entityViewParams.Entity))
				{
					GameObjectViewComponent gameObjectViewComp = entityViewParams.Entity.GameObjectViewComp;
					TransformComponent transformComp = entityViewParams.Entity.TransformComp;
					gameObjectViewComp.SetXYZ(Units.BoardToWorldX(transformComp.CenterX()), -1000f, Units.BoardToWorldZ(transformComp.CenterZ()));
				}
			}
			return EatResponse.NotEaten;
		}

		public int GetBuildingLastSavedX(string buildingKey)
		{
			return this.lastSavedMap.GetPosition(buildingKey).X;
		}

		public int GetBuildingLastSavedZ(string buildingKey)
		{
			return this.lastSavedMap.GetPosition(buildingKey).Z;
		}

		public bool IsBuildingStashed(Entity buildingEntity)
		{
			GameObjectViewComponent gameObjectViewComponent = buildingEntity.Get<GameObjectViewComponent>();
			if (gameObjectViewComponent != null && gameObjectViewComponent.MainTransform.position.y < 0f)
			{
				return true;
			}
			if (this.stashedBuildingMap != null)
			{
				if (!buildingEntity.Has<BuildingComponent>())
				{
					return false;
				}
				string uid = buildingEntity.Get<BuildingComponent>().BuildingType.Uid;
				if (this.stashedBuildingMap.ContainsKey(uid) && this.stashedBuildingMap[uid].Contains(buildingEntity))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsStashedBuildingListEmpty()
		{
			if (this.stashedBuildingMap == null)
			{
				return true;
			}
			foreach (KeyValuePair<string, List<Entity>> current in this.stashedBuildingMap)
			{
				List<Entity> value = current.get_Value();
				if (value.Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		public bool IsListOutOfGivenBuilding(string buildingUID)
		{
			return this.stashedBuildingMap == null || !this.stashedBuildingMap.ContainsKey(buildingUID) || this.stashedBuildingMap[buildingUID].Count < 1;
		}

		public void PauseContractsOnAllBuildings()
		{
			List<Entity> buildingListByType = Service.Get<BuildingLookupController>().GetBuildingListByType(BuildingType.Any);
			int i = 0;
			int count = buildingListByType.Count;
			while (i < count)
			{
				if (!this.IsBuildingClearable(buildingListByType[i]))
				{
					BuildingComponent buildingComponent = buildingListByType[i].Get<BuildingComponent>();
					Service.Get<ISupportController>().PauseBuilding(buildingComponent.BuildingTO.Key);
				}
				i++;
			}
		}

		public void ResumeContractsOnAllBuildings()
		{
			Service.Get<ISupportController>().UnpauseAllBuildings();
		}

		public void StashAllBuildings()
		{
			Service.Get<BuildingController>().EnsureLoweredLiftedBuilding();
			Service.Get<BuildingController>().EnsureDeselectSelectedBuilding();
			List<Entity> buildingListByType = Service.Get<BuildingLookupController>().GetBuildingListByType(BuildingType.Any);
			int i = 0;
			int count = buildingListByType.Count;
			while (i < count)
			{
				Entity buildingEntity = buildingListByType[i];
				if (!this.IsBuildingClearable(buildingEntity) && !this.IsBuildingStashed(buildingEntity))
				{
					this.StashBuilding(buildingEntity);
				}
				i++;
			}
			this.ShouldRevertMap = true;
		}

		private void StashAllMovedBuildings()
		{
			List<Entity> buildingListByType = Service.Get<BuildingLookupController>().GetBuildingListByType(BuildingType.Any);
			int i = 0;
			int count = buildingListByType.Count;
			while (i < count)
			{
				Entity entity = buildingListByType[i];
				BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
				Building buildingTO = buildingComponent.BuildingTO;
				if (buildingComponent.BuildingType.Type != BuildingType.Clearable && !this.IsBuildingStashed(entity))
				{
					string key = buildingTO.Key;
					Position position = this.lastSavedMap.GetPosition(key);
					if (position == null)
					{
						Service.Get<StaRTSLogger>().Error("BLT: Old Building position for " + key + " not found!");
					}
					else if (this.HasBuildingMoved(buildingTO, position))
					{
						this.StashBuilding(entity);
					}
				}
				i++;
			}
		}

		public void UpdateLastSavedMap()
		{
			List<Building> buildings = this.GetCurrentPlayerMap().Buildings;
			if (this.lastSavedMap == null)
			{
				this.lastSavedMap = new PositionMap();
			}
			else
			{
				this.lastSavedMap.ClearAllPositions();
			}
			int i = 0;
			int count = buildings.Count;
			while (i < count)
			{
				Position position = new Position();
				position.X = buildings[i].X;
				position.Z = buildings[i].Z;
				this.lastSavedMap.AddPosition(buildings[i].Key, position);
				i++;
			}
			this.ShouldRevertMap = false;
		}

		private void SaveBaseLayout(PositionMap diffMap)
		{
			if (Service.Get<GameStateMachine>().CurrentState is WarBaseEditorState)
			{
				Service.Get<WarBaseEditController>().SaveWarBaseMap(diffMap);
				return;
			}
			BuildingMultiMoveCommand command = new BuildingMultiMoveCommand(new BuildingMultiMoveRequest
			{
				PositionMap = diffMap
			});
			Service.Get<ServerAPI>().Enqueue(command);
		}

		public Map GetCurrentPlayerMap()
		{
			WarBaseEditController warBaseEditController = Service.Get<WarBaseEditController>();
			if (warBaseEditController.mapData != null && Service.Get<GameStateMachine>().CurrentState is WarBaseEditorState)
			{
				return warBaseEditController.mapData;
			}
			return Service.Get<CurrentPlayer>().Map;
		}

		public void SaveMap()
		{
			if (!this.IsStashedBuildingListEmpty())
			{
				Service.Get<StaRTSLogger>().Warn("BLT: We can't save the map as there are still stashed buildings");
				return;
			}
			PositionMap positionMap = new PositionMap();
			this.IsSavingBaseLayout = true;
			bool flag = false;
			List<Building> buildings = this.GetCurrentPlayerMap().Buildings;
			int i = 0;
			int count = buildings.Count;
			while (i < count)
			{
				string key = buildings[i].Key;
				Position position = this.lastSavedMap.GetPosition(key);
				if (!(Service.Get<GameStateMachine>().CurrentState is WarBaseEditorState) && position == null)
				{
					Service.Get<StaRTSLogger>().Error("BLT: Old Building position for " + key + " not found!");
				}
				else if (position == null || this.HasBuildingMoved(buildings[i], position))
				{
					positionMap.AddPosition(key, new Position
					{
						X = buildings[i].X,
						Z = buildings[i].Z
					});
					flag = true;
				}
				i++;
			}
			if (flag)
			{
				this.SaveBaseLayout(positionMap);
				this.UpdateLastSavedMap();
				this.ClearStashedBuildings();
			}
			this.ShouldRevertMap = false;
			this.IsSavingBaseLayout = false;
		}

		public void RevertToPreviousMapLayout()
		{
			if (this.ShouldRevertMap)
			{
				this.StashAllMovedBuildings();
			}
			if (this.stashedBuildingMap == null)
			{
				return;
			}
			foreach (KeyValuePair<string, List<Entity>> current in this.stashedBuildingMap)
			{
				List<Entity> value = current.get_Value();
				while (value.Count > 0)
				{
					this.UnstashBuildingByUID(current.get_Key(), true, false, false, false);
				}
			}
			Service.Get<EventManager>().SendEvent(EventId.UserLoweredBuildingAudio, null);
			this.ClearStashedBuildings();
		}

		public void ClearStashedBuildings()
		{
			if (this.stashedBuildingMap != null)
			{
				this.stashedBuildingMap.Clear();
				this.stashedBuildingMap = null;
			}
		}

		public void StashBuilding(Entity buildingEntity)
		{
			this.StashBuilding(buildingEntity, true);
		}

		public void StashBuilding(Entity buildingEntity, bool allowRevert)
		{
			BuildingComponent buildingComponent = buildingEntity.Get<BuildingComponent>();
			if (buildingComponent.BuildingType.Type == BuildingType.Clearable)
			{
				Service.Get<StaRTSLogger>().Warn("BLT: Can't stash clearable: " + buildingComponent.BuildingTO.Key + ":" + buildingComponent.BuildingTO.Uid);
				return;
			}
			string uid = buildingComponent.BuildingTO.Uid;
			if (this.stashedBuildingMap == null)
			{
				this.stashedBuildingMap = new Dictionary<string, List<Entity>>();
			}
			if (!this.stashedBuildingMap.ContainsKey(uid))
			{
				this.stashedBuildingMap.Add(uid, new List<Entity>());
			}
			List<Entity> list = this.stashedBuildingMap[uid];
			if (!list.Contains(buildingEntity))
			{
				list.Add(buildingEntity);
			}
			GameObjectViewComponent gameObjectViewComponent = buildingEntity.Get<GameObjectViewComponent>();
			if (gameObjectViewComponent != null)
			{
				TransformComponent transformComponent = buildingEntity.Get<TransformComponent>();
				gameObjectViewComponent.SetXYZ(Units.BoardToWorldX(transformComponent.CenterX()), -1000f, Units.BoardToWorldZ(transformComponent.CenterZ()));
			}
			if (buildingEntity.Has<HealthViewComponent>())
			{
				HealthViewComponent healthViewComponent = buildingEntity.Get<HealthViewComponent>();
				healthViewComponent.TeardownElements();
			}
			Service.Get<EventManager>().SendEvent(EventId.UserStashedBuilding, buildingEntity);
			Service.Get<BoardController>().RemoveEntity(buildingEntity, true);
			Service.Get<EventManager>().SendEvent(EventId.EntityDestroyed, buildingEntity.ID);
			Service.Get<EventManager>().SendEvent(EventId.BuildingMovedOnBoard, buildingEntity);
			Service.Get<BuildingController>().DisableUnstashStampingState();
			if (allowRevert)
			{
				this.ShouldRevertMap = true;
			}
		}

		public void UnstashBuildingByUID(string buildingUID, bool returnToOriginalPosition, bool stampable, bool panToBuilding, bool playLoweredSound)
		{
			if (this.stashedBuildingMap == null)
			{
				return;
			}
			if (!this.stashedBuildingMap.ContainsKey(buildingUID) || this.stashedBuildingMap[buildingUID].Count < 1)
			{
				Service.Get<StaRTSLogger>().Error("Can't unstash! No buildings of : " + buildingUID + " currently stashed");
				return;
			}
			List<Entity> list = this.stashedBuildingMap[buildingUID];
			Entity entity = list[0];
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			bool flag = false;
			if (stampable && this.IsBuildingStampable(entity))
			{
				flag = true;
			}
			Position pos = null;
			if (returnToOriginalPosition)
			{
				pos = this.lastSavedMap.GetPosition(buildingComponent.BuildingTO.Key);
				if (flag)
				{
					flag = false;
					Service.Get<StaRTSLogger>().Warn("No stamping while reverting!!");
				}
			}
			BuildingController buildingController = Service.Get<BuildingController>();
			GameObjectViewComponent gameObjectViewComponent = entity.Get<GameObjectViewComponent>();
			TransformComponent transformComponent = entity.Get<TransformComponent>();
			if (gameObjectViewComponent != null)
			{
				gameObjectViewComponent.SetXYZ(Units.BoardToWorldX(transformComponent.CenterX()), 0f, Units.BoardToWorldZ(transformComponent.CenterZ()));
			}
			if (entity.Has<HealthViewComponent>())
			{
				HealthViewComponent healthViewComponent = entity.Get<HealthViewComponent>();
				healthViewComponent.SetupElements();
			}
			List<Entity> list2 = this.stashedBuildingMap[buildingUID];
			if (list2.Contains(entity))
			{
				list2.Remove(entity);
				if (!buildingController.PositionUnstashedBuilding(entity, pos, flag, panToBuilding, playLoweredSound))
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Unable to place building from stash.  Building {0} {1}", new object[]
					{
						entity.Get<BuildingComponent>().BuildingTO.Key,
						entity.Get<BuildingComponent>().BuildingType.Uid
					});
					Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(Service.Get<Lang>().Get("NO_VALID_POSITION_FOR_UNSTASH", new object[0]));
					this.StashBuilding(entity);
				}
			}
		}

		public void StampUnstashBuildingByUID(string buildingUID)
		{
			BuildingController buildingController = Service.Get<BuildingController>();
			Entity selectedBuilding = buildingController.SelectedBuilding;
			BoardCell<Entity> currentCell = selectedBuilding.Get<BoardItemComponent>().BoardItem.CurrentCell;
			buildingController.SaveLastStampLocation(currentCell.X, currentCell.Z);
			if (this.IsListOutOfGivenBuilding(buildingUID))
			{
				buildingController.DisableUnstashStampingState();
				return;
			}
			this.UnstashBuildingByUID(buildingUID, false, true, true, true);
		}

		private bool HasBuildingMoved(Building building, Position oldPosition)
		{
			return oldPosition.X != building.X || oldPosition.Z != building.Z;
		}

		public bool IsBuildingClearable(Entity buildingEntity)
		{
			BuildingComponent buildingComponent = buildingEntity.Get<BuildingComponent>();
			return buildingComponent.BuildingType.Type == BuildingType.Clearable;
		}

		public bool IsBuildingStampable(Entity buildingEntity)
		{
			return buildingEntity.Get<BuildingComponent>().BuildingType.Type == BuildingType.Wall;
		}

		public bool IsActive()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			return currentState is BaseLayoutToolState || currentState is WarBaseEditorState;
		}

		public bool ShouldChecksumLastSaveData()
		{
			bool flag = this.IsBaseLayoutModeActive && !this.IsSavingBaseLayout;
			return flag && !(Service.Get<GameStateMachine>().CurrentState is WarBaseEditorState);
		}

		protected internal BaseLayoutToolController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).ClearStashedBuildings();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).EnterBaseLayoutTool();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).ExitBaseLayoutTool();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsBaseLayoutModeActive);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsQuickStashModeEnabled);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsSavingBaseLayout);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).ShouldRevertMap);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).GetBuildingLastSavedX(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).GetBuildingLastSavedZ(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentPlayerMap());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).HasBuildingMoved((Building)GCHandledObjects.GCHandleToObject(*args), (Position)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsActive());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsBuildingClearable((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsBuildingStampable((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsBuildingStashed((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsListOutOfGivenBuilding(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsStashedBuildingListEmpty());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).PauseContractsOnAllBuildings();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).ResumeContractsOnAllBuildings();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).RevertToPreviousMapLayout();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).SaveBaseLayout((PositionMap)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).SaveMap();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsBaseLayoutModeActive = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsQuickStashModeEnabled = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).IsSavingBaseLayout = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).ShouldRevertMap = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).ShouldChecksumLastSaveData());
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).StampUnstashBuildingByUID(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).StashAllBuildings();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).StashAllMovedBuildings();
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).StashBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).StashBuilding((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).UnstashBuildingByUID(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0, *(sbyte*)(args + 4) != 0);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((BaseLayoutToolController)GCHandledObjects.GCHandleToObject(instance)).UpdateLastSavedMap();
			return -1L;
		}
	}
}
