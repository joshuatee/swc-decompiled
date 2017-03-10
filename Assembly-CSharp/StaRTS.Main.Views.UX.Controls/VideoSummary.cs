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
using WinRTBridge;

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
				return;
			}
			if (this.summaryData.Style == VideoSummaryStyle.HolonetFeatured)
			{
				this.summaryData.Style = VideoSummaryStyle.HolonetFeaturedError;
				return;
			}
			if (this.summaryData.Style == VideoSummaryStyle.SquadChat)
			{
				this.summaryData.Style = VideoSummaryStyle.SquadChatError;
			}
		}

		private void SwitchToNoResultsMessage()
		{
			if (this.summaryData.Style == VideoSummaryStyle.Holonet)
			{
				this.summaryData.Style = VideoSummaryStyle.HolonetEmpty;
				return;
			}
			if (this.summaryData.Style == VideoSummaryStyle.HolonetFeatured)
			{
				this.summaryData.Style = VideoSummaryStyle.HolonetFeaturedEmpty;
				return;
			}
			if (this.summaryData.Style == VideoSummaryStyle.SquadChat)
			{
				this.summaryData.Style = VideoSummaryStyle.SquadChatEmpty;
			}
		}

		private void SwitchToSummaryDisplay()
		{
			if (this.summaryData.Style == VideoSummaryStyle.HolonetEmpty || this.summaryData.Style == VideoSummaryStyle.HolonetError)
			{
				this.summaryData.Style = VideoSummaryStyle.Holonet;
				return;
			}
			if (this.summaryData.Style == VideoSummaryStyle.HolonetFeaturedEmpty || this.summaryData.Style == VideoSummaryStyle.HolonetFeaturedError)
			{
				this.summaryData.Style = VideoSummaryStyle.HolonetFeatured;
				return;
			}
			if (this.summaryData.Style == VideoSummaryStyle.SquadChatEmpty || this.summaryData.Style == VideoSummaryStyle.SquadChatError)
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
				Service.Get<StaRTSLogger>().Error("OnTagQueried called before VideoSummary set up");
				return;
			}
			if (videoGuidList == null)
			{
				this.SwitchToErrorMessage();
				this.summaryDisplay.GenerateSummary();
				return;
			}
			if (videoGuidList.Count == 0)
			{
				this.SwitchToNoResultsMessage();
				this.summaryDisplay.GenerateSummary();
				return;
			}
			this.SwitchToSummaryDisplay();
			this.videoId = videoGuidList[0];
			this.summaryData.Guid = this.videoId;
			this.QueryVideoData();
		}

		private void QueryVideoData()
		{
			if (string.IsNullOrEmpty(this.videoId))
			{
				Service.Get<VideoDataManager>().GetFeatured(new VideoDataManager.DataListQueryCompleteDelegate(this.OnSearchQueried));
				return;
			}
			Service.Get<VideoDataManager>().GetAllEnvironmentVideos(new VideoDataManager.DataListQueryCompleteDelegate(this.OnSearchQueried));
		}

		private void OnSearchQueried(List<string> videoGuidList)
		{
			if (this.cleanedUp)
			{
				return;
			}
			if (this.summaryDisplay == null || this.summaryData == null)
			{
				Service.Get<StaRTSLogger>().Error("OnSearchQueried called before VideoSummary set up");
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
				if (keyValuePair.get_Key().Contains(this.summaryDisplay.GetGuid()))
				{
					this.summaryDisplay.UpdateSourceType(keyValuePair.get_Value());
				}
			}
			return EatResponse.NotEaten;
		}

		protected internal VideoSummary(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).summaryData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).summaryDisplay);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).thumbSize);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).UIName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).OnSearchQueried((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).OnTagQueried((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).PopulateDisplay();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).QueryStart();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).QueryVideoData();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).summaryData = (VideoSummaryData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).summaryDisplay = (VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).thumbSize = (ThumbnailSize)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).SetSummary((VideoSummaryData)GCHandledObjects.GCHandleToObject(*args), (VideoSummaryDisplay)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).SwitchToErrorMessage();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).SwitchToNoResultsMessage();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((VideoSummary)GCHandledObjects.GCHandleToObject(instance)).SwitchToSummaryDisplay();
			return -1L;
		}
	}
}
