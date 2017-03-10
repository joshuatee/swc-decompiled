using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1330 : $UnityType
	{
		public unsafe $UnityType1330()
		{
			*(UnityEngine.Internal.$Metadata.data + 650408) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatPointsRequest((UIntPtr)0);
		}
	}
}
