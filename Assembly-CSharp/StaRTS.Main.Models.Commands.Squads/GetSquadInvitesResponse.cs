using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Squads;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads
{
	public class GetSquadInvitesResponse : AbstractResponse
	{
		public List<SquadInvite> SquadInvites
		{
			get;
			private set;
		}

		public GetSquadInvitesResponse()
		{
			this.SquadInvites = new List<SquadInvite>();
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
					SquadInvite squadInvite = new SquadInvite();
					squadInvite.FromObject(list[i]);
					this.SquadInvites.Add(squadInvite);
					i++;
				}
			}
			return this;
		}

		protected internal GetSquadInvitesResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadInvitesResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadInvitesResponse)GCHandledObjects.GCHandleToObject(instance)).SquadInvites);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GetSquadInvitesResponse)GCHandledObjects.GCHandleToObject(instance)).SquadInvites = (List<SquadInvite>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
