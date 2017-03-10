using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Pvp
{
	public class PvpRevengeRequest : PlayerIdChecksumRequest
	{
		public string OpponentId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("opponentId", this.OpponentId);
			return startedSerializer.End().ToString();
		}

		public PvpRevengeRequest()
		{
		}

		protected internal PvpRevengeRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpRevengeRequest)GCHandledObjects.GCHandleToObject(instance)).OpponentId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PvpRevengeRequest)GCHandledObjects.GCHandleToObject(instance)).OpponentId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpRevengeRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
