using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1791 : $UnityType
	{
		public unsafe $UnityType1791()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 683980) = ldftn($Invoke0);
			*(data + 684008) = ldftn($Invoke1);
			*(data + 684036) = ldftn($Invoke2);
			*(data + 684064) = ldftn($Invoke3);
			*(data + 684092) = ldftn($Invoke4);
			*(data + 684120) = ldftn($Invoke5);
			*(data + 684148) = ldftn($Invoke6);
			*(data + 684176) = ldftn($Invoke7);
			*(data + 684204) = ldftn($Invoke8);
			*(data + 684232) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new TrapNode((UIntPtr)0);
		}
	}
}
