using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1013 : $UnityType
	{
		public unsafe $UnityType1013()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 606812) = ldftn($Invoke0);
			*(data + 606840) = ldftn($Invoke1);
			*(data + 606868) = ldftn($Invoke2);
			*(data + 606896) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new BuffEventData((UIntPtr)0);
		}
	}
}
