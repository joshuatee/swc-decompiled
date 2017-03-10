using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Identity
{
	public class PlayerIdentityGetResponse : AbstractResponse
	{
		public PlayerIdentityInfo Info
		{
			get;
			private set;
		}

		public PlayerIdentityGetResponse()
		{
			this.Info = new PlayerIdentityInfo();
		}

		public override ISerializable FromObject(object obj)
		{
			this.Info.FromObject(obj);
			return this;
		}

		protected internal PlayerIdentityGetResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdentityGetResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdentityGetResponse)GCHandledObjects.GCHandleToObject(instance)).Info);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlayerIdentityGetResponse)GCHandledObjects.GCHandleToObject(instance)).Info = (PlayerIdentityInfo)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
