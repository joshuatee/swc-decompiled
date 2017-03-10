using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1320 : $UnityType
	{
		public unsafe $UnityType1320()
		{
			*(UnityEngine.Internal.$Metadata.data + 650016) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatDeployableUpgradeRequest((UIntPtr)0);
		}
	}
}
