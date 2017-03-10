using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1352 : $UnityType
	{
		public unsafe $UnityType1352()
		{
			*(UnityEngine.Internal.$Metadata.data + 650800) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSetPerkActivationStateRequest((UIntPtr)0);
		}
	}
}
