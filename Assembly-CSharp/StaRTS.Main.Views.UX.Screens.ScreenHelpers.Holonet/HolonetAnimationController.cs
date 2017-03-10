using System;
using UnityEngine;
using WinRTBridge;

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

		protected internal HolonetAnimationController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).CloseHolonet();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).OpenToCommandCenter();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).OpenToIncomingTransmission();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ResetAllTriggers();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowBlank();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowCommandCenter();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowCommandCenterFromPostView();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowDevNotes();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowMoreVideos();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowMoreVideosFromPostView();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowTransmissionLog();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowVideoPostView();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowVideos();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowVideosFromMoreVideos();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((HolonetAnimationController)GCHandledObjects.GCHandleToObject(instance)).ShowVideosFromPostView();
			return -1L;
		}
	}
}
