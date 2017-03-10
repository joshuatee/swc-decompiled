using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1022 : $UnityType
	{
		public unsafe $UnityType1022()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 607708) = ldftn($Invoke0);
			*(data + 607736) = ldftn($Invoke1);
			*(data + 607764) = ldftn($Invoke2);
			*(data + 607792) = ldftn($Invoke3);
			*(data + 607820) = ldftn($Invoke4);
			*(data + 607848) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new ContractNode((UIntPtr)0);
		}
	}
}
