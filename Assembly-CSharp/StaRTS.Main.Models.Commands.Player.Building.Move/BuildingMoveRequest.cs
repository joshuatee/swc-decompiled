using StaRTS.Main.Models.Commands.TransferObjects;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Move
{
	public class BuildingMoveRequest : PlayerIdChecksumRequest
	{
		public string BuildingId
		{
			get;
			set;
		}

		public Position Position
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("buildingId", this.BuildingId);
			startedSerializer.AddObject<Position>("position", this.Position);
			return startedSerializer.End().ToString();
		}

		public BuildingMoveRequest()
		{
		}

		protected internal BuildingMoveRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingMoveRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingMoveRequest)GCHandledObjects.GCHandleToObject(instance)).Position);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BuildingMoveRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BuildingMoveRequest)GCHandledObjects.GCHandleToObject(instance)).Position = (Position)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingMoveRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
