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

namespace StaRTS.Main.Controllers.GameStates
{
	public class BattlePlaybackState : IGameState, IEventObserver, IState
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
	}
}
