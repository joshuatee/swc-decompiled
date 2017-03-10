using StaRTS.Main.Models.Commands.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1560 : $UnityType
	{
		public unsafe $UnityType1560()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 660880) = ldftn($Invoke0);
			*(data + 660908) = ldftn($Invoke1);
			*(data + 660936) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GetSquadInvitesResponse((UIntPtr)0);
		}
	}
}
