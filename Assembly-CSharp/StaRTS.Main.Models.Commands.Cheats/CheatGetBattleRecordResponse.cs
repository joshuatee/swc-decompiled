using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatGetBattleRecordResponse : AbstractResponse
	{
		public BattleRecord ReplayData
		{
			get;
			set;
		}

		public string BattleId
		{
			get;
			set;
		}

		public CheatGetBattleRecordResponse(string battleId)
		{
			this.BattleId = battleId;
		}

		public override ISerializable FromObject(object obj)
		{
			this.ReplayData = new BattleRecord();
			this.ReplayData.FromObject(obj);
			return this;
		}

		protected internal CheatGetBattleRecordResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatGetBattleRecordResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatGetBattleRecordResponse)GCHandledObjects.GCHandleToObject(instance)).BattleId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatGetBattleRecordResponse)GCHandledObjects.GCHandleToObject(instance)).ReplayData);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CheatGetBattleRecordResponse)GCHandledObjects.GCHandleToObject(instance)).BattleId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((CheatGetBattleRecordResponse)GCHandledObjects.GCHandleToObject(instance)).ReplayData = (BattleRecord)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
