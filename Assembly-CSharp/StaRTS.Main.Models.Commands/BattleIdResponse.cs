using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands
{
	public class BattleIdResponse : AbstractResponse
	{
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
				return null;
			}
			if (dictionary.ContainsKey("battleId"))
			{
				this.BattleId = (string)dictionary["battleId"];
			}
			return this;
		}

		public BattleIdResponse()
		{
		}

		protected internal BattleIdResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleIdResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleIdResponse)GCHandledObjects.GCHandleToObject(instance)).BattleId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BattleIdResponse)GCHandledObjects.GCHandleToObject(instance)).BattleId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
