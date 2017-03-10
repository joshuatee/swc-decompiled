using StaRTS.Main.Views.Cameras;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2163 : $UnityType
	{
		public unsafe $UnityType2163()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 865252) = ldftn($Invoke0);
			*(data + 865280) = ldftn($Invoke1);
			*(data + 865308) = ldftn($Invoke2);
			*(data + 865336) = ldftn($Invoke3);
			*(data + 865364) = ldftn($Invoke4);
			*(data + 865392) = ldftn($Invoke5);
			*(data + 865420) = ldftn($Invoke6);
			*(data + 865448) = ldftn($Invoke7);
			*(data + 865476) = ldftn($Invoke8);
			*(data + 865504) = ldftn($Invoke9);
			*(data + 865532) = ldftn($Invoke10);
			*(data + 865560) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new UXCamera((UIntPtr)0);
		}
	}
}
