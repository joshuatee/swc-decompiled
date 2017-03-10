using StaRTS.Externals.Manimal.TransferObjects.Request;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType611 : $UnityType
	{
		public unsafe $UnityType611()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 505452) = ldftn($Invoke0);
			*(data + 505480) = ldftn($Invoke1);
			*(data + 505508) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlayerIdRequest((UIntPtr)0);
		}
	}
}
