using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle.Replay
{
	public class HeroAbilityAction : IBattleAction, ISerializable
	{
		private const string TIME_KEY = "time";

		private const string TROOP_ID_KEY = "troopId";

		public const string ACTION_ID = "HeroAbilityActivated";

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

		public string ActionId
		{
			get
			{
				return "HeroAbilityActivated";
			}
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Time = Convert.ToUInt32(dictionary["time"], CultureInfo.InvariantCulture);
			this.TroopUid = (dictionary["troopId"] as string);
			return this;
		}

		public string ToJson()
		{
			return Serializer.Start().AddString("actionId", this.ActionId).Add<uint>("time", this.Time).AddString("troopId", this.TroopUid).End().ToString();
		}

		public HeroAbilityAction()
		{
		}

		protected internal HeroAbilityAction(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroAbilityAction)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroAbilityAction)GCHandledObjects.GCHandleToObject(instance)).ActionId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroAbilityAction)GCHandledObjects.GCHandleToObject(instance)).TroopUid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HeroAbilityAction)GCHandledObjects.GCHandleToObject(instance)).TroopUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroAbilityAction)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
