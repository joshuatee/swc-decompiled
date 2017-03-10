using StaRTS.Main.Models.Commands.Test.Config;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1668 : $UnityType
	{
		public unsafe $UnityType1668()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 665332) = ldftn($Invoke0);
			*(data + 665360) = ldftn($Invoke1);
			*(data + 665388) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new AuthTokenIsPlayerIdResponse((UIntPtr)0);
		}
	}
}
