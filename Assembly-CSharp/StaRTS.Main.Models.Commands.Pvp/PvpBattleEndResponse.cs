using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Pvp
{
	public class PvpBattleEndResponse : AbstractResponse
	{
		public BattleEntry BattleEntry
		{
			get;
			private set;
		}

		public Tournament TournamentData
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			this.BattleEntry = (new BattleEntry().FromObject(obj) as BattleEntry);
			this.BattleEntry.SetupExpendedTroops();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("attackerTournament"))
			{
				this.TournamentData = new Tournament();
				this.TournamentData.FromObject(dictionary["attackerTournament"]);
			}
			return this;
		}

		public PvpBattleEndResponse()
		{
		}

		protected internal PvpBattleEndResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpBattleEndResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpBattleEndResponse)GCHandledObjects.GCHandleToObject(instance)).BattleEntry);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpBattleEndResponse)GCHandledObjects.GCHandleToObject(instance)).TournamentData);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PvpBattleEndResponse)GCHandledObjects.GCHandleToObject(instance)).BattleEntry = (BattleEntry)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PvpBattleEndResponse)GCHandledObjects.GCHandleToObject(instance)).TournamentData = (Tournament)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
