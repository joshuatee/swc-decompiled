using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class AssetComponent : ComponentBase
	{
		public string AssetName
		{
			get;
			set;
		}

		public string RequestedAssetName
		{
			get;
			set;
		}

		public AssetHandle RequestedAssetHandle
		{
			get;
			set;
		}

		public List<AssetHandle> AddOnsAssetHandles
		{
			get;
			private set;
		}

		public AssetComponent(string assetName)
		{
			this.AssetName = assetName;
			this.RequestedAssetName = null;
			this.RequestedAssetHandle = AssetHandle.Invalid;
			this.AddOnsAssetHandles = new List<AssetHandle>();
		}

		protected internal AssetComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetComponent)GCHandledObjects.GCHandleToObject(instance)).AddOnsAssetHandles);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetComponent)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetComponent)GCHandledObjects.GCHandleToObject(instance)).RequestedAssetHandle);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetComponent)GCHandledObjects.GCHandleToObject(instance)).RequestedAssetName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AssetComponent)GCHandledObjects.GCHandleToObject(instance)).AddOnsAssetHandles = (List<AssetHandle>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AssetComponent)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AssetComponent)GCHandledObjects.GCHandleToObject(instance)).RequestedAssetHandle = (AssetHandle)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AssetComponent)GCHandledObjects.GCHandleToObject(instance)).RequestedAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
