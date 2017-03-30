using StaRTS.Externals.BI;
using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

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
			this.guid = string.Empty;
			this.index = 0;
			this.summary = null;
			this.sourceTypeHelper = new QuerySourceTypes();
		}

		public void ShowPage(string guid)
		{
			this.guid = guid;
			this.summary = new VideoSummaryData(this.index, string.Empty, guid, ThumbnailSize.XXLARGE, VideoSummaryStyle.PostView);
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
	}
}
