using StaRTS.Externals.FacebookApi;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType398 : $UnityType
	{
		public unsafe $UnityType398()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 441724) = ldftn($Invoke0);
			*(data + 441752) = ldftn($Invoke1);
			*(data + 441780) = ldftn($Invoke2);
			*(data + 441808) = ldftn($Invoke3);
			*(data + 441836) = ldftn($Invoke4);
			*(data + 441864) = ldftn($Invoke5);
			*(data + 441892) = ldftn($Invoke6);
			*(data + 441920) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new GraphResult((UIntPtr)0);
		}
	}
}
