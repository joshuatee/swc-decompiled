using System;

namespace StaRTS.Main.Models.Squads
{
	public enum SquadMsgType
	{
		Invalid,
		Chat,
		Join,
		JoinRequest,
		JoinRequestAccepted,
		JoinRequestRejected,
		InviteAccepted,
		Leave,
		Ejected,
		Promotion,
		Demotion,
		ShareBattle,
		ShareLink,
		TroopRequest,
		TroopDonation,
		WarMatchMakingBegin,
		WarMatchMakingCancel,
		WarStarted,
		WarPrepared,
		WarBuffBaseAttackStart,
		WarBuffBaseAttackComplete,
		WarPlayerAttackStart,
		WarPlayerAttackComplete,
		WarEnded,
		SquadLevelUp,
		PerkUnlocked,
		PerkUpgraded,
		PerkInvest,
		Invite,
		InviteRejected
	}
}
