using StaRTS.Main.Models.Commands.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1432 : $UnityType
	{
		public unsafe $UnityType1432()
		{
			*(UnityEngine.Internal.$Metadata.data + 653376) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new RelocatePlanetRequest((UIntPtr)0);
		}
	}
}
