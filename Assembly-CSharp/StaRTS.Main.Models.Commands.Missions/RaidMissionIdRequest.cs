using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Missions
{
	public class RaidMissionIdRequest : PlayerIdChecksumRequest
	{
		public string RaidMissionUid
		{
			get;
			private set;
		}

		public RaidMissionIdRequest(string missionUid)
		{
			this.RaidMissionUid = missionUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("raidMissionId", this.RaidMissionUid);
			return startedSerializer.End().ToString();
		}

		protected internal RaidMissionIdRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionIdRequest)GCHandledObjects.GCHandleToObject(instance)).RaidMissionUid);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((RaidMissionIdRequest)GCHandledObjects.GCHandleToObject(instance)).RaidMissionUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidMissionIdRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
