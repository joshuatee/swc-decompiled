using Source.StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType342 : $UnityType
	{
		public unsafe $UnityType342()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 428648) = ldftn($Invoke0);
			*(data + 428676) = ldftn($Invoke1);
			*(data + 428704) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GetAuthTokenResponse((UIntPtr)0);
		}
	}
}
