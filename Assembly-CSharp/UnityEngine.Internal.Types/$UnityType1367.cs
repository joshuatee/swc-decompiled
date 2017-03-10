using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1367 : $UnityType
	{
		public unsafe $UnityType1367()
		{
			*(UnityEngine.Internal.$Metadata.data + 651500) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSquadWarTurnsRequest((UIntPtr)0);
		}
	}
}
