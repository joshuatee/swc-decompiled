using StaRTS.Externals.Maker;
using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Views.UX.Controls
{
	public class VideoSummary : IEventObserver
	{
		private string videoId;

		private string uiName;

		private VideoSummaryStyle style;

		private ScreenBase parentScreen;

		private bool cleanedUp;

		private QuerySourceTypes sourceTypeHelper;

		public VideoSummaryDisplay summaryDisplay
		{
			get;
			set;
		}

		public VideoSummaryData summaryData
		{
			get;
			set;
		}

		public ThumbnailSize thumbSize
		{
			get;
			set;
		}

		public string UIName
		{
			get
			{
				return this.uiName;
			}
		}

		public VideoSummary(string videoId, string uiName, VideoSummaryStyle style, ScreenBase parentScreen)
		{
			this.videoId = videoId;
			this.uiName = uiName;
			this.style = style;
			this.parentScreen = parentScreen;
			this.summaryDisplay = null;
			this.summaryData = null;
			this.cleanedUp = false;
			this.thumbSize = ThumbnailSize.SMALL;
			this.sourceTypeHelper = new QuerySourceTypes();
		}

		private void SetSummary(VideoSummaryData data, VideoSummaryDisplay display)
		{
			this.summaryData = data;
			this.summaryDisplay = display;
		}

		public void PopulateDisplay()
		{
			if (this.summaryDisplay == null)
			{
				VideoSummaryData videoSummaryData = new VideoSummaryData(0, this.uiName, this.videoId, this.thumbSize, this.style);
				VideoSummaryDisplay display = new VideoSummaryDisplay(this.parentScreen, videoSummaryData);
				this.SetSummary(videoSummaryData, display);
				Service.Get<EventManager>().RegisterObserver(this, EventId.UIVideosRefresh, EventPriority.Default);
				Service.Get<EventManager>().RegisterObserver(this, EventId.UIVideosSourceTypeResponse, EventPriority.Default);
			}
			this.QueryStart();
		}

		private void QueryStart()
		{
			this.summaryDisplay.ShowLoading();
			this.QueryVideoData();
		}

		private void SwitchToErrorMessage()
		{
			if (this.summaryData.Style == VideoSummaryStyle.Holonet)
			{
				this.summaryData.Style = VideoSummaryStyle.HolonetError;
			}
			else if (this.summaryData.Style == VideoSummaryStyle.HolonetFeatured)
			{
				this.summaryData.Style = VideoSummaryStyle.HolonetFeaturedError;
			}
			else if (this.summaryData.Style == VideoSummaryStyle.SquadChat)
			{
				this.summaryData.Style = VideoSummaryStyle.SquadChatError;
			}
		}

		private void SwitchToNoResultsMessage()
		{
			if (this.summaryData.Style == VideoSummaryStyle.Holonet)
			{
				this.summaryData.Style = VideoSummaryStyle.HolonetEmpty;
			}
			else if (this.summaryData.Style == VideoSummaryStyle.HolonetFeatured)
			{
				this.summaryData.Style = VideoSummaryStyle.HolonetFeaturedEmpty;
			}
			else if (this.summaryData.Style == VideoSummaryStyle.SquadChat)
			{
				this.summaryData.Style = VideoSummaryStyle.SquadChatEmpty;
			}
		}

		private void SwitchToSummaryDisplay()
		{
			if (this.summaryData.Style == VideoSummaryStyle.HolonetEmpty || this.summaryData.Style == VideoSummaryStyle.HolonetError)
			{
				this.summaryData.Style = VideoSummaryStyle.Holonet;
			}
			else if (this.summaryData.Style == VideoSummaryStyle.HolonetFeaturedEmpty || this.summaryData.Style == VideoSummaryStyle.HolonetFeaturedError)
			{
				this.summaryData.Style = VideoSummaryStyle.HolonetFeatured;
			}
			else if (this.summaryData.Style == VideoSummaryStyle.SquadChatEmpty || this.summaryData.Style == VideoSummaryStyle.SquadChatError)
			{
				this.summaryData.Style = VideoSummaryStyle.SquadChat;
			}
		}

		private void OnTagQueried(List<string> videoGuidList)
		{
			if (this.cleanedUp)
			{
				return;
			}
			if (this.summaryDisplay == null || this.summaryData == null)
			{
				Service.Get<Logger>().Error("OnTagQueried called before VideoSummary set up");
				return;
			}
			if (videoGuidList == null)
			{
				this.SwitchToErrorMessage();
				this.summaryDisplay.GenerateSummary();
			}
			else if (videoGuidList.Count == 0)
			{
				this.SwitchToNoResultsMessage();
				this.summaryDisplay.GenerateSummary();
			}
			else
			{
				this.SwitchToSummaryDisplay();
				this.videoId = videoGuidList[0];
				this.summaryData.Guid = this.videoId;
				this.QueryVideoData();
			}
		}

		private void QueryVideoData()
		{
			if (string.IsNullOrEmpty(this.videoId))
			{
				Service.Get<VideoDataManager>().GetFeatured(new VideoDataManager.DataListQueryCompleteDelegate(this.OnSearchQueried));
			}
			else
			{
				Service.Get<VideoDataManager>().GetAllEnvironmentVideos(new VideoDataManager.DataListQueryCompleteDelegate(this.OnSearchQueried));
			}
		}

		private void OnSearchQueried(List<string> videoGuidList)
		{
			if (this.cleanedUp)
			{
				return;
			}
			if (this.summaryDisplay == null || this.summaryData == null)
			{
				Service.Get<Logger>().Error("OnSearchQueried called before VideoSummary set up");
				return;
			}
			if (videoGuidList == null)
			{
				this.SwitchToErrorMessage();
				this.summaryDisplay.GenerateSummary();
			}
			else if (videoGuidList.Count == 0)
			{
				this.SwitchToNoResultsMessage();
				this.summaryDisplay.GenerateSummary();
			}
			else if (videoGuidList.Count > 0 && (string.IsNullOrEmpty(this.videoId) || videoGuidList.Contains(this.videoId)))
			{
				this.SwitchToSummaryDisplay();
				if (string.IsNullOrEmpty(this.videoId))
				{
					this.videoId = videoGuidList[0];
					this.summaryData.Guid = this.videoId;
				}
				this.summaryDisplay.GenerateSummary();
				this.sourceTypeHelper.QueryStart();
			}
			else
			{
				this.SwitchToNoResultsMessage();
			}
			KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>> keyValuePair = new KeyValuePair<VideoSummaryStyle, List<VideoSummaryData>>(this.summaryData.Style, new List<VideoSummaryData>
			{
				this.summaryData
			});
			Service.Get<EventManager>().SendEvent(EventId.UIVideosQueryResponse, keyValuePair);
		}

		public void Cleanup()
		{
			if (this.summaryData != null)
			{
				this.summaryData.Cleanup();
				this.summaryData = null;
			}
			if (this.summaryDisplay != null)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.UIVideosRefresh);
				Service.Get<EventManager>().UnregisterObserver(this, EventId.UIVideosSourceTypeResponse);
				this.summaryDisplay.Cleanup();
				this.summaryDisplay = null;
			}
			this.cleanedUp = true;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.UIVideosRefresh)
			{
				VideoSummaryDisplay videoSummaryDisplay = (VideoSummaryDisplay)cookie;
				if (videoSummaryDisplay == this.summaryDisplay)
				{
					this.QueryStart();
				}
			}
			else if (id == EventId.UIVideosSourceTypeResponse && this.summaryDisplay != null)
			{
				KeyValuePair<List<string>, string> keyValuePair = (KeyValuePair<List<string>, string>)cookie;
				if (keyValuePair.Key.Contains(this.summaryDisplay.GetGuid()))
				{
					this.summaryDisplay.UpdateSourceType(keyValuePair.Value);
				}
			}
			return EatResponse.NotEaten;
		}
	}
}
