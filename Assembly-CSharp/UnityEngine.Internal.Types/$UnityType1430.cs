using StaRTS.Main.Models.Commands.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1430 : $UnityType
	{
		public unsafe $UnityType1430()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 653292) = ldftn($Invoke0);
			*(data + 653320) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PlanetStatsRequest((UIntPtr)0);
		}
	}
}
