using StaRTS.Main.Models.Commands.Crates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1383 : $UnityType
	{
		public unsafe $UnityType1383()
		{
			*(UnityEngine.Internal.$Metadata.data + 651808) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new BuyLimitedEditionItemRequest((UIntPtr)0);
		}
	}
}
