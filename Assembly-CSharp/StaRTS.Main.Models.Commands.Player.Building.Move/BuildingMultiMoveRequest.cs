using StaRTS.Main.Models.Commands.TransferObjects;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Move
{
	public class BuildingMultiMoveRequest : PlayerIdChecksumRequest
	{
		public PositionMap PositionMap
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddObject<PositionMap>("positions", this.PositionMap);
			return startedSerializer.End().ToString();
		}

		public BuildingMultiMoveRequest()
		{
		}

		protected internal BuildingMultiMoveRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingMultiMoveRequest)GCHandledObjects.GCHandleToObject(instance)).PositionMap);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BuildingMultiMoveRequest)GCHandledObjects.GCHandleToObject(instance)).PositionMap = (PositionMap)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingMultiMoveRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
