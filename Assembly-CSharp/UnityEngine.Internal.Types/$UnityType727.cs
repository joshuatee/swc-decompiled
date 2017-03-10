using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType727 : $UnityType
	{
		public unsafe $UnityType727()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 541320) = ldftn($Invoke0);
			*(data + 541348) = ldftn($Invoke1);
			*(data + 541376) = ldftn($Invoke2);
			*(data + 541404) = ldftn($Invoke3);
			*(data + 541432) = ldftn($Invoke4);
			*(data + 541460) = ldftn($Invoke5);
			*(data + 541488) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new LimitedEditionItemController((UIntPtr)0);
		}
	}
}
