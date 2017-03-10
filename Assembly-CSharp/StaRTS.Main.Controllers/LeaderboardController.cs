using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Commands.Squads;
using StaRTS.Main.Models.Commands.Squads.Requests;
using StaRTS.Main.Models.Commands.Squads.Responses;
using StaRTS.Main.Models.Leaderboard;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class LeaderboardController : IEventObserver
	{
		public delegate void OnUpdateData(bool success);

		public delegate void OnUpdateSquadData(Squad squad, bool success);

		private const string PARAM_CALLBACK = "callback";

		private const string PARAM_LIST = "list";

		private const string PARAM_NEARBY = "listnearby";

		private List<Squad> cachedSquads;

		private List<PlayerLBEntity> cachedPlayers;

		private const int DATA_REFRESH_THROTTLE = 30;

		private const int RECHECK_LB_FRIENDS_COUNT = 0;

		private const int RECHECK_LB_LEADERS_COUNT = 10;

		public List<PlayerLBEntity> topPlayers;

		public LeaderboardList<Squad> TopSquads
		{
			get;
			private set;
		}

		public LeaderboardList<Squad> SquadsNearMe
		{
			get;
			private set;
		}

		public LeaderboardList<Squad> FeaturedSquads
		{
			get;
			private set;
		}

		public LeaderboardList<Squad> SearchedSquads
		{
			get;
			private set;
		}

		public LeaderboardList<PlayerLBEntity> Friends
		{
			get;
			private set;
		}

		public LeaderboardList<PlayerLBEntity> GlobalLeaders
		{
			get;
			private set;
		}

		public LeaderboardList<PlayerLBEntity> GlobalNearMeLeaders
		{
			get;
			private set;
		}

		public Dictionary<string, LeaderboardList<PlayerLBEntity>> LeadersByPlanet
		{
			get;
			private set;
		}

		public Dictionary<string, LeaderboardList<PlayerLBEntity>> LeadersNearMeByPlanet
		{
			get;
			private set;
		}

		public Dictionary<string, LeaderboardList<PlayerLBEntity>> TournamentLeadersByPlanet
		{
			get;
			private set;
		}

		public Dictionary<string, LeaderboardList<PlayerLBEntity>> TournamentLeadersNearMeByPlanet
		{
			get;
			private set;
		}

		public LeaderboardController()
		{
			Service.Set<LeaderboardController>(this);
			this.TopSquads = new LeaderboardList<Squad>();
			this.SquadsNearMe = new LeaderboardList<Squad>();
			this.FeaturedSquads = new LeaderboardList<Squad>();
			this.SearchedSquads = new LeaderboardList<Squad>();
			this.Friends = new LeaderboardList<PlayerLBEntity>();
			this.GlobalLeaders = new LeaderboardList<PlayerLBEntity>();
			this.GlobalNearMeLeaders = new LeaderboardList<PlayerLBEntity>();
			this.LeadersByPlanet = new Dictionary<string, LeaderboardList<PlayerLBEntity>>();
			this.LeadersNearMeByPlanet = new Dictionary<string, LeaderboardList<PlayerLBEntity>>();
			this.TournamentLeadersByPlanet = new Dictionary<string, LeaderboardList<PlayerLBEntity>>();
			this.TournamentLeadersNearMeByPlanet = new Dictionary<string, LeaderboardList<PlayerLBEntity>>();
			this.topPlayers = new List<PlayerLBEntity>();
			this.cachedSquads = new List<Squad>();
			this.cachedPlayers = new List<PlayerLBEntity>();
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
			eventManager.RegisterObserver(this, EventId.SquadLeft);
		}

		public void InitLeaderBoardListForPlanet()
		{
			List<PlanetVO> allPlayerFacingPlanets = PlanetUtils.GetAllPlayerFacingPlanets();
			for (int i = 0; i < allPlayerFacingPlanets.Count; i++)
			{
				string uid = allPlayerFacingPlanets[i].Uid;
				LeaderboardList<PlayerLBEntity> value = new LeaderboardList<PlayerLBEntity>();
				this.LeadersByPlanet.Add(uid, value);
				value = new LeaderboardList<PlayerLBEntity>();
				this.LeadersNearMeByPlanet.Add(uid, value);
			}
			List<TournamentVO> allLiveAndClosingTournaments = TournamentController.GetAllLiveAndClosingTournaments();
			int j = 0;
			int count = allLiveAndClosingTournaments.Count;
			while (j < count)
			{
				string planetId = allLiveAndClosingTournaments[j].PlanetId;
				if (this.TournamentLeadersByPlanet.ContainsKey(planetId))
				{
					Service.Get<StaRTSLogger>().Error("Multiple tournaments are active on planet " + planetId);
				}
				else
				{
					this.InitTournamentListForPlanet(planetId);
				}
				j++;
			}
		}

		private void InitTournamentListForPlanet(string planetUid)
		{
			LeaderboardList<PlayerLBEntity> value = new LeaderboardList<PlayerLBEntity>();
			this.TournamentLeadersByPlanet.Add(planetUid, value);
			value = new LeaderboardList<PlayerLBEntity>();
			this.TournamentLeadersNearMeByPlanet.Add(planetUid, value);
		}

		public void UpdateTopSquads(LeaderboardController.OnUpdateData callback)
		{
			GetLeaderboardSquadsCommand getLeaderboardSquadsCommand = new GetLeaderboardSquadsCommand(new PlayerIdRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId
			});
			getLeaderboardSquadsCommand.AddSuccessCallback(new AbstractCommand<PlayerIdRequest, LeaderboardResponse>.OnSuccessCallback(this.OnGetTopSquadsSuccess));
			getLeaderboardSquadsCommand.AddFailureCallback(new AbstractCommand<PlayerIdRequest, LeaderboardResponse>.OnFailureCallback(this.OnUpdateFailure));
			getLeaderboardSquadsCommand.Context = callback;
			Service.Get<ServerAPI>().Sync(getLeaderboardSquadsCommand);
			this.TopSquads.LastRefreshTime = Service.Get<ServerAPI>().ServerTime;
		}

		private void OnGetTopSquadsSuccess(LeaderboardResponse response, object cookie)
		{
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			this.ParseSquadResponse(response.TopData, this.TopSquads, false, currentSquad);
			this.ParseSquadResponse(response.SurroundingData, this.SquadsNearMe, false, currentSquad);
			this.FireCallbackFromCookie(cookie, true);
		}

		public void UpdateFeaturedSquads(LeaderboardController.OnUpdateData callback)
		{
			GetFeaturedSquadsCommand getFeaturedSquadsCommand = new GetFeaturedSquadsCommand(new PlayerIdRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId
			});
			getFeaturedSquadsCommand.AddSuccessCallback(new AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>.OnSuccessCallback(this.OnGetFeaturedSquadsSuccess));
			getFeaturedSquadsCommand.AddFailureCallback(new AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>.OnFailureCallback(this.OnUpdateFailure));
			getFeaturedSquadsCommand.Context = callback;
			Service.Get<ServerAPI>().Sync(getFeaturedSquadsCommand);
			this.FeaturedSquads.LastRefreshTime = Service.Get<ServerAPI>().ServerTime;
		}

		private void OnGetFeaturedSquadsSuccess(FeaturedSquadsResponse response, object cookie)
		{
			this.ParseSquadResponse(response.SquadData, this.FeaturedSquads, true, null);
			this.FeaturedSquads.List.Sort();
			this.FireCallbackFromCookie(cookie, true);
		}

		public void SearchSquadsByName(string searchStr, LeaderboardController.OnUpdateData callback)
		{
			SearchSquadsByNameCommand searchSquadsByNameCommand = new SearchSquadsByNameCommand(new SearchSquadsByNameRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId,
				SearchTerm = searchStr
			});
			searchSquadsByNameCommand.AddSuccessCallback(new AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>.OnSuccessCallback(this.OnGetSearchedSquadsSuccess));
			searchSquadsByNameCommand.AddFailureCallback(new AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>.OnFailureCallback(this.OnUpdateFailure));
			searchSquadsByNameCommand.Context = callback;
			Service.Get<ServerAPI>().Sync(searchSquadsByNameCommand);
			this.SearchedSquads.LastRefreshTime = Service.Get<ServerAPI>().ServerTime;
		}

		private void OnGetSearchedSquadsSuccess(FeaturedSquadsResponse response, object cookie)
		{
			this.ParseSquadResponse(response.SquadData, this.SearchedSquads, true, null);
			this.SearchedSquads.List.Sort();
			this.FireCallbackFromCookie(cookie, true);
		}

		public void UpdateSquadDetails(string squadId, LeaderboardController.OnUpdateSquadData callback)
		{
			GetPublicSquadCommand getPublicSquadCommand = new GetPublicSquadCommand(new SquadIDRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId,
				SquadId = squadId
			});
			getPublicSquadCommand.AddSuccessCallback(new AbstractCommand<SquadIDRequest, SquadResponse>.OnSuccessCallback(this.OnUpdateSquadSuccess));
			getPublicSquadCommand.AddFailureCallback(new AbstractCommand<SquadIDRequest, SquadResponse>.OnFailureCallback(this.OnUpdateSquadFailure));
			getPublicSquadCommand.Context = callback;
			Service.Get<ServerAPI>().Sync(getPublicSquadCommand);
		}

		private void OnUpdateSquadSuccess(SquadResponse response, object cookie)
		{
			Squad orCreateSquad = this.GetOrCreateSquad(response.SquadId);
			orCreateSquad.FromObject(response.SquadData);
			LeaderboardController.OnUpdateSquadData onUpdateSquadData = (LeaderboardController.OnUpdateSquadData)cookie;
			if (onUpdateSquadData != null)
			{
				onUpdateSquadData(orCreateSquad, true);
			}
		}

		private void OnUpdateSquadFailure(uint status, object cookie)
		{
			LeaderboardController.OnUpdateSquadData onUpdateSquadData = (LeaderboardController.OnUpdateSquadData)cookie;
			if (onUpdateSquadData != null)
			{
				onUpdateSquadData(null, false);
			}
		}

		private void ParseSquadResponse(List<object> squads, LeaderboardList<Squad> leaderboardList, bool featured, Squad currentPlayerSquad)
		{
			if (squads != null && squads.Count > 0)
			{
				leaderboardList.List.Clear();
				int i = 0;
				int count = squads.Count;
				while (i < count)
				{
					Dictionary<string, object> dictionary = squads[i] as Dictionary<string, object>;
					if (dictionary != null)
					{
						string text = null;
						if (dictionary.ContainsKey("_id"))
						{
							text = Convert.ToString(dictionary["_id"], CultureInfo.InvariantCulture);
						}
						if (text != null)
						{
							Squad orCreateSquad = this.GetOrCreateSquad(text);
							if (featured)
							{
								orCreateSquad.FromFeaturedObject(dictionary);
							}
							else
							{
								orCreateSquad.FromLeaderboardObject(dictionary);
							}
							if (orCreateSquad.MemberCount > 0)
							{
								leaderboardList.List.Add(orCreateSquad);
							}
							if (currentPlayerSquad != null && text == currentPlayerSquad.SquadID)
							{
								leaderboardList.AlwaysRefresh = true;
							}
						}
					}
					i++;
				}
			}
		}

		public Squad GetOrCreateSquad(string squadID)
		{
			Squad squad = this.GetCachedSquad(squadID);
			if (squad == null)
			{
				squad = new Squad(squadID);
				this.cachedSquads.Add(squad);
			}
			return squad;
		}

		private Squad GetCachedSquad(string squadID)
		{
			int i = 0;
			int count = this.cachedSquads.Count;
			while (i < count)
			{
				Squad squad = this.cachedSquads[i];
				if (squad.SquadID == squadID)
				{
					return squad;
				}
				i++;
			}
			return null;
		}

		public void UpdateFriends(string friendIds, LeaderboardController.OnUpdateData callback)
		{
			GetLeaderboardFriendsCommand getLeaderboardFriendsCommand = new GetLeaderboardFriendsCommand(new FriendLBIDRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId,
				FriendIDs = friendIds
			});
			getLeaderboardFriendsCommand.AddSuccessCallback(new AbstractCommand<FriendLBIDRequest, LeaderboardResponse>.OnSuccessCallback(this.OnFriendsUpdated));
			getLeaderboardFriendsCommand.AddFailureCallback(new AbstractCommand<FriendLBIDRequest, LeaderboardResponse>.OnFailureCallback(this.OnUpdateFailure));
			getLeaderboardFriendsCommand.Context = callback;
			Service.Get<ServerAPI>().Sync(getLeaderboardFriendsCommand);
			this.Friends.LastRefreshTime = Service.Get<ServerAPI>().ServerTime;
		}

		private void OnFriendsUpdated(LeaderboardResponse response, object cookie)
		{
			this.ParsePlayerResponse(response.TopData, this.Friends);
			this.Friends.List.Sort();
			this.FireCallbackFromCookie(cookie, true);
		}

		public void UpdateLeaders(PlanetVO planetVO, LeaderboardController.OnUpdateData callback)
		{
			LeaderboardList<PlayerLBEntity> leaderboardList = null;
			LeaderboardList<PlayerLBEntity> value = null;
			string planetId = (planetVO != null) ? planetVO.Uid : null;
			this.GetPlayerLists(PlayerListType.Leaders, planetId, out leaderboardList, out value);
			PlayerLeaderboardRequest request = new PlayerLeaderboardRequest(planetId, Service.Get<CurrentPlayer>().PlayerId);
			GetLeaderboardPlayersCommand getLeaderboardPlayersCommand = new GetLeaderboardPlayersCommand(request);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["callback"] = callback;
			dictionary["list"] = leaderboardList;
			dictionary["listnearby"] = value;
			getLeaderboardPlayersCommand.Context = dictionary;
			getLeaderboardPlayersCommand.AddSuccessCallback(new AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>.OnSuccessCallback(this.OnLeadersUpdated));
			getLeaderboardPlayersCommand.AddFailureCallback(new AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>.OnFailureCallback(this.OnLeadersUpdateFailure));
			Service.Get<ServerAPI>().Sync(getLeaderboardPlayersCommand);
			leaderboardList.LastRefreshTime = Service.Get<ServerAPI>().ServerTime;
		}

		private void OnLeadersUpdated(LeaderboardResponse response, object cookie)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)cookie;
			LeaderboardList<PlayerLBEntity> leaderboardList = (LeaderboardList<PlayerLBEntity>)dictionary["list"];
			LeaderboardList<PlayerLBEntity> leaderboardList2 = (LeaderboardList<PlayerLBEntity>)dictionary["listnearby"];
			object cookie2 = dictionary["callback"];
			this.ParsePlayerResponse(response.TopData, leaderboardList);
			this.ParsePlayerResponse(response.SurroundingData, leaderboardList2);
			this.FireCallbackFromCookie(cookie2, true);
		}

		private void OnLeadersUpdateFailure(uint status, object cookie)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)cookie;
			object cookie2 = dictionary["callback"];
			this.FireCallbackFromCookie(cookie2, false);
		}

		public void UpdateTournamentLeaders(PlanetVO planetVO, LeaderboardController.OnUpdateData callback)
		{
			if (planetVO == null)
			{
				Service.Get<StaRTSLogger>().Error("Tournament leaderboard requested without setting planetVO");
				return;
			}
			PlayerLeaderboardRequest request = new PlayerLeaderboardRequest(planetVO.Uid, Service.Get<CurrentPlayer>().PlayerId);
			LeaderboardList<PlayerLBEntity> leaderboardList = null;
			LeaderboardList<PlayerLBEntity> value = null;
			this.GetPlayerLists(PlayerListType.TournamentLeaders, planetVO.Uid, out leaderboardList, out value);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["callback"] = callback;
			dictionary["list"] = leaderboardList;
			dictionary["listnearby"] = value;
			GetTournamentLeaderboardPlayersCommand getTournamentLeaderboardPlayersCommand = new GetTournamentLeaderboardPlayersCommand(request);
			getTournamentLeaderboardPlayersCommand.Context = dictionary;
			getTournamentLeaderboardPlayersCommand.AddSuccessCallback(new AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>.OnSuccessCallback(this.OnLeadersUpdated));
			getTournamentLeaderboardPlayersCommand.AddFailureCallback(new AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>.OnFailureCallback(this.OnLeadersUpdateFailure));
			Service.Get<ServerAPI>().Sync(getTournamentLeaderboardPlayersCommand);
			leaderboardList.LastRefreshTime = Service.Get<ServerAPI>().ServerTime;
		}

		private void ParsePlayerResponse(List<object> players, LeaderboardList<PlayerLBEntity> leaderboardList)
		{
			if (players != null && players.Count > 0)
			{
				leaderboardList.List.Clear();
				int i = 0;
				int count = players.Count;
				while (i < count)
				{
					Dictionary<string, object> dictionary = players[i] as Dictionary<string, object>;
					if (dictionary != null)
					{
						string text = null;
						if (dictionary.ContainsKey("_id"))
						{
							text = Convert.ToString(dictionary["_id"], CultureInfo.InvariantCulture);
						}
						if (text != null)
						{
							PlayerLBEntity orCreatePlayer = this.GetOrCreatePlayer(text);
							if (orCreatePlayer.FromObject(dictionary) != null)
							{
								leaderboardList.List.Add(orCreatePlayer);
								if (orCreatePlayer.PlayerID == Service.Get<CurrentPlayer>().PlayerId)
								{
									leaderboardList.AlwaysRefresh = true;
								}
							}
							else
							{
								Service.Get<StaRTSLogger>().Warn("Player Leaderboard Entry Failed to parse.");
							}
						}
					}
					i++;
				}
			}
		}

		private PlayerLBEntity GetOrCreatePlayer(string playerId)
		{
			PlayerLBEntity playerLBEntity = this.GetCachedPlayer(playerId);
			if (playerLBEntity == null)
			{
				playerLBEntity = new PlayerLBEntity(playerId);
				this.cachedPlayers.Add(playerLBEntity);
			}
			return playerLBEntity;
		}

		private PlayerLBEntity GetCachedPlayer(string playerId)
		{
			int i = 0;
			int count = this.cachedPlayers.Count;
			while (i < count)
			{
				PlayerLBEntity playerLBEntity = this.cachedPlayers[i];
				if (playerLBEntity.PlayerID == playerId)
				{
					return playerLBEntity;
				}
				i++;
			}
			return null;
		}

		private void OnUpdateFailure(uint status, object cookie)
		{
			this.FireCallbackFromCookie(cookie, false);
		}

		private void FireCallbackFromCookie(object cookie, bool success)
		{
			LeaderboardController.OnUpdateData onUpdateData = (LeaderboardController.OnUpdateData)cookie;
			if (onUpdateData != null)
			{
				onUpdateData(success);
			}
		}

		private void GetPlayerLists(PlayerListType listType, string planetId, out LeaderboardList<PlayerLBEntity> leaderboardList, out LeaderboardList<PlayerLBEntity> nearbyLeaderboardList)
		{
			leaderboardList = null;
			nearbyLeaderboardList = null;
			switch (listType)
			{
			case PlayerListType.Friends:
				leaderboardList = this.Friends;
				nearbyLeaderboardList = this.Friends;
				return;
			case PlayerListType.Leaders:
				if (planetId == null)
				{
					leaderboardList = this.GlobalLeaders;
					nearbyLeaderboardList = this.GlobalNearMeLeaders;
					return;
				}
				leaderboardList = this.LeadersByPlanet[planetId];
				nearbyLeaderboardList = this.LeadersNearMeByPlanet[planetId];
				return;
			case PlayerListType.TournamentLeaders:
				if (!string.IsNullOrEmpty(planetId))
				{
					if (!this.TournamentLeadersByPlanet.ContainsKey(planetId))
					{
						this.InitTournamentListForPlanet(planetId);
					}
					leaderboardList = this.TournamentLeadersByPlanet[planetId];
					nearbyLeaderboardList = this.TournamentLeadersNearMeByPlanet[planetId];
					return;
				}
				Service.Get<StaRTSLogger>().Error("planetId value is null or empty in tournament leaderboard response handling");
				return;
			default:
				return;
			}
		}

		public bool ShouldRefreshData(PlayerListType listType, string planetId)
		{
			bool flag = false;
			bool flag2 = false;
			uint serverTime = Service.Get<ServerAPI>().ServerTime;
			uint timeB = 0u;
			switch (listType)
			{
			case PlayerListType.FeaturedSquads:
				if (this.FeaturedSquads.List.Count == 0)
				{
					flag = true;
				}
				else
				{
					flag2 = true;
				}
				break;
			case PlayerListType.SearchedSquads:
				flag = false;
				flag2 = true;
				break;
			case PlayerListType.Squads:
				flag = this.TopSquads.AlwaysRefresh;
				timeB = this.TopSquads.LastRefreshTime;
				break;
			case PlayerListType.Friends:
				flag = this.Friends.AlwaysRefresh;
				timeB = this.Friends.LastRefreshTime;
				break;
			case PlayerListType.Leaders:
				if (string.IsNullOrEmpty(planetId))
				{
					flag = this.GlobalLeaders.AlwaysRefresh;
					timeB = this.GlobalLeaders.LastRefreshTime;
				}
				else
				{
					flag = this.LeadersByPlanet[planetId].AlwaysRefresh;
					timeB = this.LeadersByPlanet[planetId].LastRefreshTime;
				}
				break;
			case PlayerListType.TournamentLeaders:
				if (this.TournamentLeadersByPlanet.ContainsKey(planetId))
				{
					flag = this.TournamentLeadersByPlanet[planetId].AlwaysRefresh;
					timeB = this.TournamentLeadersByPlanet[planetId].LastRefreshTime;
				}
				break;
			}
			if (!flag && !flag2)
			{
				int timeDifferenceSafe = GameUtils.GetTimeDifferenceSafe(serverTime, timeB);
				flag = (timeDifferenceSafe >= 30);
			}
			return flag;
		}

		public void TopPlayer(PlanetVO planetVO)
		{
			this.UpdateLeaders(planetVO, new LeaderboardController.OnUpdateData(this.OnTopPlayerSuccess));
		}

		private void OnTopPlayerSuccess(bool success)
		{
			if (!success)
			{
				Service.Get<EventManager>().SendEvent(EventId.HolonetLeaderBoardUpdated, null);
				return;
			}
			this.topPlayers.Clear();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (this.LeadersByPlanet.ContainsKey(currentPlayer.PlanetId) && this.LeadersByPlanet[currentPlayer.PlanetId].List.Count >= 0)
			{
				LeaderboardList<PlayerLBEntity> leaderboardList = this.LeadersByPlanet[currentPlayer.PlanetId];
				if (leaderboardList.List.Count > 0)
				{
					this.topPlayers.Add(leaderboardList.List[0]);
					int num = 0;
					int count = leaderboardList.List.Count;
					while (num < count && this.topPlayers.Count < 2)
					{
						if (leaderboardList.List[num].Faction != this.topPlayers[0].Faction)
						{
							this.topPlayers.Add(leaderboardList.List[num]);
						}
						num++;
					}
				}
			}
			Service.Get<EventManager>().SendEvent(EventId.HolonetLeaderBoardUpdated, null);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.SquadLeft)
			{
				if (id == EventId.SquadJoinedByCurrentPlayer)
				{
					string squadID = Service.Get<SquadController>().StateManager.GetCurrentSquad().SquadID;
					int i = 0;
					int count = this.TopSquads.List.Count;
					while (i < count)
					{
						if (this.TopSquads.List[i].SquadID == squadID)
						{
							this.TopSquads.AlwaysRefresh = true;
							break;
						}
						i++;
					}
				}
			}
			else
			{
				this.TopSquads.AlwaysRefresh = false;
			}
			return EatResponse.NotEaten;
		}

		protected internal LeaderboardController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).FireCallbackFromCookie(GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).FeaturedSquads);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).Friends);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).GlobalLeaders);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).GlobalNearMeLeaders);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).LeadersByPlanet);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).LeadersNearMeByPlanet);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).SearchedSquads);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).SquadsNearMe);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).TopSquads);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).TournamentLeadersByPlanet);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).TournamentLeadersNearMeByPlanet);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).GetCachedPlayer(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).GetCachedSquad(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).GetOrCreatePlayer(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).GetOrCreateSquad(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).InitLeaderBoardListForPlanet();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).InitTournamentListForPlanet(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).OnFriendsUpdated((LeaderboardResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).OnGetFeaturedSquadsSuccess((FeaturedSquadsResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).OnGetSearchedSquadsSuccess((FeaturedSquadsResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).OnGetTopSquadsSuccess((LeaderboardResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).OnLeadersUpdated((LeaderboardResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).OnTopPlayerSuccess(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).OnUpdateSquadSuccess((SquadResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).ParsePlayerResponse((List<object>)GCHandledObjects.GCHandleToObject(*args), (LeaderboardList<PlayerLBEntity>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).ParseSquadResponse((List<object>)GCHandledObjects.GCHandleToObject(*args), (LeaderboardList<Squad>)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, (Squad)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).SearchSquadsByName(Marshal.PtrToStringUni(*(IntPtr*)args), (LeaderboardController.OnUpdateData)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).FeaturedSquads = (LeaderboardList<Squad>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).Friends = (LeaderboardList<PlayerLBEntity>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).GlobalLeaders = (LeaderboardList<PlayerLBEntity>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).GlobalNearMeLeaders = (LeaderboardList<PlayerLBEntity>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).LeadersByPlanet = (Dictionary<string, LeaderboardList<PlayerLBEntity>>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).LeadersNearMeByPlanet = (Dictionary<string, LeaderboardList<PlayerLBEntity>>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).SearchedSquads = (LeaderboardList<Squad>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).SquadsNearMe = (LeaderboardList<Squad>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).TopSquads = (LeaderboardList<Squad>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).TournamentLeadersByPlanet = (Dictionary<string, LeaderboardList<PlayerLBEntity>>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).TournamentLeadersNearMeByPlanet = (Dictionary<string, LeaderboardList<PlayerLBEntity>>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).ShouldRefreshData((PlayerListType)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).TopPlayer((PlanetVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).UpdateFeaturedSquads((LeaderboardController.OnUpdateData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).UpdateFriends(Marshal.PtrToStringUni(*(IntPtr*)args), (LeaderboardController.OnUpdateData)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).UpdateLeaders((PlanetVO)GCHandledObjects.GCHandleToObject(*args), (LeaderboardController.OnUpdateData)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).UpdateSquadDetails(Marshal.PtrToStringUni(*(IntPtr*)args), (LeaderboardController.OnUpdateSquadData)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).UpdateTopSquads((LeaderboardController.OnUpdateData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((LeaderboardController)GCHandledObjects.GCHandleToObject(instance)).UpdateTournamentLeaders((PlanetVO)GCHandledObjects.GCHandleToObject(*args), (LeaderboardController.OnUpdateData)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
