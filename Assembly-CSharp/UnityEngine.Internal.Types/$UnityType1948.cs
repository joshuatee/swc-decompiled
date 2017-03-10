using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1948 : $UnityType
	{
		public unsafe $UnityType1948()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 775540) = ldftn($Invoke0);
			*(data + 775568) = ldftn($Invoke1);
			*(data + 775596) = ldftn($Invoke2);
			*(data + 775624) = ldftn($Invoke3);
			*(data + 775652) = ldftn($Invoke4);
			*(data + 775680) = ldftn($Invoke5);
			*(data + 775708) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new ProfanityVO((UIntPtr)0);
		}
	}
}
