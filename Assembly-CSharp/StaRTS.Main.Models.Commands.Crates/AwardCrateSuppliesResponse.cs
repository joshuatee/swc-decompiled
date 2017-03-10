using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Crates
{
	public class AwardCrateSuppliesResponse : AbstractResponse
	{
		public override ISerializable FromObject(object obj)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			currentPlayer.Prizes.Crates.FromObject(obj);
			Service.Get<EventManager>().SendEvent(EventId.InventoryCrateOpenedAndGranted, null);
			return this;
		}

		public AwardCrateSuppliesResponse()
		{
		}

		protected internal AwardCrateSuppliesResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AwardCrateSuppliesResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
