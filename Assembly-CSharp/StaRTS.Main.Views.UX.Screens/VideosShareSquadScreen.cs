using StaRTS.Externals.BI;
using StaRTS.Externals.Maker;
using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Controllers;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using UnityEngine;

namespace StaRTS.Main.Views.UX.Screens
{
	public class VideosShareSquadScreen : ClosableScreen, IUserInputObserver
	{
		private const string SHARE_CLOSE_BUTTON = "MakerButtonCloseShare";

		private const string SHARE_SEND_BUTTON = "MakerButtonSendShare";

		private const string SHARE_SEND_BUTTON_LABEL = "MakerLabelSendShare";

		private const string SHARE_TITLE = "MakerLabelTitleShare";

		private const string SHARE_INPUT = "MakerLabelInputMessageShare";

		private const string SHARE_AUTHOR = "MakerLabelVideoCreatorShare";

		private const string SHARE_VIDEO_TITLE = "MakerLabelVideoTitleShare";

		private const string INVALID_TEXT = "INVALID_TEXT";

		private const string BUTTON_SEND = "BUTTON_SEND";

		private const string HN_UI_SQUAD_SHARE = "hn_ui_squad_share";

		private const string HN_UI_SHARE_TEXT = "hn_ui_share_text";

		private const string HN_UI_AUTHOR = "hn_ui_author";

		protected UIInput nguiInputBox;

		protected UXInput inputBox;

		protected UXLabel requestLabel;

		protected UXButton sendButton;

		private string videoId;

		private string sharingSource;

		public VideosShareSquadScreen(string videoId, string sharingSource) : base("gui_maker_share")
		{
			this.videoId = videoId;
			this.sharingSource = sharingSource;
		}

		protected override void OnScreenLoaded()
		{
			this.CloseButton = base.GetElement<UXButton>("MakerButtonCloseShare");
			this.CloseButton.OnClicked = new UXButtonClickedDelegate(this.OnCloseButtonClicked);
			this.CloseButton.Enabled = true;
			base.CurrentBackButton = this.CloseButton;
			this.sendButton = base.GetElement<UXButton>("MakerButtonSendShare");
			this.sendButton.OnClicked = new UXButtonClickedDelegate(this.SendRequestClicked);
			UXLabel element = base.GetElement<UXLabel>("MakerLabelSendShare");
			element.Text = this.lang.Get("BUTTON_SEND", new object[0]);
			this.requestLabel = base.GetElement<UXLabel>("MakerLabelTitleShare");
			this.requestLabel.Text = this.lang.Get("hn_ui_squad_share", new object[0]);
			this.inputBox = base.GetElement<UXInput>("MakerLabelInputMessageShare");
			this.inputBox.InitText(this.lang.Get("hn_ui_share_text", new object[0]));
			this.nguiInputBox = this.inputBox.GetUIInputComponent();
			if (this.nguiInputBox != null)
			{
				this.nguiInputBox.onValidate = new UIInput.OnValidate(LangUtils.OnValidateWNewLines);
				EventDelegate item = new EventDelegate(new EventDelegate.Callback(this.OnSubmit));
				this.nguiInputBox.onSubmit.Add(item);
				this.nguiInputBox.enabled = true;
			}
			base.GetElement<UXLabel>("MakerLabelVideoCreatorShare").Text = string.Empty;
			base.GetElement<UXLabel>("MakerLabelVideoTitleShare").Text = string.Empty;
			Service.Get<UserInputManager>().RegisterObserver(this, UserInputLayer.Screen);
			Service.Get<VideoDataManager>().GetVideoDetails(this.videoId, delegate(string guid)
			{
				VideoData videoData = Service.Get<VideoDataManager>().VideoDatas[guid];
				if (videoData == null)
				{
					return;
				}
				base.GetElement<UXLabel>("MakerLabelVideoCreatorShare").Text = ((!string.IsNullOrEmpty(videoData.Author)) ? this.lang.Get("hn_ui_author", new object[]
				{
					videoData.Author
				}) : string.Empty);
				base.GetElement<UXLabel>("MakerLabelVideoTitleShare").Text = videoData.Title;
			});
		}

		protected void OnSubmit()
		{
			if (this.nguiInputBox != null)
			{
				this.nguiInputBox.enabled = true;
				this.nguiInputBox.isSelected = false;
			}
		}

		protected void LabelSelected()
		{
			if (!TouchScreenKeyboard.visible)
			{
				Service.Get<Logger>().Debug(">>>>> Inputs SquadRequestScreen : Selected keyboard missing");
				TouchScreenKeyboard.Open(this.requestLabel.Text, TouchScreenKeyboardType.Default, false, true, false);
			}
		}

		protected bool CheckForValidInput()
		{
			if (!Service.Get<ProfanityController>().IsValid(this.inputBox.Text, false))
			{
				AlertScreen.ShowModal(false, null, this.lang.Get("INVALID_TEXT", new object[0]), null, null);
				return false;
			}
			return true;
		}

		public EatResponse OnPress(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			return EatResponse.NotEaten;
		}

		public EatResponse OnDrag(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			return EatResponse.NotEaten;
		}

		public EatResponse OnRelease(int id)
		{
			if (this.nguiInputBox != null && this.nguiInputBox.isSelected)
			{
				this.LabelSelected();
			}
			else
			{
				UIInput[] array = NGUITools.FindActive<UIInput>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].isSelected)
					{
						this.LabelSelected();
						break;
					}
				}
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnScroll(float delta, Vector2 screenPosition)
		{
			return EatResponse.NotEaten;
		}

		public override void OnDestroyElement()
		{
			Service.Get<UserInputManager>().UnregisterObserver(this, UserInputLayer.Screen);
			base.OnDestroyElement();
		}

		protected virtual void SendRequestClicked(UXButton btn)
		{
			if (!this.CheckForValidInput())
			{
				return;
			}
			string text = this.inputBox.Text;
			if (string.IsNullOrEmpty(text))
			{
				text = this.lang.Get("hn_ui_share_text", new object[0]);
			}
			VideoSharing.ShareVideo(VideoSharingNetwork.SQUAD, this.videoId, text);
			Service.Get<BILoggingController>().TrackVideoSharing(VideoSharingNetwork.SQUAD, this.sharingSource, this.videoId);
			this.Close(null);
		}

		public override void Close(object modalResult)
		{
			Service.Get<UserInputManager>().UnregisterObserver(this, UserInputLayer.Screen);
			base.Close(modalResult);
		}
	}
}
