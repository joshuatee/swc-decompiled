using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatDeployablesRequest : PlayerIdRequest
	{
		private Dictionary<string, int> deployables;

		public CheatDeployablesRequest(Dictionary<string, int> deployables)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.deployables = deployables;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddDictionary<int>("amount", this.deployables);
			return serializer.End().ToString();
		}

		protected internal CheatDeployablesRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatDeployablesRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
