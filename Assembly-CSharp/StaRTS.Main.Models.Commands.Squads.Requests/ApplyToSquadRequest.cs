using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class ApplyToSquadRequest : PlayerIdRequest
	{
		public string SquadId
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
			serializer.AddString("guildId", this.SquadId);
			serializer.AddString("message", this.Message);
			return serializer.End().ToString();
		}

		public ApplyToSquadRequest()
		{
		}

		protected internal ApplyToSquadRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ApplyToSquadRequest)GCHandledObjects.GCHandleToObject(instance)).Message);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ApplyToSquadRequest)GCHandledObjects.GCHandleToObject(instance)).SquadId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ApplyToSquadRequest)GCHandledObjects.GCHandleToObject(instance)).Message = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ApplyToSquadRequest)GCHandledObjects.GCHandleToObject(instance)).SquadId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ApplyToSquadRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
