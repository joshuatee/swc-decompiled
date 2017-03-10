using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType715 : $UnityType
	{
		public unsafe $UnityType715()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 536420) = ldftn($Invoke0);
			*(data + 536448) = ldftn($Invoke1);
			*(data + 536476) = ldftn($Invoke2);
			*(data + 536504) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new HomeModeController((UIntPtr)0);
		}
	}
}
