using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadScreenOverviewView : AbstractSquadScreenViewModule, IEventObserver
	{
		private const string OVERVIEW_CONTAINER = "OverviewContainer";

		private const string TAB_BUTTON_NAME = "SocialOverviewBtn";

		private const string FEATURED_SQUADS_CTA = "FEATURED_SQUADS_CTA";

		private const string SQUAD_OPEN_TO_ALL = "SQUAD_OPEN_TO_ALL";

		private const string SQUAD_INVITE_ONLY = "SQUAD_INVITE_ONLY";

		private const string SQUAD_BATTLESCORE = "SQUAD_BATTLESCORE";

		private const string SQUAD_MEMBERS = "SQUAD_MEMBERS";

		private const string SQUAD_ACTIVE_MEMBERS = "SQUAD_ACTIVE_MEMBERS";

		private const string SQUAD_TYPE = "SQUAD_TYPE";

		private const string SQUAD_REQUIRED_TROPHIES = "SQUAD_REQUIRED_TROPHIES";

		private const string SQUAD_HIGHEST_RANK = "SQUAD_HIGHEST_RANK";

		private const string FRACTION = "FRACTION";

		private const string LEAVING_SQUAD_ALERT_TITLE = "LEAVING_SQUAD_ALERT_TITLE";

		private const string LEAVING_SQUAD_ALERT = "LEAVING_SQUAD_ALERT";

		private const string LEAVING_SQUAD_CONFIRM = "LEAVING_SQUAD_CONFIRM";

		private const string ACCOUNT_CONFLICT_CONFIRM_CANCEL = "ACCOUNT_CONFLICT_CONFIRM_CANCEL";

		private const string LEAVE_A_SQUAD = "LEAVE_A_SQUAD";

		private const string IN_WAR_CANT_LEAVE_SQUAD = "IN_WAR_CANT_LEAVE_SQUAD";

		private const string LEAVING_SQUAD_ALERT_WITH_REWARDS_TITLE = "LEAVING_SQUAD_ALERT_WITH_REWARDS_TITLE";

		private const string LEAVING_SQUAD_ALERT_WITH_REWARDS = "LEAVING_SQUAD_ALERT_WITH_REWARDS";

		private const string OVERVIEW = "overview";

		private const string SO_MEMBERS_TITLE_LABEL = "LabelMembersTitle";

		private const string SO_ACTIVE_MEMBERS_TITLE_LABEL = "LabelMembersActiveTitle";

		private const string SO_SCORE_TITLE_LABEL = "LabelScoreTitle";

		private const string SO_TROPHIES_TITLE_LABEL = "LabelTrophiesTitle";

		private const string SO_TYPE_TITLE_LABEL = "LabelTypeTitle";

		private const string SO_HIGHESTRANK_TITLE_LABEL = "LabelHighestRankTitle";

		private const string SO_MEMBERS_LABEL = "LabelMembers";

		private const string SO_ACTIVE_MEMBERS_LABEL = "LabelActiveMembers";

		private const string SO_SCORE_LABEL = "LabelScore";

		private const string SO_TROPHIES_LABEL = "LabelTrophies";

		private const string SO_TYPE_LABEL = "LabelType";

		private const string SQUAD_LOGO_SPRITE = "SpriteSquadLogo";

		private const string SQUAD_NAME_LABEL = "LabelSquadName";

		private const string SQUAD_RANK_LABEL = "LabelSquadRank";

		private const string SQUAD_HIGHESTRANK_LABEL = "LabelSquadHighestRank";

		private const string SQUAD_DESC_LABEL = "LabelSquadDescription";

		private const string SQUAD_RANK_TITLE_LABEL = "LabelRankTitle";

		private const string SQUAD_RANK = "SQUAD_OVERVIEW_RANK";

		private const string SQUAD_LEVEL_LABEL = "LabelSquadLvlOverview";

		private const string OVERVIEW_FACTION_TEXTURE = "TextureSquadFaction";

		public const string EDIT_SQUAD_BTN = "BtnEdit";

		public const string LEAVE_SQUAD_BTN = "BtnLeave";

		public const string FEATURE_SQUAD_BTN = "BtnFeaturedSquads";

		public const string FEATURE_SQUAD_BTN_LABEL = "LabelBtnFeaturedSquads";

		private UXElement overviewContainer;

		private UXCheckbox tabButton;

		private UXLabel statsScoreLabel;

		private UXLabel statsMembersLabel;

		private UXLabel statsActiveMembersLabel;

		private UXButton leaveButton;

		private UXLabel squadRankLabel;

		private UXLabel squadHighestRankLabel;

		private UXTexture factionTexture;

		private UXButton featureSquadButton;

		private UXLabel statsMembersTitle;

		private UXLabel statsActiveMembersTitle;

		private UXLabel statsScoreTitle;

		private UXLabel statsTrophiesTitle;

		private UXLabel statsTypeTitle;

		private UXLabel statsTrophiesLabel;

		private UXLabel statsTypeLabel;

		private UXLabel squadHighestRankTitleLabel;

		private UXLabel squadLevelLabel;

		private UXLabel squadTitleLabel;

		private UXLabel squadDescLabel;

		private UXSprite squadIcon;

		private UXLabel squadRankTitleLabel;

		private UXButton editButton;

		public SquadScreenOverviewView(SquadSlidingScreen screen) : base(screen)
		{
		}

		public override void OnScreenLoaded()
		{
			this.overviewContainer = this.screen.GetElement<UXElement>("OverviewContainer");
			this.tabButton = this.screen.GetElement<UXCheckbox>("SocialOverviewBtn");
			this.tabButton.OnSelected = new UXCheckboxSelectedDelegate(this.OnTabButtonSelected);
			this.InitOverview();
		}

		public override void ShowView()
		{
			EventManager eventManager = Service.Get<EventManager>();
			this.overviewContainer.Visible = true;
			eventManager.SendEvent(EventId.SquadSelect, null);
			eventManager.SendEvent(EventId.UISquadScreenTabShown, "overview");
			eventManager.RegisterObserver(this, EventId.SquadDetailsUpdated);
			eventManager.RegisterObserver(this, EventId.SquadLeveledUp);
			this.RefreshView();
			this.tabButton.Selected = true;
		}

		public override void HideView()
		{
			this.overviewContainer.Visible = false;
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.SquadDetailsUpdated);
			eventManager.UnregisterObserver(this, EventId.SquadLeveledUp);
			this.tabButton.Selected = false;
		}

		public override void RefreshView()
		{
			SquadController squadController = Service.Get<SquadController>();
			Squad currentSquad = squadController.StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return;
			}
			if (currentSquad.InviteType == 1)
			{
				this.statsTypeLabel.Text = this.lang.Get("SQUAD_OPEN_TO_ALL", new object[0]);
			}
			else
			{
				this.statsTypeLabel.Text = this.lang.Get("SQUAD_INVITE_ONLY", new object[0]);
			}
			this.statsTrophiesLabel.Text = this.lang.ThousandsSeparated(currentSquad.RequiredTrophies);
			this.squadTitleLabel.Text = currentSquad.SquadName;
			this.squadDescLabel.Text = currentSquad.Description;
			this.squadIcon.SpriteName = currentSquad.Symbol;
			this.SetupButtonBasedOnRole(this.editButton, null, true);
			this.UpdateStatsLabels();
			this.UpdateSquadRank();
		}

		private void OnTabButtonSelected(UXCheckbox checkbox, bool selected)
		{
			if (selected)
			{
				SquadController squadController = Service.Get<SquadController>();
				squadController.StateManager.SquadScreenState = SquadScreenState.Overview;
				this.screen.RefreshViews();
			}
		}

		protected void InitOverview()
		{
			this.statsMembersTitle = this.screen.GetElement<UXLabel>("LabelMembersTitle");
			this.statsActiveMembersTitle = this.screen.GetElement<UXLabel>("LabelMembersActiveTitle");
			this.statsScoreTitle = this.screen.GetElement<UXLabel>("LabelScoreTitle");
			this.statsTrophiesTitle = this.screen.GetElement<UXLabel>("LabelTrophiesTitle");
			this.statsTypeTitle = this.screen.GetElement<UXLabel>("LabelTypeTitle");
			this.statsTrophiesLabel = this.screen.GetElement<UXLabel>("LabelTrophies");
			this.statsTypeLabel = this.screen.GetElement<UXLabel>("LabelType");
			this.squadHighestRankTitleLabel = this.screen.GetElement<UXLabel>("LabelHighestRankTitle");
			this.statsActiveMembersLabel = this.screen.GetElement<UXLabel>("LabelActiveMembers");
			this.statsMembersLabel = this.screen.GetElement<UXLabel>("LabelMembers");
			this.statsScoreLabel = this.screen.GetElement<UXLabel>("LabelScore");
			this.statsScoreTitle.Text = this.lang.Get("SQUAD_BATTLESCORE", new object[0]);
			this.statsMembersTitle.Text = this.lang.Get("SQUAD_MEMBERS", new object[0]);
			this.statsActiveMembersTitle.Text = this.lang.Get("SQUAD_ACTIVE_MEMBERS", new object[0]);
			this.statsTypeTitle.Text = this.lang.Get("SQUAD_TYPE", new object[0]);
			this.statsTrophiesTitle.Text = this.lang.Get("SQUAD_REQUIRED_TROPHIES", new object[0]);
			this.squadHighestRankTitleLabel.Text = this.lang.Get("SQUAD_HIGHEST_RANK", new object[0]);
			this.squadTitleLabel = this.screen.GetElement<UXLabel>("LabelSquadName");
			this.squadRankLabel = this.screen.GetElement<UXLabel>("LabelSquadRank");
			this.squadHighestRankLabel = this.screen.GetElement<UXLabel>("LabelSquadHighestRank");
			this.squadDescLabel = this.screen.GetElement<UXLabel>("LabelSquadDescription");
			this.squadIcon = this.screen.GetElement<UXSprite>("SpriteSquadLogo");
			this.squadRankTitleLabel = this.screen.GetElement<UXLabel>("LabelRankTitle");
			this.squadRankTitleLabel.Text = this.lang.Get("SQUAD_OVERVIEW_RANK", new object[0]);
			this.squadLevelLabel = this.screen.GetElement<UXLabel>("LabelSquadLvlOverview");
			this.editButton = this.screen.GetElement<UXButton>("BtnEdit");
			this.editButton.OnClicked = new UXButtonClickedDelegate(this.OnEditSquadClicked);
			this.SetupButtonBasedOnRole(this.editButton, null, true);
			this.leaveButton = this.screen.GetElement<UXButton>("BtnLeave");
			this.leaveButton.OnClicked = new UXButtonClickedDelegate(this.OnLeaveSquadClicked);
			this.screen.GetElement<UXLabel>("LabelBtnFeaturedSquads").Text = this.lang.Get("FEATURED_SQUADS_CTA", new object[0]);
			this.featureSquadButton = this.screen.GetElement<UXButton>("BtnFeaturedSquads");
			this.featureSquadButton.Enabled = true;
			this.featureSquadButton.OnClicked = new UXButtonClickedDelegate(this.OnFeatureSquadClicked);
			this.factionTexture = this.screen.GetElement<UXTexture>("TextureSquadFaction");
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.Faction == FactionType.Rebel)
			{
				this.factionTexture.LoadTexture("SquadFactionRebel");
				return;
			}
			this.factionTexture.LoadTexture("SquadFactionEmpire");
		}

		private void UpdateSquadRank()
		{
			SquadController squadController = Service.Get<SquadController>();
			Squad currentSquad = squadController.StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return;
			}
			this.squadRankLabel.Text = this.lang.ThousandsSeparated(currentSquad.Rank);
			this.squadHighestRankLabel.Text = this.lang.ThousandsSeparated(currentSquad.HighestRank);
			if (SquadUtils.CanLeaveSquad())
			{
				this.leaveButton.VisuallyEnableButton();
				return;
			}
			this.leaveButton.VisuallyDisableButton();
		}

		private void UpdateStatsLabels()
		{
			SquadController squadController = Service.Get<SquadController>();
			Squad currentSquad = squadController.StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return;
			}
			this.statsScoreLabel.Text = this.lang.ThousandsSeparated(currentSquad.BattleScore);
			this.statsMembersLabel.Text = this.lang.Get("FRACTION", new object[]
			{
				currentSquad.MemberCount,
				currentSquad.MemberMax
			});
			this.statsActiveMembersLabel.Text = this.lang.Get("FRACTION", new object[]
			{
				currentSquad.ActiveMemberCount,
				currentSquad.MemberMax
			});
			this.squadLevelLabel.Text = currentSquad.Level.ToString();
		}

		private void SetupButtonBasedOnRole(UXButton btn, SquadMember member, bool ownerOnly)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (member != null && (currentPlayer.PlayerId == member.MemberID || member.Role == SquadRole.Owner))
			{
				btn.Visible = false;
				return;
			}
			SquadStateManager stateManager = Service.Get<SquadController>().StateManager;
			if (stateManager.Role == SquadRole.Owner)
			{
				btn.Visible = true;
				return;
			}
			if (!ownerOnly && stateManager.Role == SquadRole.Officer)
			{
				btn.Visible = true;
				return;
			}
			btn.Visible = false;
		}

		private void OnEditSquadClicked(UXButton button)
		{
			if (Service.Get<GameStateMachine>().CurrentState is GalaxyState)
			{
				Service.Get<GalaxyViewController>().GoToHome();
			}
			this.screen.InstantClose(false, null);
			Service.Get<EventManager>().SendEvent(EventId.SquadEdited, null);
			Service.Get<ScreenController>().AddScreen(new SquadCreateScreen(false));
		}

		private void OnFeatureSquadClicked(UXButton button)
		{
			this.screen.AnimateClosed(false, null);
			this.featureSquadButton.Enabled = false;
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is GalaxyState)
			{
				ScreenController screenController = Service.Get<ScreenController>();
				screenController.CloseAll();
				Service.Get<GalaxyViewController>().GoToHome(true, new WipeCompleteDelegate(this.InternalFeatureSquadClicked), null);
				return;
			}
			if (currentState is WarBoardState)
			{
				Service.Get<UXController>().HUD.SetSquadScreenAlwaysOnTop(false);
				this.InternalFeatureSquadClicked(null);
				return;
			}
			this.InternalFeatureSquadClicked(null);
		}

		private void InternalFeatureSquadClicked(object cookie)
		{
			this.featureSquadButton.Enabled = true;
			Service.Get<UXController>().HUD.OpenJoinSquadPanel();
		}

		private void OnLeaveSquadClicked(UXButton button)
		{
			this.screen.HideSquadSlideButton();
			this.screen.AnimateClosed(false, null);
			if (Service.Get<GameStateMachine>().CurrentState is GalaxyState)
			{
				ScreenController screenController = Service.Get<ScreenController>();
				screenController.CloseAll();
				Service.Get<GalaxyViewController>().GoToHome(true, new WipeCompleteDelegate(this.InternalOnLeaveSquadClicked), null);
				return;
			}
			this.InternalOnLeaveSquadClicked(null);
		}

		private void InternalOnLeaveSquadClicked(object cookie)
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadEdited, null);
			SquadController squadController = Service.Get<SquadController>();
			SquadMemberWarData currentMemberWarData = squadController.WarManager.GetCurrentMemberWarData();
			uint serverTime = Service.Get<ServerAPI>().ServerTime;
			int unclaimedSquadWarRewardsCount = SquadUtils.GetUnclaimedSquadWarRewardsCount(currentMemberWarData, serverTime);
			if (!SquadUtils.CanLeaveSquad())
			{
				string message = Service.Get<Lang>().Get("IN_WAR_CANT_LEAVE_SQUAD", new object[0]);
				AlertScreen.ShowModal(false, null, message, null, null, true);
				this.screen.ShowSquadSlideButton();
				return;
			}
			if (unclaimedSquadWarRewardsCount > 0)
			{
				YesNoScreen.ShowModal(this.lang.Get("LEAVING_SQUAD_ALERT_WITH_REWARDS_TITLE", new object[0]), this.lang.Get("LEAVING_SQUAD_ALERT_WITH_REWARDS", new object[]
				{
					unclaimedSquadWarRewardsCount
				}), false, this.lang.Get("LEAVING_SQUAD_CONFIRM", new object[0]), this.lang.Get("ACCOUNT_CONFLICT_CONFIRM_CANCEL", new object[0]), new OnScreenModalResult(this.OnAlertLeaveResult), null, false);
				return;
			}
			YesNoScreen.ShowModal(this.lang.Get("LEAVING_SQUAD_ALERT_TITLE", new object[0]), this.lang.Get("LEAVING_SQUAD_ALERT", new object[0]), false, this.lang.Get("LEAVING_SQUAD_CONFIRM", new object[0]), this.lang.Get("ACCOUNT_CONFLICT_CONFIRM_CANCEL", new object[0]), new OnScreenModalResult(this.OnAlertLeaveResult), null, false);
		}

		private void OnAlertLeaveResult(object result, object cookie)
		{
			if (result != null)
			{
				SquadMsg message = SquadMsgUtils.CreateLeaveSquadMessage(new SquadController.ActionCallback(this.OnLeaveSquadComplete), null);
				Service.Get<SquadController>().TakeAction(message);
				if (Service.Get<GameStateMachine>().CurrentState is WarBoardState)
				{
					SquadWarScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<SquadWarScreen>();
					if (highestLevelScreen != null)
					{
						highestLevelScreen.CloseSquadWarScreen(null);
						return;
					}
				}
			}
			else
			{
				this.screen.ShowSquadSlideButton();
			}
		}

		private void OnLeaveSquadComplete(bool success, object cookie)
		{
			this.screen.ShowSquadSlideButton();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SquadDetailsUpdated || id == EventId.SquadLeveledUp)
			{
				this.RefreshView();
			}
			return EatResponse.NotEaten;
		}

		public override void OnDestroyElement()
		{
		}

		public override bool IsVisible()
		{
			return this.overviewContainer.Visible;
		}

		protected internal SquadScreenOverviewView(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).HideView();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).InitOverview();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).InternalFeatureSquadClicked(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).InternalOnLeaveSquadClicked(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).IsVisible());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).OnAlertLeaveResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).OnEditSquadClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).OnFeatureSquadClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).OnLeaveSquadClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).OnLeaveSquadComplete(*(sbyte*)args != 0, GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).OnTabButtonSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).SetupButtonBasedOnRole((UXButton)GCHandledObjects.GCHandleToObject(*args), (SquadMember)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).ShowView();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).UpdateSquadRank();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SquadScreenOverviewView)GCHandledObjects.GCHandleToObject(instance)).UpdateStatsLabels();
			return -1L;
		}
	}
}
