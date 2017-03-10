using StaRTS.Externals.Manimal;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Squads.Responses;
using StaRTS.Main.Models.Perks;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Squads
{
	public class TroopDonationTrackController
	{
		private const string REP_REWARD_AMOUNT = "PERK_TROOP_DONATE_REP_REWARD_AMOUNT";

		private const string REP_REWARD_DESC = "PERK_TROOP_DONATE_REP_REWARD_DESC";

		public TroopDonationTrackController()
		{
			Service.Set<TroopDonationTrackController>(this);
		}

		public void UpdateTroopDonationProgress(TroopDonateResponse response)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			currentPlayer.UpdateTroopDonationProgress(response.DonationCount, response.LastTrackedDonationTime, response.DonationCooldownEndTime);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.SendEvent(EventId.TroopDonationTrackProgressUpdated, null);
			if (response.ReputationAwarded)
			{
				Inventory inventory = currentPlayer.Inventory;
				inventory.ModifyReputation(GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD);
				Lang lang = Service.Get<Lang>();
				string status = lang.Get("PERK_TROOP_DONATE_REP_REWARD_AMOUNT", new object[]
				{
					GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD
				});
				string toast = lang.Get("PERK_TROOP_DONATE_REP_REWARD_DESC", new object[0]);
				Service.Get<UXController>().MiscElementsManager.ShowToast(toast, status, string.Empty);
				eventManager.SendEvent(EventId.TroopDonationTrackRewardReceived, null);
			}
		}

		public bool IsTroopDonationProgressComplete()
		{
			return this.GetTroopDonationProgressAmount() >= GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD;
		}

		public int GetTroopDonationProgressAmount()
		{
			TroopDonationProgress troopDonationProgress = Service.Get<CurrentPlayer>().TroopDonationProgress;
			int timeRemainingUntilNextProgressTrack = this.GetTimeRemainingUntilNextProgressTrack();
			if (timeRemainingUntilNextProgressTrack <= 0 && troopDonationProgress.DonationCount >= GameConstants.SQUADPERK_DONATION_REPUTATION_AWARD_THRESHOLD)
			{
				return 0;
			}
			return troopDonationProgress.DonationCount;
		}

		public int GetTimeRemainingUntilNextProgressTrack()
		{
			TroopDonationProgress troopDonationProgress = Service.Get<CurrentPlayer>().TroopDonationProgress;
			uint donationCooldownEndTime = (uint)troopDonationProgress.DonationCooldownEndTime;
			uint serverTime = Service.Get<ServerAPI>().ServerTime;
			return (int)(donationCooldownEndTime - serverTime);
		}

		protected internal TroopDonationTrackController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonationTrackController)GCHandledObjects.GCHandleToObject(instance)).GetTimeRemainingUntilNextProgressTrack());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonationTrackController)GCHandledObjects.GCHandleToObject(instance)).GetTroopDonationProgressAmount());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonationTrackController)GCHandledObjects.GCHandleToObject(instance)).IsTroopDonationProgressComplete());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TroopDonationTrackController)GCHandledObjects.GCHandleToObject(instance)).UpdateTroopDonationProgress((TroopDonateResponse)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
