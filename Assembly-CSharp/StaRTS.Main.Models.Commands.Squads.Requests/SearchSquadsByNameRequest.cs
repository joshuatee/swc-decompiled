using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class SearchSquadsByNameRequest : PlayerIdRequest
	{
		public string SearchTerm
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("searchTerm", this.SearchTerm);
			return serializer.End().ToString();
		}

		public SearchSquadsByNameRequest()
		{
		}

		protected internal SearchSquadsByNameRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SearchSquadsByNameRequest)GCHandledObjects.GCHandleToObject(instance)).SearchTerm);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SearchSquadsByNameRequest)GCHandledObjects.GCHandleToObject(instance)).SearchTerm = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SearchSquadsByNameRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
