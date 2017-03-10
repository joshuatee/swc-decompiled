using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType824 : $UnityType
	{
		public unsafe $UnityType824()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572960) = ldftn($Invoke0);
			*(data + 572988) = ldftn($Invoke1);
			*(data + 573016) = ldftn($Invoke2);
			*(data + 573044) = ldftn($Invoke3);
			*(data + 573072) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new BaseLayoutToolState((UIntPtr)0);
		}
	}
}
