using StaRTS.Externals.Maker.MRSS;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType443 : $UnityType
	{
		public unsafe $UnityType443()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 449452) = ldftn($Invoke0);
			*(data + 449480) = ldftn($Invoke1);
			*(data + 449508) = ldftn($Invoke2);
			*(data + 449536) = ldftn($Invoke3);
			*(data + 449564) = ldftn($Invoke4);
			*(data + 449592) = ldftn($Invoke5);
			*(data + 449620) = ldftn($Invoke6);
			*(data + 449648) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new QueryURLBuilder((UIntPtr)0);
		}
	}
}
