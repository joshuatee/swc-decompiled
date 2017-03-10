using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class RegisterDeviceResponse : PlayerResourceResponse
	{
		public override ISerializable FromObject(object obj)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("crystals"))
			{
				base.CrystalsDelta = Convert.ToInt32(dictionary["crystals"], CultureInfo.InvariantCulture);
				currentPlayer.Inventory.ModifyCrystals(base.CrystalsDelta);
			}
			if (base.CrystalsDelta > 0)
			{
				currentPlayer.IsPushIncentivized = true;
			}
			return this;
		}

		public RegisterDeviceResponse()
		{
		}

		protected internal RegisterDeviceResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterDeviceResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
