using StaRTS.Main.Models.Commands.Crates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1381 : $UnityType
	{
		public unsafe $UnityType1381()
		{
			*(UnityEngine.Internal.$Metadata.data + 651780) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new BuyCrateRequest((UIntPtr)0);
		}
	}
}
