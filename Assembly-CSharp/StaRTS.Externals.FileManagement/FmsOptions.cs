using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.FileManagement
{
	public class FmsOptions
	{
		public string CodeName
		{
			get;
			set;
		}

		public FmsEnvironment Env
		{
			get;
			set;
		}

		public FmsMode Mode
		{
			get;
			set;
		}

		public string ManifestVersion
		{
			get;
			set;
		}

		public MonoBehaviour Engine
		{
			get;
			set;
		}

		public string LocalRootUrl
		{
			get;
			set;
		}

		public string RemoteRootUrl
		{
			get;
			set;
		}

		public override string ToString()
		{
			return string.Format("CodeName: {0}, Env {1}, Mode {2}, ManifestVersion {3}, RootUrl {4}", new object[]
			{
				this.CodeName,
				this.Env,
				this.Mode,
				this.ManifestVersion,
				this.LocalRootUrl
			});
		}

		public FmsOptions()
		{
		}

		protected internal FmsOptions(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).CodeName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).Engine);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).Env);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).LocalRootUrl);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).ManifestVersion);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).Mode);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).RemoteRootUrl);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).CodeName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).Engine = (MonoBehaviour)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).Env = (FmsEnvironment)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).LocalRootUrl = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).ManifestVersion = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).Mode = (FmsMode)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).RemoteRootUrl = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FmsOptions)GCHandledObjects.GCHandleToObject(instance)).ToString());
		}
	}
}
