using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1782 : $UnityType
	{
		public unsafe $UnityType1782()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 682692) = ldftn($Invoke0);
			*(data + 682720) = ldftn($Invoke1);
			*(data + 682748) = ldftn($Invoke2);
			*(data + 682776) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new StarportNode((UIntPtr)0);
		}
	}
}
