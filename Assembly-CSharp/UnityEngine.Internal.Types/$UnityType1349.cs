using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1349 : $UnityType
	{
		public unsafe $UnityType1349()
		{
			*(UnityEngine.Internal.$Metadata.data + 650744) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSetObjectivesProgressRequest((UIntPtr)0);
		}
	}
}
