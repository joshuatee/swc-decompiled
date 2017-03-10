using StaRTS.Main.Models.Commands.Equipment;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatEarnEquipmentShardRequest : EquipmentIdRequest
	{
		public int ShardsToInvest
		{
			get;
			private set;
		}

		public CheatEarnEquipmentShardRequest(string equipmentID, int shardsToInvest) : base(equipmentID)
		{
			this.ShardsToInvest = shardsToInvest;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("equipmentId", base.EquipmentID);
			startedSerializer.Add<int>("shardsToInvest", this.ShardsToInvest);
			return startedSerializer.End().ToString();
		}

		protected internal CheatEarnEquipmentShardRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatEarnEquipmentShardRequest)GCHandledObjects.GCHandleToObject(instance)).ShardsToInvest);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CheatEarnEquipmentShardRequest)GCHandledObjects.GCHandleToObject(instance)).ShardsToInvest = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatEarnEquipmentShardRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
