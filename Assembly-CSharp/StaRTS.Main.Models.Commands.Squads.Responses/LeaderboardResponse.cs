using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Responses
{
	public class LeaderboardResponse : AbstractResponse
	{
		public List<object> TopData
		{
			get;
			private set;
		}

		public List<object> SurroundingData
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("leaders"))
			{
				this.TopData = (dictionary["leaders"] as List<object>);
			}
			if (dictionary.ContainsKey("surrounding"))
			{
				this.SurroundingData = (dictionary["surrounding"] as List<object>);
			}
			Service.Get<EventManager>().SendEvent(EventId.SquadLeaderboardUpdated, null);
			return this;
		}

		public LeaderboardResponse()
		{
		}

		protected internal LeaderboardResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardResponse)GCHandledObjects.GCHandleToObject(instance)).SurroundingData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardResponse)GCHandledObjects.GCHandleToObject(instance)).TopData);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LeaderboardResponse)GCHandledObjects.GCHandleToObject(instance)).SurroundingData = (List<object>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LeaderboardResponse)GCHandledObjects.GCHandleToObject(instance)).TopData = (List<object>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
