using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1461 : $UnityType
	{
		public unsafe $UnityType1461()
		{
			*(UnityEngine.Internal.$Metadata.data + 655168) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SetFactionRequest((UIntPtr)0);
		}
	}
}
