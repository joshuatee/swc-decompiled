using StaRTS.Main.Models.Commands.Crates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1389 : $UnityType
	{
		public unsafe $UnityType1389()
		{
			*(UnityEngine.Internal.$Metadata.data + 651892) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheckDailyCrateResponse((UIntPtr)0);
		}
	}
}
