using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Identity
{
	public class PlayerIdentityRequest : PlayerIdChecksumRequest
	{
		public int IdentityIndex
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.Add<int>("identityIndex", this.IdentityIndex);
			return startedSerializer.End().ToString();
		}

		public PlayerIdentityRequest()
		{
		}

		protected internal PlayerIdentityRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdentityRequest)GCHandledObjects.GCHandleToObject(instance)).IdentityIndex);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlayerIdentityRequest)GCHandledObjects.GCHandleToObject(instance)).IdentityIndex = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdentityRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
