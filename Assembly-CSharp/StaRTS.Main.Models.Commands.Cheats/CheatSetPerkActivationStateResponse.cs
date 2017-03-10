using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetPerkActivationStateResponse : AbstractResponse
	{
		public override ISerializable FromObject(object rawPerksData)
		{
			Service.Get<PerkManager>().UpdatePlayerPerksData(rawPerksData);
			return this;
		}

		public CheatSetPerkActivationStateResponse()
		{
		}

		protected internal CheatSetPerkActivationStateResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetPerkActivationStateResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
