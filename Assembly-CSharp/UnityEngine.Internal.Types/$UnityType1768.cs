using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1768 : $UnityType
	{
		public unsafe $UnityType1768()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 679948) = ldftn($Invoke0);
			*(data + 679976) = ldftn($Invoke1);
			*(data + 680004) = ldftn($Invoke2);
			*(data + 680032) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new GeneratorNode((UIntPtr)0);
		}
	}
}
