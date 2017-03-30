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
					Service.Get<Logger>().ErrorFormat("ShareVideo: could not find {0}", new object[]
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
					break;
				case VideoSharingNetwork.GOOGLEPLUS:
					videoSharing.OnVideoDetailsGP(guid);
					break;
				case VideoSharingNetwork.SQUAD:
					videoSharing.OnVideoDetailsSquad(videoData, userMessage);
					break;
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
				Service.Get<Logger>().ErrorFormat("OnVideoDetailsFB: could not find {0}", new object[]
				{
					guid
				});
				return;
			}
			ISocialDataController socialDataController = Service.Get<ISocialDataController>();
			if (socialDataController.IsLoggedIn)
			{
				this.PostFB(videoData);
			}
			else
			{
				this.videoGuid = guid;
				socialDataController.Login(new OnAllDataFetchedDelegate(this.OnFBLoggedIn));
			}
		}

		private void OnFBLoggedIn()
		{
			VideoData videoData = null;
			Service.Get<VideoDataManager>().VideoDatas.TryGetValue(this.videoGuid, out videoData);
			if (videoData == null)
			{
				Service.Get<Logger>().ErrorFormat("OnFBLoggedIn: could not find {0}", new object[]
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
			return string.Format(GameConstants.HOLONET_MAKER_VIDEO_URL, id);
		}

		private void PostFB(VideoData videoData)
		{
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
			}
			else
			{
				EventManager eventManager = Service.Get<EventManager>();
				eventManager.RegisterObserver(this, EventId.GameServicesSignedIn);
				eventManager.RegisterObserver(this, EventId.GameServicesSignedOut);
				GameServicesManager.SignIn();
			}
		}

		private void OnGPLoggedIn()
		{
			VideoData videoData = null;
			Service.Get<VideoDataManager>().VideoDatas.TryGetValue(this.videoGuid, out videoData);
			if (videoData == null)
			{
				Service.Get<Logger>().ErrorFormat("OnGPLoggedIn: could not find {0}", new object[]
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
					if ((int)cookie == 2)
					{
						this.Cleanup();
					}
				}
			}
			else if ((int)cookie == 2)
			{
				this.OnGPLoggedIn();
			}
			return EatResponse.NotEaten;
		}
	}
}
