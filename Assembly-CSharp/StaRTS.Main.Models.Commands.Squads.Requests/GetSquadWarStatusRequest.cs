using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class GetSquadWarStatusRequest : PlayerIdRequest
	{
		public string WarId
		{
			get;
			set;
		}

		public GetSquadWarStatusRequest(string playerId, string warId)
		{
			base.PlayerId = playerId;
			this.WarId = warId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("warId", this.WarId);
			return serializer.End().ToString();
		}

		protected internal GetSquadWarStatusRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusRequest)GCHandledObjects.GCHandleToObject(instance)).WarId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GetSquadWarStatusRequest)GCHandledObjects.GCHandleToObject(instance)).WarId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
