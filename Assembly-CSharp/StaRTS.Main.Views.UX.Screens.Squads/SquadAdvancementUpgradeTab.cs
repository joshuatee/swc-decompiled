using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadAdvancementUpgradeTab : SquadAdvancementBaseTab, IEventObserver
	{
		private const int UPGRADE_SORT_ORDER_LOCK_OFFSET = 10000;

		private const int UPGRADE_SORT_ORDER_MAX_TIER_OFFSET = 20000;

		private const int UPGRADE_SORT_ORDER_INVISIBLE_OFFSET = 40000;

		private const string ROOT_UPGRADE_VIEW = "UpgradeMainGroupPerks";

		private const string PERK_UPGRADE_TITLE = "PERK_UPGRADE_TITLE";

		private const string PERK_UPGRADE_MAX_TIER = "PERK_UPGRADE_MAX_TIER";

		private const string PERK_ACTIVE_UPGRADE_CARD_LVL_REQ = "PERK_ACTIVATE_UPGRADE_CARD_LVL_REQ";

		private const string PERK_UPGRADE_CARD_UNLOCK_TIER = "PERK_UPGRADE_CARD_UNLOCK_TIER";

		private const string UPGRADE_TAB_TITLE = "LabelTitleUpgradePerks";

		private const string UPRADE_VIEW_FILTER_GRID = "GridUpFilterPerks";

		private const string UPGRADE_VIEW_FILTER_TEMPLATE = "TemplateUpFilterPerks";

		private const string UPGRADE_VIEW_FILTER_TEMPLATE_BTN = "BtnUpFilterPerks";

		private const string UPGRADE_VIEW_FILTER_TEMPLATE_LABEL = "LabelUpFilterPerks";

		private const string UPGRADE_VIEW_PERK_GRID = "GridUpPerks";

		private const string UPGRADE_VIEW_PERK_GRID_TEMPLATE = "TemplateUpCardPerks";

		private const string UPGRADE_VIEW_PERK_GRID_BUTTON = "BtnUpCardPerks";

		private const string UPGRADE_VIEW_PERK_GRID_LOCKED_GROUP = "LockedGroupUpCardPerks";

		private const string UPGRADE_VIEW_PERK_GRID_LOCKED_LVL_LABEL = "LabelSquadLvlLockedUpCardPerks";

		private const string UPGRADE_VIEW_PERK_GRID_LOCKED_REQ_LABEL = "LabelSquadLvlLockedUpCardPerks";

		private const string UPGRADE_VIEW_PERK_GRID_COST_LABEL = "LabelCostUpPerk";

		private const string UPGRADE_VIEW_PERK_GRID_COST_ICON = "CostUpPerkReputationIcon";

		private const string UPGRADE_VIEW_PERK_PROGRESS_LABEL = "LabelpBarUpInvestAmtPerks";

		private const string UPGRADE_VIEW_PERK_PROGRESS_BAR = "pBarUpInvestAmtPerks";

		private const string UPGRADE_VIEW_PERK_TIER_LABEL = "LabelPerkLvlUpCardPerks";

		private const string UPGRADE_VIEW_PERK_TITLE = "LabelPerkTitleUpCardPerks";

		private const string UPGRADE_VIEW_PERK_IMAGE = "TexturePerkArtUpCardPerks";

		private const string UPGRADE_VIEW_PERK_MAX_TIER_GROUP = "MaxLvlGroupUpCardPerks";

		private const string UPGRADE_VIEW_PERK_MAX_TIER_LABEL = "LabelMaxLvlUpCardPerks";

		private UXLabel tabTitle;

		private UXGrid perkGrid;

		private SquadScreenConfirmPerkUpgradeView confirmInfoView;

		private bool showPerkUpgradeAfterCelebration;

		public SquadAdvancementUpgradeTab(SquadSlidingScreen screen, string tabLabelName, string tabLabelString) : base(screen, "UpgradeMainGroupPerks", tabLabelName, tabLabelString)
		{
			this.InitUI();
		}

		private void InitUI()
		{
			this.tabTitle = this.screen.GetElement<UXLabel>("LabelTitleUpgradePerks");
			this.perkGrid = this.screen.GetElement<UXGrid>("GridUpPerks");
			base.InitFilterGrid("GridUpFilterPerks", "TemplateUpFilterPerks", "BtnUpFilterPerks", "LabelUpFilterPerks", this.perkGrid);
			this.InitViewLabels();
			this.InitPerkGrid();
		}

		private void InitViewLabels()
		{
			Lang lang = Service.Get<Lang>();
			this.tabTitle.Text = lang.Get("PERK_UPGRADE_TITLE", new object[0]);
		}

		public override void OnDestroyElement()
		{
			this.showPerkUpgradeAfterCelebration = false;
			if (this.perkGrid != null)
			{
				this.perkGrid.Clear();
				this.perkGrid = null;
			}
			this.CleanUpConfirmInfoView();
			this.UnregisterEvents();
			base.OnDestroyElement();
		}

		protected override void RegisterEvents()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.SquadLeveledUp);
			eventManager.RegisterObserver(this, EventId.PerkInvestment);
			eventManager.RegisterObserver(this, EventId.SquadPerkUpdated);
			eventManager.RegisterObserver(this, EventId.PerkCelebClosed);
			base.RegisterEvents();
		}

		protected override void UnregisterEvents()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.SquadLeveledUp);
			eventManager.UnregisterObserver(this, EventId.PerkInvestment);
			eventManager.UnregisterObserver(this, EventId.SquadPerkUpdated);
			eventManager.UnregisterObserver(this, EventId.PerkCelebClosed);
			base.UnregisterEvents();
		}

		protected override void OnShow()
		{
			this.showPerkUpgradeAfterCelebration = false;
			this.CleanUpConfirmInfoView();
			this.RegisterEvents();
			this.RefreshPerkStates();
		}

		protected override void OnHide()
		{
			this.lastGridPosition = 0f;
			this.showPerkUpgradeAfterCelebration = false;
			this.CleanUpConfirmInfoView();
			this.UnregisterEvents();
		}

		public void CleanUpConfirmInfoView()
		{
			this.openedModalOnTop = false;
			if (this.confirmInfoView != null)
			{
				this.confirmInfoView.HideAndCleanUp();
				this.confirmInfoView = null;
			}
		}

		private void InitPerkGrid()
		{
			this.perkGrid.SetTemplateItem("TemplateUpCardPerks");
		}

		public override void RefreshPerkStates()
		{
			Lang lang = Service.Get<Lang>();
			this.perkBadgeMap.Clear();
			List<UXElement> list = new List<UXElement>();
			PerkManager perkManager = Service.Get<PerkManager>();
			PerkViewController perkViewController = Service.Get<PerkViewController>();
			IDataController dataController = Service.Get<IDataController>();
			Squad squad = Service.Get<CurrentPlayer>().Squad;
			if (squad == null)
			{
				return;
			}
			Dictionary<string, string> available = squad.Perks.Available;
			Dictionary<string, int> inProgress = squad.Perks.InProgress;
			foreach (PerkVO current in dataController.GetAll<PerkVO>())
			{
				string uid = current.Uid;
				bool flag = perkManager.IsPerkMaxTier(current) && available.ContainsValue(uid);
				bool flag2 = perkManager.IsPerkLevelLocked(current, squad.Level);
				if (perkManager.HasPrerequistesUnlocked(current, available) && (flag || !available.ContainsValue(uid)))
				{
					string perkGroup = current.PerkGroup;
					string text = "PerkItem_" + perkGroup;
					UXElement uXElement = base.FetchPerkGridItem(this.perkGrid, text);
					uXElement.Tag = current;
					UXElement subElement = this.perkGrid.GetSubElement<UXElement>(text, "LockedGroupUpCardPerks");
					UXElement subElement2 = this.perkGrid.GetSubElement<UXElement>(text, "MaxLvlGroupUpCardPerks");
					UXButton subElement3 = this.perkGrid.GetSubElement<UXButton>(text, "BtnUpCardPerks");
					subElement3.Tag = current;
					subElement3.OnClicked = new UXButtonClickedDelegate(this.OnPerkClicked);
					int reputationCost = current.ReputationCost;
					int squadLevelUnlock = current.SquadLevelUnlock;
					int perkInvestedProgress = perkManager.GetPerkInvestedProgress(current, inProgress);
					string text2 = reputationCost.ToString();
					UXLabel subElement4 = this.perkGrid.GetSubElement<UXLabel>(text, "LabelPerkTitleUpCardPerks");
					subElement4.Text = perkViewController.GetPerkNameForGroup(current.PerkGroup);
					UXLabel subElement5 = this.perkGrid.GetSubElement<UXLabel>(text, "LabelCostUpPerk");
					subElement5.Text = text2;
					UXSprite subElement6 = this.perkGrid.GetSubElement<UXSprite>(text, "CostUpPerkReputationIcon");
					string text3 = perkInvestedProgress.ToString();
					UXLabel subElement7 = this.perkGrid.GetSubElement<UXLabel>(text, "LabelpBarUpInvestAmtPerks");
					subElement7.Text = text3 + "/" + text2;
					UXSlider subElement8 = this.perkGrid.GetSubElement<UXSlider>(text, "pBarUpInvestAmtPerks");
					subElement8.Value = (float)perkInvestedProgress / (float)reputationCost;
					UXLabel subElement9 = this.perkGrid.GetSubElement<UXLabel>(text, "LabelPerkLvlUpCardPerks");
					string text4 = string.Empty;
					if (current.PerkTier > 1)
					{
						text4 = StringUtils.GetRomanNumeral(current.PerkTier - 1);
					}
					else
					{
						text4 = lang.Get("PERK_UPGRADE_CARD_UNLOCK_TIER", new object[0]);
					}
					subElement9.Text = text4;
					UXTexture subElement10 = this.perkGrid.GetSubElement<UXTexture>(text, "TexturePerkArtUpCardPerks");
					perkViewController.SetPerkImage(subElement10, current);
					bool visible = !flag2 && !flag;
					subElement8.Visible = visible;
					subElement7.Visible = visible;
					subElement5.Visible = visible;
					subElement6.Visible = visible;
					subElement9.Visible = visible;
					subElement2.Visible = flag;
					subElement.Visible = (flag2 && !flag);
					if (flag2)
					{
						UXLabel subElement11 = this.perkGrid.GetSubElement<UXLabel>(text, "LabelSquadLvlLockedUpCardPerks");
						subElement11.Text = current.SquadLevelUnlock.ToString();
						UXLabel subElement12 = this.perkGrid.GetSubElement<UXLabel>(text, "LabelSquadLvlLockedUpCardPerks");
						subElement12.Text = lang.Get("PERK_ACTIVATE_UPGRADE_CARD_LVL_REQ", new object[]
						{
							squadLevelUnlock
						});
					}
					else if (flag)
					{
						UXLabel subElement13 = this.perkGrid.GetSubElement<UXLabel>(text, "LabelMaxLvlUpCardPerks");
						subElement13.Text = lang.Get("PERK_UPGRADE_MAX_TIER", new object[0]);
					}
					base.SetupPerkBadge(current, text, "UpCardPerks");
					list.Add(uXElement);
				}
			}
			list.Sort(new Comparison<UXElement>(this.SortUpgradeList));
			UXUtils.SortListForTwoRowGrids(list, this.perkGrid);
			int count = list.Count;
			this.perkGrid.ClearWithoutDestroy();
			for (int i = 0; i < count; i++)
			{
				this.perkGrid.AddItem(list[i], i);
			}
			this.perkGrid.RepositionItemsFrameDelayed(new UXDragDelegate(base.OnRepositionComplete));
			base.RefreshPerkStates();
		}

		private int SortUpgradeList(UXElement a, UXElement b)
		{
			PerkManager perkManager = Service.Get<PerkManager>();
			Squad squad = Service.Get<CurrentPlayer>().Squad;
			Dictionary<string, string> available = squad.Perks.Available;
			int level = squad.Level;
			PerkVO perkVO = (PerkVO)a.Tag;
			PerkVO perkVO2 = (PerkVO)b.Tag;
			int num = perkVO.SortOrder;
			int num2 = perkVO2.SortOrder;
			if (perkManager.IsPerkLevelLocked(perkVO, level) && perkManager.IsPerkLevelLocked(perkVO2, level))
			{
				num = perkVO.SquadLevelUnlock;
				num2 = perkVO2.SquadLevelUnlock;
			}
			if (perkManager.IsPerkMaxTier(perkVO) && available.ContainsValue(perkVO.Uid))
			{
				num += 20000;
			}
			else if (perkManager.IsPerkLevelLocked(perkVO, level))
			{
				num += 10000;
			}
			if (!a.Visible)
			{
				num += 40000;
			}
			if (perkManager.IsPerkMaxTier(perkVO2) && available.ContainsValue(perkVO2.Uid))
			{
				num2 += 20000;
			}
			else if (perkManager.IsPerkLevelLocked(perkVO2, level))
			{
				num2 += 10000;
			}
			if (!b.Visible)
			{
				num2 += 40000;
			}
			return num - num2;
		}

		private void OnPerkClicked(UXButton btn)
		{
			if (base.ShouldBlockInput())
			{
				return;
			}
			PerkViewController perkViewController = Service.Get<PerkViewController>();
			PerkVO perkVO = (PerkVO)btn.Tag;
			string perkGroup = perkVO.PerkGroup;
			if (perkViewController.IsPerkGroupBadged(perkGroup))
			{
				perkViewController.RemovePerkGroupFromBadgeList(perkGroup);
				this.screen.UpdateBadges();
			}
			this.ShowConfirmUpgradeView(perkVO);
			Service.Get<EventManager>().SendEvent(EventId.PerkSelected, null);
		}

		private void ShowConfirmUpgradeView(PerkVO perkData)
		{
			this.showPerkUpgradeAfterCelebration = false;
			PerkUpgradeConfirmState state = PerkUpgradeConfirmState.Unlocked;
			PerkManager perkManager = Service.Get<PerkManager>();
			Squad squad = Service.Get<CurrentPlayer>().Squad;
			if (squad == null)
			{
				return;
			}
			Dictionary<string, string> available = squad.Perks.Available;
			int level = squad.Level;
			if (perkManager.IsPerkLevelLocked(perkData, level))
			{
				state = PerkUpgradeConfirmState.Locked;
			}
			else if (perkManager.IsPerkMaxTier(perkData) && available.ContainsValue(perkData.Uid))
			{
				state = PerkUpgradeConfirmState.MaxTier;
			}
			this.lastGridPosition = this.perkGrid.GetCurrentScrollPosition(true);
			this.baseView.Visible = false;
			this.confirmInfoView = new SquadScreenConfirmPerkUpgradeView(perkData, this.screen, state, new Action(this.OnConfirmClosed));
			this.confirmInfoView.Show();
		}

		private void OnConfirmClosed()
		{
			this.CleanUpConfirmInfoView();
			this.baseView.Visible = true;
			this.RefreshPerkStates();
		}

		protected override void FilterGridBasedOnId(string filterId)
		{
			base.FilterGridBasedOnId(filterId);
			List<UXElement> list = new List<UXElement>(this.perkGrid.GetElementList());
			this.perkGrid.ClearWithoutDestroy();
			list.Sort(new Comparison<UXElement>(this.SortUpgradeList));
			UXUtils.SortListForTwoRowGrids(list, this.perkGrid);
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				UXElement uXElement = list[i];
				PerkVO perkVO = uXElement.Tag as PerkVO;
				string rootName = "PerkItem_" + perkVO.PerkGroup;
				uXElement.SetRootName(rootName);
				this.perkGrid.AddItem(uXElement, i);
			}
			List<UXElement> list2 = this.filterMap[filterId];
			this.gridToFilter.MaxItemsPerLine = (list2.Count + 1) / 2;
			this.perkGrid.RepositionItemsFrameDelayed(new UXDragDelegate(base.OnRepositionComplete));
			list.Clear();
		}

		public override bool ShouldBlockTabChanges()
		{
			return this.openedModalOnTop;
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			switch (id)
			{
			case EventId.SquadPerkUpdated:
			case EventId.SquadLeveledUp:
			case EventId.PerkInvestment:
				this.RefreshPerkStates();
				if (this.confirmInfoView != null)
				{
					this.confirmInfoView.RefreshProgressFromServer();
				}
				break;
			case EventId.PerkUnlocked:
			case EventId.PerkUpgraded:
				if (this.confirmInfoView != null && this.confirmInfoView.IsVisible())
				{
					PerkViewController perkViewController = Service.Get<PerkViewController>();
					PerkVO perkVO = (PerkVO)cookie;
					string perkGroup = perkVO.PerkGroup;
					if (perkViewController.IsPerkGroupBadged(perkGroup))
					{
						PerkVO perkDataShown = this.confirmInfoView.GetPerkDataShown();
						this.showPerkUpgradeAfterCelebration = perkGroup.Equals(perkDataShown.PerkGroup);
						this.CleanUpConfirmInfoView();
					}
				}
				break;
			case EventId.PerkCelebClosed:
			{
				this.openedModalOnTop = false;
				this.baseView.Visible = true;
				this.RefreshPerkStates();
				Squad squad = Service.Get<CurrentPlayer>().Squad;
				if (squad != null && cookie != null && this.showPerkUpgradeAfterCelebration)
				{
					PerkVO perkVO2 = (PerkVO)cookie;
					PerkManager perkManager = Service.Get<PerkManager>();
					Dictionary<string, string> available = squad.Perks.Available;
					int num = 1;
					if (perkManager.IsPerkMaxTier(perkVO2) && available.ContainsValue(perkVO2.Uid))
					{
						num = 0;
					}
					string perkGroup2 = perkVO2.PerkGroup;
					int perkTier = perkVO2.PerkTier + num;
					PerkVO perkByGroupAndTier = GameUtils.GetPerkByGroupAndTier(perkGroup2, perkTier);
					this.ShowConfirmUpgradeView(perkByGroupAndTier);
				}
				this.showPerkUpgradeAfterCelebration = false;
				break;
			}
			}
			return base.OnEvent(id, cookie);
		}

		protected internal SquadAdvancementUpgradeTab(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).CleanUpConfirmInfoView();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).FilterGridBasedOnId(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).InitPerkGrid();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).InitUI();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).InitViewLabels();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).OnConfirmClosed();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).OnHide();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).OnPerkClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).OnShow();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).RefreshPerkStates();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).RegisterEvents();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).ShouldBlockTabChanges());
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).ShowConfirmUpgradeView((PerkVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).SortUpgradeList((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SquadAdvancementUpgradeTab)GCHandledObjects.GCHandleToObject(instance)).UnregisterEvents();
			return -1L;
		}
	}
}
