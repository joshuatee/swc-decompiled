using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Perks;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadScreenActivationInfoView : SquadScreenBasePerkInfoView, IViewClockTimeObserver
	{
		private const string SHOW_ACT = "ShowActivation";

		private static readonly string[] singleCostElementNames = new string[]
		{
			"CostModalOnePerks"
		};

		private static readonly string[] dualCostElementNames = new string[]
		{
			"CostModalTwoTopPerks",
			"CostModalTwoBotPerks"
		};

		private const string LANG_PERK_ACTIVATE_POPUP_TITLE = "PERK_ACTIVATE_POPUP_TITLE";

		private const string LANG_PERK_ACTIVATE_POPUP_TIMER = "PERK_ACTIVATE_POPUP_TIMER";

		private const string LANG_PERK_ACTIVATE_NO_SLOT_AVAILABLE = "PERK_ACTIVATE_NO_SLOT_AVAILABLE";

		private const string LANG_PERK_UPGRADE_LVL_REQ = "PERK_UPGRADE_LVL_REQ";

		private const string LANG_PERK_ACTIVATE_POPUP_LVL_REQ_SQUAD = "PERK_UPGRADE_POPUP_LVL_REQ";

		private const string LANG_PERK_ACTIVATE_POPUP_LVL_REQ = "PERK_UPGRADE_POPUP_LVL_REQ2";

		private const string LANG_PERK_ACTIVATE_POPUP_REP_REQ = "PERK_ACTIVATE_POPUP_REP_REQ";

		private UXLabel activationTimerLabel;

		private ActivatedPerkData activatedPerkData;

		private bool isActivation;

		public SquadScreenActivationInfoView(SquadSlidingScreen screen, PerkVO targetPerkVO, bool isActivation) : base(screen, targetPerkVO)
		{
			this.isActivation = isActivation;
			this.InitUI();
		}

		protected override void InitUI()
		{
			base.InitUI();
			Lang lang = Service.Get<Lang>();
			PerkViewController perkViewController = Service.Get<PerkViewController>();
			PerkManager perkManager = Service.Get<PerkManager>();
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return;
			}
			Dictionary<string, string> available = currentSquad.Perks.Available;
			int level = currentSquad.Level;
			int squadLevelUnlock = this.targetPerkVO.SquadLevelUnlock;
			UXButton element = this.squadScreen.GetElement<UXButton>("BtnContinuePerkUpgradeCeleb");
			element.Visible = false;
			UXLabel element2 = this.squadScreen.GetElement<UXLabel>("LabelModalTitlePerks");
			element2.Text = lang.Get("PERK_ACTIVATE_POPUP_TITLE", new object[]
			{
				perkViewController.GetPerkNameForGroup(this.targetPerkVO.PerkGroup),
				this.targetPerkVO.PerkTier
			});
			UXLabel element3 = this.squadScreen.GetElement<UXLabel>("LabelModalStoryPerks");
			element3.Text = perkViewController.GetPerkDescForGroup(this.targetPerkVO.PerkGroup);
			perkViewController.SetupStatGridForPerk(this.targetPerkVO, this.statGrid, "TemplateModalStatsPerks", "LabelModalStatsInfoPerks", "LabelModalStatsValuePerks", false);
			UXTexture element4 = this.squadScreen.GetElement<UXTexture>("TexturePerkArtModalCardPerks");
			perkViewController.SetPerkImage(element4, this.targetPerkVO);
			this.activationTimerLabel = this.squadScreen.GetElement<UXLabel>("LabelTickerModalPerks");
			UXButton element5 = this.squadScreen.GetElement<UXButton>("BtnModalOneCurrencyPerks");
			UXButton element6 = this.squadScreen.GetElement<UXButton>("BtnModalTwoCurrencyPerks");
			this.activationTimerLabel.Visible = false;
			element5.Visible = false;
			element6.Visible = false;
			bool flag = !available.ContainsKey(this.targetPerkVO.PerkGroup);
			if (flag)
			{
				bool flag2 = perkManager.IsPerkLevelLocked(this.targetPerkVO, level);
				if (flag2)
				{
					UXLabel element7 = this.squadScreen.GetElement<UXLabel>("LabelPrimaryLvlLockedModalPerks");
					UXLabel element8 = this.squadScreen.GetElement<UXLabel>("LabelSquadLvlLockedModalPerks");
					UXLabel element9 = this.squadScreen.GetElement<UXLabel>("LabelSecondaryLvlLockedModalPerks");
					element7.Text = lang.Get("PERK_UPGRADE_POPUP_LVL_REQ", new object[0]);
					element8.Text = squadLevelUnlock.ToString();
					element9.Text = lang.Get("PERK_UPGRADE_POPUP_LVL_REQ2", new object[0]);
					this.levelLockedGroup.Visible = true;
				}
				else
				{
					UXLabel element10 = this.squadScreen.GetElement<UXLabel>("LabelPrimaryRepLockedModalPerks");
					this.repLockedGroup.Visible = true;
					element10.Text = lang.Get("PERK_ACTIVATE_POPUP_REP_REQ", new object[0]);
				}
			}
			if (this.isActivation && !flag)
			{
				Dictionary<string, int> hQScaledCostForPlayer = GameUtils.GetHQScaledCostForPlayer(this.targetPerkVO.ActivationCost);
				int count = hQScaledCostForPlayer.Count;
				bool flag3 = count == 2;
				element5.Visible = !flag3;
				element6.Visible = flag3;
				UXButton uXButton = flag3 ? element6 : element5;
				uXButton.OnClicked = new UXButtonClickedDelegate(this.OnActivateButtonClicked);
				this.rootInfoView.Visible = true;
				string[] costElementNames = flag3 ? SquadScreenActivationInfoView.dualCostElementNames : SquadScreenActivationInfoView.singleCostElementNames;
				UXUtils.SetupMultiCostElements(this.squadScreen, costElementNames, null, this.targetPerkVO.ActivationCost, count);
				this.rootInfoView.Visible = false;
			}
			if (perkManager.IsPerkActive(this.targetPerkVO.Uid))
			{
				this.activatedPerkData = Service.Get<PerkManager>().GetPlayerPerk(this.targetPerkVO.Uid);
				if (this.activatedPerkData != null)
				{
					this.activationTimerLabel.Visible = true;
					this.UpdateActivationTimeRemaining();
					Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
				}
			}
		}

		private void OnActivateButtonClicked(UXButton button)
		{
			PerkManager perkManager = Service.Get<PerkManager>();
			string purchaseContextForActivationCost = perkManager.GetPurchaseContextForActivationCost(this.targetPerkVO);
			if (!perkManager.HasPlayerActivatedFirstPerk())
			{
				this.CompletePerkActivation();
				return;
			}
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return;
			}
			string squadLevelUIDFromSquad = GameUtils.GetSquadLevelUIDFromSquad(currentSquad);
			if (!perkManager.HasEmptyPerkActivationSlot(squadLevelUIDFromSquad))
			{
				string instructions = Service.Get<Lang>().Get("PERK_ACTIVATE_NO_SLOT_AVAILABLE", new object[0]);
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(instructions);
				return;
			}
			if (MultiResourcePayMeScreen.ShowIfNotEnoughMultipleCurrencies(this.targetPerkVO.ActivationCost, purchaseContextForActivationCost, new OnScreenModalResult(this.OnPayMeForCurrencyResult)))
			{
				return;
			}
			this.CompletePerkActivation();
		}

		private void OnPayMeForCurrencyResult(object result, object cookie)
		{
			bool flag = result != null;
			if (flag)
			{
				MultiCurrencyTag multiCurrencyTag = (MultiCurrencyTag)cookie;
				bool flag2 = GameUtils.BuySoftCurrenciesWithCrystals(multiCurrencyTag.Credits, multiCurrencyTag.Materials, multiCurrencyTag.Contraband, multiCurrencyTag.Crystals, multiCurrencyTag.PurchaseContext, this.targetPerkVO.Uid);
				if (flag2)
				{
					this.CompletePerkActivation();
					return;
				}
			}
			else
			{
				this.HideAndCleanUp();
			}
		}

		private void CompletePerkActivation()
		{
			string uid = this.targetPerkVO.Uid;
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return;
			}
			Dictionary<string, string> available = currentSquad.Perks.Available;
			string squadLevelUIDFromSquad = GameUtils.GetSquadLevelUIDFromSquad(currentSquad);
			PerkManager perkManager = Service.Get<PerkManager>();
			bool flag = perkManager.ActivatePlayerPerk(uid, available, squadLevelUIDFromSquad);
			if (flag)
			{
				List<Entity> buildingsForPerk = perkManager.GetBuildingsForPerk(this.targetPerkVO);
				int i = 0;
				int count = buildingsForPerk.Count;
				while (i < count)
				{
					Service.Get<BuildingController>().UpdateBuildingHighlightForPerks(buildingsForPerk[i]);
					i++;
				}
				Service.Get<EventManager>().SendEvent(EventId.PerkActivated, null);
			}
			this.HideAndCleanUp();
		}

		public void Show()
		{
			this.squadScreen.CurrentBackButton = this.closeBtn;
			this.squadScreen.CurrentBackDelegate = new UXButtonClickedDelegate(base.OnCloseButtonClicked);
			this.rootInfoView.Visible = true;
			this.rootInfoView.InitAnimator();
			this.rootInfoView.SetTrigger("ShowActivation");
		}

		public override void HideAndCleanUp()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			Service.Get<EventManager>().SendEvent(EventId.PerkActivationClosed, null);
			base.HideAndCleanUp();
		}

		public void OnViewClockTime(float dt)
		{
			this.UpdateActivationTimeRemaining();
		}

		private void UpdateActivationTimeRemaining()
		{
			uint endTime = this.activatedPerkData.EndTime;
			if (ServerTime.Time > endTime)
			{
				this.activationTimerLabel.Visible = false;
				Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
				return;
			}
			int num = (int)(endTime - ServerTime.Time);
			string text = LangUtils.FormatTime((long)num);
			this.activationTimerLabel.Text = Service.Get<Lang>().Get("PERK_ACTIVATE_POPUP_TIMER", new object[]
			{
				text
			});
		}

		protected internal SquadScreenActivationInfoView(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadScreenActivationInfoView)GCHandledObjects.GCHandleToObject(instance)).CompletePerkActivation();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadScreenActivationInfoView)GCHandledObjects.GCHandleToObject(instance)).HideAndCleanUp();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadScreenActivationInfoView)GCHandledObjects.GCHandleToObject(instance)).InitUI();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadScreenActivationInfoView)GCHandledObjects.GCHandleToObject(instance)).OnActivateButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadScreenActivationInfoView)GCHandledObjects.GCHandleToObject(instance)).OnPayMeForCurrencyResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadScreenActivationInfoView)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadScreenActivationInfoView)GCHandledObjects.GCHandleToObject(instance)).Show();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadScreenActivationInfoView)GCHandledObjects.GCHandleToObject(instance)).UpdateActivationTimeRemaining();
			return -1L;
		}
	}
}
