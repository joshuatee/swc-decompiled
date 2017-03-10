using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2247 : $UnityType
	{
		public unsafe $UnityType2247()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 895100) = ldftn($Invoke0);
			*(data + 895128) = ldftn($Invoke1);
			*(data + 895156) = ldftn($Invoke2);
			*(data + 895184) = ldftn($Invoke3);
			*(data + 895212) = ldftn($Invoke4);
			*(data + 895240) = ldftn($Invoke5);
			*(data + 895268) = ldftn($Invoke6);
			*(data + 895296) = ldftn($Invoke7);
			*(data + 895324) = ldftn($Invoke8);
			*(data + 895352) = ldftn($Invoke9);
			*(data + 895380) = ldftn($Invoke10);
			*(data + 895408) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new UXButtonComponent((UIntPtr)0);
		}
	}
}
