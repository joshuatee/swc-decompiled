using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Commands.Tournament;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class TournamentController : IEventObserver, IViewFrameTimeObserver
	{
		public delegate void PlayerRankUpdatedCallback(TournamentRank oldRank, TournamentRank rank, string tournamentUId);

		private static List<TournamentTierVO> tiers;

		public const string LANG_LOOT_BONUS = "LOOT_BONUS";

		public const string VICTORY_BONUS_MESSAGE = "VICTORY_BONUS_MESSAGE";

		private const char TOURNAMENT_PREFS_DELIMITER = '|';

		private const string PARAM_TOURNAMENT_ID = "tournamentId";

		private const string PARAM_CALLBACK = "callback";

		private Dictionary<string, bool> viewedTournaments;

		public TournamentVO CurrentPlanetActiveTournament;

		public TournamentVO NextExpiringConflict;

		private TournamentProgress progress;

		private CurrentPlayer currentPlayer;

		private bool conflictPointPopupShown;

		private bool showConflictPopup;

		public TournamentVO NotifyEndForTournamentVO
		{
			get;
			set;
		}

		public TournamentController()
		{
			TournamentController.LoadTierData();
			Service.Set<TournamentController>(this);
			this.viewedTournaments = new Dictionary<string, bool>();
			this.currentPlayer = Service.Get<CurrentPlayer>();
			this.currentPlayer.TournamentProgress.RemoveMissingTournamentData();
			this.LoadTournamentViewedData();
			this.UpdateTournamentsData(false);
			Service.Get<EventManager>().RegisterObserver(this, EventId.PlanetRelocate, EventPriority.Default);
			this.conflictPointPopupShown = (Service.Get<SharedPlayerPrefs>().GetPref<int>("ConfPtsPopup") >= 1);
			this.UpdatePlayerRanks();
		}

		public void SetConflictPopupIsShown()
		{
			this.conflictPointPopupShown = true;
			Service.Get<SharedPlayerPrefs>().SetPref("ConfPtsPopup", "1");
		}

		public void LoadTournamentViewedData()
		{
			string pref = Service.Get<ServerPlayerPrefs>().GetPref(ServerPref.TournamentViewed);
			string[] array = pref.Split(new char[]
			{
				'|'
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]) && !this.viewedTournaments.ContainsKey(array[i]))
				{
					this.viewedTournaments.Add(array[i], true);
				}
			}
		}

		public int NumberOfTournamentsNotViewed()
		{
			int num = 0;
			bool flag = Service.Get<BuildingLookupController>().HasNavigationCenter();
			List<TournamentVO> allActiveTournaments = TournamentController.GetAllActiveTournaments();
			for (int i = 0; i < allActiveTournaments.Count; i++)
			{
				TournamentVO tournamentVO = allActiveTournaments[i];
				if (tournamentVO != null && this.viewedTournaments != null && !this.viewedTournaments.ContainsKey(tournamentVO.Uid) && GameUtils.ConflictStartsInBadgePeriod(tournamentVO) && (flag || this.currentPlayer.PlanetId == tournamentVO.PlanetId))
				{
					num++;
				}
			}
			return num;
		}

		public void OnGalaxyViewed()
		{
			bool flag = false;
			List<TournamentVO> allActiveTournaments = TournamentController.GetAllActiveTournaments();
			for (int i = 0; i < allActiveTournaments.Count; i++)
			{
				TournamentVO tournamentVO = allActiveTournaments[i];
				if (tournamentVO != null && this.viewedTournaments != null && GameUtils.ConflictStartsInBadgePeriod(tournamentVO) && !this.viewedTournaments.ContainsKey(tournamentVO.Uid))
				{
					this.viewedTournaments.Add(tournamentVO.Uid, true);
					flag = true;
				}
			}
			if (flag)
			{
				this.UpdateAndSyncTournamentViewedData();
			}
		}

		public void OnTournamentViewed(string tournamentId)
		{
			if (!string.IsNullOrEmpty(tournamentId) && this.viewedTournaments != null && !this.viewedTournaments.ContainsKey(tournamentId))
			{
				this.viewedTournaments.Add(tournamentId, true);
				this.UpdateAndSyncTournamentViewedData();
			}
		}

		public void UpdateAndSyncTournamentViewedData()
		{
			string text = "";
			foreach (KeyValuePair<string, bool> current in this.viewedTournaments)
			{
				if (current.get_Value())
				{
					text = text + current.get_Key() + "|";
				}
			}
			Service.Get<ServerPlayerPrefs>().SetPref(ServerPref.TournamentViewed, text);
			Service.Get<ServerAPI>().Enqueue(new SetPrefsCommand(false));
		}

		private TournamentVO GetActiveTournamentOnCurrentPlanet()
		{
			return TournamentController.GetActiveTournamentOnPlanet(this.currentPlayer.PlanetId);
		}

		public void UpdateTournamentsData(bool canShowDialog)
		{
			this.FindNextExpiringConflict();
			this.CurrentPlanetActiveTournament = this.GetActiveTournamentOnCurrentPlanet();
			List<TournamentVO> tournamentVOs = TournamentController.GetTournamentVOs(TournamentFilter.All);
			for (int i = 0; i < tournamentVOs.Count; i++)
			{
				TournamentVO tournamentVO = tournamentVOs[i];
				TimedEventState state = TimedEventUtils.GetState(tournamentVO);
				if (tournamentVO != null && state != TimedEventState.Live && state != TimedEventState.Upcoming && this.IsPlayerInTournament(tournamentVO) && !this.IsTournamentRedeemed(tournamentVO))
				{
					this.RedeemTournaments(canShowDialog);
					return;
				}
			}
		}

		private void FindNextExpiringConflict()
		{
			this.NextExpiringConflict = null;
			uint num = 4294967295u;
			List<TournamentVO> tournamentVOs = TournamentController.GetTournamentVOs(TournamentFilter.Live);
			for (int i = 0; i < tournamentVOs.Count; i++)
			{
				TournamentVO tournamentVO = tournamentVOs[i];
				if (tournamentVO != null && !this.IsTournamentRedeemed(tournamentVO) && (long)tournamentVO.EndTimestamp < (long)((ulong)num))
				{
					num = (uint)tournamentVO.EndTimestamp;
					this.NextExpiringConflict = tournamentVO;
				}
			}
			if (this.NextExpiringConflict != null)
			{
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			}
		}

		public void OnViewFrameTime(float dt)
		{
			if (this.NextExpiringConflict != null && TimedEventUtils.GetState(this.NextExpiringConflict) == TimedEventState.Closing)
			{
				Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
				this.UpdateTournamentsData(true);
			}
		}

		public bool IsPlayerInTournament(TournamentVO tournamentVO)
		{
			bool result = false;
			if (tournamentVO != null)
			{
				result = this.currentPlayer.TournamentProgress.HasTournament(tournamentVO);
			}
			return result;
		}

		public int GetTournamentRating(TournamentVO tournamentVO)
		{
			int result = 0;
			if (tournamentVO != null)
			{
				Tournament tournament = this.currentPlayer.TournamentProgress.GetTournament(tournamentVO.Uid);
				if (tournament != null)
				{
					result = tournament.Rating;
				}
				else
				{
					result = tournamentVO.StartingRating;
				}
			}
			return result;
		}

		private void EnterTournament(TournamentVO tournamentVO)
		{
			if (tournamentVO == null || tournamentVO.PlanetId != this.currentPlayer.PlanetId)
			{
				return;
			}
			Tournament tournament = new Tournament();
			tournament.Uid = tournamentVO.Uid;
			tournament.Rating = tournamentVO.StartingRating;
			tournament.RedeemedRewards.Clear();
			tournament.Collected = false;
			this.currentPlayer.TournamentProgress.AddTournament(tournamentVO.Uid, tournament);
			Service.Get<EventManager>().SendEvent(EventId.TournamentEntered, tournamentVO);
		}

		public bool ShouldShowTournamentLeaderboard(TournamentVO tournamentVO)
		{
			if (tournamentVO != null)
			{
				TimedEventState state = TimedEventUtils.GetState(tournamentVO);
				return state == TimedEventState.Live || state == TimedEventState.Closing;
			}
			return false;
		}

		public bool IsThisTournamentLive(TournamentVO tournamentVO)
		{
			return tournamentVO != null && TimedEventUtils.GetState(tournamentVO) == TimedEventState.Live;
		}

		public bool IsBattleInCurrentTournament(BattleEntry battleEntry)
		{
			bool result = false;
			if (this.CurrentPlanetActiveTournament != null)
			{
				int endBattleServerTime = (int)battleEntry.EndBattleServerTime;
				result = (endBattleServerTime >= this.CurrentPlanetActiveTournament.StartTimestamp && endBattleServerTime <= this.CurrentPlanetActiveTournament.EndTimestamp);
			}
			return result;
		}

		public void EnterPlanetConflict()
		{
			if (this.CurrentPlanetActiveTournament != null && this.IsThisTournamentLive(this.CurrentPlanetActiveTournament) && !this.IsPlayerInTournament(this.CurrentPlanetActiveTournament))
			{
				this.EnterTournament(this.CurrentPlanetActiveTournament);
			}
		}

		public void OnPvpBattleComplete(Tournament tournamentDataFromServer, int ratingDelta)
		{
			if (this.IsThisTournamentLive(this.CurrentPlanetActiveTournament))
			{
				Tournament tournament = this.currentPlayer.TournamentProgress.GetTournament(this.CurrentPlanetActiveTournament.Uid);
				if (tournament != null)
				{
					tournament.Sync(tournamentDataFromServer);
					if (!this.conflictPointPopupShown && ratingDelta > 0)
					{
						this.showConflictPopup = true;
						Service.Get<EventManager>().RegisterObserver(this, EventId.WorldInTransitionComplete, EventPriority.Default);
					}
					string pref = Service.Get<ServerPlayerPrefs>().GetPref(ServerPref.TournamentTierChangeTimeViewed);
					uint num = Convert.ToUInt32(pref, CultureInfo.InvariantCulture);
					uint num2 = (ServerTime.Time - num) / 3600u;
					if (num2 >= GameConstants.TOURNAMENT_TIER_CHANGE_VIEW_THROTTLE)
					{
						this.UpdatePlayerRank(new TournamentController.PlayerRankUpdatedCallback(this.CheckForTierChangeAfterBattle), this.CurrentPlanetActiveTournament);
						return;
					}
				}
				else
				{
					Service.Get<StaRTSLogger>().Warn("Conflict is live but we are missing progress for it (id):" + this.CurrentPlanetActiveTournament.Uid);
				}
			}
		}

		private void CheckForTierChangeAfterBattle(TournamentRank oldRank, TournamentRank rank, string tournamentUID)
		{
			if (oldRank != null && !string.IsNullOrEmpty(oldRank.TierUid))
			{
				IDataController dataController = Service.Get<IDataController>();
				TournamentTierVO tournamentTierVO = dataController.Get<TournamentTierVO>(oldRank.TierUid);
				TournamentTierVO tournamentTierVO2 = dataController.Get<TournamentTierVO>(rank.TierUid);
				if (tournamentTierVO2.Percentage < tournamentTierVO.Percentage)
				{
					Service.Get<EventManager>().RegisterObserver(this, EventId.WorldInTransitionComplete, EventPriority.Default);
				}
			}
		}

		private void UpdatePlayerRanks()
		{
			GetRanksCommand getRanksCommand = new GetRanksCommand(new PlayerIdRequest
			{
				PlayerId = this.currentPlayer.PlayerId
			});
			getRanksCommand.AddSuccessCallback(new AbstractCommand<PlayerIdRequest, ConflictRanks>.OnSuccessCallback(this.OnGetRanks));
			Service.Get<ServerAPI>().Enqueue(getRanksCommand);
		}

		private void OnGetRanks(ConflictRanks ranks, object cookie)
		{
			if (ranks != null && ranks.tournamentRankResponse != null)
			{
				foreach (KeyValuePair<string, object> current in ranks.tournamentRankResponse)
				{
					Tournament tournament = this.currentPlayer.TournamentProgress.GetTournament(current.get_Key());
					if (tournament != null)
					{
						tournament.UpdateRatingAndCurrentRank(current.get_Value());
					}
				}
			}
		}

		public void UpdatePlayerRank(TournamentController.PlayerRankUpdatedCallback callback, TournamentVO tournamentVO)
		{
			if (tournamentVO != null)
			{
				GetTournamentRankCommand getTournamentRankCommand = new GetTournamentRankCommand(new TournamentRankRequest
				{
					PlayerId = this.currentPlayer.PlayerId,
					PlanetId = tournamentVO.PlanetId
				});
				getTournamentRankCommand.AddSuccessCallback(new AbstractCommand<TournamentRankRequest, TournamentRankResponse>.OnSuccessCallback(this.OnGetTournamentRank));
				getTournamentRankCommand.Context = new Dictionary<string, object>
				{
					{
						"tournamentId",
						tournamentVO.Uid
					},
					{
						"callback",
						callback
					}
				};
				Service.Get<ServerAPI>().Sync(getTournamentRankCommand);
			}
		}

		private void OnGetTournamentRank(TournamentRankResponse rankResponse, object cookie)
		{
			TournamentRank tournamentRank = rankResponse.Rank;
			TournamentRank oldRank = null;
			Tournament tournament = null;
			Dictionary<string, object> dictionary = (Dictionary<string, object>)cookie;
			string text = (string)dictionary["tournamentId"];
			TournamentController.PlayerRankUpdatedCallback playerRankUpdatedCallback = (TournamentController.PlayerRankUpdatedCallback)dictionary["callback"];
			if (!string.IsNullOrEmpty(text))
			{
				tournament = this.currentPlayer.TournamentProgress.GetTournament(text);
				oldRank = tournament.CurrentRank;
				tournament.CurrentRank = tournamentRank;
				tournament.Sync(rankResponse.TournamentData);
			}
			if (tournamentRank == null)
			{
				tournamentRank = new TournamentRank();
				if (tournament != null)
				{
					tournament.CurrentRank = tournamentRank;
				}
			}
			if (string.IsNullOrEmpty(tournamentRank.TierUid))
			{
				TournamentTierVO tournamentTierVO = null;
				IDataController dataController = Service.Get<IDataController>();
				foreach (TournamentTierVO current in dataController.GetAll<TournamentTierVO>())
				{
					if (tournamentTierVO == null || current.Percentage > tournamentTierVO.Percentage)
					{
						tournamentTierVO = current;
						tournamentRank.TierUid = tournamentTierVO.Uid;
					}
				}
				if (tournamentTierVO != null)
				{
					tournamentRank.Percentile = (double)tournamentTierVO.Percentage;
				}
				else
				{
					tournamentRank.Percentile = 100.0;
				}
			}
			if (playerRankUpdatedCallback != null)
			{
				playerRankUpdatedCallback(oldRank, tournamentRank, text);
			}
		}

		public bool IsTournamentRedeemed(TournamentVO tournamentVO)
		{
			bool result = false;
			if (tournamentVO != null && this.currentPlayer.TournamentProgress.HasTournament(tournamentVO))
			{
				Tournament tournament = this.currentPlayer.TournamentProgress.GetTournament(tournamentVO.Uid);
				result = tournament.Collected;
			}
			return result;
		}

		private void RedeemTournaments(bool canShowDialog)
		{
			RedeemTournamentRewardCommand redeemTournamentRewardCommand = new RedeemTournamentRewardCommand(new PlayerIdChecksumRequest
			{
				PlayerId = this.currentPlayer.PlayerId
			});
			if (canShowDialog)
			{
				redeemTournamentRewardCommand.AddSuccessCallback(new AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>.OnSuccessCallback(this.OnTournamentRedeemed));
			}
			else
			{
				redeemTournamentRewardCommand.AddSuccessCallback(new AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>.OnSuccessCallback(this.OnTournamentRedeemedNoDialog));
			}
			Service.Get<ServerAPI>().Sync(redeemTournamentRewardCommand);
		}

		private Tournament ParseLastTournament(TournamentResponse response)
		{
			Tournament tournament = null;
			if (response.TournamentsData.Count == 0)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Client is trying to redeem tournament(s), however server does not redeem anything. Player ID: {0}", new object[]
				{
					this.currentPlayer.PlayerId
				});
			}
			foreach (Tournament current in response.TournamentsData)
			{
				tournament = this.currentPlayer.TournamentProgress.GetTournament(current.Uid);
				tournament.Sync(current);
				this.NotifyEndForTournamentVO = Service.Get<IDataController>().Get<TournamentVO>(tournament.Uid);
			}
			InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
			Dictionary<string, object> cratesJsonData = response.CratesJsonData;
			if (response.Rewards.Count > 0)
			{
				crates.UpdateAndBadgeFromServerObject(cratesJsonData);
			}
			else
			{
				crates.FromObject(cratesJsonData);
			}
			return tournament;
		}

		private void OnTournamentRedeemed(TournamentResponse response, object cookie)
		{
			Tournament cookie2 = this.ParseLastTournament(response);
			Service.Get<EventManager>().SendEvent(EventId.TournamentRedeemed, cookie2);
			if (Service.Get<GameStateMachine>().CurrentState is HomeState)
			{
				this.ShowTournamentEnded();
				return;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldInTransitionComplete, EventPriority.Default);
		}

		private void OnTournamentRedeemedNoDialog(TournamentResponse response, object cookie)
		{
			Tournament cookie2 = this.ParseLastTournament(response);
			Service.Get<EventManager>().SendEvent(EventId.TournamentRedeemed, cookie2);
		}

		private void ShowTournamentEnded()
		{
			if (this.NotifyEndForTournamentVO != null)
			{
				Tournament tournament = this.currentPlayer.TournamentProgress.GetTournament(this.NotifyEndForTournamentVO.Uid);
				if (tournament != null)
				{
					Service.Get<ScreenController>().AddScreen(new TournamentEndedScreen(tournament));
				}
				this.NotifyEndForTournamentVO = null;
			}
		}

		public string GetTierIconName(TournamentTierVO tierVO)
		{
			string result = null;
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			if (faction != FactionType.Empire)
			{
				if (faction == FactionType.Rebel)
				{
					result = tierVO.SpriteNameRebel;
				}
			}
			else
			{
				result = tierVO.SpriteNameEmpire;
			}
			return result;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.WorldInTransitionComplete)
			{
				if (id == EventId.PlanetRelocate)
				{
					this.CurrentPlanetActiveTournament = this.GetActiveTournamentOnCurrentPlanet();
				}
			}
			else if (Service.Get<GameStateMachine>().CurrentState is HomeState)
			{
				if (this.showConflictPopup)
				{
					this.showConflictPopup = false;
					this.SetConflictPopupIsShown();
					AlertScreen.ShowModal(false, null, Service.Get<Lang>().Get("VICTORY_BONUS_MESSAGE", new object[0]), null, null);
				}
				else if (this.NotifyEndForTournamentVO != null)
				{
					this.ShowTournamentEnded();
				}
				else
				{
					TournamentRank tournamentCurrentRank = this.currentPlayer.TournamentProgress.GetTournamentCurrentRank(this.CurrentPlanetActiveTournament.Uid);
					Service.Get<ScreenController>().AddScreen(new TournamentTierChangedScreen(tournamentCurrentRank));
					string newValue = ServerTime.Time.ToString();
					Service.Get<ServerPlayerPrefs>().SetPref(ServerPref.TournamentTierChangeTimeViewed, newValue);
					SetPrefsCommand command = new SetPrefsCommand(false);
					Service.Get<ServerAPI>().Enqueue(command);
				}
				Service.Get<EventManager>().UnregisterObserver(this, EventId.WorldInTransitionComplete);
			}
			return EatResponse.NotEaten;
		}

		public static List<TournamentVO> GetAllActiveTournaments()
		{
			return TournamentController.GetTournamentVOs(TournamentFilter.Active);
		}

		public static List<TournamentVO> GetAllLiveAndClosingTournaments()
		{
			return TournamentController.GetTournamentVOs(TournamentFilter.LiveOrClosing);
		}

		public static List<TournamentVO> GetTournamentVOs(TournamentFilter tournamentFilter)
		{
			List<TournamentVO> list = new List<TournamentVO>();
			IDataController dataController = Service.Get<IDataController>();
			foreach (TournamentVO current in dataController.GetAll<TournamentVO>())
			{
				if (TournamentController.IsPlanetaryConflict(current))
				{
					bool flag;
					switch (tournamentFilter)
					{
					case TournamentFilter.Live:
						flag = TimedEventUtils.IsTimedEventLive(current);
						break;
					case TournamentFilter.LiveOrClosing:
						flag = TimedEventUtils.IsTimedEventLiveOrClosing(current);
						break;
					case TournamentFilter.Active:
						flag = TimedEventUtils.IsTimedEventActive(current);
						break;
					case TournamentFilter.All:
						goto IL_60;
					default:
						goto IL_60;
					}
					IL_63:
					if (flag)
					{
						list.Add(current);
						continue;
					}
					continue;
					IL_60:
					flag = true;
					goto IL_63;
				}
			}
			return list;
		}

		private static TournamentVO GetLiveTournamentForBonus(uint timeToCheck)
		{
			string planetId = Service.Get<CurrentPlayer>().PlanetId;
			IDataController dataController = Service.Get<IDataController>();
			foreach (TournamentVO current in dataController.GetAll<TournamentVO>())
			{
				if (TournamentController.IsPlanetaryConflict(current) && current.PlanetId == planetId && TimedEventUtils.IsTimedEventLive(current, timeToCheck))
				{
					return current;
				}
			}
			return null;
		}

		public static TournamentVO GetActiveTournamentOnPlanet(string planetId)
		{
			IDataController dataController = Service.Get<IDataController>();
			foreach (TournamentVO current in dataController.GetAll<TournamentVO>())
			{
				if (current.PlanetId == planetId && TimedEventUtils.IsTimedEventActive(current))
				{
					return current;
				}
			}
			return null;
		}

		private static bool IsPlanetaryConflict(TournamentVO tournamentVO)
		{
			return !string.IsNullOrEmpty(tournamentVO.PlanetId);
		}

		public static void LoadTierData()
		{
			Dictionary<string, TournamentTierVO>.ValueCollection all = Service.Get<IDataController>().GetAll<TournamentTierVO>();
			TournamentController.tiers = new List<TournamentTierVO>();
			foreach (TournamentTierVO current in all)
			{
				TournamentController.tiers.Add(current);
			}
			TournamentController.tiers.Sort(new Comparison<TournamentTierVO>(TournamentController.CompareTiers));
		}

		private static int CompareTiers(TournamentTierVO a, TournamentTierVO b)
		{
			return a.Percentage.CompareTo(b.Percentage);
		}

		public static TournamentTierVO GetIdForTopTier()
		{
			if (TournamentController.tiers != null)
			{
				return TournamentController.tiers[0];
			}
			return null;
		}

		public static TournamentTierVO GetVOForNextTier(TournamentTierVO tierVO)
		{
			for (int i = TournamentController.tiers.Count - 1; i >= 0; i--)
			{
				if (TournamentController.tiers[i].Percentage < tierVO.Percentage)
				{
					return TournamentController.tiers[i];
				}
			}
			return tierVO;
		}

		protected internal TournamentController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).CheckForTierChangeAfterBattle((TournamentRank)GCHandledObjects.GCHandleToObject(*args), (TournamentRank)GCHandledObjects.GCHandleToObject(args[1]), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentController.CompareTiers((TournamentTierVO)GCHandledObjects.GCHandleToObject(*args), (TournamentTierVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).EnterPlanetConflict();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).EnterTournament((TournamentVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).FindNextExpiringConflict();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).NotifyEndForTournamentVO);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).GetActiveTournamentOnCurrentPlanet());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentController.GetActiveTournamentOnPlanet(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentController.GetAllActiveTournaments());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentController.GetAllLiveAndClosingTournaments());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentController.GetIdForTopTier());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).GetTierIconName((TournamentTierVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).GetTournamentRating((TournamentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentController.GetTournamentVOs((TournamentFilter)(*(int*)args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentController.GetVOForNextTier((TournamentTierVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).IsBattleInCurrentTournament((BattleEntry)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentController.IsPlanetaryConflict((TournamentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).IsPlayerInTournament((TournamentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).IsThisTournamentLive((TournamentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).IsTournamentRedeemed((TournamentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			TournamentController.LoadTierData();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).LoadTournamentViewedData();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).NumberOfTournamentsNotViewed());
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).OnGalaxyViewed();
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).OnGetRanks((ConflictRanks)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).OnGetTournamentRank((TournamentRankResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).OnPvpBattleComplete((Tournament)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).OnTournamentRedeemed((TournamentResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).OnTournamentRedeemedNoDialog((TournamentResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).OnTournamentViewed(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).ParseLastTournament((TournamentResponse)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).RedeemTournaments(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).NotifyEndForTournamentVO = (TournamentVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).SetConflictPopupIsShown();
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentController)GCHandledObjects.GCHandleToObject(instance)).ShouldShowTournamentLeaderboard((TournamentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).ShowTournamentEnded();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).UpdateAndSyncTournamentViewedData();
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).UpdatePlayerRank((TournamentController.PlayerRankUpdatedCallback)GCHandledObjects.GCHandleToObject(*args), (TournamentVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).UpdatePlayerRanks();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((TournamentController)GCHandledObjects.GCHandleToObject(instance)).UpdateTournamentsData(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
