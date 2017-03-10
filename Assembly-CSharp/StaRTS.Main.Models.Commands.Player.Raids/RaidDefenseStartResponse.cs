using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Raids
{
	public class RaidDefenseStartResponse : AbstractResponse
	{
		private const string BATTLE_ID = "battleId";

		public string BattleId
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().Error("Attempted to create invalid RaidDefenseStartResponse.");
				return null;
			}
			if (dictionary.ContainsKey("battleId"))
			{
				this.BattleId = (string)dictionary["battleId"];
			}
			return this;
		}

		public RaidDefenseStartResponse()
		{
		}

		protected internal RaidDefenseStartResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseStartResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseStartResponse)GCHandledObjects.GCHandleToObject(instance)).BattleId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((RaidDefenseStartResponse)GCHandledObjects.GCHandleToObject(instance)).BattleId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
