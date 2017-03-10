using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

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
				Service.Get<StaRTSLogger>().ErrorFormat("Could not find VideoData {0} to load thumbnail", new object[]
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
				return;
			}
			Service.Get<Engine>().StartCoroutine(this.LoadImage(thumbnailURL, onLoadComplete));
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
				Service.Get<StaRTSLogger>().ErrorFormat("GetThumbnailLocal failed {0}", new object[]
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

		[IteratorStateMachine(typeof(ThumbnailManager.<LoadImage>d__9))]
		private IEnumerator LoadImage(string url, ThumbnailManager.ImageLoadCompleteDelegate onLoadComplete)
		{
			WWW wWW = new WWW(url);
			WWWManager.Add(wWW);
			yield return wWW;
			if (!WWWManager.Remove(wWW))
			{
				yield break;
			}
			string error = wWW.error;
			Texture2D texture = wWW.texture;
			if (string.IsNullOrEmpty(error))
			{
				this.Store(url, texture);
				onLoadComplete(texture);
				this.Remove(url);
			}
			else
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Error fetching thumbnail at {0}", new object[]
				{
					url
				});
				onLoadComplete(null);
			}
			wWW.Dispose();
			yield break;
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

		protected internal ThumbnailManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ThumbnailManager)GCHandledObjects.GCHandleToObject(instance)).AdjustSize((ThumbnailSize)(*(int*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ThumbnailManager)GCHandledObjects.GCHandleToObject(instance)).Clear();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ThumbnailManager)GCHandledObjects.GCHandleToObject(instance)).GetThumbnail(Marshal.PtrToStringUni(*(IntPtr*)args), (ThumbnailSize)(*(int*)(args + 1)), (ThumbnailManager.ImageLoadCompleteDelegate)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ThumbnailManager)GCHandledObjects.GCHandleToObject(instance)).GetThumbnailLocal(Marshal.PtrToStringUni(*(IntPtr*)args), (ThumbnailSize)(*(int*)(args + 1))));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ThumbnailManager)GCHandledObjects.GCHandleToObject(instance)).LoadImage(Marshal.PtrToStringUni(*(IntPtr*)args), (ThumbnailManager.ImageLoadCompleteDelegate)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ThumbnailManager)GCHandledObjects.GCHandleToObject(instance)).PurgeThumbnail(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ThumbnailManager)GCHandledObjects.GCHandleToObject(instance)).Remove(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ThumbnailManager)GCHandledObjects.GCHandleToObject(instance)).Store(Marshal.PtrToStringUni(*(IntPtr*)args), (Texture2D)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
