using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Crates
{
	public class CheatAddCrateResponse : AbstractResponse
	{
		public CheatAddCrateResponse()
		{
		}

		public override ISerializable FromObject(object obj)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			InventoryCrates crates = currentPlayer.Prizes.Crates;
			crates.UpdateAndBadgeFromServerObject(obj);
			return this;
		}

		protected internal CheatAddCrateResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatAddCrateResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
