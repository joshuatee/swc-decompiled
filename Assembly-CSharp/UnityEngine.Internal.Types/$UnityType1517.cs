using StaRTS.Main.Models.Commands.Player.Identity;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1517 : $UnityType
	{
		public unsafe $UnityType1517()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 658164) = ldftn($Invoke0);
			*(data + 658192) = ldftn($Invoke1);
			*(data + 658220) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlayerIdentityRequest((UIntPtr)0);
		}
	}
}
