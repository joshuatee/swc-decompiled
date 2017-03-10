using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Planets
{
	public class RelocatePlanetRequest : PlayerIdChecksumRequest
	{
		private string planetUID;

		private bool payWithHardCurrency;

		public RelocatePlanetRequest(string planetUID, bool pay)
		{
			this.planetUID = planetUID;
			this.payWithHardCurrency = pay;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("playerId", base.PlayerId);
			startedSerializer.AddString("planet", this.planetUID);
			startedSerializer.AddBool("payWithHardCurrency", this.payWithHardCurrency);
			return startedSerializer.End().ToString();
		}

		protected internal RelocatePlanetRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RelocatePlanetRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
