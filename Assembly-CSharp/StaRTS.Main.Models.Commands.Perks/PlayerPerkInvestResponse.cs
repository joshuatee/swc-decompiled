using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Perks
{
	public class PlayerPerkInvestResponse : AbstractResponse
	{
		private const string PERK_STATUS = "perkStatus";

		private const string SQUAD_LEVEL = "guildLevel";

		private const string SQUAD_TOTAL_INVESTMENT = "totalRepInvested";

		private const string NEW_REP_INVESTED = "newRepInvested";

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (dictionary.ContainsKey("perkStatus"))
			{
				currentSquad.UpdateSquadPerks(dictionary["perkStatus"]);
			}
			if (dictionary.ContainsKey("guildLevel") && dictionary.ContainsKey("totalRepInvested"))
			{
				int squadLevel = Convert.ToInt32(dictionary["guildLevel"], CultureInfo.InvariantCulture);
				int totalRepInvested = Convert.ToInt32(dictionary["totalRepInvested"], CultureInfo.InvariantCulture);
				currentSquad.UpdateSquadLevel(squadLevel, totalRepInvested);
			}
			if (dictionary.ContainsKey("newRepInvested"))
			{
				Service.Get<SquadController>().StateManager.NumRepDonatedInSession += Convert.ToInt32(dictionary["newRepInvested"], CultureInfo.InvariantCulture);
			}
			Service.Get<EventManager>().SendEvent(EventId.SquadPerkUpdated, null);
			return this;
		}

		public PlayerPerkInvestResponse()
		{
		}

		protected internal PlayerPerkInvestResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerPerkInvestResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
