using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetTroopDonateRepResponse : AbstractResponse
	{
		public override ISerializable FromObject(object rawDonateData)
		{
			Service.Get<CurrentPlayer>().SetTroopDonationProgress(rawDonateData);
			return this;
		}

		public CheatSetTroopDonateRepResponse()
		{
		}

		protected internal CheatSetTroopDonateRepResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetTroopDonateRepResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
