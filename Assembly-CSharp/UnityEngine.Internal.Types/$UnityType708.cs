using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType708 : $UnityType
	{
		public unsafe $UnityType708()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 534824) = ldftn($Invoke0);
			*(data + 534852) = ldftn($Invoke1);
			*(data + 534880) = ldftn($Invoke2);
			*(data + 534908) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new EntityController((UIntPtr)0);
		}
	}
}
