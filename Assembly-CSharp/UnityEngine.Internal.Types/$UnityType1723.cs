using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1723 : $UnityType
	{
		public unsafe $UnityType1723()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 671884) = ldftn($Invoke0);
			*(data + 671912) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ShieldBorderComponent((UIntPtr)0);
		}
	}
}
