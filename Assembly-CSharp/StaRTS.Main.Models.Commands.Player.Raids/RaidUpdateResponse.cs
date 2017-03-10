using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Raids
{
	public class RaidUpdateResponse : AbstractResponse
	{
		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> raidData = obj as Dictionary<string, object>;
			Service.Get<CurrentPlayer>().SetupRaidFromDictionary(raidData);
			return this;
		}

		public RaidUpdateResponse()
		{
		}

		protected internal RaidUpdateResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidUpdateResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
