using StaRTS.Externals.Maker.MRSS;
using StaRTS.Externals.Maker.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.Maker
{
	public class VideoSummaryData : IEventObserver
	{
		private bool cleanedUp;

		private ThumbnailSize thumbnailSize;

		public int Location
		{
			get;
			private set;
		}

		public string Guid
		{
			get;
			set;
		}

		public string UILabel
		{
			get;
			set;
		}

		public bool IsVisible
		{
			get;
			set;
		}

		public VideoSummaryStyle Style
		{
			get;
			set;
		}

		public VideoSummaryData(int location, string label, string videoGuid, ThumbnailSize scale = ThumbnailSize.TINY, VideoSummaryStyle style = VideoSummaryStyle.Featured)
		{
			this.Location = location;
			this.Guid = videoGuid;
			this.UILabel = label;
			this.Style = style;
			this.cleanedUp = false;
			this.IsVisible = true;
			this.thumbnailSize = scale;
		}

		public void DownloadThumbnail()
		{
			if (Service.Get<VideoDataManager>().VideoDatas.ContainsKey(this.Guid))
			{
				Service.Get<ThumbnailManager>().GetThumbnail(this.Guid, this.thumbnailSize, new ThumbnailManager.ImageLoadCompleteDelegate(this.OnThumbnailReceived));
				return;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("video data not found for '{0}'", new object[]
			{
				this.Guid
			});
		}

		private void OnThumbnailReceived(Texture2D thumbnail)
		{
			if (!this.cleanedUp)
			{
				if (this.IsVisible)
				{
					Service.Get<EventManager>().SendEvent(EventId.UIVideosThumbnailResponse, this);
					return;
				}
			}
			else
			{
				Service.Get<ThumbnailManager>().PurgeThumbnail(this.Guid);
			}
		}

		public Texture2D GetThumbnailImage()
		{
			return Service.Get<ThumbnailManager>().GetThumbnailLocal(this.Guid, this.thumbnailSize);
		}

		public void Cleanup()
		{
			Service.Get<ThumbnailManager>().PurgeThumbnail(this.Guid);
			this.cleanedUp = true;
			Service.Get<EventManager>().UnregisterObserver(this, EventId.VideoEnd);
		}

		public VideoData GetVideoData()
		{
			VideoData result;
			Service.Get<VideoDataManager>().VideoDatas.TryGetValue(this.Guid, out result);
			return result;
		}

		public void PlayVideo(string action)
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.VideoEnd);
			VideoPlayer.Play(this.Guid, action);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.VideoEnd)
			{
				KeyValuePair<bool, string> keyValuePair = (KeyValuePair<bool, string>)cookie;
				if (keyValuePair.get_Value() == this.Guid)
				{
					EventManager eventManager = Service.Get<EventManager>();
					eventManager.SendEvent(EventId.UIVideosViewComplete, keyValuePair);
					eventManager.UnregisterObserver(this, EventId.VideoEnd);
				}
			}
			return EatResponse.NotEaten;
		}

		protected internal VideoSummaryData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).DownloadThumbnail();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).Guid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).IsVisible);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).Location);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).Style);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).UILabel);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).GetThumbnailImage());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).GetVideoData());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).OnThumbnailReceived((Texture2D)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).PlayVideo(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).Guid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).IsVisible = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).Location = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).Style = (VideoSummaryStyle)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((VideoSummaryData)GCHandledObjects.GCHandleToObject(instance)).UILabel = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
