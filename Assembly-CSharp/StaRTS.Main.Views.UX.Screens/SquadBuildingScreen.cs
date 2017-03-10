using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.Leaderboard;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class SquadBuildingScreen : BuildingInfoScreen, IViewClockTimeObserver
	{
		protected const int SQUAD_SLIDER_HITPOINTS = 0;

		protected const int SQUAD_SLIDER_CAPACITY = 1;

		protected const int SQUAD_SLIDER_REPUTATION = 2;

		protected const int SQUAD_SLIDER_COUNT = 3;

		private const string GROUP_STORAGE_ITEMS = "StorageItemsPanel";

		private const string BUTTON_CREATE = "ButtonCreateSquad";

		private const string BUTTON_JOIN = "ButtonJoinSquad";

		private const string LABEL_CREATE = "LabelCreateSquad";

		private const string LABEL_JOIN = "LabelJoinSquad";

		private const string LABEL_JOIN_DESC = "LabelSquadIncentive";

		private const string LABEL_REQUEST = "LabelBtnRequestTroops";

		private const string RESEND_COST = "CostResendRequest";

		private const string TROOP_ITEM_CARD = "StorageItemsCardTroop";

		private const string ITEM_UID_TROOP_REQUEST = "troop_request";

		private const string SQUAD_DONATED_TROOPS_TITLE = "SQUAD_DONATED_TROOPS_TITLE";

		private const string JOIN_A_SQUAD = "JOIN_A_SQUAD";

		private const string CREATE_SQUAD = "CREATE_SQUAD";

		private const string SQUAD_INCENTIVE = "SQUAD_INCENTIVE";

		private const string STARPORT_CAPACITY = "STARPORT_CAPACITY";

		private const string FRACTION = "FRACTION";

		private const string SQUAD_DONATED_TROOP = "SQUAD_DONATED_TROOP";

		private const string SQUAD_DONATED_TROOP_SENDER = "SQUAD_DONATED_TROOP_SENDER";

		private const string SQUAD_DONATED_TROOP_SENDER_OTHER = "SQUAD_DONATED_TROOP_SENDER_OTHER";

		private const string REQUEST_TROOPS = "context_RequestTroops";

		protected const string BUILDING_REPUTATION = "BUILDING_REPUTATION";

		private const int MAX_DONATORS_DISPLAYED = 3;

		private bool inVisitMode;

		private UXElement requestResendCost;

		public SquadBuildingScreen(Entity squadBuilding) : base(squadBuilding)
		{
			this.useStorageGroup = true;
			this.inVisitMode = GameUtils.IsVisitingBase();
			this.observingClockViewTime = false;
		}

		protected override void InitLabels()
		{
			base.InitLabels();
			this.labelStorage.Text = this.lang.Get("SQUAD_DONATED_TROOPS_TITLE", new object[0]);
			UXLabel element = base.GetElement<UXLabel>("LabelJoinSquad");
			element.Text = this.lang.Get("JOIN_A_SQUAD", new object[0]);
			UXLabel element2 = base.GetElement<UXLabel>("LabelCreateSquad");
			element2.Text = this.lang.Get("CREATE_SQUAD", new object[0]);
			UXLabel element3 = base.GetElement<UXLabel>("LabelSquadIncentive");
			element3.Text = this.lang.Get("SQUAD_INCENTIVE", new object[0]);
		}

		protected override void InitButtons()
		{
			base.InitButtons();
			UXButton element = base.GetElement<UXButton>("ButtonCreateSquad");
			element.OnClicked = new UXButtonClickedDelegate(this.CreateSquadClicked);
			UXButton element2 = base.GetElement<UXButton>("ButtonJoinSquad");
			element2.OnClicked = new UXButtonClickedDelegate(this.JoinSquadClicked);
		}

		protected override void OnLoaded()
		{
			base.InitControls(3);
			this.InitHitpoints(0);
			this.InitReputation();
			this.sliders[1].DescLabel.Text = this.lang.Get("STARPORT_CAPACITY", new object[0]);
			this.InitTroopGrid();
			this.UpdateHousingSpace();
			Service.Get<EventManager>().RegisterObserver(this, EventId.SquadTroopsReceived);
		}

		protected virtual void InitReputation()
		{
			this.sliders[2].CurrentLabel.Visible = true;
			this.sliders[2].CurrentSlider.Visible = true;
			this.sliders[2].DescLabel.Visible = true;
			this.sliders[2].Background.Visible = true;
			this.sliders[2].DescLabel.Text = this.lang.Get("BUILDING_REPUTATION", new object[0]);
			this.UpdateReputation(2);
		}

		protected virtual void UpdateReputation(int sliderIndex)
		{
			Inventory inventory = Service.Get<CurrentPlayer>().Inventory;
			if (!inventory.HasItem("reputation"))
			{
				this.sliders[sliderIndex].HideAll();
				Service.Get<StaRTSLogger>().WarnFormat("No reputation found in your inventory", new object[0]);
				return;
			}
			int itemAmount = inventory.GetItemAmount("reputation");
			int itemCapacity = inventory.GetItemCapacity("reputation");
			UXLabel currentLabel = this.sliders[sliderIndex].CurrentLabel;
			currentLabel.Text = this.lang.Get("FRACTION", new object[]
			{
				this.lang.ThousandsSeparated(itemAmount),
				this.lang.ThousandsSeparated(itemCapacity)
			});
			UXSlider currentSlider = this.sliders[sliderIndex].CurrentSlider;
			currentSlider.Value = ((itemCapacity == 0) ? 0f : ((float)itemAmount / (float)itemCapacity));
		}

		protected override void OnScreenLoaded()
		{
			base.OnScreenLoaded();
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			if (worldOwner.Squad == null)
			{
				base.GetElement<UXElement>("NoSquad").Visible = true;
				base.GetElement<UXElement>("StorageItemsPanel").Visible = false;
				this.labelStorage.Visible = false;
			}
			else
			{
				UXSprite element = base.GetElement<UXSprite>("SpriteSquadSymbol");
				element.Visible = true;
				element.SpriteName = worldOwner.Squad.Symbol;
			}
			if (this.inVisitMode)
			{
				base.GetElement<UXElement>("NoSquad").Visible = false;
			}
		}

		private void UpdateHousingSpace()
		{
			int donatedTroopStorageUsedByWorldOwner = SquadUtils.GetDonatedTroopStorageUsedByWorldOwner();
			int storage = this.buildingInfo.Storage;
			UXLabel currentLabel = this.sliders[1].CurrentLabel;
			currentLabel.Text = this.lang.Get("FRACTION", new object[]
			{
				this.lang.ThousandsSeparated(donatedTroopStorageUsedByWorldOwner),
				this.lang.ThousandsSeparated(storage)
			});
			UXSlider currentSlider = this.sliders[1].CurrentSlider;
			float value = (storage == 0) ? 0f : ((float)donatedTroopStorageUsedByWorldOwner / (float)storage);
			currentSlider.Value = value;
		}

		protected void InitTroopGrid()
		{
			base.InitGrid();
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			List<SquadDonatedTroop> worldOwnerSquadBuildingTroops = SquadUtils.GetWorldOwnerSquadBuildingTroops();
			int count = worldOwnerSquadBuildingTroops.Count;
			if (count > 0)
			{
				Dictionary<string, string> dictionary = null;
				if (worldOwner.Squad != null)
				{
					dictionary = new Dictionary<string, string>();
					List<SquadMember> memberList = worldOwner.Squad.MemberList;
					int i = 0;
					int count2 = memberList.Count;
					while (i < count2)
					{
						SquadMember squadMember = memberList[i];
						dictionary.Add(squadMember.MemberID, squadMember.MemberName);
						i++;
					}
				}
				IDataController dataController = Service.Get<IDataController>();
				for (int j = 0; j < count; j++)
				{
					SquadDonatedTroop squadDonatedTroop = worldOwnerSquadBuildingTroops[j];
					TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(squadDonatedTroop.TroopUid);
					int totalAmount = squadDonatedTroop.GetTotalAmount();
					if (totalAmount > 0)
					{
						string tooltipString = this.GetTooltipString(troopTypeVO, squadDonatedTroop, dictionary);
						base.AddTroopItem(troopTypeVO, totalAmount, tooltipString);
					}
				}
			}
			if (!this.inVisitMode && worldOwner.Squad != null && SquadUtils.GetDonatedTroopStorageUsedByWorldOwner() < this.buildingInfo.Storage)
			{
				this.AddTroopRequestItem();
			}
			else
			{
				this.requestResendCost = null;
			}
			base.RepositionGridItems();
		}

		private string GetTooltipString(TroopTypeVO troopVO, SquadDonatedTroop donation, Dictionary<string, string> squadMemberLookup)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.lang.Get("SQUAD_DONATED_TROOP", new object[]
			{
				LangUtils.GetTroopDisplayName(troopVO)
			}));
			int num = 0;
			int num2 = 0;
			if (squadMemberLookup != null)
			{
				foreach (KeyValuePair<string, int> current in donation.SenderAmounts)
				{
					string key = current.get_Key();
					int value = current.get_Value();
					string text;
					if (num2 < 3 && squadMemberLookup.TryGetValue(key, out text))
					{
						num2++;
						stringBuilder.Append('\n');
						stringBuilder.Append(this.lang.Get("SQUAD_DONATED_TROOP_SENDER", new object[]
						{
							text,
							value
						}));
					}
					else
					{
						num += value;
					}
				}
				if (num2 > 0 && num > 0)
				{
					stringBuilder.Append('\n');
					stringBuilder.Append(this.lang.Get("SQUAD_DONATED_TROOP_SENDER_OTHER", new object[]
					{
						num
					}));
				}
			}
			return stringBuilder.ToString();
		}

		private void AddTroopRequestItem()
		{
			string itemUid = "troop_request";
			UXElement item = this.storageItemGrid.CloneTemplateItem(itemUid);
			UXElement subElement = this.storageItemGrid.GetSubElement<UXElement>(itemUid, "StorageItemsCardTroop");
			subElement.Visible = false;
			UXLabel subElement2 = this.storageItemGrid.GetSubElement<UXLabel>(itemUid, "LabelBtnRequestTroops");
			subElement2.Text = this.lang.Get("context_RequestTroops", new object[0]);
			UXButton subElement3 = this.storageItemGrid.GetSubElement<UXButton>(itemUid, "StorageItemsCard");
			subElement3.OnClicked = new UXButtonClickedDelegate(this.RequestTroopsClicked);
			this.requestResendCost = this.storageItemGrid.GetSubElement<UXElement>(itemUid, "CostResendRequest");
			this.UpdateRequestState();
			this.storageItemGrid.AddItem(item, 99999999);
		}

		private void RequestTroopsClicked(UXButton btn)
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
			Service.Get<SquadController>().ShowTroopRequestScreen(null, false);
		}

		private void CreateSquadClicked(UXButton btn)
		{
			Service.Get<ScreenController>().AddScreen(new SquadCreateScreen(true));
			this.OnButtonClicked();
		}

		private void JoinSquadClicked(UXButton btn)
		{
			Service.Get<ScreenController>().AddScreen(new SquadJoinScreen());
			this.OnButtonClicked();
		}

		private void OnButtonClicked()
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadEdited, null);
			this.Close(null);
		}

		private void UpdateRequestState()
		{
			if (this.requestResendCost != null)
			{
				uint serverTime = Service.Get<ServerAPI>().ServerTime;
				uint troopRequestDate = Service.Get<SquadController>().StateManager.TroopRequestDate;
				int troopRequestTimeLeft = SquadUtils.GetTroopRequestTimeLeft(serverTime, troopRequestDate);
				bool flag = troopRequestTimeLeft <= 0;
				if (flag)
				{
					this.requestResendCost.Visible = false;
					if (this.observingClockViewTime && !Service.Get<PostBattleRepairController>().IsEntityInRepair(this.selectedBuilding))
					{
						this.observingClockViewTime = false;
						Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
						return;
					}
				}
				else
				{
					int troopRequestCrystalCost = SquadUtils.GetTroopRequestCrystalCost(serverTime, troopRequestDate);
					this.requestResendCost.Visible = true;
					this.requestResendCost.Enabled = GameUtils.CanAffordCrystals(troopRequestCrystalCost);
					UXUtils.SetupCostElements(this, "CostResendRequest", "troop_request", 0, 0, 0, troopRequestCrystalCost, false, null);
					if (!this.observingClockViewTime)
					{
						this.observingClockViewTime = true;
						Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
					}
				}
			}
		}

		public override void OnViewClockTime(float dt)
		{
			this.UpdateRequestState();
			if (Service.Get<PostBattleRepairController>().IsEntityInRepair(this.selectedBuilding))
			{
				this.UpdateHitpoints();
			}
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SquadTroopsReceived)
			{
				this.InitTroopGrid();
				this.UpdateHousingSpace();
				this.UpdateRequestState();
			}
			return base.OnEvent(id, cookie);
		}

		public override void OnDestroyElement()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadTroopsReceived);
			base.OnDestroyElement();
		}

		protected internal SquadBuildingScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).AddTroopRequestItem();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).CreateSquadClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).GetTooltipString((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (SquadDonatedTroop)GCHandledObjects.GCHandleToObject(args[1]), (Dictionary<string, string>)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).InitReputation();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).InitTroopGrid();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).JoinSquadClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnButtonClicked();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnLoaded();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).RequestTroopsClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateHousingSpace();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateReputation(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SquadBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateRequestState();
			return -1L;
		}
	}
}
