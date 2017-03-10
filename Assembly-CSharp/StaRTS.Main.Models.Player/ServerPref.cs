using System;

namespace StaRTS.Main.Models.Player
{
	public enum ServerPref
	{
		Locale,
		AgeGate,
		LoginPopups,
		LastLoginTime,
		NewspaperArticlesViewed,
		[Obsolete("There is now a dedicated property in CurrentPlayer", true)]
		LastTroopRequestTime,
		NumStoreItemsNotViewed,
		NumRateAppViewed,
		RatedApp,
		NumInventoryItemsNotViewed,
		ChapterMissionViewed,
		SpecopsMissionViewed,
		TournamentViewed,
		LastPaymentTime,
		BattlesAdCount,
		SquadIntroViewed,
		TournamentTierChangeTimeViewed,
		FactionFlippingViewed,
		FactionFlippingSkipConfirmation,
		FactionFlipped,
		LastChatViewTime,
		LastPushAuthPromptTroopRequestTime,
		LastPushAuthPromptSquadJoinedTime,
		PushAuthPromptedCount,
		PlanetsTutorialViewed,
		FirstRelocationPlanet,
		NumInventoryCratesNotViewed,
		NumInventoryTroopsNotViewed,
		NumInventoryCurrencyNotViewed,
		COUNT
	}
}
