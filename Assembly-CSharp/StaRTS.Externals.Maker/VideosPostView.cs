using StaRTS.Externals.BI;
using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Maker
{
	public class VideosPostView
	{
		private string guid;

		private int index;

		private VideoSummaryData summary;

		private QuerySourceTypes sourceTypeHelper;

		public VideosPostView()
		{
			this.guid = "";
			this.index = 0;
			this.summary = null;
			this.sourceTypeHelper = new QuerySourceTypes();
		}

		public void ShowPage(string guid)
		{
			this.guid = guid;
			this.summary = new VideoSummaryData(this.index, "", guid, ThumbnailSize.XXLARGE, VideoSummaryStyle.PostView);
			Service.Get<VideoDataManager>().GetFeed(new VideoDataManager.DataListQueryCompleteDelegate(this.OnSearchQueried));
		}

		private void OnSearchQueried(List<string> videoGuidList)
		{
			if (videoGuidList != null && videoGuidList.Contains(this.guid))
			{
				KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>> keyValuePair = new KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>>(VideoSummaryStyle.PostView, new List<VideoSummaryData>
				{
					this.summary
				});
				Service.Get<EventManager>().SendEvent(EventId.UIVideosQueryResponse, keyValuePair);
				this.sourceTypeHelper.QueryStart();
			}
		}

		public void ShareToSquad()
		{
			VideoSharing.ShareVideo(VideoSharingNetwork.SQUAD, this.guid, null);
			this.TrackVideoSharing(VideoSharingNetwork.SQUAD);
		}

		public void ShareToFacebook()
		{
			VideoSharing.ShareVideo(VideoSharingNetwork.FACEBOOK, this.guid, null);
			this.TrackVideoSharing(VideoSharingNetwork.FACEBOOK);
		}

		public void ShareToGoogle()
		{
			VideoSharing.ShareVideo(VideoSharingNetwork.GOOGLEPLUS, this.guid, null);
			this.TrackVideoSharing(VideoSharingNetwork.GOOGLEPLUS);
		}

		public void Cleanup()
		{
			if (this.summary != null)
			{
				this.summary.Cleanup();
				this.summary = null;
			}
		}

		private void TrackVideoSharing(VideoSharingNetwork network)
		{
			Service.Get<BILoggingController>().TrackVideoSharing(network, "postwatch", this.guid);
		}

		protected internal VideosPostView(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideosPostView)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideosPostView)GCHandledObjects.GCHandleToObject(instance)).OnSearchQueried((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((VideosPostView)GCHandledObjects.GCHandleToObject(instance)).ShareToFacebook();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VideosPostView)GCHandledObjects.GCHandleToObject(instance)).ShareToGoogle();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((VideosPostView)GCHandledObjects.GCHandleToObject(instance)).ShareToSquad();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((VideosPostView)GCHandledObjects.GCHandleToObject(instance)).ShowPage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((VideosPostView)GCHandledObjects.GCHandleToObject(instance)).TrackVideoSharing((VideoSharingNetwork)(*(int*)args));
			return -1L;
		}
	}
}
