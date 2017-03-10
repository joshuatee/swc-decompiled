using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType787 : $UnityType
	{
		public unsafe $UnityType787()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 567276) = ldftn($Invoke0);
			*(data + 567304) = ldftn($Invoke1);
			*(data + 567332) = ldftn($Invoke2);
			*(data + 567360) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new WarBaseEditController((UIntPtr)0);
		}
	}
}
