using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Contracts
{
	public class BuildingContractRequest : PlayerIdChecksumRequest
	{
		protected string buildingKey;

		protected string tag;

		protected override bool CalculateChecksumManually
		{
			get
			{
				return true;
			}
		}

		public BuildingContractRequest()
		{
		}

		public BuildingContractRequest(string buildingKey, bool isCancelOrBuyout, string tag)
		{
			this.buildingKey = buildingKey;
			this.tag = tag;
			bool simulateTroopContractUpdate = !isCancelOrBuyout;
			base.CalculateChecksum(null, false, simulateTroopContractUpdate);
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("instanceId", this.buildingKey);
			if (!string.IsNullOrEmpty(this.tag))
			{
				startedSerializer.AddString("tag", this.tag);
			}
			return startedSerializer.End().ToString();
		}

		protected internal BuildingContractRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingContractRequest)GCHandledObjects.GCHandleToObject(instance)).CalculateChecksumManually);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingContractRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
