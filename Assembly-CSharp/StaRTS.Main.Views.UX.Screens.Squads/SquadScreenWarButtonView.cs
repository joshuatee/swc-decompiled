using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadScreenWarButtonView : AbstractSquadScreenViewModule
	{
		private const string BTN_START_WAR = "BtnStartWar";

		private const string LABEL_BTN_START_WAR = "LabelBtnStartWar";

		private const string SQUAD_WAR_VIEW_WAR = "SQUAD_WAR_VIEW_WAR";

		private const string LABEL_FILTER = "LabelFilterBy";

		private const string LABEL_CHAT_TITLE = "LabelChatTitle";

		private const string WAR_BUTTON_CANCEL_MATCHMAKING = "WAR_BUTTON_CANCEL_MATCHMAKING";

		private const string WAR_BUTTON_START_WAR = "WAR_BUTTON_START_WAR";

		private const string SQUAD_CHAT_TITLE = "SQUAD_CHAT_TITLE";

		private UXButton btnStartWar;

		private UXLabel labelBtnStartWar;

		private UXLabel labelFilterBy;

		private UXLabel labelChatTitle;

		public SquadScreenWarButtonView(SquadSlidingScreen screen) : base(screen)
		{
		}

		public override void OnScreenLoaded()
		{
			this.btnStartWar = this.screen.GetElement<UXButton>("BtnStartWar");
			this.btnStartWar.OnClicked = new UXButtonClickedDelegate(this.OnClickStartWarButton);
			this.labelBtnStartWar = this.screen.GetElement<UXLabel>("LabelBtnStartWar");
			this.labelFilterBy = this.screen.GetElement<UXLabel>("LabelFilterBy");
			this.labelChatTitle = this.screen.GetElement<UXLabel>("LabelChatTitle");
		}

		public override void ShowView()
		{
		}

		public override void HideView()
		{
		}

		public override void RefreshView()
		{
			SquadController squadController = Service.Get<SquadController>();
			SquadScreenState squadScreenState = squadController.StateManager.SquadScreenState;
			SquadRole role = squadController.StateManager.Role;
			bool flag = role == SquadRole.Owner || role == SquadRole.Officer;
			bool flag2 = flag && Service.Get<BuildingLookupController>().GetHighestLevelHQ() > GameConstants.WAR_PARTICIPANT_MIN_LEVEL;
			string squadName = squadController.StateManager.GetCurrentSquad().SquadName;
			this.labelChatTitle.Text = this.lang.Get("SQUAD_CHAT_TITLE", new object[]
			{
				squadName
			});
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (squadScreenState != SquadScreenState.Advancement && currentState is HomeState)
			{
				switch (squadController.WarManager.GetCurrentStatus())
				{
				case SquadWarStatusType.PhaseOpen:
					if (!flag2)
					{
						this.btnStartWar.Visible = false;
						return;
					}
					if (squadController.WarManager.IsCurrentSquadMatchmaking())
					{
						this.btnStartWar.Visible = true;
						this.btnStartWar.SetDefaultColor(1f, 0f, 0f, 1f);
						this.labelBtnStartWar.Text = this.lang.Get("WAR_BUTTON_CANCEL_MATCHMAKING", new object[0]);
					}
					else
					{
						this.btnStartWar.SetDefaultColor(1f, 1f, 1f, 1f);
						this.labelBtnStartWar.Text = this.lang.Get("WAR_BUTTON_START_WAR", new object[0]);
						if (squadController.WarManager.MatchMakingPrepMode)
						{
							this.btnStartWar.Visible = false;
						}
						else
						{
							this.btnStartWar.Visible = squadController.WarManager.CanStartSquadWar();
						}
					}
					break;
				case SquadWarStatusType.PhasePrep:
				case SquadWarStatusType.PhasePrepGrace:
				case SquadWarStatusType.PhaseAction:
				case SquadWarStatusType.PhaseActionGrace:
				case SquadWarStatusType.PhaseCooldown:
					this.btnStartWar.SetDefaultColor(1f, 1f, 1f, 1f);
					this.labelBtnStartWar.Text = this.lang.Get("SQUAD_WAR_VIEW_WAR", new object[0]);
					this.btnStartWar.Visible = true;
					break;
				}
			}
			else
			{
				this.btnStartWar.Visible = false;
			}
			this.labelFilterBy.Visible = !this.btnStartWar.Visible;
		}

		public override void OnDestroyElement()
		{
		}

		private void OnClickStartWarButton(UXButton button)
		{
			SquadController squadController = Service.Get<SquadController>();
			switch (squadController.WarManager.GetCurrentStatus())
			{
			case SquadWarStatusType.PhaseOpen:
				if (squadController.WarManager.IsCurrentSquadMatchmaking())
				{
					this.screen.AnimateClosed(false, null);
					Service.Get<ScreenController>().AddScreen(new SquadWarMatchMakeScreen());
					return;
				}
				this.btnStartWar.Visible = false;
				squadController.WarManager.StartMatchMakingPreparation();
				this.screen.RefreshViews();
				return;
			case SquadWarStatusType.PhasePrep:
			case SquadWarStatusType.PhasePrepGrace:
			case SquadWarStatusType.PhaseAction:
			case SquadWarStatusType.PhaseActionGrace:
			case SquadWarStatusType.PhaseCooldown:
				this.screen.InstantClose(false, null);
				Service.Get<EventManager>().SendEvent(EventId.WarLaunchFlow, null);
				return;
			default:
				return;
			}
		}

		public override bool IsVisible()
		{
			return this.btnStartWar.Visible;
		}

		protected internal SquadScreenWarButtonView(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadScreenWarButtonView)GCHandledObjects.GCHandleToObject(instance)).HideView();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadScreenWarButtonView)GCHandledObjects.GCHandleToObject(instance)).IsVisible());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadScreenWarButtonView)GCHandledObjects.GCHandleToObject(instance)).OnClickStartWarButton((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadScreenWarButtonView)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadScreenWarButtonView)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadScreenWarButtonView)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadScreenWarButtonView)GCHandledObjects.GCHandleToObject(instance)).ShowView();
			return -1L;
		}
	}
}
