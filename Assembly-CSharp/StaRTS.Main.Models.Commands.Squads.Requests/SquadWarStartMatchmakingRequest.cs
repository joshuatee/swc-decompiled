using StaRTS.Main.Models.Squads;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class SquadWarStartMatchmakingRequest : PlayerIdChecksumRequest
	{
		public List<string> Participants;

		public bool AllowSameFactionMatchMaking;

		public SquadWarStartMatchmakingRequest(SqmWarParticipantData participantData)
		{
			this.Participants = new List<string>();
			int i = 0;
			int count = participantData.Participants.Count;
			while (i < count)
			{
				this.Participants.Add(participantData.Participants[i]);
				i++;
			}
			this.AllowSameFactionMatchMaking = participantData.AllowSameFactionMatchMaking;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddArrayOfPrimitives<string>("participantIds", this.Participants);
			startedSerializer.AddBool("isSameFactionWarAllowed", this.AllowSameFactionMatchMaking);
			return startedSerializer.End().ToString();
		}

		protected internal SquadWarStartMatchmakingRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarStartMatchmakingRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
