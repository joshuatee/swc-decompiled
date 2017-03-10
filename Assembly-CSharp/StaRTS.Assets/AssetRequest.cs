using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Assets
{
	public class AssetRequest
	{
		public AssetHandle Handle
		{
			get;
			private set;
		}

		public string AssetName
		{
			get;
			private set;
		}

		public AssetSuccessDelegate OnSuccess
		{
			get;
			private set;
		}

		public AssetFailureDelegate OnFailure
		{
			get;
			private set;
		}

		public object Cookie
		{
			get;
			private set;
		}

		public int DelayLoadFrameCount
		{
			get;
			set;
		}

		public AssetRequest(AssetHandle handle, string assetName, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, object cookie)
		{
			this.Handle = handle;
			this.AssetName = assetName;
			this.OnSuccess = onSuccess;
			this.OnFailure = onFailure;
			this.Cookie = cookie;
			this.DelayLoadFrameCount = 0;
		}

		protected internal AssetRequest(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).Cookie);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).DelayLoadFrameCount);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).Handle);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).OnFailure);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).OnSuccess);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).Cookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).DelayLoadFrameCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).Handle = (AssetHandle)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).OnFailure = (AssetFailureDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AssetRequest)GCHandledObjects.GCHandleToObject(instance)).OnSuccess = (AssetSuccessDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
