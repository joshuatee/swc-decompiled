using StaRTS.Externals.BI;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Controllers.World.Transitions;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Crates;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Commands.Squads;
using StaRTS.Main.Models.Commands.Squads.Requests;
using StaRTS.Main.Models.Commands.Squads.Responses;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class SquadWarManager : IEventObserver, IViewClockTimeObserver
	{
		public const int MAX_VICTORY_POINTS_LEFT = 3;

		public const int PLAYER_SQUAD = 0;

		public const int OPPONENT_SQUAD = 1;

		public const int NUM_SQUADS_IN_WAR = 2;

		public const int NOT_IN_WAR_OR_MATCHMAKING = 0;

		public const int IN_WAR_OR_MATCHMAKING = 1;

		private const string BUFF_BASE_ATTACK_STARTED = "WAR_ATTACK_BUFF_BASE_STARTED";

		private const string BUFF_BASE_ATTACK_ENDED = "WAR_ATTACK_BUFF_BASE_ENDED";

		private const string BUFF_BASE_ATTACK_WON = "WAR_BUFF_BASE_CAPTURED";

		private const string PLAYER_ATTACK_STARTED = "WAR_ATTACK_PLAYER_STARTED";

		private const string PLAYER_ATTACKED = "WAR_PLAYER_ATTACKED";

		private const string BUFF_BASE_OWNERSHIP_PLAYER = "BUFF_BASE_OWNERSHIP_PLAYER";

		private const string BUFF_BASE_OWNERSHIP_OPPONENT = "BUFF_BASE_OWNERSHIP_OPPONENT";

		private const string NOT_ENOUGH_TROOPS_TITLE_STRING = "NOT_ENOUGH_TROOPS_TITLE";

		private const string NOT_ENOUGH_TROOPS_FOR_ATTACK_STRING = "NOT_ENOUGH_TROOPS_FOR_ATTACK";

		private const string WAR_BUFF_BASE_ALREADY_CAPTURED = "WAR_BUFF_BASE_ALREADY_CAPTURED";

		private const string WAR_NO_TURNS_NO_SCOUTING = "WAR_NO_TURNS_NO_SCOUTING";

		private const string WAR_TARGET_NO_POINTS = "WAR_TARGET_NO_POINTS";

		private const string WAR_PREVENT_ENEMY_SCOUT_WRONG_PHASE = "WAR_PREVENT_ENEMY_SCOUT_WRONG_PHASE";

		private const string WAR_PREVENT_ALLY_SCOUT_WRONG_PHASE = "WAR_PREVENT_ALLY_SCOUT_WRONG_PHASE";

		private const string WAR_PREVENT_BUFF_BASE_SCOUT_WRONG_PHASE = "WAR_PREVENT_BUFF_BASE_SCOUT_WRONG_PHASE";

		private const string WAR_BOARD_INSUFFICIENT_LEVEL_TITLE = "WAR_BOARD_INSUFFICIENT_LEVEL_TITLE";

		private const string WAR_BOARD_INSUFFICIENT_LEVEL_BODY = "WAR_BOARD_INSUFFICIENT_LEVEL_BODY";

		private const string WAR_DISABLED_TITLE = "WAR_DISABLED_TITLE";

		private const string WAR_DISABLED_BODY = "WAR_DISABLED_BODY";

		private const string WAR_MATCHMAKING_STARTED = "WAR_MATCHMAKING_STARTED";

		private const string WAR_BOARD_MATCHMAKING_EXIT_TITLE = "WAR_BOARD_MATCHMAKING_EXIT_TITLE";

		private const string WAR_BOARD_MATCHMAKING_EXIT_BODY = "WAR_BOARD_MATCHMAKING_EXIT_BODY";

		public int NumParticipants;

		public SquadWarData CurrentSquadWar;

		private SquadWarStatusType previousStatus;

		private string currentlyScoutedBuffBaseId;

		private SquadController controller;

		private SqmWarEventData eventDataToBeShown;

		private List<string> warParty;

		public bool MatchMakingPrepMode;

		private SquadMemberWarData currentMemberWarData;

		private SquadWarParticipantState currentParticipantState;

		public bool EnableSquadWarMode
		{
			get;
			set;
		}

		public SquadWarManager(SquadController controller)
		{
			this.EnableSquadWarMode = true;
			this.MatchMakingPrepMode = false;
			this.controller = controller;
			this.warParty = new List<string>();
			Service.Get<EventManager>().RegisterObserver(this, EventId.WarLaunchFlow);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ContractCompleted);
			Service.Get<EventManager>().RegisterObserver(this, EventId.WarPhaseChanged);
		}

		public SquadWarStatusType GetCurrentStatus()
		{
			return SquadUtils.GetWarStatus(this.CurrentSquadWar, Service.Get<ServerAPI>().ServerTime);
		}

		public bool IsTimeWithinCurrentSquadWarPhase(int serverTime)
		{
			return SquadUtils.IsTimeWithinSquadWarPhase(this.CurrentSquadWar, (uint)serverTime);
		}

		public void UpdateSquadWar(SquadWarData squadwarData)
		{
			this.CurrentSquadWar = squadwarData;
			SquadWarSquadData squadWarSquadData = this.CurrentSquadWar.Squads[0];
			Squad currentSquad = this.controller.StateManager.GetCurrentSquad();
			squadWarSquadData.Faction = currentSquad.Faction;
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
			this.NumParticipants = squadWarSquadData.Participants.Count;
			string playerId = Service.Get<CurrentPlayer>().PlayerId;
			for (int i = 0; i < this.NumParticipants; i++)
			{
				SquadWarParticipantState squadWarParticipantState = squadWarSquadData.Participants[i];
				if (squadWarParticipantState.SquadMemberId == playerId)
				{
					this.currentParticipantState = squadWarParticipantState;
					return;
				}
			}
		}

		public WarScheduleVO GetCurrentWarScheduleData()
		{
			IDataController dataController = Service.Get<IDataController>();
			foreach (WarScheduleVO current in dataController.GetAll<WarScheduleVO>())
			{
				if (current.StartTime <= this.CurrentSquadWar.StartTimeStamp && this.CurrentSquadWar.StartTimeStamp <= current.EndTime)
				{
					return current;
				}
			}
			Service.Get<StaRTSLogger>().Error("Could not find war schedule data for current squad war!");
			return null;
		}

		public SquadWarSquadData GetSquadData(SquadWarSquadType squadType)
		{
			SquadWarSquadData result;
			if (squadType == SquadWarSquadType.PLAYER_SQUAD)
			{
				result = this.CurrentSquadWar.Squads[0];
			}
			else
			{
				result = this.CurrentSquadWar.Squads[1];
			}
			return result;
		}

		public SquadWarSquadData GetSquadData(string squadId)
		{
			for (int i = 0; i < 2; i++)
			{
				if (this.CurrentSquadWar.Squads[i].SquadId == squadId)
				{
					return this.CurrentSquadWar.Squads[i];
				}
			}
			return null;
		}

		public SquadWarParticipantState GetParticipantState(int index, SquadWarSquadType squadType)
		{
			SquadWarParticipantState result;
			if (squadType == SquadWarSquadType.PLAYER_SQUAD)
			{
				result = this.CurrentSquadWar.Squads[0].Participants[index];
			}
			else
			{
				result = this.CurrentSquadWar.Squads[1].Participants[index];
			}
			return result;
		}

		public SquadWarSquadType GetParticipantSquad(string squadMemberId)
		{
			List<SquadWarParticipantState> participants = this.CurrentSquadWar.Squads[0].Participants;
			int i = 0;
			int count = participants.Count;
			while (i < count)
			{
				if (participants[i].SquadMemberId == squadMemberId)
				{
					return SquadWarSquadType.PLAYER_SQUAD;
				}
				i++;
			}
			return SquadWarSquadType.OPPONENT_SQUAD;
		}

		public void UpdateCurrentMemberWarData(SquadMemberWarDataResponse response)
		{
			this.currentMemberWarData = response.MemberWarData;
			if (this.currentMemberWarData != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.CurrentPlayerMemberDataUpdated, null);
			}
		}

		public SquadMemberWarData GetCurrentMemberWarData()
		{
			return this.currentMemberWarData;
		}

		public SquadWarParticipantState GetCurrentParticipantState()
		{
			return this.currentParticipantState;
		}

		public SquadWarParticipantState GetCurrentOpponentState()
		{
			string playerId = Service.Get<BattleController>().GetCurrentBattle().Defender.PlayerId;
			if (string.IsNullOrEmpty(playerId))
			{
				return null;
			}
			return this.FindParticipantState(playerId);
		}

		public SquadWarRewardData GetCurrentPlayerCurrentWarReward()
		{
			if (this.currentMemberWarData != null && this.currentMemberWarData.WarReward != null && this.currentMemberWarData.WarReward.WarId == this.CurrentSquadWar.WarId)
			{
				return this.currentMemberWarData.WarReward;
			}
			return null;
		}

		private bool RemoveCurrentPlayerCurrentWarReward()
		{
			return this.currentMemberWarData.WarReward != null && this.currentMemberWarData.WarReward.WarId == this.CurrentSquadWar.WarId && this.currentMemberWarData.RemoveSquadWarReward();
		}

		public int GetCurrentSquadWarResult()
		{
			int currentSquadScore = this.GetCurrentSquadScore(SquadWarSquadType.PLAYER_SQUAD);
			int currentSquadScore2 = this.GetCurrentSquadScore(SquadWarSquadType.OPPONENT_SQUAD);
			return Math.Sign(currentSquadScore - currentSquadScore2);
		}

		public int GetCurrentSquadScore(SquadWarSquadType squadType)
		{
			int num = 0;
			SquadWarSquadData squadWarSquadData = this.CurrentSquadWar.Squads[(int)squadType];
			int i = 0;
			int count = squadWarSquadData.Participants.Count;
			while (i < count)
			{
				SquadWarParticipantState participantState = this.GetParticipantState(i, squadType);
				num += participantState.Score;
				i++;
			}
			return num;
		}

		public void ClaimSquadWarReward(string warId)
		{
			ProcessingScreen.Show();
			SquadWarClaimRewardRequest request = new SquadWarClaimRewardRequest(warId);
			SquadWarClaimRewardCommand squadWarClaimRewardCommand = new SquadWarClaimRewardCommand(request);
			squadWarClaimRewardCommand.Context = warId;
			squadWarClaimRewardCommand.AddSuccessCallback(new AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>.OnSuccessCallback(this.OnSquadWarClaimRewardSuccess));
			squadWarClaimRewardCommand.AddFailureCallback(new AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>.OnFailureCallback(this.OnSquadWarClaimRewardFailed));
			Service.Get<ServerAPI>().Sync(squadWarClaimRewardCommand);
		}

		private void OnSquadWarClaimRewardSuccess(CrateDataResponse response, object cookie)
		{
			ProcessingScreen.Hide();
			string text = cookie as string;
			if (this.currentMemberWarData == null || this.currentMemberWarData.WarRewards == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Attempting to claim reward for war {0} but memberWarData contains no rewards", new object[]
				{
					text
				});
				return;
			}
			SquadWarRewardData squadWarRewardData = null;
			int i = 0;
			int count = this.currentMemberWarData.WarRewards.Count;
			while (i < count)
			{
				if (this.currentMemberWarData.WarRewards[i].WarId == text)
				{
					squadWarRewardData = this.currentMemberWarData.WarRewards[i];
					this.currentMemberWarData.WarRewards.RemoveAt(i);
					break;
				}
				i++;
			}
			if (squadWarRewardData == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Could not find reward for war {0} in memberWarData", new object[]
				{
					text
				});
				return;
			}
			CrateData crateDataTO = response.CrateDataTO;
			if (crateDataTO != null)
			{
				this.ShowWarRewardSupplyCrateAnimation(squadWarRewardData, crateDataTO);
			}
			Service.Get<EventManager>().SendEvent(EventId.WarRewardClaimed, null);
		}

		private void OnSquadWarClaimRewardFailed(uint status, object data)
		{
			ProcessingScreen.Hide();
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is WarBoardState)
			{
				this.CloseWarEndedScreen();
			}
		}

		public bool ClaimCurrentPlayerCurrentWarReward()
		{
			SquadWarRewardData currentPlayerCurrentWarReward = this.GetCurrentPlayerCurrentWarReward();
			if (currentPlayerCurrentWarReward != null)
			{
				this.ClaimSquadWarReward(currentPlayerCurrentWarReward.WarId);
				return true;
			}
			Service.Get<StaRTSLogger>().Error("Trying to claim a non existant squad war reward");
			return false;
		}

		private void ShowWarRewardSupplyCrateAnimation(SquadWarRewardData rewardData, CrateData crateData)
		{
			SquadWarEndCelebrationScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<SquadWarEndCelebrationScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.PlayCloseAnimation();
			}
			List<string> resolvedSupplyIdList = GameUtils.GetResolvedSupplyIdList(crateData);
			InventoryCrateRewardController inventoryCrateRewardController = Service.Get<InventoryCrateRewardController>();
			InventoryCrateAnimation inventoryCrateAnimation = inventoryCrateRewardController.GrantInventoryCrateReward(resolvedSupplyIdList, crateData);
			inventoryCrateAnimation.InvCrateCollectionScreen.OnModalResult = new OnScreenModalResult(this.OnCrateScreenClosed);
		}

		private void OnCrateScreenClosed(object result, object cookie)
		{
			this.CloseWarEndedScreen();
		}

		public bool ShowWarEndedScreen()
		{
			if (Service.Get<ScreenController>().GetHighestLevelScreen<SquadWarEndCelebrationScreen>() != null)
			{
				return false;
			}
			if (this.GetCurrentPlayerCurrentWarReward() == null)
			{
				return false;
			}
			SquadWarSquadData squadWarSquadData = this.CurrentSquadWar.Squads[0];
			FactionType faction = squadWarSquadData.Faction;
			string squadName = squadWarSquadData.SquadName;
			int currentSquadWarResult = this.GetCurrentSquadWarResult();
			SquadWarSquadData squadWarSquadData2 = this.CurrentSquadWar.Squads[1];
			FactionType faction2 = squadWarSquadData2.Faction;
			bool sameFaction = faction == faction2;
			SquadWarEndCelebrationScreen screen = new SquadWarEndCelebrationScreen(currentSquadWarResult, faction, squadName, sameFaction);
			Service.Get<ScreenController>().AddScreen(screen);
			Service.Get<UXController>().HUD.SlideSquadScreenClosedInstantly();
			Service.Get<UXController>().HUD.SetSquadScreenVisibility(false);
			return true;
		}

		private void CloseWarEndedScreen()
		{
			SquadWarEndCelebrationScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<SquadWarEndCelebrationScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.OnModalResult = new OnScreenModalResult(this.OnCelebrationScreenClosed);
				highestLevelScreen.Close(null);
			}
			Service.Get<UXController>().HUD.SetSquadScreenVisibility(true);
		}

		public SquadWarBuffBaseData GetBuffBaseData(int index)
		{
			return this.CurrentSquadWar.BuffBases[index];
		}

		public bool IsBuffBaseUnderAttack(SquadWarBuffBaseData buffBaseData)
		{
			bool result = false;
			if (buffBaseData.AttackExpirationDate > 0u)
			{
				result = (ServerTime.Time <= buffBaseData.AttackExpirationDate);
			}
			return result;
		}

		public SquadWarScoutState CanAttackBuffBase(SquadWarBuffBaseData buffBaseData)
		{
			if (this.GetCurrentStatus() != SquadWarStatusType.PhaseAction)
			{
				return SquadWarScoutState.NotInActionPhase;
			}
			if (this.currentParticipantState == null)
			{
				return SquadWarScoutState.NotPatricipantInWar;
			}
			if (this.currentParticipantState.TurnsLeft <= 0)
			{
				return SquadWarScoutState.NoTurnsLeft;
			}
			if (this.IsBuffBaseUnderAttack(buffBaseData))
			{
				return SquadWarScoutState.UnderAttack;
			}
			WarBuffVO warBuffVO = Service.Get<IDataController>().Get<WarBuffVO>(buffBaseData.BuffBaseId);
			string planetId = warBuffVO.PlanetId;
			if (!Service.Get<CurrentPlayer>().IsPlanetUnlocked(planetId))
			{
				return SquadWarScoutState.DestinationUnavailable;
			}
			return SquadWarScoutState.AttackAvailable;
		}

		public SquadWarBuffBaseData GetCurrentlyScoutedBuffBaseData()
		{
			return this.FindBuffBaseData(this.currentlyScoutedBuffBaseId);
		}

		public SquadWarScoutState CanAttackCurrentlyScoutedBuffBase()
		{
			if (string.IsNullOrEmpty(this.currentlyScoutedBuffBaseId))
			{
				return SquadWarScoutState.Invalid;
			}
			SquadWarBuffBaseData buffBaseData = this.FindBuffBaseData(this.currentlyScoutedBuffBaseId);
			return this.CanAttackBuffBase(buffBaseData);
		}

		public bool IsCurrentlyScoutingOwnedBuffBase()
		{
			Squad currentSquad = this.controller.StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return false;
			}
			SquadWarBuffBaseData squadWarBuffBaseData = this.FindBuffBaseData(this.currentlyScoutedBuffBaseId);
			return squadWarBuffBaseData != null && squadWarBuffBaseData.OwnerId != null && squadWarBuffBaseData.OwnerId == currentSquad.SquadID;
		}

		public bool IsOpponentUnderAttack(SquadWarParticipantState opponentState)
		{
			bool result = false;
			if (opponentState.DefendingAttackExpirationDate > 0u)
			{
				result = (ServerTime.Time <= opponentState.DefendingAttackExpirationDate);
			}
			return result;
		}

		public SquadWarScoutState CanAttackOpponent(SquadWarParticipantState opponentState)
		{
			if (this.GetCurrentStatus() != SquadWarStatusType.PhaseAction)
			{
				return SquadWarScoutState.NotInActionPhase;
			}
			if (this.currentParticipantState == null)
			{
				return SquadWarScoutState.NotPatricipantInWar;
			}
			if (this.currentParticipantState.TurnsLeft <= 0)
			{
				return SquadWarScoutState.NoTurnsLeft;
			}
			if (opponentState.VictoryPointsLeft <= 0)
			{
				return SquadWarScoutState.OpponentHasNoVictoryPointsLeft;
			}
			if (this.IsOpponentUnderAttack(opponentState))
			{
				return SquadWarScoutState.UnderAttack;
			}
			return SquadWarScoutState.AttackAvailable;
		}

		public SquadWarScoutState CanAttackCurrentlyScoutedOpponent()
		{
			string playerId = Service.Get<BattleController>().GetCurrentBattle().Defender.PlayerId;
			if (string.IsNullOrEmpty(playerId))
			{
				return SquadWarScoutState.Invalid;
			}
			SquadWarParticipantState opponentState = this.FindParticipantState(playerId);
			return this.CanAttackOpponent(opponentState);
		}

		public void ClearSquadWarData()
		{
			this.CurrentSquadWar = null;
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad != null)
			{
				currentSquad.ClearSquadWarId();
			}
		}

		public void EndSquadWar()
		{
			this.ClearSquadWarData();
		}

		public void OnViewClockTime(float dt)
		{
			uint serverTime = Service.Get<ServerAPI>().ServerTime;
			SquadWarStatusType warStatus = SquadUtils.GetWarStatus(this.CurrentSquadWar, serverTime);
			if (warStatus != this.previousStatus)
			{
				Service.Get<EventManager>().SendEvent(EventId.WarPhaseChanged, warStatus);
				this.previousStatus = warStatus;
			}
		}

		public void HandleWarEventMsg(SquadMsg msg)
		{
			SqmOwnerData ownerData = msg.OwnerData;
			SqmWarEventData warEventData = msg.WarEventData;
			if (warEventData == null)
			{
				return;
			}
			string text = (ownerData != null) ? ownerData.PlayerId : null;
			string text2 = (ownerData != null) ? ownerData.PlayerName : null;
			string opponentId = warEventData.OpponentId;
			string opponentName = warEventData.OpponentName;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			bool flag = text == currentPlayer.PlayerId;
			string text3 = null;
			switch (msg.Type)
			{
			case SquadMsgType.WarBuffBaseAttackStart:
			{
				SquadWarSquadData squad = null;
				this.FindParticipantState(text, out squad);
				SquadWarBuffBaseData buffBase = this.FindBuffBaseData(warEventData.BuffBaseUid);
				this.UpdateAttackStateForBuffBase(buffBase, warEventData.AttackExpirationTime);
				if (!flag)
				{
					this.UpdateStringBasedOnSquad(squad, ref text2);
					text3 = Service.Get<Lang>().Get("WAR_ATTACK_BUFF_BASE_STARTED", new object[]
					{
						this.GetWarBuffDisplayName(warEventData.BuffBaseUid),
						text2
					});
				}
				Service.Get<EventManager>().SendEvent(EventId.WarAttackBuffBaseStarted, warEventData.BuffBaseUid);
				break;
			}
			case SquadMsgType.WarBuffBaseAttackComplete:
			{
				SquadWarSquadData squadWarSquadData = null;
				SquadWarParticipantState participant = this.FindParticipantState(text, out squadWarSquadData);
				this.DeductTurnFromParticipant(participant);
				SquadWarBuffBaseData buffBase2 = this.FindBuffBaseData(warEventData.BuffBaseUid);
				this.UpdateAttackStateForBuffBase(buffBase2, 0u);
				if (warEventData.BuffBaseCaptured)
				{
					this.CaptureBuffBase(buffBase2, squadWarSquadData);
					if (!flag)
					{
						this.UpdateStringBasedOnSquad(squadWarSquadData, ref text2);
						text3 = Service.Get<Lang>().Get("WAR_BUFF_BASE_CAPTURED", new object[]
						{
							this.GetWarBuffDisplayName(warEventData.BuffBaseUid),
							text2
						});
					}
				}
				Service.Get<EventManager>().SendEvent(EventId.WarAttackBuffBaseCompleted, warEventData.BuffBaseUid);
				break;
			}
			case SquadMsgType.WarPlayerAttackStart:
			{
				SquadWarSquadData squad2 = null;
				this.FindParticipantState(text, out squad2);
				SquadWarSquadData squad3 = null;
				SquadWarParticipantState participant2 = this.FindParticipantState(opponentId, out squad3);
				this.UpdateUnderAttackStateForParticipant(participant2, warEventData.AttackExpirationTime);
				if (!flag)
				{
					this.UpdateStringBasedOnSquad(squad3, ref opponentName);
					this.UpdateStringBasedOnSquad(squad2, ref text2);
					text3 = Service.Get<Lang>().Get("WAR_ATTACK_PLAYER_STARTED", new object[]
					{
						opponentName,
						text2
					});
				}
				Service.Get<EventManager>().SendEvent(EventId.WarAttackPlayerStarted, opponentId);
				break;
			}
			case SquadMsgType.WarPlayerAttackComplete:
			{
				SquadWarSquadData squadWarSquadData2 = null;
				SquadWarParticipantState squadWarParticipantState = this.FindParticipantState(text, out squadWarSquadData2);
				this.DeductTurnFromParticipant(squadWarParticipantState);
				SquadWarSquadData squad4 = null;
				SquadWarParticipantState squadWarParticipantState2 = this.FindParticipantState(opponentId, out squad4);
				this.UpdateUnderAttackStateForParticipant(squadWarParticipantState2, 0u);
				this.ExchangeVictoryPoints(squadWarParticipantState, squadWarParticipantState2, squadWarSquadData2, warEventData.VictoryPointsTaken);
				if (!flag)
				{
					this.UpdateStringBasedOnSquad(squad4, ref opponentName);
					this.UpdateStringBasedOnSquad(squadWarSquadData2, ref text2);
					text3 = Service.Get<Lang>().Get("WAR_PLAYER_ATTACKED", new object[]
					{
						opponentName,
						text2,
						warEventData.StarsEarned,
						warEventData.VictoryPointsTaken
					});
				}
				Service.Get<EventManager>().SendEvent(EventId.WarAttackPlayerCompleted, opponentId);
				break;
			}
			case SquadMsgType.WarEnded:
				this.controller.UpdateCurrentSquadWar();
				break;
			}
			if (text3 != null && Service.IsSet<UXController>())
			{
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(text3);
			}
		}

		private void UpdateStringBasedOnSquad(SquadWarSquadData squad, ref string text)
		{
			if (squad != null)
			{
				if (squad.Faction == FactionType.Empire)
				{
					text = UXUtils.WrapTextInColor(text, "c0d0ff");
					return;
				}
				if (squad.Faction == FactionType.Rebel)
				{
					text = UXUtils.WrapTextInColor(text, "f0dfc1");
				}
			}
		}

		public void EnterWarBoardMode()
		{
			Service.Get<GameStateMachine>().SetState(new WarBoardState());
		}

		public void ExitWarBoardMode(TransitionCompleteDelegate callback)
		{
			HomeMapDataLoader dataLoader = Service.Get<HomeMapDataLoader>();
			HomeState.GoToHomeState(callback, true);
			this.SetupWarBoardLightingRemoval(dataLoader, true);
		}

		public void SetupWarBoardLightingRemoval(HomeMapDataLoader dataLoader, bool softWipeTransition)
		{
			LightingEffectsController lightingEffectsController = Service.Get<LightingEffectsController>();
			EventId triggerEvent = EventId.WorldOutTransitionComplete;
			if (softWipeTransition)
			{
				triggerEvent = EventId.WipeCameraSnapshotTaken;
			}
			lightingEffectsController.LightingEffects.SetupDelayedLightingOverrideRemoval(triggerEvent);
		}

		public void SetupWarBoardLighting(HomeMapDataLoader dataLoader, bool softWipeTransition)
		{
			string warBoardLightingAsset = dataLoader.GetPlanetData().WarBoardLightingAsset;
			LightingEffectsController lightingEffectsController = Service.Get<LightingEffectsController>();
			EventId triggerEvent = EventId.WorldOutTransitionComplete;
			if (softWipeTransition)
			{
				triggerEvent = EventId.WipeCameraSnapshotTaken;
			}
			lightingEffectsController.LightingEffects.ApplyDelayedLightingDataOverride(triggerEvent, warBoardLightingAsset);
		}

		private void UpdateUnderAttackStateForParticipant(SquadWarParticipantState participant, uint expirationTime)
		{
			if (participant != null)
			{
				participant.DefendingAttackExpirationDate = expirationTime;
			}
		}

		private void UpdateAttackStateForBuffBase(SquadWarBuffBaseData buffBase, uint expirationTime)
		{
			if (buffBase != null)
			{
				buffBase.AttackExpirationDate = expirationTime;
			}
		}

		private void DeductTurnFromParticipant(SquadWarParticipantState participant)
		{
			if (participant != null && participant.TurnsLeft > 0)
			{
				participant.TurnsLeft--;
			}
		}

		private void ExchangeVictoryPoints(SquadWarParticipantState attacker, SquadWarParticipantState defender, SquadWarSquadData attackerSquad, int victoryPoints)
		{
			if (attacker != null)
			{
				attacker.Score += victoryPoints;
			}
			if (defender != null && defender.VictoryPointsLeft >= victoryPoints)
			{
				defender.VictoryPointsLeft -= victoryPoints;
			}
			Service.Get<EventManager>().SendEvent(EventId.WarVictoryPointsUpdated, attacker);
			Service.Get<EventManager>().SendEvent(EventId.WarVictoryPointsUpdated, defender);
		}

		private void CaptureBuffBase(SquadWarBuffBaseData buffBase, SquadWarSquadData attackerSquad)
		{
			if (buffBase == null)
			{
				return;
			}
			if (attackerSquad != null)
			{
				buffBase.OwnerId = attackerSquad.SquadId;
			}
			WarBuffVO warBuffVO = Service.Get<IDataController>().Get<WarBuffVO>(buffBase.BuffBaseId);
			string[] array = null;
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			if (faction != FactionType.Empire)
			{
				if (faction == FactionType.Rebel)
				{
					array = warBuffVO.RebelBattlesByLevel;
				}
			}
			else
			{
				array = warBuffVO.EmpireBattlesByLevel;
			}
			if (array != null && buffBase.BaseLevel < array.Length - 1)
			{
				buffBase.BaseLevel++;
			}
			Service.Get<EventManager>().SendEvent(EventId.WarBuffBaseCaptured, buffBase);
		}

		private SquadWarParticipantState FindParticipantState(string playerId)
		{
			SquadWarSquadData squadWarSquadData;
			return this.FindParticipantState(playerId, out squadWarSquadData);
		}

		private SquadWarParticipantState FindParticipantState(string playerId, out SquadWarSquadData squad)
		{
			SquadWarParticipantState result = null;
			squad = null;
			if (this.CurrentSquadWar == null)
			{
				return null;
			}
			for (int i = 0; i < 2; i++)
			{
				SquadWarSquadData squadWarSquadData = this.CurrentSquadWar.Squads[i];
				int j = 0;
				int count = squadWarSquadData.Participants.Count;
				while (j < count)
				{
					if (squadWarSquadData.Participants[j].SquadMemberId == playerId)
					{
						result = squadWarSquadData.Participants[j];
						squad = squadWarSquadData;
						break;
					}
					j++;
				}
			}
			return result;
		}

		private SquadWarBuffBaseData FindBuffBaseData(string buffBaseId)
		{
			if (this.CurrentSquadWar == null)
			{
				return null;
			}
			SquadWarBuffBaseData result = null;
			int i = 0;
			int count = this.CurrentSquadWar.BuffBases.Count;
			while (i < count)
			{
				if (this.CurrentSquadWar.BuffBases[i].BuffBaseId == buffBaseId)
				{
					result = this.CurrentSquadWar.BuffBases[i];
					break;
				}
				i++;
			}
			return result;
		}

		public List<string> GetBuffBasesOwnedBySquad(string squadId)
		{
			if (string.IsNullOrEmpty(squadId))
			{
				return null;
			}
			List<string> list = null;
			int i = 0;
			int count = this.CurrentSquadWar.BuffBases.Count;
			while (i < count)
			{
				if (this.CurrentSquadWar.BuffBases[i].OwnerId == squadId)
				{
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(this.CurrentSquadWar.BuffBases[i].BuffBaseId);
				}
				i++;
			}
			return list;
		}

		public int CalculateVictoryPointsTaken(BattleEntry battle)
		{
			return Math.Max(0, battle.EarnedStars - GameConstants.WAR_VICTORY_POINTS + battle.WarVictoryPointsAvailable);
		}

		private void OnTransitionComplete()
		{
		}

		public bool CanScoutBuffBase(SquadWarBuffBaseData buffBaseData, ref string message)
		{
			message = string.Empty;
			SquadWarStatusType currentStatus = this.GetCurrentStatus();
			if (currentStatus != SquadWarStatusType.PhasePrep && currentStatus != SquadWarStatusType.PhaseAction)
			{
				message = Service.Get<Lang>().Get("WAR_PREVENT_BUFF_BASE_SCOUT_WRONG_PHASE", new object[0]);
				return false;
			}
			SquadWarParticipantState arg_35_0 = this.currentParticipantState;
			return true;
		}

		private void OnGetBuffBaseStatusSuccess(SquadWarBuffBaseResponse response, object cookie)
		{
			ProcessingScreen.Hide();
			Squad currentSquad = this.controller.StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return;
			}
			SquadWarBuffBaseData squadWarBuffBaseData = (response.SquadWarBuffBaseData != null) ? response.SquadWarBuffBaseData : this.FindBuffBaseData(this.currentlyScoutedBuffBaseId);
			if (squadWarBuffBaseData == null)
			{
				return;
			}
			string ownerId = squadWarBuffBaseData.OwnerId;
			int baseLevel = squadWarBuffBaseData.BaseLevel;
			WarBuffVO warBuffVO = Service.Get<IDataController>().Get<WarBuffVO>(this.currentlyScoutedBuffBaseId);
			bool flag = ownerId != null && ownerId == currentSquad.SquadID;
			string[] array = null;
			SquadWarSquadData squadData = this.GetSquadData(SquadWarSquadType.PLAYER_SQUAD);
			SquadWarSquadData squadData2 = this.GetSquadData(SquadWarSquadType.OPPONENT_SQUAD);
			FactionType faction;
			if (flag)
			{
				faction = squadData.Faction;
			}
			else
			{
				faction = squadData2.Faction;
			}
			if (faction != FactionType.Empire)
			{
				if (faction == FactionType.Rebel)
				{
					array = warBuffVO.EmpireBattlesByLevel;
				}
			}
			else
			{
				array = warBuffVO.RebelBattlesByLevel;
			}
			string text = null;
			if (array != null && baseLevel < array.Length)
			{
				text = array[baseLevel];
			}
			if (string.IsNullOrEmpty(text))
			{
				Service.Get<StaRTSLogger>().Error("Can't assign base name for:" + this.currentlyScoutedBuffBaseId);
				return;
			}
			this.LogScoutBIGameAction(squadWarBuffBaseData.BuffBaseId);
			CampaignMissionVO vo = Service.Get<IDataController>().Get<CampaignMissionVO>(text);
			BattleInitializationData data = BattleInitializationData.CreateBuffBaseBattleFromCampaignMissionVO(vo, squadWarBuffBaseData);
			BattleStartState.GoToBattleStartState(data, new TransitionCompleteDelegate(this.OnBattleReady));
		}

		private void OnGetBuffBaseStatusFailure(uint status, object cookie)
		{
			ProcessingScreen.Hide();
			this.ShowPlayerInstructionErrorBasedOnStatus(status, true);
			this.ReleaseCurrentlyScoutedBuffBase();
		}

		private void LogScoutBIGameAction(string id)
		{
			SquadWarStatusType currentStatus = this.GetCurrentStatus();
			if (currentStatus == SquadWarStatusType.PhasePrep || currentStatus == SquadWarStatusType.PhaseAction)
			{
				string text = "null";
				if (this.currentParticipantState != null)
				{
					text = this.currentParticipantState.TurnsLeft.ToString();
				}
				string message = string.Concat(new string[]
				{
					ServerTime.Time.ToString(),
					"|",
					id,
					"|",
					text
				});
				Service.Get<BILoggingController>().TrackGameAction(this.CurrentSquadWar.WarId, "squad_wars_scout", message, null, 1);
			}
		}

		private void OnStartAttackOnBuffBaseFailure(uint status, object cookie)
		{
			this.ShowPlayerInstructionErrorBasedOnStatus(status, true);
			Service.Get<EventManager>().SendEvent(EventId.WarAttackCommandFailed, null);
		}

		private void OnStartAttackOnWarMemberFailure(uint status, object cookie)
		{
			this.ShowPlayerInstructionErrorBasedOnStatus(status, false);
			Service.Get<EventManager>().SendEvent(EventId.WarAttackCommandFailed, null);
		}

		private void OnScoutWarMemberFailure(uint status)
		{
			this.ShowPlayerInstructionErrorBasedOnStatus(status, false);
		}

		private void ShowPlayerInstructionErrorBasedOnStatus(uint status, bool buffBaseBattle)
		{
			bool isPvp = !buffBaseBattle;
			string failureStringIdByStatus = SquadUtils.GetFailureStringIdByStatus(status, isPvp);
			string instructions = Service.Get<Lang>().Get(failureStringIdByStatus, new object[0]);
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(instructions);
		}

		public bool ScoutBuffBase(string buffBaseUid)
		{
			if (this.currentParticipantState != null && !this.EnsurePlayerHasTroops())
			{
				return false;
			}
			if (this.controller.StateManager.GetCurrentSquad() == null)
			{
				return false;
			}
			if (this.FindBuffBaseData(buffBaseUid) == null)
			{
				return false;
			}
			ProcessingScreen.Show();
			this.currentlyScoutedBuffBaseId = buffBaseUid;
			SquadWarGetBuffBaseStatusRequest request = new SquadWarGetBuffBaseStatusRequest(Service.Get<CurrentPlayer>().PlayerId, buffBaseUid);
			SquadWarGetBuffBaseStatusCommand squadWarGetBuffBaseStatusCommand = new SquadWarGetBuffBaseStatusCommand(request);
			squadWarGetBuffBaseStatusCommand.AddSuccessCallback(new AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>.OnSuccessCallback(this.OnGetBuffBaseStatusSuccess));
			squadWarGetBuffBaseStatusCommand.AddFailureCallback(new AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>.OnFailureCallback(this.OnGetBuffBaseStatusFailure));
			Service.Get<ServerAPI>().Sync(squadWarGetBuffBaseStatusCommand);
			return true;
		}

		public void StartAttackOnScoutedBuffBase()
		{
			SquadWarAttackBuffBaseRequest request = new SquadWarAttackBuffBaseRequest(this.currentlyScoutedBuffBaseId);
			SquadWarAttackBuffBaseCommand squadWarAttackBuffBaseCommand = new SquadWarAttackBuffBaseCommand(request);
			squadWarAttackBuffBaseCommand.AddSuccessCallback(new AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>.OnSuccessCallback(this.OnStartAttackOnBuffBaseSuccess));
			squadWarAttackBuffBaseCommand.AddFailureCallback(new AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>.OnFailureCallback(this.OnStartAttackOnBuffBaseFailure));
			Service.Get<ServerAPI>().Sync(squadWarAttackBuffBaseCommand);
		}

		private void OnStartAttackOnBuffBaseSuccess(BattleIdResponse response, object cookie)
		{
			Service.Get<BattleController>().GetCurrentBattle().RecordID = response.BattleId;
			Service.Get<GameStateMachine>().SetState(new BattlePlayState());
		}

		private void BattleInitializationShowBuffs(bool showOpponentBuffs)
		{
			Service.Get<StaRTSLogger>().Debug("BattleInitializationShowBuffs");
			if (this.CurrentSquadWar != null)
			{
				Lang lang = Service.Get<Lang>();
				IDataController dataController = Service.Get<IDataController>();
				MiscElementsManager miscElementsManager = Service.Get<UXController>().MiscElementsManager;
				List<SquadWarBuffBaseData> buffBases = this.CurrentSquadWar.BuffBases;
				Squad currentSquad = this.controller.StateManager.GetCurrentSquad();
				int i = 0;
				int count = buffBases.Count;
				while (i < count)
				{
					SquadWarBuffBaseData squadWarBuffBaseData = buffBases[i];
					WarBuffVO warBuffVO = dataController.Get<WarBuffVO>(squadWarBuffBaseData.BuffBaseId);
					string text = string.Empty;
					if (squadWarBuffBaseData.OwnerId == currentSquad.SquadID)
					{
						text = lang.Get("BUFF_BASE_OWNERSHIP_PLAYER", new object[]
						{
							lang.Get(warBuffVO.BuffStringTitle, new object[0])
						});
					}
					else if (showOpponentBuffs && !string.IsNullOrEmpty(squadWarBuffBaseData.OwnerId))
					{
						text = lang.Get("BUFF_BASE_OWNERSHIP_OPPONENT", new object[]
						{
							lang.Get(warBuffVO.BuffStringTitle, new object[0])
						});
					}
					if (!string.IsNullOrEmpty(text))
					{
						miscElementsManager.ShowPlayerInstructions(text);
					}
					i++;
				}
			}
		}

		public bool CanScoutWarMember(string memberId, ref string message)
		{
			SquadWarParticipantState squadWarParticipantState = this.FindParticipantState(memberId);
			if (squadWarParticipantState == null)
			{
				return false;
			}
			SquadWarStatusType currentStatus = this.GetCurrentStatus();
			SquadWarSquadType participantSquad = this.GetParticipantSquad(squadWarParticipantState.SquadMemberId);
			if (currentStatus != SquadWarStatusType.PhasePrep && currentStatus != SquadWarStatusType.PhaseAction)
			{
				if (participantSquad == SquadWarSquadType.OPPONENT_SQUAD)
				{
					message = Service.Get<Lang>().Get("WAR_PREVENT_ENEMY_SCOUT_WRONG_PHASE", new object[0]);
				}
				else
				{
					message = Service.Get<Lang>().Get("WAR_PREVENT_ALLY_SCOUT_WRONG_PHASE", new object[0]);
				}
				return false;
			}
			SquadWarParticipantState arg_65_0 = this.currentParticipantState;
			return true;
		}

		public bool ScoutWarMember(string memberId)
		{
			if (this.currentParticipantState != null && !this.EnsurePlayerHasTroops())
			{
				return false;
			}
			ProcessingScreen.Show();
			ScoutSquadWarParticipantCommand scoutSquadWarParticipantCommand = new ScoutSquadWarParticipantCommand(new SquadWarParticipantIdRequest
			{
				ParticipantId = memberId
			});
			scoutSquadWarParticipantCommand.AddSuccessCallback(new AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>.OnSuccessCallback(this.OnScoutWarMemberSuccess));
			Service.Get<ServerAPI>().Sync(scoutSquadWarParticipantCommand);
			return true;
		}

		private void OnScoutWarMemberSuccess(SquadMemberWarDataResponse response, object cookie)
		{
			ProcessingScreen.Hide();
			if (SquadUtils.IsNotFatalServerError(response.ScoutingStatus))
			{
				this.OnScoutWarMemberFailure(response.ScoutingStatus);
				return;
			}
			SquadMemberWarData memberWarData = response.MemberWarData;
			SquadWarSquadType participantSquad = this.GetParticipantSquad(memberWarData.SquadMemberId);
			SquadWarSquadData squadData = this.GetSquadData(participantSquad);
			BattleInitializationData battleInitializationData = BattleInitializationData.CreateFromMemberWarData(memberWarData, response.DonatedSquadTroops, response.Champions, squadData.Faction, squadData.SquadId, response.Equipment);
			TransitionCompleteDelegate onComplete = new TransitionCompleteDelegate(this.OnBattleReady);
			if (battleInitializationData.Attacker.PlayerFaction == battleInitializationData.Defender.PlayerFaction)
			{
				onComplete = new TransitionCompleteDelegate(this.OnSameFactionScoutReady);
			}
			this.LogScoutBIGameAction(memberWarData.SquadMemberId);
			BattleStartState.GoToBattleStartState(battleInitializationData, onComplete);
		}

		private bool EnsurePlayerHasTroops()
		{
			if (!GameUtils.HasAvailableTroops(false, null))
			{
				Lang lang = Service.Get<Lang>();
				AlertScreen.ShowModal(false, lang.Get("NOT_ENOUGH_TROOPS_TITLE", new object[0]), lang.Get("NOT_ENOUGH_TROOPS_FOR_ATTACK", new object[0]), null, null);
				Service.Get<EventManager>().SendEvent(EventId.UISquadWarScreen, new ActionMessageBIData("PvP_or_buffbase", "no_troops"));
				return false;
			}
			return true;
		}

		public void StartAttackOnScoutedWarMember()
		{
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			SquadWarAttackPlayerStartRequest request = new SquadWarAttackPlayerStartRequest(currentBattle.Defender.PlayerId);
			SquadWarAttackPlayerStartCommand squadWarAttackPlayerStartCommand = new SquadWarAttackPlayerStartCommand(request);
			squadWarAttackPlayerStartCommand.AddSuccessCallback(new AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>.OnSuccessCallback(this.OnStartAttackOnWarMemberSuccess));
			squadWarAttackPlayerStartCommand.AddFailureCallback(new AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>.OnFailureCallback(this.OnStartAttackOnWarMemberFailure));
			Service.Get<ServerAPI>().Sync(squadWarAttackPlayerStartCommand);
		}

		private void OnStartAttackOnWarMemberSuccess(BattleIdResponse response, object cookie)
		{
			Service.Get<BattleController>().GetCurrentBattle().RecordID = response.BattleId;
			Service.Get<GameStateMachine>().SetState(new BattlePlayState());
		}

		private void OnBattleReady()
		{
			this.BattleInitializationShowBuffs(true);
		}

		private void OnSameFactionScoutReady()
		{
			this.BattleInitializationShowBuffs(false);
		}

		public string GetCurrentlyScoutedBuffBaseId()
		{
			return this.currentlyScoutedBuffBaseId;
		}

		public void ReleaseCurrentlyScoutedBuffBase()
		{
			this.currentlyScoutedBuffBaseId = null;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.ContractCompleted)
			{
				if (id != EventId.WorldInTransitionComplete)
				{
					if (id == EventId.ContractCompleted)
					{
						if (this.currentParticipantState != null)
						{
							ContractEventData contractEventData = cookie as ContractEventData;
							ContractType contractType = contractEventData.Contract.ContractTO.ContractType;
							if (contractType == ContractType.Upgrade)
							{
								BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(contractEventData.Contract.ProductUid);
								if (buildingTypeVO.Type == BuildingType.Squad)
								{
									this.controller.UpdateCurrentSquad();
								}
							}
						}
					}
				}
				else
				{
					this.CheckForWarboardForceExit();
					Service.Get<EventManager>().UnregisterObserver(this, EventId.WorldInTransitionComplete);
				}
			}
			else if (id != EventId.WarPhaseChanged)
			{
				if (id == EventId.WarLaunchFlow)
				{
					this.LaunchSquadWarFlow();
				}
			}
			else if ((SquadWarStatusType)cookie == SquadWarStatusType.PhaseOpen)
			{
				this.EndSquadWar();
			}
			return EatResponse.NotEaten;
		}

		public bool WarExists()
		{
			return this.CurrentSquadWar != null && !string.IsNullOrEmpty(this.CurrentSquadWar.WarId);
		}

		public bool CanStartSquadWar()
		{
			SquadRole role = this.controller.StateManager.Role;
			return !this.WarExists() && (role == SquadRole.Officer || role == SquadRole.Owner);
		}

		public bool IsMemberInWarParty(string memberId)
		{
			return this.warParty.Contains(memberId);
		}

		public bool IsSquadMemberInWarOrMatchmaking(SquadMember squadMember)
		{
			return squadMember.WarParty == 1;
		}

		public int GetWarPartyCount()
		{
			return this.warParty.Count;
		}

		public bool IsEligibleForWarParty(SquadMember squadMember)
		{
			bool flag = squadMember.HQLevel >= GameConstants.WAR_PARTICIPANT_MIN_LEVEL;
			bool flag2 = this.warParty.Count < GameConstants.WAR_PARTICIPANT_COUNT;
			bool flag3 = !this.warParty.Contains(squadMember.MemberID);
			return flag & flag2 & flag3;
		}

		public bool WarPartyAdd(SquadMember squadMember)
		{
			if (this.IsEligibleForWarParty(squadMember))
			{
				this.warParty.Add(squadMember.MemberID);
				return true;
			}
			return false;
		}

		public bool WarPartyRemove(string memberId)
		{
			return !(memberId == Service.Get<CurrentPlayer>().PlayerId) && this.warParty.Remove(memberId);
		}

		public void StartMatchMakingPreparation()
		{
			if (!GameConstants.WAR_ALLOW_MATCHMAKING)
			{
				Lang lang = Service.Get<Lang>();
				AlertScreen.ShowModal(false, lang.Get("WAR_DISABLED_TITLE", new object[0]), lang.Get("WAR_DISABLED_BODY", new object[0]), null, null);
				return;
			}
			BuildingLookupController blc = Service.Get<BuildingLookupController>();
			if (!SquadUtils.CanStartMatchmakingPrep(this.controller, blc))
			{
				return;
			}
			this.warParty.Clear();
			this.MatchMakingPrepMode = true;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Squad currentSquad = this.controller.StateManager.GetCurrentSquad();
			SquadMember squadMemberById = SquadUtils.GetSquadMemberById(currentSquad, currentPlayer.PlayerId);
			this.WarPartyAdd(squadMemberById);
			List<SquadMember> memberList = this.controller.StateManager.GetCurrentSquad().MemberList;
			List<SquadMember> list = new List<SquadMember>();
			int i = 0;
			int count = memberList.Count;
			while (i < count)
			{
				if (this.IsEligibleForWarParty(memberList[i]))
				{
					list.Add(memberList[i]);
				}
				i++;
			}
			list.Sort(new Comparison<SquadMember>(this.SortPotentialWarParty));
			int num = 0;
			int count2 = list.Count;
			while (num < count2 && this.warParty.Count < GameConstants.WAR_PARTICIPANT_COUNT)
			{
				this.WarPartyAdd(list[num]);
				num++;
			}
			this.controller.StateManager.SquadScreenState = SquadScreenState.Members;
			Service.Get<UXController>().HUD.SlideSquadScreenOpen();
		}

		private int SortPotentialWarParty(SquadMember firstMember, SquadMember secondMember)
		{
			if (firstMember != null && secondMember == null)
			{
				return 1;
			}
			if (firstMember == null && secondMember != null)
			{
				return -1;
			}
			if (firstMember == null && secondMember == null)
			{
				return 0;
			}
			if (secondMember.BaseScore > firstMember.BaseScore)
			{
				return 1;
			}
			if (secondMember.BaseScore == firstMember.BaseScore)
			{
				if (secondMember.HQLevel > firstMember.HQLevel)
				{
					return 1;
				}
				if (secondMember.HQLevel == firstMember.HQLevel)
				{
					return secondMember.Score.CompareTo(firstMember.Score);
				}
			}
			return -1;
		}

		public bool IsCurrentSquadMatchmaking()
		{
			return this.controller.StateManager.GetCurrentSquad().WarSignUpTime != 0;
		}

		public void StartMatchMaking(bool allowSameFaction)
		{
			this.MatchMakingPrepMode = false;
			this.CurrentSquadWar = null;
			this.controller.StateManager.GetCurrentSquad().WarSignUpTime = (int)Service.Get<ServerAPI>().ServerTime;
			SquadMsg message = SquadMsgUtils.CreateStartMatchmakingMessage(this.warParty, allowSameFaction);
			this.controller.TakeAction(message);
		}

		public void CancelEnteringMatchmaking()
		{
			this.MatchMakingPrepMode = false;
			this.warParty.Clear();
		}

		public void CancelMatchMaking()
		{
			Squad currentSquad = this.controller.StateManager.GetCurrentSquad();
			int i = 0;
			int count = currentSquad.MemberList.Count;
			while (i < count)
			{
				currentSquad.MemberList[i].WarParty = 0;
				i++;
			}
			this.controller.StateManager.GetCurrentSquad().WarSignUpTime = 0;
			this.warParty.Clear();
		}

		public void CancelMatchMakingTakeAction()
		{
			this.CancelMatchMaking();
			SquadMsg message = SquadMsgUtils.CreateCancelMatchmakingMessage();
			this.controller.TakeAction(message);
		}

		public void OnWarMatchMakingBegin()
		{
			this.controller.StateManager.GetCurrentSquad().WarSignUpTime = (int)Service.Get<ServerAPI>().ServerTime;
			if (this.MatchMakingPrepMode)
			{
				this.CancelEnteringMatchmaking();
			}
			IGameState gameState = Service.Get<GameStateMachine>().CurrentState as IGameState;
			if (this.WarExists() && gameState is WarBoardState)
			{
				this.CheckForWarboardForceExit();
				return;
			}
			string instructions = Service.Get<Lang>().Get("WAR_MATCHMAKING_STARTED", new object[0]);
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(instructions);
			this.EndSquadWar();
		}

		public void OnCelebrationScreenClosed(object result, object cookie)
		{
			this.CheckForWarboardForceExit();
		}

		public void CheckForWarboardForceExit()
		{
			ScreenController screenController = Service.Get<ScreenController>();
			if (!this.WarExists() || !this.IsCurrentSquadMatchmaking())
			{
				return;
			}
			if (Service.Get<WorldTransitioner>().IsTransitioning())
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.WorldInTransitionComplete);
				return;
			}
			SquadWarEndCelebrationScreen highestLevelScreen = screenController.GetHighestLevelScreen<SquadWarEndCelebrationScreen>();
			if (highestLevelScreen != null && !highestLevelScreen.IsClosing)
			{
				return;
			}
			InventoryCrateCollectionScreen highestLevelScreen2 = screenController.GetHighestLevelScreen<InventoryCrateCollectionScreen>();
			if (highestLevelScreen2 != null && !highestLevelScreen2.IsClosing)
			{
				return;
			}
			if (Service.Get<ScreenController>().GetHighestLevelScreen<AlertScreen>() != null)
			{
				return;
			}
			IGameState gameState = Service.Get<GameStateMachine>().CurrentState as IGameState;
			if (!(gameState is WarBoardState))
			{
				return;
			}
			this.EndSquadWar();
			Service.Get<UXController>().HUD.SlideSquadScreenClosedInstantly();
			Lang lang = Service.Get<Lang>();
			AlertScreen.ShowModal(false, lang.Get("WAR_BOARD_MATCHMAKING_EXIT_TITLE", new object[0]), lang.Get("WAR_BOARD_MATCHMAKING_EXIT_BODY", new object[0]), null, null, true);
		}

		public void ShowInfoScreen(UXButton button)
		{
			SquadWarInfoScreen screen = new SquadWarInfoScreen(-1);
			Service.Get<ScreenController>().AddScreen(screen);
		}

		private void LaunchSquadWarFlow()
		{
			if (this.controller.StateManager.GetCurrentSquad() == null)
			{
				SquadWarStartScreen screen = new SquadWarStartScreen();
				Service.Get<ScreenController>().AddScreen(screen);
				return;
			}
			if (Service.Get<BuildingLookupController>().GetHighestLevelHQ() < GameConstants.WAR_PARTICIPANT_MIN_LEVEL)
			{
				Lang lang = Service.Get<Lang>();
				AlertScreen.ShowModal(false, lang.Get("WAR_BOARD_INSUFFICIENT_LEVEL_TITLE", new object[0]), lang.Get("WAR_BOARD_INSUFFICIENT_LEVEL_BODY", new object[0]), new OnScreenModalResult(this.OnAcceptInsufficientLevel), null);
				return;
			}
			switch (this.GetCurrentStatus())
			{
			case SquadWarStatusType.PhaseOpen:
			{
				if (this.IsCurrentSquadMatchmaking())
				{
					SquadWarMatchMakeScreen screen2 = new SquadWarMatchMakeScreen();
					Service.Get<ScreenController>().AddScreen(screen2);
					return;
				}
				SquadWarStartScreen screen3 = new SquadWarStartScreen();
				Service.Get<ScreenController>().AddScreen(screen3);
				return;
			}
			case SquadWarStatusType.PhasePrep:
			case SquadWarStatusType.PhasePrepGrace:
			case SquadWarStatusType.PhaseAction:
			case SquadWarStatusType.PhaseActionGrace:
			case SquadWarStatusType.PhaseCooldown:
				this.EnterWarBoardMode();
				return;
			default:
				return;
			}
		}

		private void OnAcceptInsufficientLevel(object result, object cookie)
		{
			Service.Get<UXController>().HUD.RefreshView();
		}

		public void StartTranstionFromWarBaseToWarBoard()
		{
			HomeMapDataLoader homeMapDataLoader = Service.Get<HomeMapDataLoader>();
			Service.Get<WorldTransitioner>().StartTransition(new WarbaseToWarboardTransition(new WarBoardState(), homeMapDataLoader, null, false, false));
			this.SetupWarBoardLighting(homeMapDataLoader, false);
		}

		public bool IsAlliedBuffBase(SquadWarBuffBaseData baseData)
		{
			SquadWarSquadData squadData = this.GetSquadData(SquadWarSquadType.PLAYER_SQUAD);
			string ownerId = baseData.OwnerId;
			return ownerId == squadData.SquadId;
		}

		public string GetWarBuffDisplayName(string buffUid)
		{
			Lang lang = Service.Get<Lang>();
			IDataController dataController = Service.Get<IDataController>();
			WarBuffVO optional = dataController.GetOptional<WarBuffVO>(buffUid);
			if (optional != null)
			{
				return lang.Get(optional.BuffBaseName, new object[0]);
			}
			return string.Empty;
		}

		public void Destroy()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			this.warParty.Clear();
			this.warParty = null;
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.WarLaunchFlow);
			eventManager.UnregisterObserver(this, EventId.ContractCompleted);
			eventManager.UnregisterObserver(this, EventId.WorldInTransitionComplete);
			eventManager.UnregisterObserver(this, EventId.WarPhaseChanged);
		}

		protected internal SquadWarManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).BattleInitializationShowBuffs(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CalculateVictoryPointsTaken((BattleEntry)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CanAttackBuffBase((SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CanAttackCurrentlyScoutedBuffBase());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CanAttackCurrentlyScoutedOpponent());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CanAttackOpponent((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CancelEnteringMatchmaking();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CancelMatchMaking();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CancelMatchMakingTakeAction();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CanStartSquadWar());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CaptureBuffBase((SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(*args), (SquadWarSquadData)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CheckForWarboardForceExit();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ClaimCurrentPlayerCurrentWarReward());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ClaimSquadWarReward(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ClearSquadWarData();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).CloseWarEndedScreen();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).DeductTurnFromParticipant((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).EndSquadWar();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).EnsurePlayerHasTroops());
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).EnterWarBoardMode();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ExchangeVictoryPoints((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(*args), (SquadWarParticipantState)GCHandledObjects.GCHandleToObject(args[1]), (SquadWarSquadData)GCHandledObjects.GCHandleToObject(args[2]), *(int*)(args + 3));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ExitWarBoardMode((TransitionCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).FindBuffBaseData(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).FindParticipantState(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).EnableSquadWarMode);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetBuffBaseData(*(int*)args));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetBuffBasesOwnedBySquad(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentlyScoutedBuffBaseData());
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentlyScoutedBuffBaseId());
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentMemberWarData());
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentOpponentState());
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentParticipantState());
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentPlayerCurrentWarReward());
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentSquadScore((SquadWarSquadType)(*(int*)args)));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentSquadWarResult());
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentStatus());
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentWarScheduleData());
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetParticipantSquad(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetParticipantState(*(int*)args, (SquadWarSquadType)(*(int*)(args + 1))));
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetSquadData((SquadWarSquadType)(*(int*)args)));
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetSquadData(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetWarBuffDisplayName(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).GetWarPartyCount());
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).HandleWarEventMsg((SquadMsg)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).IsAlliedBuffBase((SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).IsBuffBaseUnderAttack((SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).IsCurrentlyScoutingOwnedBuffBase());
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).IsCurrentSquadMatchmaking());
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).IsEligibleForWarParty((SquadMember)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).IsMemberInWarParty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).IsOpponentUnderAttack((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).IsSquadMemberInWarOrMatchmaking((SquadMember)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).IsTimeWithinCurrentSquadWarPhase(*(int*)args));
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).LaunchSquadWarFlow();
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).LogScoutBIGameAction(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnAcceptInsufficientLevel(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnBattleReady();
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnCelebrationScreenClosed(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnCrateScreenClosed(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnGetBuffBaseStatusSuccess((SquadWarBuffBaseResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnSameFactionScoutReady();
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnScoutWarMemberSuccess((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnSquadWarClaimRewardSuccess((CrateDataResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnStartAttackOnBuffBaseSuccess((BattleIdResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnStartAttackOnWarMemberSuccess((BattleIdResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnTransitionComplete();
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).OnWarMatchMakingBegin();
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ReleaseCurrentlyScoutedBuffBase();
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).RemoveCurrentPlayerCurrentWarReward());
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ScoutBuffBase(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ScoutWarMember(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).EnableSquadWarMode = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).SetupWarBoardLighting((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).SetupWarBoardLightingRemoval((HomeMapDataLoader)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ShowInfoScreen((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ShowWarEndedScreen());
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).ShowWarRewardSupplyCrateAnimation((SquadWarRewardData)GCHandledObjects.GCHandleToObject(*args), (CrateData)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).SortPotentialWarParty((SquadMember)GCHandledObjects.GCHandleToObject(*args), (SquadMember)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).StartAttackOnScoutedBuffBase();
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).StartAttackOnScoutedWarMember();
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).StartMatchMaking(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).StartMatchMakingPreparation();
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).StartTranstionFromWarBaseToWarBoard();
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentMemberWarData((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).UpdateSquadWar((SquadWarData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).WarExists());
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).WarPartyAdd((SquadMember)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarManager)GCHandledObjects.GCHandleToObject(instance)).WarPartyRemove(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}
