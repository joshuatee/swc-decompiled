using StaRTS.Externals.EnvironmentManager;
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
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Leaderboard
{
	public abstract class AbstractLeaderboardScreen : ClosableScreen, IViewFrameTimeObserver
	{
		private const string TITLE_LABEL = "LabelLeaderboards";

		private const string CONTAINER = "LeaderboardsContainer";

		private const string PANEL = "LeaderboardsPanel";

		private const string SCROLL_UP = "SpriteLeaderboardsPanelScrollUp";

		private const string SCROLL_DOWN = "SpriteLeaderboardsPanelScrollDown";

		private const string NAVIGATION = "LeaderboardNavigation";

		private const string LEADERBOARD_NAVIGATION_ROW = "NavigationRow";

		private const string SEARCH_WINDOW = "ContainerSearchWindow";

		private const string GRID = "SquadGrid";

		private const string SQUAD_CREATE_TEMPLATE_ITEM = "SquadCreateItem";

		private const string TEMPLATE_ITEM = "SquadItem";

		private const string FB_TEMPLATE_ITEM = "SquadFacebookItem";

		protected const string BTN_SQUADS = "BtnSquads";

		protected const string BTN_SQUADS_LABEL = "LabelSquads";

		protected const string BTN_LEADERS = "BtnLeaders";

		protected const string BTN_LEADERS_LABEL = "LabelBtnLeaders";

		protected const string BTN_FRIENDS = "BtnFriends";

		protected const string BTN_FRIENDS_LABEL = "LabelBtnFriends";

		protected const string BTN_TOURNAMENT = "BtnTournament";

		protected const string BTN_TOURNAMENT_LABEL = "LabelBtnTournament";

		protected const string BTN_TOP_50 = "BtnSortTop";

		protected const string BTN_TOP_50_LABEL = "LabelBtnSortTop";

		protected const string BTN_FIND_ME = "BtnFindMe";

		protected const string BTN_FIND_ME_LABEL = "LabelBtnFindMe";

		protected const string LB_SQUAD_GRID = "LBSquadGrid";

		protected const string LB_FRIEND_GRID = "LBFriendsGrid";

		protected const string LB_PLAYER_GRID = "LBPlayersGrid";

		protected const string LB_TOURNAMENT_GRID = "LBTournamentGrid";

		protected const string FEATURE_SQUAD_BUTTON = "BtnFeaturedSquads";

		protected const string LABEL_FEATURE_SQUAD_BUTTON = "LabelBtnFeaturedSquads";

		private const int MAX_ELEMENTS = 51;

		private const float SCROLL_DELTA = 30f;

		protected const int DEFAULT_SCROLL_ARROW_ANCHOR = -4;

		protected const int SEARCH_SCROLL_ARROW_ANCHOR = -175;

		private static readonly Vector3 OFFSCREEN_GRID_POSITION = new Vector3(3000f, 0f, 0f);

		private const string GENERIC_SQUAD_INFO_ISSUE = "GENERIC_SQUAD_INFO_ISSUE";

		protected GridLoadHelper gridLoadHelper;

		protected UIScrollView scrollView;

		protected Dictionary<SocialTabs, SocialTabInfo> tabs;

		protected SocialTabs initialTab;

		protected PlanetVO initialPlanetVO;

		protected SquadInfoView squadInfoView;

		protected BackButtonHelper backButtonHelper;

		protected List<AbstractLeaderboardRowView> rowViews;

		protected AbstractLeaderboardRowView selectedRow;

		private LeaderboardRowFacebookView facebookRow;

		protected UXElement itemTemplate;

		protected UXElement createSquadItemTemplate;

		protected UXElement fbItemTemplate;

		protected UXLabel titleLabel;

		protected UXElement container;

		protected UXElement navigation;

		protected UXElement navigationRow;

		protected UXElement panel;

		protected UXElement searchOverlay;

		protected UXSprite scrollUp;

		protected UXSprite scrollDown;

		protected SocialTabs curTab;

		private int currentPlayerTileIndex;

		private float currentScrollOffset;

		public AbstractLeaderboardScreen()
		{
			this.curTab = SocialTabs.Empty;
			base..ctor("gui_leaderboards");
			Service.Get<BuildingController>().CancelEditModeTimer();
			this.backButtonHelper = new BackButtonHelper(this);
			this.squadInfoView = new SquadInfoView(this);
			this.backButtonHelper.BackButtonCallBack = new Action(this.CheckBackButton);
			this.rowViews = new List<AbstractLeaderboardRowView>();
			this.tabs = new Dictionary<SocialTabs, SocialTabInfo>();
			this.InitTabInfo();
		}

		protected abstract void InitTabInfo();

		public string GetTabString()
		{
			return this.curTab.ToString();
		}

		protected SocialTabInfo GetTabInfo(SocialTabs socialTab)
		{
			return this.tabs[socialTab];
		}

		private void CheckBackButton()
		{
			if (this.backButtonHelper.IsBackButtonEnabled())
			{
				base.CurrentBackDelegate = new UXButtonClickedDelegate(this.GoBackDelagate);
				base.CurrentBackButton = this.backButtonHelper.GetBackButton();
				return;
			}
			base.InitDefaultBackDelegate();
		}

		private void GoBackDelagate(UXButton btn)
		{
			this.backButtonHelper.GoBack();
		}

		protected override void OnScreenLoaded()
		{
			this.squadInfoView.OnScreenLoaded();
			this.createSquadItemTemplate = base.GetElement<UXElement>("SquadCreateItem");
			this.fbItemTemplate = base.GetElement<UXElement>("SquadFacebookItem");
			this.itemTemplate = base.GetElement<UXElement>("SquadItem");
			this.createSquadItemTemplate.Visible = false;
			this.fbItemTemplate.Visible = false;
			this.itemTemplate.Visible = false;
			this.searchOverlay = base.GetElement<UXElement>("ContainerSearchWindow");
			this.searchOverlay.Visible = false;
			this.scrollUp = base.GetElement<UXSprite>("SpriteLeaderboardsPanelScrollUp");
			this.scrollDown = base.GetElement<UXSprite>("SpriteLeaderboardsPanelScrollDown");
			this.titleLabel = base.GetElement<UXLabel>("LabelLeaderboards");
			this.container = base.GetElement<UXElement>("LeaderboardsContainer");
			this.container.Visible = true;
			this.navigation = base.GetElement<UXElement>("LeaderboardNavigation");
			this.navigation.Visible = true;
			this.navigationRow = base.GetElement<UXElement>("NavigationRow");
			this.navigationRow.Visible = true;
			this.panel = base.GetElement<UXElement>("LeaderboardsPanel");
			this.scrollView = NGUITools.FindInParents<UIScrollView>(this.panel.Root);
			this.InitButtons();
			List<UXElement> elements = new List<UXElement>
			{
				this.navigation,
				this.container
			};
			this.backButtonHelper.InitWithMultipleElementsLayer(elements);
			UXGrid element = base.GetElement<UXGrid>("SquadGrid");
			element.BypassLocalPositionOnAdd = true;
			element.LocalPosition = AbstractLeaderboardScreen.OFFSCREEN_GRID_POSITION;
			element.Enabled = false;
			this.InitIndividualGrids(element);
			UIPanel component = this.panel.Root.GetComponent<UIPanel>();
			Vector2 clipOffset = component.clipOffset;
			clipOffset.x = 0f;
			component.clipOffset = clipOffset;
			SocialTabInfo tabInfo = this.GetTabInfo(this.initialTab);
			tabInfo.TabButton.SetSelected(true);
			this.gridLoadHelper = tabInfo.TabGridLoadHelper;
		}

		protected abstract void InitIndividualGrids(UXGrid baseGrid);

		protected GridLoadHelper CreateGridLoadHelperByCloningGrid(UXGrid originalGrid, string itemId)
		{
			UXGrid grid = base.CloneElement<UXGrid>(originalGrid, itemId, originalGrid.Root.transform.parent.gameObject);
			GridLoadHelper result = new GridLoadHelper(new GridLoadHelper.CreateUXElementFromGridItem(this.CreateUXElementFromGridItem), grid);
			this.EnableGridLoadHelper(result, false);
			Service.Get<EventManager>().SendEvent(EventId.AllUXElementsCreated, this);
			return result;
		}

		protected void EnableGridLoadHelper(GridLoadHelper gridLoadHelper, bool isEnabled)
		{
			UXGrid grid = gridLoadHelper.GetGrid();
			grid.Enabled = isEnabled;
			grid.Visible = isEnabled;
			grid.LocalPosition = (isEnabled ? Vector3.zero : AbstractLeaderboardScreen.OFFSCREEN_GRID_POSITION);
		}

		protected void ResetGrid()
		{
			this.CleanupRowViews();
			this.facebookRow = null;
			this.gridLoadHelper.ResetGrid();
		}

		protected void TabClicked(bool selected, SocialTabs clickedTab)
		{
			if (!selected)
			{
				return;
			}
			if (this.curTab != clickedTab)
			{
				this.curTab = clickedTab;
				SocialTabInfo tabInfo = this.GetTabInfo(clickedTab);
				this.DoTabClickedReset(tabInfo.TabGridLoadHelper);
				tabInfo.LoadAction.Invoke();
				string cookie = (tabInfo.EventActionId != null) ? tabInfo.EventActionId : this.GetSelectedFactionString();
				Service.Get<EventManager>().SendEvent(tabInfo.TabEventId, cookie);
				Service.Get<UXController>().MiscElementsManager.TryCloseNonFatalAlertScreen();
				this.PositionScrollViewForTop50();
				foreach (KeyValuePair<SocialTabs, SocialTabInfo> current in this.tabs)
				{
					if (current.get_Key() == clickedTab)
					{
						current.get_Value().TabLabel.TextColor = UXUtils.COLOR_NAV_TAB_ENABLED;
					}
					else
					{
						current.get_Value().TabLabel.TextColor = UXUtils.COLOR_NAV_TAB_DISABLED;
					}
				}
			}
		}

		public void ForceSwitchTab(SocialTabs tab)
		{
			foreach (KeyValuePair<SocialTabs, SocialTabInfo> current in this.tabs)
			{
				current.get_Value().TabButton.Selected = false;
			}
			this.GetTabInfo(tab).TabButton.Selected = true;
			this.TabClicked(true, tab);
		}

		private void DoTabClickedReset(GridLoadHelper switchingGridLoadHelper)
		{
			ProcessingScreen.Hide();
			this.EnableGridLoadHelper(this.gridLoadHelper, false);
			this.gridLoadHelper = switchingGridLoadHelper;
			this.EnableGridLoadHelper(this.gridLoadHelper, true);
			Service.Get<EventManager>().SendEvent(EventId.SquadSelect, null);
			this.ResetGrid();
			this.RepositionGridItems();
			this.scrollView.ResetPosition();
		}

		protected void AddItemsToGrid<T>(List<T> list, bool addOverTime, bool resetGrid)
		{
			this.currentPlayerTileIndex = 0;
			if (resetGrid)
			{
				this.ResetGrid();
			}
			if (list != null && list.Count > 0)
			{
				this.CullElementList<T>(list);
				GridDataCookie gridDataCookie = new GridDataCookie(this.curTab, this.GetSelectedFaction(), this.GetSelectedPlanetId());
				if (addOverTime)
				{
					this.gridLoadHelper.StartAddingItemOverTime<T>(list, gridDataCookie, null);
				}
				else
				{
					this.gridLoadHelper.AddItems<T>(list, gridDataCookie);
				}
			}
			this.scrollView.ResetPosition();
		}

		protected void RepositionGridItems()
		{
			this.gridLoadHelper.GetGrid().RepositionItemsFrameDelayed();
		}

		private UXElement CreateUXElementFromGridItem(object itemObject, object cookie, int position)
		{
			GridDataCookie gridDataCookie = (GridDataCookie)cookie;
			SocialTabs selectedTab = gridDataCookie.SelectedTab;
			FactionToggle selectedFaction = gridDataCookie.SelectedFaction;
			string selectedPlanet = gridDataCookie.SelectedPlanet;
			AbstractLeaderboardRowView abstractLeaderboardRowView = null;
			if (itemObject is PlayerLBEntity)
			{
				abstractLeaderboardRowView = this.AddPlayerRow((PlayerLBEntity)itemObject, selectedTab, selectedFaction, selectedPlanet, position);
			}
			else if (itemObject is Squad)
			{
				abstractLeaderboardRowView = this.AddSquadRow((Squad)itemObject, selectedTab, selectedFaction, position);
			}
			else if (itemObject is SquadInvite)
			{
				abstractLeaderboardRowView = this.AddSquadInviteRow((SquadInvite)itemObject, selectedTab, selectedFaction, position);
			}
			UXElement result = null;
			if (abstractLeaderboardRowView != null)
			{
				result = abstractLeaderboardRowView.GetItem();
				this.rowViews.Add(abstractLeaderboardRowView);
			}
			return result;
		}

		private AbstractLeaderboardRowView AddPlayerRow(PlayerLBEntity player, SocialTabs tab, FactionToggle faction, string planetUid, int position)
		{
			if (player == null || string.IsNullOrEmpty(player.PlayerName))
			{
				return null;
			}
			if (!this.IsFactionToggleValidFactionType(faction, player.Faction) || position >= 51)
			{
				return null;
			}
			if (player.PlayerID == Service.Get<CurrentPlayer>().PlayerId)
			{
				this.currentPlayerTileIndex = position;
			}
			return new LeaderboardRowPlayerView(this, this.gridLoadHelper.GetGrid(), this.itemTemplate, tab, faction, position, player, planetUid);
		}

		private AbstractLeaderboardRowView AddSquadRow(Squad squad, SocialTabs tab, FactionToggle faction, int position)
		{
			if (squad == null)
			{
				return null;
			}
			if (!this.IsFactionToggleValidFactionType(faction, squad.Faction) || position >= 51)
			{
				return null;
			}
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad != null && currentSquad.SquadID == squad.SquadID)
			{
				this.currentPlayerTileIndex = position;
			}
			return new LeaderboardRowSquadView(this, this.gridLoadHelper.GetGrid(), this.itemTemplate, tab, faction, position, squad);
		}

		private AbstractLeaderboardRowView AddSquadInviteRow(SquadInvite invite, SocialTabs tab, FactionToggle faction, int position)
		{
			if (invite == null || string.IsNullOrEmpty(invite.SquadId))
			{
				return null;
			}
			Squad orCreateSquad = Service.Get<LeaderboardController>().GetOrCreateSquad(invite.SquadId);
			if (!this.IsFactionToggleValidFactionType(faction, orCreateSquad.Faction) || position >= 51)
			{
				return null;
			}
			return new LeaderboardRowSquadInviteView(this, this.gridLoadHelper.GetGrid(), this.itemTemplate, tab, faction, position, invite, orCreateSquad);
		}

		public virtual void OnRowSelected(AbstractLeaderboardRowView row)
		{
			if (this.selectedRow != null && this.selectedRow == row)
			{
				this.selectedRow.Deselect();
			}
			int i = 0;
			int count = this.rowViews.Count;
			while (i < count)
			{
				if (this.rowViews[i] != row)
				{
					this.rowViews[i].Deselect();
				}
				i++;
			}
			if (this.selectedRow != row)
			{
				this.selectedRow = row;
				return;
			}
			this.selectedRow = null;
		}

		protected abstract FactionToggle GetSelectedFaction();

		protected string GetSelectedFactionString()
		{
			return this.GetSelectedFaction().ToString().ToLower();
		}

		protected abstract string GetSelectedPlanetId();

		private bool IsFactionToggleValidFactionType(FactionToggle selectedFaction, FactionType faction)
		{
			return selectedFaction == FactionToggle.All || (faction == FactionType.Empire && selectedFaction == FactionToggle.Empire) || (faction == FactionType.Rebel && selectedFaction == FactionToggle.Rebel);
		}

		protected void LoadFriends()
		{
			this.ResetGrid();
			ProcessingScreen.Show();
			Service.Get<ISocialDataController>().FriendsDetailsCB = new OnFBFriendsDelegate(this.OnFriendsListLoaded);
			Service.Get<ISocialDataController>().UpdateFriends();
			UXUtils.SetSpriteTopAnchorPoint(this.scrollUp, -4);
		}

		private void OnFriendsListLoaded()
		{
			if (!this.Visible)
			{
				ProcessingScreen.Hide();
				return;
			}
			this.OnPlayersListLoaded(Service.Get<LeaderboardController>().Friends.List, new Action(this.PopulateFriendsOnGrid));
		}

		private void PopulateFriendsOnGrid()
		{
			ProcessingScreen.Hide();
			if (this.curTab == SocialTabs.Friends)
			{
				this.ResetGrid();
				this.AddFBConnectItem(SocialTabs.Friends);
				this.AddItemsToGrid<PlayerLBEntity>(Service.Get<LeaderboardController>().Friends.List, true, false);
			}
		}

		protected void OnPlayersListLoaded(List<PlayerLBEntity> playerList, Action populateItemsCallback)
		{
			if (playerList != null && GameConstants.SQUAD_INVITES_ENABLED && Service.Get<SquadController>().StateManager.GetCurrentSquad() != null)
			{
				FactionType faction = Service.Get<CurrentPlayer>().Faction;
				List<string> list = new List<string>();
				int i = 0;
				int count = playerList.Count;
				while (i < count)
				{
					PlayerLBEntity playerLBEntity = playerList[i];
					if (string.IsNullOrEmpty(playerLBEntity.SquadID) && playerLBEntity.Faction == faction)
					{
						list.Add(playerLBEntity.PlayerID);
					}
					i++;
				}
				Service.Get<SquadController>().CheckSquadInvitesSentToPlayers(list, populateItemsCallback);
				return;
			}
			populateItemsCallback.Invoke();
		}

		public virtual void OnVisitClicked(UXButton button)
		{
			GameUtils.ExitEditState();
			Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
			string text = button.Tag as string;
			this.Close(text);
			Service.Get<UXController>().HUD.DestroySquadScreen();
			Service.Get<NeighborVisitManager>().VisitNeighbor(text);
		}

		public virtual void ViewSquadInfoClicked(UXButton button)
		{
			string squadId = (string)button.Tag;
			this.squadInfoView.ToggleInfoVisibility(false);
			ProcessingScreen.Show();
			Service.Get<LeaderboardController>().UpdateSquadDetails(squadId, new LeaderboardController.OnUpdateSquadData(this.OnSquadDetailsUpdated));
			this.CheckBackButton();
			Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
		}

		public void ViewSquadInfoInviteClicked(UXButton button)
		{
			SquadInvite squadInvite = (SquadInvite)button.Tag;
			Squad orCreateSquad = Service.Get<LeaderboardController>().GetOrCreateSquad(squadInvite.SquadId);
			this.DisplaySquadInfoView(orCreateSquad, false, null);
			Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
		}

		private void OnSquadDetailsUpdated(Squad squad, bool success)
		{
			ProcessingScreen.Hide();
			if (this.IsClosedOrClosing())
			{
				return;
			}
			if (!success)
			{
				string message = this.lang.Get("GENERIC_SQUAD_INFO_ISSUE", new object[0]);
				AlertScreen.ShowModal(false, null, message, null, null);
				return;
			}
			if (squad != null)
			{
				string detailsString;
				bool showRequestButton = SquadUtils.CanCurrentPlayerJoinSquad(Service.Get<CurrentPlayer>(), Service.Get<SquadController>().StateManager.GetCurrentSquad(), squad, Service.Get<Lang>(), out detailsString);
				this.DisplaySquadInfoView(squad, showRequestButton, detailsString);
				return;
			}
			success = false;
		}

		private void DisplaySquadInfoView(Squad squad, bool showRequestButton, string detailsString)
		{
			this.backButtonHelper.AddLayer(this.squadInfoView.Container);
			this.squadInfoView.DisplaySquadInfo(squad, showRequestButton, detailsString);
			this.CheckBackButton();
		}

		protected void AddFBConnectItem(SocialTabs socialTab)
		{
			if (Service.Get<EnvironmentController>().IsRestrictedProfile())
			{
				return;
			}
			if (this.facebookRow != null)
			{
				return;
			}
			this.facebookRow = new LeaderboardRowFacebookView(this, this.gridLoadHelper.GetGrid(), this.fbItemTemplate, this.curTab);
			this.rowViews.Add(this.facebookRow);
			this.gridLoadHelper.AddElement(this.facebookRow.GetItem());
			this.RepositionGridItems();
			this.scrollView.ResetPosition();
		}

		public void OnFacebookLoggedIn()
		{
			if (this.curTab == SocialTabs.Friends)
			{
				this.CancelLoaderAndResetGrid();
				this.LoadFriends();
			}
			Service.Get<EventManager>().SendEvent(EventId.FacebookLoggedIn, "UI_leaderboard_friends");
		}

		private void CullElementList<T>(List<T> elementList)
		{
			if (elementList.Count >= 51)
			{
				int num = elementList.Count - 51 + 1;
				elementList.RemoveRange(elementList.Count - num, num);
			}
		}

		protected void PositionScrollViewForFindMe()
		{
			if (this.currentPlayerTileIndex > 0)
			{
				this.currentPlayerTileIndex--;
				UXGrid grid = this.gridLoadHelper.GetGrid();
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
				this.currentScrollOffset = (float)this.currentPlayerTileIndex * (grid.CellHeight / this.uxCamera.Scale);
			}
		}

		protected void PositionScrollViewForTop50()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			this.currentScrollOffset = 0f;
		}

		public void OnViewFrameTime(float dt)
		{
			this.currentScrollOffset -= 30f;
			if (this.currentScrollOffset > 0f)
			{
				this.scrollView.MoveRelative(new Vector3(0f, 30f));
				return;
			}
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		protected void DestroyGridLoadHelper(GridLoadHelper tmpGridLoadHelper, bool isDestroyGrid)
		{
			if (tmpGridLoadHelper != null)
			{
				tmpGridLoadHelper.OnDestroyElement();
				if (isDestroyGrid)
				{
					base.DestroyElement(tmpGridLoadHelper.GetGrid());
				}
			}
		}

		protected void CancelLoaderAndResetGrid()
		{
			ProcessingScreen.Hide();
			this.ResetGrid();
			this.RepositionGridItems();
			this.scrollView.ResetPosition();
		}

		public void RemoveAndDestroyRow(AbstractLeaderboardRowView rowView)
		{
			UXElement item = rowView.GetItem();
			this.gridLoadHelper.RemoveElement(item);
			base.DestroyElement(item);
			rowView.Destroy();
			this.rowViews.Remove(rowView);
		}

		private void CleanupRowViews()
		{
			int i = 0;
			int count = this.rowViews.Count;
			while (i < count)
			{
				this.rowViews[i].Destroy();
				i++;
			}
			this.rowViews.Clear();
		}

		public override void Close(object modalResult)
		{
			this.curTab = SocialTabs.Empty;
			base.Close(modalResult);
		}

		private bool IsClosedOrClosing()
		{
			return !this.Visible || this.curTab == SocialTabs.Empty;
		}

		public override void OnDestroyElement()
		{
			this.CleanupRowViews();
			this.squadInfoView.Destroy();
			foreach (KeyValuePair<SocialTabs, SocialTabInfo> current in this.tabs)
			{
				this.DestroyGridLoadHelper(current.get_Value().TabGridLoadHelper, true);
			}
			base.OnDestroyElement();
		}

		protected internal AbstractLeaderboardScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).AddFBConnectItem((SocialTabs)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).AddPlayerRow((PlayerLBEntity)GCHandledObjects.GCHandleToObject(*args), (SocialTabs)(*(int*)(args + 1)), (FactionToggle)(*(int*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), *(int*)(args + 4)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).AddSquadInviteRow((SquadInvite)GCHandledObjects.GCHandleToObject(*args), (SocialTabs)(*(int*)(args + 1)), (FactionToggle)(*(int*)(args + 2)), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).AddSquadRow((Squad)GCHandledObjects.GCHandleToObject(*args), (SocialTabs)(*(int*)(args + 1)), (FactionToggle)(*(int*)(args + 2)), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).CancelLoaderAndResetGrid();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).CheckBackButton();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).CleanupRowViews();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).CreateGridLoadHelperByCloningGrid((UXGrid)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).CreateUXElementFromGridItem(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).DestroyGridLoadHelper((GridLoadHelper)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).DisplaySquadInfoView((Squad)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).DoTabClickedReset((GridLoadHelper)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).EnableGridLoadHelper((GridLoadHelper)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).ForceSwitchTab((SocialTabs)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).GetSelectedFaction());
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).GetSelectedFactionString());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).GetSelectedPlanetId());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).GetTabInfo((SocialTabs)(*(int*)args)));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).GetTabString());
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).GoBackDelagate((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).InitIndividualGrids((UXGrid)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).InitTabInfo();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).IsClosedOrClosing());
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).IsFactionToggleValidFactionType((FactionToggle)(*(int*)args), (FactionType)(*(int*)(args + 1))));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).LoadFriends();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).OnFacebookLoggedIn();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).OnFriendsListLoaded();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).OnPlayersListLoaded((List<PlayerLBEntity>)GCHandledObjects.GCHandleToObject(*args), (Action)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).OnRowSelected((AbstractLeaderboardRowView)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).OnSquadDetailsUpdated((Squad)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).OnVisitClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).PopulateFriendsOnGrid();
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).PositionScrollViewForFindMe();
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).PositionScrollViewForTop50();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).RemoveAndDestroyRow((AbstractLeaderboardRowView)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).RepositionGridItems();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).ResetGrid();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).TabClicked(*(sbyte*)args != 0, (SocialTabs)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).ViewSquadInfoClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((AbstractLeaderboardScreen)GCHandledObjects.GCHandleToObject(instance)).ViewSquadInfoInviteClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
