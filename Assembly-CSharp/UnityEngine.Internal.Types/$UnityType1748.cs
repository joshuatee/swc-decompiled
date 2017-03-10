using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1748 : $UnityType
	{
		public unsafe $UnityType1748()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 676588) = ldftn($Invoke0);
			*(data + 676616) = ldftn($Invoke1);
			*(data + 676644) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new WalkerComponent((UIntPtr)0);
		}
	}
}
