using StaRTS.Externals.Maker.MRSS;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType444 : $UnityType
	{
		public unsafe $UnityType444()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 449676) = ldftn($Invoke0);
			*(data + 449704) = ldftn($Invoke1);
			*(data + 449732) = ldftn($Invoke2);
			*(data + 449760) = ldftn($Invoke3);
			*(data + 449788) = ldftn($Invoke4);
			*(data + 449816) = ldftn($Invoke5);
			*(data + 449844) = ldftn($Invoke6);
			*(data + 449872) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new ThumbnailManager((UIntPtr)0);
		}
	}
}
