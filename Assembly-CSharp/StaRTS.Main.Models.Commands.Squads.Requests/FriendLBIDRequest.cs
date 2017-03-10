using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class FriendLBIDRequest : PlayerIdRequest
	{
		public string FriendIDs
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("friendIds", this.FriendIDs);
			return serializer.End().ToString();
		}

		public FriendLBIDRequest()
		{
		}

		protected internal FriendLBIDRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FriendLBIDRequest)GCHandledObjects.GCHandleToObject(instance)).FriendIDs);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((FriendLBIDRequest)GCHandledObjects.GCHandleToObject(instance)).FriendIDs = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FriendLBIDRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
