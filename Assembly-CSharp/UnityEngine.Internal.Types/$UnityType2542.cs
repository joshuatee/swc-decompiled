using StaRTS.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2542 : $UnityType
	{
		public unsafe $UnityType2542()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 991672) = ldftn($Invoke0);
			*(data + 991700) = ldftn($Invoke1);
			*(data + 991728) = ldftn($Invoke2);
			*(data + 991756) = ldftn($Invoke3);
			*(data + 991784) = ldftn($Invoke4);
			*(data + 991812) = ldftn($Invoke5);
			*(data + 991840) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new Rand((UIntPtr)0);
		}
	}
}
