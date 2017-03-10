using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1067 : $UnityType
	{
		public unsafe $UnityType1067()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 627644) = ldftn($Invoke0);
			*(data + 627672) = ldftn($Invoke1);
			*(data + 627700) = ldftn($Invoke2);
			*(data + 627728) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new StrIntPair((UIntPtr)0);
		}
	}
}
