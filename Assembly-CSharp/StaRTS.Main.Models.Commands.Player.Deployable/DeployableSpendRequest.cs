using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Deployable
{
	public class DeployableSpendRequest : PlayerIdRequest
	{
		public List<DeploymentRecord> units
		{
			get;
			set;
		}

		public string BattleId
		{
			get;
			private set;
		}

		public bool SquadDeployed
		{
			get;
			private set;
		}

		public DeployableSpendRequest(string battleId, uint time, int boardX, int boardZ)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.BattleId = battleId;
			DeploymentRecord item = new DeploymentRecord(null, "SquadTroopPlaced", time, boardX, boardZ);
			this.units = new List<DeploymentRecord>();
			this.units.Add(item);
			this.SquadDeployed = true;
		}

		public DeployableSpendRequest(string battleId, List<DeploymentRecord> units)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.BattleId = battleId;
			this.units = new List<DeploymentRecord>(units);
			this.SquadDeployed = false;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("battleId", this.BattleId);
			serializer.AddBool("guildTroopsSpent", this.SquadDeployed);
			serializer.AddArray<DeploymentRecord>("units", this.units);
			return serializer.End().ToString();
		}

		protected internal DeployableSpendRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableSpendRequest)GCHandledObjects.GCHandleToObject(instance)).BattleId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableSpendRequest)GCHandledObjects.GCHandleToObject(instance)).SquadDeployed);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableSpendRequest)GCHandledObjects.GCHandleToObject(instance)).units);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DeployableSpendRequest)GCHandledObjects.GCHandleToObject(instance)).BattleId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DeployableSpendRequest)GCHandledObjects.GCHandleToObject(instance)).SquadDeployed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DeployableSpendRequest)GCHandledObjects.GCHandleToObject(instance)).units = (List<DeploymentRecord>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableSpendRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
