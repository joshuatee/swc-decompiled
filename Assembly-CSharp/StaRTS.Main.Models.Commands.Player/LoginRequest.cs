using StaRTS.Externals.EnvironmentManager;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class LoginRequest : PlayerIdRequest
	{
		public string LocalePreference
		{
			get;
			set;
		}

		public string DeviceToken
		{
			get;
			set;
		}

		public double TimeZoneOffset
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			if (this.LocalePreference != null)
			{
				serializer.AddString("locale", this.LocalePreference);
			}
			if (this.DeviceToken != null)
			{
				serializer.AddString("deviceToken", this.DeviceToken);
			}
			serializer.AddString("deviceType", "f");
			serializer.Add<double>("timeZoneOffset", this.TimeZoneOffset);
			EnvironmentController environmentController = Service.Get<EnvironmentController>();
			serializer.AddString("clientVersion", "4.7.0.2");
			serializer.AddString("model", environmentController.GetModel());
			serializer.AddString("os", environmentController.GetOS());
			serializer.AddString("osVersion", environmentController.GetOSVersion());
			serializer.AddString("platform", environmentController.GetPlatform());
			serializer.AddString("sessionId", Service.Get<ServerAPI>().SessionId);
			serializer.AddString("deviceId", environmentController.GetDeviceIDForEvent2());
			serializer.AddString("deviceIdType", environmentController.GetDeviceIdType());
			return serializer.End().ToString();
		}

		public LoginRequest()
		{
		}

		protected internal LoginRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LoginRequest)GCHandledObjects.GCHandleToObject(instance)).DeviceToken);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LoginRequest)GCHandledObjects.GCHandleToObject(instance)).LocalePreference);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((LoginRequest)GCHandledObjects.GCHandleToObject(instance)).DeviceToken = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LoginRequest)GCHandledObjects.GCHandleToObject(instance)).LocalePreference = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LoginRequest)GCHandledObjects.GCHandleToObject(instance)).TimeZoneOffset = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LoginRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
