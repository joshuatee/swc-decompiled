using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands
{
	public class PlayerResourceResponse : AbstractResponse
	{
		public int CrystalsDelta
		{
			get;
			protected set;
		}

		public override ISerializable FromObject(object obj)
		{
			return this;
		}

		public PlayerResourceResponse()
		{
		}

		protected internal PlayerResourceResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerResourceResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerResourceResponse)GCHandledObjects.GCHandleToObject(instance)).CrystalsDelta);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlayerResourceResponse)GCHandledObjects.GCHandleToObject(instance)).CrystalsDelta = *(int*)args;
			return -1L;
		}
	}
}
