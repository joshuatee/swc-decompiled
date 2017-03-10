using StaRTS.Externals.FacebookApi;
using StaRTS.Externals.GameServices;
using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Maker
{
	public class VideoSharing : IEventObserver
	{
		private const string HN_UI_SHARE_VDEO_FROM_SWC = "hn_ui_share_video_from_swc";

		private string videoGuid;

		public static void ShareVideo(VideoSharingNetwork network, string videoGuid, string userMessage = null)
		{
			Service.Get<VideoDataManager>().GetVideoDetails(videoGuid, delegate(string guid)
			{
				VideoData videoData = null;
				Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData);
				if (videoData == null)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("ShareVideo: could not find {0}", new object[]
					{
						guid
					});
					return;
				}
				VideoSharing videoSharing = new VideoSharing();
				switch (network)
				{
				case VideoSharingNetwork.FACEBOOK:
					videoSharing.OnVideoDetailsFB(guid);
					return;
				case VideoSharingNetwork.GOOGLEPLUS:
					videoSharing.OnVideoDetailsGP(guid);
					return;
				case VideoSharingNetwork.SQUAD:
					videoSharing.OnVideoDetailsSquad(videoData, userMessage);
					return;
				default:
					return;
				}
			});
		}

		private void Cleanup()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.GameServicesSignedIn);
			eventManager.UnregisterObserver(this, EventId.GameServicesSignedOut);
		}

		private void OnVideoDetailsFB(string guid)
		{
			VideoData videoData = null;
			Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData);
			if (videoData == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("OnVideoDetailsFB: could not find {0}", new object[]
				{
					guid
				});
				return;
			}
			ISocialDataController socialDataController = Service.Get<ISocialDataController>();
			if (socialDataController.IsLoggedIn)
			{
				this.PostFB(videoData);
				return;
			}
			this.videoGuid = guid;
			socialDataController.Login(new OnAllDataFetchedDelegate(this.OnFBLoggedIn));
		}

		private void OnFBLoggedIn()
		{
			VideoData videoData = null;
			Service.Get<VideoDataManager>().VideoDatas.TryGetValue(this.videoGuid, out videoData);
			if (videoData == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("OnFBLoggedIn: could not find {0}", new object[]
				{
					this.videoGuid
				});
				return;
			}
			if (Service.Get<ISocialDataController>().IsLoggedIn)
			{
				this.PostFB(videoData);
			}
		}

		private string GetYoutubeLink(string id)
		{
			return string.Format(GameConstants.HOLONET_MAKER_VIDEO_URL, new object[]
			{
				id
			});
		}

		private void PostFB(VideoData videoData)
		{
			FacebookManager.Instance.PostVideoToWall(this.GetYoutubeLink(videoData.YoutubeId), videoData.Title, videoData.Title, videoData.GetThumbnailURL(ThumbnailSize.MEDIUM));
		}

		private void OnVideoDetailsSquad(VideoData videoData, string userMessage)
		{
			SquadMsg message = SquadMsgUtils.CreateShareVideoMessage(videoData.Guid, userMessage);
			Service.Get<SquadController>().TakeAction(message);
		}

		private void OnVideoDetailsGP(string guid)
		{
			this.videoGuid = guid;
			if (GameServicesManager.IsUserAuthenticated())
			{
				this.OnGPLoggedIn();
				return;
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.GameServicesSignedIn);
			eventManager.RegisterObserver(this, EventId.GameServicesSignedOut);
			GameServicesManager.SignIn();
		}

		private void OnGPLoggedIn()
		{
			VideoData videoData = null;
			Service.Get<VideoDataManager>().VideoDatas.TryGetValue(this.videoGuid, out videoData);
			if (videoData == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("OnGPLoggedIn: could not find {0}", new object[]
				{
					this.videoGuid
				});
				return;
			}
			GameServicesManager.Share(Service.Get<Lang>().Get("hn_ui_share_video_from_swc", new object[0]), this.GetYoutubeLink(videoData.YoutubeId), videoData.GetThumbnailURL(ThumbnailSize.MEDIUM));
			this.Cleanup();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.GameServicesSignedIn)
			{
				if (id == EventId.GameServicesSignedOut)
				{
					if ((AccountProvider)cookie == AccountProvider.GOOGLEPLAY)
					{
						this.Cleanup();
					}
				}
			}
			else if ((AccountProvider)cookie == AccountProvider.GOOGLEPLAY)
			{
				this.OnGPLoggedIn();
			}
			return EatResponse.NotEaten;
		}

		public VideoSharing()
		{
		}

		protected internal VideoSharing(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideoSharing)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSharing)GCHandledObjects.GCHandleToObject(instance)).GetYoutubeLink(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSharing)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VideoSharing)GCHandledObjects.GCHandleToObject(instance)).OnFBLoggedIn();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((VideoSharing)GCHandledObjects.GCHandleToObject(instance)).OnGPLoggedIn();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((VideoSharing)GCHandledObjects.GCHandleToObject(instance)).OnVideoDetailsFB(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((VideoSharing)GCHandledObjects.GCHandleToObject(instance)).OnVideoDetailsGP(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((VideoSharing)GCHandledObjects.GCHandleToObject(instance)).OnVideoDetailsSquad((VideoData)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((VideoSharing)GCHandledObjects.GCHandleToObject(instance)).PostFB((VideoData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			VideoSharing.ShareVideo((VideoSharingNetwork)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}
	}
}
