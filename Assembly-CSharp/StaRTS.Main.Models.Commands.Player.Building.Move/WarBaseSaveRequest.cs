using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Commands.TransferObjects;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Move
{
	public class WarBaseSaveRequest : PlayerIdRequest
	{
		public PositionMap PositionMap
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddObject<PositionMap>("positions", this.PositionMap);
			return serializer.End().ToString();
		}

		public WarBaseSaveRequest()
		{
		}

		protected internal WarBaseSaveRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBaseSaveRequest)GCHandledObjects.GCHandleToObject(instance)).PositionMap);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WarBaseSaveRequest)GCHandledObjects.GCHandleToObject(instance)).PositionMap = (PositionMap)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBaseSaveRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
