using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.World
{
	public class Map : ISerializable
	{
		private const string PLANET_KEY = "planet";

		private const string NEXT_BUILDING_NUMBER_KEY = "next";

		private string planetUid;

		public List<Building> Buildings
		{
			get;
			set;
		}

		public PlanetVO Planet
		{
			get;
			set;
		}

		public int NextBuildingNumber
		{
			get;
			set;
		}

		public ISerializable FromObject(object obj)
		{
			this.Buildings = new List<Building>();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("planet"))
			{
				this.planetUid = (dictionary["planet"] as string);
			}
			if (dictionary.ContainsKey("buildings"))
			{
				List<object> list = dictionary["buildings"] as List<object>;
				foreach (object current in list)
				{
					this.Buildings.Add(new Building().FromObject(current) as Building);
				}
			}
			if (dictionary.ContainsKey("next"))
			{
				this.NextBuildingNumber = Convert.ToInt32(dictionary["next"], CultureInfo.InvariantCulture);
			}
			else
			{
				Service.Get<StaRTSLogger>().Debug("Map does not contain nextBuildingNumber.");
			}
			return this;
		}

		public void GetAllBuildingsWithBaseUid(string baseUId, List<Building> outMatchingBuildings)
		{
			for (int i = 0; i < this.Buildings.Count; i++)
			{
				Building building = this.Buildings[i];
				string text = building.Uid;
				int indexOfFirstNumericCharacter = StringUtils.GetIndexOfFirstNumericCharacter(text);
				if (indexOfFirstNumericCharacter > 0)
				{
					text = text.Substring(0, indexOfFirstNumericCharacter);
				}
				if (text == baseUId)
				{
					outMatchingBuildings.Add(building);
				}
			}
		}

		public Building GetHighestLevelBuilding(string buildingID)
		{
			int num = -1;
			Building result = null;
			int i = 0;
			int count = this.Buildings.Count;
			while (i < count)
			{
				Building building = this.Buildings[i];
				string text = building.Uid;
				int indexOfFirstNumericCharacter = StringUtils.GetIndexOfFirstNumericCharacter(text);
				int num2 = -1;
				if (indexOfFirstNumericCharacter > 0)
				{
					string text2 = text.Substring(indexOfFirstNumericCharacter);
					text = text.Substring(0, indexOfFirstNumericCharacter);
					if (int.TryParse(text2, ref num2) && text == buildingID && num2 > num)
					{
						num = num2;
						result = building;
					}
				}
				i++;
			}
			return result;
		}

		public void ReinitializePlanet(string planet)
		{
			this.planetUid = planet;
			this.InitializePlanet();
		}

		public void InitializePlanet()
		{
			IDataController dataController = Service.Get<IDataController>();
			if (!string.IsNullOrEmpty(this.planetUid))
			{
				this.Planet = dataController.Get<PlanetVO>(this.planetUid);
			}
			if (this.Planet == null)
			{
				this.Planet = dataController.Get<PlanetVO>("planet1");
			}
		}

		public string PlanetId()
		{
			return this.planetUid;
		}

		public string ToJson()
		{
			return Serializer.Start().Add<int>("next", this.NextBuildingNumber).AddString("planet", this.Planet.Uid).AddArray<Building>("buildings", this.Buildings).End().ToString();
		}

		public int FindHighestHqLevel()
		{
			IDataController dataController = Service.Get<IDataController>();
			int num = -1;
			int num2 = num;
			if (dataController == null || this.Buildings == null)
			{
				return num2;
			}
			int i = 0;
			int count = this.Buildings.Count;
			while (i < count)
			{
				BuildingTypeVO optional = dataController.GetOptional<BuildingTypeVO>(this.Buildings[i].Uid);
				if (optional != null && optional.Type == BuildingType.HQ && optional.Lvl >= num2)
				{
					num2 = optional.Lvl;
				}
				i++;
			}
			return num2;
		}

		public bool ScoutTowerExists()
		{
			IDataController dataController = Service.Get<IDataController>();
			int i = 0;
			int count = this.Buildings.Count;
			while (i < count)
			{
				BuildingTypeVO optional = dataController.GetOptional<BuildingTypeVO>(this.Buildings[i].Uid);
				if (optional != null && optional.Type == BuildingType.ScoutTower)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public int GetSquadStorageCapacity()
		{
			IDataController dataController = Service.Get<IDataController>();
			int i = 0;
			int count = this.Buildings.Count;
			while (i < count)
			{
				BuildingTypeVO optional = dataController.GetOptional<BuildingTypeVO>(this.Buildings[i].Uid);
				if (optional != null && optional.Type == BuildingType.Squad)
				{
					return optional.Storage;
				}
				i++;
			}
			return 0;
		}

		public void OnRemoveBuildingFromMap()
		{
			int nextBuildingNumber = this.NextBuildingNumber;
			this.NextBuildingNumber = nextBuildingNumber - 1;
		}

		public int GetNextBuildingNumberAndIncrement()
		{
			int nextBuildingNumber = this.NextBuildingNumber;
			this.NextBuildingNumber = nextBuildingNumber + 1;
			return nextBuildingNumber;
		}

		public Map()
		{
			this.planetUid = "";
			base..ctor();
		}

		protected internal Map(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).FindHighestHqLevel());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).Buildings);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).NextBuildingNumber);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).Planet);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Map)GCHandledObjects.GCHandleToObject(instance)).GetAllBuildingsWithBaseUid(Marshal.PtrToStringUni(*(IntPtr*)args), (List<Building>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).GetHighestLevelBuilding(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).GetNextBuildingNumberAndIncrement());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).GetSquadStorageCapacity());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((Map)GCHandledObjects.GCHandleToObject(instance)).InitializePlanet();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((Map)GCHandledObjects.GCHandleToObject(instance)).OnRemoveBuildingFromMap();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).PlanetId());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((Map)GCHandledObjects.GCHandleToObject(instance)).ReinitializePlanet(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).ScoutTowerExists());
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((Map)GCHandledObjects.GCHandleToObject(instance)).Buildings = (List<Building>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((Map)GCHandledObjects.GCHandleToObject(instance)).NextBuildingNumber = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((Map)GCHandledObjects.GCHandleToObject(instance)).Planet = (PlanetVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Map)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
