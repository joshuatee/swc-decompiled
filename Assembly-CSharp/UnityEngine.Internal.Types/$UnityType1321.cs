using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1321 : $UnityType
	{
		public unsafe $UnityType1321()
		{
			*(UnityEngine.Internal.$Metadata.data + 650044) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatDeployablesCommand((UIntPtr)0);
		}
	}
}
