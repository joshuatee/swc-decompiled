using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Assets
{
	public class AssetInfo
	{
		public AssetHandle BundleHandle
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			private set;
		}

		public AssetType AssetType
		{
			get;
			private set;
		}

		public object AssetObject
		{
			get;
			set;
		}

		public int LoadCount
		{
			get;
			set;
		}

		public List<AssetHandle> UnloadHandles
		{
			get;
			set;
		}

		public bool AllContentsExtracted
		{
			get;
			set;
		}

		public UnityEngine.Object[] Prefabs
		{
			get;
			set;
		}

		public List<AssetRequest> AssetRequests
		{
			get;
			set;
		}

		public AssetInfo(string assetName, AssetType assetType)
		{
			this.BundleHandle = AssetHandle.Invalid;
			this.AssetName = assetName;
			this.AssetType = assetType;
			this.AssetObject = null;
			this.AssetRequests = null;
			this.LoadCount = 0;
			this.UnloadHandles = null;
			this.AllContentsExtracted = false;
			this.Prefabs = null;
		}

		protected internal AssetInfo(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AllContentsExtracted);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AssetObject);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AssetRequests);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AssetType);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).BundleHandle);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).LoadCount);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).Prefabs);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).UnloadHandles);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AllContentsExtracted = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AssetObject = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AssetRequests = (List<AssetRequest>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).AssetType = (AssetType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).BundleHandle = (AssetHandle)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).LoadCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).Prefabs = (UnityEngine.Object[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((AssetInfo)GCHandledObjects.GCHandleToObject(instance)).UnloadHandles = (List<AssetHandle>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
