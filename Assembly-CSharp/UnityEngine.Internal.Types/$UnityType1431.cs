using StaRTS.Main.Models.Commands.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1431 : $UnityType
	{
		public unsafe $UnityType1431()
		{
			*(UnityEngine.Internal.$Metadata.data + 653348) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PlanetStatsResponse((UIntPtr)0);
		}
	}
}
