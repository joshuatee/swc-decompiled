using StaRTS.Assets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType350 : $UnityType
	{
		public unsafe $UnityType350()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 430860) = ldftn($Invoke0);
			*(data + 430888) = ldftn($Invoke1);
			*(data + 430916) = ldftn($Invoke2);
			*(data + 430944) = ldftn($Invoke3);
			*(data + 430972) = ldftn($Invoke4);
			*(data + 431000) = ldftn($Invoke5);
			*(data + 431028) = ldftn($Invoke6);
			*(data + 431056) = ldftn($Invoke7);
			*(data + 431084) = ldftn($Invoke8);
			*(data + 431112) = ldftn($Invoke9);
			*(data + 431140) = ldftn($Invoke10);
			*(data + 431168) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new AssetRequest((UIntPtr)0);
		}
	}
}
