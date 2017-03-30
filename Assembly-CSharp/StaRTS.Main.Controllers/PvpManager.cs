using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Controllers.World.Transitions;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Pvp;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaRTS.Main.Controllers
{
	public class PvpManager : IEventObserver
	{
		private const string GET_PVP_TARGET_FAILURE = "GET_PVP_TARGET_FAILURE";

		private const string BI_DELIMITER = "|";

		private const int NOT_FOUND = -1;

		private const float SHOW_BATTLE_SUMMARY_SCREEN_DELAY = 1f;

		private PvpCountdownView countdown;

		private Dictionary<int, int> costByHqLevel;

		private int maxHqLevel;

		private uint pvpSearchTimerId;

		private List<BattleEntry> recentBattleList;

		private PvpGetNextTargetCommand nextTargetCommand;

		public PvpTarget CurrentPvpTarget
		{
			get;
			private set;
		}

		public PvpManager()
		{
			Service.Set<PvpManager>(this);
			this.recentBattleList = new List<BattleEntry>();
			this.CurrentPvpTarget = null;
			this.costByHqLevel = new Dictionary<int, int>();
			string pVP_SEARCH_COST_BY_HQ_LEVEL = GameConstants.PVP_SEARCH_COST_BY_HQ_LEVEL;
			string[] array = pVP_SEARCH_COST_BY_HQ_LEVEL.Split(new char[]
			{
				' '
			});
			this.costByHqLevel.Add(-1, Convert.ToInt32(array[0]));
			int i;
			for (i = 0; i < array.Length; i++)
			{
				int key = i + 1;
				int value = Convert.ToInt32(array[i]);
				this.costByHqLevel.Add(key, value);
			}
			this.maxHqLevel = i;
			Service.Get<EventManager>().RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
			this.pvpSearchTimerId = 0u;
		}

		public List<BattleEntry> GetBattlesThatHappenOffline()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			uint lastLoginTime = currentPlayer.LastLoginTime;
			List<BattleEntry> list = new List<BattleEntry>();
			List<BattleEntry> battleHistory = currentPlayer.BattleHistory.GetBattleHistory();
			for (int i = 0; i < battleHistory.Count; i++)
			{
				if (battleHistory[i].IsPvP() && battleHistory[i].DefenderID == currentPlayer.PlayerId && battleHistory[i].EndBattleServerTime > lastLoginTime)
				{
					list.Add(battleHistory[i]);
				}
			}
			return list;
		}

		public void PurchaseNextBattle()
		{
			HomeMapDataLoader homeMapDataLoader = Service.Get<HomeMapDataLoader>();
			WorldToWorldTransition transition = new WorldToWorldTransition(null, homeMapDataLoader, null, false, false);
			Service.Get<WorldTransitioner>().StartWipe(transition, homeMapDataLoader.GetPlanetData());
			this.KillTimer();
			PvpGetNextTargetRequest request = new PvpGetNextTargetRequest();
			this.nextTargetCommand = new PvpGetNextTargetCommand(request);
			this.nextTargetCommand.AddSuccessCallback(new AbstractCommand<PvpGetNextTargetRequest, PvpTarget>.OnSuccessCallback(this.OnPvpTargetFound));
			this.nextTargetCommand.AddFailureCallback(new AbstractCommand<PvpGetNextTargetRequest, PvpTarget>.OnFailureCallback(this.OnPvpError));
			Service.Get<ServerAPI>().Sync(this.nextTargetCommand);
			this.StartSearchTimer();
			Service.Get<TournamentController>().EnterPlanetConflict();
			Service.Get<UserInputInhibitor>().DenyAll();
		}

		private void OnPvpTargetFound(PvpTarget target, object cookie)
		{
			this.KillSearchTimer();
			this.CurrentPvpTarget = target;
			int pvpMatchCost = this.GetPvpMatchCost();
			if (pvpMatchCost != target.CreditsCharged)
			{
				Service.Get<Logger>().ErrorFormat("Pvp target credit cost mismatch. Client: {0}, Server: {1}", new object[]
				{
					pvpMatchCost,
					target.CreditsCharged
				});
			}
			else
			{
				GameUtils.SpendCurrency(target.CreditsCharged, 0, 0, false);
			}
			ISupportController supportController = Service.Get<ISupportController>();
			supportController.SimulateCheckAllContractsWithCurrentTime();
			supportController.SyncCurrentPlayerInventoryWithServer(target.AttackerDeployableServerData);
			this.OnTargetReady(target, false);
			Service.Get<UserInputInhibitor>().AllowAll();
		}

		private void OnPvpCheatTargetFound(PvpTarget target, object cookie)
		{
			this.CurrentPvpTarget = target;
			this.OnTargetReady(target, false);
			Service.Get<UserInputInhibitor>().AllowAll();
		}

		public void StartRevenge(string opponentId)
		{
			PvpRevengeCommand pvpRevengeCommand = new PvpRevengeCommand(new PvpRevengeRequest
			{
				OpponentId = opponentId
			});
			pvpRevengeCommand.AddSuccessCallback(new AbstractCommand<PvpRevengeRequest, PvpTarget>.OnSuccessCallback(this.OnPvpRevengeFound));
			pvpRevengeCommand.AddFailureCallback(new AbstractCommand<PvpRevengeRequest, PvpTarget>.OnFailureCallback(this.OnPvpRevengeError));
			Service.Get<ServerAPI>().Sync(pvpRevengeCommand);
			Service.Get<TournamentController>().EnterPlanetConflict();
		}

		private void OnPvpRevengeFound(PvpTarget target, object cookie)
		{
			this.CurrentPvpTarget = target;
			this.OnTargetReady(target, true);
		}

		private void OnTargetReady(PvpTarget target, bool isRevenge)
		{
			Service.Get<CurrentPlayer>().ProtectedUntil = 0u;
			BattleInitializationData data = BattleInitializationData.CreateFromPvpTargetData(target, isRevenge);
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is BattleStartState)
			{
				BattleStartState.GoToBattleStartState((BattleStartState)currentState, data, new TransitionCompleteDelegate(this.OnBattleReady));
			}
			else
			{
				BattleStartState.GoToBattleStartState(data, new TransitionCompleteDelegate(this.OnBattleReady));
			}
		}

		private void OnPvpError(uint statusCode, object cookie)
		{
			this.KillSearchTimer();
			Service.Get<EventManager>().SendEvent(EventId.PvpOpponentNotFound, this.PVPOpponentNotFoundMessage("none_found"));
		}

		private void OnPvpRevengeError(uint statusCode, object cookie)
		{
			this.KillSearchTimer();
			Service.Get<EventManager>().SendEvent(EventId.PvpRevengeOpponentNotFound, "none_found");
			Service.Get<UserInputInhibitor>().AllowAll();
		}

		public void ReleasePvpTarget()
		{
			this.CurrentPvpTarget = null;
			PlayerIdChecksumRequest request = new PlayerIdChecksumRequest();
			PvpReleaseTargetcommand command = new PvpReleaseTargetcommand(request);
			Service.Get<ServerAPI>().Enqueue(command);
		}

		private void OnBattleReady()
		{
			Service.Get<UXController>().HUD.EnableNextBattleButton();
			this.StartCountdown();
		}

		public void StartCountdown()
		{
			this.KillTimer();
			this.countdown = new PvpCountdownView(new Action(this.OnCountdownComplete));
		}

		private void OnCountdownComplete()
		{
			Service.Get<GameStateMachine>().SetState(new BattlePlayState());
		}

		public void OnPvpGetNextTargetFailure()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			WorldTransitioner worldTransitioner = Service.Get<WorldTransitioner>();
			worldTransitioner.SetAlertMessage(Service.Get<Lang>().Get("GET_PVP_TARGET_FAILURE", new object[0]));
			if (worldTransitioner.IsSoftWiping())
			{
				worldTransitioner.FinishWipe();
			}
			if (!(currentState is HomeState))
			{
				if (currentState is GalaxyState)
				{
					HomeState.GoToHomeState(null, false);
				}
				else
				{
					worldTransitioner.SetTransitionInStartCallback(new TransitionInStartDelegate(this.OnTransitionInStartLoadHome));
				}
			}
			Service.Get<UserInputInhibitor>().AllowAll();
		}

		private void OnTransitionInStartLoadHome()
		{
			HomeState.GoToHomeStateAndReloadMap();
		}

		private void PVPSearchTimeOut(uint id, object cookie)
		{
			this.nextTargetCommand.RemoveAllCallbacks();
			Service.Get<EventManager>().SendEvent(EventId.PvpOpponentNotFound, this.PVPOpponentNotFoundMessage("time_out"));
			this.OnPvpGetNextTargetFailure();
		}

		public string PVPOpponentNotFoundMessage(string cause)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int currentXPAmount = currentPlayer.CurrentXPAmount;
			int playerMedals = currentPlayer.PlayerMedals;
			float num = GameConstants.PVP_MATCH_BONUS_ATTACKER_SLOPE * (float)playerMedals + GameConstants.PVP_MATCH_BONUS_ATTACKER_Y_INTERCEPT;
			int value = (int)Math.Round((double)((float)currentXPAmount * num));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(cause);
			stringBuilder.Append("|");
			stringBuilder.Append(value);
			stringBuilder.Append("|");
			stringBuilder.Append(currentXPAmount);
			stringBuilder.Append("|");
			stringBuilder.Append(playerMedals);
			stringBuilder.Append("|");
			stringBuilder.Append(currentPlayer.PlanetId);
			return stringBuilder.ToString();
		}

		private void StartSearchTimer()
		{
			this.KillSearchTimer();
			this.pvpSearchTimerId = Service.Get<ViewTimerManager>().CreateViewTimer((float)GameConstants.PVP_SEARCH_TIMEOUT_DURATION, false, new TimerDelegate(this.PVPSearchTimeOut), null);
		}

		private void KillSearchTimer()
		{
			if (this.pvpSearchTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.pvpSearchTimerId);
				this.pvpSearchTimerId = 0u;
			}
		}

		public void KillTimer()
		{
			if (this.countdown != null)
			{
				this.countdown.Destroy();
				this.countdown = null;
			}
		}

		public void OnPvpRevengeFailure(uint status)
		{
			string message;
			switch (status)
			{
			case 2100u:
				message = Service.Get<Lang>().Get("PVP_TARGET_UNDER_PROTECTION", new object[0]);
				break;
			case 2101u:
				message = Service.Get<Lang>().Get("PVP_TARGET_UNDER_ATTACK", new object[0]);
				break;
			case 2102u:
				message = Service.Get<Lang>().Get("PVP_TARGET_ONLINE", new object[0]);
				break;
			case 2103u:
			case 2104u:
			case 2105u:
			case 2106u:
				message = Service.Get<Lang>().Get("PVP_TARGET_CANNOT_BE_ATTACKED", new object[0]);
				break;
			case 2107u:
				message = Service.Get<Lang>().Get("PVP_TARGET_HAS_RELOCATED", new object[0]);
				break;
			default:
				message = Service.Get<Lang>().Get("GET_PVP_TARGET_FAILURE", new object[0]);
				break;
			}
			AlertScreen.ShowModal(false, null, message, null, null);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.GameStateChanged)
			{
				this.KillTimer();
			}
			return EatResponse.NotEaten;
		}

		public void OnPvpBattleComplete(PvpBattleEndResponse endResponse, object cookie)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			currentPlayer.BattleHistory.AddBattle(endResponse.BattleEntry);
			this.CurrentPvpTarget = null;
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			currentBattle.Attacker = endResponse.BattleEntry.Attacker;
			currentBattle.Defender = endResponse.BattleEntry.Defender;
			currentPlayer.AttackRating += endResponse.BattleEntry.Attacker.AttackRatingDelta;
			currentPlayer.DefenseRating += endResponse.BattleEntry.Attacker.DefenseRatingDelta;
			if (endResponse.BattleEntry.Won)
			{
				currentPlayer.AttacksWon++;
			}
			AchievementController achievementController = Service.Get<AchievementController>();
			achievementController.TryUnlockAchievementByValue(AchievementType.LootCreditsPvp, currentPlayer.BattleHistory.GetTotalPvpCreditsLooted());
			achievementController.TryUnlockAchievementByValue(AchievementType.LootAlloyPvp, currentPlayer.BattleHistory.GetTotalPvpMaterialLooted());
			achievementController.TryUnlockAchievementByValue(AchievementType.LootContrabandPvp, currentPlayer.BattleHistory.GetTotalPvpContrabandLooted());
			achievementController.TryUnlockAchievementByValue(AchievementType.PvpWon, currentPlayer.BattleHistory.GetTotalPvpWins());
			this.recentBattleList.Add(endResponse.BattleEntry);
			Service.Get<TournamentController>().OnPvpBattleComplete(endResponse.TournamentData, currentBattle.Attacker.TournamentRatingDelta);
		}

		public void HandleNotEnoughCreditsForNextBattle()
		{
			if (this.countdown != null)
			{
				this.countdown.Pause();
			}
			string message = Service.Get<Lang>().Get("NOT_ENOUGH_RESOURCES", new object[0]);
			AlertScreen.ShowModal(false, null, message, new OnScreenModalResult(this.OnNotEnoughCreditsModalResult), null);
		}

		private void OnNotEnoughCreditsModalResult(object modalResult, object cookie)
		{
			if (this.countdown != null)
			{
				this.countdown.Resume();
			}
		}

		public int GetPvpMatchCost()
		{
			int num = Service.Get<CurrentPlayer>().Map.FindHighestHqLevel();
			num = Math.Min(num, this.maxHqLevel);
			return this.costByHqLevel[num];
		}

		public void ReplayBattle(string battleId, BattleParticipant defender, string sharerPlayerId)
		{
			ProcessingScreen.Show();
			ReplayMapDataLoader replayMapDataLoader = Service.Get<ReplayMapDataLoader>();
			replayMapDataLoader.Initialize(defender, sharerPlayerId);
			GetReplayCommand getReplayCommand = new GetReplayCommand(Service.Get<CurrentPlayer>().PlayerId, battleId, defender.PlayerId);
			getReplayCommand.AddSuccessCallback(new AbstractCommand<GetReplayRequest, GetReplayResponse>.OnSuccessCallback(replayMapDataLoader.OnReplayLoaded));
			getReplayCommand.AddFailureCallback(new AbstractCommand<GetReplayRequest, GetReplayResponse>.OnFailureCallback(replayMapDataLoader.OnReplayLoadFailed));
			Service.Get<ServerAPI>().Sync(getReplayCommand);
			Service.Get<BattlePlaybackController>().LogReplayViewed(battleId, defender.PlayerId, sharerPlayerId);
		}
	}
}
