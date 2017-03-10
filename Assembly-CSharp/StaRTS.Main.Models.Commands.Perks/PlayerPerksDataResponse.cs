using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Perks
{
	public class PlayerPerksDataResponse : AbstractResponse
	{
		public override ISerializable FromObject(object obj)
		{
			Service.Get<PerkManager>().UpdatePlayerPerksData(obj);
			return this;
		}

		public PlayerPerksDataResponse()
		{
		}

		protected internal PlayerPerksDataResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerPerksDataResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
