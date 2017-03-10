using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1025 : $UnityType
	{
		public unsafe $UnityType1025()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 608492) = ldftn($Invoke0);
			*(data + 608520) = ldftn($Invoke1);
			*(data + 608548) = ldftn($Invoke2);
			*(data + 608576) = ldftn($Invoke3);
			*(data + 608604) = ldftn($Invoke4);
			*(data + 608632) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new CurrencyCollectionTag((UIntPtr)0);
		}
	}
}
