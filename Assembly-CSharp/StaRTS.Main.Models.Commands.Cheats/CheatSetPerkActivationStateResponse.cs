using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetPerkActivationStateResponse : AbstractResponse
	{
		public override ISerializable FromObject(object rawPerksData)
		{
			Service.Get<PerkManager>().UpdatePlayerPerksData(rawPerksData);
			return this;
		}
	}
}
