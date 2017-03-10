using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class ReplayMapDataLoader : IMapDataLoader
	{
		private MapLoadedDelegate OnMapLoaded;

		private BattleParticipant defender;

		private string replayOwnerPlayerId;

		private GetReplayResponse replayResponseData;

		private const WorldType worldType = WorldType.Replay;

		public ReplayMapDataLoader()
		{
			Service.Set<ReplayMapDataLoader>(this);
		}

		public ReplayMapDataLoader Initialize(BattleParticipant defender, string replayOwnerPlayerId)
		{
			this.defender = defender;
			this.replayOwnerPlayerId = replayOwnerPlayerId;
			this.replayResponseData = null;
			return this;
		}

		public void SetReplayResponseData(GetReplayResponse replayResponseData)
		{
			this.replayResponseData = replayResponseData;
		}

		public ReplayMapDataLoader InitializeFromData(BattleEntry battleEntry, BattleRecord battleRecord)
		{
			this.replayResponseData = new GetReplayResponse();
			this.replayResponseData.EntryData = battleEntry;
			this.replayResponseData.ReplayData = battleRecord;
			this.defender = battleEntry.Defender;
			this.replayOwnerPlayerId = null;
			return this;
		}

		public void LoadMapData(MapLoadedDelegate onMapLoaded, MapLoadFailDelegate onMapLoadFail)
		{
			BattleRecord battleRecord = this.replayResponseData.GetOriginalReplayRecord();
			if (battleRecord == null)
			{
				battleRecord = this.replayResponseData.ReplayData;
			}
			onMapLoaded(battleRecord.CombatEncounter.map);
		}

		public void OnReplayLoaded(GetReplayResponse response, object cookie)
		{
			ProcessingScreen.Hide();
			this.replayResponseData = response;
			BattlePlaybackState.GoToBattlePlaybackState(this);
			BattleRecord replayData = response.ReplayData;
			BattleEntry entryData = response.EntryData;
			replayData.RecordId = entryData.RecordID;
			entryData.SharerPlayerId = this.replayOwnerPlayerId;
			bool flag = Service.Get<CurrentPlayer>().PlayerId == entryData.AttackerID || this.replayOwnerPlayerId == entryData.AttackerID;
			entryData.Won = (flag ? (entryData.EarnedStars > 0) : (entryData.EarnedStars == 0));
			Service.Get<EventManager>().SendEvent(EventId.BattleRecordRetrieved, this.replayResponseData);
		}

		public void OnReplayLoadFailed(uint status, object cookie)
		{
			ProcessingScreen.Hide();
			if (!(Service.Get<GameStateMachine>().CurrentState is HomeState))
			{
				HomeState.GoToHomeState(null, false);
			}
			if (status == 2110u)
			{
				string message = Service.Get<Lang>().Get("REPLAY_DATA_NOT_FOUND", new object[0]);
				AlertScreen.ShowModal(false, null, message, null, null);
			}
		}

		public List<IAssetVO> GetPreloads()
		{
			BattleRecord replayData = this.replayResponseData.ReplayData;
			return MapDataLoaderUtils.GetBattleRecordPreloads(replayData);
		}

		public List<IAssetVO> GetProjectilePreloads(Map map)
		{
			BattleRecord replayData = this.replayResponseData.ReplayData;
			return ProjectileUtils.GetBattleRecordProjectileAssets(map, replayData, replayData.AttackerWarBuffs, replayData.DefenderWarBuffs, replayData.AttackerEquipment, replayData.DefenderEquipment);
		}

		public WorldType GetWorldType()
		{
			return WorldType.Replay;
		}

		public string GetWorldName()
		{
			return this.defender.PlayerName;
		}

		public string GetFactionAssetName()
		{
			return UXUtils.GetIconNameFromFactionType(this.defender.PlayerFaction);
		}

		public PlanetVO GetPlanetData()
		{
			if (this.replayResponseData != null && this.replayResponseData.ReplayData != null)
			{
				return Service.Get<IDataController>().Get<PlanetVO>(this.replayResponseData.ReplayData.PlanetId);
			}
			return null;
		}

		protected internal ReplayMapDataLoader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetFactionAssetName());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPlanetData());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPreloads());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetProjectilePreloads((Map)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldType());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).Initialize((BattleParticipant)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).InitializeFromData((BattleEntry)GCHandledObjects.GCHandleToObject(*args), (BattleRecord)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).LoadMapData((MapLoadedDelegate)GCHandledObjects.GCHandleToObject(*args), (MapLoadFailDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).OnReplayLoaded((GetReplayResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).SetReplayResponseData((GetReplayResponse)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
