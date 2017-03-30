using StaRTS.Externals.Maker.MRSS;
using StaRTS.Externals.Maker.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;

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
			}
			else
			{
				Service.Get<Logger>().ErrorFormat("video data not found for '{0}'", new object[]
				{
					this.Guid
				});
			}
		}

		private void OnThumbnailReceived(Texture2D thumbnail)
		{
			if (!this.cleanedUp)
			{
				if (this.IsVisible)
				{
					Service.Get<EventManager>().SendEvent(EventId.UIVideosThumbnailResponse, this);
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
				if (keyValuePair.Value == this.Guid)
				{
					EventManager eventManager = Service.Get<EventManager>();
					eventManager.SendEvent(EventId.UIVideosViewComplete, keyValuePair);
					eventManager.UnregisterObserver(this, EventId.VideoEnd);
				}
			}
			return EatResponse.NotEaten;
		}
	}
}
