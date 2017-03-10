using System;

namespace StaRTS.Main.Models.Squads.War
{
	public enum SquadWarScoutState
	{
		Invalid,
		AttackAvailable,
		NotInActionPhase,
		NoTurnsLeft,
		NotPatricipantInWar,
		UnderAttack,
		OpponentHasNoVictoryPointsLeft,
		DestinationUnavailable
	}
}
