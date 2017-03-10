using StaRTS.Utils.MetaData;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2578 : $UnityType
	{
		public unsafe $UnityType2578()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 998000) = ldftn($Invoke0);
			*(data + 998028) = ldftn($Invoke1);
			*(data + 998056) = ldftn($Invoke2);
			*(data + 998084) = ldftn($Invoke3);
			*(data + 998112) = ldftn($Invoke4);
			*(data + 998140) = ldftn($Invoke5);
			*(data + 998168) = ldftn($Invoke6);
			*(data + 998196) = ldftn($Invoke7);
			*(data + 998224) = ldftn($Invoke8);
			*(data + 998252) = ldftn($Invoke9);
			*(data + 998280) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new Sheet((UIntPtr)0);
		}
	}
}
