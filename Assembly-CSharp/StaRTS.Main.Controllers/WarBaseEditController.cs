using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player.Building.Move;
using StaRTS.Main.Models.Commands.TransferObjects;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class WarBaseEditController
	{
		public Map mapData;

		public WarBaseEditController()
		{
			Service.Set<WarBaseEditController>(this);
		}

		public void EnterWarBaseEditing(Map warBaseMap)
		{
			this.mapData = warBaseMap;
			BaseLayoutToolController baseLayoutToolController = Service.Get<BaseLayoutToolController>();
			baseLayoutToolController.EnterBaseLayoutTool();
		}

		public void ExitWarBaseEditing()
		{
			this.mapData = null;
			BaseLayoutToolController baseLayoutToolController = Service.Get<BaseLayoutToolController>();
			baseLayoutToolController.ExitBaseLayoutTool();
		}

		public void CheckForNewBuildings()
		{
			if (this.mapData == null)
			{
				Service.Get<StaRTSLogger>().Warn("No war base data found, not adding new buildings");
				return;
			}
			List<Building> buildings = Service.Get<CurrentPlayer>().Map.Buildings;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (this.mapData.Buildings != null)
			{
				int i = 0;
				int count = this.mapData.Buildings.Count;
				while (i < count)
				{
					dictionary[this.mapData.Buildings[i].Key] = this.mapData.Buildings[i].Uid;
					i++;
				}
			}
			bool flag = false;
			int j = 0;
			int count2 = buildings.Count;
			while (j < count2)
			{
				string key = buildings[j].Key;
				if (!dictionary.ContainsKey(key))
				{
					BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(buildings[j].Uid);
					if (buildingTypeVO.Type != BuildingType.Clearable && !ContractUtils.IsBuildingConstructing(key))
					{
						Building building = buildings[j].Clone();
						this.mapData.Buildings.Add(building);
						Entity entity = Service.Get<EntityFactory>().CreateBuildingEntity(building, false, true, false);
						Service.Get<WorldController>().AddEntityToWorld(entity);
						Service.Get<BaseLayoutToolController>().StashBuilding(entity, false);
						flag = true;
					}
				}
				j++;
			}
			if (flag)
			{
				Service.Get<UXController>().HUD.BaseLayoutToolView.RefreshWholeStashTray();
			}
		}

		public void SaveWarBaseMap(PositionMap diffMap)
		{
			WarBaseSaveCommand command = new WarBaseSaveCommand(new WarBaseSaveRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId,
				PositionMap = diffMap
			});
			Service.Get<ServerAPI>().Enqueue(command);
		}

		protected internal WarBaseEditController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((WarBaseEditController)GCHandledObjects.GCHandleToObject(instance)).CheckForNewBuildings();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WarBaseEditController)GCHandledObjects.GCHandleToObject(instance)).EnterWarBaseEditing((Map)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WarBaseEditController)GCHandledObjects.GCHandleToObject(instance)).ExitWarBaseEditing();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WarBaseEditController)GCHandledObjects.GCHandleToObject(instance)).SaveWarBaseMap((PositionMap)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
