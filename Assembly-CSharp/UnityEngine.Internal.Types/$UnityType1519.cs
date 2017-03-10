using StaRTS.Main.Models.Commands.Player.Identity;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1519 : $UnityType
	{
		public unsafe $UnityType1519()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 658248) = ldftn($Invoke0);
			*(data + 658276) = ldftn($Invoke1);
			*(data + 658304) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlayerIdentitySwitchResponse((UIntPtr)0);
		}
	}
}
