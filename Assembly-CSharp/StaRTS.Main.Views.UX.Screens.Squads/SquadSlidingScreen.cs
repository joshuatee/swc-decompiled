using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Perks;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadSlidingScreen : PersistentAnimatedScreen, IEventObserver
	{
		private const string CHAT_HOLDER_PANEL = "ChatHolderPanel";

		private const string OPEN_CLOSE_ANIMATION = "ChatHolder";

		private const string OPEN_CLOSE_STATE_ANIM = "anim_chat_display";

		private const string BTN_SQUAD_SCREEN_SLIDE = "ButtonChat";

		private const string SQUAD_MEMBERS = "Squad_Members";

		private const string BTN_CLOSE_SQUAD = "BtnCloseSquad";

		private const string BG_DIALOG = "BgDialog";

		private const string SQUAD_NAVIGATION = "SquadNavigation";

		private SquadScreenChatView chatView;

		private SquadScreenMembersView membersView;

		private SquadScreenOverviewView overviewView;

		private SquadScreenTroopDonationView troopDonationView;

		private SquadScreenWarButtonView warButtonView;

		private SquadScreenWarLogView warLogView;

		private SquadScreenAdvancementView advancementView;

		private UXElement chatHolderPanel;

		private UXButton squadSlideBtn;

		private UXElement bgDialog;

		private UXButton closeSquadBtn;

		private UXElement squadNavigation;

		private JewelControl squadBadge;

		public override bool ShowCurrencyTray
		{
			get
			{
				return true;
			}
		}

		public SquadSlidingScreen() : base("gui_squad")
		{
			this.chatView = new SquadScreenChatView(this);
			this.membersView = new SquadScreenMembersView(this);
			this.overviewView = new SquadScreenOverviewView(this);
			this.troopDonationView = new SquadScreenTroopDonationView(this);
			this.warButtonView = new SquadScreenWarButtonView(this);
			this.warLogView = new SquadScreenWarLogView(this);
			this.advancementView = new SquadScreenAdvancementView(this);
		}

		protected override void OnScreenLoaded()
		{
			Service.Get<BuildingController>().CancelEditModeTimer();
			base.InitAnimations("ChatHolder", "anim_chat_display");
			this.chatHolderPanel = base.GetElement<UXElement>("ChatHolderPanel");
			this.squadSlideBtn = base.GetElement<UXButton>("ButtonChat");
			this.squadSlideBtn.OnClicked = new UXButtonClickedDelegate(this.OnSquadSlideClicked);
			this.bgDialog = base.GetElement<UXElement>("BgDialog");
			this.bgDialog.Visible = false;
			this.squadNavigation = base.GetElement<UXElement>("SquadNavigation");
			this.squadNavigation.Visible = false;
			this.squadBadge = JewelControl.Create(this, "Chat");
			this.closeSquadBtn = base.GetElement<UXButton>("BtnCloseSquad");
			this.closeSquadBtn.OnClicked = new UXButtonClickedDelegate(this.OnCloseSquad);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.CurrentPlayerMemberDataUpdated);
			eventManager.RegisterObserver(this, EventId.GameStateChanged);
			eventManager.RegisterObserver(this, EventId.HUDVisibilityChanged);
			eventManager.RegisterObserver(this, EventId.SquadLeft);
			eventManager.RegisterObserver(this, EventId.SquadUpdated);
			eventManager.RegisterObserver(this, EventId.WarRewardClaimed);
			this.chatView.OnScreenLoaded();
			this.membersView.OnScreenLoaded();
			this.overviewView.OnScreenLoaded();
			this.troopDonationView.OnScreenLoaded();
			this.warButtonView.OnScreenLoaded();
			this.warLogView.OnScreenLoaded();
			this.advancementView.OnScreenLoaded();
			this.HideAllViews();
		}

		private void OnCloseSquad(UXButton closeBtrn)
		{
			if (base.IsOpen())
			{
				this.HideAllViews();
				base.AnimateClosed(false, null);
			}
		}

		public void OnSquadSlideClicked(UXButton slideButton)
		{
			this.ToggleSquadSlideScren();
		}

		public void ToggleSquadSlideScren()
		{
			if (base.IsClosed())
			{
				SquadController squadController = Service.Get<SquadController>();
				squadController.StateManager.SquadScreenState = SquadScreenState.Chat;
				this.AnimateOpen();
				return;
			}
			if (base.IsOpen())
			{
				this.HideAllViews();
				base.AnimateClosed(false, null);
			}
		}

		public override void AnimateOpen()
		{
			base.AnimateOpen();
			this.UpdateBadges();
		}

		protected override void OnOpening()
		{
			this.bgDialog.Visible = true;
			this.squadNavigation.Visible = true;
			this.RefreshViews();
			base.OnOpening();
		}

		protected override void OnClosing()
		{
			base.OnClosing();
			this.UpdateBadges();
		}

		protected override void OnOpen()
		{
			base.OnOpen();
			this.chatHolderPanel.RefreshPanel();
			SquadController squadController = Service.Get<SquadController>();
			squadController.StateManager.SetSquadScreenOpen(true);
			if (!this.chatView.ChatDisplaySetup)
			{
				this.chatView.SetupChatDisplay();
			}
		}

		protected override void OnClose()
		{
			base.OnClose();
			this.HideAllViews();
			this.bgDialog.Visible = false;
			this.squadNavigation.Visible = false;
			Service.Get<SquadController>().WarManager.MatchMakingPrepMode = false;
			SquadController squadController = Service.Get<SquadController>();
			squadController.StateManager.SetSquadScreenOpen(false);
		}

		private void HideAllOtherVisibleContainerViews(AbstractSquadScreenViewModule view)
		{
			if (view != this.chatView && this.chatView.IsVisible())
			{
				this.chatView.HideView();
			}
			if (view != this.membersView && this.membersView.IsVisible())
			{
				this.membersView.HideView();
			}
			if (view != this.overviewView && this.overviewView.IsVisible())
			{
				this.overviewView.HideView();
			}
			if (view != this.troopDonationView && this.troopDonationView.IsVisible())
			{
				this.troopDonationView.HideView();
			}
			if (view != this.warLogView && this.warLogView.IsVisible())
			{
				this.warLogView.HideView();
			}
			if (view != this.advancementView && this.advancementView.IsVisible())
			{
				this.advancementView.HideView();
			}
		}

		public void RefreshViews()
		{
			SquadController squadController = Service.Get<SquadController>();
			switch (squadController.StateManager.SquadScreenState)
			{
			case SquadScreenState.Chat:
				this.HideAllOtherVisibleContainerViews(this.chatView);
				if (!this.chatView.IsVisible())
				{
					this.chatView.ShowView();
				}
				else
				{
					this.chatView.RefreshView();
				}
				break;
			case SquadScreenState.Members:
				this.HideAllOtherVisibleContainerViews(this.membersView);
				if (!this.membersView.IsVisible())
				{
					this.membersView.ShowView();
				}
				else
				{
					this.membersView.RefreshView();
				}
				break;
			case SquadScreenState.Overview:
				this.HideAllOtherVisibleContainerViews(this.overviewView);
				if (!this.overviewView.IsVisible())
				{
					this.overviewView.ShowView();
				}
				else
				{
					this.overviewView.RefreshView();
				}
				break;
			case SquadScreenState.Donation:
				this.HideAllOtherVisibleContainerViews(this.chatView);
				if (!this.chatView.IsVisible())
				{
					this.chatView.ShowView();
				}
				else
				{
					this.chatView.RefreshView();
				}
				if (!this.troopDonationView.IsVisible())
				{
					this.troopDonationView.ShowView();
				}
				else
				{
					this.troopDonationView.RefreshView();
				}
				break;
			case SquadScreenState.WarLog:
				this.HideAllOtherVisibleContainerViews(this.warLogView);
				if (!this.warLogView.IsVisible())
				{
					this.warLogView.ShowView();
				}
				else
				{
					this.warLogView.RefreshView();
				}
				break;
			case SquadScreenState.Advancement:
				this.HideAllOtherVisibleContainerViews(this.advancementView);
				if (!this.advancementView.IsVisible())
				{
					this.advancementView.ShowView();
				}
				else
				{
					this.advancementView.RefreshView();
				}
				break;
			}
			this.warButtonView.RefreshView();
		}

		protected override CurrencyTrayType GetDisplayCurrencyTrayType()
		{
			if (this.advancementView != null && this.advancementView.IsVisible())
			{
				return this.advancementView.GetDisplayCurrencyTrayType();
			}
			return base.GetDisplayCurrencyTrayType();
		}

		private void HideAllViews()
		{
			this.chatView.HideView();
			this.membersView.HideView();
			this.overviewView.HideView();
			this.troopDonationView.HideView();
			this.warButtonView.HideView();
			this.warLogView.HideView();
			this.advancementView.HideView();
		}

		public void RefreshVisibility()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			HUD hUD = Service.Get<UXController>().HUD;
			HudConfig currentHudConfig = hUD.CurrentHudConfig;
			SquadController squadController = Service.Get<SquadController>();
			Squad currentSquad = squadController.StateManager.GetCurrentSquad();
			bool visible = (currentState is GalaxyState || currentHudConfig.Has("SquadScreen")) && currentSquad != null;
			this.Visible = visible;
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.SquadUpdated)
			{
				if (id != EventId.GameStateChanged && id != EventId.HUDVisibilityChanged && id != EventId.SquadUpdated)
				{
					goto IL_4A;
				}
			}
			else if (id != EventId.SquadLeft)
			{
				if (id != EventId.CurrentPlayerMemberDataUpdated && id != EventId.WarRewardClaimed)
				{
					goto IL_4A;
				}
				this.UpdateBadges();
				goto IL_4A;
			}
			this.RefreshVisibility();
			IL_4A:
			return base.OnEvent(id, cookie);
		}

		public override void OnDestroyElement()
		{
			this.HideAllViews();
			this.chatView.OnDestroyElement();
			this.membersView.OnDestroyElement();
			this.overviewView.OnDestroyElement();
			this.troopDonationView.OnDestroyElement();
			this.warButtonView.OnDestroyElement();
			this.warLogView.OnDestroyElement();
			this.advancementView.OnDestroyElement();
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.CurrentPlayerMemberDataUpdated);
			eventManager.UnregisterObserver(this, EventId.GameStateChanged);
			eventManager.UnregisterObserver(this, EventId.HUDVisibilityChanged);
			eventManager.UnregisterObserver(this, EventId.SquadLeft);
			eventManager.UnregisterObserver(this, EventId.SquadUpdated);
			eventManager.UnregisterObserver(this, EventId.WarRewardClaimed);
			base.OnDestroyElement();
		}

		protected override void OnAnimationComplete(uint id, object cookie)
		{
			base.OnAnimationComplete(id, cookie);
			if (!this.shouldCloseOnAnimComplete)
			{
				if (base.IsClosed())
				{
					this.RefreshVisibility();
					return;
				}
			}
			else
			{
				base.ClearCloseOnAnimFlags();
				Service.Get<UXController>().HUD.PrepForSquadScreenCreate();
			}
		}

		public void ShowSquadSlideButton()
		{
			this.squadSlideBtn.Visible = true;
		}

		public void HideSquadSlideButton()
		{
			this.squadSlideBtn.Visible = false;
		}

		public void OpenDonationView(string recipientId, string recipientUserName, int alreadyDonatedSize, int totalCapacity, int currentPlayerDonationCount, string requestId, bool isWarRequest, int troopDonationLimit, TroopDonationProgress donationProgress)
		{
			this.troopDonationView.InitView(recipientId, recipientUserName, alreadyDonatedSize, totalCapacity, currentPlayerDonationCount, requestId, isWarRequest, troopDonationLimit, donationProgress);
			SquadController squadController = Service.Get<SquadController>();
			squadController.StateManager.SquadScreenState = SquadScreenState.Donation;
			this.RefreshViews();
		}

		public void CloseDonationView()
		{
			SquadController squadController = Service.Get<SquadController>();
			if (squadController.StateManager.SquadScreenState == SquadScreenState.Donation)
			{
				this.troopDonationView.CloseView();
			}
		}

		public void UpdateBadges()
		{
			int num = this.chatView.UpdateBadges();
			int num2 = this.advancementView.UpdateBadge();
			int num3 = this.warLogView.RefreshBadge();
			bool flag = num > 0 || num3 > 0 || num2 > 0;
			if (!((base.IsAnimClosing() || base.IsClosed()) & flag))
			{
				this.squadBadge.Value = 0;
				return;
			}
			if (num3 > 0)
			{
				this.squadBadge.Text = "!";
				return;
			}
			this.squadBadge.Value = num + num2;
		}

		protected internal SquadSlidingScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).AnimateOpen();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).CloseDonationView();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).ShowCurrencyTray);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).GetDisplayCurrencyTrayType());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).HideAllOtherVisibleContainerViews((AbstractSquadScreenViewModule)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).HideAllViews();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).HideSquadSlideButton();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OnClose();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OnCloseSquad((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OnClosing();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OnOpen();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OnOpening();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OnSquadSlideClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).OpenDonationView(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), *(sbyte*)(args + 6) != 0, *(int*)(args + 7), (TroopDonationProgress)GCHandledObjects.GCHandleToObject(args[8]));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshViews();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshVisibility();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).ShowSquadSlideButton();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).ToggleSquadSlideScren();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SquadSlidingScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateBadges();
			return -1L;
		}
	}
}
