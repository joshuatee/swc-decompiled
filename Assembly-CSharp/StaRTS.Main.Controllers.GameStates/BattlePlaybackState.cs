using StaRTS.Main.Controllers.World;
using StaRTS.Main.Controllers.World.Transitions;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.GameStates
{
	public class BattlePlaybackState : IGameState, IState, IEventObserver
	{
		private BattleRecord battleRecord;

		private BattleEntry battleEntry;

		protected BattlePlaybackState()
		{
		}

		public static void GoToBattlePlaybackState(BattleRecord battleRecord, BattleEntry battleEntry, IMapDataLoader mapDataLoader)
		{
			BattlePlaybackState battlePlaybackState = new BattlePlaybackState();
			Service.Get<BattlePlaybackController>().InitPlayback(battleRecord, battleEntry);
			battlePlaybackState.Setup(battleRecord, battleEntry, mapDataLoader);
		}

		public static void GoToBattlePlaybackState(ReplayMapDataLoader mapDataLoader)
		{
			BattlePlaybackState battlePlaybackState = new BattlePlaybackState();
			battlePlaybackState.Setup(null, null, mapDataLoader);
			Service.Get<EventManager>().RegisterObserver(battlePlaybackState, EventId.BattleRecordRetrieved, EventPriority.Default);
		}

		protected void Setup(BattleRecord battleRecord, BattleEntry battleEntry, IMapDataLoader mapDataLoader)
		{
			this.battleRecord = battleRecord;
			this.battleEntry = battleEntry;
			if (battleRecord != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.BattleReplaySetup, battleRecord);
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.MapDataProcessingStart, EventPriority.Default);
			Service.Get<WorldTransitioner>().StartTransition(new WorldToWorldTransition(this, mapDataLoader, new TransitionCompleteDelegate(this.OnWorldTransitionComplete), false, true));
		}

		public void OnEnter()
		{
			Service.Get<UXController>().HUD.Visible = true;
			Service.Get<UXController>().HUD.ConfigureControls(new HudConfig(new string[]
			{
				"Currency",
				"OpponentInfo",
				"ButtonHome",
				"DamageStars",
				"LabelBaseNameOpponent",
				"LabelCurrencyValueOpponent",
				"ReplayControls"
			}));
		}

		public void OnExit(IState nextState)
		{
			Service.Get<BattleController>().EndBattleRightAway();
			Service.Get<BattlePlaybackController>().EndPlayback();
		}

		private void OnMapProcessingStart()
		{
			Service.Get<Rand>().SimSeed = this.battleRecord.SimSeed;
		}

		private void OnWorldTransitionComplete()
		{
			Service.Get<BattlePlaybackController>().StartPlayback();
			Service.Get<ChampionController>().DestroyAllChampionEntities();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.MapDataProcessingStart)
			{
				if (id == EventId.BattleRecordRetrieved)
				{
					GetReplayResponse getReplayResponse = (GetReplayResponse)cookie;
					this.battleRecord = getReplayResponse.ReplayData;
					this.battleEntry = getReplayResponse.EntryData;
					Service.Get<BattlePlaybackController>().InitPlayback(this.battleRecord, this.battleEntry);
				}
			}
			else
			{
				this.OnMapProcessingStart();
				Service.Get<EventManager>().UnregisterObserver(this, EventId.MapDataProcessingStart);
			}
			return EatResponse.NotEaten;
		}

		public bool CanUpdateHomeContracts()
		{
			return false;
		}

		protected internal BattlePlaybackState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattlePlaybackState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			BattlePlaybackState.GoToBattlePlaybackState((ReplayMapDataLoader)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			BattlePlaybackState.GoToBattlePlaybackState((BattleRecord)GCHandledObjects.GCHandleToObject(*args), (BattleEntry)GCHandledObjects.GCHandleToObject(args[1]), (IMapDataLoader)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BattlePlaybackState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattlePlaybackState)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((BattlePlaybackState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((BattlePlaybackState)GCHandledObjects.GCHandleToObject(instance)).OnMapProcessingStart();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BattlePlaybackState)GCHandledObjects.GCHandleToObject(instance)).OnWorldTransitionComplete();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((BattlePlaybackState)GCHandledObjects.GCHandleToObject(instance)).Setup((BattleRecord)GCHandledObjects.GCHandleToObject(*args), (BattleEntry)GCHandledObjects.GCHandleToObject(args[1]), (IMapDataLoader)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}
	}
}
