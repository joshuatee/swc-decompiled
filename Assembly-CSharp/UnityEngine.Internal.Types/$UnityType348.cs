using StaRTS.Assets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType348 : $UnityType
	{
		public unsafe $UnityType348()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 430524) = ldftn($Invoke0);
			*(data + 430552) = ldftn($Invoke1);
			*(data + 430580) = ldftn($Invoke2);
			*(data + 430608) = ldftn($Invoke3);
			*(data + 430636) = ldftn($Invoke4);
			*(data + 430664) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new AssetProfiler((UIntPtr)0);
		}
	}
}
