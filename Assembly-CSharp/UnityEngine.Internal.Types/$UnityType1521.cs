using StaRTS.Main.Models.Commands.Player.Raids;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1521 : $UnityType
	{
		public unsafe $UnityType1521()
		{
			*(UnityEngine.Internal.$Metadata.data + 658332) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new RaidDefenseCompleteRequest((UIntPtr)0);
		}
	}
}
