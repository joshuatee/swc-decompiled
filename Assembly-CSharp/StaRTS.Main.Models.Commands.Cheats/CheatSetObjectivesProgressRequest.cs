using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetObjectivesProgressRequest : PlayerIdRequest
	{
		private string uid;

		private int count;

		public CheatSetObjectivesProgressRequest(string uid, int count)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.uid = uid;
			this.count = count;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("uid", this.uid);
			serializer.Add<int>("count", this.count);
			return serializer.End().ToString();
		}

		protected internal CheatSetObjectivesProgressRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetObjectivesProgressRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
