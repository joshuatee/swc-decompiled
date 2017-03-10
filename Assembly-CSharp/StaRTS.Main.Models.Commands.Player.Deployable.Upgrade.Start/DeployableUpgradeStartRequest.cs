using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Deployable.Upgrade.Start
{
	public class DeployableUpgradeStartRequest : PlayerIdChecksumRequest
	{
		public string BuildingId
		{
			get;
			set;
		}

		public string TroopUid
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("playerId", base.PlayerId);
			startedSerializer.AddString("buildingId", this.BuildingId);
			startedSerializer.AddString("troopUid", this.TroopUid);
			return startedSerializer.End().ToString();
		}

		public DeployableUpgradeStartRequest()
		{
		}

		protected internal DeployableUpgradeStartRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).TroopUid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DeployableUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).BuildingId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DeployableUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).TroopUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableUpgradeStartRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
