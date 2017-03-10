using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Equipment
{
	public class EquipmentIdRequest : PlayerIdChecksumRequest
	{
		public string EquipmentID
		{
			get;
			private set;
		}

		public EquipmentIdRequest(string equipmentID)
		{
			this.EquipmentID = equipmentID;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("equipmentId", this.EquipmentID);
			return startedSerializer.End().ToString();
		}

		protected internal EquipmentIdRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentIdRequest)GCHandledObjects.GCHandleToObject(instance)).EquipmentID);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EquipmentIdRequest)GCHandledObjects.GCHandleToObject(instance)).EquipmentID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentIdRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
