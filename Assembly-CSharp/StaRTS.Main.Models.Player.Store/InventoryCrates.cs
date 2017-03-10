using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Store
{
	public class InventoryCrates : ISerializable
	{
		private const uint DAILY_CRATE_SYNC_BUFFER_SEC = 5u;

		private const string DAILY_CRATE_CONTEXT = "daily";

		public Dictionary<string, CrateData> Available
		{
			get;
			private set;
		}

		public uint NextDailyCrateTime
		{
			get;
			private set;
		}

		public InventoryCrates()
		{
			this.Available = new Dictionary<string, CrateData>();
		}

		public string ToJson()
		{
			return "{}";
		}

		public uint GetNextDailCrateTimeWithSyncBuffer()
		{
			return this.NextDailyCrateTime + 5u;
		}

		public void UpdateAndBadgeFromServerObject(object obj)
		{
			int count = this.Available.Count;
			this.FromObject(obj);
			int count2 = this.Available.Count;
			int delta = Math.Max(count2 - count, 1);
			GameUtils.UpdateInventoryCrateBadgeCount(delta);
		}

		public void UpdateBadgingBasedOnAvailableCrates()
		{
			ServerPlayerPrefs serverPlayerPrefs = Service.Get<ServerPlayerPrefs>();
			int num = Convert.ToInt32(serverPlayerPrefs.GetPref(ServerPref.NumInventoryCratesNotViewed), CultureInfo.InvariantCulture);
			int availableCountAfterLastView = this.GetAvailableCountAfterLastView();
			num = availableCountAfterLastView - num;
			GameUtils.UpdateInventoryCrateBadgeCount(num);
		}

		public CrateData GetDailyCrateIfAvailable()
		{
			CrateData crateData = null;
			foreach (CrateData current in this.Available.Values)
			{
				if (current.Context == "daily" && !current.Claimed && (crateData == null || current.ReceivedTimeStamp > crateData.ReceivedTimeStamp))
				{
					crateData = current;
				}
			}
			return crateData;
		}

		public string GetNextDailyCrateId()
		{
			string result = null;
			DayOfWeek dayOfWeek = DateUtils.DateFromSeconds(this.NextDailyCrateTime).get_DayOfWeek();
			int num = dayOfWeek + 6;
			if (num >= 7)
			{
				num %= 6;
			}
			string[] array = GameConstants.CRATE_DAY_OF_THE_WEEK_REWARD.Split(new char[]
			{
				' '
			});
			if (array.Length >= 7)
			{
				result = array[num];
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("InventoryCrates.GetNextDailyCrateId Day of the week list invalid");
			}
			return result;
		}

		private int GetAvailableCountAfterLastView()
		{
			int num = 0;
			uint time = ServerTime.Time;
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			uint pref = sharedPlayerPrefs.GetPref<uint>("HQInvLastViewTime");
			foreach (CrateData current in this.Available.Values)
			{
				if (current.ReceivedTimeStamp > pref && current.ExpiresTimeStamp > time)
				{
					num++;
				}
			}
			return num;
		}

		private void ParseDataObjectIntoAvailable(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, object> current in dictionary)
				{
					CrateData crateData = new CrateData();
					crateData.FromObject(current.get_Value());
					this.Available.Add(current.get_Key(), crateData);
				}
			}
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("available"))
			{
				this.Available.Clear();
				this.ParseDataObjectIntoAvailable(dictionary["available"]);
			}
			if (dictionary.ContainsKey("nextDailyCrateTime"))
			{
				this.NextDailyCrateTime = Convert.ToUInt32(dictionary["nextDailyCrateTime"], CultureInfo.InvariantCulture);
			}
			if (Service.Get<GameStateMachine>().CurrentState is HomeState)
			{
				Service.Get<InventoryCrateRewardController>().ScheduleGivingNextDailyCrate();
			}
			Service.Get<EventManager>().SendEvent(EventId.CrateInventoryUpdated, null);
			return this;
		}

		protected internal InventoryCrates(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).Available);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).GetAvailableCountAfterLastView());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).GetDailyCrateIfAvailable());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).GetNextDailyCrateId());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).ParseDataObjectIntoAvailable(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).Available = (Dictionary<string, CrateData>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).UpdateAndBadgeFromServerObject(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((InventoryCrates)GCHandledObjects.GCHandleToObject(instance)).UpdateBadgingBasedOnAvailableCrates();
			return -1L;
		}
	}
}
