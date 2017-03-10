using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1644 : $UnityType
	{
		public unsafe $UnityType1644()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 662504) = ldftn($Invoke0);
			*(data + 662532) = ldftn($Invoke1);
			*(data + 662560) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TournamentLeaderboardRequest((UIntPtr)0);
		}
	}
}
