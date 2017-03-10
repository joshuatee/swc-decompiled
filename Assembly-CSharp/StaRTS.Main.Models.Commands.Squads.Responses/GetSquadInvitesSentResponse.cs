using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Responses
{
	public class GetSquadInvitesSentResponse : AbstractResponse
	{
		public List<string> PlayersWithPendingInvites
		{
			get;
			private set;
		}

		public GetSquadInvitesSentResponse()
		{
			this.PlayersWithPendingInvites = new List<string>();
		}

		public override ISerializable FromObject(object obj)
		{
			List<object> list = obj as List<object>;
			if (list != null)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					this.PlayersWithPendingInvites.Add(list[i] as string);
					i++;
				}
			}
			return this;
		}

		protected internal GetSquadInvitesSentResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadInvitesSentResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadInvitesSentResponse)GCHandledObjects.GCHandleToObject(instance)).PlayersWithPendingInvites);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GetSquadInvitesSentResponse)GCHandledObjects.GCHandleToObject(instance)).PlayersWithPendingInvites = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
