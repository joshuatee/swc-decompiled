using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1336 : $UnityType
	{
		public unsafe $UnityType1336()
		{
			*(UnityEngine.Internal.$Metadata.data + 650436) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatResetSquadLevelRequest((UIntPtr)0);
		}
	}
}
