using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatDeployableUpgradeRequest : PlayerIdRequest
	{
		private List<string> uids;

		public CheatDeployableUpgradeRequest(List<string> uids)
		{
			this.uids = uids;
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddArrayOfPrimitives<string>("uids", this.uids);
			return serializer.End().ToString();
		}

		protected internal CheatDeployableUpgradeRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatDeployableUpgradeRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
