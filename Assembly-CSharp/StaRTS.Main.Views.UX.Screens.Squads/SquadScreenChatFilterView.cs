using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils.Chat;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadScreenChatFilterView : AbstractSquadScreenViewModule
	{
		private const string SHOW_ALL = "s_ShowAll";

		private const string SHOW_MSG = "s_Messages";

		private const string SHOW_REQ = "s_Requests";

		private const string SHOW_REPLAY = "s_Replays";

		private const string SHOW_UPDATES = "s_Updates";

		private const string SHOW_VIDEOS = "s_Videos";

		private const string BTN_CHAT_FILTER_SHOWALL = "BtnShowAll";

		private const string BTN_CHAT_FILTER_SHOWMSG = "BtnMessages";

		private const string BTN_CHAT_FILTER_SHOWREQUESTS = "BtnRequests";

		private const string BTN_CHAT_FILTER_SHOWREPLAYS = "BtnReplays";

		private const string BTN_CHAT_FILTER_SHOWUPDATES = "BtnUpdates";

		private const string BTN_CHAT_FILTER_SHOWVIDEOS = "BtnVideos";

		private const string SPR_CHAT_FILTER_BG = "SpriteFilterBg";

		private const string SPR_CHAT_FILTER_BG_NO_VIDEO = "SpriteFilterBg2";

		private const string BTN_OPEN_CHAT_FILTER = "BtnFilterChat";

		private const string LABEL_FILTER_CHAT = "LabelBtnFilterChat";

		public const string FILTERTYPE_PREF_KEY = "ChatFilterType";

		public const string MSGCOUNT_PREF_KEY = "ChatFilterMessageCount";

		public const string REQCOUNT_PREF_KEY = "ChatFilterRequestCount";

		public const string REPLAYCOUNT_PREF_KEY = "ChatFilterReplayCount";

		public const string UPDATECOUNT_PREF_KEY = "ChatFilterUpdateCount";

		public const string VIDEOCOUNT_PREF_KEY = "ChatFilterVideoCount";

		private const string CHAT_FILTER_OPTIONS_PANEL = "ChatFilterOptionsPanel";

		private UXElement chatFilterPanel;

		private UXSprite chatFilterBG;

		private UXSprite chatFilterBGNoVideo;

		private UXCheckbox chatShowAllBtn;

		private UXCheckbox chatShowMsgBtn;

		private UXCheckbox chatShowRequestsBtn;

		private UXCheckbox chatShowReplaysBtn;

		private UXCheckbox chatShowUpdatesBtn;

		private UXCheckbox chatShowVideosBtn;

		private UXButton chatOpenFiltersButton;

		private UXLabel chatOpenFiltersLabel;

		private UXLabel chatFilterVideosLabel;

		private int totalChatCount;

		private int msgCount;

		private int requestCount;

		private int replayCount;

		private int updateCount;

		private int videoCount;

		private JewelControl chatBadge;

		private JewelControl messageBadge;

		private JewelControl requestsBadge;

		private JewelControl replaysBadge;

		private JewelControl updatesBadge;

		private JewelControl videosBadge;

		public SquadScreenChatFilterView(SquadSlidingScreen screen) : base(screen)
		{
		}

		public override void OnScreenLoaded()
		{
			this.chatFilterPanel = this.screen.GetElement<UXElement>("ChatFilterOptionsPanel");
			this.chatFilterPanel.Visible = false;
			this.chatShowAllBtn = this.screen.GetElement<UXCheckbox>("BtnShowAll");
			this.chatShowAllBtn.Tag = ChatFilterType.ShowAll;
			this.chatShowAllBtn.OnSelected = new UXCheckboxSelectedDelegate(this.OnChatFilterSelected);
			this.chatShowMsgBtn = this.screen.GetElement<UXCheckbox>("BtnMessages");
			this.chatShowMsgBtn.Tag = ChatFilterType.Messages;
			this.chatShowMsgBtn.OnSelected = new UXCheckboxSelectedDelegate(this.OnChatFilterSelected);
			this.chatShowRequestsBtn = this.screen.GetElement<UXCheckbox>("BtnRequests");
			this.chatShowRequestsBtn.Tag = ChatFilterType.Requests;
			this.chatShowRequestsBtn.OnSelected = new UXCheckboxSelectedDelegate(this.OnChatFilterSelected);
			this.chatShowReplaysBtn = this.screen.GetElement<UXCheckbox>("BtnReplays");
			this.chatShowReplaysBtn.Tag = ChatFilterType.Replays;
			this.chatShowReplaysBtn.OnSelected = new UXCheckboxSelectedDelegate(this.OnChatFilterSelected);
			this.chatShowUpdatesBtn = this.screen.GetElement<UXCheckbox>("BtnUpdates");
			this.chatShowUpdatesBtn.Tag = ChatFilterType.Updates;
			this.chatShowUpdatesBtn.OnSelected = new UXCheckboxSelectedDelegate(this.OnChatFilterSelected);
			this.chatShowVideosBtn = this.screen.GetElement<UXCheckbox>("BtnVideos");
			this.chatShowVideosBtn.Tag = ChatFilterType.Videos;
			this.chatShowVideosBtn.OnSelected = new UXCheckboxSelectedDelegate(this.OnChatFilterSelected);
			this.chatOpenFiltersButton = this.screen.GetElement<UXButton>("BtnFilterChat");
			this.chatOpenFiltersButton.OnClicked = new UXButtonClickedDelegate(this.OnChatFilterButton);
			this.chatOpenFiltersLabel = this.screen.GetElement<UXLabel>("LabelBtnFilterChat");
			this.chatFilterBG = this.screen.GetElement<UXSprite>("SpriteFilterBg");
			this.chatFilterBGNoVideo = this.screen.GetElement<UXSprite>("SpriteFilterBg2");
			if (GameConstants.IsMakerVideoEnabled())
			{
				this.chatFilterBG.Visible = true;
				this.chatFilterBGNoVideo.Visible = false;
				this.chatShowVideosBtn.Visible = true;
			}
			else
			{
				this.chatFilterBG.Visible = false;
				this.chatFilterBGNoVideo.Visible = true;
				this.chatShowVideosBtn.Visible = false;
			}
			this.chatBadge = JewelControl.Create(this.screen, "SocialChat");
			this.messageBadge = JewelControl.Create(this.screen, "Messages");
			this.requestsBadge = JewelControl.Create(this.screen, "Requests");
			this.replaysBadge = JewelControl.Create(this.screen, "Replays");
			this.updatesBadge = JewelControl.Create(this.screen, "Updates");
			this.videosBadge = JewelControl.Create(this.screen, "Videos");
			SquadController squadController = Service.Get<SquadController>();
			uint squadPlayerPref = this.GetSquadPlayerPref("ChatFilterType", 1u);
			squadController.StateManager.SetSquadScreenChatFilterType((ChatFilterType)squadPlayerPref);
			this.RefreshView();
		}

		public override void ShowView()
		{
			this.chatFilterPanel.Visible = true;
			this.RefreshView();
		}

		public override void HideView()
		{
			this.chatFilterPanel.Visible = false;
		}

		public override void RefreshView()
		{
			SquadController squadController = Service.Get<SquadController>();
			switch (squadController.StateManager.GetSquadScreenChatFilterType())
			{
			case ChatFilterType.ShowAll:
				this.chatOpenFiltersLabel.Text = this.lang.Get("s_ShowAll", new object[0]);
				return;
			case ChatFilterType.Messages:
				this.chatOpenFiltersLabel.Text = this.lang.Get("s_Messages", new object[0]);
				return;
			case ChatFilterType.Requests:
				this.chatOpenFiltersLabel.Text = this.lang.Get("s_Requests", new object[0]);
				return;
			case ChatFilterType.Replays:
				this.chatOpenFiltersLabel.Text = this.lang.Get("s_Replays", new object[0]);
				return;
			case ChatFilterType.Updates:
				this.chatOpenFiltersLabel.Text = this.lang.Get("s_Updates", new object[0]);
				return;
			case ChatFilterType.Videos:
				this.chatOpenFiltersLabel.Text = this.lang.Get("s_Videos", new object[0]);
				return;
			default:
				return;
			}
		}

		public override void OnDestroyElement()
		{
		}

		private void OnChatFilterButton(UXButton btn)
		{
			this.ToggleView();
		}

		private void ToggleView()
		{
			if (this.chatFilterPanel.Visible)
			{
				this.HideView();
				return;
			}
			this.ShowView();
		}

		private void OnChatFilterSelected(UXCheckbox chatFilterOption, bool selected)
		{
			if (selected)
			{
				chatFilterOption.Selected = false;
				this.HandleFilterSelection((ChatFilterType)chatFilterOption.Tag, false);
			}
			this.HideView();
		}

		public void OnChatViewOpened()
		{
			SquadController squadController = Service.Get<SquadController>();
			this.HandleFilterSelection(squadController.StateManager.GetSquadScreenChatFilterType(), true);
		}

		private void HandleFilterSelection(ChatFilterType type, bool forceViewTimeUpdate)
		{
			SquadController squadController = Service.Get<SquadController>();
			squadController.StateManager.SetSquadScreenChatFilterType(type);
			this.SetSquadPlayerPref("ChatFilterType", (uint)type);
			this.RefreshView();
			uint unixTimestamp = ChatTimeConversionUtils.GetUnixTimestamp();
			if (type == ChatFilterType.ShowAll)
			{
				this.SetLastViewedTime(ChatFilterType.Messages, unixTimestamp, forceViewTimeUpdate);
				this.SetLastViewedTime(ChatFilterType.Requests, unixTimestamp, forceViewTimeUpdate);
				this.SetLastViewedTime(ChatFilterType.Replays, unixTimestamp, forceViewTimeUpdate);
				this.SetLastViewedTime(ChatFilterType.Updates, unixTimestamp, forceViewTimeUpdate);
				this.SetLastViewedTime(ChatFilterType.Videos, unixTimestamp, forceViewTimeUpdate);
			}
			this.SetLastViewedTime(type, unixTimestamp, forceViewTimeUpdate);
			this.UpdateBadges();
		}

		public int UpdateBadges()
		{
			this.CountChatMessages();
			this.chatBadge.Value = this.totalChatCount;
			this.messageBadge.Value = this.msgCount;
			this.requestsBadge.Value = this.requestCount;
			this.replaysBadge.Value = this.replayCount;
			this.updatesBadge.Value = this.updateCount;
			this.videosBadge.Value = this.videoCount;
			return this.totalChatCount;
		}

		private void IncrementBadgeByMessageType(SquadMsgType type)
		{
			switch (type)
			{
			case SquadMsgType.Chat:
				this.msgCount++;
				return;
			case SquadMsgType.Join:
			case SquadMsgType.JoinRequest:
			case SquadMsgType.JoinRequestAccepted:
			case SquadMsgType.JoinRequestRejected:
			case SquadMsgType.InviteAccepted:
			case SquadMsgType.Leave:
			case SquadMsgType.Ejected:
			case SquadMsgType.Promotion:
			case SquadMsgType.Demotion:
			case SquadMsgType.WarMatchMakingBegin:
				this.updateCount++;
				return;
			case SquadMsgType.ShareBattle:
				this.replayCount++;
				return;
			case SquadMsgType.ShareLink:
				this.videoCount++;
				return;
			case SquadMsgType.TroopRequest:
			case SquadMsgType.TroopDonation:
				this.requestCount++;
				return;
			default:
				return;
			}
		}

		private void CountChatMessages()
		{
			this.msgCount = 0;
			this.requestCount = 0;
			this.replayCount = 0;
			this.updateCount = 0;
			this.videoCount = 0;
			this.totalChatCount = 0;
			SquadController squadController = Service.Get<SquadController>();
			List<SquadMsg> existingMessages = squadController.MsgManager.GetExistingMessages();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int i = 0;
			int count = existingMessages.Count;
			while (i < count)
			{
				SquadMsg squadMsg = existingMessages[i];
				if (squadMsg != null && squadMsg.OwnerData != null && squadMsg.OwnerData.PlayerId != currentPlayer.PlayerId && squadMsg.TimeSent > this.GetLastViewedTimeByMessageType(squadMsg.Type))
				{
					this.IncrementBadgeByMessageType(squadMsg.Type);
				}
				i++;
			}
			this.totalChatCount = this.msgCount + this.requestCount + this.replayCount + this.updateCount + this.videoCount;
		}

		private string GetPrefsKeyByType(ChatFilterType type)
		{
			string result = string.Empty;
			switch (type)
			{
			case ChatFilterType.Messages:
				result = "ChatFilterMessageCount";
				break;
			case ChatFilterType.Requests:
				result = "ChatFilterRequestCount";
				break;
			case ChatFilterType.Replays:
				result = "ChatFilterReplayCount";
				break;
			case ChatFilterType.Updates:
				result = "ChatFilterUpdateCount";
				break;
			case ChatFilterType.Videos:
				result = "ChatFilterVideoCount";
				break;
			}
			return result;
		}

		private uint GetLastViewedTimeByMessageType(SquadMsgType type)
		{
			ChatFilterType type2 = ChatFilterType.Invalid;
			switch (type)
			{
			case SquadMsgType.Chat:
				type2 = ChatFilterType.Messages;
				break;
			case SquadMsgType.Join:
			case SquadMsgType.JoinRequest:
			case SquadMsgType.JoinRequestAccepted:
			case SquadMsgType.JoinRequestRejected:
			case SquadMsgType.InviteAccepted:
			case SquadMsgType.Leave:
			case SquadMsgType.Ejected:
			case SquadMsgType.Promotion:
			case SquadMsgType.Demotion:
			case SquadMsgType.WarMatchMakingBegin:
				type2 = ChatFilterType.Updates;
				break;
			case SquadMsgType.ShareBattle:
				type2 = ChatFilterType.Replays;
				break;
			case SquadMsgType.ShareLink:
				type2 = ChatFilterType.Videos;
				break;
			case SquadMsgType.TroopRequest:
			case SquadMsgType.TroopDonation:
				type2 = ChatFilterType.Requests;
				break;
			}
			string prefsKeyByType = this.GetPrefsKeyByType(type2);
			return this.GetSquadPlayerPref(prefsKeyByType, 0u);
		}

		private void SetLastViewedTime(ChatFilterType type, uint data, bool forceUpdate)
		{
			if ((this.screen.IsOpen() || this.screen.IsOpening()) | forceUpdate)
			{
				string prefsKeyByType = this.GetPrefsKeyByType(type);
				this.SetSquadPlayerPref(prefsKeyByType, data);
			}
		}

		private void SetSquadPlayerPref(string key, uint value)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			UserPlayerPrefsController.SetInt(key + currentPlayer.Faction, (int)value);
		}

		private uint GetSquadPlayerPref(string key, uint defaultValue)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return (uint)UserPlayerPrefsController.GetInt(key + currentPlayer.Faction, (int)defaultValue);
		}

		public override bool IsVisible()
		{
			return this.chatFilterPanel.Visible;
		}

		protected internal SquadScreenChatFilterView(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).CountChatMessages();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).GetPrefsKeyByType((ChatFilterType)(*(int*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).HandleFilterSelection((ChatFilterType)(*(int*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).HideView();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).IncrementBadgeByMessageType((SquadMsgType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).IsVisible());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).OnChatFilterButton((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).OnChatFilterSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).OnChatViewOpened();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).ShowView();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).ToggleView();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenChatFilterView)GCHandledObjects.GCHandleToObject(instance)).UpdateBadges());
		}
	}
}
