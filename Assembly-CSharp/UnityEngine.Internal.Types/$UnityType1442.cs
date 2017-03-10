using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1442 : $UnityType
	{
		public unsafe $UnityType1442()
		{
			*(UnityEngine.Internal.$Metadata.data + 653964) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new GetPlayerPvpStatusResponse((UIntPtr)0);
		}
	}
}
