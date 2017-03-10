using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Perks
{
	public class PlayerPerkSkipCooldownResponse : AbstractResponse
	{
		public override ISerializable FromObject(object obj)
		{
			Service.Get<PerkManager>().PurchaseCooldownSkipResponse(obj);
			return this;
		}

		public PlayerPerkSkipCooldownResponse()
		{
		}

		protected internal PlayerPerkSkipCooldownResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerPerkSkipCooldownResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
