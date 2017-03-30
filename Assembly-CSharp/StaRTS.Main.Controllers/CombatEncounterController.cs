using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Cee.Serializables;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.IO;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers
{
	public class CombatEncounterController
	{
		public CombatEncounterController()
		{
			Service.Set<CombatEncounterController>(this);
		}

		public void Load(string fileName)
		{
			string json = FileUtils.Read(FileUtils.GetAbsFilePathInMyDocuments(fileName, "/src/maps"));
			object obj = new JsonParser(json).Parse();
			CombatEncounter combatEncounter = new CombatEncounter().FromObject(obj) as CombatEncounter;
			Service.Get<CurrentPlayer>().Map = combatEncounter.map;
			Service.Get<WorldTransitioner>().SetSkipTransitions(true);
			Service.Get<WorldInitializer>().PrepareWorld(combatEncounter.map);
			Service.Get<EditBaseController>().Enable(false);
			Service.Get<EditBaseController>().Enable(true);
		}

		public void LoadMeta(BattleTypeVO battle)
		{
			string json = FileUtils.Read(FileUtils.GetAbsFilePathInMyDocuments(battle.AssetName + ".json", "/src/starts-game-assets/develop/battles"));
			object obj = new JsonParser(json).Parse();
			CombatEncounter combatEncounter = new CombatEncounter().FromObject(obj) as CombatEncounter;
			Service.Get<CurrentPlayer>().Map = combatEncounter.map;
			Service.Get<WorldTransitioner>().SetSkipTransitions(true);
			Service.Get<WorldInitializer>().PrepareWorld(combatEncounter.map);
			Service.Get<EditBaseController>().Enable(true);
		}

		public void Save(string fileName)
		{
			IDataController dataController = Service.Get<IDataController>();
			CombatEncounter currentCombatEncounter = this.GetCurrentCombatEncounter();
			List<Building> buildings = currentCombatEncounter.map.Buildings;
			int i = 0;
			int count = buildings.Count;
			while (i < count)
			{
				buildings[i].Key = "bld_" + (i + 1);
				if (dataController.Get<BuildingTypeVO>(buildings[i].Uid).Type == BuildingType.Trap)
				{
					buildings[i].CurrentStorage = 1;
				}
				i++;
			}
			string text = this.Serialize(currentCombatEncounter);
			FileUtils.Write(FileUtils.GetAbsFilePathInMyDocuments(fileName, "/src/maps"), text);
			Service.Get<Logger>().Debug("Json Saved: " + text);
		}

		public void Verify(string fileName)
		{
			Logger logger = Service.Get<Logger>();
			logger.Debug("Using data path: " + fileName);
			string text = FileUtils.Read(FileUtils.GetAbsFilePathInMyDocuments(fileName, "/src/maps"));
			logger.Debug("Read json: " + text);
			CombatEncounter data = this.Deserialize(text);
			logger.Debug("De-serialized json to model...");
			string text2 = this.Serialize(data);
			logger.Debug("Serialized model to json...");
			FileUtils.Write(FileUtils.GetAbsFilePathInMyDocuments(fileName, "/src/maps"), text2);
			logger.Debug("Saved json: " + text2);
			text = text.Replace(" ", string.Empty).Trim();
			text2 = text2.Replace(" ", string.Empty).Trim();
			if (text.Equals(text2))
			{
				logger.Debug("Verification passed");
				return;
			}
			logger.Debug(text);
			logger.Debug(text2);
			logger.Debug("Verification failed");
		}

		public string Serialize(CombatEncounter data)
		{
			return data.ToJson();
		}

		public CombatEncounter Deserialize(string json)
		{
			object obj = new JsonParser(json).Parse();
			return new CombatEncounter().FromObject(obj) as CombatEncounter;
		}

		public CombatEncounter GetCurrentCombatEncounter()
		{
			CombatEncounter combatEncounter = new CombatEncounter();
			combatEncounter.map = new Map();
			combatEncounter.map.Buildings = new List<Building>();
			BoardController boardController = Service.Get<BoardController>();
			Board<Entity> board = boardController.Board;
			LinkedList<BoardItem<Entity>> children = board.Children;
			if (children != null)
			{
				foreach (BoardItem<Entity> current in children)
				{
					BoardCell<Entity> currentCell = current.CurrentCell;
					Entity data = current.Data;
					BuildingComponent buildingComponent = data.Get<BuildingComponent>();
					if (buildingComponent != null)
					{
						Building building = new Building();
						building.Key = buildingComponent.BuildingTO.Key;
						building.Uid = buildingComponent.BuildingType.Uid;
						building.X = Units.BoardToGridX(currentCell.X);
						building.Z = Units.BoardToGridZ(currentCell.Z);
						building.CurrentStorage = buildingComponent.BuildingTO.CurrentStorage;
						combatEncounter.map.Buildings.Add(building);
					}
				}
			}
			combatEncounter.map.Planet = Service.Get<CurrentPlayer>().Map.Planet;
			return combatEncounter;
		}
	}
}
