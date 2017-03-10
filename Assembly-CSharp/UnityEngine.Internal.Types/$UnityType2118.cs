using StaRTS.Main.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2118 : $UnityType
	{
		public unsafe $UnityType2118()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 854276) = ldftn($Invoke0);
			*(data + 854304) = ldftn($Invoke1);
			*(data + 854332) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new NetworkConnectionTester((UIntPtr)0);
		}
	}
}
