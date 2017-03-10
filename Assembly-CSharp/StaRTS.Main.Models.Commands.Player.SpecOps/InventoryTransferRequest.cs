using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.SpecOps
{
	public class InventoryTransferRequest : PlayerIdChecksumRequest
	{
		private string uid;

		private int amount;

		public InventoryTransferRequest(string uid, int amount)
		{
			this.uid = uid;
			this.amount = amount;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("uid", this.uid);
			startedSerializer.Add<int>("count", this.amount);
			return startedSerializer.End().ToString();
		}

		protected internal InventoryTransferRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryTransferRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
