using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace Source.StaRTS.Main.Models.Commands.Player
{
	internal class GetAuthTokenRequest : PlayerIdRequest
	{
		public string RequestToken
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("requestToken", this.RequestToken);
			return serializer.End().ToString();
		}

		public GetAuthTokenRequest()
		{
		}

		protected internal GetAuthTokenRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetAuthTokenRequest)GCHandledObjects.GCHandleToObject(instance)).RequestToken);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GetAuthTokenRequest)GCHandledObjects.GCHandleToObject(instance)).RequestToken = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetAuthTokenRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
