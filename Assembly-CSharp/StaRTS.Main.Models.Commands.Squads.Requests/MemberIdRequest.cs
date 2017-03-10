using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class MemberIdRequest : PlayerIdRequest
	{
		public string MemberId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("memberId", this.MemberId);
			return serializer.End().ToString();
		}

		public MemberIdRequest()
		{
		}

		protected internal MemberIdRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MemberIdRequest)GCHandledObjects.GCHandleToObject(instance)).MemberId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((MemberIdRequest)GCHandledObjects.GCHandleToObject(instance)).MemberId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MemberIdRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
