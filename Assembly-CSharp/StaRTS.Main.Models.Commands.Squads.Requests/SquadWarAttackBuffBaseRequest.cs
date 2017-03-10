using StaRTS.Externals.FileManagement;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class SquadWarAttackBuffBaseRequest : PlayerIdChecksumRequest
	{
		private string buffBaseUid;

		public SquadWarAttackBuffBaseRequest(string buffBaseUid)
		{
			this.buffBaseUid = buffBaseUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("buffBaseUid", this.buffBaseUid);
			startedSerializer.AddString("cmsVersion", Service.Get<FMS>().GetFileVersion("patches/base.json").ToString());
			startedSerializer.AddString("battleVersion", "21.0");
			return startedSerializer.End().ToString();
		}

		protected internal SquadWarAttackBuffBaseRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarAttackBuffBaseRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
