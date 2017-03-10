using StaRTS.Assets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType349 : $UnityType
	{
		public unsafe $UnityType349()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 430692) = ldftn($Invoke0);
			*(data + 430720) = ldftn($Invoke1);
			*(data + 430748) = ldftn($Invoke2);
			*(data + 430776) = ldftn($Invoke3);
			*(data + 430804) = ldftn($Invoke4);
			*(data + 430832) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new AssetProfilerFetchData((UIntPtr)0);
		}
	}
}
