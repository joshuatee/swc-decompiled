using StaRTS.Main.Models.Commands.Crates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1385 : $UnityType
	{
		public unsafe $UnityType1385()
		{
			*(UnityEngine.Internal.$Metadata.data + 651864) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatAddCrateResponse((UIntPtr)0);
		}
	}
}
