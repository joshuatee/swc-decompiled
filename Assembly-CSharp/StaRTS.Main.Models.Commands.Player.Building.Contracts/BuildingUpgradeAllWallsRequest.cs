using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Contracts
{
	public class BuildingUpgradeAllWallsRequest : PlayerIdChecksumRequest
	{
		public string BuildingUid
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("buildingUid", this.BuildingUid);
			return startedSerializer.End().ToString();
		}

		public BuildingUpgradeAllWallsRequest()
		{
		}

		protected internal BuildingUpgradeAllWallsRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingUpgradeAllWallsRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingUid);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BuildingUpgradeAllWallsRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingUpgradeAllWallsRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
