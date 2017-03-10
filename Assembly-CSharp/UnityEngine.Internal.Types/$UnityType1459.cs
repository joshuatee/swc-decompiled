using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1459 : $UnityType
	{
		public unsafe $UnityType1459()
		{
			*(UnityEngine.Internal.$Metadata.data + 655140) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ResetStateCommand((UIntPtr)0);
		}
	}
}
