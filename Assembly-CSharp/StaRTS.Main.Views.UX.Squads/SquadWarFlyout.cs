using StaRTS.Audio;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Squads
{
	public class SquadWarFlyout : AbstractSquadWarBoardElement
	{
		private SquadWarParticipantState opponentState;

		private SquadWarBuffBaseData buffBaseData;

		private const string SHOW_TOP = "ShowTop";

		private const string SHOW_BOTTOM = "ShowBottom";

		private const string OFF = "Off";

		public const string TOP_OPTION_1_BUTTON = "BtnOption1Top";

		private const string TOP_OPTION_1_LABEL = "LabelOption1Top";

		public const string TOP_OPTION_2_BUTTON = "BtnOption2Top";

		private const string TOP_OPTION_2_LABEL = "LabelOption2Top";

		public const string BOTTOM_OPTION_1_BUTTON = "BtnOption1Bottom";

		private const string BOTTOM_OPTION_1_LABEL = "LabelOption1Bottom";

		public const string BOTTOM_OPTION_2_BUTTON = "BtnOption2Bottom";

		private const string BOTTOM_OPTION_2_LABEL = "LabelOption2Bottom";

		private const string INFO = "context_Info";

		private const string SCOUT = "SCOUT";

		private UXButton scoutMemberButton;

		private UXLabel scoutMemberLabel;

		private UXButton scoutBuffBaseButton;

		private UXLabel scoutBuffBaseLabel;

		public SquadWarFlyout() : base("gui_squadwar_flyout")
		{
		}

		protected override void OnScreenLoaded(object cookie)
		{
			base.OnScreenLoaded(cookie);
			base.InitAnimator();
			Lang lang = Service.Get<Lang>();
			string text = lang.Get("context_Info", new object[0]);
			string text2 = lang.Get("SCOUT", new object[0]);
			this.scoutMemberLabel = base.GetElement<UXLabel>("LabelOption1Top");
			this.scoutMemberLabel.Text = text2;
			this.scoutMemberButton = base.GetElement<UXButton>("BtnOption1Top");
			this.scoutMemberButton.OnClicked = new UXButtonClickedDelegate(this.OnScoutMemberClicked);
			base.GetElement<UXLabel>("LabelOption2Top").Text = text;
			base.GetElement<UXButton>("BtnOption2Top").OnClicked = new UXButtonClickedDelegate(this.OnMemberInfoClicked);
			this.scoutBuffBaseLabel = base.GetElement<UXLabel>("LabelOption1Bottom");
			this.scoutBuffBaseLabel.Text = text2;
			this.scoutBuffBaseButton = base.GetElement<UXButton>("BtnOption1Bottom");
			this.scoutBuffBaseButton.OnClicked = new UXButtonClickedDelegate(this.OnScoutBuffBaseClicked);
			base.GetElement<UXLabel>("LabelOption2Bottom").Text = text;
			base.GetElement<UXButton>("BtnOption2Bottom").OnClicked = new UXButtonClickedDelegate(this.OnBuffBaseInfoClicked);
		}

		public bool IsShowingParticipantOptions(SquadWarParticipantState state)
		{
			return state != null && this.opponentState != null && this.opponentState.SquadMemberId == state.SquadMemberId;
		}

		public void ShowParticipantOptions(GameObject obj, SquadWarParticipantState state)
		{
			if (this.opponentState != null && this.opponentState == state)
			{
				return;
			}
			this.opponentState = state;
			this.buffBaseData = null;
			this.Visible = true;
			this.transformToTrack = obj.transform;
			this.animator.Play("Off", 0, 1f);
			this.animator.ResetTrigger("Off");
			this.animator.ResetTrigger("ShowBottom");
			this.animator.SetTrigger("ShowTop");
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			string empty = string.Empty;
			if (warManager.CanScoutWarMember(this.opponentState.SquadMemberId, ref empty))
			{
				this.scoutMemberButton.VisuallyEnableButton();
				this.scoutMemberLabel.TextColor = this.scoutBuffBaseLabel.OriginalTextColor;
			}
			else
			{
				this.scoutMemberButton.VisuallyDisableButton();
				this.scoutMemberLabel.TextColor = UXUtils.COLOR_LABEL_DISABLED;
			}
			this.PlayShowAudioClip();
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		public bool IsShowingBuffBaseOptions(SquadWarBuffBaseData data)
		{
			return data != null && this.buffBaseData != null && this.buffBaseData.BuffBaseId == data.BuffBaseId;
		}

		public void ShowBuffBaseOptions(UXCheckbox checkbox, SquadWarBuffBaseData data)
		{
			if (this.buffBaseData != null && this.buffBaseData == data)
			{
				return;
			}
			this.buffBaseData = data;
			this.opponentState = null;
			this.Visible = true;
			this.transformToTrack = null;
			Vector3[] worldCorners = checkbox.GetWorldCorners();
			Vector3 position = checkbox.Root.transform.position;
			if (worldCorners != null)
			{
				position.y = worldCorners[0].y;
			}
			this.rootTrans.position = position;
			this.animator.Play("Off", 0, 1f);
			this.animator.ResetTrigger("Off");
			this.animator.ResetTrigger("ShowTop");
			this.animator.SetTrigger("ShowBottom");
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			string empty = string.Empty;
			if (warManager.CanScoutBuffBase(this.buffBaseData, ref empty))
			{
				this.scoutBuffBaseButton.VisuallyEnableButton();
				this.scoutBuffBaseLabel.TextColor = this.scoutBuffBaseLabel.OriginalTextColor;
			}
			else
			{
				this.scoutBuffBaseButton.VisuallyDisableButton();
				this.scoutBuffBaseLabel.TextColor = UXUtils.COLOR_LABEL_DISABLED;
			}
			this.PlayShowAudioClip();
		}

		private void PlayShowAudioClip()
		{
			Service.Get<AudioManager>().PlayAudio("sfx_ui_squadwar_flyout");
		}

		public void Hide()
		{
			this.opponentState = null;
			this.buffBaseData = null;
			if (this.Visible)
			{
				this.animator.ResetTrigger("ShowTop");
				this.animator.ResetTrigger("ShowBottom");
				this.animator.SetTrigger("Off");
			}
			this.Visible = false;
			Service.Get<EventManager>().SendEvent(EventId.WarBoardFlyoutHidden, null);
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public override void Destroy()
		{
			base.Destroy();
			this.animator = null;
			this.opponentState = null;
			this.buffBaseData = null;
		}

		private void OnScoutMemberClicked(UXButton button)
		{
			if (this.opponentState != null)
			{
				SquadWarManager warManager = Service.Get<SquadController>().WarManager;
				string empty = string.Empty;
				if (warManager.CanScoutWarMember(this.opponentState.SquadMemberId, ref empty))
				{
					string squadMemberId = this.opponentState.SquadMemberId;
					if (warManager.ScoutWarMember(squadMemberId))
					{
						this.Hide();
						return;
					}
				}
				else
				{
					Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(empty);
				}
			}
		}

		private void OnScoutBuffBaseClicked(UXButton button)
		{
			if (this.buffBaseData != null)
			{
				SquadWarManager warManager = Service.Get<SquadController>().WarManager;
				string empty = string.Empty;
				if (warManager.CanScoutBuffBase(this.buffBaseData, ref empty))
				{
					string buffBaseId = this.buffBaseData.BuffBaseId;
					if (Service.Get<SquadController>().WarManager.ScoutBuffBase(buffBaseId))
					{
						this.Hide();
						return;
					}
				}
				else
				{
					Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(empty);
				}
			}
		}

		private void OnMemberInfoClicked(UXButton button)
		{
			Service.Get<ScreenController>().AddScreen(new SquadWarPlayerDetailsScreen(this.opponentState));
			this.Hide();
		}

		private void OnBuffBaseInfoClicked(UXButton button)
		{
			Service.Get<ScreenController>().AddScreen(new SquadWarBuffBaseDetails(this.buffBaseData));
			this.Hide();
		}

		private void CloseWarScreen()
		{
			SquadWarScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<SquadWarScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.Close(null);
			}
		}

		public override void OnViewFrameTime(float dt)
		{
			if (this.buffBaseData == null && this.transformToTrack != null)
			{
				base.OnViewFrameTime(dt);
			}
		}

		protected internal SquadWarFlyout(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).CloseWarScreen();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).Hide();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).IsShowingBuffBaseOptions((SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).IsShowingParticipantOptions((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).OnBuffBaseInfoClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).OnMemberInfoClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).OnScoutBuffBaseClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).OnScoutMemberClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).PlayShowAudioClip();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).ShowBuffBaseOptions((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), (SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadWarFlyout)GCHandledObjects.GCHandleToObject(instance)).ShowParticipantOptions((GameObject)GCHandledObjects.GCHandleToObject(*args), (SquadWarParticipantState)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
