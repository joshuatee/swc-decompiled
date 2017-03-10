using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Cameras
{
	public class RenderTextureItem
	{
		public RenderTexture RenderTexture
		{
			get;
			private set;
		}

		public int Width
		{
			get;
			private set;
		}

		public int Height
		{
			get;
			private set;
		}

		public bool InUse
		{
			get;
			set;
		}

		public RenderTextureItem(RenderTexture renderTexture, int width, int height)
		{
			this.RenderTexture = renderTexture;
			this.Width = width;
			this.Height = height;
			this.InUse = false;
		}

		protected internal RenderTextureItem(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RenderTextureItem)GCHandledObjects.GCHandleToObject(instance)).Height);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RenderTextureItem)GCHandledObjects.GCHandleToObject(instance)).InUse);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RenderTextureItem)GCHandledObjects.GCHandleToObject(instance)).RenderTexture);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RenderTextureItem)GCHandledObjects.GCHandleToObject(instance)).Width);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((RenderTextureItem)GCHandledObjects.GCHandleToObject(instance)).Height = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((RenderTextureItem)GCHandledObjects.GCHandleToObject(instance)).InUse = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((RenderTextureItem)GCHandledObjects.GCHandleToObject(instance)).RenderTexture = (RenderTexture)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((RenderTextureItem)GCHandledObjects.GCHandleToObject(instance)).Width = *(int*)args;
			return -1L;
		}
	}
}
