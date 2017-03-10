using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Swap
{
	public class BuildingSwapRequest : PlayerIdChecksumRequest
	{
		public string goingToBuildingUid
		{
			get;
			set;
		}

		public string InstanceId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("buildingId", this.InstanceId);
			startedSerializer.AddString("buildingUid", this.goingToBuildingUid);
			return startedSerializer.End().ToString();
		}

		public BuildingSwapRequest()
		{
		}

		protected internal BuildingSwapRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSwapRequest)GCHandledObjects.GCHandleToObject(instance)).goingToBuildingUid);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSwapRequest)GCHandledObjects.GCHandleToObject(instance)).InstanceId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BuildingSwapRequest)GCHandledObjects.GCHandleToObject(instance)).goingToBuildingUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BuildingSwapRequest)GCHandledObjects.GCHandleToObject(instance)).InstanceId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingSwapRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
