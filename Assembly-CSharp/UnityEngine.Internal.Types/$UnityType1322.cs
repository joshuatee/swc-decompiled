using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1322 : $UnityType
	{
		public unsafe $UnityType1322()
		{
			*(UnityEngine.Internal.$Metadata.data + 650072) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatDeployablesRequest((UIntPtr)0);
		}
	}
}
