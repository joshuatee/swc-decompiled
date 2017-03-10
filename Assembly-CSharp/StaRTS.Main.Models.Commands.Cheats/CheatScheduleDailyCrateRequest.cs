using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatScheduleDailyCrateRequest : PlayerIdRequest
	{
		public int RewardHour
		{
			get;
			private set;
		}

		public int RewardMinute
		{
			get;
			private set;
		}

		public CheatScheduleDailyCrateRequest(int rewardHour, int rewardMinute)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.RewardHour = rewardHour;
			this.RewardMinute = rewardMinute;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.Add<int>("rewardHour", this.RewardHour);
			serializer.Add<int>("rewardMinute", this.RewardMinute);
			return serializer.End().ToString();
		}

		protected internal CheatScheduleDailyCrateRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatScheduleDailyCrateRequest)GCHandledObjects.GCHandleToObject(instance)).RewardHour);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatScheduleDailyCrateRequest)GCHandledObjects.GCHandleToObject(instance)).RewardMinute);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CheatScheduleDailyCrateRequest)GCHandledObjects.GCHandleToObject(instance)).RewardHour = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CheatScheduleDailyCrateRequest)GCHandledObjects.GCHandleToObject(instance)).RewardMinute = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatScheduleDailyCrateRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
