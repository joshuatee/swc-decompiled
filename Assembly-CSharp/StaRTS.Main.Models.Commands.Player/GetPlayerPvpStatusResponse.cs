using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class GetPlayerPvpStatusResponse : AbstractResponse
	{
		public override ISerializable FromObject(object obj)
		{
			return this;
		}

		public GetPlayerPvpStatusResponse()
		{
		}

		protected internal GetPlayerPvpStatusResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetPlayerPvpStatusResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
