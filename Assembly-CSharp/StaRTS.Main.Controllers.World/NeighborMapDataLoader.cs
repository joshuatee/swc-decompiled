using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class NeighborMapDataLoader : IMapDataLoader
	{
		private const WorldType worldType = WorldType.Neighbor;

		private VisitNeighborResponse response;

		public NeighborMapDataLoader(VisitNeighborResponse response)
		{
			this.response = response;
		}

		public void LoadMapData(MapLoadedDelegate onMapLoaded, MapLoadFailDelegate onMapLoadFail)
		{
			onMapLoaded(this.response.MapData);
		}

		public WorldType GetWorldType()
		{
			return WorldType.Neighbor;
		}

		public string GetWorldName()
		{
			return this.response.Name;
		}

		public List<IAssetVO> GetPreloads()
		{
			return null;
		}

		public List<IAssetVO> GetProjectilePreloads(Map map)
		{
			return null;
		}

		public string GetFactionAssetName()
		{
			FactionType faction = this.response.Faction;
			return UXUtils.GetIconNameFromFactionType(faction);
		}

		public PlanetVO GetPlanetData()
		{
			return this.response.MapData.Planet;
		}

		protected internal NeighborMapDataLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NeighborMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetFactionAssetName());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NeighborMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPlanetData());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NeighborMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPreloads());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NeighborMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetProjectilePreloads((Map)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NeighborMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NeighborMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldType());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((NeighborMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).LoadMapData((MapLoadedDelegate)GCHandledObjects.GCHandleToObject(*args), (MapLoadFailDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
