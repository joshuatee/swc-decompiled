using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers.SquadWar;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.UX.Screens.Squads;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Squads
{
	public class SquadController
	{
		public delegate void SquadMsgsCallback(List<SquadMsg> msgs);

		public delegate void ActionCallback(bool success, object cookie);

		private const string LANG_NO_SQUADMATES_TO_DONATE = "s_noSquadmatesToDonate";

		private const string REQUEST_TROOPS_DEFAULT = "REQUEST_TROOPS_DEFAULT";

		private const string REQUEST_WAR_TROOPS_DEFAULT = "REQUEST_WAR_TROOPS_DEFAULT";

		private const string CURRENT_PLAYER_EJECTED = "CURRENT_PLAYER_EJECTED";

		private const string LEAVE_A_SQUAD = "LEAVE_A_SQUAD";

		private const string REQUEST_TROOPS = "requestTroops";

		private const string SQUAD_ACTION = "squad_action";

		private const string CONSUMABLE = "consumable";

		private const float PULL_FREQUENCY_CHAT_OPEN_DEFAULT = 5f;

		private const float PULL_FREQUENCY_CHAT_CLOSED_DEFAULT = 30f;

		private const int MESSAGE_LIMIT = 50;

		public SquadServerManager ServerManager
		{
			get;
			private set;
		}

		public SquadStateManager StateManager
		{
			get;
			private set;
		}

		public SquadMsgManager MsgManager
		{
			get;
			private set;
		}

		public SquadWarManager WarManager
		{
			get;
			private set;
		}

		public SquadWarBuffManager BuffManager
		{
			get;
			private set;
		}

		public float PullFrequencyChatOpen
		{
			get;
			private set;
		}

		public float PullFrequencyChatClosed
		{
			get;
			private set;
		}

		public int MessageLimit
		{
			get;
			private set;
		}

		public SquadController()
		{
			Service.Set<SquadController>(this);
			this.ServerManager = new SquadServerManager(this);
			this.StateManager = new SquadStateManager();
			this.MsgManager = new SquadMsgManager(this);
			this.WarManager = new SquadWarManager(this);
			this.BuffManager = new SquadWarBuffManager(this);
		}

		public void Enable()
		{
			this.PullFrequencyChatOpen = ((GameConstants.PULL_FREQUENCY_CHAT_OPEN == 0f) ? 5f : GameConstants.PULL_FREQUENCY_CHAT_OPEN);
			this.PullFrequencyChatClosed = ((GameConstants.PULL_FREQUENCY_CHAT_CLOSED == 0f) ? 30f : GameConstants.PULL_FREQUENCY_CHAT_CLOSED);
			this.MessageLimit = ((GameConstants.SQUAD_MESSAGE_LIMIT == 0) ? 50 : GameConstants.SQUAD_MESSAGE_LIMIT);
			this.ServerManager.Init();
			this.StateManager.Init();
			this.MsgManager.Enable();
			this.ServerManager.QueueUpdateCurrentMemberWarData();
			if (this.StateManager.GetCurrentSquad() != null)
			{
				this.UpdateCurrentSquad();
			}
			else
			{
				Service.Get<EventManager>().SendEvent(EventId.SquadUpdateCompleted, null);
			}
			if (GameConstants.SQUAD_INVITES_ENABLED)
			{
				this.UpdateSquadInvitesReceived();
			}
			if (GameConstants.PULL_FREQUENCY_CHAT_OPEN == 0f)
			{
				this.PullFrequencyChatOpen = 5f;
			}
			else
			{
				this.PullFrequencyChatOpen = GameConstants.PULL_FREQUENCY_CHAT_OPEN;
			}
			if (GameConstants.PULL_FREQUENCY_CHAT_CLOSED == 0f)
			{
				this.PullFrequencyChatClosed = 30f;
				return;
			}
			this.PullFrequencyChatClosed = GameConstants.PULL_FREQUENCY_CHAT_CLOSED;
		}

		public void TakeAction(SquadMsg message)
		{
			this.ServerManager.TakeAction(message);
		}

		private void ClearPrefsForNewSquad(int squadLevelPref)
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			sharedPlayerPrefs.SetPref("HighestViewedSquadLvlUP", squadLevelPref.ToString());
			sharedPlayerPrefs.SetPrefUnlimitedLength("perk_badges", "");
		}

		public void OnPlayerActionSuccess(SquadAction actionType, SquadMsg msg)
		{
			Squad currentSquad = this.StateManager.GetCurrentSquad();
			switch (actionType)
			{
			case SquadAction.Create:
				GameUtils.SpendCurrency(GameConstants.SQUAD_CREATE_COST, 0, 0, true);
				this.StateManager.SetCurrentSquad(msg.RespondedSquad);
				this.ClearPrefsForNewSquad(0);
				return;
			case SquadAction.Join:
				this.StateManager.SetCurrentSquad(msg.RespondedSquad);
				this.StateManager.OnSquadJoined(msg.BISource);
				this.SetLastViewedSquadLevelUp(msg.RespondedSquad.Level);
				return;
			case SquadAction.Leave:
			{
				string message = Service.Get<Lang>().Get("LEAVE_A_SQUAD", new object[]
				{
					currentSquad.SquadName
				});
				this.LeaveSquad(message);
				return;
			}
			case SquadAction.Edit:
			{
				SqmSquadData squadData = msg.SquadData;
				this.StateManager.EditSquad(squadData.Open, squadData.Icon, squadData.Desc, squadData.MinTrophies);
				return;
			}
			case SquadAction.ApplyToJoin:
				if (!this.StateManager.SquadJoinRequestsPending.Contains(msg.SquadData.Id))
				{
					this.StateManager.SquadJoinRequestsPending.Add(msg.SquadData.Id);
					return;
				}
				break;
			case SquadAction.AcceptApplicationToJoin:
				SquadUtils.AddSquadMember(currentSquad, msg.SquadMemberResponse);
				this.UpdateCurrentSquad();
				this.StateManager.OnSquadJoinApplicationAcceptedByCurrentPlayer(msg.BISource);
				return;
			case SquadAction.RejectApplicationToJoin:
			case SquadAction.ShareVideo:
				break;
			case SquadAction.SendInviteToJoin:
				this.StateManager.PlayersInvitedToSquad.Add(msg.FriendInviteData.PlayerId);
				return;
			case SquadAction.AcceptInviteToJoin:
				this.StateManager.RemoveInviteToSquad(msg.SquadData.Id);
				this.UpdateCurrentSquad();
				this.StateManager.OnSquadJoinInviteAccepted();
				return;
			case SquadAction.RejectInviteToJoin:
				this.StateManager.RemoveInviteToSquad(msg.SquadData.Id);
				return;
			case SquadAction.PromoteMember:
				SquadUtils.SetSquadMemberRole(currentSquad, msg.MemberData.MemberId, msg.MemberData.MemberRole);
				return;
			case SquadAction.DemoteMember:
				SquadUtils.SetSquadMemberRole(currentSquad, msg.MemberData.MemberId, msg.MemberData.MemberRole);
				return;
			case SquadAction.RemoveMember:
				SquadUtils.RemoveSquadMember(currentSquad, msg.MemberData.MemberId);
				return;
			case SquadAction.RequestTroops:
				this.StateManager.TroopRequestDate = Service.Get<ServerAPI>().ServerTime;
				this.StateManager.OnSquadTroopsRequested();
				return;
			case SquadAction.DonateTroops:
			case SquadAction.DonateWarTroops:
			{
				Dictionary<string, int> donations = msg.DonationData.Donations;
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				int num = 0;
				foreach (KeyValuePair<string, int> current in donations)
				{
					string key = current.get_Key();
					int value = current.get_Value();
					currentPlayer.Inventory.Troop.ModifyItemAmount(key, -value);
					num += value;
				}
				StorageSpreadUtils.UpdateAllStarportFullnessMeters();
				Service.Get<ISupportController>().UnfreezeAllBuildings(ServerTime.Time);
				this.StateManager.NumTroopDonationsInSession += num;
				this.StateManager.OnSquadTroopsDonated(donations);
				return;
			}
			case SquadAction.RequestWarTroops:
				this.StateManager.WarTroopRequestDate = Service.Get<ServerAPI>().ServerTime;
				this.StateManager.OnSquadWarTroopsRequested();
				return;
			case SquadAction.ShareReplay:
				this.StateManager.OnSquadReplayShared(msg.ReplayData);
				break;
			default:
				return;
			}
		}

		public void OnPlayerActionFailure(SquadMsg actionMsg, uint status)
		{
			SquadAction type = actionMsg.ActionData.Type;
			if (type == SquadAction.RequestTroops || type == SquadAction.RequestWarTroops)
			{
				SqmRequestData requestData = actionMsg.RequestData;
				if (requestData.PayToSkip && requestData.ResendCrystalCost > 0)
				{
					Service.Get<CurrentPlayer>().Inventory.ModifyCrystals(requestData.ResendCrystalCost);
				}
			}
			string messageForServerActionFailure = SquadUtils.GetMessageForServerActionFailure(type, status);
			AlertScreen.ShowModal(false, null, Service.Get<Lang>().Get(messageForServerActionFailure, new object[0]), null, null);
		}

		private uint GetTimeForNewMsgFiltering(SquadMsg msg)
		{
			uint result = Service.Get<CurrentPlayer>().LoginTime;
			switch (msg.Type)
			{
			case SquadMsgType.SquadLevelUp:
			case SquadMsgType.PerkUnlocked:
			case SquadMsgType.PerkUpgraded:
			case SquadMsgType.PerkInvest:
				result = Service.Get<PerkViewController>().GetLastViewedPerkTime();
				break;
			}
			return result;
		}

		public void OnNewSquadMsgsReceived(List<SquadMsg> msgs)
		{
			bool flag = false;
			Squad currentSquad = this.StateManager.GetCurrentSquad();
			bool flag2 = false;
			string playerId = Service.Get<CurrentPlayer>().PlayerId;
			int i = 0;
			int count = msgs.Count;
			while (i < count)
			{
				SquadMsg squadMsg = msgs[i];
				uint timeForNewMsgFiltering = this.GetTimeForNewMsgFiltering(squadMsg);
				if (squadMsg.TimeSent >= timeForNewMsgFiltering)
				{
					bool flag3 = squadMsg.OwnerData == null || squadMsg.OwnerData.PlayerId == null;
					bool flag4 = squadMsg.OwnerData != null && squadMsg.OwnerData.PlayerId == playerId;
					switch (squadMsg.Type)
					{
					case SquadMsgType.Join:
					case SquadMsgType.InviteAccepted:
					case SquadMsgType.Leave:
						flag = true;
						break;
					case SquadMsgType.JoinRequestAccepted:
					{
						string text = (squadMsg.SquadData != null) ? squadMsg.SquadData.Id : null;
						if (flag4 && !string.IsNullOrEmpty(text))
						{
							this.StateManager.OnSquadJoinApplicationAccepted(text);
							Squad currentSquad2 = this.StateManager.GetCurrentSquad();
							this.ClearPrefsForNewSquad(currentSquad2.Level);
						}
						flag = true;
						break;
					}
					case SquadMsgType.JoinRequestRejected:
					{
						string text2 = (squadMsg.SquadData != null) ? squadMsg.SquadData.Id : null;
						if (flag3 && !string.IsNullOrEmpty(text2))
						{
							this.StateManager.OnSquadJoinApplicationRejected(text2);
						}
						break;
					}
					case SquadMsgType.Ejected:
						if (flag3)
						{
							this.LeaveSquad(Service.Get<Lang>().Get("CURRENT_PLAYER_EJECTED", new object[0]));
						}
						else
						{
							SquadUtils.RemoveSquadMember(currentSquad, squadMsg.OwnerData.PlayerId);
						}
						flag = true;
						break;
					case SquadMsgType.Promotion:
					case SquadMsgType.Demotion:
					{
						SquadRole role = (squadMsg.MemberData != null) ? squadMsg.MemberData.MemberRole : SquadRole.Member;
						if (flag4)
						{
							this.StateManager.Role = role;
						}
						SquadUtils.SetSquadMemberRole(currentSquad, squadMsg.OwnerData.PlayerId, role);
						break;
					}
					case SquadMsgType.TroopDonation:
						if (squadMsg.DonationData != null && squadMsg.DonationData.RecipientId == playerId)
						{
							if (!string.IsNullOrEmpty(squadMsg.DonationData.RequestId))
							{
								SquadMsg msgById = this.MsgManager.GetMsgById(squadMsg.DonationData.RequestId);
								if (msgById != null && msgById.RequestData != null && msgById.RequestData.IsWarRequest)
								{
									flag = true;
									break;
								}
							}
							string text3 = (squadMsg.OwnerData != null) ? squadMsg.OwnerData.PlayerId : null;
							Dictionary<string, int> donations = squadMsg.DonationData.Donations;
							if (text3 != null && donations != null)
							{
								SquadMember squadMemberById = SquadUtils.GetSquadMemberById(currentSquad, text3);
								string donorName = (squadMemberById != null) ? squadMemberById.MemberName : null;
								this.StateManager.OnSquadTroopsReceived(donations, text3, donorName);
							}
						}
						break;
					case SquadMsgType.WarMatchMakingBegin:
						flag = true;
						this.WarManager.OnWarMatchMakingBegin();
						break;
					case SquadMsgType.WarMatchMakingCancel:
						if (!flag4)
						{
							this.WarManager.CancelMatchMaking();
						}
						flag = true;
						break;
					case SquadMsgType.WarStarted:
					case SquadMsgType.WarBuffBaseAttackStart:
					case SquadMsgType.WarBuffBaseAttackComplete:
					case SquadMsgType.WarPlayerAttackStart:
					case SquadMsgType.WarPlayerAttackComplete:
					case SquadMsgType.WarEnded:
						this.WarManager.HandleWarEventMsg(squadMsg);
						break;
					case SquadMsgType.WarPrepared:
						flag = true;
						break;
					case SquadMsgType.SquadLevelUp:
						flag2 = true;
						this.OnSquadLeveledUp(squadMsg);
						break;
					case SquadMsgType.PerkUnlocked:
						flag2 = true;
						this.OnPerkUnlocked(squadMsg);
						break;
					case SquadMsgType.PerkUpgraded:
						flag2 = true;
						this.OnPerkUpgraded(squadMsg);
						break;
					case SquadMsgType.PerkInvest:
						flag2 = true;
						this.OnPerkInvestment(squadMsg);
						break;
					case SquadMsgType.Invite:
						if (squadMsg.FriendInviteData != null & flag3)
						{
							SquadInvite invite = SquadMsgUtils.GenerateSquadInvite(squadMsg);
							this.StateManager.AddSquadInvite(invite);
						}
						break;
					case SquadMsgType.InviteRejected:
					{
						string text4 = (squadMsg.FriendInviteData != null) ? squadMsg.FriendInviteData.PlayerId : null;
						if (text4 != null & flag3)
						{
							this.StateManager.PlayersInvitedToSquad.Remove(text4);
						}
						break;
					}
					}
					SquadMsgType[] array = null;
					switch (squadMsg.Type)
					{
					case SquadMsgType.JoinRequestAccepted:
					case SquadMsgType.JoinRequestRejected:
						array = new SquadMsgType[]
						{
							SquadMsgType.JoinRequest
						};
						break;
					case SquadMsgType.Leave:
					case SquadMsgType.Ejected:
						array = new SquadMsgType[]
						{
							SquadMsgType.JoinRequest,
							SquadMsgType.ShareBattle,
							SquadMsgType.ShareLink,
							SquadMsgType.TroopRequest
						};
						break;
					}
					if (array != null && squadMsg.OwnerData != null)
					{
						this.MsgManager.RemoveMsgsByType(squadMsg.OwnerData.PlayerId, array);
					}
				}
				i++;
			}
			if (flag2)
			{
				Service.Get<PerkViewController>().UpdateLastViewedPerkTime();
			}
			if (flag)
			{
				this.UpdateCurrentSquad();
			}
		}

		public void UpdateCurrentSquadWar()
		{
			this.ServerManager.UpdateCurrentSquadWar();
		}

		public void UpdateCurrentSquad()
		{
			this.ServerManager.UpdateCurrentSquad();
		}

		private void UpdateSquadInvitesReceived()
		{
			this.ServerManager.UpdateSquadInvitesReceived();
		}

		public void HandleSquadInvitesReceived(List<SquadInvite> invites)
		{
			this.StateManager.AddSquadInvites(invites);
		}

		public void InitSquadWarState(SquadWarData warData)
		{
			if (warData != null)
			{
				this.WarManager.UpdateSquadWar(warData);
			}
		}

		public void SetLastViewedSquadLevelUp(int squadLevel)
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			sharedPlayerPrefs.SetPref("HighestViewedSquadLvlUP", squadLevel.ToString());
		}

		public int GetLastViewedSquadLevelUp()
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			return sharedPlayerPrefs.GetPref<int>("HighestViewedSquadLvlUP");
		}

		public bool HavePendingSquadLevelCelebration()
		{
			Squad currentSquad = this.StateManager.GetCurrentSquad();
			int num = 0;
			int num2 = 0;
			if (currentSquad != null)
			{
				num = currentSquad.Level;
				num2 = this.GetLastViewedSquadLevelUp();
			}
			return num > 1 && num2 < num;
		}

		private void OnSquadLeveledUp(SquadMsg msg)
		{
			Squad currentSquad = this.StateManager.GetCurrentSquad();
			SqmSquadData squadData = msg.SquadData;
			int level = squadData.Level;
			if (level > currentSquad.Level)
			{
				currentSquad.Level = level;
			}
			currentSquad.TotalRepInvested = squadData.TotalRepInvested;
			Service.Get<EventManager>().SendEvent(EventId.SquadLeveledUp, null);
		}

		private void OnPerkInvestment(SquadMsg msg)
		{
			Squad currentSquad = this.StateManager.GetCurrentSquad();
			SquadPerks perks = currentSquad.Perks;
			SqmSquadData squadData = msg.SquadData;
			SqmPerkData perkData = msg.PerkData;
			int level = squadData.Level;
			if (level > currentSquad.Level)
			{
				currentSquad.Level = level;
			}
			currentSquad.TotalRepInvested = squadData.TotalRepInvested;
			perks.UpdatePerkInvestedAmt(perkData.PerkUId, perkData.PerkInvestedAmt);
			Service.Get<EventManager>().SendEvent(EventId.SquadPerkUpdated, null);
			Service.Get<EventManager>().SendEvent(EventId.PerkInvestment, null);
		}

		private void OnPerkUnlocked(SquadMsg msg)
		{
			IDataController dataController = Service.Get<IDataController>();
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				Service.Get<StaRTSLogger>().Error("SquadController.OnPerkUnlocked: Current Squad Null");
				return;
			}
			SquadPerks perks = currentSquad.Perks;
			string perkUId = msg.PerkData.PerkUId;
			PerkVO optional = dataController.GetOptional<PerkVO>(perkUId);
			if (optional == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("SquadController.OnPerkUnlocked: Given Perk is Null {0}", new object[]
				{
					perkUId
				});
				return;
			}
			perks.UpdateUnlockedPerk(perkUId);
			Service.Get<PerkViewController>().AddToPerkBadgeList(perkUId);
			Service.Get<EventManager>().SendEvent(EventId.SquadPerkUpdated, null);
			Service.Get<EventManager>().SendEvent(EventId.PerkUnlocked, optional);
		}

		private void OnPerkUpgraded(SquadMsg msg)
		{
			IDataController dataController = Service.Get<IDataController>();
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				Service.Get<StaRTSLogger>().Error("SquadController.OnPerkUpgraded: Current Squad Null");
				return;
			}
			SquadPerks perks = currentSquad.Perks;
			string perkUId = msg.PerkData.PerkUId;
			PerkVO optional = dataController.GetOptional<PerkVO>(perkUId);
			if (optional == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("SquadController.OnPerkUpgraded: Given Perk is Null {0}", new object[]
				{
					perkUId
				});
				return;
			}
			perks.UpdateUnlockedPerk(perkUId);
			Service.Get<PerkViewController>().AddToPerkBadgeList(perkUId);
			Service.Get<EventManager>().SendEvent(EventId.SquadPerkUpdated, null);
			Service.Get<EventManager>().SendEvent(EventId.PerkUpgraded, optional);
		}

		private void LeaveSquad(string message)
		{
			this.StateManager.Troops.Clear();
			this.StateManager.NumRepDonatedInSession = 0;
			this.StateManager.PlayersInvitedToSquad.Clear();
			this.StateManager.SetCurrentSquad(null);
			this.MsgManager.ClearAllMsgs();
			this.UpdateSquadInvitesReceived();
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(message);
			Service.Get<EventManager>().SendEvent(EventId.SquadLeft, null);
			this.ClearPrefsForNewSquad(0);
		}

		public void CheckSquadInvitesSentToPlayers(List<string> playerIds, Action callback)
		{
			if (playerIds != null && playerIds.Count > 0)
			{
				this.ServerManager.UpdateSquadInvitesSentToPlayers(playerIds, callback);
				return;
			}
			if (callback != null)
			{
				callback.Invoke();
			}
		}

		public void HandleSquadInvitesSentToPlayers(List<string> playerIds, Action callback)
		{
			if (playerIds != null)
			{
				HashSet<string> playersInvitedToSquad = this.StateManager.PlayersInvitedToSquad;
				int i = 0;
				int count = playerIds.Count;
				while (i < count)
				{
					if (!playersInvitedToSquad.Contains(playerIds[i]))
					{
						playersInvitedToSquad.Add(playerIds[i]);
					}
					i++;
				}
			}
			if (callback != null)
			{
				callback.Invoke();
			}
		}

		public void PublishChatMessage(string message)
		{
			this.ServerManager.PublishChatMessage(message);
		}

		public void SyncCurrentPlayerRole()
		{
			SquadMember squadMemberById = SquadUtils.GetSquadMemberById(this.StateManager.GetCurrentSquad(), Service.Get<CurrentPlayer>().PlayerId);
			if (squadMemberById != null)
			{
				this.StateManager.Role = squadMemberById.Role;
				if (this.StateManager.JoinDate == 0u)
				{
					this.StateManager.JoinDate = squadMemberById.JoinDate;
				}
			}
		}

		public void SyncCurrentPlayerPlanet()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			SquadMember squadMemberById = SquadUtils.GetSquadMemberById(this.StateManager.GetCurrentSquad(), currentPlayer.PlayerId);
			if (squadMemberById != null)
			{
				squadMemberById.Planet = currentPlayer.PlanetId;
			}
		}

		public void ShowTroopRequestScreen(string requestText, bool isWarRequest)
		{
			Squad currentSquad = this.StateManager.GetCurrentSquad();
			if (currentSquad != null)
			{
				if (currentSquad.MemberCount <= 1)
				{
					Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(Service.Get<Lang>().Get("s_noSquadmatesToDonate", new object[0]));
					return;
				}
				if (string.IsNullOrEmpty(requestText))
				{
					if (isWarRequest)
					{
						requestText = Service.Get<Lang>().Get("REQUEST_WAR_TROOPS_DEFAULT", new object[0]);
					}
					else
					{
						requestText = Service.Get<Lang>().Get("REQUEST_TROOPS_DEFAULT", new object[0]);
					}
				}
				SquadTroopRequestScreen squadTroopRequestScreen = new SquadTroopRequestScreen(requestText, isWarRequest);
				squadTroopRequestScreen.IsAlwaysOnTop = true;
				Service.Get<ScreenController>().AddScreen(squadTroopRequestScreen);
			}
		}

		public void SendTroopRequest(string requestText, bool isWarRequest = false)
		{
			bool flag = true;
			bool payToSkip = false;
			uint serverTime = Service.Get<ServerAPI>().ServerTime;
			SquadController squadController = Service.Get<SquadController>();
			uint troopRequestDate;
			if (isWarRequest)
			{
				troopRequestDate = squadController.StateManager.WarTroopRequestDate;
			}
			else
			{
				troopRequestDate = squadController.StateManager.TroopRequestDate;
			}
			int troopRequestCrystalCost = SquadUtils.GetTroopRequestCrystalCost(serverTime, troopRequestDate);
			if (troopRequestCrystalCost > 0)
			{
				payToSkip = true;
				flag = GameUtils.SpendCrystals(troopRequestCrystalCost);
			}
			if (flag)
			{
				string empty = string.Empty;
				string itemId = "requestTroops";
				int itemCount = 1;
				string type = "squad_action";
				string subType = "consumable";
				Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(-troopRequestCrystalCost, empty, itemId, itemCount, type, subType);
				if (string.IsNullOrEmpty(requestText))
				{
					if (isWarRequest)
					{
						requestText = Service.Get<Lang>().Get("REQUEST_WAR_TROOPS_DEFAULT", new object[0]);
					}
					else
					{
						requestText = Service.Get<Lang>().Get("REQUEST_TROOPS_DEFAULT", new object[0]);
					}
				}
				SquadMsg message;
				if (isWarRequest)
				{
					message = SquadMsgUtils.CreateRequestWarTroopsMessage(payToSkip, troopRequestCrystalCost, requestText);
					Service.Get<EventManager>().SendEvent(EventId.SquadWarTroopsRequestStartedByCurrentPlayer, null);
				}
				else
				{
					message = SquadMsgUtils.CreateRequestTroopsMessage(payToSkip, troopRequestCrystalCost, requestText);
				}
				this.TakeAction(message);
			}
			Service.Get<EventManager>().SendEvent(EventId.SquadEdited, null);
		}

		public void Destroy()
		{
			this.WarManager.Destroy();
			this.ServerManager.Destroy();
			this.MsgManager.Destroy();
			this.StateManager.Destroy();
		}

		protected internal SquadController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).CheckSquadInvitesSentToPlayers((List<string>)GCHandledObjects.GCHandleToObject(*args), (Action)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).ClearPrefsForNewSquad(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).Enable();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).BuffManager);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).MessageLimit);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).MsgManager);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).PullFrequencyChatClosed);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).PullFrequencyChatOpen);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).ServerManager);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).StateManager);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).WarManager);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).GetLastViewedSquadLevelUp());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).HandleSquadInvitesReceived((List<SquadInvite>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).HandleSquadInvitesSentToPlayers((List<string>)GCHandledObjects.GCHandleToObject(*args), (Action)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadController)GCHandledObjects.GCHandleToObject(instance)).HavePendingSquadLevelCelebration());
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).InitSquadWarState((SquadWarData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).LeaveSquad(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).OnNewSquadMsgsReceived((List<SquadMsg>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).OnPerkInvestment((SquadMsg)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).OnPerkUnlocked((SquadMsg)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).OnPerkUpgraded((SquadMsg)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).OnPlayerActionSuccess((SquadAction)(*(int*)args), (SquadMsg)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).OnSquadLeveledUp((SquadMsg)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).PublishChatMessage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).SendTroopRequest(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).BuffManager = (SquadWarBuffManager)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).MessageLimit = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).MsgManager = (SquadMsgManager)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).PullFrequencyChatClosed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).PullFrequencyChatOpen = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).ServerManager = (SquadServerManager)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).StateManager = (SquadStateManager)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).WarManager = (SquadWarManager)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).SetLastViewedSquadLevelUp(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).ShowTroopRequestScreen(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).SyncCurrentPlayerPlanet();
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).SyncCurrentPlayerRole();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).TakeAction((SquadMsg)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentSquad();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentSquadWar();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((SquadController)GCHandledObjects.GCHandleToObject(instance)).UpdateSquadInvitesReceived();
			return -1L;
		}
	}
}
