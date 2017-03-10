using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public class Mission : ISerializable
	{
		public string Uid
		{
			get;
			set;
		}

		public string CampaignUid
		{
			get;
			set;
		}

		public int EarnedStars
		{
			get;
			set;
		}

		public MissionStatus Status
		{
			get;
			private set;
		}

		public int GrindMissionRetries
		{
			get;
			set;
		}

		public Dictionary<string, int> Counters
		{
			get;
			set;
		}

		public int[] LootRemaining
		{
			get;
			set;
		}

		public bool Locked
		{
			get;
			set;
		}

		public bool Activated
		{
			get;
			set;
		}

		public bool Completed
		{
			get;
			set;
		}

		public bool Collected
		{
			get;
			set;
		}

		public static Mission CreateFromCampaignMissionVO(CampaignMissionVO missionVO)
		{
			return new Mission
			{
				Uid = missionVO.Uid,
				CampaignUid = missionVO.CampaignUid,
				Locked = false,
				Activated = false,
				Completed = false,
				EarnedStars = 0,
				Counters = null,
				LootRemaining = null,
				GrindMissionRetries = 0
			};
		}

		private void EnsureCounters()
		{
			if (this.Counters == null)
			{
				this.Counters = new Dictionary<string, int>();
			}
		}

		private void EnsureLootRemaining()
		{
			if (this.LootRemaining == null)
			{
				int num = 6;
				this.LootRemaining = new int[num];
				for (int i = 0; i < num; i++)
				{
					this.LootRemaining[i] = -1;
				}
			}
		}

		public void AddToCounter(string counterKey, int delta)
		{
			this.EnsureCounters();
			if (this.Counters.ContainsKey(counterKey))
			{
				Dictionary<string, int> counters = this.Counters;
				counters[counterKey] += delta;
				return;
			}
			this.Counters.Add(counterKey, delta);
		}

		public void SetLootRemaining(int credits, int materials, int contraband)
		{
			this.EnsureLootRemaining();
			this.LootRemaining[1] = ((credits > 0) ? credits : 0);
			this.LootRemaining[2] = ((materials > 0) ? materials : 0);
			this.LootRemaining[3] = ((contraband > 0) ? contraband : 0);
		}

		public string ToJson()
		{
			return "{}";
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Uid = dictionary["uid"].ToString();
			this.CampaignUid = dictionary["campaignUid"].ToString();
			this.EarnedStars = Convert.ToInt32(dictionary["earnedStars"], CultureInfo.InvariantCulture);
			this.Counters = null;
			if (dictionary.ContainsKey("grindMissionRetries"))
			{
				this.GrindMissionRetries = Convert.ToInt32(dictionary["grindMissionRetries"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("counters"))
			{
				Dictionary<string, object> dictionary2 = dictionary["counters"] as Dictionary<string, object>;
				if (dictionary2 != null && dictionary2.Count != 0)
				{
					this.EnsureCounters();
					foreach (KeyValuePair<string, object> current in dictionary2)
					{
						this.Counters.Add(current.get_Key(), Convert.ToInt32(current.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			this.LootRemaining = null;
			if (dictionary.ContainsKey("lootRemaining"))
			{
				Dictionary<string, object> dictionary3 = dictionary["lootRemaining"] as Dictionary<string, object>;
				if (dictionary3 != null && dictionary3.Count != 0)
				{
					this.EnsureLootRemaining();
					foreach (KeyValuePair<string, object> current2 in dictionary3)
					{
						CurrencyType currencyType = StringUtils.ParseEnum<CurrencyType>(current2.get_Key());
						this.LootRemaining[(int)currencyType] = Convert.ToInt32(current2.get_Value(), CultureInfo.InvariantCulture);
					}
				}
			}
			if (dictionary.ContainsKey("status"))
			{
				this.Status = StringUtils.ParseEnum<MissionStatus>(dictionary["status"] as string);
				if (this.Status == MissionStatus.Claimed)
				{
					this.Collected = true;
					this.Activated = true;
				}
				if (this.Status == MissionStatus.Completed || this.Status == MissionStatus.Claimed)
				{
					this.Completed = true;
					this.Activated = true;
				}
				else if (this.Status != MissionStatus.Default)
				{
					this.Activated = true;
				}
				this.Locked = false;
			}
			else
			{
				this.Locked = true;
				this.Completed = false;
				this.Collected = false;
			}
			return this;
		}

		public Mission()
		{
		}

		protected internal Mission(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).AddToCounter(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Mission.CreateFromCampaignMissionVO((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).EnsureCounters();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).EnsureLootRemaining();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).Activated);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).CampaignUid);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).Collected);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).Completed);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).Counters);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).EarnedStars);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).GrindMissionRetries);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).Locked);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).LootRemaining);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).Status);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).Activated = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).CampaignUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).Collected = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).Completed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).Counters = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).EarnedStars = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).GrindMissionRetries = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).Locked = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).LootRemaining = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).Status = (MissionStatus)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((Mission)GCHandledObjects.GCHandleToObject(instance)).SetLootRemaining(*(int*)args, *(int*)(args + 1), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Mission)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
