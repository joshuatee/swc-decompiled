using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1341 : $UnityType
	{
		public unsafe $UnityType1341()
		{
			*(UnityEngine.Internal.$Metadata.data + 650632) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatScheduleDailyCrateResponse((UIntPtr)0);
		}
	}
}
