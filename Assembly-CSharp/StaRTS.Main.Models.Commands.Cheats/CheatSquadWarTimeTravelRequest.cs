using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSquadWarTimeTravelRequest : PlayerIdRequest
	{
		public string WarId
		{
			get;
			private set;
		}

		public double TimeOffset
		{
			get;
			private set;
		}

		public CheatSquadWarTimeTravelRequest(string warId, double timeOffset)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.WarId = warId;
			this.TimeOffset = timeOffset;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("warId", this.WarId);
			serializer.AddString("offset", this.TimeOffset.ToString());
			return serializer.End().ToString();
		}

		protected internal CheatSquadWarTimeTravelRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSquadWarTimeTravelRequest)GCHandledObjects.GCHandleToObject(instance)).WarId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CheatSquadWarTimeTravelRequest)GCHandledObjects.GCHandleToObject(instance)).TimeOffset = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CheatSquadWarTimeTravelRequest)GCHandledObjects.GCHandleToObject(instance)).WarId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSquadWarTimeTravelRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
