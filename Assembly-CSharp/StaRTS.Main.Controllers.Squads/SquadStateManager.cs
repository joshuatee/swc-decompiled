using StaRTS.Externals.Manimal;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers.Squads
{
	public class SquadStateManager
	{
		public const SquadScreenState DEFAULT_SQUAD_SCREEN_STATE = SquadScreenState.Chat;

		public bool SquadScreenOpen;

		public SquadScreenState SquadScreenState;

		private ChatFilterType squadScreenChatFilterType;

		public SquadRole Role
		{
			get;
			set;
		}

		public uint JoinDate
		{
			get;
			set;
		}

		public uint TroopRequestDate
		{
			get;
			set;
		}

		public uint WarTroopRequestDate
		{
			get;
			set;
		}

		public int NumTroopDonationsInSession
		{
			get;
			set;
		}

		public int NumRepDonatedInSession
		{
			get;
			set;
		}

		public List<SquadDonatedTroop> Troops
		{
			get;
			set;
		}

		public List<SquadInvite> SquadInvites
		{
			get;
			private set;
		}

		public HashSet<string> PlayersInvitedToSquad
		{
			get;
			private set;
		}

		public HashSet<string> SquadJoinRequestsPending
		{
			get;
			private set;
		}

		public List<uint> SquadScreenTimers
		{
			get;
			private set;
		}

		public SquadStateManager()
		{
			this.SquadJoinRequestsPending = new HashSet<string>();
			this.PlayersInvitedToSquad = new HashSet<string>();
			this.SquadScreenTimers = new List<uint>();
		}

		public void Init()
		{
			this.SquadScreenState = SquadScreenState.Chat;
		}

		public void OnSquadJoinApplicationAcceptedByCurrentPlayer(string biSource)
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadJoinApplicationAcceptedByCurrentPlayer, biSource);
		}

		public void OnSquadJoinApplicationAccepted(string squadId)
		{
			if (this.SquadJoinRequestsPending.Contains(squadId))
			{
				this.SquadJoinRequestsPending.Remove(squadId);
			}
			Squad orCreateSquad = Service.Get<LeaderboardController>().GetOrCreateSquad(squadId);
			this.SetCurrentSquad(orCreateSquad);
			Service.Get<EventManager>().SendEvent(EventId.SquadJoinApplicationAccepted, orCreateSquad.SquadName);
		}

		public void OnSquadJoinApplicationRejected(string squadId)
		{
			if (this.SquadJoinRequestsPending.Contains(squadId))
			{
				this.SquadJoinRequestsPending.Remove(squadId);
			}
		}

		public void OnSquadJoinInviteAccepted()
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadJoinInviteAcceptedByCurrentPlayer, null);
		}

		public void OnSquadTroopsRequested()
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadTroopsRequestedByCurrentPlayer, null);
		}

		public void OnSquadWarTroopsRequested()
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadWarTroopsRequestedByCurrentPlayer, null);
		}

		public void OnSquadTroopsDonated(Dictionary<string, int> troopsActuallyDonated)
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadTroopsDonatedByCurrentPlayer, troopsActuallyDonated);
		}

		public void OnSquadTroopsReceived(Dictionary<string, int> troopsReceived, string donorId, string donorName)
		{
			if (this.Troops == null)
			{
				this.Troops = new List<SquadDonatedTroop>();
			}
			foreach (KeyValuePair<string, int> current in troopsReceived)
			{
				SquadDonatedTroop squadDonatedTroop = null;
				string key = current.Key;
				int value = current.Value;
				int i = 0;
				int count = this.Troops.Count;
				while (i < count)
				{
					if (this.Troops[i].TroopUid == key)
					{
						squadDonatedTroop = this.Troops[i];
						break;
					}
					i++;
				}
				if (squadDonatedTroop == null)
				{
					squadDonatedTroop = new SquadDonatedTroop(key);
					this.Troops.Add(squadDonatedTroop);
				}
				squadDonatedTroop.AddSenderAmount(donorId, value);
				Service.Get<EventManager>().SendEvent(EventId.SquadTroopsReceived, current);
			}
			Service.Get<EventManager>().SendEvent(EventId.SquadTroopsReceivedFromDonor, donorName);
		}

		public void OnSquadReplayShared(SqmReplayData replayData)
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadReplaySharedByCurrentPlayer, replayData);
		}

		public void AddSquadInvites(List<SquadInvite> invites)
		{
			if (this.SquadInvites == null)
			{
				this.SquadInvites = new List<SquadInvite>();
			}
			this.SquadInvites.AddRange(invites);
			Service.Get<EventManager>().SendEvent(EventId.SquadJoinInvitesReceived, invites);
		}

		public void AddSquadInvite(SquadInvite invite)
		{
			if (this.SquadInvites == null)
			{
				this.SquadInvites = new List<SquadInvite>();
			}
			this.SquadInvites.Add(invite);
			Service.Get<EventManager>().SendEvent(EventId.SquadJoinInviteReceived, invite);
		}

		public void RemoveInviteToSquad(string squadId)
		{
			if (this.SquadInvites != null)
			{
				int i = 0;
				int count = this.SquadInvites.Count;
				while (i < count)
				{
					if (this.SquadInvites[i].SquadId == squadId)
					{
						this.SquadInvites.RemoveAt(i);
						Service.Get<EventManager>().SendEvent(EventId.SquadJoinInviteRemoved, null);
						break;
					}
					i++;
				}
			}
		}

		public Squad GetCurrentSquad()
		{
			return Service.Get<CurrentPlayer>().Squad;
		}

		public void SetCurrentSquad(Squad squad)
		{
			Service.Get<CurrentPlayer>().Squad = squad;
			if (squad != null)
			{
				this.JoinDate = ServerTime.Time;
				Service.Get<SquadController>().SyncCurrentPlayerRole();
			}
			Service.Get<EventManager>().SendEvent(EventId.SquadUpdated, squad);
		}

		public ChatFilterType GetSquadScreenChatFilterType()
		{
			return this.squadScreenChatFilterType;
		}

		public void SetSquadScreenChatFilterType(ChatFilterType type)
		{
			this.squadScreenChatFilterType = type;
			Service.Get<EventManager>().SendEvent(EventId.SquadChatFilterUpdated, this.squadScreenChatFilterType);
		}

		public void OnSquadJoined(string biSource)
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadJoinedByCurrentPlayer, biSource);
		}

		public void EditSquad(bool open, string symbol, string description, int requiredTrophies)
		{
			Squad currentSquad = this.GetCurrentSquad();
			currentSquad.InviteType = ((!open) ? 0 : 1);
			currentSquad.Symbol = symbol;
			currentSquad.Description = description;
			currentSquad.RequiredTrophies = requiredTrophies;
		}

		public void SetSquadScreenOpen(bool open)
		{
			this.SquadScreenOpen = open;
			Service.Get<EventManager>().SendEvent(EventId.SquadScreenOpenedOrClosed, this.SquadScreenOpen);
		}

		public void Destroy()
		{
			if (this.Troops != null)
			{
				this.Troops.Clear();
				this.Troops = null;
			}
			if (this.SquadInvites != null)
			{
				this.SquadInvites.Clear();
				this.SquadInvites = null;
			}
			if (this.PlayersInvitedToSquad != null)
			{
				this.PlayersInvitedToSquad.Clear();
				this.PlayersInvitedToSquad = null;
			}
			if (this.SquadJoinRequestsPending != null)
			{
				this.SquadJoinRequestsPending.Clear();
				this.SquadJoinRequestsPending = null;
			}
		}
	}
}
