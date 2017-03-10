using StaRTS.Main.Models;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class UserWarBaseMapDataLoader : IMapDataLoader
	{
		private Map baseMap;

		private string squadMemberName;

		private FactionType faction;

		private const WorldType worldType = WorldType.WarBase;

		public UserWarBaseMapDataLoader()
		{
		}

		public void Initialize(Map baseMap, string squadMemberName, FactionType faction)
		{
			this.baseMap = baseMap;
			this.squadMemberName = squadMemberName;
			this.faction = faction;
		}

		public void LoadMapData(MapLoadedDelegate onMapLoaded, MapLoadFailDelegate onMapLoadFail)
		{
			onMapLoaded(this.baseMap);
		}

		public List<IAssetVO> GetPreloads()
		{
			return null;
		}

		public List<IAssetVO> GetProjectilePreloads(Map map)
		{
			return null;
		}

		public WorldType GetWorldType()
		{
			return WorldType.WarBase;
		}

		public string GetWorldName()
		{
			return this.squadMemberName;
		}

		public string GetFactionAssetName()
		{
			return UXUtils.GetIconNameFromFactionType(this.faction);
		}

		public PlanetVO GetPlanetData()
		{
			return this.baseMap.Planet;
		}

		protected internal UserWarBaseMapDataLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserWarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetFactionAssetName());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserWarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPlanetData());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserWarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPreloads());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserWarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetProjectilePreloads((Map)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserWarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserWarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldType());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UserWarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).Initialize((Map)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (FactionType)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UserWarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).LoadMapData((MapLoadedDelegate)GCHandledObjects.GCHandleToObject(*args), (MapLoadFailDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
