using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1464 : $UnityType
	{
		public unsafe $UnityType1464()
		{
			*(UnityEngine.Internal.$Metadata.data + 655224) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SetPrefsCommand((UIntPtr)0);
		}
	}
}
