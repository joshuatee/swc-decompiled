using StaRTS.Main.Models.Commands.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1420 : $UnityType
	{
		public unsafe $UnityType1420()
		{
			*(UnityEngine.Internal.$Metadata.data + 653124) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PlayerPerkCancelRequest((UIntPtr)0);
		}
	}
}
