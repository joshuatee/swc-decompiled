using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1759 : $UnityType
	{
		public unsafe $UnityType1759()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 677848) = ldftn($Invoke0);
			*(data + 677876) = ldftn($Invoke1);
			*(data + 677904) = ldftn($Invoke2);
			*(data + 677932) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ClearableNode((UIntPtr)0);
		}
	}
}
