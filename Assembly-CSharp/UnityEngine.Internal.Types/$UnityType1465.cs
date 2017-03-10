using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1465 : $UnityType
	{
		public unsafe $UnityType1465()
		{
			*(UnityEngine.Internal.$Metadata.data + 655252) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SetPrefsRequest((UIntPtr)0);
		}
	}
}
