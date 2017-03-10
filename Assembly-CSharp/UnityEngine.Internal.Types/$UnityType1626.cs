using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1626 : $UnityType
	{
		public unsafe $UnityType1626()
		{
			*(UnityEngine.Internal.$Metadata.data + 661104) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CreateSquadRequest((UIntPtr)0);
		}
	}
}
