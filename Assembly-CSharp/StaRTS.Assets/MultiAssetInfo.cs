using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Assets
{
	public class MultiAssetInfo : AssetRequest
	{
		public RefCount RefCount
		{
			get;
			private set;
		}

		public AssetsCompleteDelegate OnComplete
		{
			get;
			private set;
		}

		public object CompleteCookie
		{
			get;
			private set;
		}

		public MultiAssetInfo(string assetName, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, object cookie, RefCount refCount, AssetsCompleteDelegate onComplete, object completeCookie) : base(AssetHandle.Invalid, assetName, onSuccess, onFailure, cookie)
		{
			this.RefCount = refCount;
			this.OnComplete = onComplete;
			this.CompleteCookie = completeCookie;
		}

		protected internal MultiAssetInfo(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultiAssetInfo)GCHandledObjects.GCHandleToObject(instance)).CompleteCookie);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultiAssetInfo)GCHandledObjects.GCHandleToObject(instance)).OnComplete);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultiAssetInfo)GCHandledObjects.GCHandleToObject(instance)).RefCount);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((MultiAssetInfo)GCHandledObjects.GCHandleToObject(instance)).CompleteCookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((MultiAssetInfo)GCHandledObjects.GCHandleToObject(instance)).OnComplete = (AssetsCompleteDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((MultiAssetInfo)GCHandledObjects.GCHandleToObject(instance)).RefCount = (RefCount)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
