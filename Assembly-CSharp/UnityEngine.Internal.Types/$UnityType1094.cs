using StaRTS.Main.Models.Battle;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1094 : $UnityType
	{
		public unsafe $UnityType1094()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 636940) = ldftn($Invoke0);
			*(data + 636968) = ldftn($Invoke1);
			*(data + 636996) = ldftn($Invoke2);
			*(data + 637024) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new Target((UIntPtr)0);
		}
	}
}
