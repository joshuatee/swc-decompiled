using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Missions
{
	public class GetMissionMapResponse : AbstractResponse
	{
		public string BattleUid
		{
			get;
			private set;
		}

		public GetMissionMapResponse()
		{
		}

		public override ISerializable FromObject(object obj)
		{
			this.BattleUid = (string)obj;
			return this;
		}

		protected internal GetMissionMapResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetMissionMapResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetMissionMapResponse)GCHandledObjects.GCHandleToObject(instance)).BattleUid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GetMissionMapResponse)GCHandledObjects.GCHandleToObject(instance)).BattleUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
