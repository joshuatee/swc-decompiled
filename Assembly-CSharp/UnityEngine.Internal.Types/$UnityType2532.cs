using StaRTS.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2532 : $UnityType
	{
		public unsafe $UnityType2532()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 988228) = ldftn($Invoke0);
			*(data + 988256) = ldftn($Invoke1);
			*(data + 988284) = ldftn($Invoke2);
			*(data + 988312) = ldftn($Invoke3);
			*(data + 988340) = ldftn($Invoke4);
			*(data + 988368) = ldftn($Invoke5);
			*(data + 988396) = ldftn($Invoke6);
			*(data + 988424) = ldftn($Invoke7);
			*(data + 988452) = ldftn($Invoke8);
			*(data + 988480) = ldftn($Invoke9);
			*(data + 988508) = ldftn($Invoke10);
			*(data + 988536) = ldftn($Invoke11);
			*(data + 988564) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new DampedSpring((UIntPtr)0);
		}
	}
}
