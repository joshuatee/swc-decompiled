using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class ShareReplayRequest : PlayerIdRequest
	{
		public string BattleId
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("battleId", this.BattleId);
			serializer.AddString("message", this.Message);
			return serializer.End().ToString();
		}

		public ShareReplayRequest()
		{
		}

		protected internal ShareReplayRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShareReplayRequest)GCHandledObjects.GCHandleToObject(instance)).BattleId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShareReplayRequest)GCHandledObjects.GCHandleToObject(instance)).Message);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ShareReplayRequest)GCHandledObjects.GCHandleToObject(instance)).BattleId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ShareReplayRequest)GCHandledObjects.GCHandleToObject(instance)).Message = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShareReplayRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
