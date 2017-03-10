using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatUpgradeBuildingsRequest : PlayerIdRequest
	{
		private List<string> buildingIds;

		public CheatUpgradeBuildingsRequest(List<string> buildingIds)
		{
			this.buildingIds = buildingIds;
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddArrayOfPrimitives<string>("buildingIds", this.buildingIds);
			return serializer.End().ToString();
		}

		protected internal CheatUpgradeBuildingsRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatUpgradeBuildingsRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
