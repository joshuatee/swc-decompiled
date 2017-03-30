using System;
using UnityEngine;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet
{
	public class HolonetAnimationController
	{
		public const float CLOSE_DURATION = 3f;

		private const string HIDE_HOLONET = "HideHolonet";

		private const string OPEN_TO_INCOMING = "OpenHolonetToIncoming";

		private const string OPEN_TO_COMMAND_CENTER = "OpenHolonetToCC";

		private const string SHOW_COMMAND_CENTER = "ShowCommandCenterTab";

		private const string SHOW_VIDEOS = "ShowVideosTab";

		private const string SHOW_MORE_VIDEOS = "ShowMoreVideos";

		private const string SHOW_DEV_NOTES = "ShowDevNotesTab";

		private const string SHOW_TRANSMISSIONS = "ShowTransmissionsTab";

		private const string SHOW_POST = "ShowPostViewVideo";

		private const string SHOW_BLANK = "ShowBlank";

		private const string BACK_TO_VIDEOS_FROM_POST = "BackToVideosFromPost";

		private const string BACK_TO_VIDEOS_FROM_MORE_VIDEOS = "BackToVideosFromMoreVideos";

		private const string BACK_TO_MORE_VIDEOS_FROM_POST = "BackToMoreVideosFromPost";

		private const string BACK_TO_COMMAND_CENTER_FROM_POST = "BackToCCFromPost";

		private HolonetScreen screen;

		private Animator anim;

		public HolonetAnimationController(HolonetScreen screen)
		{
			this.screen = screen;
			this.anim = this.screen.Root.GetComponent<Animator>();
		}

		public void OpenToIncomingTransmission()
		{
			this.anim.SetTrigger("OpenHolonetToIncoming");
		}

		public void OpenToCommandCenter()
		{
			this.anim.SetTrigger("OpenHolonetToCC");
		}

		public void CloseHolonet()
		{
			this.anim.SetTrigger("HideHolonet");
		}

		public void ShowCommandCenter()
		{
			this.anim.ResetTrigger("ShowBlank");
			this.anim.SetTrigger("ShowCommandCenterTab");
		}

		public void ShowVideos()
		{
			this.anim.ResetTrigger("ShowMoreVideos");
			this.anim.SetTrigger("ShowVideosTab");
		}

		public void ShowDevNotes()
		{
			this.anim.SetTrigger("ShowDevNotesTab");
		}

		public void ShowTransmissionLog()
		{
			this.anim.SetTrigger("ShowTransmissionsTab");
		}

		public void ShowMoreVideos()
		{
			this.anim.SetTrigger("ShowMoreVideos");
		}

		public void ShowBlank()
		{
			this.anim.SetTrigger("ShowBlank");
		}

		public void ShowVideoPostView()
		{
			this.ResetAllTriggers();
			this.anim.SetTrigger("ShowPostViewVideo");
		}

		public void ShowVideosFromPostView()
		{
			this.anim.ResetTrigger("ShowBlank");
			this.anim.SetTrigger("BackToVideosFromPost");
		}

		public void ShowVideosFromMoreVideos()
		{
			this.anim.SetTrigger("BackToVideosFromMoreVideos");
		}

		public void ShowMoreVideosFromPostView()
		{
			this.anim.ResetTrigger("ShowBlank");
			this.anim.SetTrigger("BackToMoreVideosFromPost");
		}

		public void ShowCommandCenterFromPostView()
		{
			this.anim.ResetTrigger("ShowBlank");
			this.anim.SetTrigger("BackToCCFromPost");
		}

		private void ResetAllTriggers()
		{
			this.anim.ResetTrigger("HideHolonet");
			this.anim.ResetTrigger("OpenHolonetToIncoming");
			this.anim.ResetTrigger("OpenHolonetToCC");
			this.anim.ResetTrigger("ShowCommandCenterTab");
			this.anim.ResetTrigger("ShowVideosTab");
			this.anim.ResetTrigger("ShowMoreVideos");
			this.anim.ResetTrigger("ShowDevNotesTab");
			this.anim.ResetTrigger("ShowTransmissionsTab");
			this.anim.ResetTrigger("ShowPostViewVideo");
			this.anim.ResetTrigger("ShowBlank");
			this.anim.ResetTrigger("BackToVideosFromPost");
			this.anim.ResetTrigger("BackToVideosFromMoreVideos");
			this.anim.ResetTrigger("BackToMoreVideosFromPost");
			this.anim.ResetTrigger("BackToCCFromPost");
		}

		public void Cleanup()
		{
			this.screen = null;
			this.anim = null;
		}
	}
}
