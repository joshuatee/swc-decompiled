using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetTroopDonateRepResponse : AbstractResponse
	{
		public override ISerializable FromObject(object rawDonateData)
		{
			Service.Get<CurrentPlayer>().SetTroopDonationProgress(rawDonateData);
			return this;
		}
	}
}
