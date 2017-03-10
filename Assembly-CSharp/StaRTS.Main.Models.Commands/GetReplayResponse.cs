using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands
{
	public class GetReplayResponse : AbstractResponse
	{
		public BattleEntry EntryData
		{
			get;
			set;
		}

		public BattleRecord ReplayData
		{
			get;
			set;
		}

		public Dictionary<string, object> Response
		{
			get;
			set;
		}

		public override ISerializable FromObject(object obj)
		{
			this.Response = (obj as Dictionary<string, object>);
			if (this.Response.ContainsKey("battleLog"))
			{
				this.EntryData = new BattleEntry();
				this.EntryData.FromObject(this.Response["battleLog"]);
				this.EntryData.SetupExpendedTroops();
			}
			if (this.Response.ContainsKey("replayData"))
			{
				this.ReplayData = new BattleRecord();
				this.ReplayData.FromObject(this.Response["replayData"]);
			}
			if (this.ReplayData.BattleType == BattleType.Pvp)
			{
				this.EntryData.AllowReplay = true;
			}
			return this;
		}

		public BattleRecord GetOriginalReplayRecord()
		{
			if (this.Response != null && this.Response.ContainsKey("replayData"))
			{
				this.ReplayData = new BattleRecord();
				this.ReplayData.FromObject(this.Response["replayData"]);
				return this.ReplayData;
			}
			return null;
		}

		public GetReplayResponse()
		{
		}

		protected internal GetReplayResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetReplayResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetReplayResponse)GCHandledObjects.GCHandleToObject(instance)).EntryData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetReplayResponse)GCHandledObjects.GCHandleToObject(instance)).ReplayData);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetReplayResponse)GCHandledObjects.GCHandleToObject(instance)).Response);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetReplayResponse)GCHandledObjects.GCHandleToObject(instance)).GetOriginalReplayRecord());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GetReplayResponse)GCHandledObjects.GCHandleToObject(instance)).EntryData = (BattleEntry)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GetReplayResponse)GCHandledObjects.GCHandleToObject(instance)).ReplayData = (BattleRecord)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GetReplayResponse)GCHandledObjects.GCHandleToObject(instance)).Response = (Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
