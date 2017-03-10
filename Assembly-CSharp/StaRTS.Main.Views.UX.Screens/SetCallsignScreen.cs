using StaRTS.Audio;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class SetCallsignScreen : ScreenBase
	{
		private const string NEXT_BUTTON = "BtnPrimary";

		private const string INPUT_TEXT_FIELD = "LabelInput";

		private const string TITLE_LABEL = "LabelTitle";

		private const string BODY_LABEL = "LabelBody";

		private const string INPUT_BACKGROUND = "InputField";

		private const string ERROR_LABEL = "LabelError";

		private const string NEXT_BUTTON_LABEL = "LabelBtnPrimary";

		private const uint ERROR_TOO_SHORT = 9999u;

		private const uint ERROR_RESERVED_WORD = 1000u;

		private UXInput inputTextField;

		private UIInput inputScript;

		private UXSprite inputBackground;

		private UXLabel errorLabel;

		private UXButton nextButton;

		private bool successfullySetUserName;

		private bool isAuthenticating;

		private bool hasTypedAnything;

		private bool doBackendAuthentication;

		protected override bool AllowGarbageCollection
		{
			get
			{
				return false;
			}
		}

		public SetCallsignScreen(bool doBackendAuthentication) : base("gui_call_sign")
		{
			this.doBackendAuthentication = doBackendAuthentication;
			this.successfullySetUserName = false;
			this.isAuthenticating = false;
			this.hasTypedAnything = false;
		}

		protected override void OnScreenLoaded()
		{
			this.errorLabel = base.GetElement<UXLabel>("LabelError");
			this.errorLabel.Visible = false;
			this.nextButton = base.GetElement<UXButton>("BtnPrimary");
			this.nextButton.OnClicked = new UXButtonClickedDelegate(this.OnNextButton);
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			if (faction != FactionType.Empire)
			{
				if (faction == FactionType.Rebel)
				{
					base.GetElement<UXLabel>("LabelTitle").Text = this.lang.Get("CALL_SIGN_TITLE_REBEL", new object[0]);
				}
			}
			else
			{
				base.GetElement<UXLabel>("LabelTitle").Text = this.lang.Get("CALL_SIGN_TITLE_EMPIRE", new object[0]);
			}
			base.GetElement<UXLabel>("LabelBody").Text = this.lang.Get("CALL_SIGN_DESCRIPTION", new object[0]);
			base.GetElement<UXLabel>("LabelBtnPrimary").Text = this.lang.Get("s_Confirm", new object[0]);
			this.inputBackground = base.GetElement<UXSprite>("InputField");
			this.inputTextField = base.GetElement<UXInput>("LabelInput");
			this.inputTextField.InitText(this.lang.Get("CALL_SIGN_TYPE_HERE", new object[0]));
			this.inputScript = this.inputTextField.GetUIInputComponent();
			this.inputScript.onValidate = new UIInput.OnValidate(LangUtils.OnValidate);
			this.inputScript.characterLimit = GameConstants.USER_NAME_MAX_CHARACTERS;
			this.inputScript.label.maxLineCount = 1;
			EventDelegate item = new EventDelegate(new EventDelegate.Callback(this.OnChange));
			this.inputScript.onChange.Add(item);
			this.inputTextField.Text = "";
		}

		private void OnSubmit()
		{
			if (!this.successfullySetUserName && this.hasTypedAnything && !string.IsNullOrEmpty(this.inputTextField.Text))
			{
				if (!this.ValidateFinalString(this.inputTextField.Text))
				{
					return;
				}
				Service.Get<AudioManager>().PlayAudio("sfx_button_next");
				if (GameConstants.SET_CALLSIGN_CONFIRM_SCREEN_ENABLED)
				{
					YesNoScreen.ShowModal(this.lang.Get("CALL_SIGN_CONFIRMATION_TITLE", new object[0]), this.lang.Get("CALL_SIGN_CONFIRMATION_DESCRIPTION", new object[]
					{
						this.inputTextField.Text
					}), false, true, new OnScreenModalResult(this.OnCloseConfrimScreen), null, false);
					return;
				}
				this.SetNameFinalAndClose();
			}
		}

		private void OnCloseConfrimScreen(object result, object cookie)
		{
			if (result != null)
			{
				this.SetNameFinalAndClose();
				return;
			}
			this.inputTextField.Text = "";
			this.successfullySetUserName = false;
			this.hasTypedAnything = false;
		}

		private void SetNameFinalAndClose()
		{
			if (this.doBackendAuthentication)
			{
				this.isAuthenticating = true;
				SetPlayerNameCommand command = new SetPlayerNameCommand(this.inputTextField.Text);
				Service.Get<ServerAPI>().Sync(command);
				this.inputScript.enabled = false;
				this.nextButton.Enabled = false;
				this.inputScript.isSelected = false;
				this.OnSuccess(null, null);
				this.Close(null);
				return;
			}
			this.OnSuccess(null, null);
			this.Close(null);
		}

		private bool ValidateFinalString(string input)
		{
			input = input.Trim();
			char[] sPECIAL_CHARS = ProfanityController.SPECIAL_CHARS;
			for (int i = 0; i < sPECIAL_CHARS.Length; i++)
			{
				char c = sPECIAL_CHARS[i];
				string text = c.ToString() + c.ToString();
				while (input.Contains(text))
				{
					input = input.Replace(text, c.ToString());
				}
			}
			this.inputTextField.Text = input;
			if (!Service.Get<ProfanityController>().IsValid(input, true))
			{
				this.OnFailure(1000u, null);
				return false;
			}
			if (input.get_Length() < GameConstants.USER_NAME_MIN_CHARACTERS)
			{
				this.OnFailure(9999u, null);
				return false;
			}
			return true;
		}

		private void OnChange()
		{
			this.hasTypedAnything = true;
			if (this.inputScript.isSelected)
			{
				this.errorLabel.Visible = false;
			}
		}

		private void OnNextButton(UXButton button)
		{
			if (this.successfullySetUserName)
			{
				this.Close(null);
				return;
			}
			if (!this.isAuthenticating)
			{
				this.OnSubmit();
			}
		}

		private void OnFailure(uint status, object cookie)
		{
			this.isAuthenticating = false;
			string text;
			if (status == 9999u)
			{
				text = this.lang.Get("CALL_SIGN_ERROR_TOO_SHORT", new object[]
				{
					GameConstants.USER_NAME_MIN_CHARACTERS
				});
			}
			else
			{
				text = this.lang.Get("CALL_SIGN_ERROR_INVALID", new object[0]);
				this.inputTextField.Text = "";
			}
			this.errorLabel.Text = text;
			this.errorLabel.Visible = true;
			this.inputScript.isSelected = false;
			this.inputScript.enabled = true;
			this.nextButton.Enabled = true;
		}

		private void OnSuccess(DefaultResponse response, object cookie)
		{
			this.isAuthenticating = false;
			this.successfullySetUserName = true;
			Service.Get<CurrentPlayer>().PlayerName = this.inputTextField.Text;
			this.inputBackground.Color = Color.green;
			this.nextButton.Enabled = true;
		}

		protected internal SetCallsignScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SetCallsignScreen)GCHandledObjects.GCHandleToObject(instance)).AllowGarbageCollection);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SetCallsignScreen)GCHandledObjects.GCHandleToObject(instance)).OnChange();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SetCallsignScreen)GCHandledObjects.GCHandleToObject(instance)).OnCloseConfrimScreen(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SetCallsignScreen)GCHandledObjects.GCHandleToObject(instance)).OnNextButton((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SetCallsignScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SetCallsignScreen)GCHandledObjects.GCHandleToObject(instance)).OnSubmit();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SetCallsignScreen)GCHandledObjects.GCHandleToObject(instance)).OnSuccess((DefaultResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SetCallsignScreen)GCHandledObjects.GCHandleToObject(instance)).SetNameFinalAndClose();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SetCallsignScreen)GCHandledObjects.GCHandleToObject(instance)).ValidateFinalString(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}
