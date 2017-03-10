using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType635 : $UnityType
	{
		public unsafe $UnityType635()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 510772) = ldftn($Invoke0);
			*(data + 510800) = ldftn($Invoke1);
			*(data + 510828) = ldftn($Invoke2);
			*(data + 510856) = ldftn($Invoke3);
			*(data + 510884) = ldftn($Invoke4);
			*(data + 510912) = ldftn($Invoke5);
			*(data + 510940) = ldftn($Invoke6);
			*(data + 510968) = ldftn($Invoke7);
			*(data + 510996) = ldftn($Invoke8);
			*(data + 511024) = ldftn($Invoke9);
			*(data + 511052) = ldftn($Invoke10);
			*(data + 511080) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new MultipleEmittersPool((UIntPtr)0);
		}
	}
}
