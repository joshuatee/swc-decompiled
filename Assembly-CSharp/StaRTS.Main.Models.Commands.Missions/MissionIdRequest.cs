using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Missions
{
	public class MissionIdRequest : PlayerIdChecksumRequest
	{
		public string MissionUid
		{
			get;
			private set;
		}

		public string BattleUid
		{
			get;
			private set;
		}

		public MissionIdRequest(string missionUid)
		{
			this.MissionUid = missionUid;
		}

		public MissionIdRequest(string missionUid, string battleUid)
		{
			this.MissionUid = missionUid;
			this.BattleUid = battleUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("missionUid", this.MissionUid);
			if (this.BattleUid != null)
			{
				startedSerializer.AddString("battleUid", this.BattleUid);
			}
			return startedSerializer.End().ToString();
		}

		protected internal MissionIdRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionIdRequest)GCHandledObjects.GCHandleToObject(instance)).BattleUid);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionIdRequest)GCHandledObjects.GCHandleToObject(instance)).MissionUid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((MissionIdRequest)GCHandledObjects.GCHandleToObject(instance)).BattleUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((MissionIdRequest)GCHandledObjects.GCHandleToObject(instance)).MissionUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionIdRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
