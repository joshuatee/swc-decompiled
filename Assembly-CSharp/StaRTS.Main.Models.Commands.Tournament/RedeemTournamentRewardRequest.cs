using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Tournament
{
	public class RedeemTournamentRewardRequest : PlayerIdRequest
	{
		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			return serializer.End().ToString();
		}

		public RedeemTournamentRewardRequest()
		{
		}

		protected internal RedeemTournamentRewardRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RedeemTournamentRewardRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
