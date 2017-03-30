using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Externals.Maker
{
	public class VideosFeatured
	{
		private const int MESSAGE_GRID_LOCATION = 1;

		private DataState dataState;

		private List<VideoSummaryData> featuredVids;

		private string vidSummaryLabel;

		private int numFeatured;

		private QuerySourceTypes sourceTypeHelper;

		public VideosFeatured(int numFeatured, string vidSummaryLabel)
		{
			this.featuredVids = new List<VideoSummaryData>();
			this.dataState = DataState.NotLoaded;
			this.numFeatured = numFeatured;
			this.vidSummaryLabel = vidSummaryLabel;
			this.sourceTypeHelper = new QuerySourceTypes();
		}

		private void CreateMessage(VideoSummaryStyle messageType)
		{
			VideoSummaryData item = new VideoSummaryData(1, this.vidSummaryLabel + "_" + 1, string.Empty, ThumbnailSize.SMALL, messageType);
			KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>> keyValuePair = new KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>>(messageType, new List<VideoSummaryData>
			{
				item
			});
			Service.Get<EventManager>().SendEvent(EventId.UIVideosQueryResponse, keyValuePair);
		}

		private void CreateVideoSummary(List<string> videoGuidList)
		{
			List<VideoSummaryData> list = new List<VideoSummaryData>();
			for (int i = 0; i < this.numFeatured; i++)
			{
				if (i >= videoGuidList.Count)
				{
					break;
				}
				VideoSummaryData item = new VideoSummaryData(i, this.vidSummaryLabel + "_" + (i + 1), videoGuidList[i], ThumbnailSize.XLARGE, VideoSummaryStyle.Featured);
				this.featuredVids.Add(item);
				list.Add(item);
			}
			KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>> keyValuePair = new KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>>(VideoSummaryStyle.Featured, list);
			Service.Get<EventManager>().SendEvent(EventId.UIVideosQueryResponse, keyValuePair);
		}

		private void OnFeaturedQueried(List<string> videoGuidList)
		{
			if (this.dataState != DataState.Loading)
			{
				return;
			}
			this.dataState = DataState.Loaded;
			if (videoGuidList == null)
			{
				this.CreateMessage(VideoSummaryStyle.FeaturedError);
			}
			else if (videoGuidList.Count == 0)
			{
				this.CreateMessage(VideoSummaryStyle.FeaturedEmpty);
			}
			else
			{
				this.CreateVideoSummary(videoGuidList);
				this.sourceTypeHelper.QueryStart();
			}
		}

		public void ShowPage()
		{
			if (this.dataState == DataState.NotLoaded)
			{
				this.dataState = DataState.Loading;
				this.DoFeaturedQuery();
			}
		}

		public void HidePage()
		{
			if (this.dataState == DataState.Loading)
			{
				this.dataState = DataState.NotLoaded;
			}
		}

		public void DoFeaturedQuery()
		{
			Service.Get<EventManager>().SendEvent(EventId.UIVideosFeaturedStart, null);
			Service.Get<VideoDataManager>().GetFeatured(new VideoDataManager.DataListQueryCompleteDelegate(this.OnFeaturedQueried));
		}

		public void Cleanup()
		{
			this.HidePage();
			this.sourceTypeHelper.Active = false;
		}
	}
}
