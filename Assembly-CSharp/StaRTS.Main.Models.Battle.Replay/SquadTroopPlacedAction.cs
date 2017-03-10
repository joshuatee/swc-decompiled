using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle.Replay
{
	public class SquadTroopPlacedAction : IBattleAction, ISerializable
	{
		private const string TIME_KEY = "time";

		private const string BOARD_X_KEY = "boardX";

		private const string BOARD_Z_KEY = "boardZ";

		public const string ACTION_ID = "SquadTroopPlaced";

		public uint Time
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

		public string ActionId
		{
			get
			{
				return "SquadTroopPlaced";
			}
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Time = Convert.ToUInt32(dictionary["time"], CultureInfo.InvariantCulture);
			this.BoardX = Convert.ToInt32(dictionary["boardX"], CultureInfo.InvariantCulture);
			this.BoardZ = Convert.ToInt32(dictionary["boardZ"], CultureInfo.InvariantCulture);
			return this;
		}

		public string ToJson()
		{
			return Serializer.Start().AddString("actionId", this.ActionId).Add<uint>("time", this.Time).Add<int>("boardX", this.BoardX).Add<int>("boardZ", this.BoardZ).End().ToString();
		}

		public SquadTroopPlacedAction()
		{
		}

		protected internal SquadTroopPlacedAction(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadTroopPlacedAction)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadTroopPlacedAction)GCHandledObjects.GCHandleToObject(instance)).ActionId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadTroopPlacedAction)GCHandledObjects.GCHandleToObject(instance)).BoardX);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadTroopPlacedAction)GCHandledObjects.GCHandleToObject(instance)).BoardZ);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadTroopPlacedAction)GCHandledObjects.GCHandleToObject(instance)).BoardX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadTroopPlacedAction)GCHandledObjects.GCHandleToObject(instance)).BoardZ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadTroopPlacedAction)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
