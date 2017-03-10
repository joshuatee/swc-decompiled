using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1751 : $UnityType
	{
		public unsafe $UnityType1751()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 676840) = ldftn($Invoke0);
			*(data + 676868) = ldftn($Invoke1);
			*(data + 676896) = ldftn($Invoke2);
			*(data + 676924) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ArmoryNode((UIntPtr)0);
		}
	}
}
