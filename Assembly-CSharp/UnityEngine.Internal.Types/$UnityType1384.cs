using StaRTS.Main.Models.Commands.Crates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1384 : $UnityType
	{
		public unsafe $UnityType1384()
		{
			*(UnityEngine.Internal.$Metadata.data + 651836) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatAddCrateRequest((UIntPtr)0);
		}
	}
}
