using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Objectives
{
	public class ClaimObjectiveRequest : PlayerIdRequest
	{
		public string PlanetId
		{
			get;
			set;
		}

		public string ObjectiveId
		{
			get;
			set;
		}

		public ClaimObjectiveRequest(string playerId, string planetId, string objectiveId)
		{
			this.PlanetId = planetId;
			base.PlayerId = playerId;
			this.ObjectiveId = objectiveId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			if (!string.IsNullOrEmpty(this.PlanetId) && !string.IsNullOrEmpty(this.ObjectiveId))
			{
				serializer.AddString("planetId", this.PlanetId);
				serializer.AddString("objectiveId", this.ObjectiveId);
			}
			return serializer.End().ToString();
		}

		protected internal ClaimObjectiveRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ClaimObjectiveRequest)GCHandledObjects.GCHandleToObject(instance)).ObjectiveId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ClaimObjectiveRequest)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ClaimObjectiveRequest)GCHandledObjects.GCHandleToObject(instance)).ObjectiveId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ClaimObjectiveRequest)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ClaimObjectiveRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
