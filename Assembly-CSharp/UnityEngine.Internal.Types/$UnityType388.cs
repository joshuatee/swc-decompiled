using StaRTS.Externals.EnvironmentManager;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType388 : $UnityType
	{
		public unsafe $UnityType388()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 438700) = ldftn($Invoke0);
			*(data + 438728) = ldftn($Invoke1);
			*(data + 438756) = ldftn($Invoke2);
			*(data + 438784) = ldftn($Invoke3);
			*(data + 438812) = ldftn($Invoke4);
			*(data + 438840) = ldftn($Invoke5);
			*(data + 438868) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new EnvironmentManagerComponent((UIntPtr)0);
		}
	}
}
