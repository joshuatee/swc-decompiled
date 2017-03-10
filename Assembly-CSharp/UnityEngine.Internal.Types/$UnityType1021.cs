using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1021 : $UnityType
	{
		public unsafe $UnityType1021()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 607372) = ldftn($Invoke0);
			*(data + 607400) = ldftn($Invoke1);
			*(data + 607428) = ldftn($Invoke2);
			*(data + 607456) = ldftn($Invoke3);
			*(data + 607484) = ldftn($Invoke4);
			*(data + 607512) = ldftn($Invoke5);
			*(data + 607540) = ldftn($Invoke6);
			*(data + 607568) = ldftn($Invoke7);
			*(data + 607596) = ldftn($Invoke8);
			*(data + 607624) = ldftn($Invoke9);
			*(data + 607652) = ldftn($Invoke10);
			*(data + 607680) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new ContractEventData((UIntPtr)0);
		}
	}
}
