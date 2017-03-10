using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1629 : $UnityType
	{
		public unsafe $UnityType1629()
		{
			*(UnityEngine.Internal.$Metadata.data + 661468) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new GetSquadInvitesSentRequest((UIntPtr)0);
		}
	}
}
