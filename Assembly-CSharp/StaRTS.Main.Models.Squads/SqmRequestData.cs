using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Squads
{
	public class SqmRequestData
	{
		public string Text;

		public bool PayToSkip;

		public int StartingAvailableCapacity;

		public int TotalCapacity;

		public int TroopDonationLimit;

		public int CurrentDonationSize;

		public int CurrentPlayerDonationCount;

		public int ResendCrystalCost;

		public string WarId;

		public bool IsWarRequest
		{
			get
			{
				return !string.IsNullOrEmpty(this.WarId);
			}
		}

		public SqmRequestData()
		{
		}

		protected internal SqmRequestData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SqmRequestData)GCHandledObjects.GCHandleToObject(instance)).IsWarRequest);
		}
	}
}
