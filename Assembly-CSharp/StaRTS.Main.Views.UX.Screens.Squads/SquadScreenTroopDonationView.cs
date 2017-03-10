using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Perks;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadScreenTroopDonationView : AbstractSquadScreenViewModule, IViewClockTimeObserver
	{
		private const string DONATE_TROOPS_PANEL = "DonatePanel";

		private const string DONATE_TITLE_LABEL = "LabelDonateTitle";

		private const string DONATE_HINT = "LabelDonateHint";

		private const string REWARD_LABEL = "LabelDonateReward";

		private const string DONATE_GRID = "DonateTroopsGrid";

		private const string DONATE_CLOSE_BTN = "DonateBtnClose";

		private const string DONATE_CAPACITY_SLIDER = "PbarDonatePopup";

		private const string DONATE_CAPACITY_LABEL = "LabelPbarDonatePopup";

		private const string DONATE_NO_TROOPS_LABEL = "LabelDonateNoTroops";

		private const string DONATE_TRAIN_BUTTON = "BtnTrainTroops";

		private const string DONATE_BUILD_BUTTON = "BtnBuildVehicles";

		private const string DONATE_DIMMER = "SpriteDonateTroopsDim";

		private const string DONATE_CANCEL = "BtnDonateTroopsCancel";

		private const string DONATE_CONFIRM = "BtnDonateTroopsConfirmation";

		private const string DONATE_LABEL_CANCEL = "LabelBtnDonateTroopsCancel";

		private const string DONATE_LABEL_CONFIRM = "LabelBtnDonateTroopsConfirmation";

		private const string DONATE_LANG_CANCEL = "s_Cancel";

		private const string DONATE_LANG_CONFIRM = "s_Confirm";

		private const string DONATE_TEMPLATE = "DonateTroopsCard";

		private const string LABEL_DONATE_TROOPS_CARD = "LabelDonateTroops";

		private const string IMAGE_DONATE_TROOPS_CARD = "SpriteDonateTroopsItem";

		private const string LEVEL_DONATE_TROOPS_CARD = "LabelTroopLevel";

		private const string CAPACITY = "CAPACITY";

		private const string TROOP_COUNT_TEXT = "x{0}";

		private const string SQUAD_DONATE_TO_USER = "SQUAD_DONATE_TO_USER";

		private const string TROOP_ID_PREFIX = "troop_";

		private const string ARROW_SPRITE = "SpriteTroopDonatePanelScrollRight";

		private const string SQUAD_DONATE_LIMIT = "SQUAD_DONATE_LIMIT";

		private const string SQUAD_DONATE_NO_TROOPS = "SQUAD_DONATE_NO_TROOPS";

		private const string SQUAD_DONATE_NO_CAPACITY_FOR_UNIT = "SQUAD_DONATE_NO_CAPACITY_FOR_UNIT";

		private const string SQUAD_DONATE_NO_CAPACITY = "SQUAD_DONATE_NO_CAPACITY";

		private const string EARN_REPUTATION_PANEL = "PanelEarnReputation";

		private const string EARN_REPUTATION_COMPLETE = "EarnReputationComplete";

		private const string EARN_REPUTATION_COMPLETE_LABEL = "LabelEarnReputationComplete";

		private const string EARN_REPUTATION_PROGRESS = "EarnReputationProgress";

		private const string EARN_REPUTATION_PROGRESS_INFO_LABEL = "LabelEarnReputation";

		private const string EARN_REPUTATION_SLIDER = "PBarDonateForReputation";

		private const string EARN_REPUTATION_PROGRESS_LABEL = "LabelEarnReputationProgress";

		private const string EARN_REPUTATION_TIMER = "LabelEarnReputationTimer";

		private const string EARN_REPUTATION_REWARD_AMOUNT_LABEL = "LabelRepAmountChat";

		private const string REPUTATION_TITLE_COMPLETE_STRING = "PERK_CHAT_DONATE_TITLE_COMPLETE";

		private const string REPUTATION_DESC_STRING = "PERK_CHAT_DONATE_DESC";

		private const string COUNTDOWN_TIMER_STRING = "PERK_CHAT_DONATE_TIMER";

		private UXElement donateTroopsPanel;

		private UXLabel donateTroopsTitle;

		private UXElement troopTemplateItem;

		private UXButton troopDonateCloseBtn;

		private UXGrid troopDonateGrid;

		private UXSlider capacityBar;

		private UXLabel capacityLabel;

		private UXLabel hintLabel;

		private UXLabel noTroopsLabel;

		private UXButton trainTroopsButton;

		private UXButton buildVehiclesButton;

		private UXButton donateTroopsCancel;

		private UXButton donateTroopsConfirm;

		private UXLabel donateTroopsCancelLabel;

		private UXLabel donateTroopsConfrimLabel;

		private UXElement earnReputationPanel;

		private UXElement earnReputationComplete;

		private UXElement earnReputationProgress;

		private UXSlider earnReputationProgressBar;

		private UXLabel earnReputationProgressLabel;

		private UXLabel earnReputationTimer;

		private Dictionary<string, int> troopsToDonate;

		private bool hasTroops;

		private bool hasEligibleTroop;

		private int alreadyDonatedSize;

		private int totalCapacity;

		private int currentPlayerDonationCount;

		private string recipientUserName;

		private string recipientId;

		private string requestId;

		private bool isWarRequest;

		private int troopDonationLimit;

		private TroopDonationProgress donationProgress;

		public SquadScreenTroopDonationView(SquadSlidingScreen screen) : base(screen)
		{
			this.troopsToDonate = new Dictionary<string, int>();
			this.hasTroops = false;
			this.hasEligibleTroop = false;
		}

		public override void OnScreenLoaded()
		{
			this.donateTroopsPanel = this.screen.GetElement<UXElement>("DonatePanel");
			this.donateTroopsTitle = this.screen.GetElement<UXLabel>("LabelDonateTitle");
			this.troopTemplateItem = this.screen.GetElement<UXElement>("DonateTroopsCard");
			this.troopTemplateItem.Visible = false;
			this.troopDonateCloseBtn = this.screen.GetElement<UXButton>("DonateBtnClose");
			this.troopDonateCloseBtn.OnClicked = new UXButtonClickedDelegate(this.OnDonateClose);
			this.troopDonateGrid = this.screen.GetElement<UXGrid>("DonateTroopsGrid");
			this.troopDonateGrid.BypassLocalPositionOnAdd = true;
			this.capacityBar = this.screen.GetElement<UXSlider>("PbarDonatePopup");
			this.capacityLabel = this.screen.GetElement<UXLabel>("LabelPbarDonatePopup");
			this.hintLabel = this.screen.GetElement<UXLabel>("LabelDonateHint");
			this.noTroopsLabel = this.screen.GetElement<UXLabel>("LabelDonateNoTroops");
			this.trainTroopsButton = this.screen.GetElement<UXButton>("BtnTrainTroops");
			this.buildVehiclesButton = this.screen.GetElement<UXButton>("BtnBuildVehicles");
			this.donateTroopsCancel = this.screen.GetElement<UXButton>("BtnDonateTroopsCancel");
			this.donateTroopsConfirm = this.screen.GetElement<UXButton>("BtnDonateTroopsConfirmation");
			this.donateTroopsConfirm.Enabled = false;
			this.donateTroopsCancelLabel = this.screen.GetElement<UXLabel>("LabelBtnDonateTroopsCancel");
			this.donateTroopsCancelLabel.Text = Service.Get<Lang>().Get("s_Cancel", new object[0]);
			this.donateTroopsConfrimLabel = this.screen.GetElement<UXLabel>("LabelBtnDonateTroopsConfirmation");
			this.donateTroopsConfrimLabel.Text = Service.Get<Lang>().Get("s_Confirm", new object[0]);
			this.earnReputationPanel = this.screen.GetElement<UXElement>("PanelEarnReputation");
			this.earnReputationComplete = this.screen.GetElement<UXElement>("EarnReputationComplete");
			this.earnReputationProgress = this.screen.GetElement<UXElement>("EarnReputationProgress");
			this.earnReputationProgressBar = this.screen.GetElement<UXSlider>("PBarDonateForReputation");
			this.earnReputationProgressLabel = this.screen.GetElement<UXLabel>("LabelEarnReputationProgress");
			this.earnReputationTimer = this.screen.GetElement<UXLabel>("LabelEarnReputationTimer");
			UXLabel element = this.screen.GetElement<UXLabel>("LabelEarnReputation");
			element.Text = this.lang.Get("PERK_CHAT_DONATE_DESC", new object[]
			{
				GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD,
				GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD
			});
			UXLabel element2 = this.screen.GetElement<UXLabel>("LabelRepAmountChat");
			element2.Text = GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD.ToString();
			UXLabel element3 = this.screen.GetElement<UXLabel>("LabelEarnReputationComplete");
			element3.Text = this.lang.Get("PERK_CHAT_DONATE_TITLE_COMPLETE", new object[0]);
		}

		public void InitView(string recipientId, string recipientUserName, int alreadyDonatedSize, int totalCapacity, int currentPlayerDonationCount, string requestId, bool isWarRequest, int troopDonationLimit, TroopDonationProgress donationProgress)
		{
			this.recipientId = recipientId;
			this.recipientUserName = recipientUserName;
			this.alreadyDonatedSize = alreadyDonatedSize;
			this.totalCapacity = totalCapacity;
			this.currentPlayerDonationCount = currentPlayerDonationCount;
			this.requestId = requestId;
			this.isWarRequest = isWarRequest;
			this.troopDonationLimit = troopDonationLimit;
			this.donationProgress = donationProgress;
		}

		public override void ShowView()
		{
			this.donateTroopsPanel.Visible = true;
			this.donateTroopsTitle.Text = this.lang.Get("SQUAD_DONATE_TO_USER", new object[]
			{
				this.recipientUserName
			});
			this.hasTroops = false;
			this.hasEligibleTroop = false;
			IEnumerable<KeyValuePair<string, InventoryEntry>> allTroops = Service.Get<CurrentPlayer>().GetAllTroops();
			if (allTroops != null)
			{
				using (IEnumerator<KeyValuePair<string, InventoryEntry>> enumerator = allTroops.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, InventoryEntry> current = enumerator.get_Current();
						if (current.get_Value().Amount > 0)
						{
							string key = current.get_Key();
							IDataController dataController = Service.Get<IDataController>();
							TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(key);
							if (!troopTypeVO.PreventDonation)
							{
								bool flag = this.AddTroopItem(current, troopTypeVO);
								this.hasTroops = true;
								if (flag && !this.hasEligibleTroop)
								{
									this.hasEligibleTroop = true;
								}
							}
						}
					}
				}
			}
			this.capacityBar.Visible = this.hasEligibleTroop;
			this.hintLabel.Visible = this.hasEligibleTroop;
			this.noTroopsLabel.Visible = !this.hasEligibleTroop;
			this.trainTroopsButton.Visible = !this.hasEligibleTroop;
			this.buildVehiclesButton.Visible = !this.hasEligibleTroop;
			if (this.hasTroops)
			{
				this.troopDonateGrid.RepositionItemsFrameDelayed();
				this.hintLabel.Text = this.lang.Get("SQUAD_DONATE_LIMIT", new object[]
				{
					this.troopDonationLimit
				});
				this.troopDonateCloseBtn.Visible = false;
				this.donateTroopsCancel.OnClicked = new UXButtonClickedDelegate(this.OnDonateClose);
				this.donateTroopsConfirm.OnClicked = new UXButtonClickedDelegate(this.OnDonateConfirm);
				this.donateTroopsCancel.Visible = true;
				this.donateTroopsConfirm.Visible = true;
			}
			else
			{
				this.noTroopsLabel.Text = this.lang.Get("SQUAD_DONATE_NO_TROOPS", new object[0]);
				UXSprite element = this.screen.GetElement<UXSprite>("SpriteTroopDonatePanelScrollRight");
				element.Visible = false;
			}
			if (!this.hasEligibleTroop)
			{
				this.donateTroopsCancel.Visible = false;
				this.donateTroopsConfirm.Visible = false;
				this.SetupTrainTroopsButtons();
				this.troopDonateCloseBtn.Visible = true;
			}
			this.earnReputationPanel.Visible = true;
			if (!Service.Get<TroopDonationTrackController>().IsTroopDonationProgressComplete())
			{
				this.ShowRepProgressDonationState();
			}
			else
			{
				this.earnReputationProgress.Visible = false;
				this.earnReputationComplete.Visible = true;
				this.UpdateLabelTimeRemaining(this.earnReputationTimer);
				Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
			}
			this.RefreshView();
		}

		public void CloseView()
		{
			SquadController squadController = Service.Get<SquadController>();
			squadController.StateManager.SquadScreenState = SquadScreenState.Chat;
			this.screen.RefreshViews();
		}

		public override void HideView()
		{
			if (this.donateTroopsPanel != null)
			{
				this.donateTroopsPanel.Visible = false;
			}
			if (this.troopDonateGrid != null)
			{
				this.troopDonateGrid.Clear();
			}
			if (this.donateTroopsPanel != null)
			{
				this.donateTroopsPanel.Visible = false;
			}
			this.troopsToDonate.Clear();
			if (this.donateTroopsConfirm != null)
			{
				this.donateTroopsConfirm.Enabled = false;
			}
		}

		public override void RefreshView()
		{
			IDataController dataController = Service.Get<IDataController>();
			List<UXElement> elementList = this.troopDonateGrid.GetElementList();
			int num = this.GetNumberOfTroopsDonated() + this.currentPlayerDonationCount;
			bool flag = num < this.troopDonationLimit;
			int num2 = this.GetTotalSizeOfTroopsToDonate(dataController) + this.alreadyDonatedSize;
			int num3 = this.totalCapacity - num2;
			int i = 0;
			int count = elementList.Count;
			while (i < count)
			{
				UXElement uXElement = elementList[i];
				KeyValuePair<string, InventoryEntry> keyValuePair = (KeyValuePair<string, InventoryEntry>)uXElement.Tag;
				string key = keyValuePair.get_Key();
				TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(key);
				bool flag2 = troopTypeVO.Size <= num3;
				int num4 = 0;
				if (this.troopsToDonate.ContainsKey(key))
				{
					num4 = this.troopsToDonate[key];
				}
				bool flag3 = keyValuePair.get_Value().Amount > num4;
				bool enabled = flag2 & flag & flag3;
				this.SetVisuallyDisabledState(uXElement as UXButton, key, enabled);
				i++;
			}
			this.capacityLabel.Text = Service.Get<Lang>().Get("CAPACITY", new object[]
			{
				num2,
				this.totalCapacity
			});
			this.capacityBar.Value = (float)num2 / (float)this.totalCapacity;
			int num5 = Service.Get<TroopDonationTrackController>().GetTroopDonationProgressAmount() + this.GetNumberOfTroopsDonated();
			int sQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD = GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD;
			if (num5 > sQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD)
			{
				num5 = sQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD;
			}
			this.earnReputationProgressLabel.Text = num5 + " / " + sQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD;
			this.earnReputationProgressBar.Value = (float)num5 / (float)sQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD;
		}

		private bool AddTroopItem(KeyValuePair<string, InventoryEntry> unit, TroopTypeVO troop)
		{
			string itemUid = "troop_" + troop.Uid;
			UXButton uXButton = this.troopDonateGrid.CloneItem(itemUid, this.troopTemplateItem) as UXButton;
			uXButton.Tag = unit;
			uXButton.OnClicked = new UXButtonClickedDelegate(this.OnTroopToDonateClicked);
			UXLabel subElement = this.troopDonateGrid.GetSubElement<UXLabel>(itemUid, "LabelDonateTroops");
			subElement.Text = string.Format("x{0}", new object[]
			{
				unit.get_Value().Amount
			});
			UXLabel subElement2 = this.troopDonateGrid.GetSubElement<UXLabel>(itemUid, "LabelTroopLevel");
			subElement2.Text = LangUtils.GetLevelText(troop.Lvl);
			UXSprite subElement3 = this.troopDonateGrid.GetSubElement<UXSprite>(itemUid, "SpriteDonateTroopsItem");
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(troop, subElement3);
			projectorConfig.AnimPreference = AnimationPreference.NoAnimation;
			ProjectorUtils.GenerateProjector(projectorConfig);
			this.troopDonateGrid.AddItem(uXButton, troop.Order);
			return troop.Size <= this.totalCapacity - this.alreadyDonatedSize;
		}

		private void SetVisuallyDisabledState(UXButton item, string troopUid, bool enabled)
		{
			string itemUid = "troop_" + troopUid;
			UXSprite subElement = this.troopDonateGrid.GetSubElement<UXSprite>(itemUid, "SpriteDonateTroopsDim");
			subElement.Visible = !enabled;
			if (!enabled)
			{
				item.VisuallyDisableButton();
				return;
			}
			if (item.VisuallyDisabled)
			{
				item.VisuallyEnableButton();
			}
		}

		private void SetupTrainTroopsButtons()
		{
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			Entity highestAvailableBarracks = buildingLookupController.GetHighestAvailableBarracks();
			if (highestAvailableBarracks != null)
			{
				this.trainTroopsButton.Tag = highestAvailableBarracks;
				this.trainTroopsButton.OnClicked = new UXButtonClickedDelegate(this.OnTrainTroopsClicked);
				this.trainTroopsButton.Enabled = true;
			}
			else
			{
				this.trainTroopsButton.Enabled = false;
			}
			Entity highestAvailableFactory = buildingLookupController.GetHighestAvailableFactory();
			if (highestAvailableFactory != null)
			{
				this.buildVehiclesButton.Tag = highestAvailableFactory;
				this.buildVehiclesButton.OnClicked = new UXButtonClickedDelegate(this.OnTrainTroopsClicked);
				this.buildVehiclesButton.Enabled = true;
				return;
			}
			this.buildVehiclesButton.Enabled = false;
		}

		private void OnTrainTroopsClicked(UXButton button)
		{
			Entity entity = (Entity)button.Tag;
			if (Service.Get<GameStateMachine>().CurrentState is GalaxyState)
			{
				ScreenController screenController = Service.Get<ScreenController>();
				screenController.CloseAll();
				Service.Get<GalaxyViewController>().GoToHome(true, new WipeCompleteDelegate(this.OpenTroopTrainingScreen), entity);
			}
			else
			{
				this.OpenTroopTrainingScreen(entity);
			}
			this.screen.OnSquadSlideClicked(null);
		}

		private void OpenTroopTrainingScreen(object cookie)
		{
			Entity selectedBuilding = (Entity)cookie;
			Service.Get<ScreenController>().AddScreen(new TroopTrainingScreen(selectedBuilding));
		}

		private void OnDonateClose(UXButton button)
		{
			this.CloseView();
		}

		private void OnDonateConfirm(UXButton button)
		{
			SquadMsg message;
			if (this.isWarRequest)
			{
				message = SquadMsgUtils.CreateWarDonateMessage(this.recipientId, this.troopsToDonate, this.GetNumberOfTroopsDonated(), this.requestId, new SquadController.ActionCallback(this.OnDonationComplete), null);
			}
			else
			{
				message = SquadMsgUtils.CreateDonateMessage(this.recipientId, this.troopsToDonate, this.GetNumberOfTroopsDonated(), this.requestId, new SquadController.ActionCallback(this.OnDonationComplete), null);
			}
			SquadController squadController = Service.Get<SquadController>();
			squadController.TakeAction(message);
			ProcessingScreen.Show();
		}

		private void OnDonationComplete(bool success, object cookie)
		{
			ProcessingScreen.Hide();
			this.CloseView();
		}

		private void OnTroopToDonateClicked(UXButton button)
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadSelect, null);
			KeyValuePair<string, InventoryEntry> keyValuePair = (KeyValuePair<string, InventoryEntry>)button.Tag;
			string key = keyValuePair.get_Key();
			InventoryEntry value = keyValuePair.get_Value();
			IDataController dataController = Service.Get<IDataController>();
			TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(key);
			int amount = value.Amount;
			if (button.VisuallyDisabled)
			{
				string instructions;
				if (amount <= 0)
				{
					instructions = Service.Get<Lang>().Get("SQUAD_DONATE_NO_TROOPS", new object[0]);
				}
				else if (this.hasEligibleTroop)
				{
					instructions = Service.Get<Lang>().Get("SQUAD_DONATE_NO_CAPACITY_FOR_UNIT", new object[0]);
				}
				else
				{
					instructions = Service.Get<Lang>().Get("SQUAD_DONATE_NO_CAPACITY", new object[0]);
				}
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(instructions);
				return;
			}
			int num = this.GetTotalSizeOfTroopsToDonate(dataController) + this.alreadyDonatedSize + troopTypeVO.Size;
			int num2 = this.GetNumberOfTroopsDonated() + this.currentPlayerDonationCount;
			if (amount > 0 && num <= this.totalCapacity && num2 < this.troopDonationLimit)
			{
				string itemUid = "troop_" + troopTypeVO.Uid;
				if (this.troopsToDonate.ContainsKey(key))
				{
					Dictionary<string, int> arg_123_0 = this.troopsToDonate;
					string key2 = key;
					int num3 = arg_123_0[key2];
					arg_123_0[key2] = num3 + 1;
				}
				else
				{
					this.troopsToDonate.Add(key, 1);
				}
				int num4 = value.Amount - this.troopsToDonate[key];
				UXLabel subElement = this.troopDonateGrid.GetSubElement<UXLabel>(itemUid, "LabelDonateTroops");
				subElement.Text = string.Format("x{0}", new object[]
				{
					num4
				});
			}
			this.donateTroopsConfirm.Enabled = true;
			this.RefreshView();
		}

		private int GetTotalSizeOfTroopsToDonate(IDataController sdc)
		{
			int num = 0;
			foreach (KeyValuePair<string, int> current in this.troopsToDonate)
			{
				string key = current.get_Key();
				TroopTypeVO troopTypeVO = sdc.Get<TroopTypeVO>(key);
				num += troopTypeVO.Size * current.get_Value();
			}
			return num;
		}

		private int GetNumberOfTroopsDonated()
		{
			int num = 0;
			foreach (KeyValuePair<string, int> current in this.troopsToDonate)
			{
				num += current.get_Value();
			}
			return num;
		}

		public override void OnDestroyElement()
		{
			if (this.troopDonateGrid != null)
			{
				this.troopDonateGrid.Clear();
				this.troopDonateGrid = null;
			}
			this.troopsToDonate.Clear();
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
		}

		public override bool IsVisible()
		{
			return this.donateTroopsPanel.Visible;
		}

		public void OnViewClockTime(float dt)
		{
			uint donationCooldownEndTime = (uint)this.donationProgress.DonationCooldownEndTime;
			if (donationCooldownEndTime <= 0u)
			{
				this.ShowRepProgressDonationState();
				return;
			}
			this.UpdateLabelTimeRemaining(this.earnReputationTimer);
		}

		public void ShowRepProgressDonationState()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			this.earnReputationComplete.Visible = false;
			this.earnReputationProgress.Visible = true;
		}

		private void UpdateLabelTimeRemaining(UXLabel label)
		{
			int timeRemainingUntilNextProgressTrack = Service.Get<TroopDonationTrackController>().GetTimeRemainingUntilNextProgressTrack();
			if (timeRemainingUntilNextProgressTrack <= 0)
			{
				this.ShowRepProgressDonationState();
			}
			string text = LangUtils.FormatTime((long)timeRemainingUntilNextProgressTrack);
			label.Text = this.lang.Get("PERK_CHAT_DONATE_TIMER", new object[]
			{
				text
			});
		}

		protected internal SquadScreenTroopDonationView(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).AddTroopItem((KeyValuePair<string, InventoryEntry>)GCHandledObjects.GCHandleToObject(*args), (TroopTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).CloseView();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).GetNumberOfTroopsDonated());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).GetTotalSizeOfTroopsToDonate((IDataController)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).HideView();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).InitView(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), *(sbyte*)(args + 6) != 0, *(int*)(args + 7), (TroopDonationProgress)GCHandledObjects.GCHandleToObject(args[8]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).IsVisible());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).OnDonateClose((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).OnDonateConfirm((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).OnDonationComplete(*(sbyte*)args != 0, GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).OnTrainTroopsClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).OnTroopToDonateClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).OpenTroopTrainingScreen(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).SetupTrainTroopsButtons();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).SetVisuallyDisabledState((UXButton)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).ShowRepProgressDonationState();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).ShowView();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SquadScreenTroopDonationView)GCHandledObjects.GCHandleToObject(instance)).UpdateLabelTimeRemaining((UXLabel)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
