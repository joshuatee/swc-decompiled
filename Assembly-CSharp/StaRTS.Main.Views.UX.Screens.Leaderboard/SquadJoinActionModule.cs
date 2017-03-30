using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.Squads;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Views.UX.Screens.Leaderboard
{
	public class SquadJoinActionModule
	{
		private const string GENERIC_SQUAD_INFO_ISSUE = "GENERIC_SQUAD_INFO_ISSUE";

		private const string JOIN_SQUAD_CONFIRMATION = "JOIN_SQUAD_CONFIRMATION";

		private const string APPLY_TO_SQUAD_CONFIRMATION = "APPLY_TO_SQUAD_CONFIRMATION";

		private const string ALERT = "ALERT";

		private const string JOIN_LEAVE_SQUAD_ALERT = "JOIN_LEAVE_SQUAD_ALERT";

		private const string JOIN_CTA = "JOIN_CTA";

		private const string ACCOUNT_CONFLICT_CONFIRM_CANCEL = "ACCOUNT_CONFLICT_CONFIRM_CANCEL";

		private const string IN_WAR_CANT_LEAVE_SQUAD = "IN_WAR_CANT_LEAVE_SQUAD";

		private Squad squad;

		private ScreenBase screen;

		private UXButton button;

		private string biSource;

		public SquadJoinActionModule(Squad squad, ScreenBase screen, UXButton button)
		{
			this.squad = squad;
			this.screen = screen;
			this.button = button;
			this.biSource = string.Empty;
		}

		public void SetSquad(Squad squad)
		{
			this.squad = squad;
		}

		public void ActualJoinSquad()
		{
			SquadMsg message = SquadMsgUtils.CreateJoinSquadMessage(this.squad.SquadID, this.biSource, new SquadController.ActionCallback(this.OnJoinComplete), null);
			Service.Get<SquadController>().TakeAction(message);
		}

		public void JoinSquad(string biSource)
		{
			if (this.squad == null)
			{
				return;
			}
			Lang lang = Service.Get<Lang>();
			this.biSource = biSource;
			Squad squad = Service.Get<CurrentPlayer>().Squad;
			if (!this.button.VisuallyDisabled)
			{
				this.button.Enabled = false;
				Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
				if (this.squad.InviteType == 1)
				{
					if (squad == null)
					{
						ProcessingScreen.Show();
						this.ActualJoinSquad();
					}
					else if (!SquadUtils.CanLeaveSquad())
					{
						string message = Service.Get<Lang>().Get("IN_WAR_CANT_LEAVE_SQUAD", new object[0]);
						AlertScreen.ShowModal(false, null, message, null, null, true);
					}
					else
					{
						YesNoScreen.ShowModal(lang.Get("ALERT", new object[0]), lang.Get("JOIN_LEAVE_SQUAD_ALERT", new object[]
						{
							squad.SquadName,
							this.squad.SquadName
						}), false, lang.Get("JOIN_CTA", new object[0]), lang.Get("ACCOUNT_CONFLICT_CONFIRM_CANCEL", new object[0]), new OnScreenModalResult(this.OnLeaveAndJoinSquad), this.squad);
						Service.Get<EventManager>().SendEvent(EventId.UISquadLeaveConfirmation, squad.SquadID + "|join|" + this.squad.SquadID);
					}
				}
				else
				{
					Service.Get<ScreenController>().AddScreen(new SquadJoinRequestScreen(this.squad, new SquadController.ActionCallback(this.OnApplyToSquadComplete), this.button));
				}
			}
			else
			{
				string msg;
				SquadUtils.CanCurrentPlayerJoinSquad(Service.Get<CurrentPlayer>(), Service.Get<SquadController>().StateManager.GetCurrentSquad(), this.squad, Service.Get<Lang>(), out msg);
				this.ShowInfoText(msg);
			}
		}

		private void OnJoinComplete(bool success, object cookie)
		{
			ProcessingScreen.Hide();
			if (!this.screen.Visible)
			{
				return;
			}
			if (success)
			{
				this.OnSquadJoined();
			}
			else
			{
				this.button.Enabled = true;
			}
		}

		private void OnApplyToSquadComplete(bool success, object cookie)
		{
			if (success)
			{
				string msg = Service.Get<Lang>().Get("APPLY_TO_SQUAD_CONFIRMATION", new object[]
				{
					this.squad.SquadName
				});
				this.ShowInfoText(msg);
			}
			else
			{
				this.button.Enabled = true;
			}
		}

		public void OnSquadJoined()
		{
			string msg = Service.Get<Lang>().Get("JOIN_SQUAD_CONFIRMATION", new object[]
			{
				this.squad.SquadName
			});
			this.ShowInfoText(msg);
			this.screen.Close(null);
			HUD hUD = Service.Get<UXController>().HUD;
			hUD.RefreshPlayerSocialInformation();
			hUD.UpdateSquadJewelCount();
		}

		private void OnLeaveAndJoinSquad(object result, object cookie)
		{
			if (result != null)
			{
				ProcessingScreen.Show();
				SquadMsg message = SquadMsgUtils.CreateLeaveSquadMessage(new SquadController.ActionCallback(this.OnLeaveComplete), null);
				Service.Get<SquadController>().TakeAction(message);
			}
			else
			{
				this.button.Enabled = true;
			}
		}

		private void OnLeaveComplete(bool success, object cookie)
		{
			if (success)
			{
				this.ActualJoinSquad();
			}
			else
			{
				this.screen.Close(null);
			}
		}

		private void ShowInfoText(string msg)
		{
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(msg, 1f, 5f);
		}
	}
}
