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
using System.Runtime.InteropServices;
using WinRTBridge;

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
			string json = FileUtils.Read(FileUtils.GetAbsFilePathInMyDocuments(battle.AssetName + ".json", "/src/starts-game-assets/trunk/battles"));
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
			Service.Get<StaRTSLogger>().Debug("Json Saved: " + text);
		}

		public void Verify(string fileName)
		{
			StaRTSLogger staRTSLogger = Service.Get<StaRTSLogger>();
			staRTSLogger.Debug("Using data path: " + fileName);
			string text = FileUtils.Read(FileUtils.GetAbsFilePathInMyDocuments(fileName, "/src/maps"));
			staRTSLogger.Debug("Read json: " + text);
			CombatEncounter data = this.Deserialize(text);
			staRTSLogger.Debug("De-serialized json to model...");
			string text2 = this.Serialize(data);
			staRTSLogger.Debug("Serialized model to json...");
			FileUtils.Write(FileUtils.GetAbsFilePathInMyDocuments(fileName, "/src/maps"), text2);
			staRTSLogger.Debug("Saved json: " + text2);
			text = text.Replace(" ", string.Empty).Trim();
			text2 = text2.Replace(" ", string.Empty).Trim();
			if (text.Equals(text2))
			{
				staRTSLogger.Debug("Verification passed");
				return;
			}
			staRTSLogger.Debug(text);
			staRTSLogger.Debug(text2);
			staRTSLogger.Debug("Verification failed");
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

		protected internal CombatEncounterController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombatEncounterController)GCHandledObjects.GCHandleToObject(instance)).Deserialize(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombatEncounterController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentCombatEncounter());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CombatEncounterController)GCHandledObjects.GCHandleToObject(instance)).Load(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CombatEncounterController)GCHandledObjects.GCHandleToObject(instance)).LoadMeta((BattleTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((CombatEncounterController)GCHandledObjects.GCHandleToObject(instance)).Save(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombatEncounterController)GCHandledObjects.GCHandleToObject(instance)).Serialize((CombatEncounter)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((CombatEncounterController)GCHandledObjects.GCHandleToObject(instance)).Verify(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
