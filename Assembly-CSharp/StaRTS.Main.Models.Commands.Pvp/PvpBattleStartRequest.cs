using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Pvp
{
	public class PvpBattleStartRequest : PlayerIdChecksumRequest
	{
		private string planetId;

		public string TargetId
		{
			get;
			private set;
		}

		public string CmsVersion
		{
			get;
			private set;
		}

		public string PvpMissionUid
		{
			get;
			private set;
		}

		public PvpBattleStartRequest(string targetId, string cmsVersion, string pvpMissionUid)
		{
			this.TargetId = targetId;
			this.CmsVersion = cmsVersion;
			this.PvpMissionUid = pvpMissionUid;
			this.planetId = Service.Get<CurrentPlayer>().PlanetId;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("targetId", this.TargetId);
			startedSerializer.AddString("cmsVersion", this.CmsVersion);
			startedSerializer.AddString("battleVersion", "21.0");
			startedSerializer.AddString("simSeedA", Service.Get<BattleController>().SimSeed.SimSeedA.ToString());
			startedSerializer.AddString("simSeedB", Service.Get<BattleController>().SimSeed.SimSeedB.ToString());
			startedSerializer.AddString("planetId", this.planetId);
			if (this.PvpMissionUid != null)
			{
				startedSerializer.AddString("missionUid", this.PvpMissionUid);
			}
			return startedSerializer.End().ToString();
		}

		protected internal PvpBattleStartRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpBattleStartRequest)GCHandledObjects.GCHandleToObject(instance)).CmsVersion);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpBattleStartRequest)GCHandledObjects.GCHandleToObject(instance)).PvpMissionUid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpBattleStartRequest)GCHandledObjects.GCHandleToObject(instance)).TargetId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PvpBattleStartRequest)GCHandledObjects.GCHandleToObject(instance)).CmsVersion = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PvpBattleStartRequest)GCHandledObjects.GCHandleToObject(instance)).PvpMissionUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PvpBattleStartRequest)GCHandledObjects.GCHandleToObject(instance)).TargetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpBattleStartRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
