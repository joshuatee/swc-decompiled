using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.FacebookApi
{
	public class GraphResult : IGraphResult, IResult
	{
		public string Error
		{
			get;
			private set;
		}

		public string RawResult
		{
			get;
			private set;
		}

		public Texture2D Texture
		{
			get;
			private set;
		}

		public GraphResult(string error, string rawResult)
		{
			this.Error = error;
			this.RawResult = rawResult;
		}

		public void DownloadTexture(Action<IGraphResult> callback)
		{
			FacebookManager.Instance.StartCoroutine(this.DownloadTextureCoroutine(callback));
		}

		[IteratorStateMachine(typeof(GraphResult.<DownloadTextureCoroutine>d__14))]
		private IEnumerator DownloadTextureCoroutine(Action<IGraphResult> callback)
		{
			if (this.RawResult.Contains("is_silhouette"))
			{
				int num = this.RawResult.IndexOf("http");
				string text = this.RawResult.Substring(num, this.RawResult.IndexOf("\"", num) - num);
				text = text.Replace("\\", string.Empty);
				WWW wWW = new WWW(text);
				yield return wWW;
				this.Texture = wWW.texture;
				wWW = null;
			}
			callback.Invoke(this);
			yield break;
		}

		protected internal GraphResult(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GraphResult)GCHandledObjects.GCHandleToObject(instance)).DownloadTexture((Action<IGraphResult>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GraphResult)GCHandledObjects.GCHandleToObject(instance)).DownloadTextureCoroutine((Action<IGraphResult>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GraphResult)GCHandledObjects.GCHandleToObject(instance)).Error);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GraphResult)GCHandledObjects.GCHandleToObject(instance)).RawResult);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GraphResult)GCHandledObjects.GCHandleToObject(instance)).Texture);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GraphResult)GCHandledObjects.GCHandleToObject(instance)).Error = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GraphResult)GCHandledObjects.GCHandleToObject(instance)).RawResult = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GraphResult)GCHandledObjects.GCHandleToObject(instance)).Texture = (Texture2D)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
