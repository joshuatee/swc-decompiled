using StaRTS.Utils;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle.Replay
{
	public class ChampionDeployedAction : IBattleAction, ISerializable
	{
		private const string TIME_KEY = "time";

		private const string TROOP_ID_KEY = "troopId";

		private const string BOARD_X_KEY = "boardX";

		private const string BOARD_Z_KEY = "boardZ";

		private const string TEAM_TYPE = "teamType";

		public const string ACTION_ID = "ChampionDeployed";

		public uint Time
		{
			get;
			set;
		}

		public string TroopUid
		{
			get;
			set;
		}

		public int BoardX
		{
			get;
			set;
		}

		public int BoardZ
		{
			get;
			set;
		}

		public TeamType TeamType
		{
			get;
			set;
		}

		public string ActionId
		{
			get
			{
				return "ChampionDeployed";
			}
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Time = Convert.ToUInt32(dictionary["time"], CultureInfo.InvariantCulture);
			this.TroopUid = (dictionary["troopId"] as string);
			this.BoardX = Convert.ToInt32(dictionary["boardX"], CultureInfo.InvariantCulture);
			this.BoardZ = Convert.ToInt32(dictionary["boardZ"], CultureInfo.InvariantCulture);
			this.TeamType = StringUtils.ParseEnum<TeamType>(dictionary["teamType"] as string);
			return this;
		}

		public string ToJson()
		{
			return Serializer.Start().AddString("actionId", this.ActionId).Add<uint>("time", this.Time).AddString("troopId", this.TroopUid).Add<int>("boardX", this.BoardX).Add<int>("boardZ", this.BoardZ).AddString("teamType", this.TeamType.ToString()).End().ToString();
		}

		public ChampionDeployedAction()
		{
		}

		protected internal ChampionDeployedAction(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).ActionId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).BoardX);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).BoardZ);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).TeamType);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).TroopUid);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).BoardX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).BoardZ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).TeamType = (TeamType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).TroopUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployedAction)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
