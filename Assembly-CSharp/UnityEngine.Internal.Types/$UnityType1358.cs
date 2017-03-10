using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1358 : $UnityType
	{
		public unsafe $UnityType1358()
		{
			*(UnityEngine.Internal.$Metadata.data + 651164) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSetSquadLevelRequest((UIntPtr)0);
		}
	}
}
