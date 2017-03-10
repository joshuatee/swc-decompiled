using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Models;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.Maker.Player
{
	public class VideoPlayer
	{
		private const string AD_TAG_URL = "";

		private const string DEFAULT_AD_LANGUAGE = "en";

		private const string LOADING_TITLE_ID = "hn_makerloading_title";

		private const string LOADING_NOTE_ID = "hn_makerloading_makernote";

		private const string DONE_BUTTON_TITLE_ID = "hn_done";

		private const int VIDEO_QUALITY_LIMIT = 1024;

		private static IVideoPlayerHelper helper = null;

		private static string action = "";

		private static string videoId = "";

		public static void EnableAds(bool enable)
		{
			if (!VideoPlayer.Init())
			{
				return;
			}
			VideoPlayer.helper.EnableAds(enable);
		}

		public static void Play(string videoGuid, string action)
		{
			if (!VideoPlayer.Init())
			{
				return;
			}
			if (VideoPlayer.helper == null)
			{
				return;
			}
			if (!GameConstants.IsMakerVideoEnabled())
			{
				return;
			}
			if (VideoPlayerKeepAlive.Instance.IsDisplayed())
			{
				return;
			}
			VideoPlayer.action = action;
			VideoPlayer.videoId = videoGuid;
			VideoPlayerKeepAlive.Instance.Begin(videoGuid);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.SendEvent(EventId.UIVideosViewBegin, null);
			Service.Get<VideoDataManager>().GetAllEnvironmentVideos(new VideoDataManager.DataListQueryCompleteDelegate(VideoPlayer.OnEnvironmentVideos));
		}

		private static void OnEnvironmentVideos(List<string> videoGuidList)
		{
			string guid = VideoPlayerKeepAlive.Instance.Guid;
			if (videoGuidList.Contains(guid))
			{
				Service.Get<VideoDataManager>().GetVideoDetails(guid, new VideoDataManager.DataQueryCompleteDelegate(VideoPlayer.OnVideoDetails));
				return;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("Could not play non-production video {0}", new object[]
			{
				guid
			});
			VideoPlayer.AbandonPlayback();
		}

		private static void OnVideoDetails(string guid)
		{
			if (guid == null)
			{
				VideoPlayer.CleanupPlayback();
				return;
			}
			VideoData videoData;
			if (!Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData))
			{
				VideoPlayer.CleanupPlayback();
				return;
			}
			if (videoData == null)
			{
				VideoPlayer.CleanupPlayback();
				return;
			}
			Service.Get<VideoDataManager>().SearchSubCategory("official", new VideoDataManager.DataListQueryCompleteDelegate(VideoPlayer.OnOfficialVideoList));
		}

		private static void AbandonPlayback()
		{
			VideoPlayerKeepAlive.Instance.AbandonPlayback();
		}

		private static void CleanupPlayback()
		{
			VideoPlayerKeepAlive.Instance.EndPlayback("0");
		}

		private static void OnOfficialVideoList(List<string> videos)
		{
			if (!GameConstants.IsMakerVideoEnabled())
			{
				return;
			}
			string guid = VideoPlayerKeepAlive.Instance.Guid;
			VideoData videoData;
			if (!Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData))
			{
				VideoPlayer.CleanupPlayback();
				return;
			}
			int num = Math.Max(Screen.currentResolution.width, Screen.currentResolution.height);
			VideoQuality quality = (num <= 1024) ? VideoQuality.LOW : VideoQuality.HIGH;
			string videoURL = videoData.GetVideoURL(quality);
			bool isOfficial = videos != null && videos.Contains(guid);
			VideoPlayer.helper.Play(videoURL, isOfficial, VideoPlayer.videoId, VideoPlayer.action);
		}

		private static bool Init()
		{
			if (VideoPlayer.helper != null)
			{
				return true;
			}
			VideoPlayer.helper.SetAdLanguage("en");
			VideoPlayer.helper.SetLoadingText(Service.Get<Lang>().Get("hn_makerloading_title", new object[0]) + "\\n\\n" + Service.Get<Lang>().Get("hn_makerloading_makernote", new object[0]));
			VideoPlayer.helper.SetLoadingTextTime(GameConstants.MIN_MAKER_VID_LOAD);
			VideoPlayer.helper.SetDoneButtonText(Service.Get<Lang>().Get("hn_done", new object[0]));
			return true;
		}

		public VideoPlayer()
		{
		}

		protected internal VideoPlayer(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			VideoPlayer.AbandonPlayback();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			VideoPlayer.CleanupPlayback();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			VideoPlayer.EnableAds(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(VideoPlayer.Init());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			VideoPlayer.OnEnvironmentVideos((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			VideoPlayer.OnOfficialVideoList((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			VideoPlayer.OnVideoDetails(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			VideoPlayer.Play(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}
