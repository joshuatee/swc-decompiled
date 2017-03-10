using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Commands.Pvp;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class PvpMapDataLoader : IMapDataLoader
	{
		private BattleInitializationData battleData;

		private PvpTarget pvpTarget;

		private const WorldType worldType = WorldType.Opponent;

		public PvpMapDataLoader()
		{
			Service.Set<PvpMapDataLoader>(this);
		}

		public PvpMapDataLoader Initialize(BattleInitializationData battleData)
		{
			this.battleData = battleData;
			this.pvpTarget = battleData.PvpTarget;
			return this;
		}

		public void LoadMapData(MapLoadedDelegate onMapLoaded, MapLoadFailDelegate onMapLoadFail)
		{
			onMapLoaded(this.pvpTarget.BaseMap);
		}

		public List<IAssetVO> GetPreloads()
		{
			return MapDataLoaderUtils.GetBattlePreloads(this.battleData);
		}

		public List<IAssetVO> GetProjectilePreloads(Map map)
		{
			return ProjectileUtils.GetBattleProjectileAssets(map, null, null, null, null, this.pvpTarget.GuildDonatedTroops, this.pvpTarget.Champions, this.battleData.AttackerEquipment, this.pvpTarget.Equipment);
		}

		public WorldType GetWorldType()
		{
			return WorldType.Opponent;
		}

		public string GetWorldName()
		{
			if (!string.IsNullOrEmpty(this.pvpTarget.PlayerName))
			{
				return this.pvpTarget.PlayerName;
			}
			string playerId = this.pvpTarget.PlayerId;
			int num = playerId.get_Length();
			if (num > 10)
			{
				num = 10;
			}
			return playerId.Substring(0, num);
		}

		public string GetFactionAssetName()
		{
			return UXUtils.GetIconNameFromFactionType(this.pvpTarget.PlayerFaction);
		}

		public PlanetVO GetPlanetData()
		{
			return this.pvpTarget.BaseMap.Planet;
		}

		protected internal PvpMapDataLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetFactionAssetName());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPlanetData());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPreloads());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetProjectilePreloads((Map)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldType());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).Initialize((BattleInitializationData)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PvpMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).LoadMapData((MapLoadedDelegate)GCHandledObjects.GCHandleToObject(*args), (MapLoadFailDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
