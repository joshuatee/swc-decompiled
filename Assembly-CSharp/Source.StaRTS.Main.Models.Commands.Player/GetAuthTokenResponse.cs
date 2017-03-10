using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace Source.StaRTS.Main.Models.Commands.Player
{
	internal class GetAuthTokenResponse : AbstractResponse
	{
		public string AuthToken
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			this.AuthToken = (obj as string);
			return this;
		}

		public GetAuthTokenResponse()
		{
		}

		protected internal GetAuthTokenResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetAuthTokenResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetAuthTokenResponse)GCHandledObjects.GCHandleToObject(instance)).AuthToken);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GetAuthTokenResponse)GCHandledObjects.GCHandleToObject(instance)).AuthToken = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
