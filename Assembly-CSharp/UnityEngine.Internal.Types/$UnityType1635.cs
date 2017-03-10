using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1635 : $UnityType
	{
		public unsafe $UnityType1635()
		{
			*(UnityEngine.Internal.$Metadata.data + 661916) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SendSquadInviteRequest((UIntPtr)0);
		}
	}
}
