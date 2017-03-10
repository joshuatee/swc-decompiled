using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType740 : $UnityType
	{
		public unsafe $UnityType740()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 545044) = ldftn($Invoke0);
			*(data + 545072) = ldftn($Invoke1);
			*(data + 545100) = ldftn($Invoke2);
			*(data + 545128) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new PopupsManager((UIntPtr)0);
		}
	}
}
