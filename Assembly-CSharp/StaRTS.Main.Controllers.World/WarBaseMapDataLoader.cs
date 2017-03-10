using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class WarBaseMapDataLoader : IMapDataLoader
	{
		private BattleInitializationData battleData;

		private SquadMemberWarData memberWarData;

		private string squadId;

		private FactionType faction;

		private Dictionary<string, int> defenderSquadTroops;

		private Dictionary<string, int> defenderChampions;

		private const WorldType worldType = WorldType.Opponent;

		public WarBaseMapDataLoader()
		{
			Service.Set<WarBaseMapDataLoader>(this);
		}

		public WarBaseMapDataLoader Initialize(BattleInitializationData battleData)
		{
			this.battleData = battleData;
			this.memberWarData = battleData.MemberWarData;
			this.squadId = battleData.Defender.GuildId;
			this.faction = battleData.Defender.PlayerFaction;
			this.defenderSquadTroops = battleData.DefenderGuildTroopsAvailable;
			this.defenderChampions = battleData.DefenderChampionsAvailable;
			return this;
		}

		public void LoadMapData(MapLoadedDelegate onMapLoaded, MapLoadFailDelegate onMapLoadFail)
		{
			onMapLoaded(this.memberWarData.BaseMap);
		}

		public List<IAssetVO> GetPreloads()
		{
			return MapDataLoaderUtils.GetBattlePreloads(this.battleData);
		}

		public List<IAssetVO> GetProjectilePreloads(Map map)
		{
			List<string> attackerWarBuffs = null;
			List<string> defenderWarBuffs = null;
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad != null)
			{
				SquadWarManager warManager = Service.Get<SquadController>().WarManager;
				attackerWarBuffs = warManager.GetBuffBasesOwnedBySquad(currentSquad.SquadID);
				defenderWarBuffs = warManager.GetBuffBasesOwnedBySquad(this.squadId);
			}
			return ProjectileUtils.GetBattleProjectileAssets(map, null, null, attackerWarBuffs, defenderWarBuffs, this.defenderSquadTroops, this.defenderChampions, this.battleData.AttackerEquipment, this.battleData.DefenderEquipment);
		}

		public WorldType GetWorldType()
		{
			return WorldType.Opponent;
		}

		public string GetWorldName()
		{
			return this.memberWarData.SquadMemberName;
		}

		public string GetFactionAssetName()
		{
			return UXUtils.GetIconNameFromFactionType(this.faction);
		}

		public PlanetVO GetPlanetData()
		{
			return this.memberWarData.BaseMap.Planet;
		}

		protected internal WarBaseMapDataLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetFactionAssetName());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPlanetData());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPreloads());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetProjectilePreloads((Map)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldType());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).Initialize((BattleInitializationData)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((WarBaseMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).LoadMapData((MapLoadedDelegate)GCHandledObjects.GCHandleToObject(*args), (MapLoadFailDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
