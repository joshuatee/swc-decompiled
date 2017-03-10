using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType786 : $UnityType
	{
		public unsafe $UnityType786()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 567164) = ldftn($Invoke0);
			*(data + 567192) = ldftn($Invoke1);
			*(data + 567220) = ldftn($Invoke2);
			*(data + 567248) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ValueObjectController((UIntPtr)0);
		}
	}
}
