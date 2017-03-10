using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetTroopDonateRepRequest : PlayerIdRequest
	{
		public int TroopsDonated
		{
			get;
			set;
		}

		public CheatSetTroopDonateRepRequest(int troopsDonated)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.TroopsDonated = troopsDonated;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.Add<int>("donationCount", this.TroopsDonated);
			return serializer.End().ToString();
		}

		protected internal CheatSetTroopDonateRepRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetTroopDonateRepRequest)GCHandledObjects.GCHandleToObject(instance)).TroopsDonated);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CheatSetTroopDonateRepRequest)GCHandledObjects.GCHandleToObject(instance)).TroopsDonated = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetTroopDonateRepRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
