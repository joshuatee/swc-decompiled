using StaRTS.Externals.Maker;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType440 : $UnityType
	{
		public unsafe $UnityType440()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 448388) = ldftn($Invoke0);
			*(data + 448416) = ldftn($Invoke1);
			*(data + 448444) = ldftn($Invoke2);
			*(data + 448472) = ldftn($Invoke3);
			*(data + 448500) = ldftn($Invoke4);
			*(data + 448528) = ldftn($Invoke5);
			*(data + 448556) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new VideosPostView((UIntPtr)0);
		}
	}
}
