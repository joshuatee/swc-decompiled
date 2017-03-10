using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Collect
{
	public class BuildingCollectRequest : PlayerIdChecksumRequest
	{
		public string BuildingId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("buildingId", this.BuildingId);
			return startedSerializer.End().ToString();
		}

		public BuildingCollectRequest()
		{
		}

		protected internal BuildingCollectRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingCollectRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BuildingCollectRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingCollectRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
