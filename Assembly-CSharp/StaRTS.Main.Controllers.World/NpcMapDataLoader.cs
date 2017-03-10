using StaRTS.Assets;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Cee.Serializables;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class NpcMapDataLoader : IMapDataLoader
	{
		private BattleInitializationData battleData;

		private BattleTypeVO battle;

		private MapLoadedDelegate onMapLoaded;

		private MapLoadFailDelegate onMapLoadFail;

		private AssetHandle assetHandle;

		private const WorldType worldType = WorldType.NPC;

		private bool isPveBuffBase;

		public NpcMapDataLoader()
		{
			Service.Set<NpcMapDataLoader>(this);
		}

		public NpcMapDataLoader Initialize(BattleTypeVO battle)
		{
			this.battle = battle;
			return this;
		}

		public NpcMapDataLoader Initialize(BattleInitializationData battleData, bool isPveBuffBase)
		{
			this.battleData = battleData;
			this.battle = battleData.BattleVO;
			this.isPveBuffBase = isPveBuffBase;
			return this;
		}

		public void LoadMapData(MapLoadedDelegate onMapLoaded, MapLoadFailDelegate onMapLoadFail)
		{
			this.onMapLoaded = onMapLoaded;
			this.onMapLoadFail = onMapLoadFail;
			Service.Get<AssetManager>().Load(ref this.assetHandle, this.battle.AssetName, new AssetSuccessDelegate(this.OnBattleFileLoaded), new AssetFailureDelegate(this.OnBattleFileLoadFailed), null);
		}

		private void UnloadAsset()
		{
			if (this.assetHandle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.assetHandle);
				this.assetHandle = AssetHandle.Invalid;
			}
		}

		public List<IAssetVO> GetPreloads()
		{
			return MapDataLoaderUtils.GetBattlePreloads(this.battleData);
		}

		public List<IAssetVO> GetProjectilePreloads(Map map)
		{
			List<string> attackerWarBuffs = null;
			List<string> defenderWarBuffs = null;
			if (this.isPveBuffBase)
			{
				Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
				if (currentSquad != null)
				{
					SquadWarManager warManager = Service.Get<SquadController>().WarManager;
					attackerWarBuffs = warManager.GetBuffBasesOwnedBySquad(currentSquad.SquadID);
				}
			}
			List<string> attackerEquipment = null;
			List<string> defenderEquipment = null;
			if (this.battleData != null)
			{
				attackerEquipment = this.battleData.AttackerEquipment;
				defenderEquipment = this.battleData.DefenderEquipment;
			}
			return ProjectileUtils.GetBattleProjectileAssets(map, this.battle, null, attackerWarBuffs, defenderWarBuffs, null, null, attackerEquipment, defenderEquipment);
		}

		public WorldType GetWorldType()
		{
			return WorldType.NPC;
		}

		public string GetWorldName()
		{
			if (this.isPveBuffBase)
			{
				return LangUtils.GetBattleOnPlanetName(this.battle);
			}
			return LangUtils.GetBattleName(this.battle);
		}

		private void OnBattleFileLoaded(object asset, object cookie)
		{
			object obj = new JsonParser(asset as string).Parse();
			CombatEncounter combatEncounter = new CombatEncounter().FromObject(obj) as CombatEncounter;
			this.UnloadAsset();
			if (!string.IsNullOrEmpty(this.battle.Planet))
			{
				combatEncounter.map.Planet = Service.Get<IDataController>().Get<PlanetVO>(this.battle.Planet);
			}
			if (this.onMapLoaded != null)
			{
				this.onMapLoaded(combatEncounter.map);
			}
		}

		private void OnBattleFileLoadFailed(object cookie)
		{
			this.UnloadAsset();
			Service.Get<StaRTSLogger>().ErrorFormat("Failed to load map data {0} for battle {1}", new object[]
			{
				this.battle.AssetName,
				this.battle.Uid
			});
			this.onMapLoadFail();
		}

		public string GetFactionAssetName()
		{
			return this.battle.DefenderId;
		}

		public PlanetVO GetPlanetData()
		{
			return Service.Get<IDataController>().Get<PlanetVO>(this.battle.Planet);
		}

		protected internal NpcMapDataLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetFactionAssetName());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPlanetData());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPreloads());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetProjectilePreloads((Map)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldType());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).Initialize((BattleTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).Initialize((BattleInitializationData)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).LoadMapData((MapLoadedDelegate)GCHandledObjects.GCHandleToObject(*args), (MapLoadFailDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).OnBattleFileLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).OnBattleFileLoadFailed(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((NpcMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).UnloadAsset();
			return -1L;
		}
	}
}
