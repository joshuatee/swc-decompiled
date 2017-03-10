using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatScheduleDailyCrateResponse : AbstractResponse
	{
		public override ISerializable FromObject(object rawCratesData)
		{
			Service.Get<CurrentPlayer>().Prizes.Crates.UpdateAndBadgeFromServerObject(rawCratesData);
			return this;
		}

		public CheatScheduleDailyCrateResponse()
		{
		}

		protected internal CheatScheduleDailyCrateResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatScheduleDailyCrateResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
