using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1362 : $UnityType
	{
		public unsafe $UnityType1362()
		{
			*(UnityEngine.Internal.$Metadata.data + 651276) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSetTroopDonateRepResponse((UIntPtr)0);
		}
	}
}
