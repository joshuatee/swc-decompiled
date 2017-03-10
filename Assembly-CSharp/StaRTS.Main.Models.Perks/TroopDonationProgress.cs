using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Perks
{
	public class TroopDonationProgress : ISerializable
	{
		public int DonationCount
		{
			get;
			set;
		}

		public int LastTrackedDonationTime
		{
			get;
			set;
		}

		public int DonationCooldownEndTime
		{
			get;
			set;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("donationCount"))
			{
				this.DonationCount = Convert.ToInt32(dictionary["donationCount"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("lastTrackedDonationTime"))
			{
				this.LastTrackedDonationTime = Convert.ToInt32(dictionary["lastTrackedDonationTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("repDonationCooldownEndTime"))
			{
				this.DonationCooldownEndTime = Convert.ToInt32(dictionary["repDonationCooldownEndTime"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public string ToJson()
		{
			Service.Get<StaRTSLogger>().Warn("Attempting to inappropriately serialize TroopDonationInfo");
			return string.Empty;
		}

		public TroopDonationProgress()
		{
		}

		protected internal TroopDonationProgress(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonationProgress)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonationProgress)GCHandledObjects.GCHandleToObject(instance)).DonationCooldownEndTime);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonationProgress)GCHandledObjects.GCHandleToObject(instance)).DonationCount);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonationProgress)GCHandledObjects.GCHandleToObject(instance)).LastTrackedDonationTime);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TroopDonationProgress)GCHandledObjects.GCHandleToObject(instance)).DonationCooldownEndTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TroopDonationProgress)GCHandledObjects.GCHandleToObject(instance)).DonationCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TroopDonationProgress)GCHandledObjects.GCHandleToObject(instance)).LastTrackedDonationTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonationProgress)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
