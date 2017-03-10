using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Account.External
{
	public class UnregisterExternalAccountRequest : AbstractRequest
	{
		public AccountProvider Provider
		{
			get;
			set;
		}

		public string PlayerId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", this.PlayerId);
			serializer.AddString("providerId", this.Provider.ToString());
			return serializer.End().ToString();
		}

		public UnregisterExternalAccountRequest()
		{
		}

		protected internal UnregisterExternalAccountRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UnregisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).PlayerId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UnregisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).Provider);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UnregisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).PlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UnregisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).Provider = (AccountProvider)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UnregisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
