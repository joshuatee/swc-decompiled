using StaRTS.Main.Models.Commands.Player.Raids;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1524 : $UnityType
	{
		public unsafe $UnityType1524()
		{
			*(UnityEngine.Internal.$Metadata.data + 658444) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new RaidDefenseStartRequest((UIntPtr)0);
		}
	}
}
