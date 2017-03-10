using StaRTS.Main.Models.Commands.Crates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1378 : $UnityType
	{
		public unsafe $UnityType1378()
		{
			*(UnityEngine.Internal.$Metadata.data + 651724) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new AwardCrateSuppliesRequest((UIntPtr)0);
		}
	}
}
