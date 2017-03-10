using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class GetSyncContentResponse : AbstractResponse
	{
		public override ISerializable FromObject(object obj)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			PlayerValuesController playerValuesController = Service.Get<PlayerValuesController>();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			Dictionary<string, object> dictionary2 = dictionary["inventory"] as Dictionary<string, object>;
			if (dictionary2 != null)
			{
				currentPlayer.Inventory = new Inventory();
				currentPlayer.Inventory.FromObject(dictionary2);
			}
			playerValuesController.RecalculateAll();
			return this;
		}

		public GetSyncContentResponse()
		{
		}

		protected internal GetSyncContentResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSyncContentResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
