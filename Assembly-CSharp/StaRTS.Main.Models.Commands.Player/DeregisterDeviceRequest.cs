using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class DeregisterDeviceRequest : PlayerIdRequest
	{
		public string DeviceToken
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			if (this.DeviceToken != null)
			{
				serializer.AddString("deviceToken", this.DeviceToken);
			}
			serializer.AddString("deviceType", "m");
			return serializer.End().ToString();
		}

		public DeregisterDeviceRequest()
		{
		}

		protected internal DeregisterDeviceRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeregisterDeviceRequest)GCHandledObjects.GCHandleToObject(instance)).DeviceToken);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeregisterDeviceRequest)GCHandledObjects.GCHandleToObject(instance)).DeviceToken = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeregisterDeviceRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
