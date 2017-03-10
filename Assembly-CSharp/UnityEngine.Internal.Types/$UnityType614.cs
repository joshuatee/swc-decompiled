using StaRTS.Externals.Manimal.TransferObjects.Response;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType614 : $UnityType
	{
		public unsafe $UnityType614()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 505620) = ldftn($Invoke0);
			*(data + 505648) = ldftn($Invoke1);
			*(data + 505676) = ldftn($Invoke2);
			*(data + 505704) = ldftn($Invoke3);
			*(data + 505732) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new Data((UIntPtr)0);
		}
	}
}
