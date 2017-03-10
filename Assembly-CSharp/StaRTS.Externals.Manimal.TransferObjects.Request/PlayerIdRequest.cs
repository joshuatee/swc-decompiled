using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Request
{
	public class PlayerIdRequest : AbstractRequest
	{
		public string PlayerId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", this.PlayerId);
			return serializer.End().ToString();
		}

		public PlayerIdRequest()
		{
		}

		protected internal PlayerIdRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdRequest)GCHandledObjects.GCHandleToObject(instance)).PlayerId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlayerIdRequest)GCHandledObjects.GCHandleToObject(instance)).PlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
