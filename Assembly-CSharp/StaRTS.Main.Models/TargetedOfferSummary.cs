using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class TargetedOfferSummary
	{
		public string AvailableOffer;

		public uint NextOfferAvailableAt;

		public uint GlobalCooldownExpiresAt;

		public TargetedOfferSummary()
		{
		}

		public void FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("availableOffer"))
			{
				this.AvailableOffer = (dictionary["availableOffer"] as string);
			}
			if (dictionary.ContainsKey("nextOfferAvailableAt"))
			{
				this.NextOfferAvailableAt = Convert.ToUInt32(dictionary["nextOfferAvailableAt"] as string, CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("globalCooldownExpiresAt"))
			{
				this.GlobalCooldownExpiresAt = Convert.ToUInt32(dictionary["globalCooldownExpiresAt"] as string, CultureInfo.InvariantCulture);
			}
		}

		protected internal TargetedOfferSummary(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TargetedOfferSummary)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
