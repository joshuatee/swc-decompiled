using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

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
			VideoSummaryData item = new VideoSummaryData(1, this.vidSummaryLabel + "_" + 1, "", ThumbnailSize.SMALL, messageType);
			KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>> keyValuePair = new KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>>(messageType, new List<VideoSummaryData>
			{
				item
			});
			Service.Get<EventManager>().SendEvent(EventId.UIVideosQueryResponse, keyValuePair);
		}

		private void CreateVideoSummary(List<string> videoGuidList)
		{
			List<VideoSummaryData> list = new List<VideoSummaryData>();
			int num = 0;
			while (num < this.numFeatured && num < videoGuidList.Count)
			{
				VideoSummaryData item = new VideoSummaryData(num, this.vidSummaryLabel + "_" + (num + 1), videoGuidList[num], ThumbnailSize.XLARGE, VideoSummaryStyle.Featured);
				this.featuredVids.Add(item);
				list.Add(item);
				num++;
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
				return;
			}
			if (videoGuidList.Count == 0)
			{
				this.CreateMessage(VideoSummaryStyle.FeaturedEmpty);
				return;
			}
			this.CreateVideoSummary(videoGuidList);
			this.sourceTypeHelper.QueryStart();
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

		protected internal VideosFeatured(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideosFeatured)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideosFeatured)GCHandledObjects.GCHandleToObject(instance)).CreateMessage((VideoSummaryStyle)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((VideosFeatured)GCHandledObjects.GCHandleToObject(instance)).CreateVideoSummary((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VideosFeatured)GCHandledObjects.GCHandleToObject(instance)).DoFeaturedQuery();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((VideosFeatured)GCHandledObjects.GCHandleToObject(instance)).HidePage();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((VideosFeatured)GCHandledObjects.GCHandleToObject(instance)).OnFeaturedQueried((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((VideosFeatured)GCHandledObjects.GCHandleToObject(instance)).ShowPage();
			return -1L;
		}
	}
}
