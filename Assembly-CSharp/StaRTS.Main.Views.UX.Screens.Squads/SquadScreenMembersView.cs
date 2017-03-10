using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Chat;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadScreenMembersView : AbstractSquadScreenViewModule, IEventObserver
	{
		private const string MEMBER_CONTAINER = "MemberContainer";

		private const string TAB_BUTTON_NAME = "SocialMembersBtn";

		private const string MEMBER_PANEL = "MemberPanel";

		private const string SQUAD_MEMBERS = "SQUAD_MEMBERS";

		private const string VICTORIES = "s_Victories";

		private const string TROOPS = "s_Troops";

		private const string ATTACKS_WON = "ATTACKS_WON";

		private const string DEFENSES_WON = "DEFENSES_WON";

		private const string CANT_REMOVE_MEMBER_IN_WAR = "CANT_REMOVE_MEMBER_IN_WAR";

		private const string WAR_IN_WAR = "WAR_IN_WAR";

		private const string WAR_IN_MATCHMAKING = "WAR_IN_MATCHMAKING";

		private const string WAR_LOOK_FOR_MATCH = "WAR_LOOK_FOR_MATCH";

		private const string WAR_MEMBERS_SELECTED = "WAR_MEMBERS_SELECTED";

		private const int DEFAULT_GRID_BOTTOM_ANCHOR = 15;

		private const int WAR_SELECT_GRID_BOTTOM_ANCHOR = 120;

		private const string SORT_MEDALS = "s_Medals";

		private const string SORT_ATTACKS = "s_AttacksWon";

		private const string SORT_DEFENSES = "s_DefensesWon";

		private const string SORT_DONATED = "s_Donated";

		private const string SORT_RECEIVED = "s_Received";

		private const string SORT_LASTACTIVE = "s_LastActive";

		private const string MEMBERS = "members";

		private const string MEMBER_PREFIX = "member_";

		private const string MEMBERS_CONTAINER = "MemberContainer";

		private const string MEMBER_GRID = "MemberGrid";

		private const string MEMBER_TEMPLATE = "MemberItem";

		private const string MEMBER_TITLE = "LabelMemberTitle";

		private const string MEMBER_SORT_BTN = "BtnFilterMembers";

		private const string MEMBER_SORT_LABEL = "LabelBtnFilterMembers";

		private const string MEMBER_SORT = "MemberFilterOptions";

		private const string MEMBER_MEDAL_SORT_BTN = "BtnMedals";

		private const string MEMBER_ATTACK_SORT_BTN = "BtnAttacksWon";

		private const string MEMBER_DEFENSE_SORT_BTN = "BtnDefensesWon";

		private const string MEMBER_DONATED_SORT_BTN = "BtnDonated";

		private const string MEMBER_RECEIVED_SORT_BTN = "BtnReceived";

		private const string MEMBER_ACTIVE_SORT_BTN = "BtnLastActive";

		private const string MT_PLAYER_INFO_GROUP = "MemberInfo";

		private const string MT_PLAYER_NAME_LABEL = "LabelMemberName";

		private const string MT_PLAYER_RANK_LABEL = "LabelMemberRank";

		private const string MT_PLAYER_ROLE_LABEL = "LabelMemberRole";

		private const string MT_PLAYER_SCORE_LABEL = "LabelMemberScore";

		private const string MT_REP_INVESTED_LABEL = "LabelMemberRepAmt";

		private const string MT_TROOPS_DONATED_LABEL = "LabelTroopsDonated";

		private const string MT_TROOPS_RECEIVED_LABEL = "LabelTroopsReceived";

		private const string MT_BUTTON_CONTAINER = "ButtonContainer";

		private const string MT_BUTTON_1 = "Btn1";

		private const string MT_BUTTON_1_LABEL = "LabelBtn1";

		private const string MT_BUTTON_2 = "Btn2";

		private const string MT_BUTTON_2_LABEL = "LabelBtn2";

		private const string MT_BUTTON_3 = "Btn3";

		private const string MT_BUTTON_3_LABEL = "LabelBtn3";

		private const string MT_LABEL_VICTORIES = "LabelVictories";

		private const string MT_LABEL_TROOPS = "LabelTroops";

		private const string MT_LABEL_ATK = "LabelAttacksWon";

		private const string MT_LABEL_DEF = "LabelDefensesWon";

		private const string MT_LABEL_LAST_LOGIN = "LabelMemberLastLogin";

		private const string MT_CONFLICT_MEDAL_INFO_GROUP = "MedalInfo";

		private const string MT_LABEL_TOURNAMENT_SCORE_01 = "LabelMemberTournamentScore01";

		private const string MT_SPRITE_TOURNAMENT_SCORE_01 = "SpriteMemberTournamentMedalIcon01";

		private const string MT_LABEL_TOURNAMENT_SCORE_02 = "LabelMemberTournamentScore02";

		private const string MT_SPRITE_TOURNAMENT_SCORE_02 = "SpriteMemberTournamentMedalIcon02";

		private const string MT_LABEL_TOURNAMENT_SCORE_03 = "LabelMemberTournamentScore03";

		private const string MT_SPRITE_TOURNAMENT_SCORE_03 = "SpriteMemberTournamentMedalIcon03";

		private const string MT_TEXTURE_PLANET_BG = "TexturePlanetBg";

		private const string MT_GROUP_SELECT_MEMBER = "BtnSelectMember";

		private const string MT_SPRITE_SELECT_MEMBER = "SpriteCheckBtnSelectMember";

		private const string MT_SPRITE_MORE_OPTIONS = "SpriteMoreOptions";

		private const string MT_SPRITE_WAR = "SpriteWarIcon";

		private const string S_VIEW = "s_View";

		private const string S_REMOVE = "s_Remove";

		private const string TROOPS_DONATED = "TROOPS_DONATED";

		private const string TROOPS_RECEIVED = "TROOPS_RECEIVED";

		private const string REPUTATION_INVESTED = "REPUTATION_INVESTED";

		private const string S_DEMOTE = "s_Demote";

		private const string S_PROMOTE = "s_Promote";

		private const string CHAT_CONTAINER = "ChatContainer";

		private const string PROMOTE_SQUAD_MEMBER_ALERT = "PROMOTE_SQUAD_MEMBER_ALERT";

		private const string SQUAD_OFFICER = "SQUAD_OFFICER";

		private const string DEMOTE_SQUAD_MEMBER_ALERT = "DEMOTE_SQUAD_MEMBER_ALERT";

		private const string SQUAD_MEMBER = "SQUAD_MEMBER";

		private const string REMOVE_SQUAD_MEMBER_ALERT = "REMOVE_SQUAD_MEMBER_ALERT";

		private const string CANCEL = "CANCEL";

		private const string MEMBER_NAME_FORMAT = "WAR_MEMBER_NAME";

		private const string WAR_START_HQ_INELIGIBLE = "WAR_START_HQ_INELIGIBLE";

		private const string GROUP_START_WAR_BTNS = "GroupStartWarBtns";

		private const string GROUP_SAME_FACTION_MM = "GroupSameFactionMM";

		private const string BTN_START_WAR_CONFIRM = "BtnStartWarConfirm";

		private const string BTN_CANCEL_START_WAR = "BtnCancelStartWar";

		private const string BTN_SAME_FACTION = "BtnSameFactionMM";

		private const string BTN_INFO_SAME_FACTION = "BtnInfoSameFactionMM";

		private const string LABEL_START_WAR_SELECTED = "LabelStartWarSelected";

		private const string LABEL_BTN_START_WAR_CONFIRM = "LabelBtnStartWarConfirm";

		private const string LABEL_BTN_CANCEL_START_WAR = "LabelBtnCancelStartWar";

		private const string LABEL_SAME_FACTION = "LabelSameFactionMM";

		private const string SPRITE_CHECK_SAME_FACTION = "SpriteCheckBtnSameFactionMM";

		private const string STRING_SAME_FACTION_DESC_E = "WAR_SAME_FACTION_DESC_E";

		private const string STRING_SAME_FACTION_DESC_R = "WAR_SAME_FACTION_DESC_R";

		private const string STRING_SAME_FACTION_CHECK_E = "WAR_SAME_FACTION_CHECK_E";

		private const string STRING_SAME_FACTION_CHECK_R = "WAR_SAME_FACTION_CHECK_R";

		private UXGrid squadMemberGrid;

		private UXElement memberSortBox;

		private UXButton memberSortBtn;

		private UXLabel memberSortLabel;

		private UXCheckbox memberSortMedalsBtn;

		private UXCheckbox memberSortAttacksBtn;

		private UXCheckbox memberSortDefensesBtn;

		private UXCheckbox memberSortDontatedBtn;

		private UXCheckbox memberSortReceivedBtn;

		private UXCheckbox memberSortActiveBtn;

		private UXElement memberPanel;

		private UXElement memberContainer;

		private UXCheckbox tabButton;

		private UXElement groupStartWarBtns;

		private UXButton btnStartWarConfirm;

		private UXButton btnSameFaction;

		private UXButton btnInfoSameFaction;

		private UXButton btnCancelStartWar;

		private UXSprite spriteCheckSameFaction;

		private UXLabel labelStartWarSelected;

		private UXLabel labelBtnStartWarConfirm;

		private UXLabel labelBtnCancelStartWar;

		private UXLabel labelSameFaction;

		private SquadMemberSortType curSortType;

		private bool allowSameFaction;

		public SquadScreenMembersView(SquadSlidingScreen screen)
		{
			this.allowSameFaction = GameConstants.SAME_FACTION_MATCHMAKING_DEFAULT;
			base..ctor(screen);
		}

		public override void OnScreenLoaded()
		{
			this.memberPanel = this.screen.GetElement<UXElement>("MemberPanel");
			this.memberContainer = this.screen.GetElement<UXElement>("MemberContainer");
			this.tabButton = this.screen.GetElement<UXCheckbox>("SocialMembersBtn");
			this.tabButton.OnSelected = new UXCheckboxSelectedDelegate(this.OnTabButtonSelected);
			this.InitMemberList();
		}

		protected void InitMemberList()
		{
			this.squadMemberGrid = this.screen.GetElement<UXGrid>("MemberGrid");
			this.squadMemberGrid.SetTemplateItem("MemberItem");
			this.squadMemberGrid.BypassLocalPositionOnAdd = true;
			this.squadMemberGrid.DupeOrdersAllowed = true;
			this.squadMemberGrid.RepositionCallback = new Action(this.RepositionFinished);
			this.squadMemberGrid.SetSortModeCustom();
			UXLabel element = this.screen.GetElement<UXLabel>("LabelMemberTitle");
			element.Text = this.lang.Get("SQUAD_MEMBERS", new object[0]);
			this.memberSortMedalsBtn = this.screen.GetElement<UXCheckbox>("BtnMedals");
			this.SetupMemberSortButton(this.memberSortMedalsBtn, SquadMemberSortType.Medals);
			this.memberSortAttacksBtn = this.screen.GetElement<UXCheckbox>("BtnAttacksWon");
			this.SetupMemberSortButton(this.memberSortAttacksBtn, SquadMemberSortType.Attacks);
			this.memberSortDefensesBtn = this.screen.GetElement<UXCheckbox>("BtnDefensesWon");
			this.SetupMemberSortButton(this.memberSortDefensesBtn, SquadMemberSortType.Defenses);
			this.memberSortDontatedBtn = this.screen.GetElement<UXCheckbox>("BtnDonated");
			this.SetupMemberSortButton(this.memberSortDontatedBtn, SquadMemberSortType.Donated);
			this.memberSortReceivedBtn = this.screen.GetElement<UXCheckbox>("BtnReceived");
			this.SetupMemberSortButton(this.memberSortReceivedBtn, SquadMemberSortType.Received);
			this.memberSortActiveBtn = this.screen.GetElement<UXCheckbox>("BtnLastActive");
			this.SetupMemberSortButton(this.memberSortActiveBtn, SquadMemberSortType.Active);
			this.memberSortBox = this.screen.GetElement<UXElement>("MemberFilterOptions");
			this.memberSortBox.Visible = false;
			this.memberSortBtn = this.screen.GetElement<UXButton>("BtnFilterMembers");
			this.memberSortBtn.OnClicked = new UXButtonClickedDelegate(this.OnMemberSortOpenClicked);
			this.memberSortLabel = this.screen.GetElement<UXLabel>("LabelBtnFilterMembers");
			this.groupStartWarBtns = this.screen.GetElement<UXElement>("GroupStartWarBtns");
			this.btnStartWarConfirm = this.screen.GetElement<UXButton>("BtnStartWarConfirm");
			this.btnStartWarConfirm.OnClicked = new UXButtonClickedDelegate(this.OnStartWarConfirm);
			this.btnCancelStartWar = this.screen.GetElement<UXButton>("BtnCancelStartWar");
			this.btnCancelStartWar.OnClicked = new UXButtonClickedDelegate(this.OnCancelStartWar);
			this.labelStartWarSelected = this.screen.GetElement<UXLabel>("LabelStartWarSelected");
			this.labelBtnStartWarConfirm = this.screen.GetElement<UXLabel>("LabelBtnStartWarConfirm");
			this.labelBtnStartWarConfirm.Text = this.lang.Get("WAR_LOOK_FOR_MATCH", new object[0]);
			this.labelBtnCancelStartWar = this.screen.GetElement<UXLabel>("LabelBtnCancelStartWar");
			this.labelBtnCancelStartWar.Text = this.lang.Get("CANCEL", new object[0]);
			this.curSortType = SquadMemberSortType.Medals;
			SquadController squadController = Service.Get<SquadController>();
			Squad currentSquad = squadController.StateManager.GetCurrentSquad();
			bool flag = currentSquad.Faction == FactionType.Empire;
			this.btnSameFaction = this.screen.GetElement<UXButton>("BtnSameFactionMM");
			this.btnSameFaction.OnClicked = new UXButtonClickedDelegate(this.OnSameFaction);
			this.btnInfoSameFaction = this.screen.GetElement<UXButton>("BtnInfoSameFactionMM");
			this.btnInfoSameFaction.OnClicked = new UXButtonClickedDelegate(this.OnSameFactionInfo);
			string text;
			if (flag)
			{
				text = Service.Get<Lang>().Get("WAR_SAME_FACTION_CHECK_E", new object[0]);
			}
			else
			{
				text = Service.Get<Lang>().Get("WAR_SAME_FACTION_CHECK_R", new object[0]);
			}
			this.spriteCheckSameFaction = this.screen.GetElement<UXSprite>("SpriteCheckBtnSameFactionMM");
			this.spriteCheckSameFaction.Visible = this.allowSameFaction;
			this.labelSameFaction = this.screen.GetElement<UXLabel>("LabelSameFactionMM");
			this.labelSameFaction.Text = text;
			if (!GameConstants.WAR_ALLOW_SAME_FACTION_MATCHMAKING)
			{
				UXElement element2 = this.screen.GetElement<UXElement>("GroupSameFactionMM");
				element2.Visible = false;
			}
		}

		public override void ShowView()
		{
			EventManager eventManager = Service.Get<EventManager>();
			this.memberContainer.Visible = true;
			eventManager.SendEvent(EventId.SquadSelect, null);
			eventManager.SendEvent(EventId.UISquadScreenTabShown, "members");
			this.HandleMemberSortOptionClicked();
			this.RefreshView();
			eventManager.RegisterObserver(this, EventId.SquadDetailsUpdated);
			this.tabButton.Selected = true;
		}

		public override void HideView()
		{
			this.memberContainer.Visible = false;
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.SquadDetailsUpdated);
			this.tabButton.Selected = false;
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			if (warManager.MatchMakingPrepMode)
			{
				warManager.CancelEnteringMatchmaking();
			}
		}

		public override void RefreshView()
		{
			this.UpdateMembers();
			if (!Service.Get<SquadController>().WarManager.MatchMakingPrepMode)
			{
				this.groupStartWarBtns.Visible = false;
				this.memberPanel.SetPanelUnifiedAnchorBottomOffset(15);
				return;
			}
			this.groupStartWarBtns.Visible = true;
			this.memberPanel.SetPanelUnifiedAnchorBottomOffset(120);
			SquadController squadController = Service.Get<SquadController>();
			SquadWarManager warManager = squadController.WarManager;
			int warPartyCount = warManager.GetWarPartyCount();
			this.labelStartWarSelected.Text = this.lang.Get("WAR_MEMBERS_SELECTED", new object[]
			{
				warPartyCount,
				GameConstants.WAR_PARTICIPANT_COUNT
			});
			if (warPartyCount < GameConstants.WAR_PARTICIPANT_COUNT)
			{
				this.btnStartWarConfirm.Enabled = false;
				this.btnStartWarConfirm.VisuallyDisableButton();
				return;
			}
			this.btnStartWarConfirm.Enabled = true;
			this.btnStartWarConfirm.VisuallyEnableButton();
		}

		private void OnTabButtonSelected(UXCheckbox button, bool selected)
		{
			if (selected)
			{
				SquadController squadController = Service.Get<SquadController>();
				squadController.StateManager.SquadScreenState = SquadScreenState.Members;
				this.screen.RefreshViews();
			}
		}

		private void SetupMemberSortButton(UXCheckbox memberSortCheckbox, SquadMemberSortType type)
		{
			memberSortCheckbox.Selected = false;
			memberSortCheckbox.Tag = type;
			memberSortCheckbox.OnSelected = new UXCheckboxSelectedDelegate(this.OnMemberSortOptionClicked);
		}

		protected void OnMemberSortOptionClicked(UXCheckbox btn, bool selected)
		{
			if (selected)
			{
				btn.Selected = false;
				this.curSortType = (SquadMemberSortType)btn.Tag;
				this.HandleMemberSortOptionClicked();
				this.squadMemberGrid.RepositionItems();
				this.UpdateMembers();
				this.OpenCloseMemberSortList();
			}
		}

		private void HandleMemberSortOptionClicked()
		{
			switch (this.curSortType)
			{
			case SquadMemberSortType.Medals:
				this.memberSortLabel.Text = this.lang.Get("s_Medals", new object[0]);
				this.squadMemberGrid.SetSortComparisonCallback(new Comparison<UXElement>(this.SortByMedals));
				return;
			case SquadMemberSortType.Attacks:
				this.memberSortLabel.Text = this.lang.Get("s_AttacksWon", new object[0]);
				this.squadMemberGrid.SetSortComparisonCallback(new Comparison<UXElement>(this.SortByAttacks));
				return;
			case SquadMemberSortType.Defenses:
				this.memberSortLabel.Text = this.lang.Get("s_DefensesWon", new object[0]);
				this.squadMemberGrid.SetSortComparisonCallback(new Comparison<UXElement>(this.SortByDefenses));
				return;
			case SquadMemberSortType.Donated:
				this.memberSortLabel.Text = this.lang.Get("s_Donated", new object[0]);
				this.squadMemberGrid.SetSortComparisonCallback(new Comparison<UXElement>(this.SortByDonated));
				return;
			case SquadMemberSortType.Received:
				this.memberSortLabel.Text = this.lang.Get("s_Received", new object[0]);
				this.squadMemberGrid.SetSortComparisonCallback(new Comparison<UXElement>(this.SortByReceived));
				return;
			case SquadMemberSortType.Active:
				this.memberSortLabel.Text = this.lang.Get("s_LastActive", new object[0]);
				this.squadMemberGrid.SetSortComparisonCallback(new Comparison<UXElement>(this.SortByActive));
				return;
			default:
				return;
			}
		}

		public int SortByMedals(UXElement elementA, UXElement elementB)
		{
			SquadMember squadMember = (SquadMember)elementA.Tag;
			SquadMember squadMember2 = (SquadMember)elementB.Tag;
			return squadMember2.Score - squadMember.Score;
		}

		public int SortByAttacks(UXElement elementA, UXElement elementB)
		{
			SquadMember squadMember = (SquadMember)elementA.Tag;
			SquadMember squadMember2 = (SquadMember)elementB.Tag;
			return squadMember2.AttacksWon - squadMember.AttacksWon;
		}

		public int SortByDefenses(UXElement elementA, UXElement elementB)
		{
			SquadMember squadMember = (SquadMember)elementA.Tag;
			SquadMember squadMember2 = (SquadMember)elementB.Tag;
			return squadMember2.DefensesWon - squadMember.DefensesWon;
		}

		public int SortByDonated(UXElement elementA, UXElement elementB)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			SquadController squadController = Service.Get<SquadController>();
			SquadMember squadMember = (SquadMember)elementA.Tag;
			SquadMember squadMember2 = (SquadMember)elementB.Tag;
			int num = squadMember.TroopsDonated;
			int num2 = squadMember2.TroopsDonated;
			if (squadMember.MemberID == currentPlayer.PlayerId)
			{
				num += squadController.StateManager.NumTroopDonationsInSession;
			}
			if (squadMember2.MemberID == currentPlayer.PlayerId)
			{
				num2 += squadController.StateManager.NumTroopDonationsInSession;
			}
			return num2 - num;
		}

		public int SortByReceived(UXElement elementA, UXElement elementB)
		{
			SquadMember squadMember = (SquadMember)elementA.Tag;
			SquadMember squadMember2 = (SquadMember)elementB.Tag;
			return squadMember2.TroopsReceived - squadMember.TroopsReceived;
		}

		public int SortByActive(UXElement elementA, UXElement elementB)
		{
			SquadMember squadMember = (SquadMember)elementA.Tag;
			SquadMember squadMember2 = (SquadMember)elementB.Tag;
			return (int)(squadMember2.LastLoginTime - squadMember.LastLoginTime);
		}

		protected void OnMemberSortOpenClicked(UXButton btn)
		{
			this.OpenCloseMemberSortList();
		}

		private void OpenCloseMemberSortList()
		{
			this.memberSortBox.Visible = !this.memberSortBox.Visible;
		}

		private void RepositionFinished()
		{
			this.squadMemberGrid.ResetScrollViewPosition();
		}

		private void UpdateMembers()
		{
			SquadController squadController = Service.Get<SquadController>();
			Squad currentSquad = squadController.StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return;
			}
			bool flag = false;
			List<UXElement> elementList = this.squadMemberGrid.GetElementList();
			for (int i = 0; i < elementList.Count; i++)
			{
				SquadMember squadMember = elementList[i].Tag as SquadMember;
				if (!SquadUtils.IsPlayerInSquad(squadMember.MemberID, currentSquad))
				{
					UXElement uXElement = elementList[i];
					this.squadMemberGrid.RemoveItem(uXElement);
					this.screen.DestroyElement(uXElement);
					i--;
					flag = true;
				}
			}
			int j = 0;
			int count = currentSquad.MemberList.Count;
			while (j < count)
			{
				if (this.CheckAndCreateMemberEntry(currentSquad.MemberList[j]))
				{
					flag = true;
				}
				j++;
			}
			int k = 0;
			int count2 = currentSquad.MemberList.Count;
			while (k < count2)
			{
				this.UpdateSquadMember(currentSquad.MemberList[k]);
				k++;
			}
			if (flag)
			{
				this.squadMemberGrid.RepositionItems();
			}
		}

		private UXElement GetSquadMemberElement(SquadMember squadMember)
		{
			List<UXElement> elementList = this.squadMemberGrid.GetElementList();
			int i = 0;
			int count = elementList.Count;
			while (i < count)
			{
				SquadMember squadMember2 = elementList[i].Tag as SquadMember;
				if (squadMember2 != null && squadMember2.MemberID == squadMember.MemberID)
				{
					return elementList[i];
				}
				i++;
			}
			return null;
		}

		private bool CheckAndCreateMemberEntry(SquadMember squadMember)
		{
			if (this.GetSquadMemberElement(squadMember) == null)
			{
				UXElement uXElement = this.CreateSquadMemberElement(squadMember);
				return true;
			}
			return false;
		}

		private UXElement CreateSquadMemberElement(SquadMember squadMember)
		{
			string text = "member_" + squadMember.MemberID;
			UXButton uXButton = this.squadMemberGrid.CloneTemplateItem(text) as UXButton;
			this.squadMemberGrid.AddItem(uXButton, 0);
			uXButton.Tag = squadMember;
			uXButton.OnClicked = new UXButtonClickedDelegate(this.MemberItemClicked);
			SquadMemberElements squadMemberElements = new SquadMemberElements();
			SquadScreenMembersView.SquadMemberElementsSetup(this.squadMemberGrid, squadMemberElements, text);
			squadMemberElements.ButtonContainer.Visible = false;
			squadMemberElements.ButtonContainer.InitTweenComponent();
			return uXButton;
		}

		private void UpdateSquadMember(SquadMember squadMember)
		{
			UXElement squadMemberElement = this.GetSquadMemberElement(squadMember);
			string id = "member_" + squadMember.MemberID;
			SquadMemberElements squadMemberElements = new SquadMemberElements();
			SquadScreenMembersView.SquadMemberElementsSetup(this.squadMemberGrid, squadMemberElements, id);
			this.SetPromoteDemoteOnButton(squadMember, squadMemberElements.ButtonOne, squadMemberElements.ButtonOneLabel);
			squadMemberElements.ButtonOne.Tag = squadMember;
			if (squadMember.Role == SquadRole.Owner)
			{
				squadMemberElements.ButtonOne.Enabled = false;
			}
			else
			{
				this.SetupButtonBasedOnRole(squadMemberElements.ButtonOne, squadMember, true);
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			SquadController squadController = Service.Get<SquadController>();
			SquadWarManager warManager = squadController.WarManager;
			bool flag = squadMember.MemberID == currentPlayer.PlayerId;
			int num = 0;
			int num2 = 0;
			squadMemberElements.ButtonTwoLabel.Text = this.lang.Get("s_View", new object[0]);
			if (flag)
			{
				squadMemberElements.ButtonTwo.Enabled = false;
				num = squadController.StateManager.NumTroopDonationsInSession;
				num2 = squadController.StateManager.NumRepDonatedInSession;
			}
			else
			{
				squadMemberElements.ButtonTwo.OnClicked = new UXButtonClickedDelegate(this.OnViewClicked);
				squadMemberElements.ButtonTwo.Tag = squadMember;
			}
			squadMemberElements.ButtonThreeLabel.Text = this.lang.Get("s_Remove", new object[0]);
			squadMemberElements.ButtonThree.OnClicked = new UXButtonClickedDelegate(this.OnRemoveClicked);
			squadMemberElements.ButtonThree.Tag = squadMember;
			this.SetupButtonBasedOnRole(squadMemberElements.ButtonThree, squadMember, false);
			squadMemberElements.MemberInfoGroup.Visible = true;
			squadMemberElements.MemberNameLabel.Text = this.lang.Get("WAR_MEMBER_NAME", new object[]
			{
				squadMember.MemberName,
				squadMember.HQLevel
			});
			if (squadMemberElement != null)
			{
				squadMemberElements.MemberRankLabel.Text = (this.squadMemberGrid.GetSortedIndex(squadMemberElement) + 1).ToString();
			}
			squadMemberElements.MemberRoleLabel.Text = LangUtils.GetSquadRoleDisplayName(squadMember.Role);
			if (warManager.IsMemberInWarParty(squadMember.MemberID) || warManager.IsSquadMemberInWarOrMatchmaking(squadMember))
			{
				if (warManager.IsCurrentSquadMatchmaking())
				{
					UXLabel expr_20A = squadMemberElements.MemberRoleLabel;
					expr_20A.Text += this.lang.Get("WAR_IN_MATCHMAKING", new object[0]);
				}
				else if (warManager.WarExists())
				{
					UXLabel expr_241 = squadMemberElements.MemberRoleLabel;
					expr_241.Text += this.lang.Get("WAR_IN_WAR", new object[0]);
				}
			}
			squadMemberElements.MemberScoreLabel.Text = ((squadMember.MemberID == Service.Get<CurrentPlayer>().PlayerId) ? this.lang.ThousandsSeparated(Service.Get<CurrentPlayer>().PlayerMedals) : this.lang.ThousandsSeparated(squadMember.Score));
			squadMemberElements.TroopsDonatedLabel.Text = this.lang.Get("TROOPS_DONATED", new object[]
			{
				this.lang.ThousandsSeparated(squadMember.TroopsDonated + num)
			});
			squadMemberElements.TroopsReceivedLabel.Text = this.lang.Get("TROOPS_RECEIVED", new object[]
			{
				this.lang.ThousandsSeparated(squadMember.TroopsReceived)
			});
			squadMemberElements.RepInvestedLabel.Text = this.lang.Get("REPUTATION_INVESTED", new object[]
			{
				this.lang.ThousandsSeparated(squadMember.ReputationInvested + num2)
			});
			squadMemberElements.VictoriesLabel.Text = this.lang.Get("s_Victories", new object[0]);
			squadMemberElements.TroopsLabel.Text = this.lang.Get("s_Troops", new object[0]);
			squadMemberElements.AttacksWonLabel.Text = this.lang.Get("ATTACKS_WON", new object[]
			{
				this.lang.ThousandsSeparated(squadMember.AttacksWon)
			});
			squadMemberElements.DefensesWonLabel.Text = this.lang.Get("DEFENSES_WON", new object[]
			{
				this.lang.ThousandsSeparated(squadMember.DefensesWon)
			});
			if (squadMember.MemberID != currentPlayer.PlayerId)
			{
				squadMemberElements.LastLoginTimeLabel.Text = ChatTimeConversionUtils.GetFormattedAgeSinceLogin(squadMember.LastLoginTime, this.lang);
			}
			else
			{
				squadMemberElements.LastLoginTimeLabel.Visible = false;
			}
			if (squadMember.TournamentScore != null && squadMember.TournamentScore.Count > 0)
			{
				squadMemberElements.ConflictMedalsGroup.Visible = true;
				squadMemberElements.TournamentScoreLabel1.Visible = false;
				squadMemberElements.TournamentScoreSprite1.Visible = false;
				squadMemberElements.TournamentScoreLabel2.Visible = false;
				squadMemberElements.TournamentScoreSprite2.Visible = false;
				squadMemberElements.TournamentScoreLabel3.Visible = false;
				squadMemberElements.TournamentScoreSprite3.Visible = false;
				int num3 = 0;
				using (Dictionary<string, int>.Enumerator enumerator = squadMember.TournamentScore.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, int> current = enumerator.Current;
						if (num3 == 0)
						{
							this.UpdateTournamentScore(current, squadMemberElements.TournamentScoreLabel1, squadMemberElements.TournamentScoreSprite1);
						}
						else if (num3 == 1)
						{
							this.UpdateTournamentScore(current, squadMemberElements.TournamentScoreLabel2, squadMemberElements.TournamentScoreSprite2);
						}
						else if (num3 == 2)
						{
							this.UpdateTournamentScore(current, squadMemberElements.TournamentScoreLabel3, squadMemberElements.TournamentScoreSprite3);
						}
						num3++;
					}
					goto IL_543;
				}
			}
			squadMemberElements.ConflictMedalsGroup.Visible = false;
			IL_543:
			PlanetVO planetVO = null;
			if (!string.IsNullOrEmpty(squadMember.Planet))
			{
				planetVO = Service.Get<IDataController>().GetOptional<PlanetVO>(squadMember.Planet);
			}
			if (planetVO == null)
			{
				planetVO = Service.Get<IDataController>().GetOptional<PlanetVO>("planet1");
			}
			if (planetVO != null && squadMemberElements.PlanetBackgroundTexture != null)
			{
				squadMemberElements.PlanetBackgroundTexture.LoadTexture(planetVO.LeaderboardTileTexture, null);
			}
			if (Service.Get<SquadController>().WarManager.MatchMakingPrepMode)
			{
				bool flag2 = squadMember.HQLevel >= GameConstants.WAR_PARTICIPANT_MIN_LEVEL;
				if (flag2)
				{
					squadMemberElements.SpriteMemberSelect.Tag = true;
					squadMemberElements.SpriteMemberSelect.Visible = warManager.IsMemberInWarParty(squadMember.MemberID);
				}
				else
				{
					squadMemberElements.SpriteMemberSelect.Tag = false;
					squadMemberElements.SpriteMemberSelect.Visible = false;
				}
				squadMemberElements.GroupMemberSelect.Visible = true;
				squadMemberElements.SpriteMoreOptions.Visible = false;
			}
			else
			{
				squadMemberElements.GroupMemberSelect.Visible = false;
				squadMemberElements.SpriteMemberSelect.Visible = false;
				squadMemberElements.SpriteMoreOptions.Visible = true;
			}
			squadMemberElements.SpriteWarIcon.Visible = (squadMember.WarParty != 0);
			squadMemberElements.ButtonContainer.ResetPlayTweenTarget();
		}

		private void SetupButtonBasedOnRole(UXButton btn, SquadMember member, bool ownerOnly)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (member != null && (currentPlayer.PlayerId == member.MemberID || member.Role == SquadRole.Owner))
			{
				btn.Enabled = false;
				return;
			}
			SquadStateManager stateManager = Service.Get<SquadController>().StateManager;
			if (stateManager.Role == SquadRole.Owner)
			{
				btn.Enabled = true;
				return;
			}
			if (!ownerOnly && stateManager.Role == SquadRole.Officer && member.Role == SquadRole.Member)
			{
				btn.Enabled = true;
				return;
			}
			btn.Enabled = false;
		}

		private void UpdateTournamentScore(KeyValuePair<string, int> pair, UXLabel scoreLabel, UXSprite iconSprite)
		{
			scoreLabel.Visible = true;
			iconSprite.Visible = true;
			scoreLabel.Text = this.lang.ThousandsSeparated(pair.get_Value());
			iconSprite.SpriteName = GameUtils.GetTournamentPointIconName(pair.get_Key());
		}

		public static void SquadMemberElementsSetup(UXGrid grid, SquadMemberElements elements, string id)
		{
			elements.ButtonContainer = grid.GetSubElement<UXElement>(id, "ButtonContainer");
			elements.ButtonOne = grid.GetSubElement<UXButton>(id, "Btn1");
			elements.ButtonOneLabel = grid.GetSubElement<UXLabel>(id, "LabelBtn1");
			elements.ButtonTwo = grid.GetSubElement<UXButton>(id, "Btn2");
			elements.ButtonTwoLabel = grid.GetSubElement<UXLabel>(id, "LabelBtn2");
			elements.ButtonThree = grid.GetSubElement<UXButton>(id, "Btn3");
			elements.ButtonThreeLabel = grid.GetSubElement<UXLabel>(id, "LabelBtn3");
			elements.MemberInfoGroup = grid.GetSubElement<UXElement>(id, "MemberInfo");
			elements.MemberNameLabel = grid.GetSubElement<UXLabel>(id, "LabelMemberName");
			elements.MemberRankLabel = grid.GetSubElement<UXLabel>(id, "LabelMemberRank");
			elements.MemberRoleLabel = grid.GetSubElement<UXLabel>(id, "LabelMemberRole");
			elements.MemberScoreLabel = grid.GetSubElement<UXLabel>(id, "LabelMemberScore");
			elements.TroopsDonatedLabel = grid.GetSubElement<UXLabel>(id, "LabelTroopsDonated");
			elements.TroopsReceivedLabel = grid.GetSubElement<UXLabel>(id, "LabelTroopsReceived");
			elements.RepInvestedLabel = grid.GetSubElement<UXLabel>(id, "LabelMemberRepAmt");
			elements.VictoriesLabel = grid.GetSubElement<UXLabel>(id, "LabelVictories");
			elements.TroopsLabel = grid.GetSubElement<UXLabel>(id, "LabelTroops");
			elements.AttacksWonLabel = grid.GetSubElement<UXLabel>(id, "LabelAttacksWon");
			elements.DefensesWonLabel = grid.GetSubElement<UXLabel>(id, "LabelDefensesWon");
			elements.LastLoginTimeLabel = grid.GetSubElement<UXLabel>(id, "LabelMemberLastLogin");
			elements.ConflictMedalsGroup = grid.GetSubElement<UXElement>(id, "MedalInfo");
			elements.TournamentScoreLabel1 = grid.GetSubElement<UXLabel>(id, "LabelMemberTournamentScore01");
			elements.TournamentScoreSprite1 = grid.GetSubElement<UXSprite>(id, "SpriteMemberTournamentMedalIcon01");
			elements.TournamentScoreLabel2 = grid.GetSubElement<UXLabel>(id, "LabelMemberTournamentScore02");
			elements.TournamentScoreSprite2 = grid.GetSubElement<UXSprite>(id, "SpriteMemberTournamentMedalIcon02");
			elements.TournamentScoreLabel3 = grid.GetSubElement<UXLabel>(id, "LabelMemberTournamentScore03");
			elements.TournamentScoreSprite3 = grid.GetSubElement<UXSprite>(id, "SpriteMemberTournamentMedalIcon03");
			elements.PlanetBackgroundTexture = grid.GetSubElement<UXTexture>(id, "TexturePlanetBg");
			elements.GroupMemberSelect = grid.GetSubElement<UXElement>(id, "BtnSelectMember");
			elements.SpriteMemberSelect = grid.GetSubElement<UXSprite>(id, "SpriteCheckBtnSelectMember");
			elements.SpriteMoreOptions = grid.GetSubElement<UXSprite>(id, "SpriteMoreOptions");
			elements.SpriteWarIcon = grid.GetSubElement<UXSprite>(id, "SpriteWarIcon");
		}

		private void SetPromoteDemoteOnButton(SquadMember squadMember, UXButton btn, UXLabel label)
		{
			if (squadMember.Role == SquadRole.Officer)
			{
				label.Text = this.lang.Get("s_Demote", new object[0]);
				btn.OnClicked = new UXButtonClickedDelegate(this.OnDemoteClicked);
			}
			else
			{
				label.Text = this.lang.Get("s_Promote", new object[0]);
				btn.OnClicked = new UXButtonClickedDelegate(this.OnPromoteClicked);
			}
			btn.Enabled = true;
		}

		private void MemberItemClicked(UXButton btn)
		{
			SquadController squadController = Service.Get<SquadController>();
			Service.Get<EventManager>().SendEvent(EventId.SquadMore, null);
			SquadMember squadMember = btn.Tag as SquadMember;
			string id = "member_" + squadMember.MemberID;
			SquadMemberElements squadMemberElements = new SquadMemberElements();
			SquadScreenMembersView.SquadMemberElementsSetup(this.squadMemberGrid, squadMemberElements, id);
			if (Service.Get<SquadController>().WarManager.MatchMakingPrepMode)
			{
				if (squadMemberElements.SpriteMemberSelect.Visible && squadController.WarManager.WarPartyRemove(squadMember.MemberID))
				{
					squadMemberElements.SpriteMemberSelect.Visible = false;
				}
				else if (!squadMemberElements.SpriteMemberSelect.Visible && squadController.WarManager.WarPartyAdd(squadMember))
				{
					squadMemberElements.SpriteMemberSelect.Visible = true;
				}
				else if (!(bool)squadMemberElements.SpriteMemberSelect.Tag)
				{
					string message = this.lang.Get("WAR_START_HQ_INELIGIBLE", new object[]
					{
						GameConstants.WAR_PARTICIPANT_MIN_LEVEL
					});
					AlertScreen.ShowModal(false, null, message, null, null, true);
				}
				this.screen.RefreshViews();
				return;
			}
			squadMemberElements.ButtonContainer.Visible = true;
			squadMemberElements.ButtonContainer.PlayTween(true);
		}

		private void OnPromoteClicked(UXButton button)
		{
			SquadMember squadMember = button.Tag as SquadMember;
			Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
			string message = this.lang.Get("PROMOTE_SQUAD_MEMBER_ALERT", new object[]
			{
				squadMember.MemberName,
				this.lang.Get("SQUAD_OFFICER", new object[0])
			});
			AlertScreen.ShowModal(false, null, message, new OnScreenModalResult(this.OnAlertPromoteMemberResult), squadMember, true);
		}

		private void OnAlertPromoteMemberResult(object result, object cookie)
		{
			if (result == null)
			{
				return;
			}
			SquadMember squadMember = cookie as SquadMember;
			SquadMsg message = SquadMsgUtils.CreatePromoteMemberMessage(squadMember.MemberID, new SquadController.ActionCallback(this.OnMemberChangeComplete), squadMember);
			SquadController squadController = Service.Get<SquadController>();
			squadController.TakeAction(message);
			ProcessingScreen.Show();
		}

		private void OnDemoteClicked(UXButton button)
		{
			SquadMember squadMember = button.Tag as SquadMember;
			Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
			string message = this.lang.Get("DEMOTE_SQUAD_MEMBER_ALERT", new object[]
			{
				squadMember.MemberName,
				this.lang.Get("SQUAD_MEMBER", new object[0])
			});
			AlertScreen.ShowModal(false, null, message, new OnScreenModalResult(this.OnAlertDemoteMemberResult), squadMember, true);
		}

		private void OnAlertDemoteMemberResult(object result, object cookie)
		{
			if (result == null)
			{
				return;
			}
			SquadMember squadMember = cookie as SquadMember;
			SquadMsg message = SquadMsgUtils.CreateDemoteMemberMessage(squadMember.MemberID, new SquadController.ActionCallback(this.OnMemberChangeComplete), squadMember);
			SquadController squadController = Service.Get<SquadController>();
			squadController.TakeAction(message);
			ProcessingScreen.Show();
		}

		private void OnViewClicked(UXButton button)
		{
			GameUtils.ExitEditState();
			Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
			SquadUtils.ForceCloseSquadWarScreen();
			Service.Get<UXController>().HUD.DestroySquadScreen();
			SquadMember squadMember = button.Tag as SquadMember;
			Service.Get<NeighborVisitManager>().VisitNeighbor(squadMember.MemberID);
			PlayerVisitTag cookie = new PlayerVisitTag(true, false, "SQUAD_MEMBERS", squadMember.MemberID);
			Service.Get<EventManager>().SendEvent(EventId.VisitPlayer, cookie);
		}

		private void OnRemoveClicked(UXButton button)
		{
			SquadMember squadMember = button.Tag as SquadMember;
			Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
			SquadController squadController = Service.Get<SquadController>();
			if (squadController.WarManager.IsMemberInWarParty(squadMember.MemberID) || squadController.WarManager.IsSquadMemberInWarOrMatchmaking(squadMember))
			{
				AlertScreen.ShowModal(false, null, this.lang.Get("CANT_REMOVE_MEMBER_IN_WAR", new object[0]), null, null, true);
				return;
			}
			string message = this.lang.Get("REMOVE_SQUAD_MEMBER_ALERT", new object[]
			{
				squadMember.MemberName
			});
			AlertScreen.ShowModal(false, null, message, new OnScreenModalResult(this.OnAlertMemberRemoveResult), squadMember, true);
		}

		private void OnAlertMemberRemoveResult(object result, object cookie)
		{
			if (result == null)
			{
				return;
			}
			SquadMember squadMember = cookie as SquadMember;
			SquadMsg message = SquadMsgUtils.CreateRemoveMemberMessage(squadMember.MemberID, new SquadController.ActionCallback(this.OnMemberChangeComplete), squadMember);
			SquadController squadController = Service.Get<SquadController>();
			squadController.TakeAction(message);
			ProcessingScreen.Show();
		}

		private void OnMemberChangeComplete(bool success, object cookie)
		{
			ProcessingScreen.Hide();
			if (success)
			{
				this.UpdateMembers();
			}
		}

		private void OnStartWarConfirm(UXButton button)
		{
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			warManager.StartMatchMaking(this.allowSameFaction);
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is WarBoardState)
			{
				this.screen.InstantClose(false, null);
				SquadWarScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<SquadWarScreen>();
				if (highestLevelScreen != null)
				{
					highestLevelScreen.CloseSquadWarScreen(null);
					return;
				}
			}
			else
			{
				this.screen.AnimateClosed(false, null);
			}
		}

		private void OnCancelStartWar(UXButton button)
		{
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			warManager.CancelEnteringMatchmaking();
			this.screen.RefreshViews();
		}

		private void OnSameFaction(UXButton button)
		{
			this.allowSameFaction = !this.allowSameFaction;
			this.spriteCheckSameFaction.Visible = this.allowSameFaction;
		}

		private void OnSameFactionInfo(UXButton button)
		{
			SquadController squadController = Service.Get<SquadController>();
			Squad currentSquad = squadController.StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return;
			}
			bool flag = currentSquad.Faction == FactionType.Empire;
			string sameFactionString;
			if (flag)
			{
				sameFactionString = Service.Get<Lang>().Get("WAR_SAME_FACTION_DESC_E", new object[0]);
			}
			else
			{
				sameFactionString = Service.Get<Lang>().Get("WAR_SAME_FACTION_DESC_R", new object[0]);
			}
			Service.Get<UXController>().MiscElementsManager.ShowSameFactionMatchMakingTooltip(button, sameFactionString);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SquadDetailsUpdated)
			{
				this.RefreshView();
			}
			return EatResponse.NotEaten;
		}

		public override void OnDestroyElement()
		{
			if (this.squadMemberGrid != null)
			{
				this.squadMemberGrid.Clear();
				this.squadMemberGrid.RepositionCallback = null;
				this.squadMemberGrid = null;
			}
		}

		public override bool IsVisible()
		{
			return this.memberContainer.Visible;
		}

		protected internal SquadScreenMembersView(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).CheckAndCreateMemberEntry((SquadMember)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).CreateSquadMemberElement((SquadMember)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).GetSquadMemberElement((SquadMember)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).HandleMemberSortOptionClicked();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).HideView();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).InitMemberList();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).IsVisible());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).MemberItemClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnAlertDemoteMemberResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnAlertMemberRemoveResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnAlertPromoteMemberResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnCancelStartWar((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnDemoteClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnMemberChangeComplete(*(sbyte*)args != 0, GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnMemberSortOpenClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnMemberSortOptionClicked((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnPromoteClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnRemoveClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnSameFaction((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnSameFactionInfo((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnStartWarConfirm((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnTabButtonSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OnViewClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).OpenCloseMemberSortList();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).RepositionFinished();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).SetPromoteDemoteOnButton((SquadMember)GCHandledObjects.GCHandleToObject(*args), (UXButton)GCHandledObjects.GCHandleToObject(args[1]), (UXLabel)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).SetupButtonBasedOnRole((UXButton)GCHandledObjects.GCHandleToObject(*args), (SquadMember)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).SetupMemberSortButton((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), (SquadMemberSortType)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).ShowView();
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).SortByActive((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).SortByAttacks((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).SortByDefenses((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).SortByDonated((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).SortByMedals((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).SortByReceived((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			SquadScreenMembersView.SquadMemberElementsSetup((UXGrid)GCHandledObjects.GCHandleToObject(*args), (SquadMemberElements)GCHandledObjects.GCHandleToObject(args[1]), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).UpdateMembers();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).UpdateSquadMember((SquadMember)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((SquadScreenMembersView)GCHandledObjects.GCHandleToObject(instance)).UpdateTournamentScore((KeyValuePair<string, int>)GCHandledObjects.GCHandleToObject(*args), (UXLabel)GCHandledObjects.GCHandleToObject(args[1]), (UXSprite)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}
	}
}
