using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Responses
{
	public class FeaturedSquadsResponse : AbstractResponse
	{
		public List<object> SquadData
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			this.SquadData = new List<object>();
			List<object> list = obj as List<object>;
			if (list == null)
			{
				return this;
			}
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				this.SquadData.Add(list[i]);
				i++;
			}
			return this;
		}

		public FeaturedSquadsResponse()
		{
		}

		protected internal FeaturedSquadsResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FeaturedSquadsResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FeaturedSquadsResponse)GCHandledObjects.GCHandleToObject(instance)).SquadData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((FeaturedSquadsResponse)GCHandledObjects.GCHandleToObject(instance)).SquadData = (List<object>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
