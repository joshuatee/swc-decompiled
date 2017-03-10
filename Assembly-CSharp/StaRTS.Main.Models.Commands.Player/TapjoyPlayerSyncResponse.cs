using StaRTS.Externals.DMOAnalytics;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class TapjoyPlayerSyncResponse : ExternalCurrencySyncResponse
	{
		protected override void LogResults(KeyValuePair<string, object> entry, int diff)
		{
			if (entry.get_Key().Equals("crystals"))
			{
				string currency = "USD";
				double num = -0.01;
				double amountPaid = (double)diff * num;
				Service.Get<DMOAnalyticsController>().LogPaymentAction(currency, amountPaid, "tapjoy", 1, "tapjoy");
			}
		}

		public TapjoyPlayerSyncResponse()
		{
		}

		protected internal TapjoyPlayerSyncResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TapjoyPlayerSyncResponse)GCHandledObjects.GCHandleToObject(instance)).LogResults((KeyValuePair<string, object>)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}
	}
}
