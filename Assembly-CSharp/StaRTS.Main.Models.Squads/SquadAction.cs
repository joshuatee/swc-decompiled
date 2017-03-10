using System;

namespace StaRTS.Main.Models.Squads
{
	public enum SquadAction
	{
		Create,
		Join,
		Leave,
		Edit,
		ApplyToJoin,
		AcceptApplicationToJoin,
		RejectApplicationToJoin,
		SendInviteToJoin,
		AcceptInviteToJoin,
		RejectInviteToJoin,
		PromoteMember,
		DemoteMember,
		RemoveMember,
		RequestTroops,
		DonateTroops,
		RequestWarTroops,
		DonateWarTroops,
		ShareReplay,
		ShareVideo,
		StartWarMatchmaking,
		CancelWarMatchmaking
	}
}
