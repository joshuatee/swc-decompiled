using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Crates
{
	public class CheckDailyCrateResponse : AbstractResponse
	{
		public CheckDailyCrateResponse()
		{
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("crates"))
			{
				InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
				crates.UpdateAndBadgeFromServerObject(dictionary["crates"]);
			}
			return this;
		}

		protected internal CheckDailyCrateResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheckDailyCrateResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
