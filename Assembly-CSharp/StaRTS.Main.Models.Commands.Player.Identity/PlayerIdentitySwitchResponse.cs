using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Identity
{
	public class PlayerIdentitySwitchResponse : AbstractResponse
	{
		public string PlayerId
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			this.PlayerId = (obj as string);
			return this;
		}

		public PlayerIdentitySwitchResponse()
		{
		}

		protected internal PlayerIdentitySwitchResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdentitySwitchResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdentitySwitchResponse)GCHandledObjects.GCHandleToObject(instance)).PlayerId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlayerIdentitySwitchResponse)GCHandledObjects.GCHandleToObject(instance)).PlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
