using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1069 : $UnityType
	{
		public unsafe $UnityType1069()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 627756) = ldftn($Invoke0);
			*(data + 627784) = ldftn($Invoke1);
			*(data + 627812) = ldftn($Invoke2);
			*(data + 627840) = ldftn($Invoke3);
			*(data + 627868) = ldftn($Invoke4);
			*(data + 627896) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new SupplyData((UIntPtr)0);
		}
	}
}
