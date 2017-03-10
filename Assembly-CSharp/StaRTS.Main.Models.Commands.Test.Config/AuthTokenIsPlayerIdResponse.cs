using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Test.Config
{
	public class AuthTokenIsPlayerIdResponse : AbstractResponse
	{
		public bool AuthIsPlayerId
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			this.AuthIsPlayerId = (bool)obj;
			Service.Get<StaRTSLogger>().Debug("AuthTokenIsPlayerId returned " + this.AuthIsPlayerId.ToString());
			return this;
		}

		public AuthTokenIsPlayerIdResponse()
		{
		}

		protected internal AuthTokenIsPlayerIdResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AuthTokenIsPlayerIdResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AuthTokenIsPlayerIdResponse)GCHandledObjects.GCHandleToObject(instance)).AuthIsPlayerId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AuthTokenIsPlayerIdResponse)GCHandledObjects.GCHandleToObject(instance)).AuthIsPlayerId = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
