using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Responses
{
	public class SquadWarBuffBaseResponse : AbstractResponse
	{
		public SquadWarBuffBaseData SquadWarBuffBaseData
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			if (!(obj is Dictionary<string, object>))
			{
				return this;
			}
			this.SquadWarBuffBaseData = new SquadWarBuffBaseData();
			this.SquadWarBuffBaseData.FromObject(obj);
			return this;
		}

		public SquadWarBuffBaseResponse()
		{
		}

		protected internal SquadWarBuffBaseResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarBuffBaseResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarBuffBaseResponse)GCHandledObjects.GCHandleToObject(instance)).SquadWarBuffBaseData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadWarBuffBaseResponse)GCHandledObjects.GCHandleToObject(instance)).SquadWarBuffBaseData = (SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
