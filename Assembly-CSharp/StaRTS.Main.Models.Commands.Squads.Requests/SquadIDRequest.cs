using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class SquadIDRequest : PlayerIdRequest
	{
		public string SquadId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("guildId", this.SquadId);
			return serializer.End().ToString();
		}

		public SquadIDRequest()
		{
		}

		protected internal SquadIDRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadIDRequest)GCHandledObjects.GCHandleToObject(instance)).SquadId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadIDRequest)GCHandledObjects.GCHandleToObject(instance)).SquadId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadIDRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
