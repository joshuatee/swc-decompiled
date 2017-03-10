using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetResourcesRequest : PlayerIdRequest
	{
		public int Credits
		{
			get;
			set;
		}

		public int Materials
		{
			get;
			set;
		}

		public int Contraband
		{
			get;
			set;
		}

		public int Crystals
		{
			get;
			set;
		}

		public int Reputation
		{
			get;
			set;
		}

		public CheatSetResourcesRequest()
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.Credits = -1;
			this.Materials = -1;
			this.Contraband = -1;
			this.Crystals = -1;
			this.Reputation = -1;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			if (this.Credits >= 0)
			{
				serializer.Add<int>("credits", this.Credits);
			}
			if (this.Materials >= 0)
			{
				serializer.Add<int>("materials", this.Materials);
			}
			if (this.Contraband >= 0)
			{
				serializer.Add<int>("contraband", this.Contraband);
			}
			if (this.Crystals >= 0)
			{
				serializer.Add<int>("crystals", this.Crystals);
			}
			if (this.Reputation >= 0)
			{
				serializer.Add<int>("reputation", this.Reputation);
			}
			return serializer.End().ToString();
		}

		protected internal CheatSetResourcesRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Contraband);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Credits);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Crystals);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Materials);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Reputation);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Crystals = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).Reputation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetResourcesRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
