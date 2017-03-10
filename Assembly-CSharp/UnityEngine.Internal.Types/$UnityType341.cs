using Source.StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType341 : $UnityType
	{
		public unsafe $UnityType341()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 428564) = ldftn($Invoke0);
			*(data + 428592) = ldftn($Invoke1);
			*(data + 428620) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GetAuthTokenRequest((UIntPtr)0);
		}
	}
}
