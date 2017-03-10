using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Squads;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadScreenChatView : AbstractSquadScreenViewModule
	{
		private const string CHAT_CONTAINER = "ChatContainer";

		private const string TAB_BUTTON_NAME = "SocialChatBtn";

		private const string CHAT_TABLE = "ChatTable";

		public const string CHAT = "chat";

		private const int NUM_CREATE_PER_FRAME = 10;

		private const int NUM_DESTROY_PER_FRAME = 10;

		private const float FRAME_DELAY_TIME = 0.01f;

		private const float DESTROY_DELAY_TIME = 0.5f;

		private UXElement chatContainer;

		private UXCheckbox tabButton;

		private SquadMsgChatDisplay chatDisplay;

		private SquadScreenChatFilterView chatFilter;

		private SquadScreenChatInputView chatInput;

		private SquadScreenChatTroopDonationProgressView donationProgress;

		private int numExistingMsgsProcessed;

		private HashSet<SquadMsg> existingMsgsProcessed;

		private uint timestampUpdateTimerId;

		private const float CHAT_TIMESTAP_UPDATE_INTERVAL = 60f;

		public bool ChatDisplaySetup
		{
			get;
			private set;
		}

		public SquadScreenChatView(SquadSlidingScreen screen) : base(screen)
		{
			this.timestampUpdateTimerId = 0u;
			this.chatFilter = new SquadScreenChatFilterView(screen);
			this.chatInput = new SquadScreenChatInputView(screen);
			this.donationProgress = new SquadScreenChatTroopDonationProgressView(screen);
			this.ChatDisplaySetup = false;
			this.numExistingMsgsProcessed = 0;
			this.existingMsgsProcessed = null;
		}

		public override void OnScreenLoaded()
		{
			this.chatContainer = this.screen.GetElement<UXElement>("ChatContainer");
			this.tabButton = this.screen.GetElement<UXCheckbox>("SocialChatBtn");
			this.tabButton.OnSelected = new UXCheckboxSelectedDelegate(this.OnTabButtonSelected);
			this.chatFilter.OnScreenLoaded();
			this.chatInput.OnScreenLoaded();
			this.donationProgress.OnScreenLoaded();
			this.chatDisplay = new SquadMsgChatDisplay(this.screen, this.screen.GetElement<UXTable>("ChatTable"));
			Service.Get<SquadController>().MsgManager.RegisterObserver(this.chatDisplay);
		}

		public override void ShowView()
		{
			this.chatContainer.Visible = true;
			Service.Get<EventManager>().SendEvent(EventId.SquadSelect, null);
			Service.Get<EventManager>().SendEvent(EventId.UISquadScreenTabShown, "chat");
			this.timestampUpdateTimerId = Service.Get<ViewTimerManager>().CreateViewTimer(60f, true, new TimerDelegate(this.OnTimestampUpdateTimer), null);
			this.chatInput.ShowView();
			this.chatFilter.OnChatViewOpened();
			this.donationProgress.ShowView();
			this.tabButton.Selected = true;
		}

		public void SetupChatDisplay()
		{
			if (!this.ChatDisplaySetup)
			{
				this.ChatDisplaySetup = true;
				if (Service.Get<SquadController>().MsgManager.GetExistingMessages().Count > 0)
				{
					ProcessingScreen.Show();
					this.numExistingMsgsProcessed = 0;
					this.existingMsgsProcessed = new HashSet<SquadMsg>();
					this.SetupChatDisplayFrameDelayed();
				}
			}
		}

		public override void HideView()
		{
			this.chatContainer.Visible = false;
			if (this.timestampUpdateTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.timestampUpdateTimerId);
				this.timestampUpdateTimerId = 0u;
			}
			this.chatFilter.HideView();
			this.chatInput.HideView();
			this.donationProgress.HideView();
			this.tabButton.Selected = false;
		}

		public override void RefreshView()
		{
		}

		private void OnTimestampUpdateTimer(uint id, object cookie)
		{
			this.chatDisplay.UpdateAllTimestamps();
		}

		private void OnTabButtonSelected(UXCheckbox checkbox, bool selected)
		{
			if (selected)
			{
				SquadController squadController = Service.Get<SquadController>();
				squadController.StateManager.SquadScreenState = SquadScreenState.Chat;
				this.screen.RefreshViews();
			}
		}

		public override void OnDestroyElement()
		{
			this.chatFilter.OnDestroyElement();
			this.chatInput.OnDestroyElement();
			this.donationProgress.OnDestroyElement();
			Service.Get<SquadController>().MsgManager.UnregisterObserver(this.chatDisplay);
			if (this.timestampUpdateTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.timestampUpdateTimerId);
				this.timestampUpdateTimerId = 0u;
			}
			this.chatDisplay.Cleanup();
			if (this.ChatDisplaySetup)
			{
				this.DestroyChatDisplayFrameDelayed();
				this.ChatDisplaySetup = false;
			}
			if (this.existingMsgsProcessed != null)
			{
				this.existingMsgsProcessed.Clear();
				this.existingMsgsProcessed = null;
			}
			this.numExistingMsgsProcessed = 0;
		}

		public int UpdateBadges()
		{
			return this.chatFilter.UpdateBadges();
		}

		public override bool IsVisible()
		{
			return this.chatContainer.Visible;
		}

		private void SetupChatDisplayFrameDelayed()
		{
			this.KillExistingTimers();
			uint item = Service.Get<ViewTimerManager>().CreateViewTimer(0.01f, true, new TimerDelegate(this.OnSetupChatDisplayTimer), null);
			Service.Get<SquadController>().StateManager.SquadScreenTimers.Add(item);
		}

		private void OnSetupChatDisplayTimer(uint timerId, object cookie)
		{
			if (this.chatDisplay != null)
			{
				List<SquadMsg> existingMessages = Service.Get<SquadController>().MsgManager.GetExistingMessages();
				int count = existingMessages.Count;
				int num = count - this.numExistingMsgsProcessed;
				if (num > 10)
				{
					num = 10;
				}
				if (num > 0)
				{
					int i = this.numExistingMsgsProcessed;
					int num2 = this.numExistingMsgsProcessed + num;
					while (i < num2)
					{
						SquadMsg item = existingMessages[i];
						if (!this.existingMsgsProcessed.Contains(item))
						{
							this.numExistingMsgsProcessed++;
							this.chatDisplay.ProcessMessage(existingMessages[i], false);
							this.existingMsgsProcessed.Add(existingMessages[i]);
						}
						i++;
					}
					return;
				}
				this.KillExistingTimers();
				this.chatDisplay.OnExistingMessagesSetup();
				ProcessingScreen.Hide();
			}
		}

		private void DestroyChatDisplayFrameDelayed()
		{
			this.KillExistingTimers();
			uint item = Service.Get<ViewTimerManager>().CreateViewTimer(0.5f, false, new TimerDelegate(this.OnStartChatDisplayDestroyTimer), null);
			Service.Get<SquadController>().StateManager.SquadScreenTimers.Add(item);
		}

		private void OnStartChatDisplayDestroyTimer(uint timerId, object cookie)
		{
			uint item = Service.Get<ViewTimerManager>().CreateViewTimer(0.01f, true, new TimerDelegate(this.OnDestroyChatDisplayTimer), null);
			Service.Get<SquadController>().StateManager.SquadScreenTimers.Remove(timerId);
			Service.Get<SquadController>().StateManager.SquadScreenTimers.Add(item);
		}

		private void OnDestroyChatDisplayTimer(uint timerId, object cookie)
		{
			if (this.chatDisplay != null && this.chatDisplay.RemoveElementsByCount(10) == 0)
			{
				this.KillExistingTimers();
				this.chatDisplay.Destroy();
				this.chatDisplay = null;
			}
		}

		private void KillExistingTimers()
		{
			ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
			List<uint> squadScreenTimers = Service.Get<SquadController>().StateManager.SquadScreenTimers;
			int i = 0;
			int count = squadScreenTimers.Count;
			while (i < count)
			{
				viewTimerManager.KillViewTimer(squadScreenTimers[i]);
				i++;
			}
			squadScreenTimers.Clear();
		}

		protected internal SquadScreenChatView(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).DestroyChatDisplayFrameDelayed();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).ChatDisplaySetup);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).HideView();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).IsVisible());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).KillExistingTimers();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).OnTabButtonSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).ChatDisplaySetup = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).SetupChatDisplay();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).SetupChatDisplayFrameDelayed();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).ShowView();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenChatView)GCHandledObjects.GCHandleToObject(instance)).UpdateBadges());
		}
	}
}
