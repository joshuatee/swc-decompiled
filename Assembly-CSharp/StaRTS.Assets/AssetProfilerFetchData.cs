using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Assets
{
	public class AssetProfilerFetchData
	{
		public string AssetName
		{
			get;
			private set;
		}

		public int FetchCount
		{
			get;
			set;
		}

		public float FetchTime
		{
			get;
			set;
		}

		public AssetProfilerFetchData(string name)
		{
			this.AssetName = name;
			this.FetchCount = 0;
			this.FetchTime = 0f;
		}

		protected internal AssetProfilerFetchData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetProfilerFetchData)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetProfilerFetchData)GCHandledObjects.GCHandleToObject(instance)).FetchCount);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetProfilerFetchData)GCHandledObjects.GCHandleToObject(instance)).FetchTime);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AssetProfilerFetchData)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AssetProfilerFetchData)GCHandledObjects.GCHandleToObject(instance)).FetchCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AssetProfilerFetchData)GCHandledObjects.GCHandleToObject(instance)).FetchTime = *(float*)args;
			return -1L;
		}
	}
}
