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
using System.Runtime.InteropServices;
using WinRTBridge;

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
			if (this.button.VisuallyDisabled)
			{
				string msg;
				SquadUtils.CanCurrentPlayerJoinSquad(Service.Get<CurrentPlayer>(), Service.Get<SquadController>().StateManager.GetCurrentSquad(), this.squad, Service.Get<Lang>(), out msg);
				this.ShowInfoText(msg);
				return;
			}
			this.button.Enabled = false;
			Service.Get<EventManager>().SendEvent(EventId.SquadNext, null);
			if (this.squad.InviteType != 1)
			{
				Service.Get<ScreenController>().AddScreen(new SquadJoinRequestScreen(this.squad, new SquadController.ActionCallback(this.OnApplyToSquadComplete), this.button));
				return;
			}
			if (squad == null)
			{
				ProcessingScreen.Show();
				this.ActualJoinSquad();
				return;
			}
			if (!SquadUtils.CanLeaveSquad())
			{
				string message = Service.Get<Lang>().Get("IN_WAR_CANT_LEAVE_SQUAD", new object[0]);
				AlertScreen.ShowModal(false, null, message, null, null, true);
				return;
			}
			YesNoScreen.ShowModal(lang.Get("ALERT", new object[0]), lang.Get("JOIN_LEAVE_SQUAD_ALERT", new object[]
			{
				squad.SquadName,
				this.squad.SquadName
			}), false, lang.Get("JOIN_CTA", new object[0]), lang.Get("ACCOUNT_CONFLICT_CONFIRM_CANCEL", new object[0]), new OnScreenModalResult(this.OnLeaveAndJoinSquad), this.squad, false);
			Service.Get<EventManager>().SendEvent(EventId.UISquadLeaveConfirmation, squad.SquadID + "|join|" + this.squad.SquadID);
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
				return;
			}
			this.button.Enabled = true;
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
				return;
			}
			this.button.Enabled = true;
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
				return;
			}
			this.button.Enabled = true;
		}

		private void OnLeaveComplete(bool success, object cookie)
		{
			if (success)
			{
				this.ActualJoinSquad();
				return;
			}
			this.screen.Close(null);
		}

		private void ShowInfoText(string msg)
		{
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(msg, 1f, 5f);
		}

		protected internal SquadJoinActionModule(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadJoinActionModule)GCHandledObjects.GCHandleToObject(instance)).ActualJoinSquad();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadJoinActionModule)GCHandledObjects.GCHandleToObject(instance)).JoinSquad(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadJoinActionModule)GCHandledObjects.GCHandleToObject(instance)).OnApplyToSquadComplete(*(sbyte*)args != 0, GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadJoinActionModule)GCHandledObjects.GCHandleToObject(instance)).OnJoinComplete(*(sbyte*)args != 0, GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadJoinActionModule)GCHandledObjects.GCHandleToObject(instance)).OnLeaveAndJoinSquad(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadJoinActionModule)GCHandledObjects.GCHandleToObject(instance)).OnLeaveComplete(*(sbyte*)args != 0, GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadJoinActionModule)GCHandledObjects.GCHandleToObject(instance)).OnSquadJoined();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadJoinActionModule)GCHandledObjects.GCHandleToObject(instance)).SetSquad((Squad)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadJoinActionModule)GCHandledObjects.GCHandleToObject(instance)).ShowInfoText(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
