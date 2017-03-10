using StaRTS.Assets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType356 : $UnityType
	{
		public unsafe $UnityType356()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 431364) = ldftn($Invoke0);
			*(data + 431392) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new InternalLoadCookie((UIntPtr)0);
		}
	}
}
