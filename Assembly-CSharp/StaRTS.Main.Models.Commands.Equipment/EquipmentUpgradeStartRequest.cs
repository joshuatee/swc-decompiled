using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Equipment
{
	public class EquipmentUpgradeStartRequest : PlayerIdChecksumRequest
	{
		public string BuildingId
		{
			get;
			set;
		}

		public string EquipmentUid
		{
			get;
			set;
		}

		public EquipmentUpgradeStartRequest(string buildingId, string equipmentUid)
		{
			this.BuildingId = buildingId;
			this.EquipmentUid = equipmentUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("buildingId", this.BuildingId);
			startedSerializer.AddString("equipmentUid", this.EquipmentUid);
			return startedSerializer.End().ToString();
		}

		protected internal EquipmentUpgradeStartRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).EquipmentUid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EquipmentUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EquipmentUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).EquipmentUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
