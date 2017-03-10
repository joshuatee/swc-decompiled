using StaRTS.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2537 : $UnityType
	{
		public unsafe $UnityType2537()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 990076) = ldftn($Invoke0);
			*(data + 990104) = ldftn($Invoke1);
			*(data + 990132) = ldftn($Invoke2);
			*(data + 990160) = ldftn($Invoke3);
			*(data + 990188) = ldftn($Invoke4);
			*(data + 990216) = ldftn($Invoke5);
			*(data + 990244) = ldftn($Invoke6);
			*(data + 990272) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new IntMath((UIntPtr)0);
		}
	}
}
