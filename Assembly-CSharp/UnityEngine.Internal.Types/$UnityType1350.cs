using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1350 : $UnityType
	{
		public unsafe $UnityType1350()
		{
			*(UnityEngine.Internal.$Metadata.data + 650772) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSetObjectivesRequest((UIntPtr)0);
		}
	}
}
