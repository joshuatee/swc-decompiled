using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands
{
	public class AddPatchRequest : PlayerIdRequest
	{
		public string Patch
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			if (!string.IsNullOrEmpty(this.Patch))
			{
				serializer.AddString("patch", this.Patch);
			}
			return serializer.End().ToString();
		}

		public AddPatchRequest()
		{
		}

		protected internal AddPatchRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AddPatchRequest)GCHandledObjects.GCHandleToObject(instance)).Patch);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AddPatchRequest)GCHandledObjects.GCHandleToObject(instance)).Patch = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AddPatchRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
