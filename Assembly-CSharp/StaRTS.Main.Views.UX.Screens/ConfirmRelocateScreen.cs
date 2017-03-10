using Net.RichardLord.Ash.Core;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Perks;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ConfirmRelocateScreen : AlertScreen, IEventObserver
	{
		protected const string PLANETS_RELOCATE_CONFIRM_TITLE = "PLANETS_RELOCATE_CONFIRM_TITLE";

		protected const string PLANETS_RELOCATE_STARS_REQUIRED = "PLANETS_RELOCATE_STARS_REQUIRED";

		protected const string PLANETS_RELOCATE_CONFIRM_FIRST = "PLANETS_RELOCATE_CONFIRM_FIRST";

		protected const string PLANETS_RELOCATE_CONFIRM_STARS = "PLANETS_RELOCATE_CONFIRM_STARS";

		protected const string PLANETS_RELOCATE_MSG_REQ_MET = "PLANETS_RELOCATE_MSG_REQ_MET";

		protected const string PLANETS_RELOCATE_BUTTON = "Planets_Relocate_Button";

		protected const string CANCEL = "CANCEL";

		protected const string PLANETS_RELOCATE_STARS_ACHIEVED = "PLANETS_RELOCATE_STARS_ACHIEVED";

		protected const string PLANET_RELOCATE_EQUIPMENT_DEACTIVATE_WARNING = "PLANET_RELOCATE_EQUIPMENT_DEACTIVATE_WARNING";

		protected const string PLANETS_RELOCATE_EFFECT_PREFIX = "perkEffect_desc_Rlo";

		protected const string PLANETS_RELOCATE_EFFECT_POSTFIX = "perkEffect_descMod_Rlo";

		protected const string BUTTON_SHOW_PERKS = "btnPerksDlgSmall";

		protected const string GROUP_PERK_BENEFIT = "GroupPerkMessage";

		protected const string LABEL_PERK_BENEFIT = "LabelPerkMsg";

		protected const string PERK_EFFECT = "PerkEffectRelocation";

		protected const string PERK_TITLE_LABEL = "LabelTitlePerks";

		protected PlanetVO planetVO;

		protected bool relocationFree;

		protected bool relocationReqMet;

		protected CurrentPlayer currentPlayer;

		protected int totalCrystalCost;

		private uint perkRefreshTimer;

		public static void ShowModal(PlanetVO planetVO, OnScreenModalResult onModalResult, object modalResultCookie)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			bool flag = Service.Get<BuildingLookupController>().HasNavigationCenter();
			if (planetVO == null || !planetVO.PlayerFacing || planetVO == currentPlayer.Planet || !currentPlayer.IsPlanetUnlocked(planetVO.Uid) || !flag)
			{
				return;
			}
			string text = currentPlayer.IsRelocationRequirementMet().ToString();
			string cookie = string.Concat(new object[]
			{
				currentPlayer.GetRawRelocationStarsCount(),
				"|",
				text,
				"|",
				planetVO.PlanetBIName,
				"|",
				currentPlayer.Planet.PlanetBIName
			});
			Service.Get<EventManager>().SendEvent(EventId.PlanetRelocateButtonPressed, cookie);
			ConfirmRelocateScreen confirmRelocateScreen = new ConfirmRelocateScreen(planetVO);
			confirmRelocateScreen.OnModalResult = onModalResult;
			confirmRelocateScreen.ModalResultCookie = modalResultCookie;
			confirmRelocateScreen.IsAlwaysOnTop = true;
			Service.Get<ScreenController>().AddScreen(confirmRelocateScreen);
		}

		protected override void InitButtons()
		{
			base.InitButtons();
			UXElement element = base.GetElement<UXElement>("TitleGroupPerks");
			element.Visible = false;
			UXElement element2 = base.GetElement<UXElement>("PerkEffectRelocation");
			element2.Visible = false;
			UXLabel element3 = base.GetElement<UXLabel>("LabelPerkMsg");
			element3.Visible = false;
			List<ActivatedPerkData> activeNavCenterPerks = Service.Get<PerkManager>().GetActiveNavCenterPerks();
			if (activeNavCenterPerks != null)
			{
				int relocationCostDiscount = Service.Get<PerkManager>().GetRelocationCostDiscount();
				if (relocationCostDiscount <= 0)
				{
					return;
				}
				UXButton element4 = base.GetElement<UXButton>("btnPerksDlgSmall");
				element4.OnClicked = new UXButtonClickedDelegate(this.OnShowPerksButtonClicked);
				element.Visible = true;
				element2.Visible = true;
				this.titleLabel.Visible = false;
				UXLabel element5 = base.GetElement<UXLabel>("LabelTitlePerks");
				element5.Text = this.title;
				element3.Text = this.lang.Get("perkEffect_desc_Rlo", new object[0]) + this.lang.Get("perkEffect_descMod_Rlo", new object[]
				{
					relocationCostDiscount
				});
				element3.Visible = true;
				Service.Get<EventManager>().RegisterObserver(this, EventId.ActivePerksUpdated);
				uint num = 0u;
				int count = activeNavCenterPerks.Count;
				for (int i = 0; i < count; i++)
				{
					uint time = ServerTime.Time;
					uint num2 = activeNavCenterPerks[i].EndTime - time;
					if (num == 0u || num2 < num)
					{
						num = num2;
					}
				}
				this.perkRefreshTimer = Service.Get<ViewTimerManager>().CreateViewTimer(num, false, new TimerDelegate(this.OnPerkExpired), null);
			}
		}

		private void OnPerkExpired(uint id, object cookie)
		{
			this.RefreshScreen();
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ActivePerksUpdated)
			{
				this.RefreshScreen();
			}
			return base.OnEvent(id, cookie);
		}

		public void OnShowPerksButtonClicked(UXButton button)
		{
			Entity currentNavigationCenter = Service.Get<BuildingLookupController>().GetCurrentNavigationCenter();
			BuildingTypeVO buildingType = currentNavigationCenter.Get<BuildingComponent>().BuildingType;
			Service.Get<PerkViewController>().ShowActivePerksScreen(buildingType);
		}

		private ConfirmRelocateScreen(PlanetVO planetVO) : base(false, "", "", null, false)
		{
			this.planetVO = planetVO;
			this.currentPlayer = Service.Get<CurrentPlayer>();
			this.relocationFree = this.currentPlayer.IsRelocationFree();
			this.relocationReqMet = this.currentPlayer.IsRelocationRequirementMet();
			string title;
			string message;
			if (this.relocationFree)
			{
				title = this.lang.Get("PLANETS_RELOCATE_CONFIRM_TITLE", new object[]
				{
					LangUtils.GetPlanetDisplayName(planetVO)
				});
				message = this.lang.Get("PLANETS_RELOCATE_CONFIRM_FIRST", new object[]
				{
					LangUtils.GetPlanetDisplayName(planetVO)
				});
			}
			else if (this.relocationReqMet)
			{
				title = this.lang.Get("PLANETS_RELOCATE_CONFIRM_TITLE", new object[]
				{
					LangUtils.GetPlanetDisplayName(planetVO)
				});
				message = "";
			}
			else
			{
				title = this.lang.Get("PLANETS_RELOCATE_STARS_REQUIRED", new object[]
				{
					LangUtils.GetPlanetDisplayName(planetVO)
				});
				message = "";
			}
			this.title = title;
			this.message = message;
		}

		protected override void OnScreenLoaded()
		{
			base.OnScreenLoaded();
			this.primaryButton.Visible = false;
			this.secondary2OptionButton.Visible = true;
			this.secondary2OptionButton.OnClicked = new UXButtonClickedDelegate(this.OnCancel);
			this.buttonOption2.Visible = true;
			this.primary2Option.Text = this.lang.Get("Planets_Relocate_Button", new object[0]);
			this.secondary2Option.Text = this.lang.Get("CANCEL", new object[0]);
			if (this.relocationFree)
			{
				this.primary2OptionButton.Visible = true;
				this.primary2OptionButton.OnClicked = new UXButtonClickedDelegate(this.OnConfirmFree);
			}
			else if (this.relocationReqMet)
			{
				this.primary2OptionButton.Visible = true;
				this.primary2OptionButton.OnClicked = new UXButtonClickedDelegate(this.OnConfirmStarRequirementMet);
				this.costRequirement.Visible = true;
				string relocationStarText = this.lang.Get("PLANETS_RELOCATE_MSG_REQ_MET", new object[]
				{
					LangUtils.GetPlanetDisplayName(this.planetVO)
				});
				this.SetRelocationStarText(relocationStarText);
			}
			else
			{
				this.costRequirement.Visible = true;
				string relocationStarText2 = this.lang.Get("PLANETS_RELOCATE_CONFIRM_STARS", new object[]
				{
					LangUtils.GetPlanetDisplayName(this.currentPlayer.PlanetId),
					LangUtils.GetPlanetDisplayName(this.planetVO)
				});
				this.SetRelocationStarText(relocationStarText2);
				this.option2ButtonSkip.Visible = true;
				this.option2ButtonSkip.OnClicked = new UXButtonClickedDelegate(this.OnConfirmPayCrystals);
				this.totalCrystalCost = this.currentPlayer.GetCrystalRelocationCost();
				this.labelSkipCost.Text = this.totalCrystalCost.ToString();
			}
			this.requirementLabel.Visible = true;
			this.requirementLabel.Text = this.lang.Get("PLANET_RELOCATE_EQUIPMENT_DEACTIVATE_WARNING", new object[0]);
		}

		protected override void RefreshScreen()
		{
			ConfirmRelocateScreen.ShowModal(this.planetVO, base.OnModalResult, base.ModalResultCookie);
			base.Close(null);
		}

		private void CleanupPerkInfo()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ActivePerksUpdated);
			Service.Get<ViewTimerManager>().KillViewTimer(this.perkRefreshTimer);
			this.perkRefreshTimer = 0u;
		}

		private void SetRelocationStarText(string message)
		{
			this.labelBodyRequirement.Visible = true;
			this.labelBodyRequirement.Text = message;
			int displayRelocationStarsCount = this.currentPlayer.GetDisplayRelocationStarsCount();
			int requiredRelocationStars = this.currentPlayer.GetRequiredRelocationStars();
			float value = (float)displayRelocationStarsCount / (float)requiredRelocationStars;
			this.requirementSlider.Value = value;
			string text = this.lang.Get("PLANETS_RELOCATE_STARS_ACHIEVED", new object[]
			{
				displayRelocationStarsCount,
				requiredRelocationStars
			});
			this.labelStarProgress.Text = text;
		}

		public void OnConfirmFree(UXButton button)
		{
			base.Close(null);
			this.currentPlayer.ResetRelocationStars();
			Service.Get<PlanetRelocationController>().SendRelocationRequest(this.planetVO, false);
		}

		public void OnConfirmStarRequirementMet(UXButton button)
		{
			base.Close(null);
			this.currentPlayer.ResetRelocationStars();
			Service.Get<PlanetRelocationController>().SendRelocationRequest(this.planetVO, false);
		}

		public void OnConfirmPayCrystals(UXButton button)
		{
			base.Close(null);
			if (!GameUtils.SpendCrystals(this.totalCrystalCost))
			{
				return;
			}
			this.currentPlayer.ResetRelocationStars();
			Service.Get<PlanetRelocationController>().SendRelocationRequest(this.planetVO, true);
			int currencyAmount = -this.totalCrystalCost;
			string itemType = "relocation";
			string itemId = this.planetVO.PlanetBIName.ToLower();
			int itemCount = 1;
			string type = "speed_up_relocation";
			string subType = "consumable";
			Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, itemId, itemCount, type, subType);
		}

		public void OnCancel(UXButton button)
		{
			this.Close(null);
		}

		public override void Close(object modelResult)
		{
			Service.Get<EventManager>().SendEvent(EventId.GalaxyNotEnoughRelocateStarsClose, null);
			base.Close(null);
		}

		public override void OnDestroyElement()
		{
			this.CleanupPerkInfo();
			base.OnDestroyElement();
		}

		protected internal ConfirmRelocateScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).CleanupPerkInfo();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).OnCancel((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).OnConfirmFree((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).OnConfirmPayCrystals((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).OnConfirmStarRequirementMet((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).OnShowPerksButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshScreen();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ConfirmRelocateScreen)GCHandledObjects.GCHandleToObject(instance)).SetRelocationStarText(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			ConfirmRelocateScreen.ShowModal((PlanetVO)GCHandledObjects.GCHandleToObject(*args), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}
	}
}
