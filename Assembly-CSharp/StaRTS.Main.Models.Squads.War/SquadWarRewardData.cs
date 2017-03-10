using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Squads.War
{
	public class SquadWarRewardData : ISerializable
	{
		public string WarId
		{
			get;
			private set;
		}

		public string CrateId
		{
			get;
			private set;
		}

		public uint ExpireDate
		{
			get;
			private set;
		}

		public int RewardHqLevel
		{
			get;
			private set;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("warId"))
			{
				this.WarId = Convert.ToString(dictionary["warId"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("crateTier"))
			{
				this.CrateId = Convert.ToString(dictionary["crateTier"], CultureInfo.InvariantCulture);
			}
			else if (dictionary.ContainsKey("crateId"))
			{
				this.CrateId = Convert.ToString(dictionary["crateId"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("expiry"))
			{
				this.ExpireDate = Convert.ToUInt32(dictionary["expiry"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("hqLevel"))
			{
				this.RewardHqLevel = Convert.ToInt32(dictionary["hqLevel"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public string ToJson()
		{
			return Serializer.Start().End().ToString();
		}

		public SquadWarRewardData()
		{
		}

		protected internal SquadWarRewardData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarRewardData)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarRewardData)GCHandledObjects.GCHandleToObject(instance)).CrateId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarRewardData)GCHandledObjects.GCHandleToObject(instance)).RewardHqLevel);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarRewardData)GCHandledObjects.GCHandleToObject(instance)).WarId);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadWarRewardData)GCHandledObjects.GCHandleToObject(instance)).CrateId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadWarRewardData)GCHandledObjects.GCHandleToObject(instance)).RewardHqLevel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadWarRewardData)GCHandledObjects.GCHandleToObject(instance)).WarId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarRewardData)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
