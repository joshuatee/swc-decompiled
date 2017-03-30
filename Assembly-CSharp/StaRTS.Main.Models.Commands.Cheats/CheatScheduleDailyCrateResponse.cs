using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatScheduleDailyCrateResponse : AbstractResponse
	{
		public override ISerializable FromObject(object rawCratesData)
		{
			Service.Get<CurrentPlayer>().Prizes.Crates.UpdateAndBadgeFromServerObject(rawCratesData);
			return this;
		}
	}
}
