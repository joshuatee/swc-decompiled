using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands
{
	public class GetEndpointsResponse : AbstractResponse
	{
		public string BILogging
		{
			get;
			private set;
		}

		public string NoProxyBILogging
		{
			get;
			private set;
		}

		public string Event2BiLogging
		{
			get;
			private set;
		}

		public string Event2NoProxyBiLogging
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("biLogging"))
			{
				this.BILogging = (string)dictionary["biLogging"];
				this.NoProxyBILogging = (string)dictionary["noProxyBiLogging"];
			}
			if (dictionary.ContainsKey("event2BiLogging"))
			{
				this.Event2BiLogging = (string)dictionary["event2BiLogging"];
				this.Event2NoProxyBiLogging = (string)dictionary["event2NoProxyBiLogging"];
			}
			return this;
		}

		public GetEndpointsResponse()
		{
		}

		protected internal GetEndpointsResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetEndpointsResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetEndpointsResponse)GCHandledObjects.GCHandleToObject(instance)).BILogging);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetEndpointsResponse)GCHandledObjects.GCHandleToObject(instance)).Event2BiLogging);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetEndpointsResponse)GCHandledObjects.GCHandleToObject(instance)).Event2NoProxyBiLogging);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetEndpointsResponse)GCHandledObjects.GCHandleToObject(instance)).NoProxyBILogging);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GetEndpointsResponse)GCHandledObjects.GCHandleToObject(instance)).BILogging = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GetEndpointsResponse)GCHandledObjects.GCHandleToObject(instance)).Event2BiLogging = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GetEndpointsResponse)GCHandledObjects.GCHandleToObject(instance)).Event2NoProxyBiLogging = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((GetEndpointsResponse)GCHandledObjects.GCHandleToObject(instance)).NoProxyBILogging = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
