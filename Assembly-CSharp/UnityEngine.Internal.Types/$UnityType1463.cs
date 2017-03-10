using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1463 : $UnityType
	{
		public unsafe $UnityType1463()
		{
			*(UnityEngine.Internal.$Metadata.data + 655196) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SetPlayerNameRequest((UIntPtr)0);
		}
	}
}
