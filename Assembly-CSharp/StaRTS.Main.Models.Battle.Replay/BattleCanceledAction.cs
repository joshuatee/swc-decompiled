using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle.Replay
{
	public class BattleCanceledAction : IBattleAction, ISerializable
	{
		private const string TIME_KEY = "time";

		public const string ACTION_ID = "BattleCanceled";

		public uint Time
		{
			get;
			set;
		}

		public string ActionId
		{
			get
			{
				return "BattleCanceled";
			}
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Time = Convert.ToUInt32(dictionary["time"], CultureInfo.InvariantCulture);
			return this;
		}

		public string ToJson()
		{
			return Serializer.Start().AddString("actionId", this.ActionId).Add<uint>("time", this.Time).End().ToString();
		}

		public BattleCanceledAction()
		{
		}

		protected internal BattleCanceledAction(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleCanceledAction)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleCanceledAction)GCHandledObjects.GCHandleToObject(instance)).ActionId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleCanceledAction)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
