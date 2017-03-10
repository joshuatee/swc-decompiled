using StaRTS.Externals.Maker;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType439 : $UnityType
	{
		public unsafe $UnityType439()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 448108) = ldftn($Invoke0);
			*(data + 448136) = ldftn($Invoke1);
			*(data + 448164) = ldftn($Invoke2);
			*(data + 448192) = ldftn($Invoke3);
			*(data + 448220) = ldftn($Invoke4);
			*(data + 448248) = ldftn($Invoke5);
			*(data + 448276) = ldftn($Invoke6);
			*(data + 448304) = ldftn($Invoke7);
			*(data + 448332) = ldftn($Invoke8);
			*(data + 448360) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new VideosFilterChoice((UIntPtr)0);
		}
	}
}
