using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.BI
{
	public class DefaultDeviceInfoController : IDeviceInfoController
	{
		public DefaultDeviceInfoController()
		{
		}

		public string GetDeviceId()
		{
			return SystemInfo.deviceUniqueIdentifier;
		}

		public void AddDeviceSpecificInfo(BILog log)
		{
		}

		protected internal DefaultDeviceInfoController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DefaultDeviceInfoController)GCHandledObjects.GCHandleToObject(instance)).AddDeviceSpecificInfo((BILog)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultDeviceInfoController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceId());
		}
	}
}
