using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1643 : $UnityType
	{
		public unsafe $UnityType1643()
		{
			*(UnityEngine.Internal.$Metadata.data + 662476) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SquadWarStartMatchmakingRequest((UIntPtr)0);
		}
	}
}
