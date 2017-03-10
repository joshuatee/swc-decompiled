using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class TournamentLeaderboardRequest : PlayerIdRequest
	{
		public string PlanetId
		{
			get;
			set;
		}

		public TournamentLeaderboardRequest(string planetId, string playerId)
		{
			this.PlanetId = planetId;
			base.PlayerId = playerId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("planetUid", this.PlanetId);
			return serializer.End().ToString();
		}

		protected internal TournamentLeaderboardRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentLeaderboardRequest)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TournamentLeaderboardRequest)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentLeaderboardRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
