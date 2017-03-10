using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Rearm
{
	public class RearmTrapRequest : PlayerIdChecksumRequest
	{
		public List<string> BuildingIds
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddArrayOfPrimitives<string>("buildingIds", this.BuildingIds);
			return startedSerializer.End().ToString();
		}

		public RearmTrapRequest()
		{
		}

		protected internal RearmTrapRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RearmTrapRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingIds);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((RearmTrapRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingIds = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RearmTrapRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
