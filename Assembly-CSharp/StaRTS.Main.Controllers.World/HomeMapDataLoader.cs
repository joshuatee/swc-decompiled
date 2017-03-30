using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers.World
{
	public class HomeMapDataLoader : IMapDataLoader
	{
		private const WorldType worldType = WorldType.Home;

		private MapLoadedDelegate onMapLoaded;

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
			return string.Empty;
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
	}
}
