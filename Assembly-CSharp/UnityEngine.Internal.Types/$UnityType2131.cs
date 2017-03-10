using StaRTS.Main.Utils.Events;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2131 : $UnityType
	{
		public unsafe $UnityType2131()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 858924) = ldftn($Invoke0);
			*(data + 858952) = ldftn($Invoke1);
			*(data + 858980) = ldftn($Invoke2);
			*(data + 859008) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new AudioEventData((UIntPtr)0);
		}
	}
}
