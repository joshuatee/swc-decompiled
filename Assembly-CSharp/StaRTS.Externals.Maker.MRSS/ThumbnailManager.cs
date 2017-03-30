using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace StaRTS.Externals.Maker.MRSS
{
	public class ThumbnailManager
	{
		public delegate void ImageLoadCompleteDelegate(Texture2D thumbnail);

		private const int LARGE_LIMIT = 1024;

		private const int XLARGE_LIMIT = 2560;

		private Dictionary<string, Texture2D> thumbnails;

		public ThumbnailManager()
		{
			this.thumbnails = new Dictionary<string, Texture2D>();
			Service.Set<ThumbnailManager>(this);
		}

		public void Clear()
		{
			this.thumbnails.Clear();
		}

		public void GetThumbnail(string guid, ThumbnailSize size, ThumbnailManager.ImageLoadCompleteDelegate onLoadComplete)
		{
			size = this.AdjustSize(size);
			VideoData videoData;
			if (!Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData))
			{
				Service.Get<Logger>().ErrorFormat("Could not find VideoData {0} to load thumbnail", new object[]
				{
					guid
				});
				onLoadComplete(null);
				return;
			}
			string thumbnailURL = videoData.GetThumbnailURL(size);
			Texture2D thumbnail;
			if (this.thumbnails.TryGetValue(thumbnailURL, out thumbnail))
			{
				onLoadComplete(thumbnail);
			}
			else
			{
				Service.Get<Engine>().StartCoroutine(this.LoadImage(thumbnailURL, onLoadComplete));
			}
		}

		public Texture2D GetThumbnailLocal(string guid, ThumbnailSize size)
		{
			size = this.AdjustSize(size);
			VideoData videoData;
			if (!Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData))
			{
				return null;
			}
			if (videoData == null)
			{
				Service.Get<Logger>().ErrorFormat("GetThumbnailLocal failed {0}", new object[]
				{
					guid
				});
				return null;
			}
			Texture2D result;
			if (!this.thumbnails.TryGetValue(videoData.GetThumbnailURL(size), out result))
			{
				return null;
			}
			return result;
		}

		public void PurgeThumbnail(string guid)
		{
			VideoData videoData;
			if (!Service.Get<VideoDataManager>().VideoDatas.TryGetValue(guid, out videoData))
			{
				return;
			}
			if (videoData == null)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				this.Remove(videoData.GetThumbnailURL((ThumbnailSize)i));
			}
		}

		[DebuggerHidden]
		private IEnumerator LoadImage(string url, ThumbnailManager.ImageLoadCompleteDelegate onLoadComplete)
		{
			ThumbnailManager.<LoadImage>c__Iterator10 <LoadImage>c__Iterator = new ThumbnailManager.<LoadImage>c__Iterator10();
			<LoadImage>c__Iterator.url = url;
			<LoadImage>c__Iterator.onLoadComplete = onLoadComplete;
			<LoadImage>c__Iterator.<$>url = url;
			<LoadImage>c__Iterator.<$>onLoadComplete = onLoadComplete;
			<LoadImage>c__Iterator.<>f__this = this;
			return <LoadImage>c__Iterator;
		}

		private void Store(string url, Texture2D texture)
		{
			this.thumbnails[url] = texture;
		}

		private void Remove(string url)
		{
			this.thumbnails.Remove(url);
		}

		private ThumbnailSize AdjustSize(ThumbnailSize requested)
		{
			int num = Math.Max(Screen.currentResolution.width, Screen.currentResolution.height);
			ThumbnailSize val;
			if (num <= 1024)
			{
				val = ThumbnailSize.LARGE;
			}
			else if (num <= 2560)
			{
				val = ThumbnailSize.XLARGE;
			}
			else
			{
				val = ThumbnailSize.XXLARGE;
			}
			return (ThumbnailSize)Math.Min((int)requested, (int)val);
		}
	}
}
