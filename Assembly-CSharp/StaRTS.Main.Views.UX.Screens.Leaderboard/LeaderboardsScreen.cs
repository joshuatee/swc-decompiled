using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Leaderboard;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Leaderboard
{
	public class LeaderboardsScreen : AbstractLeaderboardScreen
	{
		private const string FIND_ME_FAIL_MESSAGE = "FIND_ME_FAIL_MESSAGE";

		private const string CONFLICT_FIND_ME_FAIL_MESSAGE = "CONFLICT_FIND_ME_FAIL_MESSAGE";

		private const string LEADERBOARDS = "LEADERBOARDS";

		private const string SQUADS = "s_Squads";

		private const string LEADERS = "s_Leaders";

		private const string FRIENDS = "s_Friends";

		private const string CONFLICTS = "s_Conflicts";

		private const string TOP_50 = "leaderboard_top_50";

		private const string FIND_ME = "leaderboard_find_me";

		private const int ANCHOR_OFFSET = -76;

		private const float ANIM_FADE_OUT = 0.5f;

		private const float ANIM_FADE_IN = 0.5f;

		private const float CALL_BACK_DELAY = 0.5f;

		private FactionSelectorDropDown factionSelectorDropDown;

		private PlanetSelectionDropDown planetSelectionDropDown;

		private UXButton top50Button;

		private UXButton findMeButton;

		private UXButton featureSquadButton;

		protected bool useScreenTransitions;

		private PlanetVO initialPlanet;

		protected override bool WantTransitions
		{
			get
			{
				return this.useScreenTransitions;
			}
		}

		public LeaderboardsScreen(bool useScreenTransition, string initalPlanetId)
		{
			this.useScreenTransitions = useScreenTransition;
			this.factionSelectorDropDown = new FactionSelectorDropDown(this);
			this.planetSelectionDropDown = new PlanetSelectionDropDown(this);
			if (TournamentController.GetAllLiveAndClosingTournaments().Count > 0)
			{
				this.initialTab = SocialTabs.Tournament;
			}
			else
			{
				this.initialTab = SocialTabs.Squads;
			}
			if (!string.IsNullOrEmpty(initalPlanetId))
			{
				this.initialPlanet = Service.Get<IDataController>().GetOptional<PlanetVO>(initalPlanetId);
			}
		}

		protected override void InitTabInfo()
		{
			SocialTabInfo value = new SocialTabInfo(new Action(this.LoadSquads), EventId.UILeaderboardSquadTabShown, null, PlayerListType.Squads);
			this.tabs.Add(SocialTabs.Squads, value);
			value = new SocialTabInfo(new Action(base.LoadFriends), EventId.UILeaderboardFriendsTabShown, null, PlayerListType.Friends);
			this.tabs.Add(SocialTabs.Friends, value);
			value = new SocialTabInfo(new Action(this.LoadLeaders), EventId.UILeaderboardPlayersTabShown, null, PlayerListType.Leaders);
			this.tabs.Add(SocialTabs.Leaders, value);
			value = new SocialTabInfo(new Action(this.LoadTournamentLeaders), EventId.UILeaderboardTournamentTabShown, null, PlayerListType.TournamentLeaders);
			this.tabs.Add(SocialTabs.Tournament, value);
		}

		protected override void OnScreenLoaded()
		{
			base.OnScreenLoaded();
			UIPanel component = this.panel.Root.GetComponent<UIPanel>();
			component.topAnchor.absolute = -76;
			component.clipSoftness = new Vector2(1f, 2f);
			base.CleanupScreenTransition(true);
			this.titleLabel.Text = this.lang.Get("LEADERBOARDS", new object[0]);
			this.factionSelectorDropDown.Init(FactionToggle.All);
			this.factionSelectorDropDown.FactionSelectCallBack += new Action<FactionToggle>(this.FactionChanged);
			this.planetSelectionDropDown.Init();
			this.planetSelectionDropDown.PlanetSelectCallBack += new Action<PlanetVO>(this.PlanetChanged);
			base.GetElement<UXElement>("ContainerJewelInvites").Visible = false;
			if (this.initialTab == SocialTabs.Tournament)
			{
				this.TournamentTabClicked(null, true);
			}
			else
			{
				this.SquadsTabClicked(null, true);
			}
			if (this.initialPlanet != null)
			{
				this.planetSelectionDropDown.SelectPlanet(this.initialPlanet);
			}
			this.navigationRow.Visible = true;
		}

		protected override void InitButtons()
		{
			base.InitButtons();
			UXCheckbox element = base.GetElement<UXCheckbox>("BtnSquads");
			element.OnSelected = new UXCheckboxSelectedDelegate(this.SquadsTabClicked);
			element.Selected = false;
			SocialTabInfo tabInfo = base.GetTabInfo(SocialTabs.Squads);
			tabInfo.TabButton = element;
			tabInfo.TabLabel = base.GetElement<UXLabel>("LabelSquads");
			tabInfo.TabLabel.Text = this.lang.Get("s_Squads", new object[0]);
			element = base.GetElement<UXCheckbox>("BtnLeaders");
			element.Selected = false;
			element.OnSelected = new UXCheckboxSelectedDelegate(this.LeadersTabClicked);
			tabInfo = base.GetTabInfo(SocialTabs.Leaders);
			tabInfo.TabButton = element;
			tabInfo.TabLabel = base.GetElement<UXLabel>("LabelBtnLeaders");
			tabInfo.TabLabel.Text = this.lang.Get("s_Leaders", new object[0]);
			element = base.GetElement<UXCheckbox>("BtnFriends");
			element.OnSelected = new UXCheckboxSelectedDelegate(this.FriendsTabClicked);
			element.Selected = false;
			tabInfo = base.GetTabInfo(SocialTabs.Friends);
			tabInfo.TabButton = element;
			tabInfo.TabLabel = base.GetElement<UXLabel>("LabelBtnFriends");
			tabInfo.TabLabel.Text = this.lang.Get("s_Friends", new object[0]);
			element = base.GetElement<UXCheckbox>("BtnTournament");
			element.Visible = (TournamentController.GetAllLiveAndClosingTournaments().Count > 0);
			element.OnSelected = new UXCheckboxSelectedDelegate(this.TournamentTabClicked);
			element.Selected = false;
			tabInfo = base.GetTabInfo(SocialTabs.Tournament);
			tabInfo.TabButton = element;
			tabInfo.TabLabel = base.GetElement<UXLabel>("LabelBtnTournament");
			tabInfo.TabLabel.Text = this.lang.Get("s_Conflicts", new object[0]);
			this.top50Button = base.GetElement<UXButton>("BtnSortTop");
			this.top50Button.OnClicked = new UXButtonClickedDelegate(this.Top50ButtonClicked);
			base.GetElement<UXLabel>("LabelBtnSortTop").Text = this.lang.Get("leaderboard_top_50", new object[0]);
			this.findMeButton = base.GetElement<UXButton>("BtnFindMe");
			this.findMeButton.OnClicked = new UXButtonClickedDelegate(this.FindMeButtonClicked);
			base.GetElement<UXLabel>("LabelBtnFindMe").Text = this.lang.Get("leaderboard_find_me", new object[0]);
			this.featureSquadButton = base.GetElement<UXButton>("BtnFeaturedSquads");
			this.featureSquadButton.OnClicked = new UXButtonClickedDelegate(this.FeatureSquadButtonClicked);
			base.GetElement<UXLabel>("LabelBtnFeaturedSquads").Text = this.lang.Get("s_Featured", new object[0]);
			this.featureSquadButton.Visible = false;
		}

		protected override void InitIndividualGrids(UXGrid baseGrid)
		{
			GridLoadHelper tabGridLoadHelper = base.CreateGridLoadHelperByCloningGrid(baseGrid, "LBSquadGrid");
			base.GetTabInfo(SocialTabs.Squads).TabGridLoadHelper = tabGridLoadHelper;
			tabGridLoadHelper = base.CreateGridLoadHelperByCloningGrid(baseGrid, "LBFriendsGrid");
			base.GetTabInfo(SocialTabs.Friends).TabGridLoadHelper = tabGridLoadHelper;
			tabGridLoadHelper = base.CreateGridLoadHelperByCloningGrid(baseGrid, "LBPlayersGrid");
			base.GetTabInfo(SocialTabs.Leaders).TabGridLoadHelper = tabGridLoadHelper;
			tabGridLoadHelper = base.CreateGridLoadHelperByCloningGrid(baseGrid, "LBTournamentGrid");
			base.GetTabInfo(SocialTabs.Tournament).TabGridLoadHelper = tabGridLoadHelper;
		}

		private void LoadSquads()
		{
			if (!Service.Get<LeaderboardController>().ShouldRefreshData(PlayerListType.Squads, null))
			{
				if (this.gridLoadHelper.IsAddedItems || this.gridLoadHelper.IsBusyAddingItems)
				{
					base.ResetGrid();
				}
				this.AddItemsToLeaderboardGrid();
				return;
			}
			base.ResetGrid();
			ProcessingScreen.Show();
			Service.Get<LeaderboardController>().UpdateTopSquads(new LeaderboardController.OnUpdateData(this.OnGetLBSquads));
		}

		private void OnGetLBSquads(bool success)
		{
			if (success)
			{
				ProcessingScreen.Hide();
				if (!this.Visible)
				{
					return;
				}
				if (this.curTab == SocialTabs.Squads)
				{
					this.AddItemsToLeaderboardGrid();
					return;
				}
			}
			else
			{
				this.OnDataUpdateFailure();
			}
		}

		private void LoadLeaders()
		{
			PlanetVO selectedPlanet = this.planetSelectionDropDown.GetSelectedPlanet();
			string planetId = (selectedPlanet == null) ? null : selectedPlanet.Uid;
			ProcessingScreen.Show();
			if (!Service.Get<LeaderboardController>().ShouldRefreshData(PlayerListType.Leaders, planetId))
			{
				if (this.gridLoadHelper.IsAddedItems || this.gridLoadHelper.IsBusyAddingItems)
				{
					base.ResetGrid();
				}
				LeaderboardList<PlayerLBEntity> leaderboardList = this.GetLeaderboardList();
				base.OnPlayersListLoaded((leaderboardList != null) ? leaderboardList.List : null, new Action(this.PopulatePlayersOnGrid));
				return;
			}
			base.ResetGrid();
			Service.Get<LeaderboardController>().UpdateLeaders(selectedPlanet, new LeaderboardController.OnUpdateData(this.OnGetLBPlayers));
		}

		private void OnGetLBPlayers(bool success)
		{
			if (!success)
			{
				this.OnDataUpdateFailure();
				return;
			}
			if (!this.Visible)
			{
				ProcessingScreen.Hide();
				return;
			}
			if (this.curTab == SocialTabs.Leaders)
			{
				LeaderboardList<PlayerLBEntity> leaderboardList = this.GetLeaderboardList();
				base.OnPlayersListLoaded((leaderboardList != null) ? leaderboardList.List : null, new Action(this.PopulatePlayersOnGrid));
			}
		}

		private void PopulatePlayersOnGrid()
		{
			ProcessingScreen.Hide();
			this.AddItemsToLeaderboardGrid();
		}

		public void LoadTournamentLeaders()
		{
			PlanetVO selectedPlanet = this.planetSelectionDropDown.GetSelectedPlanet();
			string text = (selectedPlanet == null) ? null : selectedPlanet.Uid;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (!Service.Get<LeaderboardController>().ShouldRefreshData(PlayerListType.TournamentLeaders, text))
			{
				if (this.gridLoadHelper.IsAddedItems || this.gridLoadHelper.IsBusyAddingItems)
				{
					base.ResetGrid();
				}
				this.AddItemsToLeaderboardGrid();
				return;
			}
			base.ResetGrid();
			ProcessingScreen.Show();
			Service.Get<LeaderboardController>().UpdateTournamentLeaders(selectedPlanet, new LeaderboardController.OnUpdateData(this.OnGetLBTournament));
		}

		private void OnGetLBTournament(bool success)
		{
			if (!success)
			{
				this.OnDataUpdateFailure();
				return;
			}
			ProcessingScreen.Hide();
			if (!this.Visible)
			{
				return;
			}
			if (this.curTab == SocialTabs.Tournament)
			{
				this.AddItemsToLeaderboardGrid();
			}
		}

		private void AddItemsToLeaderboardGrid()
		{
			if (this.top50Button.Enabled)
			{
				this.DisplayPlayersNearMe();
				return;
			}
			this.DisplayTop50Players();
		}

		public bool EnableFindMeButton()
		{
			bool flag = Service.Get<SquadController>().StateManager.GetCurrentSquad() != null;
			if (!this.top50Button.Enabled && this.curTab == SocialTabs.Squads && !flag)
			{
				return false;
			}
			string selectedPlanetId = this.GetSelectedPlanetId();
			if (!this.top50Button.Enabled && selectedPlanetId != null && selectedPlanetId != Service.Get<CurrentPlayer>().PlanetId && this.curTab == SocialTabs.Leaders)
			{
				return false;
			}
			FactionToggle selectedFaction = this.GetSelectedFaction();
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			return !this.top50Button.Enabled && (selectedFaction == FactionToggle.All || (faction == FactionType.Empire && selectedFaction == FactionToggle.Empire) || (faction == FactionType.Rebel && selectedFaction == FactionToggle.Rebel));
		}

		private void OnDataUpdateFailure()
		{
			ProcessingScreen.Hide();
			this.top50Button.Enabled = false;
			this.findMeButton.Enabled = false;
		}

		private void Top50ButtonClicked(UXButton button)
		{
			this.DisplayTop50Players();
		}

		private void FeatureSquadButtonClicked(UXButton button)
		{
			this.Close(button);
			Service.Get<UXController>().HUD.OpenJoinSquadPanelAfterDelay();
			Service.Get<EventManager>().SendEvent(EventId.UISquadJoinScreenShown, "leaderboard");
		}

		private void DisplayTop50Players()
		{
			TweenAlpha.Begin(this.scrollView.transform.gameObject, 0.5f, 0f);
			Service.Get<ViewTimerManager>().CreateViewTimer(0.5f, false, new TimerDelegate(this.OnTimerCallbackTop50), null);
		}

		private void OnTimerCallbackTop50(uint id, object cookie)
		{
			TweenAlpha.Begin(this.scrollView.transform.gameObject, 0.5f, 1f);
			this.top50Button.Enabled = false;
			this.findMeButton.Enabled = this.EnableFindMeButton();
			string selectedPlanetId = this.planetSelectionDropDown.GetSelectedPlanetId();
			LeaderboardList<PlayerLBEntity> leaderboardList = null;
			if (this.curTab == SocialTabs.Squads)
			{
				base.AddItemsToGrid<Squad>(Service.Get<LeaderboardController>().TopSquads.List, true, true);
				return;
			}
			if (this.curTab == SocialTabs.Leaders)
			{
				if (selectedPlanetId == null)
				{
					leaderboardList = Service.Get<LeaderboardController>().GlobalLeaders;
				}
				else
				{
					leaderboardList = Service.Get<LeaderboardController>().LeadersByPlanet[selectedPlanetId];
				}
			}
			else if (this.curTab == SocialTabs.Tournament)
			{
				leaderboardList = Service.Get<LeaderboardController>().TournamentLeadersByPlanet[selectedPlanetId];
			}
			if (leaderboardList != null)
			{
				base.AddItemsToGrid<PlayerLBEntity>(leaderboardList.List, true, true);
				base.PositionScrollViewForTop50();
			}
		}

		public void ShowFindMeFailMessage(int count)
		{
			if (count <= 0)
			{
				string message = this.lang.Get("FIND_ME_FAIL_MESSAGE", new object[0]);
				if (this.curTab == SocialTabs.Tournament)
				{
					message = this.lang.Get("CONFLICT_FIND_ME_FAIL_MESSAGE", new object[]
					{
						LangUtils.GetPlanetDisplayName(this.GetSelectedPlanetId())
					});
				}
				AlertScreen.ShowModal(false, null, message, null, null);
			}
		}

		private void FindMeButtonClicked(UXButton button)
		{
			this.findMeButton.Enabled = false;
			this.DisplayPlayersNearMe();
		}

		private LeaderboardList<PlayerLBEntity> GetLeaderboardList()
		{
			string selectedPlanetId = this.planetSelectionDropDown.GetSelectedPlanetId();
			LeaderboardList<PlayerLBEntity> result = null;
			if (this.curTab == SocialTabs.Leaders)
			{
				if (selectedPlanetId == null)
				{
					result = Service.Get<LeaderboardController>().GlobalNearMeLeaders;
				}
				else
				{
					result = Service.Get<LeaderboardController>().LeadersNearMeByPlanet[selectedPlanetId];
				}
			}
			else if (this.curTab == SocialTabs.Tournament)
			{
				result = Service.Get<LeaderboardController>().TournamentLeadersNearMeByPlanet[selectedPlanetId];
			}
			return result;
		}

		private void DisplayPlayersNearMe()
		{
			LeaderboardList<PlayerLBEntity> leaderboardList = this.GetLeaderboardList();
			if (this.curTab == SocialTabs.Squads && Service.Get<LeaderboardController>().SquadsNearMe.List.Count == 0)
			{
				this.top50Button.Enabled = true;
				this.findMeButton.Enabled = false;
				this.ShowFindMeFailMessage(Service.Get<LeaderboardController>().SquadsNearMe.List.Count);
			}
			if (leaderboardList != null && leaderboardList.List.Count == 0)
			{
				this.top50Button.Enabled = true;
				this.findMeButton.Enabled = false;
				this.ShowFindMeFailMessage(leaderboardList.List.Count);
				return;
			}
			TweenAlpha.Begin(this.scrollView.transform.gameObject, 0.5f, 0f);
			Service.Get<ViewTimerManager>().CreateViewTimer(0.5f, false, new TimerDelegate(this.OnTimerCallbackFindMe), null);
		}

		private void OnTimerCallbackFindMe(uint id, object cookie)
		{
			TweenAlpha.Begin(this.scrollView.transform.gameObject, 0.5f, 1f);
			this.top50Button.Enabled = true;
			this.findMeButton.Enabled = false;
			if (this.curTab == SocialTabs.Squads)
			{
				base.AddItemsToGrid<Squad>(Service.Get<LeaderboardController>().SquadsNearMe.List, false, true);
				base.PositionScrollViewForFindMe();
				return;
			}
			LeaderboardList<PlayerLBEntity> leaderboardList = this.GetLeaderboardList();
			if (leaderboardList != null)
			{
				base.AddItemsToGrid<PlayerLBEntity>(leaderboardList.List, false, true);
				base.PositionScrollViewForFindMe();
			}
		}

		private void SquadsTabClicked(UXCheckbox box, bool selected)
		{
			this.planetSelectionDropDown.SetStatePlanetOptions(SocialTabs.Squads);
			this.top50Button.Visible = true;
			this.findMeButton.Visible = true;
			this.top50Button.Enabled = false;
			this.findMeButton.Enabled = true;
			this.featureSquadButton.Visible = true;
			base.TabClicked(selected, SocialTabs.Squads);
		}

		protected void FriendsTabClicked(UXCheckbox box, bool selected)
		{
			this.planetSelectionDropDown.SetStatePlanetOptions(SocialTabs.Friends);
			this.top50Button.Visible = false;
			this.findMeButton.Visible = false;
			this.featureSquadButton.Visible = false;
			base.TabClicked(selected, SocialTabs.Friends);
		}

		private void LeadersTabClicked(UXCheckbox box, bool selected)
		{
			this.planetSelectionDropDown.SetStatePlanetOptions(SocialTabs.Leaders);
			this.top50Button.Visible = true;
			this.findMeButton.Visible = true;
			this.top50Button.Enabled = false;
			this.findMeButton.Enabled = true;
			this.featureSquadButton.Visible = false;
			base.TabClicked(selected, SocialTabs.Leaders);
		}

		private void TournamentTabClicked(UXCheckbox box, bool selected)
		{
			this.planetSelectionDropDown.SetStatePlanetOptions(SocialTabs.Tournament);
			this.top50Button.Visible = true;
			this.findMeButton.Visible = true;
			this.top50Button.Enabled = false;
			this.findMeButton.Enabled = true;
			this.featureSquadButton.Visible = false;
			base.TabClicked(selected, SocialTabs.Tournament);
		}

		protected void FactionChanged(FactionToggle faction)
		{
			if (!this.Visible)
			{
				return;
			}
			Service.Get<EventManager>().SendEvent(EventId.SquadSelect, null);
			this.ReloadCurrentTab();
			this.LogLeaderboardFactionUpdate();
		}

		protected void PlanetChanged(PlanetVO planet)
		{
			if (!this.Visible)
			{
				return;
			}
			Service.Get<EventManager>().SendEvent(EventId.SquadSelect, null);
			SocialTabInfo tabInfo = base.GetTabInfo(this.curTab);
			tabInfo.LoadAction.Invoke();
			this.top50Button.Enabled = false;
			this.findMeButton.Enabled = true;
			this.LogLeaderboardPlanetSelection();
		}

		private void LogLeaderboardPlanetSelection()
		{
			Service.Get<EventManager>().SendEvent(base.GetTabInfo(this.curTab).TabEventId, this.planetSelectionDropDown.GetSelectedPlanetTabName());
		}

		private void LogLeaderboardFactionUpdate()
		{
			string selectedFactionString = base.GetSelectedFactionString();
			Service.Get<EventManager>().SendEvent(base.GetTabInfo(this.curTab).TabEventId, selectedFactionString);
		}

		protected override FactionToggle GetSelectedFaction()
		{
			return this.factionSelectorDropDown.GetSelectedFaction();
		}

		protected override string GetSelectedPlanetId()
		{
			return this.planetSelectionDropDown.GetSelectedPlanetId();
		}

		public override void OnRowSelected(AbstractLeaderboardRowView row)
		{
			base.OnRowSelected(row);
			Service.Get<EventManager>().SendEvent(EventId.UILeaderboardExpand, base.GetSelectedFactionString());
		}

		public override void OnVisitClicked(UXButton button)
		{
			string playerId = button.Tag as string;
			bool isFriend = false;
			string tabName = null;
			string text = null;
			switch (this.curTab)
			{
			case SocialTabs.Friends:
				isFriend = true;
				tabName = "Leaderboard_Friends";
				text = "friends";
				break;
			case SocialTabs.Squads:
				tabName = "Leaderboard_TopSquads";
				text = "squad";
				break;
			case SocialTabs.Leaders:
				tabName = "Leaderboard_Players";
				text = "players";
				break;
			case SocialTabs.Tournament:
				tabName = "Leaderboard_Tournament";
				text = "tournament";
				break;
			}
			PlayerVisitTag cookie = new PlayerVisitTag(false, isFriend, tabName, playerId);
			Service.Get<EventManager>().SendEvent(EventId.VisitPlayer, cookie);
			if (text != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.UILeaderboardVisit, text);
			}
			base.OnVisitClicked(button);
		}

		protected void ReloadCurrentTab()
		{
			base.CancelLoaderAndResetGrid();
			SocialTabInfo tabInfo = base.GetTabInfo(this.curTab);
			tabInfo.LoadAction.Invoke();
		}

		public override void ViewSquadInfoClicked(UXButton button)
		{
			Service.Get<EventManager>().SendEvent(EventId.UILeaderboardInfo, "squad");
			base.ViewSquadInfoClicked(button);
		}

		public override void OnDestroyElement()
		{
			base.CleanupScreenTransition(false);
			this.planetSelectionDropDown.DestroyGrid();
			base.OnDestroyElement();
		}

		protected internal LeaderboardsScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).AddItemsToLeaderboardGrid();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).DisplayPlayersNearMe();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).DisplayTop50Players();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).EnableFindMeButton());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).FactionChanged((FactionToggle)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).FeatureSquadButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).FindMeButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).FriendsTabClicked((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).WantTransitions);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).GetLeaderboardList());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).GetSelectedFaction());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).GetSelectedPlanetId());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).InitIndividualGrids((UXGrid)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).InitTabInfo();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).LeadersTabClicked((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).LoadLeaders();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).LoadSquads();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).LoadTournamentLeaders();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).LogLeaderboardFactionUpdate();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).LogLeaderboardPlanetSelection();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).OnDataUpdateFailure();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).OnGetLBPlayers(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).OnGetLBSquads(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).OnGetLBTournament(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).OnRowSelected((AbstractLeaderboardRowView)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).OnVisitClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).PlanetChanged((PlanetVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).PopulatePlayersOnGrid();
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).ReloadCurrentTab();
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).ShowFindMeFailMessage(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).SquadsTabClicked((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).Top50ButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).TournamentTabClicked((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((LeaderboardsScreen)GCHandledObjects.GCHandleToObject(instance)).ViewSquadInfoClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
