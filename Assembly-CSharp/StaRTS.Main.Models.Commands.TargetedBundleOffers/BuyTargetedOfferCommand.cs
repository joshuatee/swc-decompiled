using System;

namespace StaRTS.Main.Models.Commands.TargetedBundleOffers
{
	public class BuyTargetedOfferCommand : GameActionCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>
	{
		public const string ACTION = "player.store.offers.buy";

		public BuyTargetedOfferCommand(BuyTargetedOfferRequest request) : base("player.store.offers.buy", request, new BuyTargetedOfferResponse())
		{
		}

		protected internal BuyTargetedOfferCommand(UIntPtr dummy) : base(dummy)
		{
		}
	}
}
