using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Store
{
	public class BuyMultiResourceRequest : PlayerIdChecksumRequest
	{
		private int credits;

		private int materials;

		private int contraband;

		private string subtype;

		public BuyMultiResourceRequest(int credits, int materials, int contraband, string subtype)
		{
			this.credits = credits;
			this.materials = materials;
			this.contraband = contraband;
			this.subtype = subtype;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.Add<int>("credits", this.credits);
			startedSerializer.Add<int>("materials", this.materials);
			startedSerializer.Add<int>("contraband", this.contraband);
			startedSerializer.AddString("subtype", this.subtype);
			return startedSerializer.End().ToString();
		}

		protected internal BuyMultiResourceRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuyMultiResourceRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
