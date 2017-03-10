using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class SquadWarParticipantIdRequest : PlayerIdChecksumRequest
	{
		public string ParticipantId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("participantId", this.ParticipantId);
			return startedSerializer.End().ToString();
		}

		public SquadWarParticipantIdRequest()
		{
		}

		protected internal SquadWarParticipantIdRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarParticipantIdRequest)GCHandledObjects.GCHandleToObject(instance)).ParticipantId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadWarParticipantIdRequest)GCHandledObjects.GCHandleToObject(instance)).ParticipantId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarParticipantIdRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
