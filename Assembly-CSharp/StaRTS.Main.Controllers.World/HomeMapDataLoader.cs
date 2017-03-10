using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class HomeMapDataLoader : IMapDataLoader
	{
		private MapLoadedDelegate onMapLoaded;

		private const WorldType worldType = WorldType.Home;

		public HomeMapDataLoader()
		{
			Service.Set<HomeMapDataLoader>(this);
		}

		public void LoadMapData(MapLoadedDelegate onMapLoaded, MapLoadFailDelegate onMapLoadFail)
		{
			this.DoOfflineSimulationForGenerators();
			onMapLoaded(Service.Get<CurrentPlayer>().Map);
		}

		private void DoOfflineSimulationForGenerators()
		{
			Map map = Service.Get<CurrentPlayer>().Map;
			IDataController dataController = Service.Get<IDataController>();
			ICurrencyController currencyController = Service.Get<ICurrencyController>();
			ISupportController supportController = Service.Get<ISupportController>();
			foreach (Building current in map.Buildings)
			{
				BuildingTypeVO buildingTypeVO = dataController.Get<BuildingTypeVO>(current.Uid);
				if (buildingTypeVO.Type == BuildingType.Resource && supportController.FindCurrentContract(current.Key) == null)
				{
					current.AccruedCurrency = currencyController.CalculateAccruedCurrency(current, buildingTypeVO);
				}
			}
		}

		public List<IAssetVO> GetPreloads()
		{
			List<IAssetVO> result = new List<IAssetVO>();
			Service.Get<UXController>().MiscElementsManager.ReleaseHealthSliderPool();
			return result;
		}

		public List<IAssetVO> GetProjectilePreloads(Map map)
		{
			return null;
		}

		public WorldType GetWorldType()
		{
			return WorldType.Home;
		}

		public string GetWorldName()
		{
			return "";
		}

		public string GetFactionAssetName()
		{
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			return UXUtils.GetIconNameFromFactionType(faction);
		}

		public PlanetVO GetPlanetData()
		{
			return Service.Get<CurrentPlayer>().Map.Planet;
		}

		protected internal HomeMapDataLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).DoOfflineSimulationForGenerators();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetFactionAssetName());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPlanetData());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPreloads());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetProjectilePreloads((Map)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldName());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldType());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).LoadMapData((MapLoadedDelegate)GCHandledObjects.GCHandleToObject(*args), (MapLoadFailDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
