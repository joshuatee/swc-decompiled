using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatFastForwardContractsRequest : PlayerIdRequest
	{
		public double TimeOffset
		{
			get;
			private set;
		}

		public CheatFastForwardContractsRequest(double timeOffset)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.TimeOffset = timeOffset;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("offset", this.TimeOffset.ToString());
			return serializer.End().ToString();
		}

		protected internal CheatFastForwardContractsRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CheatFastForwardContractsRequest)GCHandledObjects.GCHandleToObject(instance)).TimeOffset = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatFastForwardContractsRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
