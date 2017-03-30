using StaRTS.Externals.DMOAnalytics;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Models.Commands.Player
{
	public class TapjoyPlayerSyncResponse : ExternalCurrencySyncResponse
	{
		protected override void LogResults(KeyValuePair<string, object> entry, int diff)
		{
			if (entry.Key.Equals("crystals"))
			{
				string currency = "USD";
				double num = -0.01;
				double amountPaid = (double)diff * num;
				Service.Get<DMOAnalyticsController>().LogPaymentAction(currency, amountPaid, "tapjoy", 1, "tapjoy");
			}
		}
	}
}
