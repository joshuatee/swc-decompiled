using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1065 : $UnityType
	{
		public unsafe $UnityType1065()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 627560) = ldftn($Invoke0);
			*(data + 627588) = ldftn($Invoke1);
			*(data + 627616) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SpecialAttackTrapEventData((UIntPtr)0);
		}
	}
}
