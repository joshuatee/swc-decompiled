using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class SquadJoinRequestScreen : AbstractSquadRequestScreen
	{
		private const string APPLY_REQUEST = "APPLY_REQUEST";

		private const string APPLY_REQUEST_DEFAULT = "APPLY_REQUEST_DEFAULT";

		private const string SQUAD_JOIN_REQUEST_INFO = "SQUAD_JOIN_REQUEST_INFO";

		private const string ALERT = "ALERT";

		private const string APPLY_LEAVE_SQUAD_ALERT = "APPLY_LEAVE_SQUAD_ALERT";

		private const string APPLY_CTA = "APPLY_CTA";

		private const string ACCOUNT_CONFLICT_CONFIRM_CANCEL = "ACCOUNT_CONFLICT_CONFIRM_CANCEL";

		private Squad squad;

		private SquadController.ActionCallback callback;

		private object cookie;

		public SquadJoinRequestScreen(Squad squad, SquadController.ActionCallback callback, object cookie)
		{
			this.squad = squad;
			this.callback = callback;
			this.cookie = cookie;
		}

		protected override void OnScreenLoaded()
		{
			base.OnScreenLoaded();
			this.requestLabel.Text = this.lang.Get("APPLY_REQUEST", new object[]
			{
				this.squad.SquadName
			});
			this.instructionsLabel.Text = this.lang.Get("SQUAD_JOIN_REQUEST_INFO", new object[0]);
			this.input.InitText(this.lang.Get("APPLY_REQUEST_DEFAULT", new object[0]));
			this.CloseButton.OnClicked = new UXButtonClickedDelegate(this.OnCancel);
		}

		protected override void OnClicked(UXButton button)
		{
			if (!base.CheckForValidInput())
			{
				return;
			}
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad != null)
			{
				YesNoScreen.ShowModal(this.lang.Get("ALERT", new object[0]), this.lang.Get("APPLY_LEAVE_SQUAD_ALERT", new object[]
				{
					this.squad.SquadName,
					currentSquad.SquadName
				}), false, this.lang.Get("APPLY_CTA", new object[0]), this.lang.Get("ACCOUNT_CONFLICT_CONFIRM_CANCEL", new object[0]), new OnScreenModalResult(this.OnLeaveConfirmation), null, false);
				Service.Get<EventManager>().SendEvent(EventId.UISquadLeaveConfirmation, currentSquad.SquadID + "|apply|" + this.squad.SquadID);
				return;
			}
			this.SendRequest();
		}

		private void OnLeaveConfirmation(object result, object cookie)
		{
			if (result != null)
			{
				this.SendRequest();
				return;
			}
			this.OnCancel(null);
		}

		private void SendRequest()
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadEdited, null);
			string text = this.input.Text;
			if (string.IsNullOrEmpty(text))
			{
				text = this.lang.Get("APPLY_REQUEST_DEFAULT", new object[0]);
			}
			SquadMsg message = SquadMsgUtils.CreateApplyToJoinSquadMessage(this.squad.SquadID, text, this.callback, this.cookie);
			Service.Get<SquadController>().TakeAction(message);
			Service.Get<ScreenController>().CloseAll();
		}

		private void OnCancel(UXButton button)
		{
			if (this.callback != null)
			{
				this.callback(false, this.cookie);
			}
			this.OnCloseButtonClicked(button);
		}

		protected internal SquadJoinRequestScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadJoinRequestScreen)GCHandledObjects.GCHandleToObject(instance)).OnCancel((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadJoinRequestScreen)GCHandledObjects.GCHandleToObject(instance)).OnClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadJoinRequestScreen)GCHandledObjects.GCHandleToObject(instance)).OnLeaveConfirmation(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadJoinRequestScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadJoinRequestScreen)GCHandledObjects.GCHandleToObject(instance)).SendRequest();
			return -1L;
		}
	}
}
