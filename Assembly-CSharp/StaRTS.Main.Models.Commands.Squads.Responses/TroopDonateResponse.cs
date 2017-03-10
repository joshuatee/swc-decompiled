using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Responses
{
	public class TroopDonateResponse : AbstractResponse
	{
		public Dictionary<string, int> TroopsDonated
		{
			get;
			private set;
		}

		public int DonationCount
		{
			get;
			private set;
		}

		public int LastTrackedDonationTime
		{
			get;
			private set;
		}

		public int DonationCooldownEndTime
		{
			get;
			private set;
		}

		public bool ReputationAwarded
		{
			get;
			private set;
		}

		public TroopDonateResponse()
		{
			this.TroopsDonated = new Dictionary<string, int>();
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("troopsDonated"))
			{
				Dictionary<string, object> dictionary2 = dictionary["troopsDonated"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					foreach (KeyValuePair<string, object> current in dictionary2)
					{
						this.TroopsDonated.Add(current.get_Key(), Convert.ToInt32(current.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			if (dictionary.ContainsKey("troopDonationProgress"))
			{
				Dictionary<string, object> dictionary3 = dictionary["troopDonationProgress"] as Dictionary<string, object>;
				if (dictionary3 != null)
				{
					if (dictionary3.ContainsKey("donationCount"))
					{
						this.DonationCount = Convert.ToInt32(dictionary3["donationCount"], CultureInfo.InvariantCulture);
					}
					if (dictionary3.ContainsKey("lastTrackedDonationTime"))
					{
						this.LastTrackedDonationTime = Convert.ToInt32(dictionary3["lastTrackedDonationTime"], CultureInfo.InvariantCulture);
					}
					if (dictionary3.ContainsKey("repDonationCooldownEndTime"))
					{
						this.DonationCooldownEndTime = Convert.ToInt32(dictionary3["repDonationCooldownEndTime"], CultureInfo.InvariantCulture);
					}
				}
			}
			if (dictionary.ContainsKey("reputationAwarded"))
			{
				this.ReputationAwarded = (bool)dictionary["reputationAwarded"];
			}
			return this;
		}

		protected internal TroopDonateResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).DonationCooldownEndTime);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).DonationCount);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).LastTrackedDonationTime);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).ReputationAwarded);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).TroopsDonated);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).DonationCooldownEndTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).DonationCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).LastTrackedDonationTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).ReputationAwarded = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TroopDonateResponse)GCHandledObjects.GCHandleToObject(instance)).TroopsDonated = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
