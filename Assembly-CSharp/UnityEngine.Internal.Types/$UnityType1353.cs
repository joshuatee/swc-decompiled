using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1353 : $UnityType
	{
		public unsafe $UnityType1353()
		{
			*(UnityEngine.Internal.$Metadata.data + 650828) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSetPerkActivationStateResponse((UIntPtr)0);
		}
	}
}
